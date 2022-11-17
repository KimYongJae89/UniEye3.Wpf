using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoAjinSlave : DigitalIoAjin
    {
        public DigitalIoAjinSlave(string name) : base(name)
        {
            digitalIoType = DigitalIoType.Ajin_Slave;
        }
    }

    public class DigitalIoAjinMaster : DigitalIoAjin
    {
        public DigitalIoAjinMaster(string name) : base(name)
        {
            digitalIoType = DigitalIoType.Ajin_Master;
        }
    }

    public abstract class DigitalIoAjin : DigitalIo
    {
        private bool exist = false;
        private bool initialized = false;

        public DigitalIoAjin(string name) : base(DigitalIoType.Ajin_Slave, name)
        {
            digitalIoType = DigitalIoType.Ajin_Master;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            uint upStatus = 0;
            uint uResult;

            int result = CAXL.AxlIsOpened();
            if (result == 0)
            {
                uResult = CAXL.AxlOpen(7);
            }

            uResult = CAXD.AxdInfoIsDIOModule(ref upStatus);
            if (uResult == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                exist = (upStatus == (uint)AXT_EXISTENCE.STATUS_EXIST);
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "Fail to find digital I/O.", ((AXT_FUNC_RESULT)uResult).ToString());
                return false;
            }


            NumInPort = digitalIoInfo.NumInPort;
            NumOutPort = digitalIoInfo.NumOutPort;

            NumInPortGroup = digitalIoInfo.NumInPortGroup;
            InPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
            NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
            OutPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

            initialized = true;

            return true;

        }

        public override bool IsReady()
        {
            return initialized;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            uint value = 0;

            uint duRetCode = CAXD.AxdiReadInportDword(groupNo + InPortStartGroupIndex, 0, ref value);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToReadValue,
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToReadValue.ToString(), string.Format("Read Inport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
            }

            return value;
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            uint value = 0;


            uint duRetCode = CAXD.AxdoReadOutportDword(groupNo + OutPortStartGroupIndex, 0, ref value);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToReadValue,
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToReadValue.ToString(), string.Format("Read Outport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
            }


            return value;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            //do nothing

            //throw new NotImplementedException();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            uint duRetCode = CAXD.AxdoWriteOutportDword(groupNo + OutPortStartGroupIndex, 0, outputPortStatus);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToWriteValue,
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToWriteValue.ToString(), string.Format("Write Outport Error - Code : {0}", ((AXT_FUNC_RESULT)duRetCode).ToString()));
                return;
            }
        }

        public override void WriteOutputPort(int groupNo, int portNo, bool value)
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
