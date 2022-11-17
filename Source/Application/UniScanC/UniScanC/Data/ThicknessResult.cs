using System;
using System.Collections.Generic;
using Unieye.WPF.Base.Controls;
using UniScanC.Data;

namespace UniScanC.Data
{
    public class ThicknessResult : DynMvp.InspectData.ProductResult
    {
        #region 생성자
        public ThicknessResult() : base() { }
        #endregion


        #region 속성
        public string LotNo { get; set; }

        public int RollPosition { get; set; }

        public Dictionary<string, ThicknessScanData> ScanData { get; set; } = new Dictionary<string, ThicknessScanData>();
        #endregion


        #region 메서드
        public void SetReelPosition(int reelPosition)
        {
            foreach (ThicknessScanData scanData in ScanData.Values)
            {
                scanData.ReelPosition = reelPosition;
            }
        }

        public static List<ThicknessResult> Parse(ThicknessDataImporter dataImporter, List<Dictionary<string, object>> traverseDatas, ProgressSource progressSource = null)
        {
            var dataList = new List<ThicknessResult>();
            foreach (Dictionary<string, object> traverseData in traverseDatas)
            {
                var thicknessResult = new ThicknessResult();
                thicknessResult.LotNo = Convert.ToString(traverseData["lot_name"]);
                thicknessResult.RollPosition = Convert.ToInt32(traverseData["roll_position"]);

                var layerList = new List<string> { "Sheet", "Film" };
                foreach (string layerName in layerList)
                {
                    thicknessResult.ScanData.Add(layerName, new ThicknessScanData());
                }

                foreach (KeyValuePair<string, ThicknessScanData> scanDataPair in thicknessResult.ScanData)
                {
                    scanDataPair.Value.StartPosition = Convert.ToSingle(traverseData["start_position"]);
                    scanDataPair.Value.ValidStartPosition = Convert.ToSingle(traverseData["valid_start_position"]);
                    scanDataPair.Value.ValidEndPosition = Convert.ToSingle(traverseData["valid_end_position"]);
                    scanDataPair.Value.EndPosition = Convert.ToSingle(traverseData["end_position"]);
                    scanDataPair.Value.ReelPosition = Convert.ToInt32(traverseData["roll_position"]);
                    if (scanDataPair.Key == "Sheet")
                    {
                        scanDataPair.Value.Min = Convert.ToSingle(traverseData["sheet_min"]);
                        scanDataPair.Value.Max = Convert.ToSingle(traverseData["sheet_max"]);
                        scanDataPair.Value.Average = Convert.ToSingle(traverseData["sheet_avg"]);
                        scanDataPair.Value.LeftAverage = Convert.ToSingle(traverseData["sheet_left_avg"]);
                        scanDataPair.Value.RightAverage = Convert.ToSingle(traverseData["sheet_right_avg"]);
                    }
                    else if (scanDataPair.Key == "Film")
                    {
                        scanDataPair.Value.Min = Convert.ToSingle(traverseData["film_min"]);
                        scanDataPair.Value.Max = Convert.ToSingle(traverseData["film_max"]);
                        scanDataPair.Value.Average = Convert.ToSingle(traverseData["film_avg"]);
                        scanDataPair.Value.LeftAverage = Convert.ToSingle(traverseData["film_left_avg"]);
                        scanDataPair.Value.RightAverage = Convert.ToSingle(traverseData["film_right_avg"]);
                    }
                }

                List<Dictionary<string, object>> thicknessDatas = dataImporter.ImportThicknessData(thicknessResult.LotNo, thicknessResult.RollPosition);
                foreach (Dictionary<string, object> thicknessData in thicknessDatas)
                {
                    DateTime date = DateTime.Now;
                    float traversePosition = Convert.ToSingle(thicknessData["traverse_position"]);
                    float sheetData = Convert.ToSingle(thicknessData["sheet_data"]);
                    float filmData = Convert.ToSingle(thicknessData["film_data"]);
                    thicknessResult.ScanData["Sheet"].AddValue(date, traversePosition, sheetData);
                    thicknessResult.ScanData["Film"].AddValue(date, traversePosition, filmData);
                }

                dataList.Add(thicknessResult);
            }

            return dataList;
        }
        #endregion
    }
}
