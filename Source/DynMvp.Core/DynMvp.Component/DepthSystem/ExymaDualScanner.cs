using DynMvp.Base;
using DynMvp.Component.DepthSystem;
using DynMvp.Component.DepthSystem.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem
{
    public class ExymaDualScanner : ExymaScanner
    {
        private const int numBucket1 = 7;
        private const int numBucket2 = 5;
        private const int maxMeasureSequence = 4;
        private int activeCameraIndex = -1;
        private CameraPylon centerCamera;
        private CameraPylon[] cameraPylon = new CameraPylon[2];
        public ExymaScannerInfo ExymaScannerInfo { get; private set; }

        private Image3D[] referenceData = new Image3D[2];
        private Image3D[] scanBuffer = new Image3D[2];
        private Image3D[] imageBuffer = new Image3D[2];
        private Image3D[] zCalibrationData = new Image3D[2];
        private LensCalibration[] lensCalibration = new LensCalibration[2];
        private PmpData[][] pmpData = new PmpData[2][];
        private PmpAlgorithm[] pmpAlgorithm = new PmpAlgorithm[2];
        private ImageSequenceExyma[] imageSequence = new ImageSequenceExyma[2];
        private ImageMapper imageMapper = new ImageMapper();
        private ExymaController[] exymaController = new ExymaController[2];
        private bool onLive = false;

        public override bool Initialize(DepthScannerInfo depthScannerInfo, CameraHandler cameraHandler)
        {
            ExymaScannerInfo = (ExymaScannerInfo)depthScannerInfo;

            centerCamera = cameraHandler.GetCamera(ExymaScannerInfo.CenterCameraIndex) as CameraPylon;

            Initialize(0, depthScannerInfo, cameraHandler);
            Initialize(1, depthScannerInfo, cameraHandler);

            return true;
        }

        public bool Initialize(int scannerIdx, DepthScannerInfo depthScannerInfo, CameraHandler cameraHandler)
        {
            referenceData[scannerIdx] = new Image3D();
            scanBuffer[scannerIdx] = new Image3D();
            imageBuffer[scannerIdx] = new Image3D();
            zCalibrationData[scannerIdx] = new Image3D();
            lensCalibration[scannerIdx] = new LensCalibration();

            imageSequence[scannerIdx] = new ImageSequenceExyma();

            ExymaScannerInfo = (ExymaScannerInfo)depthScannerInfo;

            exymaController[scannerIdx] = new ExymaController();
            if (scannerIdx == 0)
            {
                exymaController[scannerIdx].Initialize(ExymaScannerInfo, ExymaScannerInfo.ControlBoardSerialInfo);
            }
            else
            {
                exymaController[scannerIdx].Initialize(ExymaScannerInfo, ExymaScannerInfo.ControlBoardSerialInfo2);
            }

            exymaController[scannerIdx].SetRamData(EEPROM.EPCN, ExymaScannerInfo.BoardSettingData[(int)EEPROM.EPCN]);

            Camera camera = null;

            if (scannerIdx == 0)
            {
                camera = cameraHandler.GetCamera(ExymaScannerInfo.CameraIndex);
            }
            else
            {
                camera = cameraHandler.GetCamera(ExymaScannerInfo.Camera2Index);
            }

            if (camera == null)
            {
                return false;
            }

            if ((camera is CameraPylon) == false)
            {
                return false;
            }

            cameraPylon[scannerIdx] = camera as CameraPylon;

            ImageSize = cameraPylon[scannerIdx].ImageSize;
            referenceData[scannerIdx].Initialize(ImageSize.Width, ImageSize.Height, ExymaScannerInfo.GetPmpCount());
            scanBuffer[scannerIdx].Initialize(ImageSize.Width, ImageSize.Height, ExymaScannerInfo.GetMaxImageCount());
            imageBuffer[scannerIdx].Initialize(ImageSize.Width, ImageSize.Height, numBucket1 + numBucket2);
            zCalibrationData[scannerIdx].Initialize(ImageSize.Width, ImageSize.Height, 4);

            referenceData[scannerIdx].LoadRaw(ExymaScannerInfo.GetReferencePath(scannerIdx));
            zCalibrationData[scannerIdx].LoadRaw(ExymaScannerInfo.GetZCalibrationPath(scannerIdx));

            pmpData[scannerIdx] = new PmpData[4];

            int index = 0;
            for (int i = 0; i < maxMeasureSequence; i++)
            {
                pmpData[scannerIdx][i] = ExymaScannerInfo.GetPmpData(i);
                if (pmpData[scannerIdx][i].enable)
                {
                    pmpData[scannerIdx][i].referenceData = (Image3D)referenceData[scannerIdx].GetLayer(index);
                    index++;

                    pmpData[scannerIdx][i].zCalibrationData = zCalibrationData[scannerIdx];
                }
            }

            lensCalibration[scannerIdx].ImageSize = ImageSize;
            lensCalibration[scannerIdx].Load(ExymaScannerInfo.GetLensCalibrationPath(scannerIdx));

            pmpAlgorithm[scannerIdx] = new PmpAlgorithm();
            pmpAlgorithm[scannerIdx].initialize(ImageSize.Width, ImageSize.Height, pmpData[scannerIdx], lensCalibration[scannerIdx]);

            imageSequence[scannerIdx] = new ImageSequenceExyma();
            imageSequence[scannerIdx].Initialize(cameraPylon[scannerIdx]);
            imageSequence[scannerIdx].ExymaScannerInfo = ExymaScannerInfo;
            imageSequence[scannerIdx].ImageScanned += imageSequence_ImageScanned;
            imageSequence[scannerIdx].ScanDone += imageSequence_ScanDone;

            return true;
        }

        public override int GetNumScanImage(int cameraIndex)
        {
            if (cameraIndex == 0)
            {
                return 48;
            }
            else
            {
                return 24;
            }
        }

        private void imageSequence_ImageScanned(ImageD image)
        {
            ImageScanned?.Invoke(image);
        }

        private void imageSequence_ScanDone()
        {
            ScanDone?.Invoke();
        }

        private void BuildPmpBuffer(int scannerIdx, List<ImageD> imageList)
        {
            LogHelper.Debug(LoggerType.Grab, "BuildPmpBuffer");

            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                if (pmpData[scannerIdx][i].enable)
                {
                    pmpData[scannerIdx][i].imageBufferList.Clear();
                    for (int j = 0; j < pmpData[scannerIdx][i].bucket; j++)
                    {
                        pmpData[scannerIdx][i].imageBufferList.Add((Image2D)imageList[index]);
                        index++;
                    }
                }
            }
        }

        private Image2D Grab2D(CameraPylon camera, float exposure2d)
        {
            camera.SetExposureTime(exposure2d);

            camera.ImageGrabbed += Pylon_ImageGrabbed;
            camera.GrabOnce();
            camera.WaitGrabDone();

            var grabImage = (Image2D)camera.GetGrabbedImage();

            camera.Stop();
            camera.SetTriggerMode(TriggerMode.Software);

            return grabImage;
        }

        public override bool Calibrate(float targetRadius2d, float targetRadius3d, float exposure2d, TransformDataList transformDataList)
        {
            TransformData transformData;

            Image2D image2d;
            if (centerCamera != null)
            {
                transformData = new TransformData();
                //image2d = Grab2D(centerCamera, exposure2d);
                //if (Calibrate2D(image2d, targetRadius2d, transformData) == false)
                //    return false;

                transformData.CameraIndex = centerCamera.Index;
                transformDataList.Add(transformData);
            }

            transformData = new TransformData();
            image2d = Grab2D(cameraPylon[0], exposure2d);
            if (Calibrate3D(0, image2d, targetRadius3d, transformData) == false)
            {
                return false;
            }

            transformData.CameraIndex = cameraPylon[0].Index;
            transformDataList.Add(transformData);

            transformData = new TransformData();
            image2d = Grab2D(cameraPylon[1], exposure2d);
            if (Calibrate3D(1, image2d, targetRadius3d, transformData) == false)
            {
                return false;
            }

            transformData.CameraIndex = cameraPylon[1].Index;
            transformDataList.Add(transformData);

            return true;
        }

        public override bool Calibrate2D(Image2D image2d, float targetRadius, TransformData transformData)
        {
            return Calibrate(centerCamera, image2d, targetRadius, null, null, transformData);
        }

        public override bool Calibrate3D(int scannerIdx, Image2D image2d, float targetDiameter, TransformData transformData)
        {
            Grab3D(scannerIdx);

            Image3D image3d = CreateDepthImage(scannerIdx);
            RoiInfo roiInfo = Calculate(scannerIdx, image3d, Rectangle.Empty);

            CameraPylon camera = cameraPylon[scannerIdx];

            return Calibrate(camera, image2d, targetDiameter, image3d, lensCalibration[scannerIdx], transformData);
        }

        private bool Calibrate(Camera camera, Image2D image2d, float targetRadius, Image3D image3d, LensCalibration lensCalibration, TransformData transformData)
        {
            var calibrationImage = (Image2D)camera.CreateCompatibleImage();

            var imageHalfSize = new Size(image2d.Width / 2, image2d.Height / 2);

            var rectangleList = new Rectangle[4]
            {
                new Rectangle(new Point(0, 0),  imageHalfSize), new Rectangle(new Point(imageHalfSize.Width, 0),  imageHalfSize),
                new Rectangle(new Point(0, imageHalfSize.Height),  imageHalfSize), new Rectangle(new Point(imageHalfSize.Width, imageHalfSize.Height),  imageHalfSize)
            };

            CircleDetector circleDetector = new OpenCvCircleDetector();

            var param = new CircleDetectorParam();
            param.InnerRadius = targetRadius * 0.8f;
            param.OutterRadius = targetRadius * 1.2f;
            circleDetector.Param = param;

            var pointList = new List<Point3d>();
            pointList.Add(new Point3d(80, 80, 0));
            pointList.Add(new Point3d(-80, 80, 0));
            pointList.Add(new Point3d(-80, -80, 0));
            pointList.Add(new Point3d(80, -80, 0));

            var transform3d = new Transform3d();
            transform3d.SetOrigin(pointList.ToArray());

            var foundPointList = new List<Point3d>();
            var foundPxPointList = new List<PointF>();

            foreach (RectangleF rect in rectangleList)
            {
                ImageD clipImage = image2d.ClipImage(Rectangle.Ceiling(rect));
                clipImage.SaveImage("E:\\Temp\\Clip.bmp", ImageFormat.Bmp);

                AlgoImage algoImage = ImageBuilder.OpenCvImageBuilder.Build(clipImage, ImageType.Grey);

                CircleEq circleEq = circleDetector.Detect(algoImage, new DebugContext(true, "E:\\Temp"));
                if (circleEq != null)
                {
                    var circlePoint = new PointF((rect.Left + circleEq.Center.X), (rect.Top + circleEq.Center.Y));

                    foundPxPointList.Add(circlePoint);

                    if (lensCalibration != null)
                    {
                        float[] height = image3d.GetRangeValue(Point.Round(circlePoint));
                        lensCalibration.CalculateMetricXY(circlePoint.X, circlePoint.Y, height[0], out double realPointX, out double realPointY);

                        foundPointList.Add(new Point3d(realPointX, realPointY, height[0]));
                    }
                    else
                    {
                        foundPointList.Add(new Point3d(circlePoint.X, circlePoint.Y, 0));
                    }
                }
            }

            if (foundPointList.Count < 4)
            {
                MessageBox.Show(StringManager.GetString("Can't find circles."));
            }

            if (foundPointList.Count > 0)
            {
                var circleListForm = new CircleListForm();
                circleListForm.Initialize(foundPointList, image2d, foundPxPointList);
                if (circleListForm.ShowDialog() == DialogResult.OK && foundPointList.Count == 4)
                {
                    transform3d.FindRT(circleListForm.PointArray);
                    transform3d.GetMapData(transformData);

                    return true;
                }
            }

            return false;
        }

        public Size GetMinImageSize()
        {
            Size size1 = cameraPylon[0].ImageSize;
            Size size2 = cameraPylon[1].ImageSize;

            return new Size(Math.Min(size1.Width, size2.Width), Math.Min(size1.Height, size2.Height));
        }

        public Size GetImageSize(int scannerIdx)
        {
            return cameraPylon[scannerIdx].ImageSize;
        }

        public override Image3D Calculate(Rectangle rectangle, float pixelRes)
        {
            return Calculate(rectangle, ExymaScannerInfo.TransformDataList, pixelRes);
        }

        public override Image3D Calculate(Rectangle rectangle, TransformDataList transformDataList, float pixelRes)
        {
            Image3D image1 = CreateDepthImage(0);
            Image3D image2 = CreateDepthImage(1);

            if (rectangle == Rectangle.Empty)
            {
                rectangle = new Rectangle(0, 0, image1.Width, image1.Height);
            }

            RoiInfo roi1 = Calculate(0, image1, rectangle);
            RoiInfo roi2 = Calculate(1, image2, rectangle);

            Point3d[] pointArray1 = roi1.ToArray();
            Point3d[] pointArray2 = roi2.ToArray();

            var transform3d = new Transform3d();
            transform3d.SetTransformData(transformDataList.GetTransformData(ExymaScannerInfo.CameraIndex));
            transform3d.Transform(pointArray1);

            transform3d.SetTransformData(transformDataList.GetTransformData(ExymaScannerInfo.Camera2Index));
            transform3d.Transform(pointArray2);

            Point3d[] pointArray = pointArray1.Concat(pointArray2).ToArray();

            var rectSize = new SizeF(rectangle.Width * pixelRes, rectangle.Height * pixelRes);
            var mappingRect = new RectangleF(-rectSize.Width / 2, -rectSize.Height / 2, rectSize.Width, rectSize.Height);

            imageMapper.Initialize(mappingRect, pixelRes);
            imageMapper.Mapping(pointArray, true);

            imageMapper.Image3d.MappingRect = mappingRect;
            imageMapper.Image3d.PointArray = pointArray;

            return imageMapper.Image3d;
        }

        public Image3D Calculate(int scannerIdx, Rectangle rectangle, float pixelRes)
        {
            //            Grab3D(scannerIdx);

            Image3D image = CreateDepthImage(scannerIdx);
            RoiInfo roiInfo = Calculate(scannerIdx, image, rectangle);

            TransformData transformData;
            if (scannerIdx == 0)
            {
                transformData = ExymaScannerInfo.TransformDataList.GetTransformData(ExymaScannerInfo.CameraIndex);
            }
            else
            {
                transformData = ExymaScannerInfo.TransformDataList.GetTransformData(ExymaScannerInfo.Camera2Index);
            }

            if (transformData.IsInitialized())
            {
                LogHelper.Debug(LoggerType.Grab, "ToArray");

                Point3d[] pointArray = roiInfo.ToArray();

                var transform3d = new Transform3d();
                transform3d.SetTransformData(transformData);
                transform3d.Transform(pointArray);

                var rectSize = new SizeF(rectangle.Width * pixelRes, rectangle.Height * pixelRes);
                var mappingRect = new RectangleF(-rectSize.Width / 2, -rectSize.Height / 2, rectSize.Width, rectSize.Height);

                imageMapper.Initialize(mappingRect, pixelRes);
                imageMapper.Mapping(pointArray, true);

                imageMapper.Image3d.MappingRect = mappingRect;
                imageMapper.Image3d.PointArray = pointArray;

                return imageMapper.Image3d;
            }
            else
            {
                return image;
            }
        }

        public override void SetRamData(EEPROM eeprom, uint value)
        {
            ExymaScannerInfo.BoardSettingData[(int)eeprom] = value;

            exymaController[0].SetRamData(eeprom, value);
            exymaController[1].SetRamData(eeprom, value);
        }

        private RoiInfo Calculate(int scannerIdx, Image3D image, Rectangle rectangle)
        {
            LogHelper.Debug(LoggerType.Grab, "Calculate");

            List<ImageD> imageList = imageSequence[scannerIdx].ImageList;
            BuildPmpBuffer(scannerIdx, imageList);

            if (rectangle == Rectangle.Empty)
            {
                rectangle = new Rectangle(0, 0, image.Width, image.Height);
            }
            var roiInfo = new RoiInfo(rectangle);
            pmpAlgorithm[scannerIdx].Calculate(pmpData[scannerIdx], roiInfo);

            image.SetData(roiInfo.ZHeight);

            AlgoImage algoImage = ImageBuilder.OpenCvImageBuilder.Build(image, ImageType.Depth);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
            imageProcessing.Median(algoImage, null, 5);
            imageProcessing.Average(algoImage);

            var filterredImage = (Image3D)algoImage.ToImageD();

            image.SetData(filterredImage.Data);

            return roiInfo;
        }

        public override void Release()
        {
            base.Release();
        }

        public override bool IsCompatibleImage(ImageD image)
        {
            return centerCamera.IsCompatibleImage(image);
        }

        public override bool IsCompatibleImage(int scannerIdx, ImageD image)
        {
            return cameraPylon[scannerIdx].IsCompatibleImage(image);
        }

        public override ImageD CreateCompatibleImage()
        {
            return centerCamera.CreateCompatibleImage();
        }

        public override ImageD CreateCompatibleImage(int scannerIdx)
        {
            return cameraPylon[scannerIdx].CreateCompatibleImage();
        }

        public override Image3D CreateDepthImage()
        {
            Size imageSize = centerCamera.ImageSize;
            var image3d = new Image3D();
            image3d.Initialize(imageSize.Width, imageSize.Height, 1);

            return image3d;
        }

        public Image3D CreateDepthImage(int scannerIdx)
        {
            LogHelper.Debug(LoggerType.Grab, "CreateDepthImage");

            Size imageSize = cameraPylon[scannerIdx].ImageSize;
            var image3d = new Image3D();
            image3d.Initialize(imageSize.Width, imageSize.Height, 1);

            return image3d;
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            centerCamera.SetTriggerDelay(triggerDelayUs);
        }

        public override void SetExposureTime(float exposureTimeUs)
        {
            centerCamera.SetExposureTime(exposureTimeUs);
        }

        public override void SetExposureTime3d(float exposureTime3dUs)
        {
            int exposureCount = (int)((exposureTime3dUs / 1000) / 14) * 14;
            if (exposureCount < 14)
            {
                exposureCount = 14;
            }

            ExymaScannerInfo.BoardSettingData[(int)EEPROM.EPCN] = (uint)exposureCount;

            exymaController[0].SetRamData(EEPROM.EPCN, (uint)exposureCount);
            exymaController[1].SetRamData(EEPROM.EPCN, (uint)exposureCount);

            //cameraPylon[0].SetExposureTime(exposureTime3dUs);
            //cameraPylon[1].SetExposureTime(exposureTime3dUs);
        }

        public override void SetExposureTime(int scannerIdx, float exposureTimeUs)
        {
            cameraPylon[scannerIdx].SetExposureTime(exposureTimeUs);
        }

        public override void GrabOnce()
        {
            centerCamera.SetTriggerMode(TriggerMode.Software);

            centerCamera.ImageGrabbed += Pylon_ImageGrabbed;
            centerCamera.GrabOnce();
        }

        public override void GrabOnce(int scannerIdx)
        {
            //            cameraPylon[scannerIdx].SetTriggerMode(TriggerMode.Software);

            cameraPylon[scannerIdx].ImageGrabbed += Pylon_ImageGrabbed;
            cameraPylon[scannerIdx].GrabOnce();
        }

        private void Pylon_ImageGrabbed(Camera camera)
        {
            ImageGrabbed?.Invoke(camera);

            if (onLive == false)
            {
                camera.ImageGrabbed -= Pylon_ImageGrabbed;
            }
        }

        public override void Grab3D()
        {
            imageSequence[0].Scan(24);
            imageSequence[0].WaitScanDone();

            cameraPylon[0].Stop();
            cameraPylon[0].SetTriggerMode(TriggerMode.Software);

            imageSequence[1].Scan(24);
            imageSequence[1].WaitScanDone();

            cameraPylon[1].Stop();
            cameraPylon[1].SetTriggerMode(TriggerMode.Software);
        }

        public override void Grab3D(int scannerIndex)
        {
            if (scannerIndex == -1)
            {
                imageSequence[0].Scan(24);
                imageSequence[0].WaitScanDone();

                cameraPylon[0].Stop();
                cameraPylon[0].SetTriggerMode(TriggerMode.Software);

                imageSequence[1].Scan(24);
                imageSequence[1].WaitScanDone();

                cameraPylon[1].Stop();
                cameraPylon[1].SetTriggerMode(TriggerMode.Software);
            }
            else
            {
                imageSequence[scannerIndex].Scan(24);
                imageSequence[scannerIndex].WaitScanDone();

                cameraPylon[scannerIndex].Stop();
                cameraPylon[scannerIndex].SetTriggerMode(TriggerMode.Software);
            }
        }

        public override void GrabMulti(int grabCount = -1)
        {
            if (grabCount == -1)
            {
                onLive = true;
            }

            centerCamera.ImageGrabbed += Pylon_ImageGrabbed;
            centerCamera.GrabMulti();

            activeCameraIndex = 0;
        }

        public override Image3D ScanMeasure(int scannerIndex, float pixelResolution)
        {
            Image3D image3d;
            if (scannerIndex == -1)
            {
                Grab3D();
                image3d = Calculate(new Rectangle(0, 0, ImageSize.Width, ImageSize.Height), pixelResolution);
            }
            else
            {
                Size imageSize = GetImageSize(scannerIndex);

                Grab3D(scannerIndex);
                image3d = Calculate(scannerIndex, new Rectangle(0, 0, imageSize.Width, imageSize.Height), pixelResolution);

                cameraPylon[scannerIndex].Stop();
            }

            return image3d;
        }

        public override void GrabMulti(int scannerIndex, int grabCount = -1)
        {
            if (grabCount == -1)
            {
                onLive = true;
            }

            cameraPylon[scannerIndex].ImageGrabbed += Pylon_ImageGrabbed;
            cameraPylon[scannerIndex].GrabMulti();

            activeCameraIndex = scannerIndex + 1;
        }

        public override void Stop()
        {
            onLive = false;

            if (imageSequence[0].IsScanDone() == false)
            {
                imageSequence[0].Stop();
            }

            if (imageSequence[1].IsScanDone() == false)
            {
                imageSequence[1].Stop();
            }

            if (activeCameraIndex > -1)
            {
                if (activeCameraIndex == 0)
                {
                    centerCamera.ImageGrabbed -= Pylon_ImageGrabbed;
                    centerCamera.Stop();
                }
                else
                {
                    cameraPylon[activeCameraIndex - 1].ImageGrabbed -= Pylon_ImageGrabbed;
                    cameraPylon[activeCameraIndex - 1].Stop();
                }
            }
        }

        public override void Reset()
        {
            centerCamera.Reset();
        }

        public override bool IsGrabDone()
        {
            return centerCamera.IsGrabDone();
        }

        public override bool IsOnLive()
        {
            return onLive;
        }

        public override ImageD GetGrabbedImage()
        {
            return centerCamera.GetGrabbedImage();
        }

        public override bool SetGain(float gain)
        {
            return true;
        }
    }
}
