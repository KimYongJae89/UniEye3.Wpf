using DynMvp.Base;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DynMvp.InspectData
{
    public class TextDailyReport
    {
        public string ResultPath { get; set; }

        protected void InitDailyReportColumns(DataTable dataTable)
        {
            dataTable.Columns.Add(StringManager.GetString("SerialNo"), typeof(string));
            dataTable.Columns.Add(StringManager.GetString("Barcode"), typeof(string));
            dataTable.Columns.Add(StringManager.GetString("Inspection Time"), typeof(DateTime));
            dataTable.Columns.Add(StringManager.GetString("Inspection Result"), typeof(string));
            dataTable.Columns.Add(StringManager.GetString("Operator"), typeof(string));
            dataTable.Columns.Add(StringManager.GetString("Model"), typeof(string));
            dataTable.Columns.Add(StringManager.GetString("InspectionId"), typeof(string));
        }

        public DataTable GetDailyReportData(string selModelName, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            InitDailyReportColumns(dataTable);

            if (selModelName != "")
            {
                string modelPath = string.Format("{0}\\{1}", ResultPath, selModelName);
                AddData(dataTable, modelPath, startDate, endDate);
            }
            else
            {
                string[] directoryNames = Directory.GetDirectories(ResultPath);

                foreach (string dirName in directoryNames)
                {
                    AddData(dataTable, dirName, startDate, endDate);
                }
            }

            return dataTable;
        }

        private void AddData(DataTable dataTable, string modelPath, DateTime startDate, DateTime endDate)
        {
            var dailyReportDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            DateTime loopEnd = endDate.Date + new TimeSpan(1, 0, 0, 0);

            for (; dailyReportDate < loopEnd; dailyReportDate += new TimeSpan(1, 0, 0, 0))
            {
                string shortDate = dailyReportDate.ToString("yyyy-MM-dd");
                string searchPath = string.Format("{0}\\{1}", modelPath, shortDate);

                if (Directory.Exists(searchPath) == false)
                {
                    continue;
                }

                string[] directoryNames = Directory.GetDirectories(searchPath);

                foreach (string dirName in directoryNames)
                {
                    string defectPath = string.Format("{0}\\result.csv", dirName);

                    if (File.Exists(defectPath) == false)
                    {
                        continue;
                    }

                    try
                    {
                        using (var reader = new StreamReader(defectPath))
                        {
                            reader.ReadLine(); // Skip
                            string[] words = reader.ReadLine().Split(new char[] { ',' });

                            string modelName;
                            string serialNo;
                            string barcodeNumber;
                            DateTime dateTime;
                            string inspectionResult;
                            string jobOperator;

                            if (words.Count() == 6)
                            {
                                modelName = words[0].Trim();
                                barcodeNumber = words[1].Trim();
                                serialNo = barcodeNumber;
                                dateTime = DateTime.Parse(words[2]);

                                if (dateTime < startDate || dateTime >= endDate)
                                {
                                    continue;
                                }

                                inspectionResult = words[3].Trim();
                                jobOperator = words[4].Trim();
                            }
                            else
                            {
                                modelName = words[0].Trim();
                                serialNo = words[1].Trim();
                                barcodeNumber = words[2].Trim();
                                dateTime = DateTime.Parse(words[3]);

                                if (dateTime < startDate || dateTime >= endDate)
                                {
                                    continue;
                                }

                                inspectionResult = words[4].Trim();
                                jobOperator = words[5].Trim();
                            }

                            string inspectionId = defectPath.Trim();

                            dataTable.Rows.Add(serialNo, barcodeNumber, dateTime, inspectionResult, jobOperator, modelName, inspectionId);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Warn(LoggerType.Operation, "Fail to read result data. " + ex.Message);
                    }
                }
            }
        }
    }
}
