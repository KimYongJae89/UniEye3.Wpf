using ControlzEx.Standard;
using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniEye.Base.Config;
using UniScanC.Data;
using UniScanC.Helpers;
using UniScanC.Models;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Manager
{
    public class TeachingManager
    {
        private ModelManager ModelManager => ModelManager.Instance();

        private IMInspectRunner InspectRunner => SystemManager.Instance().InspectRunner as IMInspectRunner;

        public void Grab(bool isStart)
        {
            SystemConfig config = SystemConfig.Instance;
            var moduleInfos = config.ModuleList.ToList();
            foreach (ModuleInfo moduleInfo in moduleInfos)
            {
                if (isStart)
                {
                    moduleInfo.Camera.SetTriggerMode(moduleInfo.TriggerMode);
                    if (moduleInfo.Camera is CameraVirtual || moduleInfo.TriggerMode == TriggerMode.Software)
                    {
                        float grabHz = UnitConvertor.GetGrabHz(config.LineSpeed, moduleInfo.ResolutionHeight);
                        moduleInfo.Camera.SetAcquisitionLineRate(grabHz);
                    }
                    moduleInfo.Camera.ImageGrabbed = TeachImageGrabbed;
                    moduleInfo.Camera.GrabMulti();
                }
                else
                {
                    moduleInfo.Camera.Stop();
                    moduleInfo.Camera.ImageGrabbed = null;
                }
            }

        }

        private void TeachImageGrabbed(Camera camera)
        {
            ModuleInfo moduleInfo = SystemConfig.Instance.ModuleList.First(f => f.Camera == camera);
            string fileName = $"ModuleGrab_{moduleInfo.ModuleNo}";
            ImageFormat imageFormat = ImageFormat.Png;

            if (File.Exists(fileName))
            {
                return;
            }

            var image2D = (Image2D)camera.GetGrabbedImage();

            if (image2D.DataPtr == IntPtr.Zero)
            {
                image2D.ConvertFromData();
            }

            BitmapSource bitmap = BitmapImage.Create(
                image2D.Width, image2D.Height, 96, 96,
                PixelFormats.Gray8, null, image2D.DataPtr,
                image2D.Pitch * image2D.Height, image2D.Pitch);

            if (SaveImage(bitmap, fileName, imageFormat))
            {
                // 티칭을 위한 이미지를 남기기 위함
                if (InspectRunner.LastGrabbedImage != null)
                {
                    InspectRunner.LastGrabbedImage.Dispose();
                }

                InspectRunner.LastGrabbedImage = image2D;
            }
        }

        private bool SaveImage(BitmapSource bitmap, string fileName, ImageFormat imageFormat)
        {
            string lockFile = Path.Combine(@"\\", SystemConfig.Instance.CMMQTTIpAddress, "Result", fileName);
            string imageFile = $"{lockFile}.{imageFormat}";

            // 최대 크기: 가로/세로 4K = 4096픽셀
            var imageSize = new System.Drawing.Size(bitmap.PixelWidth, bitmap.PixelHeight);
            double ratio = imageSize.Height * 1.0 / imageSize.Width;
            if (ratio > 1)
            // 세로로 더 길다
            {
                if (imageSize.Height > 4096)
                {
                    imageSize.Height = 4096;
                    imageSize.Width = (int)(imageSize.Height / ratio);
                }
            }
            else
            // 가로로 더 길다
            {
                if (imageSize.Width > 4096)
                {
                    imageSize.Width = 4096;
                    imageSize.Height = (int)(ratio * imageSize.Width);
                }
            }

            //if (imageD.Size != imageSize)
            //{
            //    imageD = imageD.Resize(imageSize.Width, imageSize.Height);
            //}

            if (File.Exists(lockFile))
            {
                return false;
            }

            UniScanC.Inspect.BmpImaging.SaveBitmapSource(bitmap, imageFile);

            File.WriteAllText(lockFile, "");

            return true;
        }

        public void Inspect()
        {
            // 모델 선택 여부 확인
            if (!(ModelManager.CurrentModel is UniScanC.Models.Model model))
            {
                throw new Exception("MODEL_NOT_OPEN_MESSAGE");
            }

            // 바뀐 파라미터 로드 (모델 로드)
            string curmodelName = ModelManager.CurrentModel.Name;
            ModelManager.Refresh();
            ModelManager.OpenTeachModel(curmodelName, null);
            model = ModelManager.CurrentModel as UniScanC.Models.Model;

            InspectRunner.VisionModels = model.VisionModels;

            // 검사 진행
            var moduleInfoList = SystemConfig.Instance.ModuleList.ToList();
            moduleInfoList.ForEach(moduleInfo =>
            {
                InspectResult result = InspectRunner.TeachInspect(moduleInfo);
                BitmapSource frameImage = result.FrameImageData;
                VisionModel visionModel = InspectRunner.VisionModels[moduleInfo.ModuleNo];
                float frameMarginPixelT = visionModel.FrameMarginT * result.Resolution.Height;
                float frameMarginPixelB = visionModel.FrameMarginB * result.Resolution.Height;
                float frameMarginPixelL = visionModel.FrameMarginL * result.Resolution.Width;
                float frameMarginPixelR = visionModel.FrameMarginR * result.Resolution.Width;
                float patternMarginPixelX = (float)visionModel.PatternMarginX * result.Resolution.Width;
                float patternMarginPixelY = (float)visionModel.PatternMarginY * result.Resolution.Height;

                var visual = new DrawingVisual();
                DrawingContext context = visual.RenderOpen();
                context.DrawImage(frameImage, new System.Windows.Rect(0, 0, frameImage.Width, frameImage.Height));

                // Frame 위치 그리기
                Pen framePen = new Pen(System.Windows.Media.Brushes.Red, 2);
                float topFramePos = frameMarginPixelT;
                float bottomFramePos = result.InspectRegion.Height - frameMarginPixelB;
                float leftFramePos = frameMarginPixelL;
                float rightFramePos = result.InspectRegion.Width - frameMarginPixelR;
                context.DrawRectangle(null, framePen, new Rect(new Point(leftFramePos, topFramePos), new Point(rightFramePos, bottomFramePos)));

                // Edge 위치 그리기
                Pen edgePen = new Pen(System.Windows.Media.Brushes.Green, 2);
                float topEdgePos = frameMarginPixelT;
                float bottomEdgePos = result.InspectRegion.Height - frameMarginPixelB;
                float leftEdgePos = result.EdgePos - patternMarginPixelX;
                float rightEdgePos = result.EdgePos + result.PatternSize.Width + patternMarginPixelX;
                context.DrawRectangle(null, edgePen, new Rect(new Point(leftEdgePos, topEdgePos), new Point(rightEdgePos, bottomEdgePos)));

                // Pattern 위치 그리기
                Pen patternPen = new Pen(System.Windows.Media.Brushes.Blue, 2);
                float topPatternPos = frameMarginPixelT + patternMarginPixelY;
                float bottomPatternPos = result.InspectRegion.Height - (frameMarginPixelB + patternMarginPixelY);
                float leftPatternPos = result.EdgePos;
                float rightPatternPos = result.EdgePos + result.PatternSize.Width;
                context.DrawRectangle(null, patternPen, new Rect(new Point(leftPatternPos, topPatternPos), new Point(rightPatternPos, bottomPatternPos)));

                // 이미지에 불량 이미지 위치 사각형 그리기
                result.DefectList.ForEach(g =>
                {
                    System.Drawing.RectangleF rect = g.BoundingRect;
                    Brush brush = new SolidColorBrush(Colors.Red);
                    brush.Opacity = 0;
                    Pen defectPen = new Pen(Brushes.Red, 1);
                    Rect defectRect = new Rect(Math.Max(0, rect.X - 5), Math.Max(0, rect.Y - 5), rect.Width + 10, rect.Height + 10);
                    context.DrawRectangle(brush, defectPen, defectRect);
                });
                context.Close();

                var renderTargetBitmap = new RenderTargetBitmap(frameImage.PixelWidth, frameImage.PixelHeight, 96, 96, PixelFormats.Default);
                renderTargetBitmap.Render(visual);

                // 이미지 저장
                var bmpImage = new BitmapImage();
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                using (Stream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    bmpImage.BeginInit();
                    bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                    bmpImage.StreamSource = stream;
                    bmpImage.EndInit();
                }

                if (!Directory.Exists(@"C:\temp"))
                {
                    Directory.CreateDirectory(@"C:\temp");
                }

                UniScanC.Inspect.BmpImaging.SaveBitmapSource(bmpImage, @"C:\temp\bmpImage.bmp");

                //encoder.Save(new FileStream(@"C:\temp\save.bmp", FileMode.Create));
                //bitmap.Save(@"C:\temp\image2D.bmp", ImageFormat.Bmp);
                SaveImage(bmpImage, $"ModuleResult_{moduleInfo.ModuleNo}", ImageFormat.Png);
            });
        }
    }
}
