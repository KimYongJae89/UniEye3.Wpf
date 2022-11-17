using DynMvp.Base;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    internal class DigitalIoTmcAexxx : DigitalIo
    {
        private bool initialized = true;
        private ushort cardNo = 0;

        public DigitalIoTmcAexxx(string name)
            : base(DigitalIoType.TmcAexxx, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            try
            {
                var pciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;

                int retVal = TMCAEDLL.AIO_LoadDevice();

                if (retVal < 0)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                        ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Can't load TMC AExxx Device. ( Type = {0} )", digitalIoType.ToString()));
                    return false;
                }

                cardNo = (ushort)pciDigitalIoInfo.Index;

                uint uiModel = 0;
                uint uiComm = 0;
                uint uiDiNum = 0;
                uint uiDoNum = 0;

                TMCAEDLL.AIO_BoardInfo(cardNo, ref uiModel, ref uiComm, ref uiDiNum, ref uiDoNum);

                NumInPort = (int)uiDiNum;
                NumOutPort = (int)uiDoNum;
                NumInPortGroup = pciDigitalIoInfo.NumInPortGroup;
                NumOutPortGroup = pciDigitalIoInfo.NumOutPortGroup;

                initialized = true;

                return true;

            }
            catch (Exception ex)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("TMC DIO Device initalization is failed. ( Type = {0} ) : {1}", digitalIoType.ToString(), ex.Message));
            }

            return false;
        }

        public override bool IsReady()
        {
            return initialized;
        }

        public override void Release()
        {
            base.Release();

            try
            {
                if (IsReady())
                {
                    TMCAEDLL.AIO_UnloadDevice();
                }
            }
            catch
            {
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            TMCAEDLL.AIO_PutDODWord(cardNo, 0, outputPortStatus);
            byte[] bytes = BitConverter.GetBytes(outputPortStatus);
            LogHelper.Debug(LoggerType.Device, string.Format("IO WritetputGroup : {0}", BitConverter.ToString(bytes)));
        }

        public override void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            ushort bit = (ushort)(value ? 1 : 0);
            TMCAEDLL.AIO_PutDOBit(cardNo, (ushort)portNo, bit);
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            uint outputPortStatus = 0;
            TMCAEDLL.AIO_GetDODWord(cardNo, 0, ref outputPortStatus);

            return outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            uint inputPortStatus = 0;
            TMCAEDLL.AIO_GetDIDWord(cardNo, 0, ref inputPortStatus);

            return inputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
