using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface.AllenBreadley
{
    public class AllenBreadleyMachineIfItemInfo : MachineIfItemInfo
    {
        #region 생성자
        public AllenBreadleyMachineIfItemInfo() { }

        public AllenBreadleyMachineIfItemInfo(Enum command) : base(command, false, 500) { }

        public AllenBreadleyMachineIfItemInfo(Enum command, bool use, int waitResponceMs) : base(command, use, waitResponceMs) { }
        #endregion


        #region 속성
        private string _tagName;
        public string TagName { get => _tagName; set => Set(ref _tagName, value); }

        private int _offsetByte4;
        public int OffsetByte4 { get => _offsetByte4; set => Set(ref _offsetByte4, value); }

        private int _sizeByte4;
        public int SizeByte4 { get => _sizeByte4; set => Set(ref _sizeByte4, value); }

        private bool _isWriteable;
        public bool IsWriteable { get => _isWriteable; set => Set(ref _isWriteable, value); }
        #endregion


        #region 메서드
        public override MachineIfItemInfo Clone()
        {
            return new AllenBreadleyMachineIfItemInfo(Command, Use, WaitResponceMs)
            {
                TagName = TagName,
                OffsetByte4 = OffsetByte4,
                SizeByte4 = SizeByte4,
                IsWriteable = IsWriteable
            };
        }

        public override void Copyfrom(MachineIfItemInfo machineIfItemInfo)
        {
            base.Copyfrom(machineIfItemInfo);
            if (machineIfItemInfo is AllenBreadleyMachineIfItemInfo allenBreadleyMachineIfItemInfo)
            {
                TagName = allenBreadleyMachineIfItemInfo.TagName;
                OffsetByte4 = allenBreadleyMachineIfItemInfo.OffsetByte4;
                SizeByte4 = allenBreadleyMachineIfItemInfo.SizeByte4;
                IsWriteable = allenBreadleyMachineIfItemInfo.IsWriteable;
            }
        }

        public override bool IsValid => !Use || (Use && (SizeByte4 > 0));

        public override string ToString()
        {
            return $"{Command}_{TagName}_{OffsetByte4}_{SizeByte4}";
        }
        #endregion
    }
}
