using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.MachineInterface
{
    public abstract class MachineIfDataBase
    {
        public bool IsConnected = false;
    }

    public abstract class MachineIfDataAdapter
    {
        public MachineIfDataBase MachineIfData { get; protected set; }

        public abstract void Read();
        public abstract void Write();

        public MachineIfDataAdapter(MachineIfDataBase machineIfData)
        {
            LogHelper.Debug(LoggerType.Network, $"MachineIfDataAdapter Created");
            MachineIfData = machineIfData;
        }

    }
}
