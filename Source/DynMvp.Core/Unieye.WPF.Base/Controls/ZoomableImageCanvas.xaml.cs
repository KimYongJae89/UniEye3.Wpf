using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Services;

namespace Unieye.WPF.Base.Controls
{
    public class ZoomableImageCanvasViewModel : Observable
    {
        public ZoomService ZoomService { get; set; }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            set
            {
                bool zoomFitRequired = imageSource == null ? true : false;

                Set(ref imageSource, value);

                ZoomService.FitToSize(imageSource.Width, imageSource.Height);
            }
        }

        public ZoomableImageCanvasViewModel(Canvas canvas)
        {
            ZoomService = new ZoomService(canvas);
        }
    }

    /// <summary>
    /// ImageCanvas.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ZoomableImageCanvas : UserControl
    {
        public ZoomableImageCanvasViewModel ViewModel;

        public ZoomableImageCanvas()
        {
            ViewModel = new ZoomableImageCanvasViewModel(imageCanvas);
            InitializeComponent();
        }
    }
}
