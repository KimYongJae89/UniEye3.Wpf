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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// GlyphToggleButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GlyphToggleButton : System.Windows.Controls.Primitives.ToggleButton
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(GlyphToggleButton));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
          "Glyph", typeof(string), typeof(GlyphToggleButton));

        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
          "CornerRadius", typeof(CornerRadius), typeof(GlyphToggleButton));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public GlyphToggleButton()
        {
            InitializeComponent();
        }
    }
}
