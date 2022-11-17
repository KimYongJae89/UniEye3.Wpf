using DynMvp.Base;
using DynMvp.Devices.Dio;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    internal class MotionAlphaMotion302 : Motion, IDigitalIo
    {
        private static int loadCount = 0;
        private ushort boardNo = 0;

        private enum LoadDeviceErrorCode
        {
            DeviceLoad,
        }

        private uint numDigitalInput;
        private uint numDigitalOutput;

        public MotionAlphaMotion302(string name)
            : base(MotionType.AlphaMotion302, name)
        {
        }

        public string GetName() { return Name; }
        public int GetNumInPort() { return (int)numDigitalInput; }
        public int GetNumOutPort() { return (int)numDigitalOutput; }
        public int GetNumInPortGroup() { return 1; }
        public int GetNumOutPortGroup() { return 1; }
        public int GetInPortStartGroupIndex() { return 0; }
        public int GetOutPortStartGroupIndex() { return 0; }

        public bool IsInitialized()
        {
            return IsReady();
        }

        public bool IsVirtual => false;

        public override bool Initialize(MotionInfo motionInfo)
        {
            if (IsReady() == false)
            {
                int result = TMCAADLL.TMC302A_LoadDevice();
                if (result < 0)
                {
                    string errorMsg;
                    switch (result)
                    {
                        default:
                        case tmcDef.ERR_DEVICE_LOAD:
                            errorMsg = "Fail to Load Device Driver";
                            break;
                        case tmcDef.ERR_DEVICE_EXIST:
                            errorMsg = "Device number is exist";
                            break;
                        case tmcDef.ERR_DEVICE_PCI_BUS:
                            errorMsg = "PCI Bus line error";
                            break;
                        case tmcDef.ERR_UPBOARD_LOAD:
                            errorMsg = "UP board error";
                            break;
                    }

                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                        ErrorLevel.Fatal, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), string.Format("[TMC 302] {0}", errorMsg));

                    UpdateState(DeviceState.Error, "Can't find alpha motion 302 device.");
                    return false;
                }
                else
                {
                    loadCount++;
                    boardNo = (ushort)result;
                }

                TMCAADLL.TMC302A_LogCheck(3);

                var pciMotionInfo = (PciMotionInfo)motionInfo;

                boardNo = (ushort)pciMotionInfo.Index;

                NumAxis = TMCAADLL.TMC302A_GetAxisNum(boardNo);
                numDigitalInput = TMCAADLL.TMC302A_GetDiNum(boardNo);
                numDigitalOutput = TMCAADLL.TMC302A_GetDoNum(boardNo);

                UpdateState(DeviceState.Ready, "Device Loaded");
            }

            return true;
        }

        public bool CheckError(int errorType, string errorStr, ErrorLevel errorLevel)
        {
            int errCode = TMCAADLL.TMC302A_GetErrorCode();
            if (errCode != tmcDef.ERR_SUCCESS)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, errorType, errorLevel, ErrorSection.Motion.ToString(), errorStr, TMCAADLL.TMC302A_GetErrorString(errCode));
                return true;
            }

            return false;
        }

        public override void Release()
        {
            base.Release();

            loadCount--;

            if (loadCount == 0)
            {
                TMCAADLL.TMC302A_UnloadDevice();
            }

            UpdateState(DeviceState.Idle, "Device unloaded");
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            // True일 때 Servo Off????
            TMCAADLL.TMC302A_PutSvOn(boardNo, (ushort)axisNo, Convert.ToUInt16(bOnOff));
        }

        public override float GetCommandPos(int axisNo)
        {
            return TMCAADLL.TMC302A_GetCommandPos(boardNo, (ushort)axisNo);
        }

        public override float GetActualPos(int axisNo)
        {
            return TMCAADLL.TMC302A_GetActualPos(boardNo, (ushort)axisNo);
        }

        public override void SetPosition(int axisNo, float position)
        {
            TMCAADLL.TMC302A_SetActualPos(boardNo, (ushort)axisNo, (int)position);
            TMCAADLL.TMC302A_SetCommandPos(boardNo, (ushort)axisNo, (int)position);
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeSpeed)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("HomeMove : Axis Id {0}", axisNo));

            TMCAADLL.TMC302A_SetHomeSpeed(boardNo, (ushort)axisNo, (uint)(homeSpeed.HighSpeed.StartVelocity + 1),
                (uint)(Math.Abs(homeSpeed.HighSpeed.MaxVelocity) + 1), (uint)(homeSpeed.HighSpeed.AccelerationTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_SetHomeDir(boardNo, (ushort)axisNo, 1);
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_SetHomeMode(boardNo, (ushort)axisNo, 2);
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_Home_Move(boardNo, (ushort)axisNo);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("StartMove : Axis Id {0} / Position {1} ", axisNo, position));

            if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
            {
                TMCAADLL.TMC302A_SetSpeedMode(boardNo, (ushort)axisNo, 0);
            }
            else
            {
                TMCAADLL.TMC302A_SetSpeedMode(boardNo, (ushort)axisNo, 1);
            }

            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_SetPosSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(movingParam.MaxVelocity + 1),
                    (uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs), (uint)(movingParam.DecelerationTimeMs + movingParam.SCurveTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_Abs_Move(boardNo, (ushort)axisNo, (int)position);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("StartRelativeMove : Axis Id {0} / Position {1} ", axisNo, position));

            if (AxisHandler.MovingProfileType == MovingProfileType.TCurve)
            {
                TMCAADLL.TMC302A_SetSpeedMode(boardNo, (ushort)axisNo, 0);
            }
            else
            {
                TMCAADLL.TMC302A_SetSpeedMode(boardNo, (ushort)axisNo, 1);
            }

            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_SetPosSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(movingParam.MaxVelocity + 1),
                    (uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs), (uint)(movingParam.DecelerationTimeMs + movingParam.SCurveTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            TMCAADLL.TMC302A_Inc_Move(boardNo, (ushort)axisNo, (int)position);
            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            LogHelper.Debug(LoggerType.Motion, string.Format("ContinuousMove : Axis Id {0}", axisNo));

            TMCAADLL.TMC302A_SetJogSpeed(boardNo, (ushort)axisNo, (uint)(movingParam.StartVelocity + 1), (uint)(Math.Abs(movingParam.MaxVelocity) + 1),
                    (uint)(movingParam.AccelerationTimeMs + movingParam.SCurveTimeMs));
            if (CheckError((int)CommonError.FailToWriteParam, CommonError.FailToWriteParam.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            if (movingParam.MaxVelocity < 0)
            {
                TMCAADLL.TMC302A_Jog_Move(boardNo, (ushort)axisNo, 0);
            }
            else
            {
                TMCAADLL.TMC302A_Jog_Move(boardNo, (ushort)axisNo, 1);
            }

            if (CheckError((int)MotionError.Moving, MotionError.Moving.ToString(), ErrorLevel.Error) == true)
            {
                return false;
            }

            return true;
        }

        public override void StopMove(int axisNo)
        {
            TMCAADLL.TMC302A_Sudden_Stop(boardNo, (ushort)axisNo);
        }

        public override void EmergencyStop(int axisNo)
        {
            TMCAADLL.TMC302A_Sudden_Stop(boardNo, (ushort)axisNo);
        }

        public override bool IsMoveDone(int axisNo)
        {
            return TMCAADLL.TMC302A_Done(boardNo, (ushort)axisNo) == 0;
        }

        public override bool IsAmpFault(int axisNo)
        {
            ushort stGetIO = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            return (stGetIO & 0x0010) > 0;
        }

        public override bool IsHomeOn(int axisNo)
        {
            ushort stGetIO = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            return (stGetIO & 0x0001) > 0;
        }

        public override bool IsHomeDone(int axisNo)
        {
            ushort stGetIO = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            return (stGetIO & 0x0001) > 0;
        }

        public override bool IsPositiveOn(int axisNo)
        {
            ushort stGetIO = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            return (stGetIO & 0x0020) > 0;
        }

        public override bool IsNegativeOn(int axisNo)
        {
            ushort stGetIO = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            return (stGetIO & 0x0040) > 0;
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            Debug.Assert(groupNo == 0, "Alpha Motion 302 has only 32 output port.");

            TMCAADLL.TMC302A_PutDODWord(boardNo, 0, outputPortStatus);
        }

        public uint ReadOutputGroup(int groupNo)
        {
            Debug.Assert(groupNo == 0, "Alpha Motion 302 has only 32 output port.");

            uint value = 0;
            TMCAADLL.TMC302A_GetDODWord(boardNo, 0, ref value);

            return value;
        }

        public uint ReadInputGroup(int groupNo)
        {
            Debug.Assert(groupNo == 0, "Alpha Motion 302 has only 32 input port.");

            uint value = 0;
            TMCAADLL.TMC302A_GetDIDWord(boardNo, 0, ref value);

            return value;
        }

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            var motionStatus = new MotionStatus();

            ushort axisStatus = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            motionStatus.origin = (axisStatus & 0x0001) > 0;
            motionStatus.inp = (axisStatus & 0x0008) > 0;
            motionStatus.run = (axisStatus & 0x0100) > 0;
            motionStatus.posLimit = (axisStatus & 0x0020) > 0;
            motionStatus.negLimit = (axisStatus & 0x0040) > 0;
            motionStatus.servoOn = (axisStatus & 0x4000) > 0;
            motionStatus.emg = (axisStatus & 0x0004) > 0;
            motionStatus.alarm = (axisStatus & 0x0010) > 0;

            return motionStatus;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override bool IsServoOn(int axisNo)
        {
            ushort resopnse = TMCACDLL.TMC304A_GetSvOn(boardNo, (ushort)axisNo);
            if (resopnse == 1)
            {
                return true;
            }

            return false;
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public override bool ResetAlarm(int axisNo)
        {
            throw new NotImplementedException();
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            throw new NotImplementedException();
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }

        public override bool IsEmgStop(int axisNo)
        {
            ushort stGetIO = TMCAADLL.TMC302A_GetCardStatus(boardNo, (ushort)axisNo);

            return (stGetIO & 0x0004) > 0;
        }
    }
}
