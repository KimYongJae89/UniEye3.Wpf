using DynMvp.Base;
using DynMvp.Component.DepthSystem;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniEye.Base.UI.Main
{
    public delegate void UpdateImageDelegate(ImageD grabImage);

    public class ModellerPageExtender
    {
        protected int cameraIndex;

        public ModellerPageExtender()
        {
        }

        protected Camera GetCamera(int cameraIndex)
        {
            return DeviceManager.Instance().CameraHandler.GetCamera(cameraIndex);
        }

        public virtual void GrabImage(int stepIndex, int cameraIndex, int lightTypeIndex, LightParam lightParam)
        {
            ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
            Image2D grabImage = imageAcquisition.Acquire(cameraIndex, lightParam);

            ModelBase currentModel = ModelManager.Instance().CurrentModel;

            string imagePath = Path.Combine(currentModel.ModelPath, "Image");
            if (Directory.Exists(imagePath) == false)
            {
                Directory.CreateDirectory(imagePath);
            }

            SystemManager.Instance().ImageSequence.SetImage(cameraIndex, stepIndex, lightTypeIndex, grabImage);
        }

        public virtual void StopGrab()
        {
            Camera camera = GetCamera(cameraIndex);
            if (camera == null)
            {
                return;
            }

            camera.Stop();
        }
    }

    public class HwTriggerModellerPageExtender : ModellerPageExtender
    {
        protected int stepIndex;
        protected int lightTypeIndex;

        public HwTriggerModellerPageExtender() : base()
        {
        }

        private void SetupGrab(LightParam lightParam = null)
        {
            Camera camera = GetCamera(cameraIndex);
            if (camera == null)
            {
                return;
            }

            if (lightParam == null || lightParam.LightParamType == LightParamType.Value)
            {
                GetCamera(cameraIndex).ImageGrabbed += ImageGrabbed;
            }
        }

        public override void GrabImage(int stepIndex, int cameraIndex, int lightTypeIndex, LightParam lightParam)
        {
            //this.stepIndex = stepIndex;
            //this.cameraIndex = cameraIndex;
            //this.lightTypeIndex = lightTypeIndex;

            SetupGrab();

            Camera camera = GetCamera(cameraIndex);
            if (camera == null)
            {
                return;
            }

            camera.SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);
        }

        public void ImageGrabbed(Camera camera)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ImageGrabbed");

            camera.ImageGrabbed -= ImageGrabbed;

            DeviceManager.Instance().CameraHandler.SetTriggerMode(TriggerMode.Software);

            var grabImage = (Image2D)camera.GetGrabbedImage();

            SystemManager.Instance().ImageSequence.SetImage(cameraIndex, stepIndex, lightTypeIndex, grabImage);
        }

        public override void StopGrab()
        {
            Camera camera = GetCamera(cameraIndex);
            if (camera == null)
            {
                return;
            }

            camera.ImageGrabbed -= ImageGrabbed;

            DeviceManager.Instance().CameraHandler.SetTriggerMode(TriggerMode.Software);
        }
    }
}
