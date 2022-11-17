using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DynMvp.Devices.Dio
{
    [Serializable]
    public class InputPortData
    {
        private const int maxIntSize = 32;

        public int data0;
        public int data1;
        public int data2;
        public int data3;
        public int data4;
        public int data5;
        public int data6;
        public int data7;
        public int data8;
        public int data9;
        public int data10;
        public int data11;
        public int data12;
        public int data13;
        public int data14;
        public int data15;
        public int data16;
        public int data17;
        public int data18;
        public int data19;
        public int data20;
        public int data21;
        public int data22;
        public int data23;
        public int data24;
        public int data25;
        public int data26;
        public int data27;
        public int data28;
        public int data29;
        public int data30;
        public int data31;

        public int[] GetDataArray()
        {
            int[] dataArray = new int[maxIntSize];

            dataArray[0] = data0;
            dataArray[1] = data1;
            dataArray[2] = data2;
            dataArray[3] = data3;
            dataArray[4] = data4;
            dataArray[5] = data5;
            dataArray[6] = data6;
            dataArray[7] = data7;
            dataArray[8] = data8;
            dataArray[9] = data9;
            dataArray[10] = data10;
            dataArray[11] = data11;
            dataArray[12] = data12;
            dataArray[13] = data13;
            dataArray[14] = data14;
            dataArray[15] = data15;
            dataArray[16] = data16;
            dataArray[17] = data17;
            dataArray[18] = data18;
            dataArray[19] = data19;
            dataArray[20] = data20;
            dataArray[21] = data21;
            dataArray[22] = data22;
            dataArray[23] = data23;
            dataArray[24] = data24;
            dataArray[25] = data25;
            dataArray[26] = data26;
            dataArray[27] = data27;
            dataArray[28] = data28;
            dataArray[29] = data29;
            dataArray[30] = data30;
            dataArray[31] = data31;

            return dataArray;
        }

        public uint GetInputValue()
        {
            int[] dataArray = GetDataArray();

            uint inputValue = 0;
            for (int index = 0; index < maxIntSize; index++)
            {
                if (dataArray[index] != 0)
                {
                    inputValue |= (uint)(1 << index);
                }
            }

            return inputValue;
        }
    }

    public class DigitalIoVirtual : DigitalIo
    {
        private List<uint> inputGroupStatus = new List<uint>();
        private List<uint> outputGroupStatus = new List<uint>();
        private string portStateFile;
        private DateTime lastOutputFileTime = DateTime.Now;
        private bool stopFlag = false;
        private Task checkTask;

        public DigitalIoVirtual(string name) : base(DigitalIoType.Virtual, name)
        {
        }

        public DigitalIoVirtual(DigitalIoType digitalIoType, string name) : base(digitalIoType, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            NumInPortGroup = digitalIoInfo.NumInPortGroup;
            InPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
            NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
            OutPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

            for (int i = 0; i < NumInPortGroup; i++)
            {
                uint portGroup = 0;
                inputGroupStatus.Add(portGroup);
            }

            for (int i = 0; i < NumInPortGroup; i++)
            {
                uint portGroup = 0;
                outputGroupStatus.Add(portGroup);
            }

            NumInPort = digitalIoInfo.NumInPort;
            NumOutPort = digitalIoInfo.NumOutPort;

            portStateFile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Config", "VirtualPortState.xml"));
            SavePortState();
            //LoadPortState();

            checkTask = new Task(new Action(CheckProc));
            checkTask.Start();

            return true;
        }

        private void CheckProc()
        {
            while (stopFlag == false)
            {
                try
                {
                    if (File.Exists(portStateFile) == false)
                    {
                        return;
                    }

                    var fileInfo = new FileInfo(portStateFile);
                    if (fileInfo.LastWriteTime != lastOutputFileTime)
                    {
                        if (LoadPortState() == true)
                        {
                            lastOutputFileTime = fileInfo.LastWriteTime;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public override bool IsReady()
        {
            return true;
        }

        public override void Release()
        {
            base.Release();
        }

        private bool LoadPortState()
        {
            if (File.Exists(portStateFile) == false)
            {
                return false;
            }

            var doc = new XmlDocument();

            try
            {
                doc.Load(portStateFile);
            }
            catch (IOException)
            {
                return false;
            }

            XmlElement element = doc["PortStatus"];
            if (element == null)
            {
                return false;
            }

            XmlElement inputElement = element["Input"];
            if (inputElement != null)
            {
                for (int i = 0; i < inputGroupStatus.Count; i++)
                {
                    string title = string.Format("Group{0}", i);
                    inputGroupStatus[i] = Convert.ToUInt32(XmlHelper.GetValue(inputElement, title, "0"));
                }
            }

            XmlElement outputElement = element["Output"];
            if (outputElement != null)
            {
                for (int i = 0; i < inputGroupStatus.Count; i++)
                {
                    string title = string.Format("Group{0}", i);
                    outputGroupStatus[i] = Convert.ToUInt32(XmlHelper.GetValue(outputElement, title, "0"));
                }
            }

            return true;
        }

        private void SavePortState()
        {
            var doc = new XmlDocument();
            XmlElement element = doc.CreateElement("", "PortStatus", "");
            doc.AppendChild(element);

            XmlElement inputElement = doc.CreateElement("", "Input", "");
            element.AppendChild(inputElement);
            for (int i = 0; i < inputGroupStatus.Count; i++)
            {
                string title = string.Format("Group{0}", i);
                XmlHelper.SetValue(inputElement, title, inputGroupStatus[i].ToString());
            }

            XmlElement outputElement = doc.CreateElement("", "Output", "");
            element.AppendChild(outputElement);
            for (int i = 0; i < inputGroupStatus.Count; i++)
            {
                string title = string.Format("Group{0}", i);
                XmlHelper.SetValue(outputElement, title, outputGroupStatus[i].ToString());
            }

            string srcFIleName = string.Format(@"{0}\..\Config\VirtualPortState.xml", Environment.CurrentDirectory);
            try
            {
                doc.Save(srcFIleName);
            }
            catch (IOException)
            {
                return;
            }
            catch (XmlException)
            {
                string bakFIleName = string.Format(@"{0}\..\Config\VirtualPortState.xml", Environment.CurrentDirectory);
                srcFIleName = string.Format(@"{0}\..\Config\VirtualPortState.xml~", Environment.CurrentDirectory);
                doc.Save(srcFIleName);
                FileHelper.SafeSave(srcFIleName, bakFIleName, portStateFile);
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            bool save = false;
            if (outputGroupStatus[groupNo] != outputPortStatus)
            {
                save = true;
            }

            outputGroupStatus[groupNo] = outputPortStatus;

            if (save)
            {
                SavePortState();
            }
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            bool save = false;
            if (inputGroupStatus[groupNo] != inputPortStatus)
            {
                save = true;
            }

            inputGroupStatus[groupNo] = inputPortStatus;

            if (save)
            {
                SavePortState();
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            return outputGroupStatus[groupNo];
        }

        public override uint ReadInputGroup(int groupNo)
        {
            return inputGroupStatus[groupNo];
        }

        public override void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            bool curVal = Convert.ToBoolean(outputGroupStatus[groupNo] >> portNo & 0x01);
            if (curVal == value)
            {
                return;
            }

            if (value)
            {
                outputGroupStatus[groupNo] |= (uint)0x01 << portNo;
            }
            else
            {
                outputGroupStatus[groupNo] &= ~((uint)0x01 << portNo);
            }

            SavePortState();
        }
    }
}
