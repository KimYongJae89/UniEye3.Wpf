using MahApps.Metro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Unieye.WPF.Base.Extensions;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.Settings;

namespace Unieye.WPF.Base.Services
{
    public static class ThemeSelectorService
    {
        private const string key = "Theme";

        public static IEnumerable<Accent> Accents => ThemeManager.Accents;
        public static IEnumerable<AppTheme> Themes => ThemeManager.AppThemes;

        private static Accent accent;
        public static Accent Accent
        {
            get => accent ?? ThemeManager.GetAccent("Blue");
            set
            {
                accent = value;
                ThemeManager.ChangeAppStyle(Application.Current, accent, Theme);
                SaveThemeInSettingsAsync();
            }
        }

        private static AppTheme theme;
        public static AppTheme Theme
        {
            get => theme ?? ThemeManager.GetAppTheme("BaseDark");
            set
            {
                theme = value;
                ThemeManager.ChangeAppStyle(Application.Current, Accent, theme);
                SaveThemeInSettingsAsync();
            }
        }

        public static async Task InitializeAsync()
        {
            await LoadThemeFromSettingsAsync();
        }

        private static async Task LoadThemeFromSettingsAsync()
        {
            var directoryInfo = new DirectoryInfo(DynMvp.Base.BaseConfig.Instance().ConfigPath);
            Tuple<string, string> temp = await directoryInfo.ReadAsync<Tuple<string, string>>(key);

            string themeName = temp != null ? temp.Item1 : "BaseDark";
            string accentName = temp != null ? temp.Item2 : "Blue";

            theme = ThemeManager.GetAppTheme(themeName);
            accent = ThemeManager.GetAccent(accentName);

            ThemeManager.ChangeAppStyle(Application.Current, accent, theme);
        }

        public static async void SaveThemeInSettingsAsync()
        {
            var directoryInfo = new DirectoryInfo(DynMvp.Base.BaseConfig.Instance().ConfigPath);
            await directoryInfo.SaveAsync<Tuple<string, string>>(key, new Tuple<string, string>(Theme.Name, Accent.Name));
        }
    }
}
