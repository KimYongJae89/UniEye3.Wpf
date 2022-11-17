using DynMvp.Base;
using DynMvp.Devices.Spectrometer.Algorithm;
using DynMvp.Devices.Spectrometer.Data;
using OmniDriver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.Spectrometer
{
    public class Spectrometer
    {
        public NETWrapper Wrapper { get; private set; } = null;
        public int NumberOfSpectrometers { get; private set; }
        public Dictionary<string, SpectrometerInfo> DeviceList { get; } = new Dictionary<string, SpectrometerInfo>();

        // 검사에 사용할 스펙트로미터
        public string SelectedDevice { get; set; } = string.Empty;
        // 티칭 상태에서는 Refraction 기능을 다른 방법으로 사용한다.
        // 각각의 스펙트로미터를 따로따로 제어 할 수 있도록 수정한다.
        public bool IsTeaching { get; set; } = false;
        // 굴절률 기능을 사용할지 여부
        public bool UseRefraction { get; set; } = false;
        // 굴절률 측정에 사용할 스펙트로미터
        public string SelectedRefractionDevice { get; set; } = string.Empty;
        // 굴절률 측정에 사용할 각도 값
        public float AngleValue { get; set; } = 0;

        // Spectrometer Calculate
        public MeasureDoneDelegate MeasureDone = null;

        // Resample 하기 전에 각 스펙트로미터들을 합친 범위의 파장과 스펙트럼
        // 스펙트로미터가 여러대일 경우 사용
        private double[] merged_Wavelength = null;
        private double[] merged_Spectrum = null;

        // 알고리즘이 실행되는 클래스
        private ThicknessAlgo thicknessAlgo = new ThicknessAlgo();
        // 여러 스펙트로미터의 파형을 합칠때 사용하는 변수
        private List<MemoryMap> memoryMaps = new List<MemoryMap>();

        // 새로운 스펙트럼 데이터를 저장하는 곳
        private Dictionary<string, double[]> newestSpectrum = new Dictionary<string, double[]>();
        public ConcurrentQueue<FFTInputData> FftInputDataQueue { get; } = new ConcurrentQueue<FFTInputData>();
        public Dictionary<string, Layer> LayerList { get; } = new Dictionary<string, Layer>();

        // Spectrometer Scan
        private Mutex mut = new Mutex();

        private Task preMeasureTask = null;
        private Task measureTask = null;
        private bool stopMeasureTask = false;

        // BG, REF 두께 연산 보정에 사용될 보정 데이터
        public Dictionary<string, double[]> BgSpect { get; set; } = new Dictionary<string, double[]>();
        public Dictionary<string, double[]> RefSpect { get; set; } = new Dictionary<string, double[]>();

        // 켈리브레이션 할 때 평균할 개수
        private int refbgAvgCount = 10;

        // 데이터 유무 판단
        public bool HasBgRefData
        {
            get
            {
                if (BgSpect.Count == 0 || RefSpect.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool OnWorking { get; set; } = false;
        public bool IsContinuous { get; set; } = true;

        public virtual void Initialize(SpectrometerProperty spectrometerProperty)
        {
            Wrapper = new OmniDriver.NETWrapper();

            if (NumberOfSpectrometers > 0)
            {
                Wrapper.closeAllSpectrometers();
            }

            var spectrometerTimeOutTask = Task.Run(() => SpectrometerTimeOutTask());

            LogHelper.Debug(LoggerType.Device, "Start Connect Spectrometer");
            NumberOfSpectrometers = Wrapper.openAllSpectrometers();

            spectrometerTimeOutTask.Wait();

            LogHelper.Debug(LoggerType.Device, "Connect Complete");

            for (int i = 0; i < NumberOfSpectrometers; i++)
            {
                var info = new SpectrometerInfo();
                info.Index = i;
                info.SerialNumbers = Wrapper.getSerialNumber(i);
                info.Name = Wrapper.getName(i) + "-" + info.SerialNumbers;
                info.WaveLengths = Wrapper.getWavelengths(i);
                info.NumberOfPixels = Wrapper.getNumberOfPixels(i);
                DeviceList.Add(info.Name, info);

                newestSpectrum.Add(info.Name, new double[info.WaveLengths.Count()]);

                if (spectrometerProperty.IntegrationTime.Keys.Contains(info.Name) == false)
                {
                    int integrationTime = Wrapper.getIntegrationTime(info.Index);
                    int average = Wrapper.getScansToAverage(info.Index);
                    int boxcar = Wrapper.getBoxcarWidth(info.Index);

                    spectrometerProperty.IntegrationTime.Add(info.Name, integrationTime);
                    spectrometerProperty.Average.Add(info.Name, average);
                    spectrometerProperty.Boxcar.Add(info.Name, boxcar);
                    spectrometerProperty.LampPitch.Add(info.Name, 0);
                }

                // apply reasonable defaults
                LogHelper.Debug(LoggerType.Device, string.Format("setting defaults for spectrometer {0}", info.Index));
                Wrapper.setCorrectForElectricalDark(info.Index, 1);
                Wrapper.setCorrectForDetectorNonlinearity(info.Index, 1);
                Wrapper.setStrobeEnable(info.Index, 0);
            }
            // 앞 파장인 것 부터 정렬한다.
            //deviceList.Values.ToList().Sort((x, y) => y.WaveLengths.First().CompareTo(x.WaveLengths.First()));
            // LayerList 공간을 미리 할당 해 놓는다.
            UpdateLayer(spectrometerProperty);
            // 계산에 사용할 merged_Wavelength, merged_Spectrum를 메모리에 할당한다.
            AllocMemoryAndMap();
        }

        private void SpectrometerTimeOutTask()
        {
            var timeOutTimer = new TimeOutTimer();

            timeOutTimer.Start(3000);

            while (NumberOfSpectrometers < 0)
            {
                if (timeOutTimer.TimeOut)
                {
                    if (MessageBox.Show(StringManager.GetString(GetType().FullName, string.Format("Can't find Spectrometer")),
                        StringManager.GetString(GetType().FullName, "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        timeOutTimer.Stop();

                        Application.ExitThread();
                        Environment.Exit(0);
                    }
                }

                Thread.Sleep(100);
            }

            timeOutTimer.Stop();
        }

        public void Release()
        {
            LogHelper.Debug(LoggerType.Device, string.Format("[OnFormClosing] shutting down directly"));
            if (Wrapper != null)
            {
                Wrapper.closeAllSpectrometers();
                Wrapper.Dispose();
                Wrapper = null;
            }
            DeviceList.Clear();
            newestSpectrum.Clear();
        }



        public virtual void ScanStartStop(bool start, bool isContinuous = true)
        {
            IsContinuous = isContinuous;

            if (OnWorking == start)
            {
                return;
            }

            if (start)
            {
                LogHelper.Debug(LoggerType.Device, string.Format("Start scanning"));

                OnWorking = true;

                while (FftInputDataQueue.Count != 0)
                {
                    FftInputDataQueue.TryDequeue(out FFTInputData fftInputData);
                }

                stopMeasureTask = false;

                measureTask = new Task(new Action(MeasureProc));
                measureTask.Start();

                preMeasureTask = new Task(new Action(PreMeasureProc));
                preMeasureTask.Start();
            }
            else
            {
                stopMeasureTask = true;

                if (preMeasureTask != null)
                {
                    preMeasureTask.Wait(1000);
                }

                if (measureTask != null)
                {
                    measureTask.Wait(1000);
                }

                while (FftInputDataQueue.Count != 0)
                {
                    FftInputDataQueue.TryDequeue(out FFTInputData fftInputData);
                }

                OnWorking = false;

                LogHelper.Debug(LoggerType.Device, string.Format("Stop scanning"));
            }
        }

        private void PreMeasureProc()
        {
            try
            {
                if (Wrapper == null)
                {
                    LogHelper.Debug(LoggerType.Device, string.Format("ERROR: Can't start acquisition (Missing Spectrometer or OmniDriver)"));
                    return;
                }

                bool isOK = false;

                while (stopMeasureTask == false)
                {
                    isOK = false;
                    var FFTInputData = new FFTInputData();
                    FFTInputData.Time = DateTime.Now;

                    var tasks = new List<Task>();
                    foreach (SpectrometerInfo info in DeviceList.Values)
                    {
                        tasks.Add(Task.Factory.StartNew(() =>
                        {
                            double[] newRaw = Wrapper.getSpectrum(info.Index);
                            mut.WaitOne();
                            Array.Copy(newRaw, newestSpectrum[info.Name], newestSpectrum[info.Name].Count());
                            mut.ReleaseMutex();
                        }));
                    }
                    Task.WaitAll(tasks.ToArray());

                    isOK = MergeAndBuildData(FFTInputData);
                    if (isOK == true)
                    {
                        foreach (Layer layer in LayerList.Values)
                        {
                            LayerList[layer.Name].SpecRawData.Wavelength = DeviceList.ToDictionary(x => x.Key, x => x.Value.WaveLengths);
                            LayerList[layer.Name].SpecRawData.NewSpectrum = newestSpectrum;

                            LayerList[layer.Name].SpecDividedData.DividedWavelength = FFTInputData.DividedWavelength[layer.Name];
                            LayerList[layer.Name].SpecDividedData.DividedSpectrums = FFTInputData.DividedSpectrums[layer.Name];

                            if (UseRefraction == true)
                            {
                                LayerList[layer.Name].SpecDividedData.DividedWavelengthRefraction = FFTInputData.DividedWavelengthRefraction[layer.Name];
                                LayerList[layer.Name].SpecDividedData.DividedSpectrumsRefraction = FFTInputData.DividedSpectrumsRefraction[layer.Name];
                            }
                        }

                        FftInputDataQueue.Enqueue(FFTInputData);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ScanProc\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Debug(LoggerType.Error, ex.Message + "\n" + ex.StackTrace);
            }
        }

        private bool MergeAndBuildData(FFTInputData fftInputData)
        {
            foreach (Layer layer in LayerList.Values)
            {
                fftInputData.DividedSpectrums.Add(layer.Name, new double[layer.Param.ValidDataNum]);
                fftInputData.DividedWavelength.Add(layer.Name, new double[layer.Param.ValidDataNum]);
                fftInputData.DividedSpectrumsRefraction.Add(layer.Name, new double[layer.Param.ValidDataNum]);
                fftInputData.DividedWavelengthRefraction.Add(layer.Name, new double[layer.Param.ValidDataNum]);
            }

            if (RefSpect.Count == 0 || BgSpect.Count == 0)
            {
                return false;
            }

            // 스펙트로미터를 여러개 쓸 경우 파장을 합친다.
            if (SelectedDevice == string.Empty)
            {

                int indexMap;
                string spectrometerMap;
                for (int i = 0; i < merged_Spectrum.Length; ++i)
                {
                    indexMap = memoryMaps[i].IndexMap;
                    spectrometerMap = memoryMaps[i].SpectrometerMap;

                    merged_Spectrum[i] =
                        ((newestSpectrum[spectrometerMap][indexMap] - BgSpect[spectrometerMap][indexMap]) /
                        (RefSpect[spectrometerMap][indexMap] - BgSpect[spectrometerMap][indexMap])) * 10.0;
                }

                foreach (Layer layer in LayerList.Values)
                {
                    thicknessAlgo.BuildData(ref merged_Wavelength, ref merged_Spectrum, layer, fftInputData.DividedWavelength[layer.Name], fftInputData.DividedSpectrums[layer.Name]);
                }
            }
            // 스펙트로미터를 하나만 쓸 경우 해당 스팩트로미터 정보만 가져온다.
            else
            {
                double[] wavelength = DeviceList[SelectedDevice].WaveLengths;
                double[] spectrum = new double[newestSpectrum[SelectedDevice].Length];
                for (int i = 0; i < newestSpectrum[SelectedDevice].Length; ++i)
                {
                    spectrum[i] =
                        ((newestSpectrum[SelectedDevice][i] - BgSpect[SelectedDevice][i]) /
                        (RefSpect[SelectedDevice][i] - BgSpect[SelectedDevice][i])) * 10.0;
                }

                foreach (Layer layer in LayerList.Values)
                {
                    thicknessAlgo.BuildData(ref wavelength, ref spectrum, layer, fftInputData.DividedWavelength[layer.Name], fftInputData.DividedSpectrums[layer.Name]);
                }
            }

            // 굴절률 측정을 할 경우
            if (UseRefraction == true)
            {
                double[] wavelength = DeviceList[SelectedRefractionDevice].WaveLengths;
                double[] spectrum = new double[newestSpectrum[SelectedRefractionDevice].Length];
                for (int i = 0; i < newestSpectrum[SelectedRefractionDevice].Length; ++i)
                {
                    spectrum[i] =
                        ((newestSpectrum[SelectedRefractionDevice][i] - BgSpect[SelectedRefractionDevice][i]) /
                        (RefSpect[SelectedRefractionDevice][i] - BgSpect[SelectedRefractionDevice][i])) * 10.0;
                }

                foreach (Layer layer in LayerList.Values)
                {
                    thicknessAlgo.BuildData(ref wavelength, ref spectrum, layer, fftInputData.DividedWavelengthRefraction[layer.Name], fftInputData.DividedSpectrumsRefraction[layer.Name]);
                }
            }

            return true;
        }

        private void MeasureProc()
        {
            DateTime dateTimeStart = DateTime.Now;

            while (stopMeasureTask == false)
            {
                try
                {
                    if (FftInputDataQueue.TryDequeue(out FFTInputData fftInputData) == false)
                    {
                        continue;
                    }

                    foreach (Layer layer in LayerList.Values)
                    {
                        // 굴절률 측정 기능을 사용할 경우
                        if (IsTeaching == false && UseRefraction == true)
                        {
                            double baseThickness = thicknessAlgo.GetThinkness(fftInputData.DividedSpectrums[layer.Name], layer);
                            double angleThickness = thicknessAlgo.GetThinkness(fftInputData.DividedSpectrumsRefraction[layer.Name], layer);

                            layer.Refraction = layer.Param.Refraction = thicknessAlgo.GetRefraction(baseThickness, angleThickness, AngleValue);
                            if (!double.IsNaN(layer.Param.Refraction) && !double.IsInfinity(layer.Param.Refraction))
                            {
                                layer.SpecThicknessData.MakeFFTThinkness(layer);
                            }

                            thicknessAlgo.GetThinkness(fftInputData.DividedSpectrums[layer.Name], layer);
                        }
                        // 굴절률 측정 기능을 사용하지 않을 경우
                        else
                        {
                            thicknessAlgo.GetThinkness(fftInputData.DividedSpectrums[layer.Name], layer);
                        }
                    }

                    if (MeasureDone != null && stopMeasureTask == false)
                    {
                        //Task.Run(() => { MeasureDone(fftInputData, layerList); });
                        MeasureDone(fftInputData, LayerList);
                    }

                    if (IsContinuous == false)
                    {
                        ScanStartStop(false);
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("MeasureProc\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogHelper.Debug(LoggerType.Error, "MeasureProc : " + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }



        public virtual void ReadSpectrum(string modelFolderPath)
        {
            RefSpect.Clear();
            BgSpect.Clear();

            string fileName = "";
            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                RefSpect.Add(info.Name, new double[info.WaveLengths.Length]);
                fileName = string.Format(@"{0}\..\Config\RefSpect_{1}.TXT", modelFolderPath, info.Name);
                ReadSpectrum(fileName, info.WaveLengths, RefSpect[info.Name]);

                BgSpect.Add(info.Name, new double[info.WaveLengths.Length]);
                fileName = string.Format(@"{0}\..\Config\BgSpect_{1}.TXT", modelFolderPath, info.Name);
                ReadSpectrum(fileName, info.WaveLengths, BgSpect[info.Name]);
            }
        }

        private bool ReadSpectrum(string pathName, double[] wavelengths, double[] spectrum)
        {
            if (File.Exists(pathName) == false)
            {
                return false;
            }

            using (var file = new System.IO.StreamReader(pathName))
            {
                for (int i = 0; i < spectrum.Count(); i++)
                {
                    string[] tokens = file.ReadLine().Split(',');
                    wavelengths[i] = double.Parse(tokens[0]);
                    spectrum[i] = double.Parse(tokens[1]);
                }
            }

            return true;
        }



        public virtual void SaveSequense(string modelFolderPath, bool isRef)
        {
            bool wasOnWorking = true;

            if (OnWorking == false)
            {
                ScanStartStop(true);
                wasOnWorking = false;
            }

            if (isRef == true)
            {
                SaveRefSpectrum(modelFolderPath);
            }
            else
            {
                SaveBGSpectrum(modelFolderPath);
            }

            if (wasOnWorking == false)
            {
                ScanStartStop(false);
            }
        }

        public void SaveBGSpectrum(string modelFolderPath)
        {
            var bgSpectTemp = new Dictionary<string, double[]>();

            string fileName;
            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                int avgNo = Wrapper.getScansToAverage(info.Index);
                Wrapper.setScansToAverage(info.Index, refbgAvgCount);
                Thread.Sleep(500);

                bgSpectTemp.Add(info.Name, new double[newestSpectrum[info.Name].Length]);
                Array.Copy(newestSpectrum[info.Name], bgSpectTemp[info.Name], bgSpectTemp[info.Name].Count());

                fileName = string.Format(@"{0}\..\Config\BgSpect_{1}.TXT", modelFolderPath, info.Name);
                SaveSpectrum(fileName, info.WaveLengths, bgSpectTemp[info.Name]);

                Wrapper.setScansToAverage(info.Index, avgNo);
            }
            BgSpect = bgSpectTemp;
        }

        public void SaveRefSpectrum(string modelFolderPath)
        {
            var refSpectTemp = new Dictionary<string, double[]>();

            string fileName;
            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                int avgNo = Wrapper.getScansToAverage(info.Index);
                Wrapper.setScansToAverage(info.Index, refbgAvgCount);
                Thread.Sleep(500);

                refSpectTemp.Add(info.Name, new double[newestSpectrum[info.Name].Length]);
                Array.Copy(newestSpectrum[info.Name], refSpectTemp[info.Name], refSpectTemp[info.Name].Count());

                fileName = string.Format(@"{0}\..\Config\RefSpect_{1}.TXT", modelFolderPath, info.Name);
                SaveSpectrum(fileName, info.WaveLengths, refSpectTemp[info.Name]);

                Wrapper.setScansToAverage(info.Index, avgNo);
            }
            RefSpect = refSpectTemp;
        }

        private bool SaveSpectrum(string pathName, double[] wavelength, double[] spectrum)
        {
            using (var file = new System.IO.StreamWriter(pathName))
            {
                for (int i = 0; i < spectrum.Count(); i++)
                {
                    file.WriteLine("{0:0.00}, {1:0.00000}", wavelength[i], spectrum[i]);
                }

                file.Flush();
            }

            return true;
        }



        public Dictionary<string, double[]> GetNewSpectrum()
        {
            var targetSpectrum = new Dictionary<string, double[]>();

            foreach (SpectrometerInfo info in DeviceList.Values)
            {
                targetSpectrum.Add(info.Name, new double[newestSpectrum[info.Name].Length]);
                Array.Copy(newestSpectrum[info.Name], targetSpectrum[info.Name], targetSpectrum[info.Name].Count());
            }

            return targetSpectrum;
        }

        public double GetThickness(string layerName)
        {
            return LayerList[layerName].Thickness;
        }

        public List<double> GetThicknessList()
        {
            var thicknessList = new List<double>();

            foreach (Layer layer in LayerList.Values)
            {
                thicknessList.Add(layer.Thickness);
            }

            return thicknessList;
        }

        public double GetRefraction(double baseThickness, double angleThickness, double angle)
        {
            return thicknessAlgo.GetRefraction(baseThickness, angleThickness, angle);
        }

        public virtual void UpdateLayer(SpectrometerProperty property)
        {
            LayerList.Clear();
            for (int i = 0; i < property.LayerNum; i++)
            {
                LayerList.Add(property.LayerNameList[i], new Layer(property.LayerNameList[i], new LayerParam(string.Format("LayerParam{0}", i))));
            }
        }

        public virtual void UpdateSpectrometer(SpectrometerProperty property, string specName = "")
        {
            SelectedDevice = specName;

            if (specName != "")
            {
                int index = -1;
                foreach (SpectrometerInfo info in DeviceList.Values)
                {
                    if (info.Name == specName)
                    {
                        index = info.Index;
                    }
                }

                if (index != -1)
                {
                    if (property.IntegrationTime[specName] > 0)
                    {
                        Wrapper.setIntegrationTime(index, property.IntegrationTime[specName] * 1000);
                    }

                    Wrapper.setScansToAverage(index, property.Average[specName]);
                    Wrapper.setBoxcarWidth(index, property.Boxcar[specName]);
                }
            }
            else
            {
                foreach (SpectrometerInfo info in DeviceList.Values)
                {
                    if (property.IntegrationTime.ContainsKey(info.Name) == true)
                    {
                        if (property.IntegrationTime[info.Name] > 0)
                        {
                            Wrapper.setIntegrationTime(info.Index, property.IntegrationTime[info.Name] * 1000);
                        }

                        Wrapper.setScansToAverage(info.Index, property.Average[info.Name]);
                        Wrapper.setBoxcarWidth(info.Index, property.Boxcar[info.Name]);
                    }
                }
            }
        }

        public virtual void UpdateParam(LayerParam layerParam, string layerName)
        {
            bool wasOnWorking = false;

            if (OnWorking == true)
            {
                ScanStartStop(false);
                wasOnWorking = true;
            }

            LayerList[layerName].Name = layerName;
            LayerList[layerName].Param.CopyFrom(layerParam);
            LayerList[layerName].SpecThicknessData.Initialize(LayerList[layerName].Param);

            if (SelectedDevice == string.Empty)
            {
                BuildResampledWavelength(merged_Wavelength, LayerList[layerName]);
            }
            else
            {
                BuildResampledWavelength(DeviceList[SelectedDevice].WaveLengths, LayerList[layerName]);
            }

            LayerList[layerName].SpecThicknessData.MakeFFTThinkness(LayerList[layerName]);


            if (wasOnWorking == true)
            {
                ScanStartStop(true);
            }
        }

        private int AllocMemoryAndMap()
        {
            var cuttingPoses = new List<double>();
            var waveLengthTemp = new List<double>();
            memoryMaps.Clear();

            // 스펙트로미터가 여러 개일 경우 겹치는 부분의 파장을 중간 값으로 취한다.
            var infos = DeviceList.Values.ToList();
            infos.Sort((x, y) => x.WaveLengths.First().CompareTo(y.WaveLengths.First()));
            for (int i = 0; i < infos.Count - 1; i++)
            {
                if (infos[i].WaveLengths.Last() > infos[i + 1].WaveLengths.First())
                {
                    cuttingPoses.Add((infos[i].WaveLengths.Last() + infos[i + 1].WaveLengths.First()) / 2);
                }
            }
            // 가장 마지막 파장을 끝으로 저장한다.
            cuttingPoses.Add(infos.Last().WaveLengths.Last());
            // waveLengthTemp에 여러 파장을 한번에 넣어준다.
            for (int index = 0; index < infos.Count; ++index)
            {
                for (int i = 0; i < infos[index].WaveLengths.Length; ++i)
                {
                    if (infos[index].WaveLengths[i] <= cuttingPoses[index])
                    {
                        memoryMaps.Add(new MemoryMap(infos[index].Name, i));
                        waveLengthTemp.Add(infos[index].WaveLengths[i]);
                    }
                }
            }
            // merged_Spectrum 사이즈를 waveLengthTemp에 맞게 생성하거나 재할당 한다.
            if (merged_Spectrum == null)
            {
                merged_Spectrum = new double[waveLengthTemp.Count];
            }
            else
            {
                Array.Resize<double>(ref merged_Spectrum, waveLengthTemp.Count);
            }
            // merged_Wavelength 사이즈를 waveLengthTemp에 맞게 생성하거나 재할당 한다.
            if (merged_Wavelength == null)
            {
                merged_Wavelength = new double[waveLengthTemp.Count];
            }
            else
            {
                Array.Resize<double>(ref merged_Wavelength, waveLengthTemp.Count);
            }
            // waveLengthTemp를 merged_Wavelength에 넣어준다.
            merged_Wavelength = waveLengthTemp.ToArray<double>();

            return 0;
        }

        // 합쳐진 파장을 이용하여 리셈플링한 파장을 만든다.
        private int BuildResampledWavelength(double[] wavelength, Layer layer)
        {
            SpecThicknessData spec = layer.SpecThicknessData;

            Array.Clear(spec.ResampledWavelength, 0, spec.ResampledWavelength.Length);

            int size = (int)Math.Pow(2, layer.Param.DataLengthPow);
            double step = (1.0 / wavelength[wavelength.Length - 1]) - (1.0 / wavelength[wavelength.Length - 2]);
            double temp = (1.0 / wavelength[0]);

            for (int i = 0; i < size; ++i)
            {
                temp += step;
                spec.ResampledWavelength[i] = 1.0 / temp;

                layer.Param.ValidDataNum++;

                // 합쳐진 파장보다 더 길게 리셈플링 파장을 넣지는 않는다.
                // 뒤에는 FFT를 위한 0으로 채우기 위함임.
                if (spec.ResampledWavelength[i] > wavelength[wavelength.Length - 2])
                {
                    break;
                }
            }

            return 0;
        }
    }

    public class VirtualSpectrometer : Spectrometer
    {
        public override void Initialize(SpectrometerProperty spectrometerProperty)
        {

        }

        public override void ScanStartStop(bool start, bool isContinuous = true)
        {

        }

        public override void ReadSpectrum(string modelFolderPath)
        {

        }

        public override void SaveSequense(string modelFolderPath, bool isRef)
        {

        }

        public override void UpdateLayer(SpectrometerProperty property)
        {

        }

        public override void UpdateSpectrometer(SpectrometerProperty property, string specName = "")
        {

        }

        public override void UpdateParam(LayerParam layerParam, string layerName)
        {

        }
    }

    public class MemoryMap
    {
        public string SpectrometerMap { get; set; }
        public int IndexMap { get; set; }

        public MemoryMap(string spectrometerMap, int indexMap)
        {
            SpectrometerMap = spectrometerMap;
            IndexMap = indexMap;
        }
    }

    public delegate void MeasureDoneDelegate(FFTInputData fftInputData, Dictionary<string, Layer> layerList);
}