using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.BarcodeReader
{
    public class SerialBarcodeReader : IBarcodeReader
    {
        protected string barcodeRead;
        public string BarcodeRead
        {
            get => barcodeRead;
            set => barcodeRead = value;
        }
        public SerialPortEx BarcodeReaderPort { get; private set; }

        public void Init(SerialPortInfo serialPortInfo)
        {
            BarcodeReaderPort = new SerialPortEx();
            BarcodeReaderPort.Open(serialPortInfo.PortName, serialPortInfo);

            PacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += BarcodeDataReceived;

            BarcodeReaderPort.PacketHandler.EndChar = new byte[1] { (byte)'\r' };
            BarcodeReaderPort.PacketHandler.PacketParser = packetParser;
            BarcodeReaderPort.StartListening();
        }

        private void BarcodeDataReceived(ReceivedPacket receivedPacket)
        {
            barcodeRead = receivedPacket.ReceivedData;
        }

        public void Reset()
        {
            barcodeRead = "";
        }
    }
}
