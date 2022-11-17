using DynMvp.Data;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Spectrometer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniScanC.Models;
using WPF.ThicknessMeasure.Model;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Inspect
{
    public delegate void MoveDoneDelegate();

    public class MotionController
    {
        #region 필드
        // ModelDesc
        private ScanWidth scanWidth;
        // Status
        private bool isforward = true;
        // Virtual Mode
        private float motionPos = 0;
        #endregion

        #region 생성자
        public MotionController()
        {
            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;
        }
        #endregion

        #region 속성
        // MoveDoneDelegate
        public MoveDoneDelegate MoveDone { get; set; } = null;

        // CancellationTokenSource
        public CancellationTokenSource CancellationTokenSource { get; set; }

        // Spectrometer
        private DynMvp.Devices.Spectrometer.Spectrometer Spectrometer => ((Override.DeviceManager)DynMvp.Devices.DeviceManager.Instance()).Spectrometer;

        // Motion
        private AxisHandler AxisHandler => Override.DeviceManager.Instance().RobotStage;
        #endregion

        #region 메서드
        public void ModelOpened(ModelBase modelBase)
        {
            var modelDescription = modelBase.ModelDescription as Model.ModelDescription;

            SystemConfig config = SystemConfig.Instance;
            scanWidth = config.ScanWidthList.Find(x => x.Name == modelDescription.ScanWidth);
        }

        public void ModelClosed()
        {
            scanWidth = null;
        }

        public void ModelListChanged() { }

        // Sequence
        public void ScanProc()
        {
            var scanStartPos = new AxisPosition(1);
            var scanEndPos = new AxisPosition(1);
            var movingParam = new MovingParam();
            double measureSecond = SystemConfig.Instance.MeasureSecond;

            // 포지션 일치
            //float positionOffset = SystemConfig.Instance.PositionOffset;
            scanStartPos.Position[0] = (/*positionOffset - */scanWidth.Start) * 1000f;
            scanEndPos.Position[0] = (/*positionOffset - */scanWidth.End) * 1000f;

            try
            {
                if (AxisHandler != null)
                {
                    // 검사 속도 입혀주기    
                    movingParam.MaxVelocity = ((scanWidth.End - scanWidth.Start) * 1000f) / measureSecond
                        / AxisHandler.AxisList[0].AxisParam.MicronPerPulse;

                    var progress = new ProgressSource();
                    progress.CancellationTokenSource = CancellationTokenSource;

                    AxisHandler.Move(scanStartPos, CancellationTokenSource.Token);

                    //await MessageWindowHelper.ShowProgress(null, "Preparing Start...", new Action(() =>
                    //{
                    //    AxisHandler.Move(scanStartPos, CancellationTokenSource.Token);
                    //}), true, progress);

                    CancellationTokenSource.Token.ThrowIfCancellationRequested();
                }

                isforward = true;

                while (true)
                {
                    if (AxisHandler != null)
                    {
                        if (isforward == true)
                        {
                            AxisHandler.StartMove(new int[] { 0 }, scanEndPos, movingParam);
                        }
                        else
                        {
                            AxisHandler.StartMove(new int[] { 0 }, scanStartPos, movingParam);
                        }

                        isforward = (isforward == true ? false : true);
                    }
                    else
                    {
                        motionPos = 0;
                    }

                    while (true)
                    {
                        CancellationTokenSource.Token.ThrowIfCancellationRequested();

                        if (AxisHandler != null)
                        {
                            if (AxisHandler.IsMoveDone() == true)
                            {
                                // 모션 이동 완료
                                MoveDone?.Invoke();
                                break;
                            }
                        }
                        else
                        {
                            if (motionPos > measureSecond * 50.0f)
                            {
                                MoveDone?.Invoke();
                                break;
                            }
                            motionPos++;
                            Thread.Sleep(20);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                if (AxisHandler != null)
                {
                    AxisHandler.StopMove();
                }
            }
        }

        public bool SettingSequence()
        {
            bool lampError = false;
            if (AxisHandler != null)
            {
                SystemConfig systemConfig = SystemConfig.Instance;

                // 포지션 일치
                //float positionOffset = systemConfig.PositionOffset * 1000;
                float scanReferencePos = systemConfig.ReferencePos.GetPosition()[0];
                float scanBackGroundPos = systemConfig.BackGroundPos.GetPosition()[0];

                //scanReferencePos = positionOffset - scanReferencePos;
                //scanBackGroundPos = positionOffset - scanBackGroundPos;

                SetHomeSpeed();

                SystemState.Instance().SetInspectState(InspectState.Run);

                AxisHandler.HomeMove(CancellationTokenSource.Token);

                //Task task = MessageWindowHelper.ShowProgress(null, "Calibration...", new Action(() =>
                //{
                //    AxisHandler.HomeMove(CancellationTokenSource.Token);

                //    Spectrometer.ScanStartStop(true);
                //    Thread.Sleep(500);
                //    AxisHandler.Move(0, scanReferencePos, CancellationTokenSource.Token);
                //    Thread.Sleep(500);
                //    lampError = CheckLampState();
                //    Spectrometer.SaveRefSpectrum(modelPath);
                //    AxisHandler.Move(0, scanBackGroundPos, CancellationTokenSource.Token);
                //    Thread.Sleep(500);
                //    Spectrometer.SaveBGSpectrum(modelPath);

                //    Spectrometer.ScanStartStop(false);
                //}));

                SystemState.Instance().SetIdle();
            }

            if (lampError == true)
            {
                Task task = MessageWindowHelper.ShowMessageBox(null, "Lamp Error!! Please Check the Lamp", System.Windows.MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        private bool CheckLampState()
        {
            int specAvgCount = 5;
            int lampAvgCount = 5;
            bool isLampOK = true;

            SpectrometerProperty property = SystemConfig.Instance.SpectrometerProperty;

            var averageCountList = new Dictionary<string, int>();
            var lampPitchList = new Dictionary<string, int>();

            var graphRawData = new Dictionary<string, double[]>();
            var maxValueList = new Dictionary<string, double>();
            var sumValueList = new Dictionary<string, double>();

            foreach (DynMvp.Devices.Spectrometer.SpectrometerInfo info in Spectrometer.DeviceList.Values)
            {
                averageCountList.Add(info.Name, Spectrometer.Wrapper.getScansToAverage(info.Index));
                lampPitchList.Add(info.Name, property.LampPitch[info.Name]);
                maxValueList.Add(info.Name, float.MinValue);
                sumValueList.Add(info.Name, float.MinValue);
                Spectrometer.Wrapper.setScansToAverage(info.Index, specAvgCount);
            }
            Thread.Sleep(500);

            for (int k = 0; k < lampAvgCount; k++)
            {
                graphRawData = Spectrometer.GetNewSpectrum();

                foreach (DynMvp.Devices.Spectrometer.SpectrometerInfo info in Spectrometer.DeviceList.Values)
                {
                    maxValueList[info.Name] = float.MinValue;
                    for (int i = 0; i < graphRawData[info.Name].Count(); i++)
                    {
                        maxValueList[info.Name] = Math.Max(maxValueList[info.Name], graphRawData[info.Name][i]);
                    }

                    sumValueList[info.Name] += maxValueList[info.Name];
                }
            }

            foreach (DynMvp.Devices.Spectrometer.SpectrometerInfo info in Spectrometer.DeviceList.Values)
            {
                Spectrometer.Wrapper.setScansToAverage(info.Index, averageCountList[info.Name]);

                maxValueList[info.Name] = sumValueList[info.Name] / lampAvgCount;
                isLampOK = isLampOK && (maxValueList[info.Name] > lampPitchList[info.Name]);
            }

            return isLampOK;
        }

        public void SetHomeSpeed(HomeParam homeParam = null)
        {
            double homeEndSpeed = 0;
            double homeStartSpeed = 0;
            if (homeParam != null)
            {
                homeEndSpeed = homeParam.FineSpeed.MaxVelocity;
                homeStartSpeed = homeParam.HighSpeed.MaxVelocity;
            }
            else
            {
                SystemConfig systemConfig = SystemConfig.Instance;
                homeEndSpeed = systemConfig.HomeEndSpeed;
                homeStartSpeed = systemConfig.HomeStartSpeed;
            }
            AxisHandler.GetUniqueAxis("X").AxisParam.HomeSpeed.FineSpeed.MaxVelocity = homeEndSpeed * 1000f;
            AxisHandler.GetUniqueAxis("X").AxisParam.HomeSpeed.HighSpeed.MaxVelocity = homeStartSpeed * 1000f;
        }

        public void SetMoveSpeed(MovingParam moveParam = null)
        {
            double moveSpeed = 0;
            if (moveParam != null)
            {
                moveSpeed = moveParam.MaxVelocity;
            }
            else
            {
                SystemConfig systemConfig = SystemConfig.Instance;
                moveSpeed = systemConfig.MovingSpeed;
            }
            AxisHandler.GetUniqueAxis("X").AxisParam.MovingParam.MaxVelocity = moveSpeed * 1000f;
        }

        public void SetJogSpeed(MovingParam jogParam = null)
        {
            double jogSpeed = 0;
            if (jogParam != null)
            {
                jogSpeed = jogParam.MaxVelocity;
            }
            else
            {
                SystemConfig systemConfig = SystemConfig.Instance;
                jogSpeed = systemConfig.JogSpeed;
            }
            AxisHandler.GetUniqueAxis("X").AxisParam.JogParam.MaxVelocity = jogSpeed * 1000f;
        }
        #endregion
    }
}
