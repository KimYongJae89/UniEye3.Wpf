using DynMvp.Data;
using DynMvp.InspectData;
using System.Collections.Generic;
using System.IO;
using UniScanC.Data;

namespace WPF.ThicknessMeasure.Data
{
    public class ThicknessDataExporter : IDataExporter
    {
        #region 메서드
        public void Export(ProductResult productResult)
        {
            var result = productResult as ThicknessResult;
            Dictionary<string, ThicknessScanData> scanDatas = result.ScanData;

            foreach (ThicknessScanData scanData in scanDatas.Values)
            {
                string dataFile = Path.Combine(result.ResultPath, scanData.StartTime.ToString("yyyyMMdd_HHmmssfff") + "_RefractionData.csv");
                using (var sw = new StreamWriter(dataFile, true))
                {
                    sw.WriteLine("Time,Position,Thickness,Refraction,Angle");
                    foreach (ThicknessData thicknessData in scanData.DataList)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4}",
                            thicknessData.Time.ToString("yyyyMMdd - HH:mm:ss:fff"),
                            thicknessData.Position,
                            thicknessData.Thickness,
                            thicknessData.Refraction,
                            thicknessData.Angle));
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        #endregion
    }
}
