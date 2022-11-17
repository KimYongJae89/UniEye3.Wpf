using System.Collections.Generic;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.Windows.Models;

namespace WPF.UniScanCM.Models
{
    public class ExportOptionModel : Observable
    {
        private List<WTuple<EDefectInfos, bool>> defectInfoPairs = new List<WTuple<EDefectInfos, bool>>();
        public List<WTuple<EDefectInfos, bool>> DefectInfoPairs
        {
            get => defectInfoPairs;
            set => Set(ref defectInfoPairs, value);
        }

        private List<WTuple<string, bool>> defectCategoryPairs = new List<WTuple<string, bool>>();
        public List<WTuple<string, bool>> DefectCategoryPairs
        {
            get => defectCategoryPairs;
            set => Set(ref defectCategoryPairs, value);
        }

        private List<WTuple<EChartType, bool>> chartPairs = new List<WTuple<EChartType, bool>>();
        public List<WTuple<EChartType, bool>> ChartPairs
        {
            get => chartPairs;
            set => Set(ref chartPairs, value);
        }

        private SizeChartSetting sizeChartSetting = new SizeChartSetting();
        public SizeChartSetting SizeChartSetting
        {
            get => sizeChartSetting;
            set => Set(ref sizeChartSetting, value);
        }

        private LengthChartSetting lengthChartSetting = new LengthChartSetting();
        public LengthChartSetting LengthChartSetting
        {
            get => lengthChartSetting;
            set => Set(ref lengthChartSetting, value);
        }

        private WidthChartSetting widthChartSetting = new WidthChartSetting();
        public WidthChartSetting WidthChartSetting
        {
            get => widthChartSetting;
            set => Set(ref widthChartSetting, value);
        }

        private ECustomer customerSetting = ECustomer.General;
        public ECustomer CustomerSetting
        {
            get => customerSetting;
            set => Set(ref customerSetting, value);
        }

        private ESortDirections sortDirection = ESortDirections.Ascending;
        public ESortDirections SortDirection
        {
            get => sortDirection;
            set => Set(ref sortDirection, value);
        }

        private LanguageSettings language = LanguageSettings.English;
        public LanguageSettings Language
        {
            get => language;
            set => Set(ref language, value);
        }

        public ExportOptionModel()
        {

        }

        public void CopyFrom(ExportOptionModel srcModel)
        {
            DefectInfoPairs.Clear();
            foreach (WTuple<EDefectInfos, bool> tuple in srcModel.DefectInfoPairs)
            {
                DefectInfoPairs.Add(new WTuple<EDefectInfos, bool>(tuple.Item1, tuple.Item2));
            }

            DefectCategoryPairs.Clear();
            foreach (WTuple<string, bool> tuple in srcModel.DefectCategoryPairs)
            {
                DefectCategoryPairs.Add(new WTuple<string, bool>(tuple.Item1, tuple.Item2));
            }

            ChartPairs.Clear();
            foreach (WTuple<EChartType, bool> tuple in srcModel.ChartPairs)
            {
                ChartPairs.Add(new WTuple<EChartType, bool>(tuple.Item1, tuple.Item2));
            }

            SizeChartSetting.CopyFrom(srcModel.SizeChartSetting);
            LengthChartSetting.CopyFrom(srcModel.LengthChartSetting);
            WidthChartSetting.CopyFrom(srcModel.WidthChartSetting);

            SortDirection = srcModel.SortDirection;

            Language = srcModel.Language;
        }
    }
}
