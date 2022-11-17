using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.DatabaseManager;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UniScanC.Data
{
    public class InspectDataExporter
    {
        //DB Info
        public string DbIpAddress { get; set; } = "127.0.0.1";
        public string DbName { get; set; } = "UniScan";
        public string DbUserName { get; set; } = "postgres";
        public string DbPassword { get; set; } = "masterkey";

        //Image Save Option
        public string ResultImagePath { get; set; } = "";
        public bool IsSaveFrameImage { get; set; } = false;
        public bool IsSaveDefectImage { get; set; } = true;

        public string NetworkDriveIpAddress { get; set; } = "127.0.0.1";
        public string NetworkDriveUserName { get; set; }
        public string NetworkDrivePassword { get; set; }

        //Table Info
        private List<string> LotColumnNames { get; set; }
        private List<string> FrameColumnNames { get; set; }
        private List<string> DefectColumnNames { get; set; }


        public InspectDataExporter()
        {
            InitializeColumnNames();
        }

        private void InitializeColumnNames()
        {
            LotColumnNames = new List<string>() { "start_date", "model_name", "lot_name" };
            FrameColumnNames = new List<string>() { "lot_name", "frame_index", "module_index", "judgement", "pos_edge",
                                                    "frame_width", "frame_height", "resolution", "pattern_width", "pattern_length", "image_path",
                                                    "start_time", "end_time"};
            DefectColumnNames = new List<string>() { "lot_name", "frame_index", "module_index", "defect_index", "defect_type", "skip", "pos_x", "pos_y",
                                                     "rect_width", "rect_height", "area", "min_gv", "max_gv", "avg_gv", "image_path" };
        }

        private void InitializeNetworkDrive()
        {
            string remotePath = $@"\\{Path.Combine(NetworkDriveIpAddress, "Result")}";
            if (NetworkDrive.connectToRemote(remotePath, NetworkDriveUserName, NetworkDrivePassword) != null)
            {
                MessageBox.Show($"네트워크 접근을 실패 했습니다.\n네트워크 경로 : {remotePath}\n유저 이름 : {NetworkDriveUserName}\n비밀번호 : {NetworkDrivePassword}");
            }
        }

        public void SetDataBaseInfo(string dbIpAddress, string dbName, string dbUserName, string dbPassword)
        {
            DbIpAddress = dbIpAddress;
            DbUserName = dbUserName;
            DbPassword = dbPassword;
            DbName = dbName;
        }

        public void SetNetworkDriveInfo(string networkDriveIpAddress, string networkDriveUserName, string networkDrivePassword)
        {
            NetworkDriveIpAddress = networkDriveIpAddress;
            NetworkDriveUserName = networkDriveUserName;
            NetworkDrivePassword = networkDrivePassword;

            if (NetworkDriveIpAddress != "127.0.0.1")
            {
                InitializeNetworkDrive();
            }
        }

        public void SetResultImagePath(string resultImagePath)
        {
            ResultImagePath = resultImagePath;
        }

        public void SetImageSetting(bool isSaveFrameImage, bool isSaveDefectImage)
        {
            IsSaveFrameImage = isSaveFrameImage;
            IsSaveDefectImage = isSaveDefectImage;
        }

        public void ExportLotData(string lotName)
        {
            var modelDesc = ModelManager.Instance().CurrentModel.ModelDescription as ModelDescription;

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            if (!dbManager.ConnectDatabase())
            {
                return;
            }

            dbManager.BeginTransaction();

            var lotDatas = new List<object>
            {
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                modelDesc.Name,
                lotName
            };

            dbManager.InsertData("Lot", LotColumnNames, lotDatas);
            dbManager.EndTransaction();
            dbManager.DisconnectDatabase();
        }

        // 모든 좌표 및 너비 길이 값은 픽셀로 들어와서 저장할 때 해상도를 곱해준다.
        public void ExportDefectData(ProductResult productResult)
        {
            if (!(productResult is InspectResult inspectResult))
            {
                return;
            }

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);

            if (!dbManager.ConnectDatabase())
            {
                LogHelper.Error("DataExporter::ExprotDefectData - ConnectDatabase false");
                return;
            }

            dbManager.BeginTransaction();

            var resolutionMM = new SizeF
            {
                Width = inspectResult.Resolution.Width / 1000,
                Height = inspectResult.Resolution.Height / 1000
            };

            var frameDatas = new List<object>
            {
                inspectResult.LotNo,
                inspectResult.FrameIndex,
                inspectResult.ModuleNo,
                inspectResult.Judgment.ToString(),
                inspectResult.EdgePos * resolutionMM.Width,
                inspectResult.InspectRegion.Width * resolutionMM.Width,
                inspectResult.InspectRegion.Height * resolutionMM.Height,
                inspectResult.Resolution.Width,
                inspectResult.PatternSize.Width * resolutionMM.Width,
                inspectResult.PatternSize.Height * resolutionMM.Height
            };

            if (IsSaveFrameImage && inspectResult.FrameImageData != null)
            {
                string frameImagePath = ExportFrameImage(ResultImagePath, inspectResult);
                frameDatas.Add(frameImagePath);
            }
            else
            {
                frameDatas.Add("");
            }

            frameDatas.Add(inspectResult.StartTime);
            frameDatas.Add(inspectResult.EndTime);

            dbManager.InsertData("Frame", FrameColumnNames, frameDatas);

            foreach (Defect defect in inspectResult.DefectList)
            {
                if (defect.TopDefectCategory() == null)
                {
                    continue;
                }

                var defectDatas = new List<object>
                {
                    inspectResult.LotNo,
                    defect.FrameIndex,
                    defect.ModuleNo,
                    defect.DefectNo,
                    defect.TopDefectCategory().Name,
                    defect.TopDefectCategory().IsSkip ? 1 : 0,
                    defect.DefectPos.X * resolutionMM.Width,
                    defect.DefectPos.Y * resolutionMM.Height,
                    defect.BoundingRect.Width * resolutionMM.Width,
                    defect.BoundingRect.Height * resolutionMM.Height,
                    defect.Area * resolutionMM.Width * resolutionMM.Height,
                    defect.MinGv,
                    defect.MaxGv,
                    defect.AvgGv
                };

                if (IsSaveDefectImage)
                {
                    string defectImagePath = ExportDefectImage(ResultImagePath, defect);
                    defectDatas.Add(defectImagePath);
                }
                else
                {
                    defectDatas.Add("");
                }

                dbManager.InsertData("Defect", DefectColumnNames, defectDatas);
            }

            dbManager.EndTransaction();
            dbManager.DisconnectDatabase();
        }

        private string ExportFrameImage(string directoryPath, InspectResult inspectResult)
        {
            string fileName = $"{inspectResult.FrameIndex}_{inspectResult.ModuleNo}.jpeg";
            string filePath = Path.Combine(directoryPath, fileName);
            //D 드라이브를 공유 걸었을 때를 가정하는 코드
            string remotePath = Path.Combine(DbIpAddress, "Result", filePath);

            if (Directory.Exists(Path.GetDirectoryName(remotePath)))
            {
                try
                {
                    BitmapSource bitmapSource = inspectResult.FrameImageData;
                    double scale = 1024.0 / Math.Max(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
                    if (Math.Min(bitmapSource.PixelWidth, bitmapSource.PixelHeight) > 10)
                    {
                        scale = 0.1;
                    }

                    if (scale < 1)
                    {
                        var scaleTransform = new ScaleTransform(scale, scale);
                        bitmapSource = new TransformedBitmap(inspectResult.FrameImageData, scaleTransform);
                    }

                    var jpegBitmapEncoder = new JpegBitmapEncoder();
                    jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    using (var fileStream = new FileStream(remotePath, FileMode.Create, FileAccess.Write))
                    {
                        jpegBitmapEncoder.Save(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"DataExporter::ExportFrameImage - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
            }
            return filePath;
        }

        private string ExportDefectImage(string directoryPath, Defect defect)
        {
            if (defect.DefectImage == null)
            {
                return "";
            }

            BitmapSource bitmapSource = defect.DefectImage;
            double length = Math.Sqrt(Math.Pow(bitmapSource.PixelWidth, 2) + Math.Pow(bitmapSource.PixelHeight, 2));
            if (length > 256)
            {
                double scale = 256 * 1.0 / length;
                var transform = new ScaleTransform(scale, scale);
                var transformedBitmap = new TransformedBitmap(defect.DefectImage, transform);
                transformedBitmap.Freeze();
                bitmapSource = transformedBitmap;
            }

            //D 드라이브를 공유 걸었을 때를 가정하는 코드
            string fileName = $"{defect.FrameIndex}_{defect.ModuleNo}_{defect.DefectNo}.bmp";
            string filePath = Path.Combine(directoryPath, fileName);
            string remotePath = Path.Combine(DbIpAddress, "Result", filePath);
            var bmpBitmapEncoder = new BmpBitmapEncoder();
            bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            if (Directory.Exists(Path.GetDirectoryName(remotePath)))
            {
                try
                {
                    using (var fileStream = new FileStream(remotePath, FileMode.Create, FileAccess.Write))
                    {
                        bmpBitmapEncoder.Save(fileStream);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"DataExporter::ExportDefectImage - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
            }
            return filePath;
        }
    }
}
