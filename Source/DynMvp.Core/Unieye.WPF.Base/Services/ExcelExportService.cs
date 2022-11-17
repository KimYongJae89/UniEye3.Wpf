using Infragistics.Documents.Excel;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Extensions;
using Excel = Microsoft.Office.Interop.Excel;
using InfraWorkbook = Infragistics.Documents.Excel.Workbook;
using InfraWorksheet = Infragistics.Documents.Excel.Worksheet;
using MsWorkbook = Microsoft.Office.Interop.Excel.Workbook;
using MsWorksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace Unieye.WPF.Base.Services
{
    public class ExcelTemplate
    {
        protected string FilePath { get; set; }
        protected string TemplateFilePath { get; set; }

        private Excel.Application excelApp;
        private MsWorkbook workbook;
        private MsWorksheet worksheet;

        private MsWorkbook copyTargetWorkBook;
        private MsWorkbook pasteTargetWorkBook;


        public ExcelTemplate()
        {

        }

        public ExcelTemplate(string filePath, string templateFilePath = "")
        {
            excelApp = new Excel.Application();
            FilePath = filePath;

            if (templateFilePath != "")
            {
                workbook = excelApp.Workbooks.Open(templateFilePath);
            }
            else
            {
                workbook = excelApp.Workbooks.Add();
            }

            TemplateFilePath = templateFilePath;
        }

        public virtual void Dispose()
        {
            workbook?.Close();
            copyTargetWorkBook?.Close();
            pasteTargetWorkBook?.Close();
            excelApp?.Quit();

            ReleaseExcelObject(worksheet);
            ReleaseExcelObject(workbook);
            ReleaseExcelObject(copyTargetWorkBook);
            ReleaseExcelObject(pasteTargetWorkBook);
            ReleaseExcelObject(excelApp);
        }

        // Worksheet를 선택한다.
        public virtual void SelectSheet(string sheetName, bool isCreate = false)
        {
            if (worksheet != null)
            {
                ReleaseExcelObject(worksheet);
            }

            bool isExist = false;
            foreach (MsWorksheet sheet in workbook.Sheets)
            {
                if (sheet.Name == sheetName)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist)
            {
                worksheet = workbook.Worksheets.get_Item(sheetName) as MsWorksheet;
            }
            else if (isCreate)
            {
                worksheet = workbook.Sheets.Add(After: workbook.Sheets[workbook.Sheets.Count]) as MsWorksheet;
                worksheet.Name = sheetName;
            }
        }

        public void SetChartRange()
        {
            Excel.Range chartRange;
            Range last = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            var xlCharts = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
            ChartObject myChart = xlCharts.Add(500, 20, 800, 500);
            Chart chartPage = myChart.Chart;

            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column;

            chartRange = worksheet.get_Range("b2", last);
            chartPage.SetSourceData(chartRange);
            chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
            worksheet.Columns.AutoFit();
        }

        public virtual void WriteData(int row, int col, object data)
        {
            if (worksheet != null)
            {
                worksheet.Cells[row, col] = data;
            }
        }

        /****
         * startRow : 데이터가 시작되는 열 번호
         * startCol : 데이터가 시작되는 행 번호
         * dataTuple : 영역 데이터
         * - Item1 : Row
         * - Item2 : Column
         * - Item3 : Data
         */
        public virtual void WriteRangeData(int startRow, int startCol, List<Tuple<int, int, object>> dataTuple)
        {
            if (worksheet != null && dataTuple.Count > 0)
            {
                //var cells = GetCells(worksheet, dataTuple);
                //int rows = cells.GetLength(0);
                //int cols = cells.GetLength(1);
                // Excel sheet는 1 base index기 때문에 1을 더해준다.
                //Range range = worksheet.get_Range((Range)(worksheet.Cells[startRow + 1, startCol + 1]),
                //    (Range)(worksheet.Cells[startRow + rows, startCol + cols])).Value;

                // Excel sheet는 1 base index기 때문에 1을 더해준다.
                startRow++;
                startCol++;

                //var imageFolderPath = Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath), "Images");
                //Directory.CreateDirectory(imageFolderPath);
                //var directoryInfo = new DirectoryInfo(imageFolderPath);

                foreach (Tuple<int, int, object> tuple in dataTuple)
                {
                    int row = tuple.Item1 + startRow;
                    int col = tuple.Item2 + startCol;

                    var range = worksheet.Cells[row, col] as Range;

                    //if (tuple.Item3 is BitmapSource)
                    //{
                    //    range.ColumnWidth = 50;
                    //    range.RowHeight = 50;

                    //    var imagePath = Path.Combine(imageFolderPath, $"{index}.png");
                    //    (tuple.Item3 as BitmapSource).Save(directoryInfo, $"{index}");

                    //    index++;

                    //    //var Bitmap = ImageHelper.BitmapSourceToBitmap(tuple.Item3 as BitmapSource, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    //    //ImageHelper.SaveImage(Bitmap, tempFile);
                    //    //var shape = worksheet.Shapes.AddShape(MsoAutoShapeType.msoShapeRectangle, range.Left + 1, range.Top + 1, range.ColumnWidth - 2, range.RowHeight - 2);
                    //    //shape.Fill.UserPicture(imagePath);

                    //    //worksheet.Shapes.AddPicture(tempFile, Microsoft.Office.Core.MsoTriState.msoFalse, MsoTriState.msoTrue, range.Left, range.Top, range.ColumnWidth, range.RowHeight);
                    //    //System.IO.File.Delete(tempFile);
                    //}
                    //else
                    range.Value = tuple.Item3;
                }
            }
        }

        private object[,] GetCells(MsWorksheet worksheet, List<Tuple<int, int, object>> dataTuple)
        {
            int maxRow = dataTuple.Max(x => x.Item1);
            int maxCol = dataTuple.Max(x => x.Item2);

            object[,] cells = new object[maxRow + 1, maxCol + 1];

            foreach (Tuple<int, int, object> tuple in dataTuple)
            {
                if (tuple.Item3 is BitmapSource)
                {
                    //string tempFile = "D:\\Temp.bmp";
                    //var Bitmap = ImageHelper.BitmapSourceToBitmap(tuple.Item3 as BitmapSource, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    //Bitmap.Save(tempFile);

                    //worksheet.Shapes.AddPicture2(tempFile, Microsoft.Office.Core.MsoTriState.msoFalse, MsoTriState.msoTrue, 0, 0, 50, 50);
                    //System.IO.File.Delete(tempFile);
                }
                else
                {
                    cells[tuple.Item1, tuple.Item2] = tuple.Item3;
                }
            }

            return cells;
        }

        private void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
            }
        }

        public virtual void Save()
        {
            excelApp.DisplayAlerts = false;
            workbook?.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookDefault);
            pasteTargetWorkBook?.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookDefault);
            //workbook.SaveAs(FilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
            //            Type.Missing, Type.Missing);
        }

        public virtual void Combine(string pasteTargetPath, string copyTargetPath)
        {
            if (string.IsNullOrEmpty(pasteTargetPath) || string.IsNullOrEmpty(copyTargetPath))
            {
                return;
            }

            FilePath = pasteTargetPath;
            excelApp = new Excel.Application();
            //Excel.Workbook copyTargetWorkBook;
            //Excel.Workbook pasteTargetWorkbook = workbook;
            pasteTargetWorkBook = excelApp.Workbooks.Open(pasteTargetPath);
            copyTargetWorkBook = excelApp.Workbooks.Open(copyTargetPath);

            int index = pasteTargetWorkBook.Sheets.Count;

            foreach (Excel.Worksheet sheet in copyTargetWorkBook.Worksheets)
            {
                sheet.Copy(pasteTargetWorkBook.Sheets[index++]);
            }
        }
    }

    public class ExcelTemplateInfragistics : ExcelTemplate
    {
        private InfraWorkbook workbook;
        private InfraWorksheet worksheet;

        public ExcelTemplateInfragistics(string filePath, string templateFilePath)
        {
            FilePath = filePath;
            TemplateFilePath = templateFilePath;
            if (templateFilePath != "")
            {
                workbook = InfraWorkbook.Load(templateFilePath);
            }
        }

        public override void Dispose()
        {

        }

        // Worksheet를 선택한다.
        public override void SelectSheet(string sheetName, bool isCreate = false)
        {
            if (workbook.Worksheets.Any(x => x.Name == sheetName))
            {
                worksheet = workbook.Worksheets[sheetName];
            }
            else if (isCreate)
            {
                worksheet = workbook.Worksheets.Add(sheetName);
            }
        }

        public override void WriteData(int row, int col, object data)
        {
            if (worksheet == null)
            {
                return;
            }

            worksheet.Rows[row].Cells[col].Value = data;
        }

        /****
         * startRow : 데이터가 시작되는 열 번호
         * startCol : 데이터가 시작되는 행 번호
         * dataTuple : 영역 데이터
         * - Item1 : Row
         * - Item2 : Column
         * - Item3 : Data
         */
        public override void WriteRangeData(int startRow, int startCol, List<Tuple<int, int, object>> dataTuple)
        {
            if (worksheet != null && dataTuple.Count > 0)
            {
                foreach (Tuple<int, int, object> tuple in dataTuple)
                {
                    int row = tuple.Item1 + startRow;
                    int col = tuple.Item2 + startCol;

                    WorksheetCell cell = worksheet.Rows[row].Cells[col];

                    if (tuple.Item3 is BitmapSource)
                    {
                        continue;
                    }

                    cell.Value = tuple.Item3;
                }
            }
        }

        public void WriteImage(int startRow, int startCol, int endRow, int endCol, object data)
        {
            if (worksheet == null)
            {
                return;
            }

            if (!(data is BitmapImage BitmapSource))
            {
                return;
            }

            var memoryStream = new MemoryStream();
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(BitmapSource));
            encoder.Save(memoryStream);

            //var bitmap = ImageHelper.BitmapSourceToBitmap(data as BitmapSource, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            //bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //memoryStream.Flush();
            var jpegImage = System.Drawing.Image.FromStream(memoryStream);
            //bitmap.Dispose();

            WorksheetCell startCell = worksheet.Rows[startRow].Cells[startCol];
            WorksheetCell endCell = worksheet.Rows[endRow].Cells[endCol];

            var sheetImage = new WorksheetImage(jpegImage);

            sheetImage.TopLeftCornerCell = startCell;
            sheetImage.TopLeftCornerPosition = new System.Windows.Point(0.0f, 0.0f);
            //sheetImage.TopLeftCornerPosition = new System.Windows.Point(5, 15);
            sheetImage.BottomRightCornerCell = endCell;
            //sheetImage.BottomRightCornerPosition = new System.Windows.Point(0.0f, 0.0f);
            sheetImage.BottomRightCornerPosition = new System.Windows.Point(
                worksheet.Columns[startCell.ColumnIndex].Width /*/ 30 - 20*/,
                worksheet.Rows[startCell.RowIndex].Height/* / 22 - 45*/);

            worksheet.Shapes.Add(sheetImage);
        }

        public override void Save()
        {
            workbook.Save(FilePath);
        }

        //public void SetChartRange()
        //{
        //    WorksheetRegion chartRange;
        //    WorksheetRegion last = worksheet.GetCell(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
        //    Excel.ChartObjects xlCharts = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
        //    Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(500, 20, 800, 500);
        //    Excel.Chart chartPage = myChart.Chart;

        //    int lastUsedRow = last.Row;
        //    int lastUsedColumn = last.Column;

        //    chartRange = worksheet.get_Range("b2", last);
        //    chartPage.SetSourceData(chartRange);
        //    chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
        //}
    }
}
