using System.Windows.Media;
using UniEye.Base.Data;

namespace UniScanC.Models
{
    public class StatusModel
    {
        public OpState State { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public string Text { get; set; }
    }
}
