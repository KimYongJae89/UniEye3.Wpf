using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Devices
{
    public class LabcoParticleCounter
    {
        public delegate void ParticleDataDelegate(LPCDataInfo lpcDataInfo);
        public ParticleDataDelegate ParticleDataReceived;
        private StxEtxProtocol protocol;
        public ClientSocket Socket;
        private string[] data = new string[6];

        public class LPCDataInfo
        {
            public double Temp { get; set; }
            public double Humi { get; set; }
            public int Dot5 { get; set; }
            public int Dot3 { get; set; }

            public LPCDataInfo()
            {
                Temp = 0;
                Humi = 0;
                Dot5 = 0;
                Dot3 = 0;
            }
        }

        public LabcoParticleCounter()
        {
            var startArray = new List<byte>();
            startArray.Add(Convert.ToByte('$'));

            var endArray = new List<byte>();
            endArray.Add(Convert.ToByte('#'));

            protocol = new StxEtxProtocol();
            protocol.StartChar = startArray.ToArray();
            protocol.EndChar = endArray.ToArray();
            protocol.UseChecksum = false;
        }

        public bool Connect(string ipAddress, int port)
        {
            if (Socket != null)
            {
                Socket.Dispose();
            }

            Socket = new ClientSocket(new TcpIpInfo(ipAddress, port), protocol);
            Socket.DataReceived = ParticleCounterData;
            Socket.ServerDisconnected = ParticleCounterDisconnected;

            Socket.Connect();

            return Socket.IsConnected();
        }

        public void ParticleCounterData(ReceivedPacket packet)
        {
            var encoding = new System.Text.UTF8Encoding();
            string Tempdata = encoding.GetString(packet.ReceivedDataByte);
            if (Tempdata != string.Empty)
            {
                data = Tempdata.Split(',');
            }

            var info = new LPCDataInfo();
            info.Dot3 = Convert.ToInt32(data[0]);
            info.Dot5 = Convert.ToInt32(data[1]);
            info.Temp = Convert.ToDouble(data[2]);
            info.Humi = Convert.ToDouble(data[3]);

            ParticleDataReceived?.Invoke(info);
        }

        private void ParticleCounterDisconnected(TcpIpInfo hostInfo)
        {
            ParticleDataReceived?.Invoke(null);
        }
    }
}
