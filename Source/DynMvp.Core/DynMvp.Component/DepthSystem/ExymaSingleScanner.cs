using DynMvp.Base;
using DynMvp.Component.DepthSystem;
using DynMvp.Component.DepthSystem.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem
{
    public class ExymaSingleScanner : ExymaScanner
    {
        private const int numBucket1 = 7;
        private const int numBucket2 = 5;
        private const int maxMeasureSequence = 4;
        private CameraPylon cameraPylon;
        private CameraPylon centerCamera;
        public ExymaScannerInfo ExymaScannerInfo { get; set; }

        private ExymaController exymaController;
        private Image3D referenceData = new Image3D();
        private Image3D scanBuffer = new Image3D();
        private Image3D imageBuffer = new Image3D();
        private Image3D zCalibrationData = new Image3D();
        private LensCalibration lensCalibration = new LensCalibration();
        private PmpData[] pmpData;
        private PmpAlgorithm pmpAlgorithm;
        private ImageSequenceExyma imageSequence;
        private ImageMapper imageMapper = new ImageMapper();
        private bool onLive = false;

        public override bool Initialize(DepthScannerInfo depthScannerInfo, CameraHandler cameraHandler)
        {
            ExymaScannerInfo = (ExymaScannerInfo)depthScannerInfo;

            Camera camera = cameraHandler.GetCamera(ExymaScannerInfo.CenterCameraIndex);
            if (camera != null)
            {
                centerCamera = camera as CameraPylon;
            }

            exymaController = new ExymaController();
            exymaController.Initialize(ExymaScannerInfo, ExymaScannerInfo.ControlBoardSerialInfo);

            pmpData = new PmpData[4];

            camera = cameraHandler.GetCamera(ExymaScannerInfo.CameraIndex);
            if (camera == null)
            {
                return false;
            }

            if ((camera is CameraPylon) == false)
            {
                return false;
            }

            cameraPylon = camera as CameraPylon;

            ImageSize = cameraPylon.ImageSize;
            referenceData.Initialize(ImageSize.Width, ImageSize.Height, ExymaScannerInfo.GetPmpCount());
            scanBuffer.Initialize(ImageSize.Width, ImageSize.Height, ExymaScannerInfo.GetMaxImageCount());
            imageBuffer.Initialize(ImageSize.Width, ImageSize.Height, numBucket1 + numBucket2);
            zCalibrationData.Initialize(ImageSize.Width, ImageSize.Height, 4);

            referenceData.LoadRaw(ExymaScannerInfo.GetReferencePath(0));
            zCalibrationData.LoadRaw(ExymaScannerInfo.GetZCalibrationPath(0));

            int index = 0;
            for (int i = 0; i < maxMeasureSequence; i++)
            {
                pmpData[i] = ExymaScannerInfo.GetPmpData(i);
                if (pmpData[i].enable)
                {
                    pmpData[i].referenceData = (Image3D)referenceData.GetLayer(index);
                    index++;

                    pmpData[i].zCalibrationData = zCalibrationData;
                }
            }

            lensCalibration.Load(ExymaScannerInfo.GetLensCalibrationPath(0));

            pmpAlgorithm = new PmpAlgorithm();
            pmpAlgorithm.initialize(ImageSize.Width, ImageSize.Height, pmpData, lensCalibration);

            imageSequence = new ImageSequenceExyma();
            imageSequence.Initialize(cameraPylon);
            imageSequence.ExymaScannerInfo = ExymaScannerInfo;
            imageSequence.ImageScanned += imageSequence_ImageScanned;
            imageSequence.ScanDone += imageSequence_ScanDone;

            return true;
        }

        public override int GetNumScanImage(int cameraIndex)
        {
            return 24;
        }

        private void imageSequence_ImageScanned(ImageD image)
        {
            ImageScanned?.Invoke(image);
        }

        private void imageSequence_ScanDone()
        {
            ScanDone?.Invoke();
        }

        private void BuildPmpBuffer(List<ImageD> imageList)
        {
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                if (pmpData[i].enable)
                {
                    pmpData[i].imageBufferList.Clear();
                    for (int j = 0; j < pmpData[i].bucket; j++)
                    {
                        pmpData[i].imageBufferList.Add((Image2D)imageList[index]);
                        index++;
                    }
                }
            }
        }

        public override Image3D Calculate(Rectangle rectangle, float pixelRes)
        {
            List<ImageD> imageList = imageSequence.ImageList;
            BuildPmpBuffer(imageList);

            if (rectangle == Rectangle.Empty)
            {
                rectangle = new Rectangle(0, 0, ImageSize.Width, ImageSize.Height);
            }
            var roiInfo = new RoiInfo(rectangle);
            pmpAlgorithm.Calculate(pmpData, roiInfo);

            TransformData transformData = ExymaScannerInfo.TransformDataList.GetTransformData(ExymaScannerInfo.CameraIndex);

            if (transformData != null && transformData.IsInitialized())
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
                var image = new Image3D();
                image.Initialize(rectangle.Width, rectangle.Height, 1);
                image.SetData(roiInfo.ZHeight);

                return image;
            }
        }

        public override Image3D Calculate(Rectangle rectangle, TransformDataList transformDataList, float pixelRes)
        {
            List<ImageD> imageList = imageSequence.ImageList;
            BuildPmpBuffer(imageList);

            if (rectangle == Rectangle.Empty)
            {
                rectangle = new Rectangle(0, 0, ImageSize.Width, ImageSize.Height);
            }
            var roiInfo = new RoiInfo(rectangle);
            pmpAlgorithm.Calculate(pmpData, roiInfo);

            if (transformDataList == null)
            {
                transformDataList = ExymaScannerInfo.TransformDataList;
            }

            TransformData transformData = transformDataList.GetTransformData(ExymaScannerInfo.CameraIndex);

            Image3D image;
            if (transformData?.IsInitialized() == true)
            {
                var transform3d = new Transform3d();
                transform3d.SetTransformData(transformData);

                LogHelper.Debug(LoggerType.Grab, "ToArray");

                Point3d[] pointArray = roiInfo.ToArray();

                transform3d.Transform(pointArray);

                var rectSize = new SizeF(rectangle.Width * pixelRes, rectangle.Height * pixelRes);
                var mappingRect = new RectangleF(-rectSize.Width / 2, -rectSize.Height / 2, rectSize.Width, rectSize.Height);

                imageMapper.Initialize(mappingRect, pixelRes);
                imageMapper.Mapping(pointArray, true);

                image = imageMapper.Image3d;
                image.PointArray = pointArray;
                image.MappingRect = mappingRect;
            }
            else
            {
                image = new Image3D();
                image.Initialize(rectangle.Width, rectangle.Height, 1);
                image.SetData(roiInfo.ZHeight);
            }

            return image;
        }

        public override void Release()
        {

        }

        public override bool IsCompatibleImage(ImageD image)
        {
            return cameraPylon.IsCompatibleImage(image);
        }

        public override bool IsCompatibleImage(int scannerIdx, ImageD image)
        {
            return IsCompatibleImage(image);
        }

        public override ImageD CreateCompatibleImage()
        {
            return cameraPylon.CreateCompatibleImage();
        }

        public override ImageD CreateCompatibleImage(int scannerIdx)
        {
            return CreateCompatibleImage();
        }

        public override Image3D CreateDepthImage()
        {
            Size imageSize = cameraPylon.ImageSize;
            var image3d = new Image3D();
            image3d.Initialize(imageSize.Width, imageSize.Height, 1);

            return image3d;
        }

        public override ImageD GetGrabbedImage()
        {
            return cameraPylon.GetGrabbedImage();
        }

        public override List<ImageD> GetGrabbedImageList()
        {
            return imageSequence.ImageList;
        }

        public override void SetRamData(EEPROM eeprom, uint value)
        {
            ExymaScannerInfo.BoardSettingData[(int)eeprom] = value;

            exymaController.SetRamData(eeprom, value);

            motorOn = (eeprom == EEPROM.MAON) && (value == 1);

            if (motorOn)
            {
                LogHelper.Debug(LoggerType.Grab, "Turn On Output");

                cameraPylon.WriteOutputGroup(0, 1);
                Thread.Sleep(100);
            }
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            cameraPylon.SetTriggerDelay(triggerDelayUs);
        }

        public override void SetExposureTime(float exposureTimeUs)
        {
            cameraPylon.SetExposureTime(exposureTimeUs);
        }

        public override void SetExposureTime(int scannerIdx, float exposureTimeUs)
        {
            SetExposureTime(exposureTimeUs);
        }

        public override void SetExposureTime3d(float exposureTime3dUs)
        {
            int exposureCount = (int)((exposureTime3dUs / 1000) / 14) * 14;
            if (exposureCount < 14)
            {
                exposureCount = 14;
            }

            ExymaScannerInfo.BoardSettingData[(int)EEPROM.EPCN] = (uint)exposureCount;

            exymaController.SetRamData(EEPROM.EPCN, (uint)exposureCount);

            //cameraPylon[0].SetExposureTime(exposureTime3dUs);
            //cameraPylon[1].SetExposureTime(exposureTime3dUs);
        }

        public override void GrabOnce()
        {
            cameraPylon.ImageGrabbed += Pylon_ImageGrabbed;
            cameraPylon.GrabOnce();
        }

        public override void GrabOnce(int scannerIdx)
        {
            GrabOnce();
        }

        private void Pylon_ImageGrabbed(Camera camera)
        {
            ImageGrabbed?.Invoke(camera);

            if (onLive == false)
            {
                cameraPylon.ImageGrabbed -= Pylon_ImageGrabbed;
            }
        }

        public override void Grab3D(int scannerIndex)
        {
            Grab3D();
        }

        public override void Grab3D()
        {
            imageSequence.Scan(24);
            imageSequence.WaitScanDone();

            cameraPylon.Stop();
            cameraPylon.SetTriggerMode(TriggerMode.Software);
        }

        public override void GrabMulti(int grabCount = -1)
        {
            if (grabCount == -1)
            {
                onLive = true;
            }

            cameraPylon.ImageGrabbed += Pylon_ImageGrabbed;
            cameraPylon.GrabMulti();
        }

        public override void GrabMulti(int scannerIndex, int grabCount = -1)
        {
            GrabMulti(scannerIndex, grabCount);
        }

        public override void Stop()
        {
            onLive = false;

            if (imageSequence.IsScanDone() == false)
            {
                imageSequence.Stop();
            }
            else
            {
                cameraPylon.ImageGrabbed -= Pylon_ImageGrabbed;
                cameraPylon.Stop();
            }
        }

        public override void Reset()
        {
            cameraPylon.Reset();
        }

        public override bool IsGrabDone()
        {
            return cameraPylon.IsGrabDone();
        }

        private Image2D Grab2D(CameraPylon camera, float exposure2d)
        {
            camera.SetExposureTime(exposure2d);

            camera.ImageGrabbed += Pylon_ImageGrabbed;
            camera.GrabOnce();
            camera.WaitGrabDone();

            camera.Stop();
            camera.SetTriggerMode(TriggerMode.Software);

            return (Image2D)camera.GetGrabbedImage();
        }

        public override bool Calibrate(float targetRadius2d, float targetRadius3d, float exposure2d, TransformDataList transformDataList)
        {
            var transformData = new TransformData();
            Image2D image2d = Grab2D(cameraPylon, exposure2d);
            if (Calibrate3D(0, image2d, targetRadius3d, transformData) == false)
            {
                return false;
            }

            transformDataList.Add(transformData);

            return true;
        }

        public override bool Calibrate2D(Image2D image, float value, TransformData transformData)
        {
            throw new NotImplementedException();
        }

        public override bool Calibrate3D(int deviceIndex, Image2D image, float targetRadius3d, TransformData transformData)
        {
            Grab3D();

            Image3D image3d = Calculate(Rectangle.Empty, 1.0f);

            var imageHalfSize = new Size(image.Width / 2, image.Height / 2);

            var rectangleList = new Rectangle[4]
            {
                new Rectangle(new Point(0, 0),  imageHalfSize), new Rectangle(new Point(imageHalfSize.Width, 0),  imageHalfSize),
                new Rectangle(new Point(0, imageHalfSize.Height),  imageHalfSize), new Rectangle(new Point(imageHalfSize.Width, imageHalfSize.Height),  imageHalfSize)
            };

            CircleDetector circleDetector = new OpenCvCircleDetector();

            var param = new CircleDetectorParam();
            param.InnerRadius = targetRadius3d * 0.8f;
            param.OutterRadius = targetRadius3d * 1.2f;
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
                ImageD clipImage = image.ClipImage(Rectangle.Ceiling(rect));
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
                circleListForm.Initialize(foundPointList, image, foundPxPointList);
                if (circleListForm.ShowDialog() == DialogResult.OK && foundPointList.Count == 4)
                {
                    transform3d.FindRT(circleListForm.PointArray);
                    transform3d.GetMapData(transformData);

                    transformData.CameraIndex = cameraPylon.Index;

                    return true;
                }
            }

            return false;
        }

        public override Image3D ScanMeasure(int index, float pixelResolution)
        {
            Grab3D();
            return Calculate(new Rectangle(0, 0, ImageSize.Width, ImageSize.Height), pixelResolution);
        }

        public override bool SetGain(float gain)
        {
            return true;
        }
    }
}
