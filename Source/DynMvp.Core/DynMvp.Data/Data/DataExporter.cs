using DynMvp.Base;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices.Comm;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace DynMvp.Data
{
    public class ValueData
    {
        public string ObjectName { get; set; }
        public string ValueName { get; set; }

        public ValueData()
        {
        }

        public ValueData(string objectName, string valueName)
        {
            ObjectName = objectName;
            ValueName = valueName;
        }

        public void Load(XmlElement valueDataElement)
        {
            ObjectName = XmlHelper.GetValue(valueDataElement, "ObjectName", "");
            ValueName = XmlHelper.GetValue(valueDataElement, "ValueName", "");
        }

        public void PacketLoad(XmlElement valueDataElement)
        {
            ObjectName = XmlHelper.GetValue(valueDataElement, "ObjectName", "");
            ValueName = XmlHelper.GetValue(valueDataElement, "ValueName", "");
        }

        public void Save(XmlElement valueDataElement)
        {
            XmlHelper.SetValue(valueDataElement, "ObjectName", ObjectName.ToString());
            XmlHelper.SetValue(valueDataElement, "ValueName", ValueName.ToString());
        }

        internal ValueData Clone()
        {
            var valueData = new ValueData();
            valueData.ObjectName = ObjectName;

            valueData.ValueName = ValueName;

            return valueData;
        }
    }

    public enum DelimiterType
    {
        Ascii, Hex
    }

    public class ExportPacketFormat
    {
        public DelimiterType PacketStartType { get; set; } = DelimiterType.Ascii;
        public string PacketStart { get; set; } = "<START>";
        public DelimiterType PacketEndType { get; set; } = DelimiterType.Ascii;
        public string PacketEnd { get; set; } = "<END>";
        public bool UseCheckSum { get; set; }
        public int ChecksumSize { get; set; }
        public DelimiterType SeparatorType { get; set; } = DelimiterType.Ascii;
        public string Separator { get; set; } = ",";
        public List<ValueData> ValueDataList { get; set; } = new List<ValueData>();

        private string GetPacketStartString()
        {
            if (PacketStartType == DelimiterType.Hex)
            {
                return StringHelper.HexToString(PacketStart);
            }
            else
            {
                return PacketStart;
            }
        }

        public ExportPacketFormat Clone()
        {
            var exportPacketFormat = new ExportPacketFormat();

            exportPacketFormat.PacketStartType = PacketEndType;
            exportPacketFormat.PacketStart = PacketStart;
            exportPacketFormat.PacketEndType = PacketEndType;
            exportPacketFormat.PacketEnd = PacketEnd;
            exportPacketFormat.UseCheckSum = UseCheckSum;
            exportPacketFormat.ChecksumSize = ChecksumSize;
            exportPacketFormat.SeparatorType = SeparatorType;
            exportPacketFormat.Separator = Separator;

            foreach (ValueData f in ValueDataList)
            {
                exportPacketFormat.ValueDataList.Add(f.Clone());
            }

            return exportPacketFormat;
        }

        private string GetPacketEndString()
        {
            if (PacketEndType == DelimiterType.Hex)
            {
                return StringHelper.HexToString(PacketEnd);
            }
            else
            {
                return PacketEnd;
            }
        }

        private char GetSeparatorChar()
        {
            if (SeparatorType == DelimiterType.Hex)
            {
                return StringHelper.HexToString(Separator)[0];
            }
            else
            {
                return Separator[0];
            }
        }

        public byte[] GetStartChar()
        {
            return Encoding.ASCII.GetBytes(GetPacketStartString());
        }

        public byte[] GetEndChar()
        {
            return Encoding.ASCII.GetBytes(GetPacketEndString());
        }

        public string GetPacketTitle()
        {
            var sb = new StringBuilder();
            sb.Append(GetPacketStartString());

            char separatorCh = GetSeparatorChar();

            foreach (ValueData valueData in ValueDataList)
            {
                sb.Append(valueData.ObjectName);
                sb.Append(separatorCh);
            }

            return sb.ToString().TrimEnd(separatorCh) + GetPacketEndString();
        }

        private string GetValueString(object value)
        {
            string valueStr = "";
            if (value is float)
            {
                valueStr = ((float)value).ToString("+000.000;-000.000;+000.000");
            }
            else if (value is int)
            {
                valueStr = ((int)value).ToString();
            }
            else if (value is bool)
            {
                valueStr = ((bool)value).ToString();
            }
            else if (value is Point pt)
            {
                valueStr = string.Format("{0},{1}", pt.X, pt.Y); ;
            }
            else if (value is PointF ptf)
            {
                valueStr = string.Format("{0},{1}", ptf.X, ptf.Y); ;
            }
            else if (value is ResultValueItem resultValueItem)
            {
                valueStr = resultValueItem.GetValueString();
            }

            return valueStr;
        }

        public string GetPacket(ProductResult productResult)
        {
            var sb = new StringBuilder();

            char separatorCh = GetSeparatorChar();

            foreach (ValueData valueData in ValueDataList)
            {
                ProbeResultList targetResult = productResult.GetTargetResult(valueData.ObjectName);
                if (targetResult.Count() > 0)
                {
                    sb.Append(targetResult.GetGoodNgStr());
                    sb.Append(separatorCh);
                    continue;
                }

                ProbeResult probeResult = productResult.GetProbeResult(valueData.ObjectName);
                if (probeResult != null)
                {
                    ResultValue probeResultValue = probeResult.GetResultValue(valueData.ValueName);
                    if (probeResultValue != null)
                    {
                        sb.Append(GetValueString(probeResultValue.Value));
                        sb.Append(separatorCh);
                    }
                }

                if (valueData.ObjectName == "Model")
                {
                    object value = productResult.GetAdditionalValue(valueData.ValueName);
                    if (value != null)
                    {
                        sb.Append(GetValueString(value));
                        sb.Append(separatorCh);
                    }
                }
            }

            //            string packetData = "DONE,0," + (inspectionResult.Judgment == Judgment.Accept ? "OK," : "NG,") + sb.ToString().TrimEnd(separatorCh);
            string packetData = sb.ToString().TrimEnd(separatorCh);

            string checksum = StringHelper.GetChecksum(packetData, ChecksumSize);

            return GetPacketStartString() + packetData + GetPacketEndString();
        }

        public void Load(XmlElement modelDescElement)
        {
            PacketStartType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "PacketStartType", "Ascii"));
            PacketStart = XmlHelper.GetValue(modelDescElement, "PacketStart", "");
            PacketEndType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "PacketEndType", "Ascii"));
            PacketEnd = XmlHelper.GetValue(modelDescElement, "PacketEnd", "");
            SeparatorType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "SeparatorType", "Ascii"));
            Separator = XmlHelper.GetValue(modelDescElement, "Separator", ",");
            UseCheckSum = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "UseChecksum", "False"));
            ChecksumSize = Convert.ToInt32(XmlHelper.GetValue(modelDescElement, "ChecksumSize", "2"));

            XmlElement valueDataListElement = modelDescElement["ValueDataList"];
            if (valueDataListElement != null)
            {
                ValueDataList.Clear();
                foreach (XmlElement valueDataElement in valueDataListElement)
                {
                    if (valueDataElement.Name == "ValueData")
                    {
                        var valueData = new ValueData();
                        valueData.Load(valueDataElement);

                        ValueDataList.Add(valueData);
                    }
                }
            }
        }

        public void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "PacketStartType", PacketStartType.ToString());
            XmlHelper.SetValue(modelDescElement, "PacketStart", PacketStart);
            XmlHelper.SetValue(modelDescElement, "PacketEndType", PacketEndType.ToString());
            XmlHelper.SetValue(modelDescElement, "PacketEnd", PacketEnd);
            XmlHelper.SetValue(modelDescElement, "SeparatorType", SeparatorType.ToString());
            XmlHelper.SetValue(modelDescElement, "Separator", Separator.ToString());
            XmlHelper.SetValue(modelDescElement, "UseChecksum", UseCheckSum.ToString());
            XmlHelper.SetValue(modelDescElement, "ChecksumSize", ChecksumSize.ToString());

            XmlElement valueDataListElement = modelDescElement.OwnerDocument.CreateElement("", "ValueDataList", "");
            modelDescElement.AppendChild(valueDataListElement);

            foreach (ValueData valueData in ValueDataList)
            {
                XmlElement valueDataElement = modelDescElement.OwnerDocument.CreateElement("", "ValueData", "");
                valueDataListElement.AppendChild(valueDataElement);

                valueData.Save(valueDataElement);
            }
        }
    }

    public interface IDataExporter
    {
        void Export(ProductResult productResult);
    }

    public class TextProductOverviewDataExport : IDataExporter
    {
        private string resultPath;
        private object fileLock = new object();

        public TextProductOverviewDataExport(string resultPath)
        {
            this.resultPath = resultPath;
        }

        public void Export(ProductResult productResult)
        {
            string shortTime = productResult.InspectStartTime.ToString("yyyyMMdd");
            string resultFile = string.Format("{0}\\{1}_{2}.csv", resultPath, shortTime, productResult.ModelName);

            lock (fileLock)
            {
                var fs = new FileStream(resultFile, FileMode.OpenOrCreate);

                if (fs != null)
                {
                    fs.Seek(0, SeekOrigin.End);

                    var sw = new StreamWriter(fs, Encoding.Default);

                    string resultStr = string.Format("{0},{1}", productResult.InspectionNo, productResult.GetGoodNgStr());
                    sw.WriteLine(resultStr);

                    sw.Close();
                    fs.Close();
                }
            }
        }
    }

    public class TextProductResultDataExport : IDataExporter
    {
        internal TextInspResultArchiver DataTextResult { get; } = new TextInspResultArchiver();

        public void Export(ProductResult productResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "SaveResult");

            DataTextResult.Save(productResult);
        }
    }

    public class SerialDataExporter : IDataExporter
    {
        private SerialPortEx serialPortEx;

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get => exportPacketFormat;
            set => exportPacketFormat = value;
        }

        public SerialDataExporter(SerialPortEx serialPortEx)
        {
            this.serialPortEx = serialPortEx;
        }

        public void Export(ProductResult productResult)
        {
            if (exportPacketFormat != null)
            {
                serialPortEx.WritePacket(exportPacketFormat.GetPacket(productResult));
            }
        }
    }

    public class TcpIpServerDataExporter : IDataExporter
    {
        private ServerSocket serverSocket;

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get => exportPacketFormat;
            set => exportPacketFormat = value;
        }

        public TcpIpServerDataExporter(ServerSocket serverSocket)
        {
            this.serverSocket = serverSocket;
        }

        public void Export(ProductResult productResult)
        {
            //if (exportPacketFormat != null)
            //    serverSocket.SendCommand(exportPacketFormat.GetPacket(productResult))); // server Socket
        }
    }

    public class TcpIpClientDataExporter : IDataExporter
    {
        private SinglePortSocket singlePortSocket;

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get => exportPacketFormat;
            set => exportPacketFormat = value;
        }

        public TcpIpClientDataExporter(SinglePortSocket singlePortSocket)
        {
            this.singlePortSocket = singlePortSocket;
        }

        public void Export(ProductResult productResult)
        {
            if (exportPacketFormat != null)
            {
                singlePortSocket.SendCommand(exportPacketFormat.GetPacket(productResult));
            }
        }
    }
}
