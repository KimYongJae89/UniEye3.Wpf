using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unieye.WPF.Base.InspectFlow.Models;

namespace Unieye.WPF.Base.InspectFlow.Views
{
    /// <summary>
    /// InspectFlowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectionFlowView
    {
        public static readonly DependencyProperty InspectionFlowModelProperty =
                    DependencyProperty.Register("InspectionFlowModel", typeof(InspectionFlowModel), typeof(InspectionFlowView));

        public InspectionFlowModel InspectionFlowModel
        {
            get => (InspectionFlowModel)GetValue(InspectionFlowModelProperty);
            set => SetValue(InspectionFlowModelProperty, value);
        }

        public InspectionFlowView()
        {
            InitializeComponent();
        }
    }
}
