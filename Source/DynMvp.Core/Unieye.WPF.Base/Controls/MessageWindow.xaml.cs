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
using System.Windows.Shapes;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// MessageWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MessageWindow : MahApps.Metro.SimpleChildWindow.ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region 변수

        private string messageTitle;
        public string MessageTitle
        {
            get => messageTitle;
            set
            {
                messageTitle = value;
                OnPropertyChanged("MessageTitle");
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private string yesButtonCaption;
        public string YesButtonCaption
        {
            get => yesButtonCaption;
            set
            {
                yesButtonCaption = value;
                OnPropertyChanged("YesButtonCaption");
            }
        }

        private string noButtonCaption;
        public string NoButtonCaption
        {
            get => noButtonCaption;
            set
            {
                noButtonCaption = value;
                OnPropertyChanged("NoButtonCaption");
            }
        }

        private GridLength cancelGridLength = new GridLength(1, GridUnitType.Star);
        public GridLength CancelGridLength
        {
            get => cancelGridLength;
            set
            {
                cancelGridLength = value;
                OnPropertyChanged("CancelGridLength");
            }
        }

        #endregion

        #region Command

        private ICommand yesButtonClick;
        public ICommand YesButtonClick => yesButtonClick ?? (yesButtonClick = new RelayCommand(YesButtonAction));

        private void YesButtonAction()
        {
            Close(true);
        }

        private ICommand noButtonClick;
        public ICommand NoButtonClick => noButtonClick ?? (noButtonClick = new RelayCommand(NoButtonAction));

        private void NoButtonAction()
        {
            Close(false);
        }

        #endregion

        public MessageWindow(string _title, string _message, MessageBoxButton type = MessageBoxButton.OK)
        {
            InitializeComponent();
            DataContext = this;

            MessageTitle = _title;
            Message = _message;

            switch (type)
            {
                case MessageBoxButton.OK:
                    YesButtonCaption = TranslationHelper.Instance.Translate("OK");
                    CancelGridLength = new GridLength(0, GridUnitType.Star);
                    break;
                case MessageBoxButton.OKCancel:
                    YesButtonCaption = TranslationHelper.Instance.Translate("OK");
                    NoButtonCaption = TranslationHelper.Instance.Translate("Cancel");
                    CancelGridLength = new GridLength(1, GridUnitType.Star);
                    break;
                case MessageBoxButton.YesNo:
                    YesButtonCaption = TranslationHelper.Instance.Translate("Yes");
                    NoButtonCaption = TranslationHelper.Instance.Translate("No");
                    CancelGridLength = new GridLength(1, GridUnitType.Star);
                    break;
            }
        }
    }
}
