using System;
using System.Collections.Generic;

namespace WPF.UniScanCM.Models
{
    public class ReportModel
    {
        public string ModelName { get; set; } = "";
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string LotNo { get; set; } = "";
        public double Length { get; set; } = 0;
        public int Count { get; set; } = 0;
        public int Pass { get; set; } = 0;
        public int NG { get; set; } = 0;
        public double Yield { get; set; } = 0;
        public string DatabasePath { get; set; } = "";
        public Dictionary<string, int> DefectTypes { get; set; } = new Dictionary<string, int>();
    }
}
