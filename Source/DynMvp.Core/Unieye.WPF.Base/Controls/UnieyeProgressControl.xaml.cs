using DynMvp.UI;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;

namespace Unieye.WPF.Base.Controls
{
    public enum ProgressMode
    {
        Percent,
        Text
    }

    public class ProgressSource : Observable
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }


        private ProgressMode progressMode = ProgressMode.Percent;
        public ProgressMode ProgressMode
        {
            get => progressMode;
            set => Set(ref progressMode, value);
        }


        public int Step { get; set; } = 1;
        public int Range { get; set; } = 0;

        private int pos = 0;
        public int Pos
        {
            get => pos;
            set => Set(ref pos, value);
        }

        private string text;
        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        private bool isShowCancelButton = false;
        public bool IsShowCancelButton
        {
            get => isShowCancelButton;
            set => Set(ref isShowCancelButton, value);
        }

        public void StepIt()
        {
            Pos += Step;
            if (Pos >= Range)
            {
                Pos = Range;
            }
        }
    }

    /// <summary>
    /// UnieyeProgressControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UnieyeProgressControl : ChildWindow, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        #endregion

        public delegate bool ProgressActionDelegate(IReportProgress report);
        public ProgressActionDelegate ProgressAction;

        private string titleText;
        public string TitleText
        {
            get => titleText;
            set => Set(ref titleText, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }

        private string percentText;
        public string PercentText
        {
            get => percentText;
            set => Set(ref percentText, value);
        }

        private bool isComplete = false;
        public bool IsComplete
        {
            get => isComplete;
            set => Set(ref isComplete, value);
        }

        private bool isCanceled = false;
        public bool IsCanceled
        {
            get => isCanceled;
            set => Set(ref isCanceled, value);
        }

        private bool isShowProgressRing = false;
        public bool IsShowProgressRing
        {
            get => isShowProgressRing;
            set => Set(ref isShowProgressRing, value);
        }

        private bool isShowCompleteImage = false;
        public bool IsShowCompleteImage
        {
            get => isShowCompleteImage;
            set => Set(ref isShowCompleteImage, value);
        }

        private bool isShowCancelButton = false;
        public bool IsShowCancelButton
        {
            get => isShowCancelButton;
            set => Set(ref isShowCancelButton, value);
        }

        private bool isShowCloseButton = false;
        public bool IsShowCloseButton
        {
            get => isShowCloseButton;
            set => Set(ref isShowCloseButton, value);
        }

        private object progressContent;
        public object ProgressContent
        {
            get => progressContent;
            set => Set(ref progressContent, value);
        }

        private ProgressSource progressSource;
        public ProgressSource ProgressSource
        {
            get => progressSource;
            set => Set(ref progressSource, value);
        }

        private bool autoCloseMode;
        public bool AutoCloseMode
        {
            get => autoCloseMode;
            set => Set(ref autoCloseMode, value);
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand => (cancelCommand ?? (cancelCommand = new RelayCommand(CancelAction)));

        private void CancelAction()
        {
            progressSource?.CancellationTokenSource?.Cancel();
        }

        private ICommand closeCommand;
        public ICommand CloseCommand => (closeCommand ?? (closeCommand = new RelayCommand(CloseAction)));

        private void CloseAction()
        {
            Close();
        }

        private DispatcherTimer timer = new DispatcherTimer();
        public UnieyeProgressControl()
        {
            Initialize();
        }

        private int currentActionNo;
        private int maxActionCount;
        private Task actionTask;
        private ConcurrentQueue<Action> actionQueue = new ConcurrentQueue<Action>();

        public UnieyeProgressControl(string title, string _description, Action action, bool autoCloseMode = false, ProgressSource source = null)
        {
            Initialize();

            TitleText = title;
            Description = _description;

            currentActionNo = 0;
            maxActionCount = 1;

            actionQueue.Enqueue(action);

            AutoCloseMode = autoCloseMode;

            ProgressSource = source;
        }

        public UnieyeProgressControl(string title, string _description, List<Action> actionList, bool autoCloseMode = false, ProgressSource source = null)
        {
            Initialize();

            TitleText = title;
            Description = _description;

            currentActionNo = 0;
            maxActionCount = actionList.Count;

            foreach (Action action in actionList)
            {
                actionQueue.Enqueue(action);
            }

            AutoCloseMode = autoCloseMode;

            ProgressSource = source;
        }

        private void Initialize()
        {
            InitializeComponent();

            DataContext = this;
            IsComplete = false;

            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += ProgressTimer;
            timer.Start();
        }

        private void ProgressTimer(object sender, EventArgs e)
        {
            bool isExistSource = ProgressSource != null;
            if (isExistSource && ProgressSource.CancellationTokenSource != null && ProgressSource.CancellationTokenSource.IsCancellationRequested)
            {
                PercentText = TranslationHelper.Instance.Translate("Cancel");
                IsCanceled = true;
                timer.Stop();
            }
            else
            {
                string header = TranslationHelper.Instance.Translate("Running");

                if (actionTask == null)
                {
                    if (actionQueue.TryDequeue(out Action action))
                    {
                        actionTask = Task.Run(action);
                    }
                }
                else if (actionTask.IsCompleted)
                {
                    currentActionNo++;
                    actionTask = null;
                }

                if (currentActionNo == maxActionCount)
                {
                    timer.Stop();
                    header = TranslationHelper.Instance.Translate("Complete");
                    IsComplete = true;

                    if (autoCloseMode == true)
                    {
                        Close();
                    }
                }

                if (!isExistSource || (ProgressSource.ProgressMode == ProgressMode.Percent && ProgressSource.Range == 0))
                {
                    PercentText = string.Format("{0} ... {1:0} %", header, Convert.ToDouble(currentActionNo / (double)maxActionCount) * 100);
                }
                else
                {
                    if (ProgressSource.ProgressMode == ProgressMode.Percent)
                    {
                        PercentText = string.Format("{0} ... {1:0} %", header, Convert.ToDouble(ProgressSource.Pos / (double)ProgressSource.Range) * 100);
                    }
                    else
                    {
                        PercentText = ProgressSource.Text;
                    }
                }
            }

            IsShowProgressRing = !IsComplete && !IsCanceled;
            IsShowCompleteImage = IsComplete && !IsCanceled;
            IsShowCloseButton = IsComplete || IsCanceled;
            IsShowCancelButton = isExistSource && ProgressSource.CancellationTokenSource != null && !IsShowCloseButton;
        }
    }
}
