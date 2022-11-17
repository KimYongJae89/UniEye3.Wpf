using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Windows.Views
{
    public partial class CategoryTypeEditWindow : MetroWindow, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty CategoryTypeProperty =
            DependencyProperty.Register("CategoryType", typeof(CategoryType), typeof(CategoryTypeEditWindow),
                new FrameworkPropertyMetadata(OnCategoryTypeChanged));

        private static void OnCategoryTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var local = d as CategoryTypeEditWindow;
            local.Update(e.NewValue as CategoryType);
        }

        public CategoryType CategoryType
        {
            get => (CategoryType)GetValue(CategoryTypeProperty);
            set => SetValue(CategoryTypeProperty, value);
        }

        #endregion

        private object categoryData;
        public object CategoryData
        {
            get => categoryData;
            set => Set(ref categoryData, value);
        }

        public ICommand ApplyCommand { get; }
        public ICommand CancelCommand { get; }

        public CategoryTypeEditWindow()
        {
            InitializeComponent();

            ApplyCommand = new RelayCommand(() =>
            {
                CategoryType.Data = RevertData(Convert.ToDouble(CategoryData));
                DialogResult = true;
            });

            CancelCommand = new RelayCommand(() => DialogResult = false);
        }

        private void Update(CategoryType categoryType)
        {
            if (categoryType == null)
            {
                CategoryData = 0;
                return;
            }

            double data = System.Convert.ToDouble(categoryType.Data);

            switch (categoryType.Type)
            {
                case ECategoryTypeName.EdgeLower:
                case ECategoryTypeName.EdgeUpper:
                case ECategoryTypeName.WidthLower:
                case ECategoryTypeName.WidthUpper:
                case ECategoryTypeName.HeightLower:
                case ECategoryTypeName.HeightUpper: data /= 1000; CategoryData = Convert.ToDouble(string.Format("{0:0.00}", data)); break;

                case ECategoryTypeName.AreaLower:
                case ECategoryTypeName.AreaUpper: data /= 1000000; CategoryData = Convert.ToDouble(string.Format("{0:0.0000}", data)); break;

                case ECategoryTypeName.MinGvLower:
                case ECategoryTypeName.MinGvUpper:
                case ECategoryTypeName.MaxGvLower:
                case ECategoryTypeName.MaxGvUpper:
                case ECategoryTypeName.AvgGvLower:
                case ECategoryTypeName.AvgGvUpper: break;
            }

            CategoryData = Convert.ToDouble(string.Format("{0:0.0}", data));
        }

        private double RevertData(double data)
        {
            switch (CategoryType.Type)
            {
                case ECategoryTypeName.EdgeLower:
                case ECategoryTypeName.EdgeUpper:
                case ECategoryTypeName.WidthLower:
                case ECategoryTypeName.WidthUpper:
                case ECategoryTypeName.HeightLower:
                case ECategoryTypeName.HeightUpper: data *= 1000; break;

                case ECategoryTypeName.AreaLower:
                case ECategoryTypeName.AreaUpper: data *= 1000000; break;

                case ECategoryTypeName.MinGvLower:
                case ECategoryTypeName.MinGvUpper:
                case ECategoryTypeName.MaxGvLower:
                case ECategoryTypeName.MaxGvUpper:
                case ECategoryTypeName.AvgGvLower:
                case ECategoryTypeName.AvgGvUpper: break;
            }

            return data;
        }
    }
}
