using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Config;
using UniEye.Base.Data;

namespace UniEye.Base.Inspect
{
    public class LineScanInspectRunner : UniEye.Base.Inspect.DirectTriggerInspectRunner
    {
        public int GrabImageHeight { get; set; } = 0;
        public float LineSpeedMps { get; set; } = 0;
        public TriggerMode TriggerMode { get; set; }

        public LineScanInspectRunner()
        {
        }

        public override bool EnterWaitInspection(ModelBase curModel)
        {
            if (UsedCamIndexArr == null)
            {
                CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;

                var camIndexList = new List<int>();
                foreach (Camera camera in cameraHandler)
                {
                    if (camera.IsLineScanCamera())
                    {
                        camIndexList.Add(camera.Index);
                    }
                }

                UsedCamIndexArr = camIndexList.ToArray();
            }

            SetupCamera();

            if (base.EnterWaitInspection(curModel) == false)
            {
                return false;
            }

            return true;
        }

        protected virtual void SetupCamera()
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;

            if (TriggerMode == TriggerMode.Software)
            {
                foreach (int camIndex in UsedCamIndexArr)
                {
                    Camera camera = cameraHandler.GetCamera(camIndex);
                    Calibration calibration = SystemManager.Instance().GetCameraCalibration(camIndex);

                    float pixelResoultion = calibration.PelSize.Width;

                    if (LineSpeedMps == 0)
                    {
                        LineSpeedMps = 120;
                    }

                    // um 기준으로 변환
                    float grabHz = (LineSpeedMps * 1000000) / pixelResoultion;

                    camera.SetAcquisitionLineRate(grabHz);
                    camera.UpdateBuffer();
                }
            }
            else
            {
                foreach (int camIndex in UsedCamIndexArr)
                {
                    Camera camera = cameraHandler.GetCamera(camIndex);
                    camera.UpdateBuffer();
                }
            }
        }
    }
}
