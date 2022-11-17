using DynMvp.Data;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Models;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class ModelInfoWindowViewModel : Observable
    {
        #region 생성자
        public ModelInfoWindowViewModel(List<string> modelCategoryList, List<DynMvp.Data.ModelDescription> modelDescriptionList, DynMvp.Data.ModelDescription tempModelDescription = null)
        {
            AcceptCommand = new RelayCommand<ChildWindow>(AcceptCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            var uniScanCModelDescriptionList = modelDescriptionList.FindAll(x => x is UniScanC.Models.ModelDescription).Cast<UniScanC.Models.ModelDescription>().ToList();
            SortedCategoryDic.Clear();
            foreach (string modelCategory in modelCategoryList)
            {
                SortedCategoryDic.Add(modelCategory, new CategorySet());
                var sortedList = new List<string>(uniScanCModelDescriptionList.Select(x => x.CategoryDic.ContainsKey(modelCategory) ? x.CategoryDic[modelCategory] : null));
                sortedList.RemoveAll(x => x == "");
                sortedList = sortedList.Distinct().ToList();
                sortedList.Sort();
                var categorySet = new CategorySet() { CategoryList = sortedList, SelectedCategory = "" };
                SortedCategoryDic[modelCategory] = categorySet;
            }

            if (tempModelDescription is UniScanC.Models.ModelDescription modelDescription)
            {
                Name = modelDescription.Name;
                foreach (KeyValuePair<string, string> categoryPair in modelDescription.CategoryDic)
                {
                    if (SortedCategoryDic.ContainsKey(categoryPair.Key))
                    {
                        SortedCategoryDic[categoryPair.Key].SelectedCategory = categoryPair.Value;
                    }
                }
                Description = modelDescription.Description;
            }
        }
        #endregion


        #region 속성
        public System.Windows.Input.ICommand CancelCommand { get; }
        public System.Windows.Input.ICommand AcceptCommand { get; }

        public string name = string.Empty;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private Dictionary<string, CategorySet> sortedCategoryDic = new Dictionary<string, CategorySet>();
        public Dictionary<string, CategorySet> SortedCategoryDic
        {
            get => sortedCategoryDic;
            set => Set(ref sortedCategoryDic, value);
        }

        public string description = string.Empty;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }
        #endregion


        #region 메서드
        private async void AcceptCommandAction(ChildWindow wnd)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                string header = TranslationHelper.Instance.Translate("Error");
                string message = TranslationHelper.Instance.Translate("MODELWINDOW_ERROR");
                await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);

                return;
            }

            wnd.Close(true);
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(false);
        }
        #endregion
    }
}
