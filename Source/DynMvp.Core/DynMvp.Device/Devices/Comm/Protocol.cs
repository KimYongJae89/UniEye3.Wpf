using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Comm
{
    public class PacketBuffer
    {
        #region 속성
        public byte[] DataByteFull { get; private set; }

        public bool Empty => DataByteFull == null;

        private const int MAXSIZE = 4096;
        #endregion


        #region 메서드
        public void AppendData(byte[] data, int numData)
        {
            int numDataFull = 0;
            if (DataByteFull != null)
            {
                numDataFull = DataByteFull.Length;
            }

            byte[] tempBuf = new byte[numDataFull + numData];
            if (DataByteFull != null)
            {
                Array.Copy(DataByteFull, 0, tempBuf, 0, numDataFull);
            }

            Array.Copy(data, 0, tempBuf, numDataFull, numData);
            DataByteFull = tempBuf;
        }

        public void RemoveData(int numData)
        {
            int numDataFull = 0;

            if (DataByteFull != null)
            {
                numDataFull = DataByteFull.Length;
            }

            if ((numDataFull - numData) > 0)
            {
                byte[] tempBuf = new byte[numDataFull - numData];
                Array.Copy(DataByteFull, numData, tempBuf, 0, numDataFull - numData);
                DataByteFull = tempBuf;
            }
            else
            {
                DataByteFull = null;
            }
        }

        public void Clear()
        {
            DataByteFull = null;
        }

        public override string ToString()
        {
            return System.Text.Encoding.Default.GetString(DataByteFull);
        }

        public string GetString(int maxLen)
        {
            string fullString = System.Text.Encoding.Default.GetString(DataByteFull);
            int strLen = Math.Min(fullString.Length, maxLen);

            return fullString.Substring(0, strLen);
        }
        #endregion
    };

    public class SendPacket
    {
        #region 생성자
        public SendPacket() { }

        public SendPacket(string contents)
        {
            Contents = contents;
            Data = Encoding.UTF8.GetBytes(contents);
        }

        public SendPacket(byte[] data)
        {
            Data = data;
        }
        #endregion


        #region 속성
        public string Contents { get; set; }

        public virtual byte[] Data { get; private set; }

        public bool WaitResponse { get; set; } = false;
        #endregion
    }

    public class ReceivedPacket
    {
        #region 생성자
        public ReceivedPacket() { }

        public ReceivedPacket(byte[] receivedData, string errCode = null)
        {
            ReceivedDataByte = receivedData;
            ErrCode = errCode;
        }
        #endregion


        #region 속성
        public string SenderInfo { get; set; }

        public bool Valid { get; set; } = false;

        public byte[] ReceivedDataByte { get; set; }

        public string ReceivedData { get; set; }

        public string LogString { get; set; }

        public string ErrCode { get; set; }
        #endregion
    }

    public enum DecodeResult
    {
        Complete, Incomplete, PacketError, OtherTarget
    }

    public interface IProtocol
    {
        #region 메서드
        byte[] EncodePacket(SendPacket sendPacket);

        // 패킷을 읽은 후 receivedPacket을 생성하여 반환한다. 
        // 읽은 패킷은 packetBuffer에서 제거한다.
        DecodeResult DecodePacket(PacketBuffer packetBuffer, out ReceivedPacket receivedPacket);
        #endregion
    }
}
