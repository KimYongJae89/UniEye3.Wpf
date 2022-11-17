using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Module;
using WPF.UniScanCM.Controls.ViewModels;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Service;

namespace WPF.UniScanCM.Pages.ViewModels
{
    public class TeachingPageViewModel : Observable
    {
        private ModelParameterViewModel modelParameterViewModel = new ModelParameterViewModel();
        public ModelParameterViewModel ModelParameterViewModel
        {
            get => modelParameterViewModel;
            set => Set(ref modelParameterViewModel, value);
        }

        private ModelDefectCategoryControlViewModel modelDefectCategoryControlViewModel = new ModelDefectCategoryControlViewModel();
        public ModelDefectCategoryControlViewModel ModelDefectCategoryControlViewModel
        {
            get => modelDefectCategoryControlViewModel;
            set => Set(ref modelDefectCategoryControlViewModel, value);
        }

        public TeachingPageViewModel()
        {
            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;
        }

        private void ModelOpened(ModelBase model)
        {
            ModelParameterViewModel.SetModel(model);
            ModelDefectCategoryControlViewModel.SetModel(model);
        }

        private void ModelClosed()
        {
            ModelParameterViewModel.ClearModel();
        }
    }
}
