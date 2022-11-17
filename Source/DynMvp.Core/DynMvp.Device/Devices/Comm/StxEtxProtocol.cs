using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Comm
{
    public class StxEtxProtocol : IProtocol
    {
        public byte[] StartChar { get; set; } = null;
        public byte[] EndChar { get; set; } = null;
        public bool UseChecksum { get; set; }
        public int ChecksumSize { get; set; }

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

        public byte[] EncodePacket(SendPacket sendPacket)
        {
            // 앞 / 뒤를 프로토콜 메시지로 감싼다.

            byte[] packet = null;
            if (StartChar != null)
            {
                packet = StartChar.Concat(sendPacket.Data).ToArray();
            }

            if (packet == null)
            {
                packet = sendPacket.Data;
            }

            if (EndChar != null)
            {
                packet = packet.Concat(EndChar).ToArray();
            }

            return packet;
        }

        public DecodeResult DecodePacket(PacketBuffer packetBuffer, out ReceivedPacket receivedPacket)
        {
            receivedPacket = new ReceivedPacket();

            if (EndChar != null)
            {
                byte[] bufferByte = packetBuffer.DataByteFull;

                int endPos = IndexOf(bufferByte, EndChar);

                if (endPos > -1)
                {
                    int startPos = 0;
                    if (StartChar != null)
                    {
                        startPos = IndexOf(bufferByte, StartChar);
                    }

                    string packetSample = packetBuffer.GetString(100);

                    if (startPos == -1)
                    {
                        string fullString = System.Text.Encoding.Default.GetString(bufferByte.ToArray());
                        int strLen = Math.Min(fullString.Length, 100);

                        LogHelper.Debug(LoggerType.Comm, string.Format("Invalid Packet : {0}...", packetSample));
                        packetBuffer.Clear();

                        return DecodeResult.PacketError;
                    }
                    else if (endPos < startPos)
                    {
                        packetBuffer.RemoveData(startPos + 1);

                        return DecodeResult.Incomplete;
                    }
                    else
                    {
                        if (StartChar != null)
                        {
                            startPos += StartChar.Count();
                        }

                        int length = endPos - startPos;

                        LogHelper.Debug(LoggerType.Comm, string.Format("Packet Received {0} - {1} : {2} ...", startPos, endPos, packetSample));

                        byte[] packetContents = bufferByte.Skip(startPos).Take(length).ToArray();

                        if (UseChecksum)
                        {
                            LogHelper.Debug(LoggerType.Inspection, "Make Checksum");

                            string checksumReceived = System.Text.Encoding.Default.GetString(packetContents.Skip(length - ChecksumSize).Take(ChecksumSize).ToArray());
                            packetContents = packetContents.Take(length - ChecksumSize).ToArray();

                            string checksum = StringHelper.GetChecksum(packetContents, ChecksumSize);
                        }

                        receivedPacket.ReceivedDataByte = packetContents;
                        receivedPacket.Valid = true;
                        packetBuffer.RemoveData(endPos + EndChar.Length);

                        return DecodeResult.Complete;
                    }
                }
            }

            return DecodeResult.Incomplete;
        }
    }
}
