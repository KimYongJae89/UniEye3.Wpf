using DynMvp.Base;
using DynMvp.Devices.Comm;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.Melsec;
using UniEye.Base.MachineInterface.TcpIp;

namespace UniEye.Base.MachineInterface.RabbitMQ
{
    public partial class RabbitMQMachineIf
    {
        #region 생성자
        public RabbitMQMachineIf(TcpIpMachineIfSetting tcpIpMachineIfSetting, string username = "guest", string password = "guest") : base(tcpIpMachineIfSetting, null)
        {
            // RabbitMQ에서 따로 DataReceived를 실행 하므로 기존 함수는 사용하지 않음
            ClientSocket.DataReceived = null;

            ConnectionFactory = new ConnectionFactory()
            {
                HostName = tcpIpMachineIfSetting.TcpIpInfo.IpAddress,
                UserName = username,
                Password = password
            };

            Protocol = null;
        }
        #endregion


        #region 속성
        public DataReceivedDelegate DataReceived { get; set; }

        public List<string> ExchangeList { get; } = new List<string>();

        public List<string> RecvTopicList { get; } = new List<string>();

        public List<string> SendTopicList { get; } = new List<string>();

        public bool IsAsync { get; set; } = false;

        private object ReconnectLock { get; set; } = new object();

        private ConnectionFactory ConnectionFactory { get; set; }

        private IConnection Connection { get; set; }

        private IModel Channel { get; set; }

        private EventingBasicConsumer Consumer { get; set; }
        #endregion


        #region 메서드
        public void AddReceiveTopic(string topic)
        {
            if (!RecvTopicList.Exists(x => x == topic))
            {
                RecvTopicList.Add(topic);
            }
        }

        public void ResetReceiveTopic()
        {
            RecvTopicList.Clear();
        }

        public void AddSendTopic(string topic)
        {
            if (!SendTopicList.Exists(x => x == topic))
            {
                SendTopicList.Add(topic);
            }
        }

        public void ResetSendTopic()
        {
            SendTopicList.Clear();
        }

        private void Reconnect()
        {
            lock (ReconnectLock)
            {
                try
                {
                    Stop();

                    Connection = ConnectionFactory.CreateConnection();
                    Connection.ConnectionBlocked += Connection_ConnectionBlocked;
                    Connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
                    Connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    Connection.RecoverySucceeded += Connection_RecoverySucceeded;
                    Connection.ConnectionRecoveryError += Connection_ConnectionRecoveryError;

                    Channel = Connection.CreateModel();

                    Consumer = new EventingBasicConsumer(Channel);
                    Consumer.Received += Consumer_Received;
                    Consumer.Registered += Consumer_Registered;
                    Consumer.Unregistered += Consumer_Unregistered;
                    Consumer.ConsumerCancelled += Consumer_ConsumerCancelled;

                    if (ExchangeList.Count == 0)
                    {
                        ExchangeList.Add("topic_logs");
                    }

                    if (ExchangeList.Count > 0)
                    {
                        foreach (string exchange in ExchangeList)
                        {
                            Channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);
                            string queueName = Channel.QueueDeclare().QueueName;

                            foreach (string recvTopic in RecvTopicList)
                            {
                                Channel.QueueBind(queue: queueName, exchange: exchange, routingKey: recvTopic);
                            }

                            Channel.BasicConsume(queue: queueName, autoAck: true, consumer: Consumer);
                        }
                    }
                    //else
                    //{
                    //    Channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
                    //    var queueName = Channel.QueueDeclare().QueueName;

                    //    foreach (var recvTopic in recvTopicList)
                    //        Channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: recvTopic);

                    //    Channel.BasicConsume(queue: queueName, autoAck: true, consumer: Consumer);
                    //}

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    LogHelper.Error(LoggerType.Error, "RabbitMq Connection Error\n" + ex.Message);
                    //MessageBox.Show(null, "RabbitMq Connection Error\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string body = Encoding.UTF8.GetString(e.Body);

            var receivedPacket = new ReceivedPacket();
            receivedPacket.SenderInfo = e.RoutingKey;

            if (Protocol != null)
            {
                var packetBuffer = new PacketBuffer();
                packetBuffer.AppendData(e.Body, e.Body.Length);
                Protocol.DecodePacket(packetBuffer, out receivedPacket);
                if (string.IsNullOrEmpty(receivedPacket.ReceivedData))
                {
                    receivedPacket.ReceivedData = Encoding.UTF8.GetString(receivedPacket.ReceivedDataByte);
                }
            }
            else
            {
                receivedPacket.ReceivedData = body;
            }

            System.Diagnostics.Debug.WriteLine($"RabbitMqttProtocol::Consumer_Received - From: {receivedPacket.SenderInfo}, Data: {receivedPacket.ReceivedData}");
            if (IsAsync)
            {
                IAsyncResult asyncResult = DataReceived?.BeginInvoke(receivedPacket, null, null);
                DataReceived?.EndInvoke(asyncResult);
            }
            else
            {
                DataReceived?.Invoke(receivedPacket);
            }
        }

        private void Connection_ConnectionRecoveryError(object sender, ConnectionRecoveryErrorEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Connection_ConnectionRecoveryError");
        }

        private void Connection_RecoverySucceeded(object sender, EventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Connection_RecoverySucceeded");
        }

        private void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Connection_ConnectionUnblocked");
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Connection_ConnectionShutdown");
        }

        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Connection_ConnectionBlocked");
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Consumer_Registered : " + e.ConsumerTag.ToString());
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Consumer_Unregistered : " + e.ConsumerTag.ToString());
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Network, "Consumer_ConsumerCancelled : " + e.ConsumerTag.ToString());
        }
        #endregion
    }

    public partial class RabbitMQMachineIf : TcpIpClientMachineIf
    {
        #region 메서드
        public override void Start()
        {
            Reconnect();
        }

        public override void Stop()
        {
            if (IsStarted())
            {
                Connection.Close();
                Connection = null;
            }
        }

        public override bool IsStarted()
        {
            return Connection != null && Connection.IsOpen;
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            if (IsStarted() == false)
            {
                return false;
            }

            string packetString = MakePacket(itemInfo, args);
            var packet = new SendPacket(packetString);

            if (ExchangeList.Count > 0)
            {
                foreach (string exchange in ExchangeList)
                {
                    foreach (string topic in SendTopicList)
                    {
                        Channel.BasicPublish(exchange: exchange, routingKey: topic, basicProperties: null, body: packet.Data);
                        LogHelper.Debug(LoggerType.Network, $"RabbitMqttProtocol::SendCommand - To: {topic}, Data: {packetString}");
                    }
                }
            }
            //else
            //{
            //    foreach (var topic in sendTopicList)
            //        Channel.BasicPublish(exchange: "topic_logs", routingKey: topic, basicProperties: null, body: sendPacket);
            //}

            return true;
        }
        #endregion
    }

    public partial class RabbitMQMachineIf : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // 관리되는 상태(관리되는 개체)를 삭제합니다.
                    Stop();
                }

                // 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // 큰 필드를 null로 설정합니다.

                disposedValue = true;
            }
        }

        // 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~RabbitMqttProtocol() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
