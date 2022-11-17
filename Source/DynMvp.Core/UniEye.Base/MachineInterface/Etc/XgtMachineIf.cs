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
using UniEye.Base.MachineInterface.Melsec;

namespace UniEye.Base.MachineInterface
{
    public interface IXgtMachineIfExtender
    {
        XgtMachineIf MachineIf { get; set; }
        void RequestMachineState();
        void WriteVisionState();
        void DataReceived(ReceivedPacket receivedPacket);
    }

    public class XgtMachineIfExtender : IXgtMachineIfExtender
    {
        private MelsecQSendPacket lastReadRequestPacket;
        private bool sentOnce = false;
        private byte[] lastStateData = new byte[2] { 0, 0 };
        private bool onDataReceived = false;
        public XgtMachineIf MachineIf { get; set; }

        public void RequestMachineState()
        {
            if (onDataReceived == true)
            {
                return;
            }

            lastReadRequestPacket = MelsecQPacketBuilder.Instance().CreateSendPacket(MachineIfItem.MachineState);
            if (lastReadRequestPacket != null)
            {
                MachineIf.ClientSocket.SendPacket(lastReadRequestPacket);
            }
        }

        public void WriteVisionState()
        {
            var state = SystemState.Instance();

            byte[] stateData = new byte[2] { 0, 0 };

            stateData[1] = (byte)((state.IsVisionAlive() ? 0x01 : 0) | (state.IsReady() ? 0x02 : 0) | (state.IsBusy() ? 0x04 : 0) |
                                    (state.IsComplete() ? 0x08 : 0) | (state.IsComplete() ? 0x10 : 0));

            bool statusChanged = lastStateData[0] != stateData[0] || lastStateData[1] != stateData[1];
            if (sentOnce == false || statusChanged)
            {
                sentOnce = true;

                LogHelper.Debug(LoggerType.Comm, "Write Status : " + stateData[0].ToString("X") + stateData[1].ToString("X"));

                MelsecQSendPacket statePacket = MelsecQPacketBuilder.Instance().CreateSendPacket(MachineIfItem.VisionState);
                if (statePacket != null)
                {
                    statePacket.AddData(stateData);
                    MachineIf.ClientSocket.SendPacket(statePacket);
                }

                lastStateData[0] = stateData[0];
                lastStateData[1] = stateData[1];
            }
        }

        // 수신된 PLC 정보를 이용하여 상태 정보 ( MachineIfState )를 갱신하거나, CommandHandler를 수행한다.
        public void DataReceived(ReceivedPacket receivedPacket)
        {
            onDataReceived = true;

            var melsecReceivedPacket = (MelsecQReceivedPacket)receivedPacket;
            if (lastReadRequestPacket.AddressName != MelsecQPacketBuilder.Instance().GetAddressName(MachineIfItem.MachineState))
            {
                return;
            }

            var state = SystemState.Instance();

            bool busy = state.IsBusy();

            bool machineAlive = melsecReceivedPacket.GetBit(0);
            state.SetMachineAlive(machineAlive);

            bool trigger = melsecReceivedPacket.GetBit(1);

            if (busy)
            {
                bool done = melsecReceivedPacket.GetBit(2);
                if (done)
                {
                    MachineIf.ExecuteCommand("DONE");
                }

                bool cancel = melsecReceivedPacket.GetBit(3);
                if (cancel)
                {
                    MachineIf.ExecuteCommand("CANCEL");
                }
            }
            else
            {
                if (trigger)
                {
                    MachineIf.ExecuteCommand("TRIG");
                }
            }

            onDataReceived = false;
        }
    }

    public class XgtMachineIf : MachineIf
    {
        private const int COMM_DELAY = 50;
        private const int POOLING_INTERVAL = 10;
        private const int COMM_TIMEOUT = 1000;

        // Read Task
        private bool stopPlcMonitor;
        private Task plcMonitor;
        public ClientSocket ClientSocket { get; private set; }

        private bool stateCheck = true;
        private Queue<MelsecQSendPacket> sendPacketQueue = new Queue<MelsecQSendPacket>();
        private ManualResetEvent sendPacketExistEvent = new ManualResetEvent(false);
        private MelsecQAsciiProtocol protocol;
        private IXgtMachineIfExtender ifExtender = null;

        public int RetryCount { get; set; } = 0;

        public XgtMachineIf(MachineIfSetting machineIfSetting, IProtocol protocol) : base(machineIfSetting, protocol)
        {
            var defaultIfExtender = new XgtMachineIfExtender();
            defaultIfExtender.MachineIf = this;

            ifExtender = defaultIfExtender;
        }

        public override void Start()
        {
            var melsecReadInfo = new MelsecInfo();

            if (melsecReadInfo.TcpIpInfo.IpAddress == null || melsecReadInfo.TcpIpInfo.IpAddress == "")
            {
                return;
            }

            protocol = new MelsecQAsciiProtocol(melsecReadInfo);
            ClientSocket = new ClientSocket(melsecReadInfo.TcpIpInfo, protocol);
            ClientSocket.Connect();

            Thread.Sleep(1000);

            ClientSocket.DataReceived = clientSocket_DataReceived;

            stopPlcMonitor = false;
            plcMonitor = new Task(new Action(PlcMonitorProc), TaskCreationOptions.LongRunning);
            plcMonitor.Start();
        }

        public override void Stop()
        {
            stopPlcMonitor = true;
            plcMonitor.Wait(2000);

            ClientSocket.Disconnect();
        }

        public override bool IsStarted()
        {
            throw new NotImplementedException();
        }

        public void SetIfExtender(IXgtMachineIfExtender ifExtender)
        {
            this.ifExtender = ifExtender;
        }

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

            lock (sendPacketQueue)
            {
                sendPacketQueue.Enqueue(writePacket);
                sendPacketExistEvent.Set();
            }
        }

        private void PlcMonitorProc()
        {
            if (ClientSocket == null)
            {
                LogHelper.Warn(LoggerType.Comm, "MelsecMachineIf.PlcMonitorProc : ClientSocket is null");
                return;
            }

            var bt = new BlockTracer("MelsecMachineIf.PlcMonitorProc");

            while (stopPlcMonitor == false)
            {
                try
                {
                    if (ClientSocket.WaitIdle(COMM_TIMEOUT) == false)
                    {
                        LogHelper.Warn(LoggerType.Comm, "MelsecMachineIf.PlcMonitorProc : Communication is timeout. Send next packet.");
                    }

                    if (sendPacketExistEvent.WaitOne(POOLING_INTERVAL) || sendPacketQueue.Count > 0)
                    {
                        MelsecQSendPacket writePacket = null;
                        lock (sendPacketQueue)
                        {
                            writePacket = sendPacketQueue.Dequeue();
                        }

                        ClientSocket.SendPacket(writePacket);
                    }
                    else
                    {
                        if (stateCheck)
                        {
                            ifExtender?.RequestMachineState();
                        }
                        else
                        {
                            ifExtender?.WriteVisionState();
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
        private void clientSocket_DataReceived(ReceivedPacket receivedPacket)
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

            ifExtender?.DataReceived(receivedPacket);
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
