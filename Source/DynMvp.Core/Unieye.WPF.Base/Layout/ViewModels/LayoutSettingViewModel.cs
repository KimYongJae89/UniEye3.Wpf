using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Override;
using UniEye.Translation.Helpers;
using WPFUnieye.WPF.Base.Layout.Views;

namespace Unieye.WPF.Base.Layout.ViewModels
{
    public class LayoutSettingViewModel : Observable
    {
        public ObservableRangeCollection<Type> UserControlList { get; set; } = new ObservableRangeCollection<Type>();

        public LayoutHandler LayoutHandler { get; set; } = new LayoutHandler();

        private bool isRemoveMode;
        public bool IsRemoveMode
        {
            get => isRemoveMode;
            set => Set(ref isRemoveMode, value);
        }

        private ObservableCollection<LayoutSettingModel> layoutSettingModels = new ObservableCollection<LayoutSettingModel>();
        public ObservableCollection<LayoutSettingModel> LayoutSettingModels
        {
            get => layoutSettingModels;
            set => Set(ref layoutSettingModels, value);
        }

        private LayoutSettingModel selectedLayoutSettingModel;
        public LayoutSettingModel SelectedLayoutSettingModel
        {
            get => selectedLayoutSettingModel;
            set
            {
                if (Set(ref selectedLayoutSettingModel, value) && value != null)
                {
                    UpdateLayoutGrid(value);
                }
            }
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand AddControlCommand { get; }

        public ICommand AddPageCommand { get; }

        public ICommand RemovePageCommand { get; }

        public ICommand CopyPageCommand { get; }

        public ICommand MoveLeftCommand { get; }

        public ICommand MoveRightCommand { get; }

        public ICommand LayoutButtonCommand { get; }

        private bool IsUpdate { get; set; } = false;

        public LayoutSettingViewModel(LayoutHandler layoutHandler, List<Type> userControlList)
        {
            LayoutHandler = layoutHandler.Clone();
            UserControlList.AddRange(userControlList);

            OkCommand = new RelayCommand<ChildWindow>(OkCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);
            AddPageCommand = new RelayCommand(AddPageCommandAction);
            RemovePageCommand = new RelayCommand(RemovePageCommandAction);
            CopyPageCommand = new RelayCommand(CopyPageCommandAction);
            MoveLeftCommand = new RelayCommand(MoveLeftCommandAction);
            MoveRightCommand = new RelayCommand(MoveRightCommandAction);
            AddControlCommand = new RelayCommand<Type>(AddControlCommandAction);

            LayoutButtonCommand = new RelayCommand<LayoutModel>(LayoutButtonCommandAction);

            IsUpdate = true;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var newLayoutSettingModel = new LayoutSettingModel();
                newLayoutSettingModel.LayoutPageHandler = layoutPageHandler;
                layoutPageHandler.PropertyChanged += ModelListPropertyChanged;
                UpdateLayoutGrid(newLayoutSettingModel);

                LayoutSettingModels.Add(newLayoutSettingModel);
            }
            IsUpdate = false;
        }

        private void OkCommandAction(ChildWindow wnd)
        {
            ToLayoutHandler();
            wnd.Close(true);
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(false);
        }

        private void AddPageCommandAction()
        {
            var newLayoutSettingModel = new LayoutSettingModel();
            newLayoutSettingModel.LayoutPageHandler.PropertyChanged += ModelListPropertyChanged;
            LayoutSettingModels.Add(newLayoutSettingModel);
        }

        private void RemovePageCommandAction()
        {
            SelectedLayoutSettingModel.LayoutPageHandler.PropertyChanged -= ModelListPropertyChanged;
            LayoutSettingModels.Remove(SelectedLayoutSettingModel);
        }

        private void CopyPageCommandAction()
        {
            LayoutSettingModel newLayoutSettingModel = selectedLayoutSettingModel.Clone();
            newLayoutSettingModel.LayoutPageHandler.PropertyChanged += ModelListPropertyChanged;
            LayoutSettingModels.Add(newLayoutSettingModel);
        }

        private void MoveLeftCommandAction()
        {
            int index = LayoutSettingModels.IndexOf(SelectedLayoutSettingModel);
            if (index >= 1)
            {
                LayoutSettingModels.Move(index, index - 1);
            }
        }

        private void MoveRightCommandAction()
        {
            int index = LayoutSettingModels.IndexOf(SelectedLayoutSettingModel);
            if (index < LayoutSettingModels.Count - 1)
            {
                LayoutSettingModels.Move(index, index + 1);
            }
        }

        private async void AddControlCommandAction(Type type)
        {
            var layoutModel = new LayoutModel(type);

            if (await SetControl(layoutModel))
            {
                SelectedLayoutSettingModel.LayoutPageHandler.Add(layoutModel);
                UpdateLayoutGrid(SelectedLayoutSettingModel);
            }
        }

        private async void LayoutButtonCommandAction(LayoutModel layoutModel)
        {
            if (IsRemoveMode)
            {
                SelectedLayoutSettingModel.LayoutPageHandler.Remove(layoutModel);
                IsRemoveMode = false;
            }
            else
            {
                await SetControl(layoutModel);
            }

            UpdateLayoutGrid(SelectedLayoutSettingModel);
        }

        private async Task<bool> SetControl(LayoutModel layoutModel)
        {
            var view = new LayoutOptionView();
            view.DataContext = new LayoutOptionViewModel(layoutModel);
            if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
            {
                var viewModel = view.DataContext as LayoutOptionViewModel;
                layoutModel.Row = viewModel.Row;
                layoutModel.RowSpan = viewModel.RowSpan;
                layoutModel.Column = viewModel.Column;
                layoutModel.ColumnSpan = viewModel.ColumnSpan;

                foreach (WTuple<System.Reflection.PropertyInfo, object> tuple in viewModel.PropertyList)
                {
                    layoutModel.ControlViewModel.Set(tuple.Item1, tuple.Item2);
                }

                layoutModel.DownloadProperty();

                return true;
            }

            return false;
        }

        private void ModelPropertyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsUpdate == true)
            {
                return;
            }

            IsUpdate = true;
            UpdateLayoutGrid(SelectedLayoutSettingModel);
            IsUpdate = false;
        }

        private void ModelListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsUpdate == true)
            {
                return;
            }

            IsUpdate = true;
            UpdateLayoutGrid(SelectedLayoutSettingModel);
            IsUpdate = false;
        }

        private void UpdateLayoutGrid(LayoutSettingModel layoutSettingModel)
        {
            LayoutPageHandler layoutPageHandler = layoutSettingModel.LayoutPageHandler;
            if (layoutPageHandler == null)
            {
                return;
            }

            var grid = new Grid();
            grid.ShowGridLines = true;

            int columns = layoutPageHandler.Columns;
            int rows = layoutPageHandler.Rows;

            for (int col = 0; col < columns; col++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            if (layoutSettingModel.LayoutGrid != null)
            {
                layoutSettingModel.LayoutGrid = new Grid();
            }

            foreach (LayoutModel layoutModel in layoutPageHandler.ModelList)
            {
                string layoutName = layoutModel.ControlViewModel.GetType().Name;

                var userControl = new Button();
                userControl.Content = TranslationHelper.Instance.Translate(layoutName.Substring(0, layoutName.Length - 9));
                userControl.Command = LayoutButtonCommand;
                userControl.CommandParameter = layoutModel;

                Grid.SetRow(userControl, layoutModel.Row - 1);
                Grid.SetRowSpan(userControl, layoutModel.RowSpan);
                Grid.SetColumn(userControl, layoutModel.Column - 1);
                Grid.SetColumnSpan(userControl, layoutModel.ColumnSpan);

                grid.Children.Add(userControl);
            }

            layoutSettingModel.LayoutGrid = grid;
        }

        private void ToLayoutHandler()
        {
            var newLayoutHandler = new LayoutHandler();
            foreach (LayoutPageHandler layoutPageHandler in LayoutSettingModels.Select(x => x.LayoutPageHandler))
            {
                newLayoutHandler.Add(layoutPageHandler);
            }
            LayoutHandler = newLayoutHandler;
        }
    }

    public class LayoutSettingModel : Observable
    {
        private LayoutPageHandler layoutPageHandler = new LayoutPageHandler();
        public LayoutPageHandler LayoutPageHandler
        {
            get => layoutPageHandler;
            set => Set(ref layoutPageHandler, value);
        }

        private Grid layoutGrid;
        public Grid LayoutGrid
        {
            get => layoutGrid;
            set => Set(ref layoutGrid, value);
        }

        public LayoutSettingModel Clone()
        {
            var newLayoutSettingModel = new LayoutSettingModel();
            newLayoutSettingModel.CopyFrom(this);
            return newLayoutSettingModel;
        }

        public void CopyFrom(LayoutSettingModel layoutSettingModel)
        {
            LayoutPageHandler = layoutSettingModel.LayoutPageHandler.Clone();
        }
    }
}
