using DynMvp.Base;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.FrameGrabber
{
    public class ImageAcquisition
    {
        private LightCtrlHandler lightCtrlHandler;

        // Multi Grab Data
        private int bufferIndex;
        private List<Image2D> imageList;
        public CameraHandler CameraHandler { get; private set; }

        public ImageAcquisition()
        {

        }

        public ImageBuffer CreateImageBuffer()
        {
            int numLightType = LightConfig.Instance().NumLightType;

            var imageBuffer = new ImageBuffer(numLightType);

            LightParamSet lightParamSet = LightConfig.Instance().LightParamSet;

            foreach (Camera camera in CameraHandler)
            {
                for (int lightTypeIndex = 0; lightTypeIndex < numLightType; lightTypeIndex++)
                {
                    LightParam lightParam = lightParamSet[lightTypeIndex];
                    if (lightParam.LightParamType == LightParamType.Value)
                    {
                        imageBuffer.AddImage((Image2D)camera.CreateCompatibleImage());
                    }
                    else
                    {
                        imageBuffer.AddImage(new Image2D(camera.ImageSize.Width, camera.ImageSize.Height, 1));
                    }
                }
            }

            return imageBuffer;
        }

        public void Initialize(CameraHandler cameraHandler, LightCtrlHandler lightCtrlHandler)
        {
            CameraHandler = cameraHandler;
            this.lightCtrlHandler = lightCtrlHandler;
        }

        public void AcquireMulti(int cameraIndex, List<Image2D> imageList, LightParam lightParam)
        {
            bufferIndex = 0;
            this.imageList = imageList;

            if (lightCtrlHandler != null)
            {
                lightCtrlHandler.TurnOn(lightParam);
            }

            Camera camera = CameraHandler.GetCamera(cameraIndex);
            if (camera != null)
            {
                camera.ImageGrabbed += camera_ImageGrabbed;
                camera.SetTriggerMode(TriggerMode.Hardware);
            }
        }

        private void camera_ImageGrabbed(Camera camera)
        {
            if (bufferIndex < imageList.Count)
            {
                imageList[bufferIndex++].CopyFrom(camera.GetGrabbedImage());
            }

            if (bufferIndex >= imageList.Count)
            {
                camera.SetTriggerMode(TriggerMode.Software);
            }
        }

        public Image2D Acquire(int cameraIndex, LightParam lightParam)
        {
            int numDevice = CameraHandler.NumCamera;

            if (lightParam.LightParamType == LightParamType.Value)
            {
                if (lightCtrlHandler != null)
                {
                    lightCtrlHandler.TurnOn(lightParam);
                }
            }

            Camera camera = CameraHandler.GetCamera(cameraIndex);
            if (camera != null)
            {
                camera.GrabOnce();

                bool grabOk = CameraHandler.WaitGrabDone(new int[] { cameraIndex }, 5000);
                if (grabOk)
                {
                    return (Image2D)camera.GetGrabbedImage();
                }
                else
                {
#if DEBUG == flase
                    ErrorManager.Instance().Report((int)ErrorSection.Grabber, ErrorLevel.Fatal, ErrorSection.Grabber.ToString(), "Grab Fail", "Grab Fail Message");
#endif
                    //throw new AlarmException("Grab Fail Alaram");
                }
            }

            return null;
        }

        public void Acquire(ImageBuffer imageBuffer)
        {
            int numDevice = CameraHandler.NumCamera;

            for (int cameraIndex = 0; cameraIndex < numDevice; cameraIndex++)
            {
                Camera camera = CameraHandler.GetCamera(cameraIndex);
                camera.GrabOnceAsync();
            }

            CameraHandler.WaitGrabDone();

            for (int cameraIndex = 0; cameraIndex < numDevice; cameraIndex++)
            {
                Image2D image = imageBuffer.GetImage(cameraIndex, 0);
                Camera camera = CameraHandler.GetCamera(cameraIndex);
                ImageD grabbedImage = camera.GetGrabbedImage();
                image.CopyFrom(grabbedImage);
            }

            //            for (int cameraIndex = 0; cameraIndex < numDevice; cameraIndex++)
            //            {
            //                Image2D image = imageBuffer.GetImage(cameraIndex, 0);
            //                if (image != null)
            //                    image.Clear();

            //                Camera camera = cameraHandler.GetCamera(cameraIndex);
            //                if (camera != null)
            //                {
            //                    camera.GrabOnceAsync();

            //                    bool grabOk = cameraHandler.WaitGrabDone(new int[] { cameraIndex }, 5000);
            //                    if (grabOk)
            //                    {
            //                        ImageD grabbedImage = camera.GetGrabbedImage();
            //                        image.CopyFrom(grabbedImage);
            //                    }
            //                    else
            //                    {
            //#if DEBUG == flase
            //                        ErrorManager.Instance().Report((int)ErrorSection.Grabber, ErrorLevel.Fatal, ErrorSection.Grabber.ToString(), "Grab Fail", "Grab Fail Message");
            //#endif
            //                        //throw new AlarmException("Grab Fail Alaram");
            //                    }
            //                }
            //            }
        }

        public void Acquire(ImageBuffer imageBuffer, int cameraIndex)
        {
            int numDevice = CameraHandler.NumCamera;

            Image2D image = imageBuffer.GetImage(cameraIndex, 0);
            if (image != null)
            {
                image.Clear();
            }

            Camera camera = CameraHandler.GetCamera(cameraIndex);
            if (camera != null)
            {
                camera.GrabOnce();

                bool grabOk = CameraHandler.WaitGrabDone(new int[] { cameraIndex }, 5000);
                if (grabOk)
                {
                    ImageD grabbedImage = camera.GetGrabbedImage();
                    image.CopyFrom(grabbedImage);
                }
                else
                {
#if DEBUG == flase
                    ErrorManager.Instance().Report((int)ErrorSection.Grabber, ErrorLevel.Fatal, ErrorSection.Grabber.ToString(), "Grab Fail", "Grab Fail Message");
#endif
                    //throw new AlarmException("Grab Fail Alaram");
                }
            }
        }

        public void Acquire(ImageBuffer imageBuffer, LightParam lightParam)
        {
            int numDevice = CameraHandler.NumCamera;

            if (lightParam.LightParamType == LightParamType.Value)
            {
                if (lightCtrlHandler != null)
                {
                    lightCtrlHandler.TurnOn(lightParam);
                }
            }

            for (int cameraIndex = 0; cameraIndex < numDevice; cameraIndex++)
            {
                Image2D image = imageBuffer.GetImage(cameraIndex, lightParam.Index);
                if (image != null)
                {
                    image.Clear();
                }

                if (lightParam.LightParamType == LightParamType.Calculation)
                {
                    Image2D firstImage = imageBuffer.GetImage(cameraIndex, lightParam.FirstImageIndex).GetGrayImage();
                    Image2D secondImage = imageBuffer.GetImage(cameraIndex, lightParam.SecondImageIndex).GetGrayImage();

                    ImageOperation.Operate(lightParam.OperationType, firstImage, secondImage, image);
                }
                else
                {
                    Camera camera = CameraHandler.GetCamera(cameraIndex);
                    if (camera != null)
                    {
                        camera.GrabOnce();

                        bool grabOk = CameraHandler.WaitGrabDone(new int[] { cameraIndex }, 5000);
                        if (grabOk)
                        {
                            ImageD grabbedImage = camera.GetGrabbedImage();
                            image.CopyFrom(grabbedImage);
                        }
                        else
                        {
#if DEBUG == flase
                            ErrorManager.Instance().Report((int)ErrorSection.Grabber, ErrorLevel.Fatal, ErrorSection.Grabber.ToString(), "Grab Fail", "Grab Fail Message");
#endif
                            //throw new AlarmException("Grab Fail Alaram");
                        }
                    }
                }
            }
        }

        public void Acquire(ImageBuffer imageBuffer, LightParamSet lightParamSet)
        {
            var lightTypeList = new List<int>();
            for (int lightTypeIndex = 0; lightTypeIndex < lightParamSet.NumLightType; lightTypeIndex++)
            {
                lightTypeList.Add(lightTypeIndex);
            }

            Acquire(imageBuffer, lightParamSet, lightTypeList.ToArray());
        }

        // 하나의 조명 조건으로 여러 카메라의 영상을 획득한다.
        public void Acquire(ImageBuffer imageBuffer, LightParamSet lightParamSet, int[] lightTypeArr, int[] camIndexArr = null)
        {
            if (camIndexArr == null)
            {
                camIndexArr = CameraHandler.GetCameraIndexArr();
            }

            foreach (int lightTypeIndex in lightTypeArr)
            {
                LightParam lightParam = lightParamSet[lightTypeIndex];
                LightValue lightValue = lightParam.LightValue;
                if (lightCtrlHandler != null)
                {
                    lightCtrlHandler.TurnOn(lightValue);
                }

                CameraHandler.GrabOnce(camIndexArr);

                bool grabOk = CameraHandler.WaitGrabDone(camIndexArr, 5000);

                if (lightCtrlHandler != null)
                {
                    lightCtrlHandler.TurnOff();
                }

                if (grabOk)
                {
                    foreach (int cameraIndex in camIndexArr)
                    {
                        Camera camera = CameraHandler.GetCamera(cameraIndex);
                        if (camera != null)
                        {
                            Image2D image = imageBuffer.GetImage(cameraIndex, lightTypeIndex);
                            image.CopyFrom(camera.GetGrabbedImage());
                        }
                    }
                }
            }

            CameraHandler.Stop(camIndexArr);
        }
    }
}
