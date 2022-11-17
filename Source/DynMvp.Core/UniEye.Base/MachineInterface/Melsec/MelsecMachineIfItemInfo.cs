using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface.TcpIp;

namespace UniEye.Base.MachineInterface.Melsec
{
    public class MelsecMachineIfItemInfo : TcpIpMachineIfItemInfo
    {
        #region 생성자
        public MelsecMachineIfItemInfo() { }

        public MelsecMachineIfItemInfo(Enum command) : base(command, false, 500) { }

        public MelsecMachineIfItemInfo(Enum command, bool use, int waitResponceMs, string address, bool isReadCommand, int sizeWord) : base(command, use, waitResponceMs)
        {
            if (address == null)
            {
                address = "";
            }

            Address = address;
            IsReadCommand = isReadCommand;
            SizeWord = sizeWord;
        }
        #endregion


        #region 속성
        public string Address { get; set; }

        public bool IsReadCommand { get; set; } = true;

        public int SizeWord { get; set; } = 0;
        #endregion


        #region 메서드
        public override MachineIfItemInfo Clone()
        {
            return new MelsecMachineIfItemInfo(Command, Use, WaitResponceMs, Address, IsReadCommand, SizeWord);
        }

        public override void Copyfrom(MachineIfItemInfo machineIfItemInfo)
        {
            base.Copyfrom(machineIfItemInfo);
            if (machineIfItemInfo is MelsecMachineIfItemInfo melsecMachineIfItemInfo)
            {
                Address = melsecMachineIfItemInfo.Address;
                IsReadCommand = melsecMachineIfItemInfo.IsReadCommand;
                SizeWord = melsecMachineIfItemInfo.SizeWord;
            }
        }

        public override bool IsValid => (!Use) || (Use && !string.IsNullOrEmpty(Address) && SizeWord > 0);
        #endregion
    }
}
