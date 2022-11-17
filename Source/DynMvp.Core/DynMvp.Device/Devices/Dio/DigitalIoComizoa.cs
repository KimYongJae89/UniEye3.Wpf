using DynMvp.Base;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    internal class DigitalIoComizoa : DigitalIo
    {
        private bool initialized = true;

        public DigitalIoComizoa(DigitalIoType type, string name)
            : base(type, name)
        {
            switch (type)
            {
                case DigitalIoType.ComizoaSd424f:
                    NumInPort = NumOutPort = 16;
                    break;
            }
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            try
            {
                int nNumAxes = 0;
                switch (digitalIoType)
                {
                    case DigitalIoType.ComizoaSd424f:
                        if (CMDLL.cmmGnDeviceLoad((int)Defines._TCmBool.cmTRUE, ref nNumAxes) != Defines.cmERR_NONE)
                        {
                            ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                                ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Comizoa Device registeration is failed. ( Type = {0} )", digitalIoType.ToString()));
                            return false;
                        }

                        NumInPort = NumOutPort = 16;
                        initialized = true;

                        return true;
                    default:
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.InvalidType,
                            ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.InvalidType.ToString(), string.Format("Invalid Digital IO Type. {0}", digitalIoType.ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Comizoa Device initalization is failed. ( Type = {0} ) : {1}", digitalIoType.ToString(), ex.Message));
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
                    CMDLL.cmmGnDeviceUnload();
                }
            }
            catch
            {
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            CMDLL.cmmDoPutMulti(0, 16, (int)outputPortStatus);
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            int outputPortStatus = 0;
            CMDLL.cmmDoGetMulti(0, 16, ref outputPortStatus);

            return (uint)outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            int inputPortStatus = 0;
            CMDLL.cmmDiGetMulti(0, 16, ref inputPortStatus);

            return (uint)inputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
