using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface
{
    public class MachineIfItemInfoResponce
    {
        #region 필드
        private bool isGood = false;
        #endregion


        #region 생성자
        public MachineIfItemInfoResponce(MachineIfItemInfo sentItemInfo)
        {
            SentItemInfo = sentItemInfo;
        }
        #endregion


        #region 속성
        public MachineIfItemInfo SentItemInfo { get; private set; }

        public ReceivedPacket ReceivedPacket { get; private set; } = null;

        public ManualResetEvent OnResponce { get; private set; } = new ManualResetEvent(false);

        public string ReciveData { get; private set; }

        public bool IsResponced { get; private set; } = false;

        public bool IsGood => IsResponced && isGood /*&& !string.IsNullOrEmpty(reciveData)*/;
        #endregion


        #region 메서드
        public void SetRecivedData(string reciveData, bool isGood, ReceivedPacket receivedPacket)
        {
            //if (string.IsNullOrEmpty(reciveData))
            //    return;

            ReciveData = reciveData;
            ReceivedPacket = receivedPacket;
            OnResponce.Set();
            this.isGood = isGood;
            IsResponced = true;
        }

        public bool WaitResponce(int timeoutMs = -1)
        {
            if (timeoutMs < 0)
            {
                timeoutMs = SentItemInfo.WaitResponceMs;
            }

            return OnResponce.WaitOne(timeoutMs);
        }

        /// <summary>
        /// Ascii Byte -> Unicode String
        /// </summary>
        /// <returns></returns>
        public string Convert2String()
        {
            if (IsResponced == false)
            {
                return null;
            }

            char[] chars = new char[ReciveData.Length / 2];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)Convert.ToByte(ReciveData.Substring(i * 2, 2), 16);
            }

            return new string(chars).Trim('\0');
        }

        public string Convert2StringLE()
        {
            if (IsResponced == false)
            {
                return null;
            }

            char[] chars = new char[ReciveData.Length / 2];
            for (int i = 0; i < chars.Length; i += 2)
            {
                int idx1 = (i + 1) * 2;
                if (idx1 + 1 < chars.Length)
                {
                    chars[i] = (char)Convert.ToByte(ReciveData.Substring(idx1, 2), 16);
                }

                int idx2 = (i) * 2;
                if (idx2 + 1 < chars.Length)
                {
                    chars[i + 1] = (char)Convert.ToByte(ReciveData.Substring(idx2, 2), 16);
                }
            }
            string converted = new string(chars).Trim('\0');
            return converted;
        }
        #endregion
    }

}
