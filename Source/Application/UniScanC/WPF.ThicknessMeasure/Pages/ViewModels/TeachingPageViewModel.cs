using DynMvp.Data;
using DynMvp.Devices.Spectrometer;
using DynMvp.Devices.Spectrometer.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.ThicknessMeasure.Model;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Pages.ViewModels
{
    internal class TeachingPageViewModel : Observable
    {
        #region 필드
        private bool onWorking = false;
        private bool saveRefractionData = false;
        private Model.ModelDescription modelDescription;
        private List<string> layerNameList;
        private string selectedLayerName;
        private LayerParam layerParam;
        private SpectrometerProperty spectrometerProperty;
        private List<DynMvp.Devices.Spectrometer.SpectrometerInfo> infoList;
        private DynMvp.Devices.Spectrometer.SpectrometerInfo selectedInfo;
        private int integrationTime;
        private int average;
        private int boxcar;
        private int threshold;
        private float angleValue;
        private double refractionData;
        private ObservableRangeCollection<PointF> rawData;
        private ObservableRangeCollection<PointF> divData;
        private ObservableRangeCollection<PointF> thickData;
        private ObservableRangeCollection<PointF> phaseData;
        private int queueCount;
        private Layer targetLayer;
        private Layer baseRefractionLayer;
        private double currentThickness;
        private double maxIntansity;

        private ICommand referenceCommand;
        private ICommand backgroundCommand;
        private ICommand aquireCommand;
        private ICommand dataSaveCommand;
        private ICommand saveBaseAngleDataCommand;
        private ICommand saveRefractionDataCommand;
        #endregion

        #region 생성자
        public TeachingPageViewModel()
        {
            SystemConfig config = SystemConfig.Instance;

            // Spectrometer
            SpectrometerProperty = config.SpectrometerProperty;
            InfoList = Spectrometer.DeviceList.Values.ToList();

            // Layer
            LayerNameList = config.SpectrometerProperty.LayerNameList;

            LayerParam = new LayerParam();

            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;
        }
        #endregion

        #region 속성
        // 스펙트로미터와 레이어를 골랐는지 확인하는 변수
        public bool OnSelected => (SelectedLayerName != null) && (SelectedInfo != null);
        // 스펙트로미터가 데이터를 획득 중인지 확인하는 변수
        public bool OnWorking { get => onWorking; set => Set(ref onWorking, value); }
        // Model
        public Model.ModelDescription ModelDescription
        {
            get => modelDescription;
            set => Set(ref modelDescription, value);
        }
        // Layer
        public List<string> LayerNameList { get => layerNameList; set => Set(ref layerNameList, value); }

        public string SelectedLayerName
        {
            get => selectedLayerName;
            set
            {
                Set(ref selectedLayerName, value);
                if (Model.ModelManager.Instance().CurrentModel != null)
                {
                    //LayerParam.CopyFrom(SystemConfig.Instance.LayerParamList[SelectedLayerName].Find(x => x.ParamName == ModelDescription.LayerParamList[SelectedLayerName]));
                    LayerParam = SystemConfig.Instance.LayerParamList[SelectedLayerName].Find(x => x.ParamName == ModelDescription.LayerParamList[SelectedLayerName]);
                    LayerParam.PropertyChanged += LayerParamPropertyChanged;
                    OnPropertyChanged("LayerParam");
                    Spectrometer.UpdateParam(LayerParam, SelectedLayerName);
                    Spectrometer.MeasureDone = MeasureDone;

                    OnPropertyChanged("OnSelected");
                }
            }
        }

        public LayerParam LayerParam { get => layerParam; set => Set(ref layerParam, value); }

        public Dictionary<string, Layer> LastMeasureLayerList { get; set; }

        // Spectrometer
        public SpectrometerProperty SpectrometerProperty { get => spectrometerProperty; set => Set(ref spectrometerProperty, value); }

        public DynMvp.Devices.Spectrometer.Spectrometer Spectrometer => ((DeviceManager)DeviceManager.Instance()).Spectrometer;

        public List<DynMvp.Devices.Spectrometer.SpectrometerInfo> InfoList { get => infoList; set => Set(ref infoList, value); }

        public DynMvp.Devices.Spectrometer.SpectrometerInfo SelectedInfo
        {
            get => selectedInfo;
            set
            {
                Set(ref selectedInfo, value);
                OnPropertyChanged("IntegrationTime");
                OnPropertyChanged("Average");
                OnPropertyChanged("Boxcar");
                OnPropertyChanged("Threshold");
                UpdateSpectrometer();

                OnPropertyChanged("OnSelected");
            }
        }

        public int IntegrationTime
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.IntegrationTime[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref integrationTime, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.IntegrationTime[SelectedInfo.Name] = integrationTime;
                    UpdateSpectrometer();
                }
                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public int Average
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.Average[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref average, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.Average[SelectedInfo.Name] = average;
                    UpdateSpectrometer();
                }
                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public int Boxcar
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.Boxcar[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref boxcar, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.Boxcar[SelectedInfo.Name] = boxcar;
                    UpdateSpectrometer();
                }
                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public int Threshold
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.LampPitch[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref threshold, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.LampPitch[SelectedInfo.Name] = threshold;
                    UpdateSpectrometer();
                }
                OnPropertyChanged("SpectrometerProperty");
            }
        }

        // Refraction
        public float AngleValue
        {
            get => angleValue;
            set => Set(ref angleValue, value);
        }

        public double RefractionData
        {
            get => refractionData;
            set => Set(ref refractionData, value);
        }

        // Data
        public ObservableRangeCollection<PointF> RawData { get => rawData; set => Set(ref rawData, value); }

        public ObservableRangeCollection<PointF> DivData { get => divData; set => Set(ref divData, value); }

        public ObservableRangeCollection<PointF> ThickData { get => thickData; set => Set(ref thickData, value); }

        public ObservableRangeCollection<PointF> PhaseData { get => phaseData; set => Set(ref phaseData, value); }

        public double CurrentThickness { get => currentThickness; set => Set(ref currentThickness, value); }

        public double MaxIntansity { get => maxIntansity; set => Set(ref maxIntansity, value); }

        public int QueueCount { get => queueCount; set => Set(ref queueCount, value); }

        public ICommand ReferenceCommand => referenceCommand ?? (referenceCommand = new RelayCommand(Reference));

        public ICommand BackgroundCommand => backgroundCommand ?? (backgroundCommand = new RelayCommand(Background));

        public ICommand AquireCommand => aquireCommand ?? (aquireCommand = new RelayCommand(Aquire));

        public ICommand DataSaveCommand => dataSaveCommand ?? (dataSaveCommand = new RelayCommand(DataSave));

        public ICommand SaveBaseAngleDataCommand => saveBaseAngleDataCommand ?? (saveBaseAngleDataCommand = new RelayCommand(SaveBaseAngleData));

        public ICommand SaveRefractionDataCommand => saveRefractionDataCommand ?? (saveRefractionDataCommand = new RelayCommand(SaveRefractionData));
        #endregion

        #region 메서드
        private void Reference()
        {
            Spectrometer.SaveRefSpectrum(Model.ModelManager.Instance().ModelPath);
        }

        private void Background()
        {
            Spectrometer.SaveBGSpectrum(Model.ModelManager.Instance().ModelPath);
        }

        private void Aquire()
        {
            if (Spectrometer.OnWorking == false)
            {
                Spectrometer.ScanStartStop(true);
                Spectrometer.IsTeaching = true;
                OnWorking = true;
            }
            else
            {
                Spectrometer.ScanStartStop(false);
                Spectrometer.IsTeaching = false;
                OnWorking = false;
            }
        }

        private void DataSave()
        {
            if (LastMeasureLayerList == null)
            {
                return;
            }

            foreach (KeyValuePair<string, Layer> layer in LastMeasureLayerList)
            {
                string dataFile = Path.Combine(SystemConfig.Instance.ResultPath, DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_TeachingData.csv");
                using (var sw = new StreamWriter(dataFile, true))
                {
                    sw.WriteLine($"Layer : {layer.Key}");

                    if (layer.Value.SpecRawData != null)
                    {
                        SpecRawData rawData = layer.Value.SpecRawData;
                        sw.WriteLine("Raw WaveLength");
                        foreach (KeyValuePair<string, double[]> waveLength in rawData.Wavelength)
                        {
                            sw.WriteLine($"WaveLength : {waveLength.Key}");
                            string waveLengthStr = "";
                            foreach (double waveLengthData in waveLength.Value)
                            {
                                waveLengthStr += $"{waveLengthData},";
                            }
                            sw.WriteLine(waveLengthStr);
                        }
                        sw.WriteLine("Raw Spectrum");
                        foreach (KeyValuePair<string, double[]> spectrum in rawData.NewSpectrum)
                        {
                            sw.WriteLine($"Spectrum : {spectrum.Key}");
                            string spectrumStr = "";
                            foreach (double spectrumData in spectrum.Value)
                            {
                                spectrumStr += $"{spectrumData},";
                            }
                            sw.WriteLine(spectrumStr);
                        }
                    }

                    if (layer.Value.SpecDividedData != null)
                    {
                        SpecDividedData dividedData = layer.Value.SpecDividedData;
                        string waveLengthStr = "";
                        string spectrumStr = "";
                        for (int i = 0; i < dividedData.DividedWavelength.Length; i++)
                        {
                            double waveLengthData = dividedData.DividedWavelength[i];
                            double spectrumData = dividedData.DividedSpectrums[i];
                            waveLengthStr += $"{waveLengthData},";
                            spectrumStr += $"{spectrumData},";
                        }
                        sw.WriteLine("Div WaveLength");
                        sw.WriteLine(waveLengthStr);
                        sw.WriteLine("Div Spectrum");
                        sw.WriteLine(spectrumStr);
                    }

                    if (layer.Value.SpecThicknessData != null)
                    {
                        SpecThicknessData thicknessData = layer.Value.SpecThicknessData;
                        string waveLengthStr = "";
                        string spectrumStr = "";
                        for (int i = 0; i < thicknessData.FFTThickness.Length; i++)
                        {
                            double waveLengthData = thicknessData.FFTThickness[i];
                            double spectrumData = thicknessData.FFTSpectrum[i];
                            if (waveLengthData > layer.Value.Param.MinThickness)
                            {
                                if (waveLengthData < layer.Value.Param.MaxThickness)
                                {
                                    waveLengthStr += $"{waveLengthData},";
                                    spectrumStr += $"{spectrumData},";
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        sw.WriteLine("Thick WaveLength");
                        sw.WriteLine(waveLengthStr);
                        sw.WriteLine("Thick Spectrum");
                        sw.WriteLine(spectrumStr);
                    }

                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private void SaveBaseAngleData()
        {
            baseRefractionLayer = targetLayer.Clone();
        }

        private void SaveRefractionData()
        {
            saveRefractionData = !saveRefractionData;
        }

        private void LayerParamPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SelectedLayerName == null)
            {
                return;
            }

            var param = sender as LayerParam;

            Spectrometer.UpdateParam(param, SelectedLayerName);
            SystemConfig.Instance.LayerParamList[SelectedLayerName].Find(x => x.ParamName == ModelDescription.LayerParamList[SelectedLayerName]).CopyFrom(param);
            SystemConfig.Instance.Save();
        }

        private void UpdateSpectrometer()
        {
            Spectrometer.UpdateSpectrometer(SpectrometerProperty, SelectedInfo.Name);
            SystemConfig.Instance.SpectrometerProperty = SpectrometerProperty;
            SystemConfig.Instance.Save();
        }

        private void MeasureDone(FFTInputData fftInputData, Dictionary<string, Layer> layerList)
        {
            LastMeasureLayerList = layerList;
            string layerName = SelectedLayerName;
            string spec = SelectedInfo.Name;
            targetLayer = layerList[layerName];
            CurrentThickness = targetLayer.Thickness;
            MaxIntansity = targetLayer.SpecRawData.NewSpectrum[spec].Max();

            var tempRawData = new ObservableRangeCollection<PointF>();
            for (int i = 0; i < targetLayer.SpecRawData.Wavelength[spec].Length; i++)
            {
                tempRawData.Add(new PointF(Convert.ToSingle(targetLayer.SpecRawData.Wavelength[spec][i]), Convert.ToSingle(targetLayer.SpecRawData.NewSpectrum[spec][i])));
            }

            RawData = tempRawData;

            var tempDivData = new ObservableRangeCollection<PointF>();
            for (int i = 0; i < targetLayer.SpecDividedData.DividedWavelength.Length; i++)
            {
                if (targetLayer.SpecDividedData.DividedWavelength[i] >= layerParam.StartWavelength && targetLayer.SpecDividedData.DividedWavelength[i] <= layerParam.EndWavelength)
                {
                    tempDivData.Add(new PointF(Convert.ToSingle(targetLayer.SpecDividedData.DividedWavelength[i]), Convert.ToSingle(targetLayer.SpecDividedData.DividedSpectrums[i])));
                }
            }
            DivData = tempDivData;

            var tempThickData = new ObservableRangeCollection<PointF>();
            for (int i = 0; i < targetLayer.SpecThicknessData.FFTThickness.Length; i++)
            {
                if (targetLayer.SpecThicknessData.FFTThickness[i] >= layerParam.MinThickness && targetLayer.SpecThicknessData.FFTThickness[i] <= layerParam.MaxThickness)
                {
                    tempThickData.Add(new PointF(Convert.ToSingle(targetLayer.SpecThicknessData.FFTThickness[i]), Convert.ToSingle(targetLayer.SpecThicknessData.FFTSpectrum[i])));
                }
            }
            ThickData = tempThickData;

            //ObservableRangeCollection<PointF> tempPhaseData = new ObservableRangeCollection<PointF>();
            //for (int i = 0; i < targetLayer.SpecThicknessData.FFTThickness.Length; i++)
            //{
            //    if (targetLayer.SpecThicknessData.FFTThickness[i] >= layerParam.MinThickness && targetLayer.SpecThicknessData.FFTThickness[i] <= layerParam.MaxThickness)
            //        tempPhaseData.Add(new PointF(Convert.ToSingle(targetLayer.SpecThicknessData.FFTThickness[i]), Convert.ToSingle(targetLayer.SpecThicknessData.FFTPhase[i])));
            //}
            //this.PhaseData = tempPhaseData;

            if (targetLayer != null)
            {
                RefractionData = Spectrometer.GetRefraction(baseRefractionLayer.Thickness, targetLayer.Thickness, AngleValue);
            }

            if (saveRefractionData)
            {
                SaveRefraction();
            }

            QueueCount = Spectrometer.FftInputDataQueue.Count;
        }

        private void SaveRefraction()
        {
            string dataFile = Path.Combine(SystemConfig.Instance.ResultPath, "RefractionData.csv");
            using (var sw = new StreamWriter(dataFile, true))
            {
                sw.WriteLine(string.Format("{0},{1}", DateTime.Now.ToString("yyyyMMdd - HH:mm:ss:fff"), RefractionData));
                sw.Flush();
                sw.Close();
            }
        }

        public void ModelOpened(ModelBase modelBase)
        {
            var modelDescription = modelBase.ModelDescription as Model.ModelDescription;
            ModelDescription = modelDescription;
            OnPropertyChanged("LayerParam");
        }

        private void ModelClosed()
        {
            ModelDescription = null;
        }
        #endregion
    }
}
