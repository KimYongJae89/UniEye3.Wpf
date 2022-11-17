using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Comm
{
    public abstract class PacketParser
    {
        public IProtocol Protocol { get; set; }
        public bool PacketValid { get; set; } = false;
        public bool CommandPass { get; set; } = false;
        public bool ProcessDone { get; set; }
        public string ErrorMessage { get; set; }

        public virtual byte[] EncodePacket(string packetString)
        {
            byte[] packet;

            if (Protocol != null)
            {
                var sendPacket = new SendPacket(packetString);
                packet = Protocol.EncodePacket(sendPacket);
            }
            else
            {
                packet = Encoding.ASCII.GetBytes(packetString);
            }

            return packet;
        }

        private PacketBuffer packetBuffer = new PacketBuffer();

        public virtual string DecodePacket(byte[] packet)
        {
            string packetString = string.Empty;

            if (packet != null)
            {
                if (Protocol != null)
                {
                    packetBuffer.AppendData(packet, packet.Length);


                    DecodeResult decodeResult = Protocol.DecodePacket(packetBuffer, out ReceivedPacket receivedPacket);
                    if (decodeResult == DecodeResult.Complete)
                    {
                        packetString = Encoding.ASCII.GetString(receivedPacket.ReceivedDataByte);
                    }
                    else
                    {
                        packetString = "";
                    }
                }
                else
                {
                    packetString = Encoding.Default.GetString(packet);
                }
            }

            return packetString;
        }

        public DataReceivedDelegate DataReceived;

        public abstract byte[] GetRequestPacket();
        public virtual byte[] GetRequestPacket(int index)
        {
            return null;
        }
        public DataReceivedDelegate OnDataReceived { get; set; }

        public virtual byte[] GetResponsePacket()
        {
            return null;
        }

        public abstract bool ParsePacket(byte[] PacketContents);

        public bool WaitResponse(int timeoutMs)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < timeoutMs && ProcessDone == false)
            {
                Thread.Sleep(10);
            }

            return ProcessDone;
        }

        public virtual PacketParser Clone()
        {
            return null;
        }
    }

    public class SimplePacketParser : PacketParser
    {
        private string message;

        protected byte[] startChar = null;
        public byte[] StartChar
        {
            get => startChar;
            set => startChar = value;
        }

        protected byte[] endChar = null;
        public byte[] EndChar
        {
            get => endChar;
            set => endChar = value;
        }

        public SimplePacketParser()
        {
            var stxEtxProtocol = new StxEtxProtocol();
            stxEtxProtocol.EndChar = new byte[2] { (byte)'\r', (byte)'\n' };

            Protocol = stxEtxProtocol;
        }

        public SimplePacketParser(string message)
        {
            this.message = message;
        }

        public override byte[] GetRequestPacket()
        {
            if (string.IsNullOrEmpty(message))
            {
                return new byte[0];
            }
            else
            {
                return Encoding.ASCII.GetBytes(message);
            }
        }

        public override bool ParsePacket(byte[] PacketContents)
        {
            string currentString = System.Text.Encoding.Default.GetString(PacketContents.ToArray());

            if (DataReceived != null)
            {
                var receivedPacket = new ReceivedPacket();
                receivedPacket.ReceivedData = currentString;
                DataReceived(receivedPacket);
            }

            return true;
        }
    }

    public class PacketData
    {
        public byte[] DataByteFull { get; set; } = null;
        public byte[] PacketContents { get; set; } = null;

        public string GetContentsString()
        {
            return System.Text.Encoding.Default.GetString(PacketContents);
        }
    }

    public class PacketHandler
    {
        public byte[] StartChar { get; set; } = null;
        public byte[] EndChar { get; set; } = null;
        public byte[] BufferByte { get; set; } = null;
        public bool UseChecksum { get; set; }
        public int ChecksumSize { get; set; }
        public string Name { get; set; }
        public PacketParser PacketParser { get; set; } = null;
        public PacketParser ListenPacketParser { get; set; } = null;

        public PacketHandler(PacketParser packetParser = null)
        {
            if (packetParser == null)
            {
                packetParser = new SimplePacketParser();
            }

            PacketParser = packetParser;
        }
        public bool UseLogReceivedPacket { get; set; } = false;

        private int IndexOf(byte[] dataByte, byte[] searchByte)
        {
            for (int dataIndex = 0; dataIndex < dataByte.Count(); dataIndex++)
            {
                if (dataByte[dataIndex] != searchByte[0])
                {
                    continue;
                }

                if (dataByte.Count() - dataIndex < searchByte.Count())
                {
                    return -1;
                }

                bool found = true;
                for (int searchIndex = 0; searchIndex < searchByte.Count(); searchIndex++)
                {
                    if (searchByte[searchIndex] != dataByte[dataIndex + searchIndex])
                    {
                        dataIndex += searchIndex;
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return dataIndex;
                }
            }

            return -1;
        }

        // The Mutex object that will protect image objects during processing
        private Mutex imageMutex = new Mutex();

        public bool ProcessPacket(byte[] dataByte, PacketData packetData)
        {
            bool pakcetCompleted = false;

            if (PacketParser == null && ListenPacketParser == null)
            {
                return true;
            }

            imageMutex.WaitOne();

            //LogHelper.Debug(LoggerType.Inspection, "Begin ProcessPacket");
            if (BufferByte == null)
            {
                BufferByte = new byte[0];
            }

            byte[] newBufferByte = new byte[BufferByte.Length + dataByte.Length];
            System.Buffer.BlockCopy(BufferByte, 0, newBufferByte, 0, BufferByte.Length);
            System.Buffer.BlockCopy(dataByte, 0, newBufferByte, BufferByte.Length, dataByte.Length);
            BufferByte = newBufferByte;

            LogHelper.Debug(LoggerType.Comm, string.Format("Start ProcessPacket - Buffer : {0}", Encoding.UTF8.GetHashCode().ToString()));

            //if (dataByteFull != null)
            //{
            //    byte[] newDataByteFull = new byte[dataByteFull.Length + dataByte.Length];
            //    System.Buffer.BlockCopy(dataByteFull, 0, newDataByteFull, 0, dataByteFull.Length);
            //    System.Buffer.BlockCopy(dataByte, 0, newDataByteFull, dataByteFull.Length, dataByte.Length);

            //    packetData.DataByteFull = newDataByteFull;
            //    dataByteFull = newDataByteFull;
            //}
            //else
            //{
            //    dataByteFull = new byte[dataByte.Length];
            //    System.Buffer.BlockCopy(dataByte, 0, dataByteFull, 0, dataByte.Length);

            //    packetData.DataByteFull = dataByteFull;
            //}

            if (StartChar == null && EndChar == null)
            {
                if (PacketParser != null)
                {
                    pakcetCompleted = PacketParser.ParsePacket(BufferByte);
                }

                if (pakcetCompleted == false)
                {
                    pakcetCompleted = ListenPacketParser.ParsePacket(BufferByte);
                }

                if (pakcetCompleted == true)
                {
                    BufferByte = null;
                }
            }
            else if (EndChar != null)
            {
                int endPos = IndexOf(BufferByte, EndChar);

                while (endPos > -1)
                {
                    LogHelper.Debug(LoggerType.Inspection, string.Format("End Found : {0}", endPos));

                    int startPos = 0;
                    if (StartChar != null)
                    {
                        startPos = IndexOf(BufferByte, StartChar);
                    }

                    if (UseLogReceivedPacket)
                    {
                        string fullString = System.Text.Encoding.Default.GetString(BufferByte.ToArray());
                        int strLen = Math.Min(fullString.Length, 100);

                        LogHelper.Debug(LoggerType.Comm, string.Format("Packet Received : {0}...", fullString.Substring(0, strLen)));
                    }

                    if (startPos == -1)
                    {
                        string fullString = System.Text.Encoding.Default.GetString(BufferByte.ToArray());
                        int strLen = Math.Min(fullString.Length, 100);

                        LogHelper.Debug(LoggerType.Comm, string.Format("Invalid Packet : {0}...", fullString.Substring(0, strLen)));
                        BufferByte = null;
                    }
                    else if (endPos < startPos)
                    {
                        BufferByte = BufferByte.Skip(startPos).ToArray();
                    }
                    else
                    {
                        LogHelper.Debug(LoggerType.Inspection, string.Format("Start Found : {0}", startPos));

                        if (StartChar != null)
                        {
                            startPos += StartChar.Count();
                        }

                        int length = endPos - startPos;

                        LogHelper.Debug(LoggerType.Inspection, string.Format("Packet Length : {0}", length));

                        if (length == 0)
                        {
                            BufferByte = BufferByte.Skip(startPos + 1).ToArray();
                        }
                        else
                        {
                            byte[] packetContents = packetData.PacketContents = BufferByte.Skip(startPos).Take(length).ToArray();

                            if (UseChecksum)
                            {
                                LogHelper.Debug(LoggerType.Inspection, "Make Checksum");

                                string checksumReceived = System.Text.Encoding.Default.GetString(packetContents.Skip(length - ChecksumSize).Take(ChecksumSize).ToArray());
                                packetContents = packetContents.Take(length - ChecksumSize).ToArray();

                                string checksum = StringHelper.GetChecksum(packetContents, ChecksumSize);
                            }

                            LogHelper.Debug(LoggerType.Inspection, "Parse Packet");
                            if (PacketParser != null)
                            {
                                pakcetCompleted = PacketParser.ParsePacket(packetContents);
                            }

                            if (pakcetCompleted == false && ListenPacketParser != null)
                            {
                                pakcetCompleted = ListenPacketParser.ParsePacket(packetContents);
                            }

                            pakcetCompleted = true;

                            if (endPos == (BufferByte.Length - 1))
                            {
                                BufferByte = null;
                            }
                            else
                            {
                                BufferByte = BufferByte.Skip(endPos + 1).ToArray();
                            }
                        }
                    }

                    if (BufferByte == null)
                    {
                        break;
                    }

                    endPos = IndexOf(BufferByte, EndChar);
                }
            }

            imageMutex.ReleaseMutex();

            return pakcetCompleted;
        }

        public void ClearParser()
        {
            PacketParser = null;
        }

        internal void AddPacketParser(PacketParser packetParser)
        {
            PacketParser = packetParser;
        }
    }
}
