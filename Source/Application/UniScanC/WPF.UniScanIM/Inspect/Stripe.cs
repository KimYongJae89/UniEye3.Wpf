using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using UniScanC.Data;

namespace WPF.UniScanIM.Inspect
{
    public enum TypeofStrip
    {
        Head,
        Body,
        Tail
    }

    public class Stripe
    {
        public int taskNum = 0;
        public Rectangle rectangle;
        public Defect dustDefect;
        public Image<Gray, byte> image = null;
        public bool isContinue = true;
        public int megedCnt = 0;
        public int checkCnt = 0;
        public int grabCnt = 0;
        //public int scanCnt = 0;
        public int beginInspectNum = 0;
        public int endInspectNum = 0;
        public TypeofStrip typeofStrip;
        public Stripe(Defect dustDefect, Image<Gray, byte> image, Rectangle rectangle, int taskNum, int scanCnt, int grabCnt, TypeofStrip typeofStrip)
        {
            this.dustDefect = dustDefect.Clone();
            this.image = image.Clone();
            this.rectangle = rectangle;
            this.taskNum = taskNum;
            this.grabCnt = grabCnt;
            //this.scanCnt = scanCnt;
            beginInspectNum = scanCnt;
            beginInspectNum = scanCnt;

            this.typeofStrip = typeofStrip;
        }
    }
}
