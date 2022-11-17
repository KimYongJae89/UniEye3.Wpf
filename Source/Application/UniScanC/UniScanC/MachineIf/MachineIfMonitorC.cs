using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unieye.WPF.Base.Override;
using UniEye.Base.MachineInterface;
using UniScanC.MachineIf.DataAdapter;

namespace UniScanC.MachineIf
{
    public delegate void OnUpdatedDelegate();

    public abstract class MachineIfMonitorC
    {
        #region 생성자
        public MachineIfMonitorC(MachineIfDataAdapter adapter)
        {
            LastUpdateTime = DateTime.Now;
            Adapter = adapter;
        }
        #endregion


        #region 속성
        public DateTime LastUpdateTime { get; protected set; }

        public MachineIfDataC MachineIfData => (MachineIfDataC)Adapter.MachineIfData;

        public MachineIfDataAdapter Adapter { get; protected set; }

        public float VirtualMaxLotLength { get; set; } = 300.0f;

        protected ThreadHandler Thread { get; set; }

        private float VirtualAccel = 8.0f;

        private float VirtualDecel = -8.0f;

        private bool AutoStarted { get; set; } = true;
        #endregion


        #region 메서드
        public abstract void Start();

        public abstract void Stop();

        public abstract void ThreadProc();

        public void Read()
        {
            Adapter.Read();
            PropagateData();

            LastUpdateTime = DateTime.Now;
        }

        public void VirtualRead()
        {
            if (!Adapter.MachineIfData.IsConnected)
            {
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now - LastUpdateTime;

            bool isDecel = (MachineIfData.GET_PRESENT_POSITION > VirtualMaxLotLength);
            float targetSpd = isDecel ? 0 : MachineIfData.GET_TARGET_SPEED;
            float spdDiff = targetSpd - MachineIfData.GET_PRESENT_SPEED;
            float acc = (float)((spdDiff >= 0 ? Math.Min(spdDiff, VirtualAccel) : Math.Max(spdDiff, VirtualDecel)) * timeSpan.TotalSeconds);

            MachineIfData.GET_PRESENT_SPEED += acc;
            MachineIfData.GET_PRESENT_POSITION += (float)(MachineIfData.GET_PRESENT_SPEED * timeSpan.TotalMinutes);

            if (Math.Round(spdDiff, 2) == 0)
            {
                if (isDecel)
                {
                    //ChangeRewinder();
                }
                else if (!AutoStarted)
                {
                    MachineIfData.GET_START_COATING = true;
                    AutoStarted = true;
                }
            }
            else
            {
                MachineIfData.GET_START_COATING = false;
                AutoStarted = false;
            }

            PropagateData();

            LastUpdateTime = now;
        }

        //private void ChangeRewinder()
        //{
        //    MachineIfData machineIfData = this.MachineIfData;

        //    machineIfData.GET_REWINDER_CUT = !machineIfData.GET_REWINDER_CUT;
        //    machineIfData.GET_PRESENT_POSITION = 0;

        //    string lot = machineIfData.GET_LOT;
        //    if (!string.IsNullOrEmpty(lot))
        //    {
        //        int count = 0;
        //        string body = lot;
        //        string[] tokens = lot.Split('-');
        //        if (tokens.Length > 1)
        //        {
        //            body = string.Join("-", tokens.Take(tokens.Length - 1));
        //            if (int.TryParse(tokens.LastOrDefault(), out count))
        //                count++;
        //        }
        //        machineIfData.GET_LOT = string.Format("{0}-{1}", body, count);
        //    }
        //}

        /// <summary>
        /// 읽은 MachineIfData데이터를 각 디바이스에 적용함.
        /// </summary>
        public abstract void PropagateData();

        public void Write()
        {
            ApplyData();
            Adapter.Write();
        }

        public void VirtualWrite()
        {
            if (!Adapter.MachineIfData.IsConnected)
            {
                return;
            }

            ApplyData();
        }

        /// <summary>
        /// 쓰기 전, 각 디바이스의 상태를 MachineIfData에 저장함.
        /// </summary>
        public abstract void ApplyData();
        #endregion
    }
}
