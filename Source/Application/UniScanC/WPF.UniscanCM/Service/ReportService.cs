using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Services;
using UniEye.Translation.Helpers;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.Models;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Pages.ViewModels;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Service
{
    public class ReportService
    {
        private InspectDataImporter dataImporter;

        public ReportService()
        {
            SystemConfig config = SystemConfig.Instance;
            dataImporter = new InspectDataImporter();
            dataImporter.SetDataBaseInfo(config.DatabaseIpAddress, config.DatabaseName, config.DatabaseUserName, config.DatabasePassword);
        }

        public ModelDescription GetModelDescription(ReportModel reportModel)
        {
            string modelPath = Path.Combine(SystemConfig.Instance.ResultPath, reportModel.LotNo, "Model");
            return ModelManager.Instance().LoadCopiedModelDescription(modelPath);
        }

        public UniScanC.Models.Model GetModel(ReportModel reportModel)
        {
            string modelPath = Path.Combine(SystemConfig.Instance.ResultPath, reportModel.LotNo, "Model");
            return ModelManager.Instance().LoadCopiedModel(modelPath);
        }

        public List<ReportModel> SearchResult(DateTime stratDate, DateTime endDate, ProgressSource progressSource = null)
        {
            var reportModels = new List<ReportModel>();

            List<Dictionary<string, object>> lotDatas = null;
            lotDatas = dataImporter.ImportLotData(stratDate, endDate);

            foreach (Dictionary<string, object> lotData in lotDatas.OrderByDescending(x => x["start_date"]))
            {
                var reportModel = new ReportModel();

                // 진행 길이가 0보다 큰 경우만 열람
                reportModel.LotNo = Convert.ToString(lotData["lot_name"]);
                reportModel.Length = GetLength(reportModel.LotNo);
                if (reportModel.Length > 0)
                {
                    reportModel.DateTime = (DateTime)lotData["start_date"];
                    reportModel.ModelName = Convert.ToString(lotData["model_name"]);
                    reportModel.Count = dataImporter.ImportDefectCount(reportModel.LotNo);
                    reportModel.DefectTypes = SearchDefectTypesCount(reportModel);
                    reportModels.Add(reportModel);
                }

                if (progressSource.CancellationTokenSource.IsCancellationRequested)
                {
                    return null;
                }

                if (progressSource != null)
                {
                    progressSource.StepIt();
                }
            }

            return reportModels;
        }

        public Dictionary<string, int> SearchDefectTypesCount(ReportModel reportModel)
        {
            var defectTypesCount = new Dictionary<string, int>();
            Model model = GetModel(reportModel);
            if (model == null)
            {
                return defectTypesCount;
            }

            var defectCategories = new List<DefectCategory>();
            foreach (VisionModel visionModel in model.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCategories.Find(x => x.Name == category.Name) == null)
                    {
                        defectCategories.Add(new DefectCategory(category));
                    }
                }
            }

            IEnumerable<string> defectTypes = defectCategories.Select(x => x.Name);
            foreach (string defectType in defectTypes)
            {
                int defectCount = dataImporter.ImportDefectCount(reportModel.LotNo, defectType);
                defectTypesCount.Add(defectType, defectCount);
            }
            return defectTypesCount;
        }

        public List<InspectResult> SearchDetailResult(ReportModel reportModel, bool isLoadImage, ProgressSource progressSource = null)
        {
            List<Dictionary<string, object>> frameDatas = dataImporter.ImportFrameData(reportModel.LotNo);
            List<InspectResult> defectDatas = InspectResult.Parse(dataImporter, frameDatas, 1, false, isLoadImage, progressSource);
            return defectDatas;
        }

        public List<Defect> RepositionDefects(List<InspectResult> results, ProgressSource progressSource)
        {
            var defects = new List<Defect>();
            ILookup<int, InspectResult> moduleSortResults = results.ToLookup(x => x.FrameIndex);
            foreach (IGrouping<int, InspectResult> pair in moduleSortResults)
            {
                InspectResult firstResult = pair.FirstOrDefault(x => x.ModuleNo == 0);
                if (firstResult != null)
                {
                    float leftEdge = firstResult.EdgePos;
                    foreach (InspectResult result in pair)
                    {
                        foreach (Defect defect in result.DefectList)
                        {
                            defect.BoundingRect = new RectangleF(
                                defect.BoundingRect.Left - leftEdge,
                                defect.BoundingRect.Top,
                                defect.BoundingRect.Width,
                                defect.BoundingRect.Height);

                            defect.DefectPos = new PointF(
                                defect.DefectPos.X - leftEdge,
                                defect.DefectPos.Y);

                            defects.Add(defect);

                            if (progressSource.CancellationTokenSource.IsCancellationRequested)
                            {
                                return null;
                            }
                        }
                    }
                }
            }

            return defects;
        }

        public int GetLotCount(DateTime startDate, DateTime endDate)
        {
            return dataImporter.ImportLotCount(startDate, endDate);
        }

        public int GetFrameCount(string lotName)
        {
            return dataImporter.ImportFrameCount(lotName);
        }

        public int GetDefectCount(string lotName, string defectType = "")
        {
            return dataImporter.ImportDefectCount(lotName, defectType);
        }

        private double GetLength(string lotName)
        {
            List<Dictionary<string, object>> lastFrameData = dataImporter.ImportLastFrame(lotName, 5);
            if (lastFrameData.Count == 0)
            {
                return 0;
            }

            int maxFrameIndex = lastFrameData.Max(x => Convert.ToInt32(x["frame_index"]));
            float maxFrame_height = lastFrameData.Max(x => Convert.ToSingle(x["frame_height"]));

            double lengthM = (maxFrameIndex + 1) * maxFrame_height / 1000;
            return lengthM;
        }


        // Excell Data Export
        public async void ExportAction(UniScanC.Models.Model selectedModel, IEnumerable<ProductResult> searchResults, IEnumerable<Defect> searchDefects, ReportModel searchReportModel)
        {
            if (searchResults == null || searchDefects == null)
            {
                await MessageWindowHelper.ShowMessageBox(
                    TranslationHelper.Instance.Translate("ERROR"),
                    TranslationHelper.Instance.Translate("No_results_found"));
                return;
            }

            var exportOptionView = new ExportOptionWindowView();
            exportOptionView.ReportModel = searchReportModel;

            var DefectCategoryPairs = new List<WTuple<string, bool>>();
            foreach (string defectType in searchReportModel.DefectTypes.Keys)
            {
                DefectCategoryPairs.Add(new WTuple<string, bool>(defectType, true));
            }

            DefectCategoryPairs.Add(new WTuple<string, bool>("OTHERS", true));
            DefectCategoryPairs.Add(new WTuple<string, bool>("COLOR", true));

            SystemConfig.Instance.ExportOptionModel.DefectCategoryPairs = DefectCategoryPairs;
            exportOptionView.ExportOptionModel = SystemConfig.Instance.ExportOptionModel;

            ExportOptionModel exportOption = await MessageWindowHelper.ShowChildWindow<ExportOptionModel>(exportOptionView);
            if (exportOption != null)
            {
                SystemConfig.Instance.ExportOptionModel.CopyFrom(exportOption);
                SystemConfig.Instance.Save();

                var dlg = new SaveFileDialog();
                dlg.FileName = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd"), searchReportModel.LotNo);
                dlg.Filter = "Excel(*.xlsx)|*.xlsx";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ExportExcel(dlg.FileName, selectedModel, searchResults, searchDefects, searchReportModel, exportOption);
                }
            }
        }

        private async void ExportExcel(string filePath, UniScanC.Models.Model selectedModel, IEnumerable<ProductResult> searchResults, IEnumerable<Defect> searchDefects, ReportModel searchReportModel, ExportOptionModel exportOption)
        {
            int maxScanNo = searchResults.Cast<InspectResult>().Max(x => x.FrameIndex);

            IEnumerable<Defect> filteredList = searchDefects;
            if (exportOption.SortDirection == ESortDirections.Ascending)
            {
                filteredList = filteredList.OrderBy(x => x.FrameIndex).OrderBy(x => x.DefectNo);
            }
            else
            {
                filteredList = filteredList.OrderByDescending(x => x.FrameIndex).OrderByDescending(x => x.DefectNo);
            }

            if (exportOption.DefectCategoryPairs.Exists(x => x.Item2 == true))
            {
                filteredList = FilterDefectCategory(filteredList, exportOption.DefectCategoryPairs);
            }

            int range = filteredList.Count() * 2;
            foreach (WTuple<EChartType, bool> pair in exportOption.ChartPairs)
            {
                if (pair.Item2)
                {
                    range += filteredList.Count();
                }
            }

            var progressSource = new Unieye.WPF.Base.Controls.ProgressSource();
            progressSource.Step = 1;
            progressSource.Range = range;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();

            await MessageWindowHelper.ShowProgress(
                TranslationHelper.Instance.Translate("EXCEL_EXPORT"),
                TranslationHelper.Instance.Translate("EXPORT") + "...",
                new Action(() =>
                {
                    string templateFile = "";
                    switch (exportOption.CustomerSetting)
                    {
                        case ECustomer.General:
                        default:
                            templateFile = string.Format(@"{0}\..\Result\RawDataTemplate.xlsx", Directory.GetCurrentDirectory()); break;
                    }

                    var excel = new ExcelTemplateInfragistics(filePath, templateFile);

                    ExcellExportReport(excel, exportOption, searchReportModel, searchResults, filteredList, progressSource);
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        excel.Dispose();
                        return;
                    }

                    ExcellExportParameter(excel, exportOption, selectedModel);

                    ExcellExportRawData(excel, exportOption, filteredList, progressSource);
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        excel.Dispose();
                        return;
                    }

                    if (SystemConfig.Instance.IsInspectPattern)
                    {
                        ExcellExportPattern(excel, exportOption, searchResults);
                    }

                    excel.Save();
                    excel.Dispose();

                    string directoryPath = Path.GetDirectoryName(filePath);
                    string ChartTemplateFile = string.Format(@"{0}\..\Result\ChartDataTemplate.xlsx", Directory.GetCurrentDirectory());
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string chartExcelPath = string.Format(@"{0}\{1}{2}", directoryPath, fileName, "_Chart.xlsx");

                    if (exportOption.ChartPairs.Any(x => x.Item2 == true))
                    {
                        var chartExcel = new ExcelTemplate(chartExcelPath, ChartTemplateFile);

                        foreach (WTuple<EChartType, bool> pair in exportOption.ChartPairs)
                        {
                            if (pair.Item2)
                            {
                                switch (pair.Item1)
                                {
                                    case EChartType.SizeChart: ExcellExportSizeChart(chartExcel, selectedModel, exportOption, filteredList, progressSource); break;
                                    case EChartType.LengthChart: ExcellExportLengthChart(chartExcel, selectedModel, exportOption, filteredList, progressSource); break;
                                    // TODO:[송현석] 일단은 VisionModel에서 가장 큰 사이즈 패턴으로 가져온다.
                                    case EChartType.WidthChart: ExcellExportWidthChart(chartExcel, selectedModel, exportOption, selectedModel.VisionModels.Max(x => x.PatternWidth), filteredList, progressSource); break;
                                }
                            }
                            if (progressSource.CancellationTokenSource.IsCancellationRequested)
                            {
                                chartExcel.Dispose();
                                return;
                            }
                        }

                        chartExcel.Save();
                        chartExcel.Dispose();

                        var finalExcel = new ExcelTemplate();
                        finalExcel.Combine(filePath, chartExcelPath);
                        finalExcel.Save();
                        finalExcel.Dispose();
                        File.Delete(chartExcelPath);
                    }

                    //ExportDefectImage(directoryPath, filteredList, progressSource);
                }), true, progressSource);
        }

        private void ExcellExportReport(ExcelTemplateInfragistics excel, ExportOptionModel exportOption, ReportModel reportModel, IEnumerable<ProductResult> searchResults, IEnumerable<Defect> filteredList, ProgressSource progressSource)
        {
            excel.SelectSheet("Report");
            int rowIndex = 5;
            int colIndex = 0;
            int columnCount = 0;

            var headerList = new List<Tuple<int, int, object>>();
            var dataList = new List<Tuple<int, int, object>>();
            var DefectTypePairs = new Dictionary<string, List<Defect>>();

            var firstResult = searchResults.First() as InspectResult;

            //excel.WriteData(3, 0, TranslationHelper.Instance.Translate("SUMMARY", exportOption.Language));

            excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("LOT_NO", exportOption.Language));
            excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("START_TIME", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("END_TIME", exportOption.Language));

            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("OPERATOR_NAME", exportOption.Language));
            excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("MODEL_NAME", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("THICKNESS", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("SPEED", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("MACHINE_NAME", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("PET_FILM_MODEL", exportOption.Language));

            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("SPECIFICITY_1", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("SPECIFICITY_2", exportOption.Language));
            excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("LENGTH", exportOption.Language));
            excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("DEFECT_COUNT", exportOption.Language));
            //excel.WriteData(rowIndex++, 1, TranslationHelper.Instance.Translate("Yield", exportOption.Language));

            rowIndex = 5;
            excel.WriteData(rowIndex++, 2, firstResult.LotNo);
            excel.WriteData(rowIndex++, 2, firstResult.InspectStartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //excel.WriteData(rowIndex++, 2, firstResult.InspectEndTime.ToString("yyyy-MM-dd HH:mm:ss"));

            //excel.WriteData(rowIndex++, 2, "");
            excel.WriteData(rowIndex++, 2, reportModel.ModelName);
            excel.WriteData(rowIndex++, 2, string.Format("{0:0.00}m", reportModel.Length));
            excel.WriteData(rowIndex++, 2, string.Format("{0}EA", filteredList.Count()));
            //excel.WriteData(rowIndex++, 2, string.Format("{0:0.00}%", reportModel.Yield));

            //excel.WriteData(20, 2, TranslationHelper.Instance.Translate("DEFECT_LIST", exportOption.Language));

            int defectInfoCount = Enum.GetNames(typeof(EDefectInfos)).Length;

            int startRow = 11;
            foreach (Defect defect in filteredList)
            {
                rowIndex = ((columnCount / 2) * defectInfoCount) + 1;

                if (columnCount % 2 == 0)
                {
                    colIndex = 0;
                }
                else
                {
                    colIndex = 4;
                }

                columnCount++;

                int imageRow = startRow + rowIndex;
                int endRow = startRow + rowIndex + defectInfoCount - 2;

                foreach (WTuple<EDefectInfos, bool> pair in exportOption.DefectInfoPairs)
                {
                    if (!pair.Item2)
                    {
                        continue;
                    }

                    object data = null;

                    switch (pair.Item1)
                    {
                        case EDefectInfos.DEFECT_NO:
                            data = defect.DefectNo;
                            break;
                        case EDefectInfos.DEFECT_TYPE:
                            data = defect.DefectTypeName;
                            break;
                        case EDefectInfos.POSX_MM:
                            data = Convert.ToDouble(string.Format("{0:0.000}", defect.DefectPos.X));
                            break;
                        case EDefectInfos.POSY_M:
                            data = Convert.ToDouble(string.Format("{0:0.000000}", defect.DefectPos.Y));
                            break;
                        case EDefectInfos.WIDTH_MM:
                            data = Convert.ToDouble(string.Format("{0:0.000}", defect.BoundingRect.Width));
                            break;
                        case EDefectInfos.HEIGHT_MM:
                            data = Convert.ToDouble(string.Format("{0:0.000}", defect.BoundingRect.Height));
                            break;
                        case EDefectInfos.AREA_MM2:
                            data = Convert.ToDouble(string.Format("{0:0.000000}", defect.Area));
                            break;
                        case EDefectInfos.MIN_GV:
                            data = Convert.ToDouble(string.Format("{0}", defect.MinGv));
                            break;
                        case EDefectInfos.MAX_GV:
                            data = Convert.ToDouble(string.Format("{0}", defect.MaxGv));
                            break;
                        case EDefectInfos.AVG_GV:
                            data = Convert.ToDouble(string.Format("{0:0.000}", defect.AvgGv));
                            break;
                        case EDefectInfos.IMAGE:
                            excel.WriteImage(imageRow, colIndex + 1, endRow, colIndex + 1, defect.DefectImage);
                            continue;
                    }

                    headerList.Add(new Tuple<int, int, object>(rowIndex, colIndex + 1, TranslationHelper.Instance.Translate(pair.Item1.ToString(), exportOption.Language)));
                    dataList.Add(new Tuple<int, int, object>(rowIndex, colIndex + 2, data));

                    rowIndex++;
                }

                if (progressSource != null)
                {
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    progressSource.StepIt();
                }

                List<Defect> typeDefectList;

                KeyValuePair<string, List<Defect>> findPair = DefectTypePairs.FirstOrDefault(x => x.Key == defect.DefectTypeName);
                if (findPair.Key == null)
                {
                    typeDefectList = new List<Defect>();
                    DefectTypePairs.Add(defect.DefectTypeName, typeDefectList);
                }
                else
                {
                    typeDefectList = findPair.Value;
                }

                typeDefectList.Add(defect);
            }

            excel.WriteRangeData(startRow, 1, headerList);
            excel.WriteRangeData(startRow, 1, dataList);
        }

        private void ExcellExportParameter(ExcelTemplateInfragistics excel, ExportOptionModel exportOption, UniScanC.Models.Model selectedModel)
        {
            excel.SelectSheet("ModelParameter");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();

            dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("MODEL_NAME", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", selectedModel.Name)));
            rowIndex++;

            foreach (VisionModel visionModel in selectedModel.VisionModels)
            {
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("AUTO_LIGHT_CALIBRATION", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.UseAutoLight)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("TOP_LIGHT_VALUE", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.TopLightValue)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("BOTTOM_LIGHT_VALUE", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.BottomLightValue)));

                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("PATTERN_WIDTH", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.PatternWidth)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("PATTERN_HEIGHT", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.PatternHeight)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("PATTERN_MARGIN_X", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.PatternMarginX)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("PATTERN_MARGIN_Y", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.PatternMarginY)));

                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("DARK_DEFECT_GV", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.ThresholdDark)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("LIGHT_DEFECT_GV", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.ThresholdLight)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, 0, TranslationHelper.Instance.Translate("DEFECT_SIZE_UM", exportOption.Language)));
                dataList.Add(new Tuple<int, int, object>(rowIndex++, 1, string.Format("{0}", visionModel.ThresholdSize)));

                columnIndex = 1;
                rowIndex++;
                string[] categoryTypeNameStr = Enum.GetNames(typeof(ECategoryTypeName));
                for (int i = 0; i < categoryTypeNameStr.Length; i++)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate(categoryTypeNameStr[i].ToString(), exportOption.Language)));
                }

                foreach (DefectCategory defectCategory in visionModel.DefectCategories)
                {
                    columnIndex = 0;
                    rowIndex++;
                    dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", defectCategory.Name)));
                    foreach (CategoryType categoryType in defectCategory.CategoryTypeList)
                    {
                        dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", categoryType.Use ? categoryType.Data : "-")));
                    }
                }

                rowIndex++;
                rowIndex++;
            }

            excel.WriteRangeData(1, 1, dataList);
        }

        private void ExcellExportRawData(ExcelTemplateInfragistics excel, ExportOptionModel exportOption, IEnumerable<Defect> filteredList, ProgressSource progressSource)
        {
            excel.SelectSheet("RawData");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();

            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("DEFECT_NO", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("DEFECT_TYPE", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("POSX_MM", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("POSY_M", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("WIDTH_MM", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("HEIGHT_MM", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("AREA_MM2", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("MIN_GV", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("MAX_GV", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("AVG_GV", exportOption.Language)));

            rowIndex = 1;
            foreach (Defect defect in filteredList)
            {
                columnIndex = 0;

                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToInt32(defect.DefectNo.ToString())));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, defect.DefectTypeName.ToString()));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.000}", defect.DefectPos.X))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.000000}", defect.DefectPos.Y))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.000}", defect.BoundingRect.Width))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.000}", defect.BoundingRect.Height))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.000000}", defect.Area))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0}", defect.MinGv))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0}", defect.MaxGv))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.000}", defect.AvgGv))));

                rowIndex++;

                if (progressSource != null)
                {
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        return;
                    }

                    progressSource.StepIt();
                }
            }

            excel.WriteRangeData(1, 1, dataList);
        }

        // Excell Pattern Export
        private void ExcellExportPattern(ExcelTemplateInfragistics excel, ExportOptionModel exportOption, IEnumerable<ProductResult> searchResults)
        {
            excel.SelectSheet("Pattern");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();
            IEnumerable<InspectResult> filteredList = searchResults.Cast<InspectResult>().Where(x => x.PatternSize.Width > 0 && x.PatternSize.Height > 0);

            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("Frame_Index", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("WIDTH_MM", exportOption.Language)));
            dataList.Add(new Tuple<int, int, object>(0, columnIndex++, TranslationHelper.Instance.Translate("HEIGHT_MM", exportOption.Language)));

            rowIndex = 1;
            foreach (InspectResult result in filteredList)
            {
                columnIndex = 0;

                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToInt32(result.FrameIndex.ToString())));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.0}", result.PatternSize.Width))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, Convert.ToDouble(string.Format("{0:0.0}", result.PatternSize.Height))));

                rowIndex++;
            }

            excel.WriteRangeData(1, 1, dataList);
        }

        // Excell Chart Export
        private void ExcellExportSizeChart(ExcelTemplate chartExcel, UniScanC.Models.Model selectedModel, ExportOptionModel exportOption, IEnumerable<Defect> filteredList, ProgressSource progressSource)
        {
            chartExcel.SelectSheet("SizeChart");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();

            Windows.Models.SizeChartSetting sizeChartSetting = exportOption.SizeChartSetting;

            var defectCategories = new List<DefectCategory>();
            foreach (VisionModel visionModel in selectedModel.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCategories.Find(x => x.Name == category.Name) == null)
                    {
                        defectCategories.Add(new DefectCategory(category));
                    }
                }
            }

            var totalDefectSizeCntDictionary = new Dictionary<string, Dictionary<int, int>>();
            var tempDictionary = new Dictionary<int, int>();
            foreach (DefectCategory category in defectCategories)
            {
                if (totalDefectSizeCntDictionary.ContainsKey(category.Name) == true)
                {
                    continue;
                }

                tempDictionary = new Dictionary<int, int>();
                for (int index = 0; index < sizeChartSetting.SizeStepCount; index++)
                {
                    tempDictionary.Add(index * sizeChartSetting.DefectStep + sizeChartSetting.MinDefectSize, 0);
                }

                tempDictionary.Add(-1, 0);

                totalDefectSizeCntDictionary.Add(category.Name, tempDictionary);
            }

            foreach (Defect defect in filteredList)
            {
                if (totalDefectSizeCntDictionary.ContainsKey(defect.DefectTypeName) == false)
                {
                    progressSource.StepIt();
                    continue;
                }

                Dictionary<int, int> defectCountDictionary = totalDefectSizeCntDictionary[defect.DefectTypeName];

                for (int i = 0; i < defectCountDictionary.Count(); i++)
                {
                    var defectKeys = defectCountDictionary.Keys.ToList();

                    int maxSize = defectKeys[i];
                    if (maxSize != -1)
                    {
                        int minSize = Math.Max(0, defectKeys[i] - sizeChartSetting.DefectStep);

                        double defectMaxSize = Math.Max(defect.BoundingRect.Width, defect.BoundingRect.Height) * 1000;

                        if (defectMaxSize >= minSize && defectMaxSize < maxSize)
                        {
                            defectCountDictionary[maxSize]++;
                            break;
                        }
                    }
                    else
                    {
                        defectCountDictionary[maxSize]++;
                        break;
                    }
                }
                progressSource.StepIt();
            }

            rowIndex = 0;
            columnIndex = 0;
            foreach (int defectSizeKeyValue in totalDefectSizeCntDictionary.Last().Value.Keys)
            {
                if (defectSizeKeyValue != totalDefectSizeCntDictionary.Last().Value.Last().Key)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, string.Format("~{0} um", defectSizeKeyValue)));
                }
                else
                {
                    var defectKeys = totalDefectSizeCntDictionary.Last().Value.Keys.ToList();
                    int index = defectKeys.Count() - 2;
                    dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, string.Format("{0} um↑", defectKeys[index])));
                }
            }
            chartExcel.WriteRangeData(2, 1, dataList);

            columnIndex = 0;
            foreach (KeyValuePair<string, Dictionary<int, int>> defecTypePair in totalDefectSizeCntDictionary)
            {
                rowIndex = 0;
                columnIndex++;
                foreach (KeyValuePair<int, int> sizePair in defecTypePair.Value)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, sizePair.Value));
                }
            }

            chartExcel.WriteRangeData(2, 1, dataList);
            for (int chartColIndex = 0; chartColIndex < defectCategories.Count; chartColIndex++)
            {
                chartExcel.WriteData(2, chartColIndex + 3, defectCategories[chartColIndex].Name);
            }

            chartExcel.SetChartRange();
        }

        private void ExcellExportLengthChart(ExcelTemplate chartExcel, UniScanC.Models.Model selectedModel, ExportOptionModel exportOption, IEnumerable<Defect> filteredList, ProgressSource progressSource)
        {
            chartExcel.SelectSheet("LengthTrendChart");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();

            int maxLength = (int)filteredList.Max(x => x.DefectPos.Y);
            int lengthStepCount = maxLength / exportOption.LengthChartSetting.StepLength + 2;
            int[] lengthArray = new int[lengthStepCount];

            var defectCategories = new List<DefectCategory>();
            foreach (VisionModel visionModel in selectedModel.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCategories.Find(x => x.Name == category.Name) == null)
                    {
                        defectCategories.Add(new DefectCategory(category));
                    }
                }
            }

            var totalLengthDictionary = new Dictionary<string, int[]>();
            foreach (DefectCategory category in defectCategories)
            {
                if (totalLengthDictionary.ContainsKey(category.Name) == true)
                {
                    continue;
                }

                int[] tempintArray = new int[lengthStepCount];

                for (int i = 0; i < lengthStepCount; i++)
                {
                    lengthArray[i] = i * 10;
                    tempintArray[i] = 0;
                }
                totalLengthDictionary.Add(category.Name, tempintArray);
            }

            foreach (Defect defect in filteredList)
            {
                if (totalLengthDictionary.ContainsKey(defect.DefectTypeName) == false)
                {
                    progressSource.StepIt();
                    continue;
                }

                int[] defectLengthDictionary = totalLengthDictionary[defect.DefectTypeName];

                for (int i = 0; i < defectLengthDictionary.Count(); i++)
                {
                    int maxChartLength = lengthArray[i];
                    int minChartLength = 0;

                    if (i != 0)
                    {
                        minChartLength = lengthArray[i - 1];
                    }

                    double position = defect.DefectPos.Y;
                    if (position >= minChartLength && position < maxChartLength)
                    {
                        defectLengthDictionary[i]++;
                    }
                }
                progressSource.StepIt();
            }

            rowIndex = 0;
            columnIndex = 0;
            for (int i = 0; i < lengthStepCount; i++)
            {
                dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, string.Format("{0}m", lengthArray[i])));
            }

            foreach (KeyValuePair<string, int[]> pair in totalLengthDictionary)
            {
                rowIndex = 0;
                columnIndex++;
                foreach (int lenth in pair.Value)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, lenth));
                }
            }

            chartExcel.WriteRangeData(2, 1, dataList);
            for (int chartColIndex = 0; chartColIndex < defectCategories.Count; chartColIndex++)
            {
                chartExcel.WriteData(2, chartColIndex + 3, defectCategories[chartColIndex].Name);
            }

            chartExcel.SetChartRange();
        }

        private void ExcellExportWidthChart(ExcelTemplate chartExcel, UniScanC.Models.Model selectedModel, ExportOptionModel exportOption, double selectedPatternWidth, IEnumerable<Defect> filteredList, ProgressSource progressSource)
        {
            chartExcel.SelectSheet("WidthTrendChart");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();

            int widthStepCount = (int)selectedPatternWidth / exportOption.WidthChartSetting.StepLength + 1;
            int[] widthArray = new int[widthStepCount];

            var defectCategories = new List<DefectCategory>();
            foreach (VisionModel visionModel in selectedModel.VisionModels)
            {
                foreach (DefectCategory category in visionModel.DefectCategories)
                {
                    if (defectCategories.Find(x => x.Name == category.Name) == null)
                    {
                        defectCategories.Add(new DefectCategory(category));
                    }
                }
            }

            var totalWidthDictionary = new Dictionary<string, int[]>();
            foreach (DefectCategory category in defectCategories)
            {
                if (totalWidthDictionary.ContainsKey(category.Name) == true)
                {
                    continue;
                }

                int[] tempintArray = new int[widthStepCount];

                for (int i = 0; i < widthStepCount; i++)
                {
                    widthArray[i] = i * 10;
                    tempintArray[i] = 0;
                }
                totalWidthDictionary.Add(category.Name, tempintArray);
            }

            foreach (Defect defect in filteredList)
            {
                if (totalWidthDictionary.ContainsKey(defect.DefectTypeName) == false)
                {
                    progressSource.StepIt();
                    continue;
                }

                int[] defectWidhtDictionary = totalWidthDictionary[defect.DefectTypeName];

                for (int i = 0; i < defectWidhtDictionary.Count(); i++)
                {
                    int maxChartWidth = widthArray[i];
                    int minChartWidth = 0;

                    if (i != 0)
                    {
                        minChartWidth = widthArray[i - 1];
                    }

                    double position = defect.DefectPos.X;
                    if (position >= minChartWidth && position < maxChartWidth)
                    {
                        defectWidhtDictionary[i]++;
                    }
                }
                progressSource.StepIt();
            }

            for (int i = 0; i < widthStepCount; i++)
            {
                dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, string.Format("{0}mm", widthArray[i])));
            }

            foreach (KeyValuePair<string, int[]> pair in totalWidthDictionary)
            {
                rowIndex = 0;
                columnIndex++;
                foreach (int width in pair.Value)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex++, columnIndex, width));
                }
            }

            chartExcel.WriteRangeData(2, 1, dataList);

            for (int chartColIndex = 0; chartColIndex < defectCategories.Count; chartColIndex++)
            {
                chartExcel.WriteData(2, chartColIndex + 3, defectCategories[chartColIndex].Name);
            }

            chartExcel.SetChartRange();
        }

        private void ExportDefectImage(string directoryPath, IEnumerable<Defect> filteredList, ProgressSource progressSource)
        {
            string imageFolderPath = Path.Combine(directoryPath, "defectImage");
            Directory.CreateDirectory(imageFolderPath);
            foreach (Defect defect in filteredList)
            {
                var bmpBitmapEncoder = new BmpBitmapEncoder();
                bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(defect.DefectImage));
                string fileName = string.Format("Sheet{0}_Cam{1}_No{2}.bmp", defect.FrameIndex, defect.ModuleNo, defect.DefectNo);
                string filePath = Path.Combine(imageFolderPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    bmpBitmapEncoder.Save(fileStream);
                }
            }
        }

        // Excell List Export
        public void ExcellExportSearchList(string fileName, IEnumerable<ReportModel> SortedTotalResultList, string[] defectTypesNames, ProgressSource progressSource = null)
        {
            string templateFile = string.Format(@"{0}\..\Result\SearchListTemplate.xlsx", Directory.GetCurrentDirectory());
            var excel = new ExcelTemplateInfragistics(fileName, templateFile);
            excel.SelectSheet("SearchList");
            int rowIndex = 0;
            int columnIndex = 0;
            var dataList = new List<Tuple<int, int, object>>();

            dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Date", LanguageSettings.Korean)));
            dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Time", LanguageSettings.Korean)));
            dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Model_Name", LanguageSettings.Korean)));
            dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Lot_No", LanguageSettings.Korean)));
            //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("LIGHT_VALUE", LanguageSettings.Korean)));
            dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Length_M", LanguageSettings.Korean)));
            dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Count", LanguageSettings.Korean)));
            //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Pass", LanguageSettings.Korean)));
            //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("NG", LanguageSettings.Korean)));
            //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, TranslationHelper.Instance.Translate("Yield", LanguageSettings.Korean)));

            // 콤보 박스에서 고른 모델이 있을 경우에는 불량 항목들 표시 해주기
            if (defectTypesNames != null)
            {
                foreach (string defectType in defectTypesNames)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, defectType));
                }
            }

            foreach (ReportModel reportModel in SortedTotalResultList)
            {
                columnIndex = 0;
                rowIndex++;
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.DateTime.ToString("yyyy-MM-dd"))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.DateTime.ToString("HH:mm:ss"))));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.ModelName)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.LotNo)));
                //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.LightValue)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0:0.00}", reportModel.Length)));
                dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.Count)));
                //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.Pass)));
                //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0}", reportModel.NG)));
                //dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, string.Format("{0:0.00}", reportModel.Yield)));

                foreach (int typeCount in reportModel.DefectTypes.Values)
                {
                    dataList.Add(new Tuple<int, int, object>(rowIndex, columnIndex++, typeCount));
                }

                if (progressSource != null)
                {
                    if (progressSource.CancellationTokenSource.IsCancellationRequested)
                    {
                        excel.Dispose();
                        return;
                    }
                    progressSource.StepIt();
                }

                excel.WriteRangeData(1, 1, dataList);
                excel.Save();
                excel.Dispose();
            }
        }

        // Method
        private IEnumerable<Defect> FilterDefectCategory(IEnumerable<Defect> defects, List<WTuple<string, bool>> defectCategoryPairs)
        {
            foreach (Defect defect in defects)
            {
                if (defectCategoryPairs.Exists(x => x.Item2 == true && x.Item1 == defect.DefectTypeName))
                {
                    yield return defect;
                }
            }
        }
    }
}
