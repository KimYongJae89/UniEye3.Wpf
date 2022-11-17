using AxActUtlTypeLib;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices
{
    public delegate void axActUtlDelegate();
    public delegate void axActUtlHaltedDelegate(int code);

    public class MxCompData
    {
        public string DeviceName { get; }

        private List<short> dataList = new List<short>();

        public int DataSize => dataList.Count;

        public short[] DataArray
        {
            get => dataList.ToArray();
            set { dataList.Clear(); dataList.AddRange(value); }
        }

        // Write
        public MxCompData(string deviceName)
        {
            DeviceName = deviceName;
        }

        // Read
        public MxCompData(string deviceName, int size)
        {
            DeviceName = deviceName;
            for (int i = 0; i < size; i++)
            {
                dataList.Add(0);
            }
        }

        public void SetData(int index, short value)
        {
            dataList[index] = value;
        }

        public void AddData(int data)
        {
            byte[] bytes = BitConverter.GetBytes(data);
            dataList.Add(BitConverter.ToInt16(bytes, 0));
            dataList.Add(BitConverter.ToInt16(bytes, 2));
        }

        public void AddData(bool data)
        {
            dataList.Add(Convert.ToInt16(data));
        }

        public void AddData(short data)
        {
            dataList.Add(data);
        }


        public void AddData(string data)
        {
            if (data.Length % 2 != 0)
            {
                byte[] bytes = new byte[data.Length + 1];
                bytes[0] = 0;
                byte[] tempBytes = Encoding.ASCII.GetBytes(data);
                for (int t = 1; t < bytes.Length; t++)
                {
                    bytes[t] = tempBytes[t - 1];
                }

                for (int i = 0; i < bytes.Length; i += 2)
                {
                    //byte[] subByte = new byte[2];
                    //subByte[0] = bytes[i + 1];
                    //subByte[1] = bytes[i];

                    dataList.Add(BitConverter.ToInt16(bytes, i));
                }
            }
            else
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data);

                for (int i = 0; i < bytes.Length; i += 2)
                {
                    //byte[] subByte = new byte[2];
                    //subByte[0] = bytes[i + 1];
                    //subByte[1] = bytes[i];

                    dataList.Add(BitConverter.ToInt16(bytes, i));
                }
            }
        }

        public short GetShort(int index)
        {
            if (dataList.Count <= index)
            {
                return 0;
            }

            return dataList[index];
        }

        public string GetString(int index, int size)
        {
            string dataStr = "";
            for (int i = 0; i < dataList.Count; i++)
            {
                byte[] dataByte = BitConverter.GetBytes(dataList[i]);

                dataStr += Encoding.ASCII.GetString(dataByte);
            }

            return dataStr;
        }
    }

    public class MxCompSetting
    {
        [DisplayName("Virtual Mode")]
        public bool VirtualMode { get; set; } = true;
        [DisplayName("Station Number")]
        public string StationNumber { get; set; } = "0";
        [DisplayName("Timeout")]
        public int Timeout { get; set; } = 2000;
        [DisplayName("Retry Interval")]
        public int RetryInterval { get; set; } = 3000;

        //string machineStatusAddress = "100";
        //[DisplayName("Machine Status Address")]
        //public string MachineStatusAddress
        //{
        //    get { return machineStatusAddress; }
        //    set { machineStatusAddress = value; }
        //}

        //string visionStatusAddress = "200";
        //[DisplayName("Vision Status Address")]
        //public string VisionStatusAddress
        //{
        //    get { return visionStatusAddress; }
        //    set { visionStatusAddress = value; }
        //}

        public void Save(XmlElement configElement)
        {
            XmlHelper.SetValue(configElement, "VirtualMode", VirtualMode.ToString());
            XmlHelper.SetValue(configElement, "StationNumber", StationNumber);
            XmlHelper.SetValue(configElement, "Timeout", Timeout.ToString());
            XmlHelper.SetValue(configElement, "RetryInterval", RetryInterval.ToString());
            //XmlHelper.SetValue(configElement, "MachineStatusAddress", machineStatusAddress);
            //XmlHelper.SetValue(configElement, "VisionStatusAddress", visionStatusAddress);
        }

        public void Load(XmlElement configElement)
        {
            VirtualMode = bool.Parse(XmlHelper.GetValue(configElement, "VirtualMode", "false"));
            StationNumber = XmlHelper.GetValue(configElement, "StationNumber", "0");
            Timeout = int.Parse(XmlHelper.GetValue(configElement, "Timeout", Timeout.ToString()));
            RetryInterval = int.Parse(XmlHelper.GetValue(configElement, "RetryInterval", RetryInterval.ToString()));
            //machineStatusAddress = XmlHelper.GetValue(configElement, "MachineStatusAddress", "0");
            //visionStatusAddress = XmlHelper.GetValue(configElement, "VisionStatusAddress", "0");
        }
    }

    public class MxCompWrapper
    {
        public axActUtlDelegate Opened = null;
        public axActUtlDelegate Closed = null;
        public axActUtlHaltedDelegate Halted = null;
        public AxActUtlType AxActUtlType { get; set; } = null;

        private int lastReturnCode = 0;
        private Thread openThread = null;
        public bool IsConnected { get; private set; } = false;

        public bool Open(MxCompSetting mxCompSetting)
        {
            int stationNo = 0; //int.Parse(mxCompSetting.StationNumber);
            AxActUtlType.ActLogicalStationNumber = stationNo;

            openThread = new Thread(new ParameterizedThreadStart(OpenProc));
            openThread.Start(stationNo);
            bool ok = openThread.Join(mxCompSetting.Timeout);
            if (!ok || IsConnected == false)
            {
                Halted?.Invoke(lastReturnCode);

                return false;
            }
            else
            {
                Opened?.Invoke();
                return true;
            }
        }

        private void OpenProc(object stationNo)
        {
            try
            {
                lastReturnCode = AxActUtlType.Open();
            }
            catch (Exception)
            {
                lastReturnCode = -1;
            }

            if (lastReturnCode != 0)
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = true;
            }
        }

        public void Close()
        {
            if (AxActUtlType != null)
            {
                lastReturnCode = AxActUtlType.Close();

                if (lastReturnCode == 0)
                {
                    IsConnected = false;
                    Closed?.Invoke();
                }
            }
        }

        public int ReadDevice(MxCompData data)
        {
            if (AxActUtlType == null || !IsConnected)
            {
                return -1;
            }

            short[] dataArray = data.DataArray;

            if (dataArray.Length != 1)
            {
                lastReturnCode = AxActUtlType.ReadDeviceBlock2(data.DeviceName, dataArray.Length, out dataArray[0]);
                if (lastReturnCode != 0)
                {
                    return -1;
                }

                data.DataArray = dataArray;
            }
            else
            {
                lastReturnCode = AxActUtlType.ReadDeviceBlock2(data.DeviceName, 1, out short value);
                if (lastReturnCode != 0)
                {
                    return -1;
                }

                data.SetData(0, value);
            }

            return lastReturnCode;
        }

        public int WriteDevice(MxCompData data)
        {
            if (AxActUtlType == null || !IsConnected)
            {
                return -1;
            }

            short[] dataArray = data.DataArray;

            lastReturnCode = AxActUtlType.WriteDeviceBlock2(data.DeviceName, dataArray.Length, ref dataArray[0]);
            if (lastReturnCode != 0)
            {
                IsConnected = false;
                AxActUtlType.Close();
                AxActUtlType.Disconnect();  // 이건 왜 해야하지???
                Halted?.Invoke(lastReturnCode);
            }
            return lastReturnCode;
        }

        public void Dispose()
        {
            if (AxActUtlType == null)
            {
                return;
            }

            AxActUtlType.Disconnect();  // 이건 왜 해야하지???
            Thread.Sleep(100);
            //axActUtlType.Dispose();
        }
    }

}
