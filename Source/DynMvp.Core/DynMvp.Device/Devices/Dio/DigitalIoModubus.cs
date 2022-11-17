using DynMvp.Base;
using ModbusRTU;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    internal class DigitalIoModubus : DigitalIo
    {
        private ModbusLib modbus;
        private object lockObject = new object();

        public DigitalIoModubus(string name)
            : base(DigitalIoType.Modubus, name)
        {
            NumInPort = NumOutPort = 16;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            var serialDigitalIoInfo = (SerialDigitalIoInfo)digitalIoInfo;

            modbus = new ModbusLib();
            modbus.Port = serialDigitalIoInfo.SerialPortInfo.PortName; // "COM3"
            modbus.Baudrate = serialDigitalIoInfo.SerialPortInfo.BaudRate; // 57600;
            if (modbus.Open() == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), "Unknown error on initialization process for MODBUS device");
                return false;
            }
            modbus.Timeout = 1000;

            return true;
        }

        public override bool IsReady()
        {
            return true;
        }

        public override void Release()
        {
            base.Release();
            modbus.Close();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            LogHelper.Debug(LoggerType.Device, "WriteOutputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.Device, "Start WriteOutputGroup");

                    bool[] value = new bool[16];
                    string valueStr = Convert.ToString(outputPortStatus, 2).PadLeft(16);
                    for (int i = 0; i < 16; i++)
                    {
                        value[i] = valueStr.Substring(15 - i, 1)[0] == '1';
                    }

                    modbus.MultiBitWrite(1, 5980, value);

                    LogHelper.Debug(LoggerType.Device, "End WriteOutputGroup");
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(string.Format("Modubus Timeout Error : WriteOutputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(string.Format("Modubus CRC Error : WriteOutputGroup {0}", ex.Message));
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.Device, "ReadOutputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.Device, "Start ReadOutputGroup");

                    BitArray bitData = modbus.BitRead(1, 5980, 16);

                    uint data = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        if (bitData[i])
                        {
                            data = data | (((uint)1) << i);
                        }
                    }

                    LogHelper.Debug(LoggerType.Device, "End ReadOutputGroup");

                    return data;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(string.Format("Modubus Timeout Error : ReadOutputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(string.Format("Modubus CRC Error : ReadOutputGroup {0}", ex.Message));
            }

            return 0;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.Device, "ReadInputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.Device, "Start ReadInputGroup");

                    BitArray bitData = modbus.BitRead(1, 2980, 16);

                    uint data = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        if (bitData[i])
                        {
                            data = data | (((uint)1) << i);
                        }
                    }

                    LogHelper.Debug(LoggerType.Device, "End ReadInputGroup");

                    return data;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(string.Format("Modubus Timeout Error : ReadInputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(string.Format("Modubus CRC Error : ReadInputGroup {0}", ex.Message));
            }

            return 0;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
