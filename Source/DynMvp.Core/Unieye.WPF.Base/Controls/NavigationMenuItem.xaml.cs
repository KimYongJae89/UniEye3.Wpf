using Authentication.Core;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Services;
using UniEye.Base.UI;

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// NavigationButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NavigationMenuItem : HamburgerMenuItem, IInitializable
    {
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(NavigationMenuItem), new PropertyMetadata(""));

        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
          "Glyph", typeof(string), typeof(NavigationMenuItem), new PropertyMetadata(""));

        public new bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public static new readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
          "IsEnabled", typeof(bool), typeof(NavigationMenuItem), new PropertyMetadata(true));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
          "IsSelected", typeof(bool), typeof(NavigationMenuItem), new PropertyMetadata(false));

        public TabKey TabKey { get; set; }

        public NavigationMenuItem()
        {
            InitializeComponent();
        }

        public NavigationMenuItem(TabKey tabKey, string text, string glyph, object tag)
        {
            InitializeComponent();

            Text = text;
            Glyph = glyph;
            Tag = tag;
            TabKey = tabKey;
        }

        public void Initialize()
        {
            if (Tag == null)
            {
                return;
            } (Tag as IInitializable)?.Initialize();
        }
    }
}
