using System.Drawing;
using System.Linq;
using UniScanC.Controls.ViewModels;
using UniScanC.Data;

namespace UniScanC.Controls.Views
{
    /// <summary>
    /// DefectMapChart.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DefectMapFrameControlView
    {
        public DefectMapFrameControlView()
        {
            InitializeComponent();
        }

        private void XamDataChart_SeriesMouseLeftButtonUp(object sender, Infragistics.Controls.Charts.DataChartMouseButtonEventArgs e)
        {
            var dataContext = DataContext as DefectMapFrameControlViewModel;
            if (e.Item is PointF selectedPoint)
            {
                if (dataContext.DicDefectMappingMap.ContainsValue(selectedPoint))
                {
                    Defect defect = dataContext.DicDefectMappingMap.FirstOrDefault(x => x.Value == selectedPoint).Key;
                    dataContext.SelectedDefect = defect;
                }
            }
        }
    }
}