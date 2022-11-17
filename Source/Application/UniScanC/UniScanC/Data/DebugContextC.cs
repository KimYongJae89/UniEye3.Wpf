using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanC.Data
{
    public class DebugContextC : DebugContext
    {
        public int FrameNo { get; set; }

        public DebugContextC(bool saveDebugImage, string path, int frameNo) : base(saveDebugImage, path)
        {
            FrameNo = frameNo;
        }
    }
}
