using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.BarcodeReader
{
    public enum BarcodeReaderType
    {
        Serial, TcpIp
    }

    public interface IBarcodeReader
    {
        string BarcodeRead { get; }
        void Reset();
    }

    public class BarcodeReaderFactory
    {
        public static IBarcodeReader Create(BarcodeReaderType barcodeReaderType)
        {
            return new SerialBarcodeReader();
        }
    }
}
