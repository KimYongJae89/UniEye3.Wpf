using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UniEye.StringManagement.Base.Helpers;
using UniEye.StringManagement.Helpers;

namespace UniEye.StringManagement.ViewModels
{
    public class KeyWindowViewModel : Observable
    {
        public enum WindowMode
        {
            Add, Edit
        }

        private WindowMode keyWindowMode;
        public WindowMode KeyWindowMode
        {
            get => keyWindowMode;
            set => Set(ref keyWindowMode, value);
        }

        private KeyValuePair<string, ObservableCollection<ObservableValue<string>>> selectedPair = new KeyValuePair<string, ObservableCollection<ObservableValue<string>>>();
        public KeyValuePair<string, ObservableCollection<ObservableValue<string>>> SelectedPair
        {
            get => selectedPair;
            set => Set(ref selectedPair, value);
        }

        private Dictionary<string, ObservableCollection<ObservableValue<string>>> templanguageDictionary;
        public Dictionary<string, ObservableCollection<ObservableValue<string>>> TemplanguageDictionary
        {
            get => templanguageDictionary;
            set => Set(ref templanguageDictionary, value);
        }

        private int fileCount;
        public KeyWindowViewModel(Dictionary<string, ObservableCollection<ObservableValue<string>>> templanguageDictionary)
        {
            TemplanguageDictionary = templanguageDictionary;
        }
        public KeyWindowViewModel()
        {
        }
        public KeyWindowViewModel(Dictionary<string, ObservableCollection<ObservableValue<string>>> templanguageDictionary, int fileCount)
        {
            TemplanguageDictionary = templanguageDictionary;
            this.fileCount = fileCount;
        }

        public KeyWindowViewModel(Dictionary<string, ObservableCollection<ObservableValue<string>>> templanguageDictionary, int fileCount, KeyValuePair<string, ObservableCollection<ObservableValue<string>>> seletedPair)
        {
            TemplanguageDictionary = templanguageDictionary;
            SelectedPair = seletedPair;
            this.fileCount = fileCount;
            KeyString = selectedPair.Key;
        }

        private string keyString;
        public string KeyString
        {
            get => keyString;
            set
            {
                Set(ref keyString, value);

                MessageString = "";

                if (TemplanguageDictionary.ContainsKey(keyString.ToUpper()))
                {
                    MessageString = "입력한 Key가 이미 존재합니다.";
                }
            }
        }

        private string messageString;
        public string MessageString
        {
            get => messageString;
            set => Set(ref messageString, value);
        }

        private ICommand okButtonClick;
        public ICommand OkButtonClick => okButtonClick ?? (okButtonClick = new RelayCommand<Window>(OkButtonAction));
        private void OkButtonAction(Window wnd)
        {
            MessageString = "";

            if (string.IsNullOrEmpty(KeyString))
            {
                MessageString = "Key값을 입력해주세요.";
                return;
            }

            if (keyString.Contains(" "))
            {
                MessageString = "공백없이 입력해주세요.";
                return;
            }

            switch (keyWindowMode)
            {
                case WindowMode.Add:
                    if (!AddKey())
                    {
                        return;
                    }

                    break;
                case WindowMode.Edit:
                    if (!EditKey())
                    {
                        return;
                    }

                    break;
                default:
                    break;
            }
            wnd.DialogResult = true;
        }

        private bool EditKey()
        {
            if (TemplanguageDictionary.ContainsKey(keyString.ToUpper()))
            {
                MessageString = "입력한 Key가 이미 존재합니다.";
                return false;
            }
            else
            {
                TemplanguageDictionary.Remove(selectedPair.Key);
                TemplanguageDictionary.Add(keyString.ToUpper(), selectedPair.Value);

                return true;
            }
        }

        private bool AddKey()
        {
            if (TemplanguageDictionary.ContainsKey(keyString.ToUpper()))
            {
                MessageString = "입력한 Key가 이미 존재합니다.";
                return false;
            }
            else
            {
                var list = new ObservableCollection<ObservableValue<string>>();
                for (int index = 0; index < fileCount; index++)
                {
                    var langData = new ObservableValue<string>();
                    list.Add(langData);
                }
                TemplanguageDictionary.Add(KeyString.ToUpper(), list);
                return true;
            }
        }

        private ICommand cancelButtonClick;
        public ICommand CancelButtonClick => cancelButtonClick ?? (cancelButtonClick = new RelayCommand<Window>(CancelButtonAction));
        private void CancelButtonAction(Window wnd)
        {
            wnd.DialogResult = false;
        }

        private static MetroDialogSettings settings = new MetroDialogSettings();
        public static async Task<MessageDialogResult> ShowMessage(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            settings.AnimateShow = false;
            settings.AnimateHide = false;
            return await DialogCoordinator.Instance.ShowMessageAsync(context, title, message, style, settings);
        }
    }
}
