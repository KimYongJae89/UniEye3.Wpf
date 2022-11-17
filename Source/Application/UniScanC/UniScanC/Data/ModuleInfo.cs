using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using Newtonsoft.Json;
using System.Drawing;
using UniScanC.Enums;

namespace UniScanC.Data
{
    public class ModuleInfo
    {
        [JsonIgnore]
        public Camera Camera => DeviceManager.Instance().CameraHandler?.GetCamera(CameraNo);

        [JsonIgnore]
        public Size BufferSize => UseFrameTrigger ? new Size(BufferWidth, BufferHeight) : Camera.ImageSize;

        public int ModuleNo { get; set; }
        public int CameraNo { get; set; }
        public ECamPosition CamPos { get; set; } = ECamPosition.OneCam;
        public TriggerMode TriggerMode { get; set; } = TriggerMode.Hardware;
        public float ResolutionWidth { get; set; } = 10; //um
        public float ResolutionHeight { get; set; } = 10; //um
        public int BufferWidth { get; set; } //px
        public int BufferHeight { get; set; } //px
        public string Topic { get; set; }
        public bool UseFrameTrigger { get; set; } = false;
        public bool UseDefectSignal { get; set; } = true;
        public int DefectSignalPort { get; set; } = -1;
    }
}