using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Comm
{
    public delegate void SocketClosedDelegate(ClientHandlerSocket clientHandlerSocket);

    public class ClientHandlerSocket
    {
        private Socket handlerSocket;
        private Task listeningTask;
        private Task disconnectTask;
        private IProtocol protocol;
        private bool activeReceived = true;
        private PacketBuffer packetBuffer = new PacketBuffer();
        private CancellationTokenSource cancellationTokenSource;

        public SocketClosedDelegate SocketClosed;
        public DataReceivedDelegate DataReceived;
        private object lockObject = new object();

        public ClientHandlerSocket(Socket handlerSocket, IProtocol protocol)
        {
            this.handlerSocket = handlerSocket;
            this.protocol = protocol;

            cancellationTokenSource = new CancellationTokenSource();
            listeningTask = Task.Run(new Action(ListeningProc), cancellationTokenSource.Token);
        }

        public void Start()
        {
            //listeningTask.Wait(2000);
            activeReceived = true;
        }

        public void Stop()
        {
            //listeningTask.Wait(2000);
            activeReceived = false;
        }

        public string GetIpAddress()
        {
            if (handlerSocket.Connected)
            {
                return handlerSocket.RemoteEndPoint.ToString().Split(':')[0];
            }

            return "0.0.0.0";
        }

        public void SendCommand(SendPacket sendPacket)
        {
            if (handlerSocket != null && handlerSocket.Connected == true)
            {
                byte[] sendData = protocol.EncodePacket(sendPacket);
                handlerSocket.Send(sendData);

                LogHelper.Debug(LoggerType.Comm, "Send Command : " + sendData.ToString());
                //handlerSocket.Send(sendPacket.Data);
            }
        }

        public void SendCommand(byte[] commandPacket)
        {
            if (handlerSocket != null)
            {
                handlerSocket.Send(commandPacket);
            }
        }

        private void ListeningProc()
        {
            try
            {
                byte[] Buffer = new byte[10];

                while (true)
                {
                    if (handlerSocket.Connected == true)
                    {
                        packetBuffer.Clear();
                        handlerSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, SocketDataReceived, Buffer);
                        break;
                    }

                    Thread.Sleep(10);
                }

                disconnectTask = Task.Run(new Action(DisconnectProc));
            }
            catch (SocketException e)
            {
                LogHelper.Error("Socket Exception ListeningProc: " + e.Message);
            }
        }

        private void DisconnectProc()
        {
            try
            {
                while (true)
                {
                    bool connected = !(handlerSocket.Poll(10, SelectMode.SelectRead) && handlerSocket.Available == 0);
                    if (!connected)
                    {
                        lock (lockObject)
                        {
                            SocketClosed?.Invoke(this);

                            handlerSocket.Shutdown(SocketShutdown.Both);
                            handlerSocket.Close();

                            //System.Diagnostics.Debug.WriteLine("ClientHandlerSocket : DisconnectProc()");
                        }
                        break;
                    }

                    Thread.Sleep(10);
                }
            }
            catch (SocketException e)
            {
                LogHelper.Error("Socket Exception - DisconnectProc: " + e.Message);
            }
        }

        private void SocketDataReceived(IAsyncResult ar)
        {
            try
            {
                if (!(ar.AsyncState is byte[] dataBuffer))
                {
                    return;
                }

                if (activeReceived)
                {
                    lock (lockObject)
                    {
                        if (!handlerSocket.Connected)
                        {
                            return;
                        }

                        int byteSize = handlerSocket.EndReceive(ar);
                        if (byteSize > 0)
                        {
                            //ReceivedPacket packetBuffer = new ReceivedPacket();
                            //packetBuffer.ReceivedDataByte = ao.Buffer;
                            //packet.ReceivedData = ASCIIEncoding.ASCII.GetString(ao.Buffer).TrimEnd(new char[] { (char)0 });

                            packetBuffer.AppendData(dataBuffer, byteSize);


                            DecodeResult decodeResult = protocol.DecodePacket(packetBuffer, out ReceivedPacket receivedPacket);
                            if (decodeResult == DecodeResult.Complete)
                            {
                                //packetBuffer.Clear();
                                receivedPacket.SenderInfo = GetIpAddress();
                                DataReceived?.Invoke(receivedPacket);
                            }
                        }
                    }
                }

                handlerSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, SocketDataReceived, dataBuffer);
            }
            catch (SocketException e)
            {
                LogHelper.Error("Socket Exception - SocketDataReceived: " + e.Message);
            }
        }
    }
}
