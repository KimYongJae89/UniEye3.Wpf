using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem
{
    public class ImageSequenceExyma : ImageSequence
    {
        public bool MotorOnChanged { get; set; } = false;

        //int numImage;
        private CameraPylon cameraPylon;
        private ManualResetEvent scanDoneEvent = new ManualResetEvent(true);
        public ExymaScannerInfo ExymaScannerInfo { get; set; }
        public ImageSequenceExyma()
        {

        }

        ~ImageSequenceExyma()
        {

        }

        public override void Initialize(Camera camera)
        {
            if (camera is CameraPylon)
            {
                cameraPylon = (CameraPylon)camera;
            }
        }

        public override void Scan(int numImage)
        {
            if (scanDoneEvent.WaitOne(0) == false)
            {
                LogHelper.Debug(LoggerType.Grab, "Scan is on processing");
                return;
            }

            if (imageList.Count != numImage)
            {
                imageList.Clear();
                for (int i = 0; i < numImage; i++)
                {
                    imageList.Add(cameraPylon.CreateCompatibleImage());
                }
            }

            imageIndex = 0;
            scanDoneEvent.Reset();

            cameraPylon.ImageGrabbed += ImageGrabbed;
            cameraPylon.SetExposureTime(ExymaScannerInfo.GetExploseTimeUs());
            cameraPylon.SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);

            cameraPylon.GrabMulti();

            cameraPylon.WriteOutputGroup(0, 1);

            if (ExymaScannerInfo.BoardSettingData[(int)EEPROM.MAON] == 0)
            {
                TimerHelper.Sleep(1000);
            }
            else
            {
                TimerHelper.Sleep(15);
            }

            cameraPylon.WriteOutputGroup(0, 0);
        }

        public bool WaitScanDone()
        {
            bool result = scanDoneEvent.WaitOne(5000);
            if (result == false)
            {
                scanDoneEvent.Set();
            }

            return result;
        }

        private void SequenceDone(ImageDevice imageDevice)
        {
            LogHelper.Debug(LoggerType.Grab, "Sequence Done");

            cameraPylon.ImageGrabbed -= ImageGrabbed;

            ScanDone?.Invoke();

            scanDoneEvent.Set();
        }

        private void ImageGrabbed(Camera camera)
        {
            if (ImageScanned != null)
            {
                ImageD grabbedImage = camera.GetGrabbedImage();
#if DEBUG
                //if (BaseConfig.Instance().TempPath != null)
                //{
                //    string imgePath = Path.Combine(BaseConfig.Instance().TempPath, String.Format("Grating_{0}.bmp", grabbedImage));
                //    grabbedImage.SaveImage(imgePath, ImageFormat.Bmp);
                //}
#endif
                imageList[imageIndex].CopyFrom(grabbedImage);

                imageIndex++;

                //if (imageIndex >= numImage)
                //{
                //    Stop();
                //}
                //else
                {
                    ImageScanned(grabbedImage);
                }
            }
        }

        public override bool IsScanDone()
        {
            return (scanDoneEvent.WaitOne(0) == true);
        }

        public override void Stop()
        {
            if (IsScanDone() == false)
            {
                cameraPylon.ImageGrabbed -= ImageGrabbed;

                cameraPylon.Stop();

                cameraPylon.SetTriggerMode(TriggerMode.Software);

                ScanDone?.Invoke();

                scanDoneEvent.Set();
            }
        }
    }
}
