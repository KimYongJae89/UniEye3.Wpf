using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    internal class DigitalIoDASK : DigitalIo
    {
        private short cardId;
        private uint inputPortStatus = 0;
        private uint outputPortStatus = 0;
        private static int numAdlink7230 = 0;
        private static int numAdlink7432 = 0;

        public DigitalIoDASK(DigitalIoType type, string name) : base(type, name)
        {
            switch (type)
            {
                case DigitalIoType.Adlink7230:
                    NumInPort = NumOutPort = 16;
                    break;
                case DigitalIoType.Adlink7432:
                    NumInPort = NumOutPort = 32;
                    break;
            }
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            try
            {
                switch (digitalIoType)
                {
                    case DigitalIoType.Adlink7230:
                        cardId = DASK.Register_Card(DASK.PCI_7230, (ushort)numAdlink7230);
                        NumInPort = NumOutPort = 16;
                        numAdlink7230++;
                        break;
                    case DigitalIoType.Adlink7432:
                        cardId = DASK.Register_Card(DASK.PCI_7432, (ushort)numAdlink7432);
                        NumInPort = NumOutPort = 32;
                        numAdlink7432++;
                        break;
                    default:
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.InvalidType,
                            ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.InvalidType.ToString(), string.Format("Invalid Digital IO Type. {0}", digitalIoType.ToString()));
                        break;
                }

                if (cardId >= 0)
                {
                    inputPortStatus = 0;
                    DASK.DI_ReadPort((ushort)cardId, 0, out inputPortStatus);

                    NumInPortGroup = digitalIoInfo.NumInPortGroup;
                    InPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
                    NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
                    OutPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

                    return true;
                }
                else
                {
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                        ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("DASK DIO Device registeration is failed. ( Type = {0} )", digitalIoType.ToString()));
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), string.Format("DASK DIO Device initalization is failed. ( Type = {0} ) : {1}", digitalIoType.ToString(), ex.Message));
            }

            return false;
        }

        public override bool IsReady()
        {
            return (cardId >= 0);
        }

        public override void Release()
        {
            base.Release();

            try
            {
                if (IsReady())
                {
                    DASK.Release_Card((ushort)cardId);
                }
            }
            catch
            {
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            lock (this)
            {
                this.outputPortStatus = outputPortStatus;
                for (ushort i = 0; i < 32; i++)
                {
                    DASK.DO_WriteLine((ushort)cardId, 0, i, (ushort)((outputPortStatus >> i) & 1));
                }
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            DASK.DO_ReadPort((ushort)cardId, 0, out outputPortStatus);

            return outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            DASK.DI_ReadPort((ushort)cardId, 0, out inputPortStatus);

            return inputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            DASK.DO_WriteLine((ushort)cardId, (ushort)groupNo, (ushort)portNo, (ushort)(value ? 1 : 0));
            //DASK.DO_WritePort((ushort)cardId, (uint)portNo, (uint)(value ? 1 : 0));
        }
    }
}
