using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// ImagePasswordBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GlyphPasswordBox : TextBox
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region DependencyProperty

        private static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(GlyphPasswordBox));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
          "Glyph", typeof(string), typeof(GlyphPasswordBox), new PropertyMetadata(""));

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(GlyphPasswordBox));

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public static readonly DependencyProperty ImageBackgroundProperty =
            DependencyProperty.Register("ImageBackground", typeof(Brush), typeof(GlyphPasswordBox));

        public Brush ImageBackground
        {
            get => (Brush)GetValue(ImageBackgroundProperty);
            set => SetValue(ImageBackgroundProperty, value);
        }

        #endregion

        public GlyphPasswordBox()
        {
            InitializeComponent();
        }
    }
}
