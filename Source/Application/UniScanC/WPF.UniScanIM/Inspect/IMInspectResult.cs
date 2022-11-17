namespace WPF.UniScanIM.Inspect
{
    public class IMInspectResult
    {
        public int ModuleNo { get; set; }
        public int FrameNo { get; set; }
        public System.Windows.Media.Imaging.BitmapSource SourceImage { get; set; }
        //public System.Windows.Media.Imaging.BitmapSource DefectImage { get; set; }

        public IMInspectResult() { }
    }
}
