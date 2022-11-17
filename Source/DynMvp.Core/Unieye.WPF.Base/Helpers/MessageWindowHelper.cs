using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unieye.WPF.Base.Controls;

namespace Unieye.WPF.Base.Helpers
{
    public static class MessageWindowHelper
    {
        private static MetroDialogSettings settings = new MetroDialogSettings();
        public static async Task<MessageDialogResult> ShowMessage(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            settings.AnimateShow = false;
            settings.AnimateHide = false;

            return await DialogCoordinator.Instance.ShowMessageAsync(context, title, message, style, settings);
        }

        public static async Task<bool> ShowMessageBox(string title, string message, MessageBoxButton type = MessageBoxButton.OK)
        {
            return await MessageWindowHelper.ShowChildWindow<bool>(new MessageWindow(title, message, type));
        }

        public static async Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false)
        {
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            settings.AnimateShow = false;
            settings.AnimateHide = false;

            return await DialogCoordinator.Instance.ShowProgressAsync(context, title, message, isCancelable, settings);
        }

        public static async Task ShowProgress(string title, string description, Action action, bool autoCloseMode = false, ProgressSource source = null)
        {
            await Application.Current.MainWindow.ShowChildWindowAsync(new UnieyeProgressControl(title, description, action, autoCloseMode, source));
        }

        public static async Task ShowProgress(string title, string description, List<Action> actionList, bool autoCloseMode = false, ProgressSource source = null)
        {
            await Application.Current.MainWindow.ShowChildWindowAsync(new UnieyeProgressControl(title, description, actionList, autoCloseMode, source));
        }

        public static async Task<T> ShowChildWindow<T>(ChildWindow childWindow)
        {
            return await Application.Current.MainWindow.ShowChildWindowAsync<T>(childWindow);
        }
    }
}
