using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace DynMvp.Devices.MotionController
{
    public class AjinMotionInfo : MotionInfo
    {
        public string ParamFile { get; set; }

        public override void LoadXml(XmlElement motionElement)
        {
            base.LoadXml(motionElement);

            ParamFile = XmlHelper.GetValue(motionElement, "ParamFile", "");
        }

        public override void SaveXml(XmlElement motionElement)
        {
            base.SaveXml(motionElement);

            XmlHelper.SetValue(motionElement, "ParamFile", ParamFile);
        }

        public override MotionInfo Clone()
        {
            var ajinMotionInfo = new AjinMotionInfo();
            ajinMotionInfo.Copy(this);

            return ajinMotionInfo;
        }

        public override void Copy(MotionInfo srcMotionInfo)
        {
            base.Copy(srcMotionInfo);

            var ajinMotionInfo = (AjinMotionInfo)srcMotionInfo;
            ParamFile = ajinMotionInfo.ParamFile;
        }
    }

    internal class MotionAjin : Motion, IDigitalIo
    {
        private bool dioExist = false;
        private uint numDigitalInput = 0;
        private uint numDigitalOutput = 0;
        private int numInPortGroup;
        private int inPortStartGroupIndex;
        private int numOutPortGroup;
        private int outPortStartGroupIndex;
        private object commandLock = new object();

        public MotionAjin(string name)
            : base(MotionType.Ajin, name)
        {
        }

        public string GetName() { return Name; }
        public int GetNumInPort() { return (int)numDigitalInput; }
        public int GetNumOutPort() { return (int)numDigitalOutput; }

        public int GetNumInPortGroup() { return numInPortGroup; }
        public int GetNumOutPortGroup() { return numOutPortGroup; }
        public int GetInPortStartGroupIndex() { return inPortStartGroupIndex; }
        public int GetOutPortStartGroupIndex() { return outPortStartGroupIndex; }

        public bool IsVirtual => false;

        public bool IsInitialized()
        {
            return IsReady();
        }

        public override bool Initialize(MotionInfo motionInfo)
        {
            if (IsReady() == false)
            {
                var ajinMotionInfo = (AjinMotionInfo)motionInfo;

                uint result = CAXL.AxlOpen(7);
                if (result != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "Can't initilaize Ajin Library.", ((AXT_FUNC_RESULT)result).ToString());
                    return false;
                }

                if (InitMotion(ajinMotionInfo) == false)
                {
                    return false;
                }

                UpdateState(DeviceState.Ready, "Device Loaded");
            }

            return true;
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            uint upStatus = 0;
            uint result = CAXD.AxdInfoIsDIOModule(ref upStatus);
            if (result == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                dioExist = (upStatus == (uint)AXT_EXISTENCE.STATUS_EXIST);
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "Fail to find digital I/O.", ((AXT_FUNC_RESULT)result).ToString());
                return false;
            }

            numDigitalInput = (uint)digitalIoInfo.NumInPort;
            numDigitalOutput = (uint)digitalIoInfo.NumOutPort;

            numInPortGroup = digitalIoInfo.NumInPortGroup;
            inPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
            numOutPortGroup = digitalIoInfo.NumOutPortGroup;
            outPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

            return true;
        }

        private bool InitMotion(AjinMotionInfo ajinMotionInfo)
        {
            uint upStatus = 0;
            uint result = CAXM.AxmInfoIsMotionModule(ref upStatus);
            if (result == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if (upStatus != (uint)AXT_EXISTENCE.STATUS_EXIST)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "Can't find motion module.");
                    return false;
                }
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "Fail to find motion module.", ((AXT_FUNC_RESULT)result).ToString());
                return false;
            }

            if (File.Exists(ajinMotionInfo.ParamFile))
            {
                result = CAXM.AxmMotLoadParaAll(ajinMotionInfo.ParamFile);
                if (result != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParamFile,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParamFile.ToString(), "Fail to Load Mot File.", ((AXT_FUNC_RESULT)result).ToString());
                    return false;
                }
            }

            int numAxis = 0;
            result = CAXM.AxmInfoGetAxisCount(ref numAxis);
            if (result != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), "Fail to Read Number of Axis .", ((AXT_FUNC_RESULT)result).ToString());
                return false;
            }

            NumAxis = numAxis;

            for (int i = 0; i < numAxis; i++)
            {
                CAXM.AxmMotSetAccelUnit(i, 1);
            }

            return true;
        }

        void IDigitalIo.UpdateState(DeviceState state, string stateMessage)
        {
            State = state;
            StateMessage = stateMessage;
        }

        public override void Release()
        {
            base.Release();

            if (CAXL.AxlClose() == 0)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToRelease,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToRelease.ToString(), "Fail to release Ajin library.");
                return;
            }

            UpdateState(DeviceState.Idle, "Device unloaded");
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            lock (commandLock)
            {
                CAXM.AxmSignalServoOn(axisNo, Convert.ToUInt32(bOnOff));
            }
        }

        public override float GetCommandPos(int axisNo)
        {
            double dCmdPos = 0.0;
            lock (commandLock)
            {
                CAXM.AxmStatusGetCmdPos(axisNo, ref dCmdPos);
            }

            return Convert.ToSingle(dCmdPos);
        }

        public override float GetActualPos(int axisNo)
        {
            double dCmdPos = 0.0;
            lock (commandLock)
            {
                CAXM.AxmStatusGetActPos(axisNo, ref dCmdPos);
            }

            return Convert.ToSingle(dCmdPos);
        }

        public override void SetPosition(int axisNo, float position)
        {
            lock (commandLock)
            {
                CAXM.AxmStatusSetPosMatch(axisNo, position);
            }
        }

        private int ConvertDirection(MoveDirection direction)
        {
            return Convert.ToInt32(direction);
        }

        private uint ConvertHomeSignal(HomeMode homeMode)
        {
            switch (homeMode)
            {
                case HomeMode.HomeSensor:
                    return (uint)AXT_MOTION_HOME_DETECT.HomeSensor;
                case HomeMode.NegEndLimit:
                    return (uint)AXT_MOTION_HOME_DETECT.NegEndLimit;
                case HomeMode.PosEndLimit:
                    return (uint)AXT_MOTION_HOME_DETECT.PosEndLimit;
            }

            return (uint)AXT_MOTION_HOME_DETECT.HomeSensor;
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeParam)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("HomeMove : Axis Id {0}", axisNo));

            uint duRetCode;
            lock (commandLock)
            {
                duRetCode = CAXM.AxmHomeSetMethod(axisNo, ConvertDirection(homeParam.HomeDirection),
                    ConvertHomeSignal(homeParam.HomeMode), 0, 1000, 0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), string.Format("Axis Id {0} / Start Home Move Set HomeMethod Error - Code : {1}", axisNo, duRetCode));
                    return false;
                }

                duRetCode = CAXM.AxmHomeSetVel(axisNo, homeParam.HighSpeed.MaxVelocity, homeParam.HighSpeed.MaxVelocity / 2, homeParam.MediumSpeed.MaxVelocity,
                            homeParam.FineSpeed.MaxVelocity, homeParam.HighSpeed.AccelerationTimeMs / 1000, homeParam.HighSpeed.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), string.Format("Axis Id {0} / Start Home Move Set Velocity Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }

                duRetCode = CAXM.AxmHomeSetStart(axisNo);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Homing,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Homing.ToString(), string.Format("Axis Id {0} / Start Home Move Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return true;
        }

        private bool SetProfileMode(int axisNo)
        {
            uint velocityProfile = 0;
            if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
            {
                velocityProfile = 0;
            }
            else
            {
                velocityProfile = 3;
            }

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMotSetProfileMode(axisNo, velocityProfile);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), string.Format("Axis Id {0} / Set Velocity Profile Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("StartMove : Axis Id {0} / Position {1} ", axisNo, position));

            if (IsMoveDone(axisNo) == false)
            {
                LogHelper.Debug(LoggerType.Motion, "StartMove : Moving Overlapped");
                return false;
            }

            SetProfileMode(axisNo);

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMotSetAbsRelMode(axisNo, 0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), string.Format("Axis Id {0} / StartMove Set ABS/Rel Mode Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }

                duRetCode = CAXM.AxmMoveStartPos(axisNo, position, movingParam.MaxVelocity, movingParam.AccelerationTimeMs / 1000, movingParam.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), string.Format("Axis Id {0} / StartMove Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("StartRelativeMove : Axis Id {0} / Position {1} ", axisNo, position));

            SetProfileMode(axisNo);

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMotSetAbsRelMode(axisNo, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), string.Format("Axis Id {0} /StartRelativeMove Set ABS/Rel Mode Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }

                duRetCode = CAXM.AxmMoveStartPos(axisNo, position, movingParam.MaxVelocity, movingParam.AccelerationTimeMs / 1000, movingParam.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), string.Format("Axis Id {0} / StartRelativeMove Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("ContinuousMove : Axis Id {0}", axisNo));

            SetProfileMode(axisNo);

            lock (commandLock)
            {
                double velocity = movingParam.MaxVelocity;
                if (negative == true)
                {
                    velocity *= -1;
                }

                uint duRetCode = CAXM.AxmMoveVel(axisNo, velocity, movingParam.AccelerationTimeMs / 1000, movingParam.DecelerationTimeMs / 1000);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.ContinuousMoving,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.ContinuousMoving.ToString(), string.Format("Axis Id {0} / ContinuousMove Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return true;
        }

        public override void StopMove(int axisNo)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMoveSStop(axisNo);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.StopMove,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.StopMove.ToString(), string.Format("Axis Id {0} / Stop Move Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                }
            }
        }

        public override void EmergencyStop(int axisNo)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmMoveEStop(axisNo);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.EmergencyStop,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.EmergencyStop.ToString(), string.Format("Axis Id {0} / Emergency Stop Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                }
            }
        }

        public override bool IsMoveDone(int axisNo)
        {
            uint upStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmStatusReadInMotion(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Motion Done Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return true;
                }
            }

            return upStatus == 0;
        }

        public override bool IsServoOn(int axisNo)
        {
            uint upStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalIsServoOn(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Servo On Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                }
            }

            return upStatus == 1;
        }

        public override bool IsAmpFault(int axisNo)
        {
            uint upStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalReadServoAlarm(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Servo Alarm Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                }
            }

            return upStatus == 1;
        }

        public override bool IsHomeOn(int axisNo)
        {
            uint upStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmHomeReadSignal(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Home On Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                }
            }

            return upStatus != 0;
        }

        public override bool IsHomeDone(int axisNo)
        {
            uint upStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmHomeReadSignal(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Home On Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                }
            }

            return upStatus != 0;
        }

        public override bool IsPositiveOn(int axisNo)
        {
            uint upStopMode = 0;
            uint upPositiveStatus = 0;
            uint upNegativeStatus = 0;

            uint duRetCode = 0;
            lock (commandLock)
            {
                duRetCode = CAXM.AxmSignalGetLimit(axisNo, ref upStopMode, ref upPositiveStatus, ref upNegativeStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Get Positive Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            if (upPositiveStatus == 0x2)
            {
                return false;
            }

            lock (commandLock)
            {
                duRetCode = CAXM.AxmSignalReadLimit(axisNo, ref upPositiveStatus, ref upNegativeStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Positive Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return upPositiveStatus != 0;
        }

        public override bool IsNegativeOn(int axisNo)
        {
            uint upStopMode = 0;
            uint upPositiveStatus = 0;
            uint upNegativeStatus = 0;

            uint duRetCode;
            lock (commandLock)
            {
                duRetCode = CAXM.AxmSignalGetLimit(axisNo, ref upStopMode, ref upPositiveStatus, ref upNegativeStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Get Negative Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            if (upNegativeStatus == 0x2)
            {
                return false;
            }

            lock (commandLock)
            {
                duRetCode = CAXM.AxmSignalReadLimit(axisNo, ref upPositiveStatus, ref upNegativeStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Negative Limit Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return false;
                }
            }

            return upNegativeStatus != 0;
        }

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            var motionStatus = new MotionStatus();

            uint upStatus = 0;

            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmStatusReadMechanical(axisNo, ref upStatus);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Read Mechenical State Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    return motionStatus;
                }
            }

            motionStatus.posLimit = IsPositiveOn(axisNo);
            motionStatus.negLimit = IsNegativeOn(axisNo);
            motionStatus.alarm = (upStatus & 0x0010) > 0;
            motionStatus.inp = IsMoveDone(axisNo);
            motionStatus.emg = (upStatus & 0x0040) > 0;
            motionStatus.origin = (upStatus & 0x0080) > 0;
            motionStatus.run = IsMoveDone(axisNo) == false;
            motionStatus.servoOn = IsServoOn(axisNo);

            return motionStatus;
        }

        public override bool ResetAlarm(int axisNo)
        {
            lock (commandLock)
            {
                uint duRetCode = CAXM.AxmSignalServoAlarmReset(axisNo, 1);
                uint dwOutBit = 0;
                CAXM.AxmSignalReadOutputBit(axisNo, 1, ref dwOutBit);
                if (dwOutBit != 0)
                {
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
                }
                else
                {
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.TRUE);
                }

                Thread.Sleep(500);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToReadParam,
                        ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToReadParam.ToString(), string.Format("Axis Id {0} / Alarm Reset Error - Code : {1}", axisNo, ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
                    return false;
                }


            }
            CAXM.AxmSignalWriteOutputBit(axisNo, 1, (uint)AXT_BOOLEAN.FALSE);
            return true;
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            throw new NotImplementedException();
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdoWriteOutport(groupNo * 32 + portNo, Convert.ToUInt32(value));
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToWriteValue,
                            ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToWriteValue.ToString(), string.Format("Write Outport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
                        return;
                    }

                    Thread.Sleep(5);
                }
            }
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdoWriteOutportDword(groupNo + outPortStartGroupIndex, 0, outputPortStatus);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToWriteValue,
                            ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToWriteValue.ToString(), string.Format("Write Outport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
                        return;
                    }
                }
            }
        }

        public uint ReadOutputGroup(int groupNo)
        {
            uint value = 0;

            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdoReadOutportDword(groupNo + outPortStartGroupIndex, 0, ref value);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToReadValue,
                            ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToReadValue.ToString(), string.Format("Read Outport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    }
                }
            }

            return value;
        }

        public uint ReadInputGroup(int groupNo)
        {
            uint value = 0;

            if (dioExist == true)
            {
                lock (commandLock)
                {
                    uint duRetCode = CAXD.AxdiReadInportDword(groupNo + inPortStartGroupIndex, 0, ref value);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToReadValue,
                            ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToReadValue.ToString(), string.Format("Read Inport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
                    }
                }
            }

            return value;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            //do nothing
        }

        public override bool IsEmgStop(int axisNo)
        {
            throw new NotImplementedException();
        }
    }
}
