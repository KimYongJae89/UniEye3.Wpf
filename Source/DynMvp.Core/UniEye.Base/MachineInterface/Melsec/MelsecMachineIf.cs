using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.TcpIp;

namespace UniEye.Base.MachineInterface.Melsec
{
    public class MelsecQPacketBuilder
    {
        private static MelsecQPacketBuilder _instance;
        public static MelsecQPacketBuilder Instance()
        {
            if (_instance == null)
            {
                _instance = new MelsecQPacketBuilder();
            }

            return _instance;
        }

        private Dictionary<Enum, MelsecMachineIfItemInfo> MachineIfItemInfoList { get; set; } = new Dictionary<Enum, MelsecMachineIfItemInfo>();

        // 기본 인터페이스
        public void BuildCommand() { }

        public void AddMachineIfItemInfo(Enum command, bool use, int waitResponceMs, string address, bool isReadCommand, int sizeWord)
        {
            MachineIfItemInfoList.Add(command, new MelsecMachineIfItemInfo(command, use, waitResponceMs, address, isReadCommand, sizeWord));
        }

        public void AddMachineIfItem(MelsecMachineIfItemInfo melsecMachineIfItemInfo)
        {
            MachineIfItemInfoList.Add(melsecMachineIfItemInfo.Command, melsecMachineIfItemInfo);
        }

        public MelsecQSendPacket CreateSendPacket(MelsecMachineIfItemInfo melsecMachineIfItemInfo)
        {
            var sendPacket = new MelsecQSendPacket();

            if (melsecMachineIfItemInfo.IsReadCommand)
            {
                sendPacket.SetupReadPacket(melsecMachineIfItemInfo.Address, melsecMachineIfItemInfo.SizeWord);
            }
            else
            {
                sendPacket.SetupWritePacket(melsecMachineIfItemInfo.Address);
            }

            return sendPacket;
        }

        public MelsecQSendPacket CreateSendPacket(Enum command)
        {
            if (MachineIfItemInfoList.TryGetValue(command, out MelsecMachineIfItemInfo melsecMachineIfItemInfo))
            {
                return CreateSendPacket(melsecMachineIfItemInfo);
            }

            return null;
        }

        public string GetAddressName(Enum command)
        {
            if (MachineIfItemInfoList.TryGetValue(command, out MelsecMachineIfItemInfo melsecMachineIfItemInfo))
            {
                return melsecMachineIfItemInfo.Address;
            }

            return null;
        }
    }

    public partial class MelsecMachineIf
    {
        #region 필드
        private const int COMM_DELAY = 50;

        private const int POOLING_INTERVAL = 10;

        private const int COMM_TIMEOUT = 1000;
        #endregion


        #region 생성자
        public MelsecMachineIf(MachineIfSetting machineIfSetting, bool usePlcMonitor = false, IProtocol protocol = null) : base(machineIfSetting, protocol)
        {
            UsePlcMonitor = usePlcMonitor;

            if (machineIfSetting is MelsecMachineIfSetting melsecMachineIfSetting)
            {
                MelsecInfo = melsecMachineIfSetting.MelsecInfo;

                ClientSocket.TcpIpInfo = MelsecInfo.TcpIpInfo;
                ClientSocket.DataReceived += ClientSocket_DataReceived;

                if (protocol == null)
                {
                    switch (melsecMachineIfSetting.MelsecMachineIfType)
                    {
                        case EMelsecMachineIfType.Binary: Protocol = new MelsecQBinaryProtocol(MelsecInfo); break;
                        case EMelsecMachineIfType.Ascii: Protocol = new MelsecQAsciiProtocol(MelsecInfo); break;
                        default: Protocol = new MelsecQBinaryProtocol(MelsecInfo); break;
                    }

                    ClientSocket.Protocol = Protocol;
                }
            }
        }
        #endregion


        #region 속성
        private MelsecInfo MelsecInfo { get; }

        public DataReceivedDelegate DataReceived { get; set; }

        // MelsecMachineIf 에서 자체적으로 스레드를 운영하여 데이터를 얻는 방식을 사용할 경우 Flag를 올려준다.
        public bool UsePlcMonitor { get; private set; }

        private bool StopPlcMonitor { get; set; }

        private Task PlcMonitor { get; set; }

        private Queue<MelsecQSendPacket> SendPacketQueue { get; set; } = new Queue<MelsecQSendPacket>();

        private ManualResetEvent SendPacketExistEvent { get; set; } = new ManualResetEvent(false);
        #endregion


        #region 메서드
        public void SendPacket(MelsecQSendPacket writePacket)
        {
            if (ClientSocket == null)
            {
                return;
            }

            if (ClientSocket.Connected == false)
            {
                return;
            }

            if (UsePlcMonitor)
            {
                lock (SendPacketQueue)
                {
                    SendPacketQueue.Enqueue(writePacket);
                    SendPacketExistEvent.Set();
                }
            }
            else
            {
                ClientSocket.SendPacket(writePacket);
            }
        }

        // MelsecMachineIf 에서 자체적으로 스레드를 운영하여 데이터를 얻는 방식을 사용할 경우 쓰는 메서드
        private void PlcMonitorProc()
        {
            if (ClientSocket == null)
            {
                LogHelper.Warn(LoggerType.Comm, "MelsecMachineIf.PlcMonitorProc : ClientSocket is null");
                return;
            }

            var bt = new BlockTracer("MelsecMachineIf.PlcMonitorProc");

            while (StopPlcMonitor == false)
            {
                try
                {
                    if (ClientSocket.WaitIdle(COMM_TIMEOUT) == false)
                    {
                        LogHelper.Warn(LoggerType.Comm, "MelsecMachineIf.PlcMonitorProc : Communication is timeout. Send next packet.");
                    }

                    if (SendPacketExistEvent.WaitOne(POOLING_INTERVAL) && SendPacketQueue.Count > 0)
                    {
                        MelsecQSendPacket writePacket = null;
                        lock (SendPacketQueue)
                        {
                            writePacket = SendPacketQueue.Dequeue();
                        }

                        ClientSocket.SendPacket(writePacket);
                    }
                    else
                    {
                        MelsecQSendPacket lastReadRequestPacket = MelsecQPacketBuilder.Instance().CreateSendPacket(MachineIfItem.MachineState);
                        if (lastReadRequestPacket != null)
                        {
                            SendPacket(lastReadRequestPacket);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Warn(LoggerType.Comm, "Machine IF : Send packet failed." + e.Message);
                }
            }
        }

        // Read
        private new void ClientSocket_DataReceived(ReceivedPacket receivedPacket)
        {
            var lastReceivedPacket = receivedPacket as MelsecQReceivedPacket;
            if (string.IsNullOrEmpty(lastReceivedPacket?.ReceivedData))
            {
                LogHelper.Warn(LoggerType.Comm, "Invalid Received Packet : received data is null");
                return;
            }

            var lastSentPacket = ClientSocket.LastSentPacket as MelsecQSendPacket;
            if (lastSentPacket?.AddressName == null)
            {
                LogHelper.Warn(LoggerType.Comm, "Invalid Received Packet : addressName is null");
                return;
            }

            if (ItemInfoResponce != null || ItemInfoResponce.IsResponced == false)
            {
                ItemInfoResponce.SetRecivedData(lastReceivedPacket.ReceivedData, true, lastReceivedPacket);
            }

            DataReceived?.Invoke(lastReceivedPacket);
        }
        #endregion
    }

    public partial class MelsecMachineIf : TcpIpClientMachineIf
    {
        #region 메서드
        public override void Start()
        {
            if (MelsecInfo == null || MelsecInfo.TcpIpInfo.IpAddress == null || MelsecInfo.TcpIpInfo.IpAddress == "")
            {
                return;
            }

            ClientSocket.Connect();

            if (UsePlcMonitor)
            {
                StopPlcMonitor = false;
                PlcMonitor = new Task(new Action(PlcMonitorProc), TaskCreationOptions.LongRunning);
                PlcMonitor.Start();
            }
        }

        public override void Stop()
        {
            if (UsePlcMonitor)
            {
                StopPlcMonitor = true;
                PlcMonitor.Wait(2000);
            }

            ClientSocket.Disconnect();
        }

        public override bool IsStarted()
        {
            return ClientSocket.IsConnected();
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            if (Protocol == null)
            {
                return false;
            }

            if (itemInfo is MelsecMachineIfItemInfo melsecMachineIfItemInfo)
            {
                MelsecQSendPacket sendPacket = MelsecQPacketBuilder.Instance().CreateSendPacket(melsecMachineIfItemInfo);
                foreach (string arg in args)
                {
                    sendPacket.AppendData(arg);
                }
                // ClientSocket 내부에서 엔코딩을 해주므로 여기서는 하지 않음
                return ClientSocket.SendPacket(sendPacket);
            }

            return false;
        }
        #endregion
    }
}
