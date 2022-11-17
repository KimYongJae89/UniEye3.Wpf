using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Unieye.WPF.Base.Events;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Override;

namespace Unieye.WPF.Base.Layout.ViewModels
{
    public class LayoutViewModel : Observable
    {
        private ObservableRangeCollection<LayoutPageViewModel> layoutPageList = new ObservableRangeCollection<LayoutPageViewModel>();
        public ObservableRangeCollection<LayoutPageViewModel> LayoutPageList
        {
            get => layoutPageList;
            set => Set(ref layoutPageList, value);
        }

        private LayoutPageViewModel selectedPage;
        public LayoutPageViewModel SelectedPage
        {
            get => selectedPage;
            set => Set(ref selectedPage, value);
        }

        public LayoutHandler LayoutHandler { get; set; }

        public LayoutViewModel(LayoutHandler layoutHandler)
        {
            LayoutHandler = layoutHandler;
            UpdateLayout(layoutHandler);
        }

        private void UpdateLayout(LayoutHandler layoutHandler)
        {
            LayoutPageList.Clear();
            foreach (Grid grid in LayoutHandler.ToLayoutGrid())
            {
                var newLayoutPageViewModel = new LayoutPageViewModel();
                newLayoutPageViewModel.PageName = grid.Tag.ToString();
                newLayoutPageViewModel.LayoutGrid = grid;
                LayoutPageList.Add(newLayoutPageViewModel);
            }
            SelectedPage = LayoutPageList.FirstOrDefault();
        }
    }

    public class LayoutPageViewModel : Observable
    {
        private string pageName = "Layout";
        public string PageName
        {
            get => pageName;
            set => Set(ref pageName, value);
        }

        private Grid layoutGrid;
        public Grid LayoutGrid
        {
            get => layoutGrid;
            set => Set(ref layoutGrid, value);
        }
    }
}
