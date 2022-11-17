using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.MachineInterface
{
    public enum MachineInterfaceType
    {
        None, UniEyeExchange
    }

    public class MachineInterfaceFactory
    {
        public static IMachineInterface Create(MachineInterfaceType machineInterfaceType)
        {
            switch (machineInterfaceType)
            {
                case MachineInterfaceType.UniEyeExchange:
                    return new UmxMachineInterface();
            }

            return null;
        }
    }
}
