using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    internal enum EzServoParam : byte
    {
        PULSEPERREVOLUTION = 0,
        AXISMAXSPEED,
        AXISSTARTSPEED,
        AXISACCTIME,
        AXISDECTIME,

        SPEEDOVERRIDE,
        JOGHIGHSPEED,
        JOGLOWSPEED,
        JOGACCDECTIME,

        SERVOALARMLOGIC,
        SERVOONLOGIC,
        SERVORESETLOGIC,

        SWLMTPLUSVALUE,
        SWLMTMINUSVALUE,
        SOFTLMTSTOPMETHOD,
        HARDLMTSTOPMETHOD,
        LIMITSENSORLOGIC,

        ORGSPEED,
        ORGSEARCHSPEED,
        ORGACCDECTIME,
        ORGMETHOD,
        ORGDIR,
        ORGOFFSET,
        ORGPOSITIONSET,
        ORGSENSORLOGIC,

        POSITIONLOOPGAIN,
        INPOSITIONVALUE,
        POSTRACKINGLIMIT,
        MOTIONDIR,

        LIMITSENSORDIR,
        ORGTORQUERATIO,

        POSERROVERFLOWLIMIT,
        POSVALUECOUNTINGMETHOD,
    }

    internal enum FasAxisStatus : uint
    {
        ERRORALL = 0x00000001,
        HWPOSILMT = 0x00000002,
        HWNEGALMT = 0x00000004,
        SWPOGILMT = 0x00000008,
        SWNEGALMT = 0x00000010,
        RESERVED0 = 0x00000020,
        RESERVED1 = 0x00000040,
        ERRPOSOVERFLOW = 0x00000080,
        ERROVERCURRENT = 0x00000100,
        ERROVERSPEED = 0x00000200,
        ERRPOSTRACKING = 0x00000400,
        ERROVERLOAD = 0x00000800,
        ERROVERHEAT = 0x00001000,
        ERRBACKEMF = 0x00002000,
        ERRMOTORPOWER = 0x00004000,
        ERRINPOSITION = 0x00008000,
        EMGSTOP = 0x00010000,
        SLOWSTOP = 0x00020000,
        ORIGINRETURNING = 0x00040000,
        INPOSITION = 0x00080000,
        SERVOON = 0x00100000,
        ALARMRESET = 0x00200000,
        PTSTOPPED = 0x00400000,
        ORIGINSENSOR = 0x00800000,
        ZPULSE = 0x01000000,
        ORIGINRETOK = 0x02000000,
        MOTIONDIR = 0x04000000,
        MOTIONING = 0x08000000,
        MOTIONPAUSE = 0x10000000,
        MOTIONACCEL = 0x20000000,
        MOTIONDECEL = 0x40000000,
        MOTIONCONST = 0x80000000
    }

    internal class MotionEziMotionPlusR : Motion, IDigitalIo
    {
        private bool initialized = false;
        private uint numDigitalInput = 8;
        private uint numDigitalOutput = 8;
        private byte portNo;
        private SerialMotionInfo serialMotionInfo;

        public MotionEziMotionPlusR(string name)
            : base(MotionType.FastechEziMotionPlusR, name)
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
            return initialized;
        }

        public bool IsVirtual => false;

        public override bool Initialize(MotionInfo motionInfo)
        {
            serialMotionInfo = (SerialMotionInfo)motionInfo;
            SerialPortInfo serialPortInfo = serialMotionInfo.SerialPortInfo;

            if (FAS_EziMOTIONPlusR.FAS_Connect((byte)serialPortInfo.PortNo, (uint)serialPortInfo.BaudRate) != 0)
            {
                initialized = true;

                for (byte i = 0; i < FAS_EziMOTIONPlusR.MAX_SLAVE_NUMS; i++)
                {
                    if (FAS_EziMOTIONPlusR.FAS_IsSlaveExist((byte)serialPortInfo.PortNo, i) != 0)
                    {
                        NumAxis++;
                    }
                }

                portNo = (byte)serialPortInfo.PortNo;

                return true;
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                    ErrorLevel.Fatal, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "[FASTECH] Connection Failed.");
            }

            return false;
        }

        public override void Release()
        {
            base.Release();

            SerialPortInfo serialPortInfo = serialMotionInfo.SerialPortInfo;

            FAS_EziMOTIONPlusR.FAS_Close((byte)serialPortInfo.PortNo);
            initialized = false;
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public override bool CanSyncMotion()
        {
            return false;
        }

        private string GetErrorString(int value)
        {
            switch (value)
            {
                case FAS_EziMOTIONPlusR.FMM_INVALID_PORT_NUM:
                    return "Invalid Port Number";
                case FAS_EziMOTIONPlusR.FMM_INVALID_SLAVE_NUM:
                    return "Invalid Slave Number";
                case FAS_EziMOTIONPlusR.FMC_DISCONNECTED:
                    return "Disconnected";
                case FAS_EziMOTIONPlusR.FMC_TIMEOUT_ERROR:
                    return "Timeout";
                case FAS_EziMOTIONPlusR.FMC_CRCFAILED_ERROR:
                    return "CRC Failed";
                case FAS_EziMOTIONPlusR.FMC_RECVPACKET_ERROR:
                    return "Receive Packet Error";
                case FAS_EziMOTIONPlusR.FMM_POSTABLE_ERROR:
                    return "Postable Error";
                case FAS_EziMOTIONPlusR.FMP_FRAMETYPEERROR:
                    return "Frame Type Error";
                case FAS_EziMOTIONPlusR.FMP_DATAERROR:
                    return "Data Error";
                case FAS_EziMOTIONPlusR.FMP_PACKETERROR:
                    return "Packet Error";
                case FAS_EziMOTIONPlusR.FMP_RUNFAIL:
                    return "Run Failed";
                case FAS_EziMOTIONPlusR.FMP_RESETFAIL:
                    return "Reset Failed";
                case FAS_EziMOTIONPlusR.FMP_SERVOONFAIL1:
                    return "Servo On Failed(1)";
                case FAS_EziMOTIONPlusR.FMP_SERVOONFAIL2:
                    return "Servo On Failed(2)";
                case FAS_EziMOTIONPlusR.FMP_SERVOONFAIL3:
                    return "Servo On Failed(3)";
                case FAS_EziMOTIONPlusR.FMP_PACKETCRCERROR:
                    return "Packet CRC Error";
            }

            return "Unknown Error Code";
        }

        public override void TurnOnServo(int axisNo, bool bOnOff)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_ServoEnable(portNo, (byte)axisNo, (bOnOff ? 1 : 0));
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));
            }
        }

        public override float GetCommandPos(int axisNo)
        {
            int commandPos = 0;
            int retVal = FAS_EziMOTIONPlusR.FAS_GetCommandPos(portNo, (byte)axisNo, ref commandPos);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));
            }

            return commandPos;
        }

        public override float GetActualPos(int axisNo)
        {
            int actualPos = 0;
            int retVal = FAS_EziMOTIONPlusR.FAS_GetActualPos(portNo, (byte)axisNo, ref actualPos);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));
            }

            return actualPos;
        }

        public override void SetPosition(int axisNo, float position)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_ClearPosition(portNo, (byte)axisNo);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));
            }
        }

        public int GetOrgMethodValue(HomeMode homeMode)
        {
            if (homeMode == HomeMode.NegEndLimit || homeMode == HomeMode.PosEndLimit)
            {
                return 2;
            }
            else if (homeMode == HomeMode.ZPhase)
            {
                return 1;
            }

            return 0;
        }

        public override bool StartHomeMove(int axisNo, HomeParam homeParam)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGSPEED, (int)homeParam.HighSpeed.MaxVelocity);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGSEARCHSPEED, (int)homeParam.FineSpeed.MaxVelocity);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.HARDLMTSTOPMETHOD, 0);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGMETHOD, GetOrgMethodValue(homeParam.HomeMode));
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_SetParameter(portNo, (byte)axisNo, (byte)EzServoParam.ORGDIR, (homeParam.HomeDirection == MoveDirection.CW ? 0 : 1));
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToWriteParam,
                    ErrorLevel.Warning, ErrorSection.Motion.ToString(), CommonError.FailToWriteParam.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            retVal = FAS_EziMOTIONPlusR.FAS_MoveOriginSingleAxis(portNo, (byte)axisNo);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            return true;
        }

        public override bool StartMove(int axisNo, float position, MovingParam movingParam)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_MoveSingleAxisAbsPos(portNo, (byte)axisNo, (int)position, (uint)movingParam.MaxVelocity);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            return true;
        }

        public override bool StartRelativeMove(int axisNo, float position, MovingParam movingParam)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_MoveSingleAxisIncPos(portNo, (byte)axisNo, (int)position, (uint)movingParam.MaxVelocity);
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            return true;
        }

        public override bool ContinuousMove(int axisNo, MovingParam movingParam, bool negative)
        {
            int retVal = FAS_EziMOTIONPlusR.FAS_MoveVelocity(portNo, (byte)axisNo, (uint)movingParam.MaxVelocity, (negative ? 0 : 1));
            if (retVal != FAS_EziMOTIONPlusR.FMM_OK)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.Moving,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), MotionError.Moving.ToString(), "[FASTECH] " + GetErrorString(retVal));

                return false;
            }

            return true;
        }

        public override void StopMove(int axisNo)
        {
            FAS_EziMOTIONPlusR.FAS_MoveStop(portNo, (byte)axisNo);
        }

        public override void EmergencyStop(int axisNo)
        {
            FAS_EziMOTIONPlusR.FAS_EmergencyStop(portNo, (byte)axisNo);
        }

        public override bool IsMoveDone(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.MOTIONING) == 0);
        }

        public override bool IsAmpFault(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.ERRMOTORPOWER) != 0);
        }

        public override bool IsHomeOn(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.ORIGINSENSOR) != 0);
        }

        public override bool IsHomeDone(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.ORIGINRETOK) != 0);
        }

        public override bool IsPositiveOn(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.HWPOSILMT) != 0);
        }

        public override bool IsNegativeOn(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.HWNEGALMT) != 0);
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
        }

        public uint ReadOutputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public uint ReadInputGroup(int groupNo)
        {
            uint inputStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetIOInput(portNo, (byte)groupNo, ref inputStatus);

            return inputStatus;
        }

        public override MotionStatus GetMotionStatus(int axisNo)
        {
            var motionStatus = new MotionStatus();

            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            motionStatus.origin = (axisStatus & (int)FasAxisStatus.ORIGINSENSOR) != 0;
            motionStatus.inp = (axisStatus & (int)FasAxisStatus.INPOSITION) != 0;
            motionStatus.run = (axisStatus & (int)FasAxisStatus.MOTIONING) != 0;
            motionStatus.posLimit = (axisStatus & (int)FasAxisStatus.HWPOSILMT) != 0;
            motionStatus.negLimit = (axisStatus & (int)FasAxisStatus.HWNEGALMT) != 0;
            motionStatus.servoOn = (axisStatus & (int)FasAxisStatus.SERVOON) != 0;
            motionStatus.emg = (axisStatus & (int)FasAxisStatus.EMGSTOP) != 0;
            motionStatus.alarm = (axisStatus & (int)FasAxisStatus.ERRORALL) != 0;

            return motionStatus;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override bool IsServoOn(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.SERVOON) != 0);
        }

        public override bool ResetAlarm(int axisNo)
        {
            FAS_EziMOTIONPlusR.FAS_ServoAlarmReset(portNo, (byte)axisNo);
            FAS_EziMOTIONPlusR.FAS_StepAlarmReset(portNo, (byte)axisNo, 1);

            return true;
        }

        public override bool ResetAlarmOn(int axisNo, bool isOn)
        {
            FAS_EziMOTIONPlusR.FAS_ServoAlarmReset(portNo, (byte)axisNo);
            FAS_EziMOTIONPlusR.FAS_StepAlarmReset(portNo, (byte)axisNo, 1);

            return true;
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            uint ioSetMask = 0;
            uint ioClearMask = 0;
            if (value == true)
            {
                ioSetMask = (uint)(1 << portNo);
            }
            else
            {
                ioClearMask = (uint)(1 << portNo);
            }

            FAS_EziMOTIONPlusR.FAS_SetIOOutput(this.portNo, (byte)groupNo, ioSetMask, ioClearMask);
        }

        public override bool IsEmgStop(int axisNo)
        {
            uint axisStatus = 0;
            FAS_EziMOTIONPlusR.FAS_GetAxisStatus(portNo, (byte)axisNo, ref axisStatus);

            return ((axisStatus & (int)FasAxisStatus.EMGSTOP) != 0);
        }
    }
}
