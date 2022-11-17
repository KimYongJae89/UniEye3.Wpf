using System.Collections.Generic;
using UniScanC.Enums;

namespace UniScanC.Comm
{
    public class CommandInfo
    {
        public string Sender { get; set; }
        public EUniScanCCommand Command { get; set; }
        public List<string> Parameters { get; set; } = new List<string>();
    }
}
