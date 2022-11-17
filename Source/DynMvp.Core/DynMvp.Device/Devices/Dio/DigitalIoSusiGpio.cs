using DynMvp.Base;
using Susi4.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoSusiGpio : DigitalIo
    {
        private class DeviceInfo
        {
            public uint BankNumber;
            public uint SupportInput;
            public uint SupportOutput;

            public DeviceInfo(uint bankNumber)
            {
                BankNumber = bankNumber;
                SupportInput = 0;
                SupportOutput = 0;
            }
        }

        private class DevPinInfo
        {
            public uint ID;
            public string Name { get; } = "";

            public override string ToString()
            {
                return string.Format("{0} ({1})", ID, Name);
            }

            public DevPinInfo(uint DeviceID)
            {
                ID = DeviceID;

                uint Length = 32;
                var sb = new StringBuilder((int)Length);
                if (SusiBoard.SusiBoardGetStringA(SusiBoard.SUSI_ID_MAPPING_GET_NAME_GPIO(ID), sb, ref Length) == SusiStatus.SUSI_STATUS_SUCCESS)
                {
                    Name = sb.ToString();
                }
            }
        }

        private const int MAX_BANK_NUM = 4;
        private uint inputPortStatus = 0;
        private uint outputPortStatus = 0;
        private static bool gpioInitialized = false;
        private static List<DeviceInfo> gpioDeviceList = new List<DeviceInfo>();
        private DeviceInfo selectedGpioDevice = null;
        private List<uint> inputPinIdList = new List<uint>();
        private List<uint> outputPinIdList = new List<uint>();
        private List<DeviceInfo> DevList = new List<DeviceInfo>();
        private List<DevPinInfo> DevPinList = new List<DevPinInfo>();

        public DigitalIoSusiGpio(DigitalIoType type, string name)
            : base(type, name)
        {
            NumInPort = NumOutPort = 4;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            if (gpioInitialized == false)
            {
                try
                {
                    uint Status = SusiLib.SusiLibInitialize();

                    if (Status != SusiStatus.SUSI_STATUS_SUCCESS && Status != SusiStatus.SUSI_STATUS_INITIALIZED)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                            ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), "Can't initialize SUSI device");
                        return false;
                    }
                }
                catch
                {
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                        ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), "Unknown error on initialization process for SUSI device");
                    return false;
                }

                InitializeGPIO();

                gpioInitialized = true;
            }

            var pciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;
            if (pciDigitalIoInfo.Index < gpioDeviceList.Count)
            {
                selectedGpioDevice = gpioDeviceList[pciDigitalIoInfo.Index];
                InitializePins();
                NumInPort = inputPinIdList.Count;
                NumOutPort = outputPinIdList.Count;
                NumInPort = 4;
                NumOutPort = 4;
            }

            return true;
        }

        private void InitializeGPIO()
        {
            uint status;

            for (int i = 0; i < MAX_BANK_NUM; i++)
            {
                var info = new DeviceInfo((uint)i);

                status = SusiGPIO.SusiGPIOGetCaps(info.BankNumber, SusiGPIO.SUSI_ID_GPIO_INPUT_SUPPORT, out info.SupportInput);
                if (status != SusiStatus.SUSI_STATUS_SUCCESS)
                {
                    continue;
                }

                status = SusiGPIO.SusiGPIOGetCaps(info.BankNumber, SusiGPIO.SUSI_ID_GPIO_OUTPUT_SUPPORT, out info.SupportOutput);
                if (status != SusiStatus.SUSI_STATUS_SUCCESS)
                {
                    continue;
                }

                gpioDeviceList.Add(info);
            }
        }

        private void InitializePins()
        {
            if (selectedGpioDevice == null)
            {
                return;
            }

            uint mask;

            for (int j = 0; j < 32; j++)
            {
                mask = (uint)(1 << j);
                if ((selectedGpioDevice.SupportInput & mask) > 0)
                {
                    inputPinIdList.Add((uint)((selectedGpioDevice.BankNumber << 5) + j));
                }
                else if ((selectedGpioDevice.SupportOutput & mask) > 0)
                {
                    outputPinIdList.Add((uint)((selectedGpioDevice.BankNumber << 5) + j));
                }
            }
            uint temp = 4;
            outputPinIdList.Add(temp);
            //for (int i = 0; i < gpioDeviceList.Count; i++)
            //{
            //    // 32 pins per bank
            //    for (int j = 0; j < 32; j++)
            //    {
            //        mask = (UInt32)(1 << j);
            //        if ((DevList[i].SupportInput & mask) > 0 || (DevList[i].SupportOutput & mask) > 0)
            //        {
            //            DevPinInfo pinInfo = new DevPinInfo((UInt32)((i << 5) + j));
            //            DevPinList.Add(pinInfo);
            //        }
            //    }
            //}


        }

        public override bool IsReady()
        {
            return (inputPinIdList.Count > 0 || outputPinIdList.Count > 0);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            lock (this)
            {
                this.outputPortStatus = outputPortStatus;

                uint status;
                for (ushort i = 0; i < outputPinIdList.Count; i++)
                {
                    status = SusiGPIO.SusiGPIOSetLevel(outputPinIdList[i], GetMask(), (outputPortStatus >> i) & GetMask());
                    if (status != SusiStatus.SUSI_STATUS_SUCCESS)
                    {
                        LogHelper.Error(string.Format("SusiGPIOSetLevel() failed. (0x{0:X8})", status));
                    }
                }
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            return outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            lock (this)
            {
                inputPortStatus = 0;


                uint status;
                for (ushort i = 0; i < inputPinIdList.Count; i++)
                {
                    status = SusiGPIO.SusiGPIOGetLevel(inputPinIdList[i], GetMask(), out uint value);
                    if (status != SusiStatus.SUSI_STATUS_SUCCESS)
                    {
                        LogHelper.Error(string.Format("SusiGPIOSetLevel() failed. (0x{0:X8})", status));
                    }

                    inputPortStatus |= ((value & 0x1) << i);
                }
            }

            return inputPortStatus;
        }
        private uint GetMask()
        {
            uint mask = 511;

            return mask;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
