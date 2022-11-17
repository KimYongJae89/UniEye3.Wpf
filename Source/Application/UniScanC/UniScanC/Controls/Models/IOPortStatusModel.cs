using Unieye.WPF.Base.Helpers;

namespace UniScanC.Controls.Models
{
    public class IOPortStatusModel : Observable
    {
        private int portNum;
        private string portName;
        private bool portStatus;

        public IOPortStatusModel(int portNum, string portName, bool portStatus = false)
        {
            this.portNum = portNum;
            this.portName = portName;
            this.portStatus = portStatus;
        }

        public int PortNum
        {
            get => portNum;
            set => Set(ref portNum, value);
        }

        public string PortName
        {
            get => portName;
            set => Set(ref portName, value);
        }

        public bool PortStatus
        {
            get => portStatus;
            set => Set(ref portStatus, value);
        }
    }
}
