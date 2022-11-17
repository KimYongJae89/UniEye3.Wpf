using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraHandler
    {
        public static int DefaultTimeoutMs { get; set; } = 5000;

        public List<Camera> cameraList = new List<Camera>();

        public IEnumerator<Camera> GetEnumerator()
        {
            return cameraList.GetEnumerator();
        }

        public int NumCamera => cameraList.Count;

        public void AddCamera(Grabber grabber, CameraConfiguration cameraConfiguration)
        {
            int index = 1;
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                bool cameraInitializeFailed = false;

                Camera camera = null;

                LogHelper.Debug(LoggerType.StartUp, string.Format("Initialize camera [{0}]", cameraInfo.Index));

                if (cameraInfo.Enabled == true)
                {
                    camera = grabber.CreateCamera();
                    if (camera != null)
                    {
                        grabber.UpdateCameraInfo(cameraInfo);

                        try
                        {
                            camera.Initialize(cameraInfo);
                        }
#if !DEBUG
                        catch (CameraInitializeFailedException)
                        {
                            LogHelper.Debug(LoggerType.StartUp, String.Format("Can't find device. Virtual camera[{0}] opened.", cameraInfo.Index));

                            camera = null;
                            cameraInfo.Enabled = false;
                            cameraInitializeFailed = true;
                        }
#endif
                        finally { }
                    }
                }

                if (camera == null && (cameraInitializeFailed == true || cameraInfo.GrabberType == GrabberType.Virtual || cameraInfo.Enabled == false))
                {
                    camera = new CameraVirtual();
                    camera.Initialize(cameraInfo);

                    LogHelper.Debug(LoggerType.StartUp, string.Format("Virtual camera[{0}] opened", cameraInfo.Index));
                }

                if (camera != null)
                {
                    AddCamera(camera);
                    camera.Name = string.Format("Camera {0}", index);
                    index++;
                }
            }
        }

        public void AddCamera(Camera camera)
        {
            camera.Index = cameraList.Count;
            cameraList.Add(camera);
        }

        public void AddCamera(CameraHandler cameraHandler)
        {
            foreach (Camera camera in cameraHandler)
            {
                AddCamera(camera);
            }
        }

        public Camera GetCamera(int cameraIndex)
        {
            if (cameraIndex >= 0 && cameraIndex < cameraList.Count)
            {
                return cameraList[cameraIndex];
            }

            return null;
        }

        public void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            foreach (Camera camera in cameraList)
            {
                camera.SetTriggerMode(triggerMode, triggerType);
            }
        }

        public void SetTriggerDelay(int triggerDelay)
        {
            foreach (Camera camera in cameraList)
            {
                camera.SetTriggerDelay(triggerDelay);
            }
        }

        public List<ImageD> GrabOnce(int[] deviceIndexArr)
        {
            var imageList = new List<ImageD>();

            foreach (int index in deviceIndexArr)
            {
                ImageD image = cameraList[index].GrabOnce();

                if (image != null)
                {
                    imageList.Add(image);
                }
            }

            return imageList;
        }

        public List<ImageD> GrabOnce(int cameraIndex = -1)
        {
            var imageList = new List<ImageD>();

            if (cameraIndex == -1)
            {
                foreach (Camera camera in cameraList)
                {
                    ImageD image = camera.GrabOnce();
                    if (image != null)
                    {
                        imageList.Add(image);
                    }
                }
            }
            else
            {
                ImageD image = cameraList[cameraIndex].GrabOnce();
                if (image != null)
                {
                    imageList.Add(image);
                }
            }

            return imageList;
        }

        public void GrabMulti(int[] deviceIndexArr)
        {
            foreach (int index in deviceIndexArr)
            {
                cameraList[index].GrabMulti();
            }
        }

        public void GrabMulti(int cameraIndex = -1)
        {
            if (cameraIndex == -1)
            {
                foreach (Camera camera in cameraList)
                {
                    camera.GrabMulti();
                }
            }
            else
            {
                cameraList[cameraIndex].GrabMulti();
            }
        }

        public void Stop(int[] deviceIndexArr)
        {
            foreach (int index in deviceIndexArr)
            {
                cameraList[index].Stop();
            }
        }

        public void Stop(int cameraIndex = -1)
        {
            if (cameraIndex == -1)
            {
                foreach (Camera camera in cameraList)
                {
                    camera.Stop();
                }
            }
            else
            {
                cameraList[cameraIndex].Stop();
            }
        }

        public bool IsGrabDone()
        {
            foreach (Camera camera in cameraList)
            {
                if (camera.IsGrabDone() == false)
                {
                    return false;
                }
            }

            //Debug.Write("All Grab Done\n");

            return true;
        }

        public bool IsGrabDone(int[] deviceIndexArr)
        {
            foreach (int index in deviceIndexArr)
            {
                if (cameraList[index].IsGrabDone() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool WaitGrabDone(int timeoutMs = 0)
        {
            Thread.Sleep(10);

            if (timeoutMs == 0)
            {
                timeoutMs = DefaultTimeoutMs;
            }

            LogHelper.Debug(LoggerType.Grab, "WaitGrabDone Start");

            for (int i = 0; i < timeoutMs / 10; i++)
            {
                if (IsGrabDone())
                {
                    LogHelper.Debug(LoggerType.Grab, "WaitGrabDone Ok");

                    return true;
                }

                //Thread.Sleep(10);
            }

            LogHelper.Debug(LoggerType.Error, "WaitGrabDone Fail");

            foreach (Camera camera in cameraList)
            {
                if (camera.IsGrabDone() == false)
                {
                    camera.GrabFailed = true;
                }
            }

            return false;
        }

        public bool WaitGrabDone(int[] deviceIndexArr, int timeoutMs = 0)
        {
            if (timeoutMs == 0)
            {
                timeoutMs = DefaultTimeoutMs;
            }

            LogHelper.Debug(LoggerType.Grab, "WaitGrabDone Start");

            for (int i = 0; i < timeoutMs / 10; i++)
            {
                if (IsGrabDone(deviceIndexArr))
                {
                    LogHelper.Debug(LoggerType.Grab, "WaitGrabDone Ok");

                    return true;
                }

                Thread.Sleep(10);
            }

            LogHelper.Debug(LoggerType.Error, "WaitGrabDone Fail");

            foreach (int index in deviceIndexArr)
            {
                if (cameraList[index].IsGrabDone() == false)
                {
                    cameraList[index].GrabFailed = true;
                }
            }

            return false;
        }

        public void BuildImageList(int[] deviceIndexArr, List<ImageD> imageList)
        {
            imageList.Clear();

            foreach (int index in deviceIndexArr)
            {
                imageList.Add(cameraList[index].CreateCompatibleImage());
            }
        }

        public int[] GetCameraIndexArr()
        {
            var camIndexList = new List<int>();
            cameraList.ForEach(x => camIndexList.Add(x.Index));

            return camIndexList.ToArray();
        }

        public void BuildImageList(List<ImageD> imageList)
        {
            imageList.Clear();

            foreach (Camera camera in cameraList)
            {
                imageList.Add(camera.CreateCompatibleImage());
            }
        }

        public void GetGrabImage(int[] deviceIndexArr, List<ImageD> imageList)
        {
            if (imageList.Count != deviceIndexArr.Length)
            {
                return;
            }

            foreach (int index in deviceIndexArr)
            {
                ImageD grabbedImage = cameraList[index].GetGrabbedImage();
                imageList[index].CopyFrom(grabbedImage);
            }
        }

        public void GetGrabImage(List<ImageD> imageList)
        {
            if (imageList.Count != cameraList.Count)
            {
                return;
            }

            for (int deviceIndex = 0; deviceIndex < cameraList.Count; deviceIndex++)
            {
                Debug.Assert(imageList[deviceIndex] != null);
                ImageD grabbedImage = cameraList[deviceIndex].GetGrabbedImage();
                imageList[deviceIndex].CopyFrom(grabbedImage);
            }
        }

        public void SetExposureTime(float exposureUs)
        {
            foreach (Camera camera in cameraList)
            {
                camera.SetExposureTime(exposureUs);
            }
        }

        public bool IsGrabFailed()
        {
            foreach (Camera camera in cameraList)
            {
                if (camera.GrabFailed)
                {
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            foreach (Camera camera in cameraList)
            {
                camera.Reset();
            }
        }

        public virtual void Release()
        {
            foreach (Camera camera in cameraList)
            {
                if (!camera.IsGrabStop())
                {
                    camera.WaitGrabDone();
                }

                camera.Release();
            }
        }
    }
}
