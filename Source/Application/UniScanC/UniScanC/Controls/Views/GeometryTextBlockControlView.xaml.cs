using System;
using System.ComponentModel;
using System.Windows;
using UniScanC.Enums;

namespace UniScanC.Controls.Views
{
    /// <summary>
    /// GeometryTextBlock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GeometryTextBlockControlView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static readonly DependencyProperty GeometryTypeProperty =
            DependencyProperty.Register("GeometryType", typeof(EDefectMarkerType), typeof(GeometryTextBlockControlView),
                new FrameworkPropertyMetadata(OnGeometryTypeChanged));

        public EDefectMarkerType GeometryType
        {
            get => (EDefectMarkerType)GetValue(GeometryTypeProperty);
            set => SetValue(GeometryTypeProperty, value);
        }

        private static void OnGeometryTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var local = d as GeometryTextBlockControlView;
            local.UpdateText((EDefectMarkerType)e.NewValue);
        }

        private double angle = 0;
        public double Angle
        {
            get => angle;
            set
            {
                angle = value;
                OnPropertyChanged("Angle");
            }
        }

        public GeometryTextBlockControlView()
        {
            InitializeComponent();
        }

        private void UpdateText(EDefectMarkerType type)
        {
            double angle = 0;
            int code = 0x00;

            switch (type)
            {
                case EDefectMarkerType.Circle:
                    code = 0xF136;
                    break;
                case EDefectMarkerType.Triangle:
                    code = 0xF139;
                    angle = 180;
                    break;
                case EDefectMarkerType.Pyramid:
                    code = 0xF139;
                    break;
                case EDefectMarkerType.Square:
                    code = 0xE73B;
                    break;
                case EDefectMarkerType.Diamond:
                    code = 0xE73B;
                    angle = 45;
                    break;
            }

            Text = char.ConvertFromUtf32(code);
            Angle = angle;
        }
    }
}
