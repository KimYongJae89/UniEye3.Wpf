using System;
using System.Collections.Generic;
using Unieye.WPF.Base.Controls;
using UniScanC.Data;

namespace UniScanC.Data
{
    public class GlossResult : DynMvp.InspectData.ProductResult
    {
        #region 생성자
        public GlossResult() : base() { }
        #endregion


        #region 속성
        public string LotNo { get; set; }

        public int RollPosition { get; set; }

        public Dictionary<string, GlossScanData> ScanData { get; set; } = new Dictionary<string, GlossScanData>();
        #endregion


        #region 메서드
        public void SetReelPosition(int reelPosition)
        {
            foreach (GlossScanData scanData in ScanData.Values)
            {
                scanData.ReelPosition = reelPosition;
            }
        }

        public static List<GlossResult> Parse(GlossDataImporter dataImporter, List<Dictionary<string, object>> traverseDatas, ProgressSource progressSource = null)
        {
            var dataList = new List<GlossResult>();
            foreach (Dictionary<string, object> traverseData in traverseDatas)
            {
                var glossResult = new GlossResult();
                glossResult.LotNo = Convert.ToString(traverseData["lot_name"]);
                glossResult.RollPosition = Convert.ToInt32(traverseData["roll_position"]);

                var dataTypeList = new List<string> { "Gloss", "Distance" };
                foreach (string layerName in dataTypeList)
                {
                    glossResult.ScanData.Add(layerName, new GlossScanData());
                }

                foreach (KeyValuePair<string, GlossScanData> scanDataPair in glossResult.ScanData)
                {
                    scanDataPair.Value.StartPosition = Convert.ToSingle(traverseData["start_position"]);
                    scanDataPair.Value.ValidStartPosition = Convert.ToSingle(traverseData["valid_start_position"]);
                    scanDataPair.Value.ValidEndPosition = Convert.ToSingle(traverseData["valid_end_position"]);
                    scanDataPair.Value.EndPosition = Convert.ToSingle(traverseData["end_position"]);
                    scanDataPair.Value.ReelPosition = Convert.ToInt32(traverseData["roll_position"]);
                    if (scanDataPair.Key == "Gloss")
                    {
                        scanDataPair.Value.Min = Convert.ToSingle(traverseData["gloss_min"]);
                        scanDataPair.Value.Max = Convert.ToSingle(traverseData["gloss_max"]);
                        scanDataPair.Value.Average = Convert.ToSingle(traverseData["gloss_avg"]);
                        scanDataPair.Value.Dev = Convert.ToSingle(traverseData["gloss_dev"]);
                    }
                    else if (scanDataPair.Key == "Distance")
                    {
                        scanDataPair.Value.Min = Convert.ToSingle(traverseData["distance_min"]);
                        scanDataPair.Value.Max = Convert.ToSingle(traverseData["distance_max"]);
                        scanDataPair.Value.Average = Convert.ToSingle(traverseData["distance_avg"]);
                        scanDataPair.Value.Dev = Convert.ToSingle(traverseData["distance_dev"]);
                    }
                }

                List<Dictionary<string, object>> thicknessDatas = dataImporter.ImportThicknessData(glossResult.LotNo, glossResult.RollPosition);
                foreach (Dictionary<string, object> thicknessData in thicknessDatas)
                {
                    DateTime date = DateTime.Now;
                    float traversePosition = Convert.ToSingle(thicknessData["traverse_position"]);
                    float sheetData = Convert.ToSingle(thicknessData["gloss_data"]);
                    float filmData = Convert.ToSingle(thicknessData["distance_data"]);
                    glossResult.ScanData["Gloss"].AddValue(date, traversePosition, sheetData);
                    glossResult.ScanData["Distance"].AddValue(date, traversePosition, filmData);
                }

                dataList.Add(glossResult);
            }

            return dataList;
        }
        #endregion
    }
}
