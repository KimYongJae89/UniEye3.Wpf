using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Struct;
using WPF.UniScanIM.Manager;
using WPF.UniScanIM.Override;

namespace AlgoTest
{
    internal class Program
    {
        private static string imagePath = @"C:\UniScan\생코뱅 현장이미지\Size5000_이형지";

        private static string InputPath => Path.Combine(imagePath, "Input");

        private static string OutputPath => Path.Combine(imagePath, "Output");

        private static StreamWriter logWriter;
        private static StreamWriter streamWriter;
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        private static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            logWriter = new StreamWriter(Path.Combine(imagePath, $"{dt.ToString("yyyyMMdd-HHmmss")}_Log.log"), false);
            streamWriter = new StreamWriter(Path.Combine(imagePath, $"{dt.ToString("yyyyMMdd-HHmmss")}_SizeTime.log"), false);

            // 파일 이름순 정렬
            var directoryInfo = new DirectoryInfo(InputPath);
            if (!directoryInfo.Exists)
            {
                ConsoleWriteLine("There is no INPUT path");
                Console.ReadKey();
                return;
            }

            FileInfo[] fileInfos = directoryInfo.GetFiles("*.bmp");
            var nameList = fileInfos.Select(f =>
            {
                string[] tokens = f.Name.Split('.');
                if (int.TryParse(tokens[0], out int i))
                {
                    return (i, tokens[1]);
                }

                return (-1, "");
            }).ToList();
            nameList.RemoveAll(f => f.Item1 < 0);
            nameList.Sort();

            if (nameList.Count == 0)
            {
                ConsoleWriteLine("There is no .bmp exist");
                Console.ReadKey();
                return;
            }

            string testFileName = $"{nameList[0].Item1}.{nameList[0].Item2}";
            var testImage2D = Image2D.ToImage2D(Path.Combine(InputPath, testFileName));
            int imageHeight = testImage2D.Height;

            MatroxHelper.InitApplication();

            SystemConfig.Instance.IsSaveFrameImage = true;
            SystemConfig.Instance.IsSaveDebugData = true;

            Camera camera = new CameraVirtual();
            camera.Initialize(new CameraInfoVirtual(4096, imageHeight, 500, imagePath));
            DeviceManager.Instance().CameraHandler.AddCamera(camera);
            var moduleInfo = new ModuleInfo()
            {
                //Camera = camera,
                CamPos = ECamPosition.OneCam,
                ModuleNo = 0,
                ResolutionWidth = 30,
                ResolutionHeight = 30,
                TriggerMode = TriggerMode.Software
            };
            moduleInfo.BufferWidth = moduleInfo.Camera.ImageSize.Width;
            moduleInfo.BufferHeight = moduleInfo.Camera.ImageSize.Height;

            string path = Path.Combine(BaseConfig.Instance().ConfigPath, "Settings.json");
            AlgoTaskManagerSetting settings;
            try
            {
                settings = AlgoTaskManagerSetting.Load(path);
                throw new Exception("Alwayse Throw");
            }
            catch
            {
                settings = new AlgoTaskManagerSetting();
                settings.AddTask(
                    new UniScanC.Algorithm.LineCalibrate.LineCalibratorParam(), // 0
                    new UniScanC.Algorithm.PatternSizeCheck.PatternSizeCheckerParam(), // 1
                    new UniScanC.Algorithm.ColorCheck.ColorCheckerParam(), // 2
                    new UniScanC.Algorithm.PlainFilmCheck.PlainFilmCheckerParam(), // 3
                    new UniScanC.Algorithm.Base.SetNodeParam<Defect>("UnionNode", UniScanC.Algorithm.Base.ESetNodeType.Union)); // 4

                if (false)
                {
                    //// input 0 - LineCalibrator
                    //settings.AddLink(new Link(-1, 0, 0, 0)); // ModuleImageDataByte.Out.ImageDataByte -> LineCalibrator.In.ImageDataByte

                    //// input 1 - PatternSizeChecker
                    //settings.AddLink(new Link(0, 0, 1, 0)); // LineCalibrator.Out.ImageData -> PatternSizeChecker.In.ImageData

                    //// input 2 - ColorChecker
                    //settings.AddLink(new ILink[]
                    //{
                    //        new Link(0, 0, 2, 0), // LineCalibrator.Out.ImageData -> ColorChecker.In.ImageData
                    //        new Link(1, 0, 2, 1), // PatternSizeChecker.Out.RoiMask -> ColorChecker.In.RoiMask
                    //        new Link(-1, 1, 2, 2) // Camera.Out.FrameNo -> ColorChecker.In.FrameNo
                    //});

                    //// input 3 - PlainFilmChecker
                    //settings.AddLink(new ILink[]
                    //{
                    //        new Link(0, 0, 3, 0), // LineCalibrator.Out.ImageData -> PlainFilmChecker.In.ImageData
                    //        new Link(1, 0, 3, 1), // PatternSizeChecker.Out.RoiMask -> PlainFilmChecker.In.RoiMask
                    //        new Link(-1, 1, 3, 2) // Camera.Out.FrameNo -> PlainFilmChecker.In.FrameNo
                    //});
                }
                else
                {
                    // Algorithm 0 - LineCalibrator
                    settings.AddLink(new LinkS("ModuleImageDataByte", "ImageDataByte", "LineCalibrator", "ImageDataByte"));

                    // Algorithm 1 - PatternSizeChecker
                    settings.AddLink(new LinkS("LineCalibrator", "ImageData", "PatternSizeChecker", "ImageData"));

                    // Algorithm 2 - ColorChecker
                    settings.AddLink(new LinkS("LineCalibrator", "ImageData", "ColorChecker", "ImageData"));
                    settings.AddLink(new LinkS("PatternSizeChecker", "RoiMask", "ColorChecker", "RoiMask"));
                    settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "ColorChecker", "FrameNo"));

                    // Algorithm 3 - PlainFilmChecker
                    settings.AddLink(new LinkS("LineCalibrator", "ImageData", "PlainFilmChecker", "ImageData"));
                    settings.AddLink(new LinkS("PatternSizeChecker", "RoiMask", "PlainFilmChecker", "RoiMask"));
                    settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "PlainFilmChecker", "FrameNo"));

                    // Node 4 - Union
                    settings.AddLink(new LinkS("ColorChecker", "DefectList", "UnionNode", "List"));
                    settings.AddLink(new LinkS("PlainFilmChecker", "DefectList", "UnionNode", "List"));

                    // Result
                    settings.AddLink(new LinkS("ModuleImageDataByte", "FrameNo", "InspectResult", "FrameIndex"));
                    settings.AddLink(new LinkEx("LineCalibrator", "ImageData", "InspectResult", "FrameImageData", typeof(ImageData), typeof(BitmapSource)));
                    settings.AddLink(new LinkEx("LineCalibrator", "ImageData", "InspectResult", "InspectRegion", typeof(ImageData), typeof(SizeF)));
                    settings.AddLink(new LinkEx("PatternSizeChecker", "RoiMask", "InspectResult", "EdgePos", typeof(RoiMask), typeof(float)));
                    settings.AddLink(new LinkS("PatternSizeChecker", "PatternSize", "InspectResult", "PatternSize"));
                    settings.AddLink(new LinkS("UnionNode", "List", "InspectResult", "DefectList"));

                    //settings.AddLink(new LinkListAdd<Defect>("InspectResult", "DefectList", new (string, string)[] { ("ColorChecker", "DefectList"), ("PlainFilmChecker", "DefectList") }));

                    //settings.AddLink(new LinkS("ColorChecker", "DefectList", "InspectResult", "DefectList"));
                    //settings.AddLink(new LinkS("PlainFilmChecker", "DefectList", "InspectResult", "DefectList"));
                }
                settings.Save(path);
            }

            var visionModel = new UniScanC.Models.VisionModel()
            {
                TargetIntensity = -1,

                ThresholdDark = 50,
                ThresholdLight = 75,
                ThresholdSize = 100,

                FrameMarginT = 0,
                FrameMarginB = 0,
            };

            var algoTaskManager = new AlgoTaskManager(moduleInfo, settings, visionModel);
            //settings.Save("Settings.json");
            algoTaskManager.OnInspectCompleted += AlgoTaskManager_OnInspectCompleted;
            algoTaskManager.Start();

            streamWriter.WriteLine("No, Defects[EA], Width[px], Length[px], Time[ms]");
            nameList.ForEach(f =>
            {
                SystemConfig.Instance.IsSaveDebugData = false;

                if (f.Item1 < 75)
                {
                    return;
                }

                string fileName = $"{f.Item1}.{f.Item2}";
                ConsoleWriteLine($"Load {f.Item1}");
                var image2D = Image2D.ToImage2D(Path.Combine(InputPath, fileName));
                ConsoleWriteLine($"Image Size {fileName}: W {image2D.Width}, H {image2D.Height}");
                var inspectResult = new InspectResult() { FrameIndex = f.Item1 };
                var inspectBufferSet = new InspectBufferSet(new ImageDataByte(image2D, image2D.Size), f.Item1);

                using (AlgoImage algoImage = ImageBuilder.MilImageBuilder.Build(image2D, ImageType.Grey))
                {
                    ConsoleWriteLine($"Enqueue {f.Item1}");
                    algoTaskManager.Enqueue((inspectBufferSet, inspectResult));

                    ConsoleWriteLine($"Wait Inspect Done {f.Item1}");
                    algoTaskManager.WaitAllCompleted();
                    ConsoleWriteLine($"Inspect Done {f.Item1}");

                    autoResetEvent.WaitOne();
                }
            });
            algoTaskManager.Stop();

            logWriter.Close();
            logWriter.Dispose();

            MatroxHelper.FreeApplication();
        }

        private static void ConsoleWriteLine(string v)
        {
            Console.WriteLine(v);

            string dt = $"{DateTime.Now.ToString("HH:mm:ss")} - {v}";
            logWriter.WriteLine(dt);
            logWriter.Flush();
        }

        private static void AlgoTaskManager_OnInspectCompleted(InspectResult inspectResult)
        {
            ConsoleWriteLine($"Elapsed Time {inspectResult.FrameIndex}: {inspectResult.InspectTimeSpan.TotalMilliseconds:F0}[ms] - Defects: {inspectResult.DefectList.Count}[EA], PatternLength: {inspectResult.PatternSize.Height}");
            streamWriter.WriteLine($"{inspectResult.FrameIndex}, {inspectResult.DefectList.Count}, {inspectResult.PatternSize.Width}, {inspectResult.PatternSize.Height}, {inspectResult.InspectTimeSpan.TotalMilliseconds:F0}");
            streamWriter.Flush();

            System.Threading.Tasks.Task.Run(() =>
            {
                ConsoleWriteLine($"Start Save {inspectResult.FrameIndex}");
                BitmapSource bitmapSource = GetDefectImage(inspectResult);
                if (bitmapSource == null)
                {
                    return;
                }

                string fileName = $"{inspectResult.FrameIndex}.bmp";
                var outFileInfo = new FileInfo(Path.Combine(OutputPath, fileName));
                if (!Directory.Exists(outFileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(outFileInfo.DirectoryName);
                }

                UniScanC.Inspect.BmpImaging.SaveBitmapSource(bitmapSource, outFileInfo.FullName);
                ConsoleWriteLine($"Save Done {inspectResult.FrameIndex}");
                autoResetEvent.Set();
            });
        }

        private static BitmapSource GetDefectImage(InspectResult inspectResult)
        {
            BitmapSource frameImage = inspectResult.FrameImageData;
            if (frameImage == null)
            {
                return null;
            }

            // 이미지에 그리기
            var visual = new DrawingVisual();
            DrawingContext context = visual.RenderOpen();
            context.DrawImage(frameImage, new System.Windows.Rect(0, 0, frameImage.Width, frameImage.Height));

            // 텍스트
            var frameText = new FormattedText($"F{inspectResult.FrameIndex} / D{inspectResult.DefectList.Count}", System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Verdana"), 100, System.Windows.Media.Brushes.Black);
            context.DrawText(frameText, new System.Windows.Point(0, 0));

            // 불량 사각형
            inspectResult.DefectList.ForEach(g =>
            {
                var rect = RectangleF.Inflate(g.BoundingRect, 5, 5);
                context.DrawRectangle(null, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 2), new System.Windows.Rect(rect.X, rect.Y, rect.Width, rect.Height));
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
            return bmpImage;
        }
    }
}
