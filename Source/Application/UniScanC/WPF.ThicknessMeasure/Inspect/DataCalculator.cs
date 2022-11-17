using DynMvp.Data;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Spectrometer;
using DynMvp.Devices.Spectrometer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniScanC.Data;
using UniScanC.Models;
using WPF.ThicknessMeasure.Model;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Inspect
{
    public delegate void CalDoneDelegate(Dictionary<string, ThicknessScanData> logScanData);

    public class DataCalculator
    {
        #region 필드
        // ModelDescription
        private Dictionary<string, LayerParam> layerParamList = new Dictionary<string, LayerParam>();
        private ScanWidth scanWidth;
        // Data
        private Dictionary<string, ThicknessScanData> scanData = null;
        // Status
        private bool canMeasure = true;
        // Offline Mode
        private int virtualReelPosition = 0;
        private int virtualMotionPosition = 0;
        #endregion


        #region 생성자
        public DataCalculator()
        {
            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;
        }
        #endregion


        #region 속성
        // CalDoneDelegate
        public CalDoneDelegate CalDone { get; set; } = null;

        // Spectrometer
        private DynMvp.Devices.Spectrometer.Spectrometer Spectrometer => ((Override.DeviceManager)DynMvp.Devices.DeviceManager.Instance()).Spectrometer;

        // Motion
        private AxisHandler AxisHandler => Override.DeviceManager.Instance().RobotStage;

        private AngleCaculator AngleCaculator { get; set; }

        private List<string> LayerNameList => SystemConfig.Instance.SpectrometerProperty.LayerNameList;
        #endregion


        #region 메서드
        public void ModelOpened(ModelBase modelBase)
        {
            var modelDescription = modelBase.ModelDescription as Model.ModelDescription;

            SystemConfig config = SystemConfig.Instance;

            scanData = new Dictionary<string, ThicknessScanData>();
            layerParamList.Clear();
            foreach (string layerName in modelDescription.LayerParamList.Keys)
            {
                if (config.SpectrometerProperty.LayerNameList.Exists(x => x == layerName) == true)
                {
                    scanData.Add(layerName, new ThicknessScanData());

                    LayerParam param = config.LayerParamList[layerName].Find(x => x.ParamName == modelDescription.LayerParamList[layerName]);
                    if (param != null)
                    {
                        layerParamList.Add(layerName, param);
                    }
                }
            }

            scanWidth = config.ScanWidthList.Find(x => x.Name == modelDescription.ScanWidth);

            ReadBRSpectrum();
            UpdateSpectromter(config.SpectrometerProperty, modelDescription.SensorType);
            UpdateSpectromterParam();

            AngleCaculator = new AngleCaculator();
        }

        public void ModelClosed()
        {
            scanWidth = null;
        }

        public void ModelListChanged() { }

        public void ScanProc(bool start)
        {
            if (AxisHandler == null)
            {
                virtualReelPosition = 0;
                virtualMotionPosition = 0;
            }

            if (start == true)
            {
                ClearScanData();
                Spectrometer.MeasureDone = MeasureDone;
                Spectrometer.ScanStartStop(true);

                AngleCaculator.AngleScanStartStop(true);
            }
            else
            {
                ClearScanData();
                Spectrometer.ScanStartStop(false);
                Spectrometer.MeasureDone = null;

                AngleCaculator.AngleScanStartStop(false);
            }
        }

        public void MeasureDone(FFTInputData fftInputData, Dictionary<string, Layer> layerList)
        {
            if (canMeasure == true)
            {
                if (AxisHandler != null)
                {
                    fftInputData.Position = AxisHandler.GetAxis("X").GetActualPos();
                }
                else
                {
                    fftInputData.Position = virtualReelPosition;
                }

                float position = /*SystemConfig.Instance.PositionOffset - */fftInputData.Position / 1000f;
                DateTime time = fftInputData.Time;
                float dataCount = Convert.ToSingle(SystemConfig.Instance.MeasureSecond) * 50;

                FindPinhole(layerList);

                AngleCaculator?.CalculateAngle(layerList);

                foreach (Layer layer in layerList.Values)
                {
                    string layerName = layer.Name;
                    if (AxisHandler != null)
                    {
                        if (layer.Thickness == 0)
                        {
                            scanData[layerName].AddValue(time, position, (float)layer.Thickness, (float)layer.Refraction, (float)layer.Angle);
                        }
                        else
                        {
                            scanData[layerName].AddValue(time, position, (float)layer.Thickness + scanData[layerName].Offset, (float)layer.Refraction, (float)layer.Angle);
                        }
                    }
                    else
                    {
                        float vs = scanWidth.ValidStart;
                        float ve = scanWidth.ValidEnd;
                        float range = ve - vs;
                        float virtualPos = vs + (range / dataCount * virtualMotionPosition);

                        if (virtualPos < ve)
                        {
                            scanData[layerName].AddValue(time, virtualPos, (float)layer.Thickness + scanData[layerName].Offset, (float)layer.Refraction, (float)layer.Angle);
                            virtualMotionPosition++;
                        }
                    }
                }
            }
        }

        // 모션이 끝에서 끝으로 이동을 완료하면 MoveDone Delegate 가 온다.
        public void MotionMoveDone()
        {
            Task.Run(new Action(() => AfterScanTask()));
        }

        private void FindPinhole(Dictionary<string, Layer> layerList)
        {
            foreach (Layer layer in layerList.Values)
            {
                double tempThick = 0;
                double tempSpec = 0;
                double maxSpec = double.MinValue;
                for (int i = 0; i < layer.SpecThicknessData.FFTSpectrum.Length; ++i)
                {
                    tempThick = layer.SpecThicknessData.FFTThickness[i];
                    tempSpec = layer.SpecThicknessData.FFTSpectrum[i];
                    if (layer.Param.MinThickness <= tempThick && tempThick <= layer.Param.MaxThickness)
                    {
                        maxSpec = Math.Max(maxSpec, tempSpec);
                    }
                }
                if (maxSpec < layer.Param.Threshold)
                {
                    layer.Thickness = 0;
                }
            }
        }

        private void AfterScanTask()
        {
            canMeasure = false;
            if (AxisHandler == null)
            {
                virtualMotionPosition = 0;
            }

            // PLC 연동하여 길이를 가져오면 코드 바꾸기
            if (true)
            {
                virtualReelPosition += Convert.ToInt32(SystemConfig.Instance.MeasureSecond * 2);
                foreach (ThicknessScanData tempScanData in scanData.Values)
                {
                    tempScanData.ReelPosition = virtualReelPosition;
                }
            }

            var logScanData = new Dictionary<string, ThicknessScanData>();
            foreach (string layerName in LayerNameList)
            {
                logScanData.Add(layerName, scanData[layerName].Clone());
            }

            ClearScanData();
            canMeasure = true;

            // 데이터 방향 설정
            SortData(logScanData);
            // 자체 데이터 계산
            CalculateData(logScanData);
            // 데이터 완성
            CalDone?.Invoke(logScanData);
        }

        private void ClearScanData()
        {
            foreach (ThicknessScanData tempScanData in scanData.Values)
            {
                tempScanData.Clear();
            }
        }

        // Call Data
        private void SortData(Dictionary<string, ThicknessScanData> logScanData)
        {
            foreach (string layerName in LayerNameList)
            {
                logScanData[layerName].DataList = logScanData[layerName].DataList.OrderBy(x => x.Position).ToList();
            }
        }

        private void CalculateData(Dictionary<string, ThicknessScanData> logScanData)
        {
            foreach (string layerName in LayerNameList)
            {
                int count = logScanData[layerName].DataList.Count;
                FilteringData(logScanData[layerName], scanWidth.Start, scanWidth.End);
                SetAverageData(logScanData[layerName], scanWidth.Start, scanWidth.End);
            }
        }

        private void CalculateMovingAverage(Dictionary<string, ThicknessScanData> logScanData)
        {
            foreach (string layerName in LayerNameList)
            {
                // 가공 시작
                if (SystemConfig.Instance.MovingAverageCount != 0)
                {
                    SetMovingAverage(logScanData[layerName], scanWidth.Start, scanWidth.End);
                }
            }
        }

        private void FilteringData(ThicknessScanData scanData, float startPos, float endPos)
        {
            if (scanData.ValidPointCount >= 2)
            {
                bool isStartFound = false;

                // 시작점 찾기
                while (isStartFound == false)
                {
                    if (scanData.ValidPointCount <= 1)
                    {
                        break;
                    }

                    // 상수 부분이 숫자가 시작점과 같으면 거르기, 전에 있던 데이터가 나중에 나온 데이터보다 크면 거르기
                    if (scanData.DataList[0].Position / 1 <= startPos
                        || scanData.DataList[0].Position >= scanData.DataList[1].Position)
                    {
                        scanData.DataList.RemoveAt(0);
                    }
                    else
                    {
                        isStartFound = true;
                    }
                }

                if (isStartFound == false)
                {
                    return;
                }
                // 중복된 시작점 데이터 제거
                for (int i = 1; i < scanData.ValidPointCount; i++)
                {
                    if (scanData.DataList[i].Position == scanData.DataList[i - 1].Position)
                    {
                        scanData.DataList.RemoveAt(0);
                        i--;
                    }
                    else
                    {
                        break;
                    }
                }

                // 중복된 끝점 데이터 및 벗어난 데이터 제거
                for (int i = scanData.ValidPointCount - 1; i > 0; i--)
                {
                    if (scanData.DataList[i].Position / 1 >= Math.Max(endPos - 1, 0)
                        || scanData.DataList[i].Position <= scanData.DataList[i - 1].Position)
                    {
                        scanData.DataList.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void SetAverageData(ThicknessScanData scanData, float startPos, float endPos)
        {
            var thicknessList = new List<float>();
            for (int i = 0; i < scanData.DataList.Count(); i++)
            {
                if (scanData.DataList[i].Position >= startPos || scanData.DataList[i].Position <= endPos)
                {
                    thicknessList.Add(scanData.DataList[i].Thickness);
                }
            }

            if (thicknessList.Count > 1)
            {
                scanData.Max = thicknessList.Max();
                scanData.Min = thicknessList.Min();
                scanData.Average = thicknessList.Average();
            }
        }

        private void SetMovingAverage(ThicknessScanData scanData, float startPos, float endPos)
        {
            ThicknessScanData tempScanData = scanData.Clone();

            int count = SystemConfig.Instance.MovingAverageCount;

            for (int i = 0; i < scanData.ValidPointCount; i++)
            {
                float average = 0;
                int valueCount = 0;

                for (int k = i - count; k <= i + count; k++)
                {
                    if (k < 0)
                    {
                        continue;
                    }
                    else if (k > scanData.ValidPointCount - 1)
                    {
                        break;
                    }

                    average += scanData.DataList[k].Thickness;
                    valueCount++;
                }

                if (valueCount > 0)
                {
                    average /= valueCount;
                    tempScanData.DataList[i].Thickness = average;
                }
            }

            // 데이터 초기화
            scanData = tempScanData;

            // 자체 계산값 다시 입히기
            SetAverageData(scanData, startPos, endPos);
        }

        private void ReadBRSpectrum()
        {
            Spectrometer.ReadSpectrum(PathConfig.Instance().Model);
        }

        private void UpdateSpectromter(SpectrometerProperty property, string specName = "")
        {
            Spectrometer.UpdateSpectrometer(property, specName);
        }

        private void UpdateSpectromterParam()
        {
            foreach (string key in layerParamList.Keys)
            {
                Spectrometer.UpdateParam(layerParamList[key], key);
            }
        }
        #endregion
    }
}
