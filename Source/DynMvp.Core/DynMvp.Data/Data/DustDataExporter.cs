using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.Comm;
using System.Drawing;
using DynMvp.Data.Forms;
using System.Threading;
using DynMvp.Vision;
using System.IO;
using DynMvp.InspectData;
using DynMvp.Data.UI;

namespace DynMvp.Data
{
    public class ValueData
    {
        string objectName;
        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }

        string valueName;
        public string ValueName
        {
            get { return valueName; }
            set { valueName = value; }
        }

        public ValueData()
        {
        }

        public ValueData(string objectName, string valueName)
        {
            this.objectName = objectName;
            this.valueName = valueName;
        }

        public void Load(XmlElement valueDataElement)
        {
            objectName = XmlHelper.GetValue(valueDataElement, "ObjectName", "");
            valueName = XmlHelper.GetValue(valueDataElement, "ValueName", "");
        }

        public void PacketLoad(XmlElement valueDataElement)
        {
            objectName = XmlHelper.GetValue(valueDataElement, "ObjectName", "");
            valueName = XmlHelper.GetValue(valueDataElement, "ValueName", "");
        }

        public void Save(XmlElement valueDataElement)
        {
            XmlHelper.SetValue(valueDataElement, "ObjectName", objectName.ToString());
            XmlHelper.SetValue(valueDataElement, "ValueName", valueName.ToString());
        }

        internal ValueData Clone()
        {
            ValueData valueData = new ValueData();
            valueData.objectName = this.objectName;

            valueData.valueName = this.valueName;

            return valueData;
        }
    }
    public enum DelimiterType
    {
        Ascii, Hex
    }

    public class ExportPacketFormat
    {
        DelimiterType packetStartType = DelimiterType.Ascii;
        public DelimiterType PacketStartType
        {
            get { return packetStartType; }
            set { packetStartType = value; }
        }

        string packetStart = "<START>";
        public string PacketStart
        {
            get { return packetStart; }
            set { packetStart = value; }
        }

        DelimiterType packetEndType = DelimiterType.Ascii;
        public DelimiterType PacketEndType
        {
            get { return packetEndType; }
            set { packetEndType = value; }
        }

        string packetEnd = "<END>";
        public string PacketEnd
        {
            get { return packetEnd; }
            set { packetEnd = value; }
        }

        bool useCheckSum;
        public bool UseCheckSum
        {
            get { return useCheckSum; }
            set { useCheckSum = value; }
        }

        int checksumSize;
        public int ChecksumSize
        {
            get { return checksumSize; }
            set { checksumSize = value; }
        }

        DelimiterType separatorType = DelimiterType.Ascii;
        public DelimiterType SeparatorType
        {
            get { return separatorType; }
            set { separatorType = value; }
        }

        string separator = ",";
        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        List<ValueData> valueDataList = new List<ValueData>();
        public List<ValueData> ValueDataList
        {
            get { return valueDataList; }
            set { valueDataList = value; }
        }

        private string GetPacketStartString()
        {
            if (packetStartType == DelimiterType.Hex)
                return StringHelper.HexToString(packetStart);
            else
                return packetStart;
        }

        internal ExportPacketFormat Clone()
        {
            ExportPacketFormat exportPacketFormat = new ExportPacketFormat();

            exportPacketFormat.packetStartType = this.packetEndType;
            exportPacketFormat.packetStart = this.packetStart;
            exportPacketFormat.packetEndType = this.packetEndType;
            exportPacketFormat.packetEnd = this.packetEnd;
            exportPacketFormat.useCheckSum = this.useCheckSum;
            exportPacketFormat.checksumSize = this.checksumSize;
            exportPacketFormat.separatorType = this.separatorType;
            exportPacketFormat.separator = this.separator;

            foreach (ValueData f in this.valueDataList)
                exportPacketFormat.valueDataList.Add(f.Clone());

            return exportPacketFormat;
        }

        private string GetPacketEndString()
        {
            if (packetEndType == DelimiterType.Hex)
                return StringHelper.HexToString(packetEnd);
            else
                return packetEnd;
        }

        private char GetSeparatorChar()
        {
            if (separatorType == DelimiterType.Hex)
                return StringHelper.HexToString(separator)[0];
            else
                return separator[0];
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
            StringBuilder sb = new StringBuilder();
            sb.Append(GetPacketStartString());

            char separatorCh = GetSeparatorChar();

            foreach (ValueData valueData in valueDataList)
            {
                sb.Append(valueData.ObjectName);
                sb.Append(separatorCh);
            }

            return sb.ToString().TrimEnd(separatorCh) + GetPacketEndString();
        }

        string GetValueString(object value)
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
            else if (value is Point)
            {
                Point pt = (Point)value;
                valueStr = String.Format("{0},{1}", pt.X, pt.Y); ;
            }
            else if (value is PointF)
            {
                PointF pt = (PointF)value;
                valueStr = String.Format("{0},{1}", pt.X, pt.Y); ;
            }
            else if (value is ResultValueItem)
            {
                ResultValueItem resultValueItem = (ResultValueItem)value;
                valueStr = resultValueItem.GetValueString();
            }

            return valueStr;
        }

        public string GetPacket(ProductResult productResult)
        {
            StringBuilder sb = new StringBuilder();

            char separatorCh = GetSeparatorChar();

            foreach (ValueData valueData in valueDataList)
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

            string checksum = StringHelper.GetChecksum(packetData, checksumSize);

            return GetPacketStartString() + packetData + GetPacketEndString();
        }

        public void Load(XmlElement modelDescElement)
        {
            packetStartType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "PacketStartType", "Ascii"));
            packetStart = XmlHelper.GetValue(modelDescElement, "PacketStart", "");
            packetEndType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "PacketEndType", "Ascii"));
            packetEnd = XmlHelper.GetValue(modelDescElement, "PacketEnd", "");
            separatorType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "SeparatorType", "Ascii"));
            separator = XmlHelper.GetValue(modelDescElement, "Separator", ",");
            useCheckSum = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "UseChecksum", "False"));
            checksumSize = Convert.ToInt32(XmlHelper.GetValue(modelDescElement, "ChecksumSize", "2"));

            XmlElement valueDataListElement = modelDescElement["ValueDataList"];
            if (valueDataListElement != null)
            {
                valueDataList.Clear();
                foreach (XmlElement valueDataElement in valueDataListElement)
                {
                    if (valueDataElement.Name == "ValueData")
                    {
                        ValueData valueData = new ValueData();
                        valueData.Load(valueDataElement);

                        valueDataList.Add(valueData);
                    }
                }
            }
        }

        public void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "PacketStartType", packetStartType.ToString());
            XmlHelper.SetValue(modelDescElement, "PacketStart", packetStart);
            XmlHelper.SetValue(modelDescElement, "PacketEndType", packetEndType.ToString());
            XmlHelper.SetValue(modelDescElement, "PacketEnd", packetEnd);
            XmlHelper.SetValue(modelDescElement, "SeparatorType", separatorType.ToString());
            XmlHelper.SetValue(modelDescElement, "Separator", separator.ToString());
            XmlHelper.SetValue(modelDescElement, "UseChecksum", useCheckSum.ToString());
            XmlHelper.SetValue(modelDescElement, "ChecksumSize", checksumSize.ToString());

            XmlElement valueDataListElement = modelDescElement.OwnerDocument.CreateElement("", "ValueDataList", "");
            modelDescElement.AppendChild(valueDataListElement);

            foreach (ValueData valueData in valueDataList)
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
        string resultPath;
        object fileLock = new object();

        public TextProductOverviewDataExport(string resultPath)
        {
            this.resultPath = resultPath;
        }

        public void Export(ProductResult productResult)
        {
            string shortTime = productResult.InspectTime.ToString("yyyyMMdd");
            string resultFile = String.Format("{0}\\{1}_{2}.csv", resultPath, shortTime, productResult.ModelName);

            lock (fileLock)
            {
                FileStream fs = new FileStream(resultFile, FileMode.OpenOrCreate);

                if (fs != null)
                {
                    fs.Seek(0, SeekOrigin.End);

                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                    string resultStr = String.Format("{0},{1}", productResult.InspectionNo, productResult.GetGoodNgStr());
                    sw.WriteLine(resultStr);

                    sw.Close();
                    fs.Close();
                }
            }
        }
    }

    public class TextProductResultDataExport : IDataExporter
    {
        private TextInspResultArchiver dataTextResult = new TextInspResultArchiver();
        internal TextInspResultArchiver DataTextResult
        {
            get { return DataTextResult; }
        }

        public void Export(ProductResult productResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "SaveResult");

            dataTextResult.Save(productResult);
        }
    }

    public class SerialDataExporter : IDataExporter
    {
        SerialPortEx serialPortEx;

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get { return exportPacketFormat; }
            set { exportPacketFormat = value; }
        }

        public SerialDataExporter(SerialPortEx serialPortEx)
        {
            this.serialPortEx = serialPortEx;
        }

        public void Export(ProductResult productResult)
        {
            if (exportPacketFormat != null)
                serialPortEx.WritePacket(exportPacketFormat.GetPacket(productResult));
        }
    }

    public class TcpIpServerDataExporter : IDataExporter
    {
        ServerSocket serverSocket;

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get { return exportPacketFormat; }
            set { exportPacketFormat = value; }
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
        SinglePortSocket singlePortSocket;

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get { return exportPacketFormat; }
            set { exportPacketFormat = value; }
        }

        public TcpIpClientDataExporter(SinglePortSocket singlePortSocket)
        {
            this.singlePortSocket = singlePortSocket;
        }

        public void Export(ProductResult productResult)
        {
            if (exportPacketFormat != null)
                singlePortSocket.SendCommand(exportPacketFormat.GetPacket(productResult));
        }
    }
}
