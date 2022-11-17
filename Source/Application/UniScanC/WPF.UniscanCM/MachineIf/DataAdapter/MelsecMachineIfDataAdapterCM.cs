using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.Melsec;
using UniScanC.MachineIf.DataAdapter;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.PLC;

namespace WPF.UniScanCM.MachineIf.DataAdapter
{
    public class MelsecMachineIfDataAdapterCM : MelsecMachineIfDataAdapterC
    {
        #region 생성자
        public MelsecMachineIfDataAdapterCM(MachineIfDataCM machineIfData) : base(machineIfData)
        {
            SetVisionStateItemInfoSet = GetItemInfoSet(MachineIfItemSet.SET_VISION_STATE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_READY,
             MachineIfItemCoating.SET_VISION_COATING_INSP_RUNNING,
             MachineIfItemCoating.SET_VISION_COATING_INSP_ERROR,

             MachineIfItemCoating.SET_VISION_COATING_INSP_NG_PINHOLE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_NG_DUST,

             MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_ALL,
             MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_PINHOLE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_DUST,

             MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_ALL,
             MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE,
             MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_DUST
             );
        }
        #endregion


        #region 속성
        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;

        private PlcBase PLC => DeviceManager?.PLCMachineIf;

        private MelsecMachineIfItemInfoSet SetVisionStateItemInfoSet { get; set; }
        #endregion


        #region 메서드
        public override void Parse(MachineIfItemInfoResponce responce, Tuple<Enum, int, int>[] tuples)
        {
            base.Parse(responce, tuples);
        }

        public override void Read()
        {
            var machineIfData = (MachineIfDataCM)MachineIfData;

            if (!machineIfData.IsConnected)
            {
                return;
            }

            Read(GetMachineStateItemInfoSet);
        }

        public override void Write()
        {
            var machineIfData = (MachineIfDataCM)MachineIfData;
            if (!machineIfData.IsConnected)
            {
                return;
            }

            Write(SetVisionStateItemInfoSet);
        }

        protected override void Read(MelsecMachineIfItemInfoSet melsecMachineIfItemInfoSet)
        {
            foreach (KeyValuePair<MelsecMachineIfItemInfo, Tuple<Enum, int, int>[]> pair in melsecMachineIfItemInfoSet.Dictionary)
            {
                MachineIfItemInfoResponce responce = PLC?.MachineIf.SendCommand(pair.Key);
                responce.WaitResponce();

                if (responce.IsGood && responce.IsResponced)
                {
                    Parse(responce, pair.Value);
                }
            }
        }

        protected override void Write(MelsecMachineIfItemInfoSet melsecMachineIfItemInfoSet)
        {
            foreach (KeyValuePair<MelsecMachineIfItemInfo, Tuple<Enum, int, int>[]> pair in melsecMachineIfItemInfoSet.Dictionary)
            {
                string arg = MakeArgument(pair.Value);
                MachineIfItemInfoResponce responce = PLC?.MachineIf.SendCommand(pair.Key, arg);
                responce.WaitResponce();
            }
        }

        protected override MelsecMachineIfItemInfoSet GetItemInfoSet(Enum command, params Enum[] subCommand)
        {
            MachineIfItemInfo[] MachineIfItemInfos = PLC?.MachineIf.MachineIfSetting.MachineIfItemInfoList.GetItemInfos();

            // 명렁어 -> 프로토콜 변환
            var wholeProtocol = Array.FindAll(MachineIfItemInfos, f => f.Use && subCommand.Contains(f.Command)).Cast<MelsecMachineIfItemInfo>().ToList();

            // 정렬
            wholeProtocol.Sort(new Comparison<MelsecMachineIfItemInfo>((f, g) => f.Address.CompareTo(g.Address)));

            //wholeProtocol.Sort(new MelsecMachineIfItemInfoComp());

            var protocolSet = new MelsecMachineIfItemInfoSet(command);
            if (wholeProtocol.Count == 0)
            {
                return protocolSet;
            }

            // 연결된 주소끼리 그룹화
            var subProtocolList = new List<List<MelsecMachineIfItemInfo>>();
            subProtocolList.Add(new List<MelsecMachineIfItemInfo>());
            subProtocolList[0].Add(wholeProtocol[0]);
            wholeProtocol.Aggregate((f, g) =>
            {
                int nextAddr = int.Parse(f.Address.Substring(1)) + f.SizeWord;
                if (f.IsReadCommand != g.IsReadCommand || int.Parse(g.Address.Substring(1)) != nextAddr)
                {
                    subProtocolList.Add(new List<MelsecMachineIfItemInfo>());
                }

                subProtocolList.LastOrDefault().Add(g);
                return g;
            });

            // 각 그룹별 프로토콜 생성
            subProtocolList.ForEach(f =>
            {
                var tupleList = new List<Tuple<Enum, int, int>>();
                int baseAddr = int.Parse(f[0].Address.Substring(1));
                f.ForEach(g =>
                {
                    int addr = int.Parse(g.Address.Substring(1));
                    int offset = (addr - baseAddr) * 2;
                    int size = g.SizeWord * 2;
                    tupleList.Add(new Tuple<Enum, int, int>(g.Command, offset, size));
                });

                if (tupleList.Count > 0)
                {
                    var subProtocol = new MelsecMachineIfItemInfo(null);
                    Debug.Assert(f.TrueForAll(g => g.IsReadCommand == f[0].IsReadCommand));
                    subProtocol.WaitResponceMs = f.Max(g => g.WaitResponceMs);
                    subProtocol.IsReadCommand = f[0].IsReadCommand;
                    subProtocol.Address = f[0].Address;
                    subProtocol.SizeWord = tupleList.Sum(g => g.Item3) / 2;
                    subProtocol.Use = true;
                    protocolSet.AddProtocol(subProtocol, tupleList.ToArray());
                }
            });

            return protocolSet;
        }

        protected override string MakeArgument(Tuple<Enum, int, int>[] tuples)
        {
            var machineIfData = MachineIfData as MachineIfDataCM;
            string[] aa = Array.ConvertAll<Tuple<Enum, int, int>, string>(tuples, f =>
            {
                string format = string.Format("X{0}", f.Item3 * 2);
                switch (f.Item1)
                {
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_READY:
                        return (machineIfData.SET_VISION_COATING_INSP_READY ? 1 : 0).ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_RUNNING:
                        return (machineIfData.SET_VISION_COATING_INSP_RUNNING ? 1 : 0).ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_ERROR:
                        return (machineIfData.SET_VISION_COATING_INSP_ERROR ? 1 : 0).ToString(format);

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_NG_PINHOLE:
                        return (machineIfData.SET_VISION_COATING_INSP_NG_PINHOLE ? 1 : 0).ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_NG_DUST:
                        return (machineIfData.SET_VISION_COATING_INSP_NG_DUST ? 1 : 0).ToString(format);

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_ALL:
                        return machineIfData.SET_VISION_COATING_INSP_CNT_ALL.ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_PINHOLE:
                        return machineIfData.SET_VISION_COATING_INSP_CNT_PINHOLE.ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_CNT_DUST:
                        return machineIfData.SET_VISION_COATING_INSP_CNT_DUST.ToString(format);

                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_ALL:
                        return machineIfData.SET_VISION_COATING_INSP_TOTAL_CNT_ALL.ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE:
                        return machineIfData.SET_VISION_COATING_INSP_TOTAL_CNT_PINHOLE.ToString(format);
                    case MachineIfItemCoating.SET_VISION_COATING_INSP_TOTAL_CNT_DUST:
                        return machineIfData.SET_VISION_COATING_INSP_TOTAL_CNT_DUST.ToString(format);
                    default:
                        return new string('0', f.Item3);
                }
            });

            return string.Join("", aa);
        }
        #endregion
    }
}
