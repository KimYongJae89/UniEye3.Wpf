using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using DynMvp.Vision;
using System.Drawing;
using System.Threading;

namespace DynMvp.Inspect
{
    public class InspectParam
    {
        public Calibration CameraCalibration { get; }
        public Calibration LocalCameraCalibration { get; set; }
        public PositionAligner GlobalPositionAligner { get; } = null;
        public PositionAligner LocalPositionAligner { get; set; } = null;
        public PointF InspectStepAlignedPos { get; set; }
        public int StepNo { get; }
        public ImageBuffer ImageBuffer { get; }
        public string ResultPath { get; set; }
        public ProbeResultList ProbeResultList { get; set; }
        public bool SelectedInspMode { get; set; } = false;
        public bool SingleStepMode { get; set; }
        public int InspectionStepTypeMask { get; set; } = InspectStep.StepAll;
        public bool TeachMode { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IInspectEventListener InspectEventHandler { get; set; }

        public InspectParam(int stepNo, PositionAligner globalPositionAligner, Calibration cameraCalibration, ImageBuffer imageBuffer,
            ProductResult productResult, bool teachMode, CancellationToken cancellationToken, IInspectEventListener inspectEventHandler)
        {
            StepNo = stepNo;
            GlobalPositionAligner = globalPositionAligner;
            CameraCalibration = cameraCalibration;
            ImageBuffer = imageBuffer;
            ProbeResultList = productResult;
            ResultPath = productResult.ResultPath;
            TeachMode = teachMode;
            CancellationToken = cancellationToken;
            InspectEventHandler = inspectEventHandler;
        }

        public InspectParam(int stepNo, PositionAligner globalPositionAligner, Calibration cameraCalibration, ImageBuffer imageBuffer,
            ProbeResultList probeResultList, bool teachMode, CancellationToken cancellationToken, IInspectEventListener inspectEventHandler)
        {
            StepNo = stepNo;
            GlobalPositionAligner = globalPositionAligner;
            CameraCalibration = cameraCalibration;
            ImageBuffer = imageBuffer;
            ProbeResultList = probeResultList;
            TeachMode = teachMode;
            CancellationToken = cancellationToken;
            InspectEventHandler = inspectEventHandler;
        }
    }
}
