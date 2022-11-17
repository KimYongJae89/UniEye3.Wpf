using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Comm
{
    public class SerialDeviceInfoFactory
    {
        public static SerialDeviceInfo CreateSerialDeviceInfo(ESerialDeviceType deviceType)
        {
            switch (deviceType)
            {
                case ESerialDeviceType.SerialEncoder:
                    return new SerialEncoderInfo();
                default:
                    return null;
            }
        }
    }
}
