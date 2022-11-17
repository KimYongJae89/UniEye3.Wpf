using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanC.MachineIf.DataAdapter
{
    internal enum ItemSet { GET_MACHINE_STATE };

    public abstract class MachineIfDataAdapterC : UniEye.Base.MachineInterface.MachineIfDataAdapter
    {
        public new MachineIfDataC MachineIfData => base.MachineIfData as MachineIfDataC;

        public MachineIfDataAdapterC(UniEye.Base.MachineInterface.MachineIfDataBase machineIfData) : base(machineIfData) { }
    }
}
