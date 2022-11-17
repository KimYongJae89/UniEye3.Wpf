using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Comm
{
    public delegate void SocketEventDelegate(TcpIpInfo hostInfo);

    public partial class ClientSocket
    {
        #region 생성자
        public ClientSocket(TcpIpInfo tcpIpInfo, IProtocol protocol)
        {
            TcpIpInfo = tcpIpInfo;
            Protocol = protocol;
            ReceiveBuffer = new byte[MAXSIZE];

            OnIdleEvent = new ManualResetEvent(true);
            LastSentPacket = null;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.NoDelay = true;
        }

        ~ClientSocket()
        {
            Disconnect();
        }
        #endregion


        #region 속성
        public TcpIpInfo TcpIpInfo { get; set; }

        public IProtocol Protocol { get; set; }

        public DataReceivedDelegate DataReceived { get; set; }

        public SocketEventDelegate ServerConnected { get; set; }

        public SocketEventDelegate ServerDisconnected { get; set; }

        public SendPacket LastSentPacket { get; set; } = null;

        public bool Connected => Socket != null && Socket.Connected;

        private Socket Socket { get; set; } = null;

        private PacketBuffer PacketBuffer { get; set; } = new PacketBuffer();

        private Thread ConnectionThread { get; set; }

        private bool StopConnectionThreadFlag { get; set; } = false;

        private ManualResetEvent OnIdleEvent { get; set; }

        private byte[] ReceiveBuffer { get; set; }

        private const int MAXSIZE = 8192;
        #endregion


        #region 메서드
        public bool WaitIdle(int timeout)
        {
            return OnIdleEvent.WaitOne(timeout);
        }

        public void ResetProcess()
        {
            LastSentPacket = null;
            OnIdleEvent.Set();
        }

        public void ClearBuffer()
        {
            if (Socket != null)
            {
                int dataAvailable = Socket.Available;

                if (dataAvailable > 0)
                {
                    byte[] receiveBuf = new byte[dataAvailable];
                    Socket.Receive(receiveBuf);
                }
            }
        }

        public bool SendPacket(SendPacket sendPacket)
        {
            Debug.Assert(sendPacket != null);

            try
            {
                Thread.Sleep(10);
                if (sendPacket == null)
                {
                    return false;
                }

                byte[] sendData = Protocol.EncodePacket(sendPacket);
                Socket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(SendCallBack), sendData);

                LastSentPacket = sendPacket;
                OnIdleEvent.Reset();

                return true;
            }
            catch (SocketException se)
            {
                LogHelper.Error("Fail to send data" + se.Message);
                return false;
            }
        }

        public void Receive()
        {
            Socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None,
                                 new AsyncCallback(OnReceiveCallBack), Socket);
        }

        public void WaitSendPacketEmpty()
        {

        }

        public bool IsPingAlive()
        {
            var pingSender = new Ping();
            try
            {
                PingReply reply = pingSender.Send(TcpIpInfo.IpAddress);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ConnectionProc()
        {
            LogHelper.Debug(LoggerType.Operation, "SinglePortSocket::ConnectionProc - Start");
            while (!StopConnectionThreadFlag)
            {
                if (Socket == null || Socket.Connected == false)
                {
                    try
                    {
                        Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        bool ok = IPAddress.TryParse(TcpIpInfo.IpAddress, out IPAddress ipAddr);
                        if (ok == false)
                        {
                            continue;
                        }

                        var ipEndPoint = new IPEndPoint(ipAddr, TcpIpInfo.PortNo);
                        Socket.Connect(ipEndPoint);

                        StopConnectionThreadFlag = true;

                        ConnectCallback();

                        LogHelper.Debug(LoggerType.Network, "Socket Connected");
                    }
                    catch (SocketException ex)
                    {
                        LogHelper.Debug(LoggerType.Error, "Socket Error : " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        SendPacket(new Comm.SendPacket(""));
                    }
                    catch (SocketException ex)
                    {
                        LogHelper.Debug(LoggerType.Error, "Socket Error : " + ex.Message);
                        //throw ex;
                    }
                }

                Thread.Sleep(100);
            }

            ConnectionThread = null;
            LogHelper.Debug(LoggerType.Operation, "SinglePortSocket::ConnectionProc - End");
        }

        private void ConnectCallback()
        {
            try
            {
                Console.WriteLine("Socket connected to {0}", Socket.RemoteEndPoint.ToString());

                ReceiveBuffer = new byte[MAXSIZE];

                Socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None,
                                    new AsyncCallback(OnReceiveCallBack), Socket);

                ServerConnected?.Invoke(TcpIpInfo);
            }
            catch (Exception e)
            {
                LogHelper.Error("Can't connect the server" + e.Message);
            }
        }

        private void SendCallBack(IAsyncResult IAR)
        {
            if (IAR.IsCompleted == true)
            {
                byte[] message = (byte[])IAR.AsyncState;

                string fullString = Encoding.UTF8.GetString(message);
                int strLen = Math.Min(fullString.Length, 100);

                //LogHelper.Debug(LoggerType.Comm, String.Format("Packet Sent : {0}...", fullString.Substring(0, strLen)));
            }
            else
            {
                LogHelper.Debug(LoggerType.Comm, "Packet Sent Error");
            }
        }

        private void OnReceiveCallBack(IAsyncResult iAr)
        {
            try
            {
                var tempSock = (Socket)iAr.AsyncState;
                if (tempSock.Connected)
                {
                    int dataAvailable = tempSock.EndReceive(iAr);
                    if (dataAvailable != 0)
                    {
                        int dataRead = Math.Min(dataAvailable, MAXSIZE);

                        string message = new UTF8Encoding().GetString(ReceiveBuffer, 0, dataAvailable);
                        //LogHelper.Debug(LoggerType.Comm, "Data received : " + message);

                        if (Protocol != null)
                        {
                            PacketBuffer.AppendData(ReceiveBuffer, dataAvailable);

                            DecodeResult result;

                            do
                            {
                                result = Protocol.DecodePacket(PacketBuffer, out ReceivedPacket receivedPacket);
                                if (result == DecodeResult.Complete)
                                {
                                    if (DataReceived != null && receivedPacket.Valid)
                                    {
                                        DataReceived(receivedPacket);

                                        LastSentPacket = null;
                                        OnIdleEvent.Set();
                                    }
                                }

                                if (PacketBuffer.Empty == true)
                                {
                                    break;
                                }
                            }
                            while (result == DecodeResult.Complete);
                        }
                    }

                    Receive();
                }
            }
            catch (SocketException)
            {
                LogHelper.Debug(LoggerType.Operation, "OnReceiveCallBack Reconnect.");
            }
        }
        #endregion
    }

    public partial class ClientSocket : ITcpIp
    {
        #region 메서드
        public virtual void Connect()
        {
            LogHelper.Debug(LoggerType.Operation, "SinglePortSocket::StartonnectionThread");
            if (ConnectionThread != null)
            {
                return;
            }

            StopConnectionThreadFlag = false;
            ConnectionThread = new Thread(new ThreadStart(ConnectionProc));
            ConnectionThread.IsBackground = true;
            ConnectionThread.Start();
        }

        public virtual void Disconnect()
        {
            LogHelper.Debug(LoggerType.Network, "Begin Close Socket");

            if (Socket != null)
            {
                try
                {
                    StopConnectionThreadFlag = true;

                    if (Socket.Connected == true)
                    {
                        LogHelper.Debug(LoggerType.Network, "Shutdown Socket");
                        Socket.Shutdown(SocketShutdown.Both);
                    }

                    Socket.Close();
                    Socket.Dispose();

                    Socket = null;

                    ServerDisconnected?.Invoke(TcpIpInfo);

                    LogHelper.Debug(LoggerType.Network, "Socket Closed");
                }
                catch (SocketException se)
                {
                    LogHelper.Error("Fail to close the socket" + se.Message);
                }

                Socket = null;
            }
        }

        public virtual bool IsConnected()
        {
            if (Socket == null)
            {
                return false;
            }

            bool result = Socket.Poll(10, SelectMode.SelectRead) && (Socket.Available == 0);
            bool result2 = Socket.Poll(10, SelectMode.SelectWrite) && (Socket.Available == 0);

            return (result || result2);
        }

        public virtual bool SendMessage(string message)
        {
            var packet = new SendPacket(message);
            return SendMessage(packet);
        }

        public virtual bool SendMessage(byte[] bytes)
        {
            var packet = new SendPacket(bytes);
            return SendMessage(packet);
        }

        public virtual bool SendMessage(SendPacket packet)
        {
            return SendPacket(packet);
        }
        #endregion
    }

    public partial class ClientSocket : IDisposable
    {
        #region 메서드
        public void Dispose()
        {
            Disconnect();
        }
        #endregion
    }

    public delegate void ClientEventDelegate(ClientHandlerSocket clientHandlerSocket);

    public partial class ServerSocket
    {
        #region 생성자
        public ServerSocket(int dataPort, IProtocol protocol = null)
        {
            Setup(dataPort, protocol);
        }

        public ServerSocket(TcpIpInfo tcpIpInfo, IProtocol protocol = null)
        {
            Setup(tcpIpInfo.PortNo, protocol);
        }
        #endregion


        #region 속성
        public TcpIpInfo TcpIpInfo { get; set; }

        public DataReceivedDelegate DataReceived { get; set; }

        public ManualResetEvent allDone { get; set; } = new ManualResetEvent(false);

        public PacketHandler ListeningPacketHandler { get; set; } = new PacketHandler();

        public ClientEventDelegate ClientConnected;

        public ClientEventDelegate ClientDisconnected;

        public List<ClientHandlerSocket> ClientList { get; } = new List<ClientHandlerSocket>();

        private IProtocol protocol { get; set; }

        private bool stopThreadFlag { get; set; } = false;

        private Thread listeningThread { get; set; }

        private Mutex commandMutex { get; set; } = new Mutex();

        private Socket listeningSocket { get; set; } = null;
        #endregion


        #region 메서드
        public void Setup(int dataPort, IProtocol protocol = null)
        {
            TcpIpInfo.PortNo = dataPort;

            if (protocol == null)
            {
                this.protocol = new StxEtxProtocol();
            }
            else
            {
                this.protocol = protocol;
            }
        }

        public void Setup(TcpIpInfo tcpIpInfo)
        {
            TcpIpInfo = tcpIpInfo;
        }

        public void Setup(string host, int dataPort)
        {
            if (string.IsNullOrEmpty(host))
            {
                LogHelper.Debug(LoggerType.Network, "Host address is empty");
                return;
            }

            TcpIpInfo.IpAddress = host;
            TcpIpInfo.PortNo = dataPort;
        }

        public void StopListening()
        {
            stopThreadFlag = true;
            allDone.Set();

            if (listeningSocket != null)
            {
                while (listeningThread != null && listeningThread.IsAlive)
                {
                    Thread.Sleep(10);
                }
            }
        }

        public void Stop()
        {
            StopListening();
            //while(clientList.Count>0)
            //    clientList[0].Stop();
        }

        // 가상 모드의 동작을 위해 별도 함수로 분리
        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return true; //listeningPacketHandler.ProcessPacket(receiveBuf);
        }

        private void ListeningProc()
        {

            while (stopThreadFlag == false)
            {
                try
                {
                    // Accept 실행 중 스레드 묶임 -> stopFlag를 처리 못해 스레드 종료 안됨 -> 프로그램 종료 안 됨
                    Socket handlerSocket = listeningSocket.Accept();
                    Accept(handlerSocket);
                    Thread.Sleep(100);

                    //2
                    //allDone.Reset();

                    //System.Diagnostics.Debug.WriteLine("Server Socket : BeginAccept()");

                    //// Start an asynchronous socket to listen for connections.  
                    //listeningSocket.BeginAccept(
                    //    new AsyncCallback(AcceptCallback),
                    //    listeningSocket);

                    //System.Diagnostics.Debug.WriteLine("Server Socket : Wait Client");

                    //// Wait until a connection is made before continuing.
                    //allDone.WaitOne();

                }
                catch (SocketException)
                {
                    //return;
                }
                finally
                {
                    //Close();
                    // Stop 메서드로 이동
                    //if(listeningSocket.Connected)
                    //    listeningSocket.Shutdown(SocketShutdown.Both);
                    //listeningSocket.Close();
                }
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  

            // Get the socket that handles the client request.
            try
            {
                var listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                Accept(handler);

                allDone.Set();
            }
            catch (Exception)
            {

            }
        }

        private void Accept(Socket socket)
        {
            var clientHandlerSocket = new ClientHandlerSocket(socket, protocol);
            clientHandlerSocket.SocketClosed += clientHandlerSocket_SocketClosed;
            clientHandlerSocket.DataReceived += clientHandlerSocket_DataReceived;

            LogHelper.Debug(LoggerType.Network, string.Format("Client connected : {0}", clientHandlerSocket.GetIpAddress()));

            lock (ClientList)
            {
                ClientList.Add(clientHandlerSocket);
            }

            ClientConnected?.Invoke(clientHandlerSocket);

            //System.Diagnostics.Debug.WriteLine("ServerSocket : Accept()");
        }

        private void clientHandlerSocket_DataReceived(ReceivedPacket receivedPacket)
        {
            DataReceived?.Invoke(receivedPacket);
        }

        private void clientHandlerSocket_SocketClosed(ClientHandlerSocket clientHandlerSocket)
        {
            lock (ClientList)
            {
                ClientList.Remove(clientHandlerSocket);
            }

            ClientDisconnected?.Invoke(clientHandlerSocket);
        }
        #endregion
    }

    public partial class ServerSocket : ITcpIp
    {
        #region 메서드
        public virtual void Connect()
        {
            stopThreadFlag = false;

            try
            {
                IPAddress ipAddr = IPAddress.Any;
                var ipEndPoint = new IPEndPoint(ipAddr, TcpIpInfo.PortNo);

                LogHelper.Debug(LoggerType.Network, string.Format("Server IP : {0}", ipEndPoint.Address));

                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //listeningSocket.Disconnect(true);
                listeningSocket.Bind(ipEndPoint);

                listeningSocket.Listen(50);

                listeningThread = new Thread(new ThreadStart(ListeningProc));
                listeningThread.IsBackground = true;
                listeningThread.Start();
            }
            catch (SocketException e)
            {
                LogHelper.Debug(LoggerType.Network, string.Format("Server Error : {0}", e.Message));
            }
        }

        public virtual void Disconnect()
        {
            if (listeningSocket == null)
            {
                return;
            }

            if (listeningSocket.Connected)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
            }

            listeningSocket.Close();
            listeningSocket = null;
        }

        public virtual bool IsConnected()
        {
            return listeningSocket != null && listeningSocket.Connected;
        }

        public virtual bool SendMessage(string message)
        {
            return SendMessage(Encoding.Default.GetBytes(message));
        }

        public virtual bool SendMessage(byte[] bytes)
        {
            lock (ClientList)
            {
                foreach (ClientHandlerSocket handlerSocket in ClientList)
                {
                    handlerSocket.SendCommand(bytes);
                }
            }

            return true;
        }

        public virtual bool SendMessage(SendPacket packet)
        {
            LogHelper.Debug(LoggerType.Network, "Send Command To All Client : " + packet.ToString());

            lock (ClientList)
            {
                foreach (ClientHandlerSocket handlerSocket in ClientList)
                {
                    handlerSocket.SendCommand(packet);
                }
            }

            return true;
        }
        #endregion
    }

    public class SinglePortSocket
    {
        private TcpIpInfo tcpIpInfo;

        private bool stopConnectionThreadFlag = false;
        private Thread connectionThread;

        private bool stopListeningThreadFlag = false;
        private Thread listeningThread;
        public PacketData PacketData { get; } = new PacketData();
        public PacketHandler PacketHandler { get; set; } = new PacketHandler();

        private IPEndPoint ipEndPoint = null;
        private Mutex commandMutex = new Mutex();
        private Socket clientSocket = null;
        private ManualResetEvent connectAsyncCompleted = new ManualResetEvent(true);
        public bool Connected => clientSocket == null ? false : clientSocket.Connected;

        public SinglePortSocket()
        {

        }

        ~SinglePortSocket()
        {
            Close(false);
        }

        public void Init(TcpIpInfo tcpIpInfo)
        {
            stopConnectionThreadFlag = false;

            this.tcpIpInfo = tcpIpInfo;

            if (tcpIpInfo.IpAddress == null)
            {
                return;
            }

            //1.
            //IPHostEntry ipHost = Dns.GetHostEntry(tcpIpInfo.IpAddress);
            //IPAddress ipAddr = ipHost.AddressList[0];

            //2.
            //IPHostEntry ipHost = Dns.Resolve(tcpIpInfo.IpAddress);
            //IPAddress ipAddr = ipHost.AddressList[0];

            // 3.
            bool ok = IPAddress.TryParse(tcpIpInfo.IpAddress, out IPAddress ipAddr);
            if (ok == false)
            {
                DynMvp.UI.MessageForm.Show(null, "Host Ip is invalid");
                return;
            }

            ipEndPoint = new IPEndPoint(ipAddr, tcpIpInfo.PortNo);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartconnectionThread()
        {
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::StartonnectionThread");
            if (connectionThread != null)
            {
                return;
            }

            stopConnectionThreadFlag = false;
            connectionThread = new Thread(new ThreadStart(ConnectionProc));
            connectionThread.IsBackground = true;
            connectionThread.Start();
        }

        private void ConnectionProc()
        {
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::ConnectionProc - Start");
            while (!stopConnectionThreadFlag)
            {
                if (clientSocket == null || clientSocket.Connected == false)
                {
                    ConnectAsync();
                }

                Thread.Sleep(10000);
            }

            connectionThread = null;
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::ConnectionProc - End");
        }

        public void ConnectAsync()
        {
            if (connectAsyncCompleted.WaitOne(100))
            {
                if (clientSocket == null)
                {
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }

                var e = new SocketAsyncEventArgs();
                e.RemoteEndPoint = ipEndPoint;
                e.Completed += ConnectAsync_Completed;

                connectAsyncCompleted.Reset();
                clientSocket.ConnectAsync(e);
            }
        }

        private void ConnectAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                LogHelper.Debug(LoggerType.Network, string.Format("SocketError - {0}", e.SocketError.ToString()));
                return;
            }

            StartListening();

            connectAsyncCompleted.Set();
        }

        public void Connect()
        {
            if (tcpIpInfo.IpAddress == "" || tcpIpInfo.IpAddress == null)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Network, "Connect Socket");

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(ipEndPoint);
                StartListening();

                LogHelper.Debug(LoggerType.Network, "Socket Connected");
            }
            catch (SocketException ex)
            {
                LogHelper.Error("Socket Error : " + ex.Message);
                //throw ex;
            }
        }

        public void StopConnectionThread()
        {
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::StopConnectionThread");

            if (connectionThread == null)
            {
                return;
            }

            stopConnectionThreadFlag = true;
        }

        public void Close(bool bFinalize)
        {
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::Close");

            if (bFinalize == true)
            {
                StopConnectionThread();
            }

            StopListening();

            if (clientSocket != null)
            {
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
                catch (SocketException)
                {
                }

                clientSocket = null;
            }
        }

        public void ClearBuffer()
        {
            if (clientSocket != null)
            {
                int dataAvailable = clientSocket.Available;

                if (dataAvailable > 0)
                {
                    byte[] receiveBuf = new byte[dataAvailable];
                    clientSocket.Receive(receiveBuf);
                }
            }
        }

        private void StartListening()
        {
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::StartListening");

            stopListeningThreadFlag = false;

            listeningThread = new Thread(new ThreadStart(ListeningProc));
            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

        private void StopListening()
        {
            LogHelper.Debug(LoggerType.Block, "SinglePortSocket::StopListening");

            stopListeningThreadFlag = true;

            if (clientSocket != null)
            {
                if (listeningThread != null)
                {
                    while (listeningThread.IsAlive)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }

        public bool IsMonitoring()
        {
            return (listeningThread != null && listeningThread.IsAlive);
        }

        public void SendCommand(string commandString)
        {
            SendCommand(Encoding.ASCII.GetBytes(commandString));
        }

        public bool SendCommand(byte[] commandPacket)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                {
                    return false;
                }

                bool ok = commandPacket.Length == clientSocket.Send(commandPacket);
                return ok;
            }
            catch (SocketException)
            {
                //Close(false);
                return false;
            }
        }

        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return PacketHandler.ProcessPacket(receiveBuf, PacketData);
        }

        private void ListeningProc()
        {
            const int maxBufferSize = 10240;
            bool packetCompleted = false;

            try
            {
                while (stopListeningThreadFlag == false)
                {
                    do
                    {
                        clientSocket.Send(new byte[0], 0, SocketFlags.None);   // 연결 확인 차원에서 0바이트 보냄.
                        if (clientSocket.Connected == false)
                        {
                            break;
                        }

                        if (stopListeningThreadFlag == true)
                        {
                            break;
                        }

                        if (clientSocket == null) //임시 코드 나중에 break로 처리해야하는 것이 맞음.
                        {
                            break;
                        }

                        int dataAvailable = clientSocket.Available;
                        if (dataAvailable == 0)
                        {
                            break;
                        }

                        int dataToRead = Math.Min(dataAvailable, maxBufferSize);
                        byte[] receiveBuf = new byte[dataToRead];

                        clientSocket.Receive(receiveBuf);

                        packetCompleted = ProcessDataPacket(receiveBuf);

                    } while (packetCompleted);

                    Thread.Sleep(10);
                }
            }
            catch (SocketException ex)
            {
                LogHelper.Error("Socket Error : " + ex.Message);
            }
            finally
            {
                if (clientSocket != null)
                {
                    try
                    {
                        clientSocket.Shutdown(SocketShutdown.Both); // clientSocket이 null이 아니더라도 문제가 발생될수 있음
                        //clientSocket.Disconnect(true);
                        clientSocket.Close();
                    }
                    catch (SocketException)
                    {
                    }

                    clientSocket = null;
                    stopListeningThreadFlag = true;
                }
            }
        }
    }
}
