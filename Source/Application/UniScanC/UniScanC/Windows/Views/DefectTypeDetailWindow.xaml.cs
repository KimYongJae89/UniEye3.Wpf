using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Controls.ViewModels;
using UniScanC.Controls.Views;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Windows.Views
{
    /// <summary>
    /// DefectTypeDetailView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DefectTypeDetailWindow : INotifyPropertyChanged
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

        private DefectDetailVerticalControlView defectDetailControl;
        public DefectDetailVerticalControlView DefectDetailControl
        {
            get => defectDetailControl;
            set => Set(ref defectDetailControl, value);
        }

        private DefectCategory defectCategory;
        public DefectCategory DefectCategory
        {
            get => defectCategory;
            set => Set(ref defectCategory, value);
        }

        private List<Defect> defects;
        public List<Defect> Defects
        {
            get => defects;
            set => Set(ref defects, value);
        }

        private Defect selectedDefect;
        public Defect SelectedDefect
        {
            get => selectedDefect;
            set
            {
                Set(ref selectedDefect, value);
                if (DefectDetailControl.DataContext is DefectDetailVerticalControlViewModel viewModel)
                {
                    viewModel.OnUpdateSelectedDefect(SelectedDefect);
                }
            }
        }

        private EDefectSortType selectedDefectSortType = EDefectSortType.Defect_No;
        public EDefectSortType SelectedDefectSortType
        {
            get => selectedDefectSortType;
            set
            {
                if (Set(ref selectedDefectSortType, value))
                {
                    Defects = SortDefects();
                }
            }
        }

        public ICommand VisibleChangedCommand { get; }
        public ICommand CloseCommand { get; }

        public DefectTypeDetailWindow(List<Defect> defects, DefectCategory defectCategory)
        {
            InitializeComponent();

            Defects = defects;
            Defects = SortDefects();

            var viewModel = new DefectDetailVerticalControlViewModel();
            viewModel.DefectCategory = DefectCategory = defectCategory;
            DefectDetailControl = new DefectDetailVerticalControlView();
            DefectDetailControl.DataContext = viewModel;

            VisibleChangedCommand = new RelayCommand(() =>
            {

            });

            CloseCommand = new RelayCommand(() => DialogResult = true);
        }

        private List<Defect> SortDefects()
        {
            switch (SelectedDefectSortType)
            {
                case EDefectSortType.Camera_No: return Defects?.OrderBy(x => x.ModuleNo).ToList();
                case EDefectSortType.Defect_No: return Defects?.OrderBy(x => x.DefectNo).ToList();
                case EDefectSortType.Width_MM: return Defects?.OrderBy(x => x.BoundingRect.Width).ToList();
                case EDefectSortType.Height_MM: return Defects?.OrderBy(x => x.BoundingRect.Height).ToList();
                case EDefectSortType.Area_MM2: return Defects?.OrderBy(x => x.Area).ToList();
                case EDefectSortType.PosX_MM: return Defects?.OrderBy(x => x.DefectPos.X).ToList();
                case EDefectSortType.PosY_MM: return Defects?.OrderBy(x => x.DefectPos.X).ToList();
                case EDefectSortType.Min_Gv: return Defects?.OrderBy(x => x.MinGv).ToList();
                case EDefectSortType.Max_Gv: return Defects?.OrderBy(x => x.MaxGv).ToList();
                case EDefectSortType.Avg_Gv: return Defects?.OrderBy(x => x.AvgGv).ToList();
            }
            return Defects;
        }
    }
}
