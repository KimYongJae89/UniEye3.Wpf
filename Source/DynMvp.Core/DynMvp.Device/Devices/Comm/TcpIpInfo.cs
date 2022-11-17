using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.Comm
{
    public delegate void DataReceivedDelegate(ReceivedPacket receivedPacket);

    public interface ITcpIp
    {
        TcpIpInfo TcpIpInfo { get; set; }

        [JsonIgnore]
        DataReceivedDelegate DataReceived { get; set; }

        void Connect();
        void Disconnect();
        bool IsConnected();

        bool SendMessage(string message);
        bool SendMessage(byte[] bytes);
        bool SendMessage(SendPacket packet);
    }

    public class TcpIpInfo
    {
        public string IpAddress { get; set; }
        public int PortNo { get; set; }

        public TcpIpInfo()
        {
            IpAddress = "127.0.0.1";
            PortNo = 4000;
        }

        public TcpIpInfo(TcpIpInfo tcpIpInfo)
        {
            IpAddress = tcpIpInfo.IpAddress;
            PortNo = tcpIpInfo.PortNo;
        }

        public TcpIpInfo(string ipAddress, int portNo)
        {
            IpAddress = ipAddress;
            PortNo = portNo;
        }

        public TcpIpInfo Clone()
        {
            return new TcpIpInfo(this);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", IpAddress, PortNo);
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement tcpIpElement = xmlElement[keyName];
            if (tcpIpElement == null)
            {
                return;
            }

            IpAddress = XmlHelper.GetValue(tcpIpElement, "IpAddress", "");
            PortNo = Convert.ToInt32(XmlHelper.GetValue(tcpIpElement, "PortNo", "0"));
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement tcpIpElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(tcpIpElement);

            XmlHelper.SetValue(tcpIpElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(tcpIpElement, "PortNo", PortNo.ToString());
        }
    }
}
