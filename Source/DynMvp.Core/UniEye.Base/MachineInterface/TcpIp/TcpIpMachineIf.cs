using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface.Melsec;

namespace UniEye.Base.MachineInterface.TcpIp
{
    public partial class TcpIpClientMachineIf
    {
        #region 생성자
        public TcpIpClientMachineIf(MachineIfSetting machineIfSetting, IProtocol protocol = null) : base(machineIfSetting, protocol)
        {
            if (protocol == null)
            {
                var stxEtxProtocol = new StxEtxProtocol();
                stxEtxProtocol.StartChar = Encoding.ASCII.GetBytes("<START>");
                stxEtxProtocol.EndChar = Encoding.ASCII.GetBytes("<END>");

                Protocol = stxEtxProtocol;
            }

            if (machineIfSetting is TcpIpMachineIfSetting tcpipMachineIfsetting)
            {
                ClientSocket = new ClientSocket(tcpipMachineIfsetting.TcpIpInfo, protocol);
                ClientSocket.ServerConnected = ClientSocket_Conncted;
                ClientSocket.ServerDisconnected = ClientSocket_Disconncted;
                ClientSocket.DataReceived += ClientSocket_DataReceived;
            }
        }
        #endregion


        #region 속성
        public ClientSocket ClientSocket { get; protected set; }

        public override object HostInfo => ClientSocket.Connected ? ClientSocket.TcpIpInfo : null;
        #endregion


        #region 메서드
        protected void ClientSocket_Conncted(TcpIpInfo tcpIpInfo)
        {
            OnConnect(true, tcpIpInfo);
        }

        protected void ClientSocket_Disconncted(TcpIpInfo tcpIpInfo)
        {
            OnConnect(false, tcpIpInfo);
        }

        protected void ClientSocket_DataReceived(ReceivedPacket receivedPacket)
        {
            //receivedPacket.ReceivedData = Encoding.Default.GetString(receivedPacket.ReceivedDataByte);
            //LogHelper.Debug(LoggerType.Comm, string.Format("receivedPacket - {0}", receivedPacket.ReceivedData));

            ExecuteCommand(string.Format("{0}", receivedPacket.ReceivedData));
        }
        #endregion
    }

    public partial class TcpIpClientMachineIf : MachineIf
    {
        #region 메서드
        public override void Start()
        {
            ClientSocket.Connect();
        }

        public override void Stop()
        {
            ClientSocket.Disconnect();
        }

        public override bool IsStarted()
        {
            return ClientSocket.Connected;
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            if (Protocol == null)
            {
                return false;
            }

            string packetString = MakePacket(itemInfo, args);
            if (string.IsNullOrEmpty(packetString))
            {
                return false;
            }

            // ClientSocket 내부에서 엔코딩을 해주므로 여기서는 하지 않음
            return ClientSocket.SendMessage(packetString);
        }
        #endregion
    }

    public partial class TcpIpServerMachineIf
    {
        #region 생성자
        public TcpIpServerMachineIf(MachineIfSetting machineIfSetting, IProtocol protocol = null) : base(machineIfSetting, protocol)
        {
            if (protocol == null)
            {
                var stxEtxProtocol = new StxEtxProtocol();
                stxEtxProtocol.StartChar = Encoding.ASCII.GetBytes("<START>");
                stxEtxProtocol.EndChar = Encoding.ASCII.GetBytes("<END>");

                protocol = stxEtxProtocol;
            }
            if (machineIfSetting is TcpIpMachineIfSetting tcpipMachineIfsetting)
            {
                ServerSocket = new ServerSocket(tcpipMachineIfsetting.TcpIpInfo, protocol);
                ServerSocket.ClientConnected += ServerSocket_Connected;
                ServerSocket.ClientDisconnected += ServerSocket_Disconnected;
                ServerSocket.DataReceived += ServerSocket_DataReceived;
            }
        }
        #endregion


        #region 속성
        public ServerSocket ServerSocket { get; }
        #endregion


        #region 메서드
        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            if (Protocol == null)
            {
                return false;
            }

            string packetString = MakePacket(itemInfo, args);
            if (string.IsNullOrEmpty(packetString))
            {
                return false;
            }

            // ServerSocket 내부에서 엔코딩을 해주므로 여기서는 하지 않음
            return ServerSocket.SendMessage(packetString);
        }

        protected void ServerSocket_DataReceived(ReceivedPacket receivedPacket)
        {
            if (receivedPacket.ReceivedDataByte == null)
            {
                return;
            }

            receivedPacket.ReceivedData = Encoding.Default.GetString(receivedPacket.ReceivedDataByte);
            LogHelper.Debug(LoggerType.Comm, string.Format("receivedPacket - {0}", receivedPacket.ReceivedData));

            ExecuteCommand(receivedPacket.ReceivedData);
        }

        protected void ServerSocket_Disconnected(ClientHandlerSocket clientHandlerSocket)
        {
            var info = new TcpIpInfo();
            info.IpAddress = clientHandlerSocket.GetIpAddress();
            OnConnect(false, info);
            //ExecuteCommand(String.Format("{0},{1},{2}", 0, 2000, clientHandlerSocket.GetIpAddress()));

            //System.Diagnostics.Debug.WriteLine("TcpIpMachineIf : ServerSocket_Disconnected()");
        }

        protected void ServerSocket_Connected(ClientHandlerSocket clientHandlerSocket)
        {
            var info = new TcpIpInfo();
            info.IpAddress = clientHandlerSocket.GetIpAddress();
            OnConnect(true, info);

            //System.Diagnostics.Debug.WriteLine("TcpIpMachineIf : ServerSocket_Connected()");
        }
        #endregion
    }

    public partial class TcpIpServerMachineIf : MachineIf
    {
        #region 메서드
        public override void Start()
        {
            ServerSocket.Connect();
        }

        public override void Stop()
        {
            ServerSocket.StopListening();
        }

        public override bool IsStarted()
        {
            return ServerSocket.IsConnected();
        }
        #endregion
    }
}
