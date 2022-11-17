using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynMvp.InspectData
{
    public class TextInspResultArchiver : InspResultArchiver
    {
        public override void Save(ProductResult inspectionResult)
        {
            string fileName = string.Format("{0}\\result.csv", inspectionResult.ResultPath);

            var resultCount = new ResultCount();
            inspectionResult.GetResultCount(resultCount);

            var resultStringBuilder = new StringBuilder();

            string dateTimeString = inspectionResult.InspectStartTime.ToString("yyyy\\/MM\\/dd HH:mm:ss");

            resultStringBuilder.Append("model_name, serial_number, barcode_number, inspection_time, inspection_result, job_operator, num_defects, num_probe");
            resultStringBuilder.AppendLine();
            resultStringBuilder.Append(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                inspectionResult.ModelName, inspectionResult.InspectionNo, inspectionResult.InputBarcode, dateTimeString,
                inspectionResult.GetGoodNgStr(), inspectionResult.JobOperator, resultCount.numReject, inspectionResult.Count()));
            resultStringBuilder.AppendLine();

            foreach (KeyValuePair<string, int> defectCount in resultCount.numTargetTypeDefects)
            {
                if (defectCount.Key != "")
                {
                    resultStringBuilder.Append(string.Format("PartDefectCount , {0}, {1}", defectCount.Key, defectCount.Value));
                    resultStringBuilder.AppendLine();
                }
            }

            resultStringBuilder.Append("Header, InspectionStep, TargetGroupId, TargetId, TargetName, ProbeId, ProbeType, TargetType, ResultType, ValueCount, Name, Value, Ucl, Lcl");
            resultStringBuilder.AppendLine();


            foreach (ProbeResult probeResult in inspectionResult)
            {
                Probe probe = probeResult.Probe;
                int stepNo = probe.Target.InspectStep.StepNo;
                int cameraIndex = probe.Target.CameraIndex;
                int targetId = probe.Target.Id;
                string targetName = probe.Target.Name;
                int probeId = probe.Id;
                string probeType = probe.ProbeType.ToString();
                string targetType = probe.Target.TypeName;
                int numValue = probeResult.ResultValueList.Count;

                string valueStr = "";
                foreach (ResultValue resultValue in probeResult.ResultValueList)
                {
                    if (resultValue.Name == "Result")
                    {
                        continue;
                    }

                    valueStr += resultValue.Name + ";" + resultValue.Value.ToString() + ";" + resultValue.Ucl.ToString() + ";" + resultValue.Lcl.ToString() + "/";
                }

                valueStr = valueStr.TrimEnd('/');

                resultStringBuilder.Append(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}",
                    stepNo, cameraIndex, targetId, targetName, probeId, probeType, targetType,
                    probeResult.GetGoodNgStr(), valueStr));
                resultStringBuilder.AppendLine();
            }

            File.WriteAllText(fileName, resultStringBuilder.ToString(), Encoding.Default);
        }

        public override void GetProbeResult(ProductResult inspectionResult)
        {
            string dataFile = string.Format("{0}\\result.csv", inspectionResult.ResultPath);

            string[] lines = File.ReadAllLines(dataFile, Encoding.Default);

            foreach (string line in lines)
            {
                ProbeResult probeResult;

                string[] words = line.Split(new char[] { ',' }, 9);

                string type = words[5].Trim();
                if (type == "")
                {
                    continue;
                }

                probeResult = ProbeResult.CreateProbeResult((ProbeType)Enum.Parse(typeof(ProbeType), type));

                probeResult.StepNo = Convert.ToInt32(words[0].Trim());
                probeResult.CameraIndex = Convert.ToInt32(words[1].Trim());
                probeResult.TargetId = Convert.ToInt32(words[2].Trim());
                probeResult.TargetName = words[3].Trim();
                probeResult.ProbeId = Convert.ToInt32(words[4].Trim());
                probeResult.SetResult(words[7].Trim());

                string[] valueStrList = words[8].Trim().Split('/');

                foreach (string valueStr in valueStrList)
                {
                    if (string.IsNullOrEmpty(valueStr) == false)
                    {
                        string[] tokens = valueStr.Split(';');
                        var resultValue = new ResultValue(tokens[0], "", Convert.ToSingle(tokens[2]), Convert.ToSingle(tokens[3]), Convert.ToSingle(tokens[1]));

                        probeResult.AddResultValue(resultValue);
                    }
                }

                inspectionResult.AddProbeResult(probeResult);
            }
        }

        public override List<ProductResult> Load(string dataPath, DateTime startDate, DateTime endDate)
        {
            var dailyReportDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            DateTime loopEnd = endDate.Date + new TimeSpan(1, 0, 0, 0);

            var inspectionResultList = new List<ProductResult>();

            for (; dailyReportDate < loopEnd; dailyReportDate += new TimeSpan(1, 0, 0, 0))
            {
                string shortDate = dailyReportDate.ToString("yyyy-MM-dd");
                string searchPath = string.Format("{0}\\{1}", dataPath, shortDate);

                if (Directory.Exists(searchPath) == false)
                {
                    continue;
                }

                string[] directoryNames = Directory.GetDirectories(searchPath);

                foreach (string dirName in directoryNames)
                {
                    string defectPath = string.Format("{0}\\result.csv", dirName);

                    try
                    {
                        ProductResult inspectionResult = LoadInspResult(defectPath, startDate, endDate);

                        if (inspectionResult != null)
                        {
                            inspectionResultList.Add(inspectionResult);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Warn(LoggerType.Operation, "Fail to read result data. " + ex.Message);
                    }
                }
            }

            return inspectionResultList;
        }

        private ProductResult LoadInspResult(string dataPath, DateTime startDate, DateTime endDate)
        {
            if (File.Exists(dataPath) == false)
            {
                return null;
            }

            var inspectionResult = new ProductResult();

            using (var reader = new StreamReader(dataPath))
            {
                reader.ReadLine(); // Skip
                string[] words = reader.ReadLine().Split(new char[] { ',' });

                inspectionResult.ModelName = words[0].Trim();
                inspectionResult.InspectionNo = words[1].Trim();
                inspectionResult.InputBarcode = words[2].Trim();
                inspectionResult.InspectStartTime = DateTime.Parse(words[3]);

                if (inspectionResult.InspectStartTime < startDate || inspectionResult.InspectStartTime >= endDate)
                {
                    return null;
                }

                inspectionResult.JobOperator = words[5].Trim();

                inspectionResult.ResultPath = dataPath.Trim();
            }

            return inspectionResult;
        }

        public virtual ProductResult LoadInspResult(string dataPath)
        {
            string dataFile = string.Format("{0}\\result.csv", dataPath);

            if (File.Exists(dataFile) == false)
            {
                return null;
            }

            var inspectionResult = new ProductResult();

            using (var reader = new StreamReader(dataFile))
            {
                reader.ReadLine(); // Skip Header
                string[] words = reader.ReadLine().Split(new char[] { ',' });

                inspectionResult.ModelName = words[0].Trim();
                inspectionResult.InspectionNo = words[1].Trim();
                inspectionResult.InputBarcode = words[2].Trim();
                inspectionResult.InspectStartTime = DateTime.Parse(words[3]);
                inspectionResult.JobOperator = words[5].Trim();

                inspectionResult.ResultPath = dataPath.Trim();
            }

            return inspectionResult;
        }
    }
}
