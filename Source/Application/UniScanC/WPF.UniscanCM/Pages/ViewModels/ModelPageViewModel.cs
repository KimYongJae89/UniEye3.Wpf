using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Models;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Windows.Models;
using WPF.UniScanCM.Windows.ViewModels;
using WPF.UniScanCM.Windows.Views;
using ModelManager = UniScanC.Models.ModelManager;

namespace WPF.UniScanCM.Pages.ViewModels
{
    public class ModelPageViewModel : Observable
    {
        #region 생성자
        public ModelPageViewModel()
        {
            SelectCommand = new RelayCommand(SelectCommandAction);
            EditCommand = new RelayCommand(EditCommandAction);
            AddCommand = new RelayCommand(AddCommandAction);
            RemoveCommand = new RelayCommand(RemoveCommandAction);
            CopyCommand = new RelayCommand(CopyCommandAction);
            ResetCategoryCommand = new RelayCommand(ResetCategoryCommandAction);

            UserHandler.Instance.OnUserChanged += OnUserChanged;

            UpdateSearchList();
            OnPropertyChanged("Source");
        }
        #endregion


        #region 속성
        public System.Windows.Input.ICommand SelectCommand { get; }

        public System.Windows.Input.ICommand EditCommand { get; }

        public System.Windows.Input.ICommand AddCommand { get; }

        public System.Windows.Input.ICommand RemoveCommand { get; }

        public System.Windows.Input.ICommand CopyCommand { get; }

        public System.Windows.Input.ICommand ResetCategoryCommand { get; }

        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                Set(ref selectedIndex, value);
                if (selectedIndex != -1)
                {
                    SelectedModelDescription = ModelManager.Instance().ModelDescriptionList.Find(x => x.Name == Source.Rows[selectedIndex][0].ToString());
                }
            }
        }

        private DynMvp.Data.ModelDescription selectedModelDescription;
        public DynMvp.Data.ModelDescription SelectedModelDescription
        {
            get => selectedModelDescription;
            set => Set(ref selectedModelDescription, value);
        }

        private Dictionary<string, CategorySet> sortedCategoryDic = new Dictionary<string, CategorySet>();
        public Dictionary<string, CategorySet> SortedCategoryDic
        {
            get => sortedCategoryDic;
            set => Set(ref sortedCategoryDic, value);
        }

        private string searchText = string.Empty;
        public string SearchText
        {
            get => searchText;
            set
            {
                Set(ref searchText, value);
                OnPropertyChanged("Source");
            }
        }

        private bool isAuthorized = false;
        public bool IsAuthorized
        {
            get => isAuthorized;
            set => Set(ref isAuthorized, value);
        }

        public DataTable Source => UpdateDataGrid();
        #endregion


        #region 메서드
        private void SelectCommandAction()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            LogHelper.Info(LoggerType.Operation, $"[Model Page] Select Model [{SelectedModelDescription.Name}]");
            ModelManager.Instance().OpenModel(SelectedModelDescription, null);
        }

        private async void EditCommandAction()
        {
            var modelMgr = ModelManager.Instance();
            var modelInfoWindowViewModel = new ModelInfoWindowViewModel(SystemConfig.Instance.ModelCategoryList, modelMgr.ModelDescriptionList, SelectedModelDescription);
            var modelInfoWindowView = new ModelInfoWindowView() { DataContext = modelInfoWindowViewModel };

            LogHelper.Info(LoggerType.Operation, $"[Model Page] Edit Model [{SelectedModelDescription.Name}]");
            if (await Application.Current.MainWindow.ShowChildWindowAsync<bool>(modelInfoWindowView, ChildWindowManager.OverlayFillBehavior.FullWindow))
            {
                if (!modelMgr.ModelDescriptionList.Any(m => m.Name == modelInfoWindowViewModel.Name))
                {
                    var desc = SelectedModelDescription.Clone() as UniScanC.Models.ModelDescription;
                    desc.Name = modelInfoWindowViewModel.Name;
                    desc.ModifiedDate = DateTime.Now;
                    foreach (KeyValuePair<string, CategorySet> categoryPair in modelInfoWindowViewModel.SortedCategoryDic)
                    {
                        if (desc.CategoryDic.ContainsKey(categoryPair.Key))
                        {
                            desc.CategoryDic[categoryPair.Key] = categoryPair.Value.SelectedCategory;
                        }
                        else
                        {
                            desc.CategoryDic.Add(categoryPair.Key, categoryPair.Value.SelectedCategory);
                        }
                    }
                    desc.Description = modelInfoWindowViewModel.Description;

                    modelMgr.AddModel(desc);

                    ModelBase model = modelMgr.LoadModel(SelectedModelDescription);
                    model.ModelDescription = desc.Clone();
                    var directoryInfo = new DirectoryInfo(model.ModelPath);
                    model.ModelPath = Path.Combine(directoryInfo.Parent.FullName, model.Name);
                    model.SaveModel();

                    modelMgr.DeleteModel(SelectedModelDescription.Name);
                    LogHelper.Info(LoggerType.Operation, $"[Model Page] Edit Complete [{desc.Name}]");
                }
                else
                {
                    var desc = SelectedModelDescription as UniScanC.Models.ModelDescription;
                    desc.ModifiedDate = DateTime.Now;
                    foreach (KeyValuePair<string, CategorySet> categoryPair in modelInfoWindowViewModel.SortedCategoryDic)
                    {
                        if (desc.CategoryDic.ContainsKey(categoryPair.Key))
                        {
                            desc.CategoryDic[categoryPair.Key] = categoryPair.Value.SelectedCategory;
                        }
                        else
                        {
                            desc.CategoryDic.Add(categoryPair.Key, categoryPair.Value.SelectedCategory);
                        }
                    }
                    desc.Description = modelInfoWindowViewModel.Description;

                    modelMgr.EditModel(SelectedModelDescription);
                    LogHelper.Info(LoggerType.Operation, $"[Model Page] Edit Complete [{SelectedModelDescription.Name}]");
                }
                UpdateSearchList();
                OnPropertyChanged("Source");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Model Page] Edit Cancel [{SelectedModelDescription.Name}]");
            }
        }

        private async void AddCommandAction()
        {
            var modelMgr = ModelManager.Instance();
            var modelInfoWindowViewModel = new ModelInfoWindowViewModel(SystemConfig.Instance.ModelCategoryList, modelMgr.ModelDescriptionList);
            var modelInfoWindowView = new ModelInfoWindowView() { DataContext = modelInfoWindowViewModel };

            LogHelper.Info(LoggerType.Operation, $"[Model Page] Add Model");
            if (await Application.Current.MainWindow.ShowChildWindowAsync<bool>(modelInfoWindowView, ChildWindowManager.OverlayFillBehavior.FullWindow))
            {
                if (!modelMgr.ModelDescriptionList.Any(m => m.Name == modelInfoWindowViewModel.Name))
                {
                    var desc = modelMgr.CreateModelDescription() as UniScanC.Models.ModelDescription;
                    desc.Name = modelInfoWindowViewModel.Name;
                    desc.CreatedDate = DateTime.Now;
                    desc.ModifiedDate = DateTime.Now;
                    foreach (KeyValuePair<string, CategorySet> categoryPair in modelInfoWindowViewModel.SortedCategoryDic)
                    {
                        desc.CategoryDic.Add(categoryPair.Key, categoryPair.Value.SelectedCategory);
                    }
                    desc.Description = modelInfoWindowViewModel.Description;

                    modelMgr.AddModel(desc);

                    LogHelper.Info(LoggerType.Operation, $"[Model Page] Add Model Complete [{desc.Name}]");
                    UpdateSearchList();
                    OnPropertyChanged("Source");
                }
                else
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    string message = TranslationHelper.Instance.Translate("MODEL_OVERLAP_MESSAGE");
                    await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                }
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Model Page] Add Model Cancel");
            }
        }

        private async void RemoveCommandAction()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            LogHelper.Info(LoggerType.Operation, $"[Model Page] Remove Model [{SelectedModelDescription.Name}]");
            string header = TranslationHelper.Instance.Translate("Remove");
            string message = TranslationHelper.Instance.Translate("MODEL_DELETE_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                ModelManager.Instance().DeleteModel(SelectedModelDescription.Name);

                LogHelper.Info(LoggerType.Operation, $"[Model Page] Remove Model Complete [{SelectedModelDescription.Name}]");
                UpdateSearchList();
                OnPropertyChanged("Source");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Model Page] Remove Model Cancel [{SelectedModelDescription.Name}]");
            }
        }

        private async void CopyCommandAction()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            var modelMgr = ModelManager.Instance();
            var modelInfoWindowViewModel = new ModelInfoWindowViewModel(SystemConfig.Instance.ModelCategoryList, modelMgr.ModelDescriptionList, SelectedModelDescription);
            var modelInfoWindowView = new ModelInfoWindowView() { DataContext = modelInfoWindowViewModel };

            LogHelper.Info(LoggerType.Operation, $"[Model Page] Copy Model [{SelectedModelDescription.Name}]");
            bool result = await Application.Current.MainWindow.ShowChildWindowAsync<bool>(modelInfoWindowView, ChildWindowManager.OverlayFillBehavior.FullWindow);
            if (result)
            {
                if (modelMgr.ModelDescriptionList.Any(m => m.Name.ToUpper() == modelInfoWindowViewModel.Name.ToUpper()) == false)
                {
                    var desc = SelectedModelDescription.Clone() as UniScanC.Models.ModelDescription;
                    desc.Name = modelInfoWindowViewModel.Name;
                    desc.CreatedDate = DateTime.Now;
                    desc.ModifiedDate = DateTime.Now;
                    foreach (KeyValuePair<string, CategorySet> categoryPair in modelInfoWindowViewModel.SortedCategoryDic)
                    {
                        if (desc.CategoryDic.ContainsKey(categoryPair.Key))
                        {
                            desc.CategoryDic[categoryPair.Key] = categoryPair.Value.SelectedCategory;
                        }
                        else
                        {
                            desc.CategoryDic.Add(categoryPair.Key, categoryPair.Value.SelectedCategory);
                        }
                    }
                    desc.Description = modelInfoWindowViewModel.Description;

                    modelMgr.AddModel(desc);

                    ModelBase model = modelMgr.LoadModel(SelectedModelDescription);
                    model.ModelDescription = desc.Clone();
                    var directoryInfo = new DirectoryInfo(model.ModelPath);
                    model.ModelPath = Path.Combine(directoryInfo.Parent.FullName, model.Name);
                    model.SaveModel();

                    UpdateSearchList();
                    OnPropertyChanged("Source");
                    LogHelper.Info(LoggerType.Operation, $"[Model Page] Copy Model Complete [{desc.Name}]");
                }
                else
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    string message = TranslationHelper.Instance.Translate("MODEL_OVERLAP_MESSAGE");
                    await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                }
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Model Page] Copy Model Cancel [{SelectedModelDescription.Name}]");
            }
        }

        private void ResetCategoryCommandAction()
        {
            selectedIndex = -1;
            foreach (KeyValuePair<string, CategorySet> sortedCategoryPair in SortedCategoryDic)
            {
                sortedCategoryPair.Value.SelectedCategory = "";
            }
        }

        private DataTable UpdateDataGrid()
        {
            List<string> modelCategoryList = SystemConfig.Instance.ModelCategoryList;
            IEnumerable<UniScanC.Models.ModelDescription> modelDescriptionList = ModelManager.Instance().ModelDescriptionList.Cast<UniScanC.Models.ModelDescription>();
            IEnumerable<UniScanC.Models.ModelDescription> sortedList = modelDescriptionList.Where(model => model.Name.ToLower().Contains(SearchText.ToLower()));
            foreach (KeyValuePair<string, CategorySet> sortedCatagoryPair in SortedCategoryDic)
            {
                foreach (UniScanC.Models.ModelDescription modelDesc in sortedList)
                {
                    if (!modelDesc.CategoryDic.ContainsKey(sortedCatagoryPair.Key))
                    {
                        modelDesc.CategoryDic.Add(sortedCatagoryPair.Key, "");
                    }
                }

                if (!string.IsNullOrWhiteSpace(sortedCatagoryPair.Value.SelectedCategory))
                {
                    sortedList = sortedList.Where(model => model.CategoryDic[sortedCatagoryPair.Key].Equals(sortedCatagoryPair.Value.SelectedCategory));
                }
            }

            var table = new DataTable();
            table.Columns.Add(TranslationHelper.Instance.Translate("Name"), typeof(string));
            table.Columns.Add(TranslationHelper.Instance.Translate("Registered_Date"), typeof(string));
            table.Columns.Add(TranslationHelper.Instance.Translate("Modified_Date"), typeof(string));
            foreach (string category in modelCategoryList)
            {
                table.Columns.Add(category, typeof(string));
            }
            //table.Columns.Add(TranslationHelper.Instance.Translate("Description"), typeof(string));

            foreach (UniScanC.Models.ModelDescription modelDesc in sortedList)
            {
                DataRow row = table.NewRow();
                row[TranslationHelper.Instance.Translate("Name")] = modelDesc.Name;
                row[TranslationHelper.Instance.Translate("Registered_Date")] = modelDesc.CreatedDate.ToString("yyyy.MM.dd");
                row[TranslationHelper.Instance.Translate("Modified_Date")] = modelDesc.ModifiedDate.ToString("yyyy.MM.dd");
                foreach (string category in modelCategoryList)
                {
                    if (modelDesc.CategoryDic.ContainsKey(category) == true)
                    {
                        row[category] = modelDesc.CategoryDic[category];
                    }
                }
                //row[TranslationHelper.Instance.Translate("Description")] = modelDesc.Description;

                table.Rows.Add(row);
            }

            return table;
        }

        private void UpdateSearchList()
        {
            var modelMgr = ModelManager.Instance();
            var modelDescriptionList = modelMgr.ModelDescriptionList.FindAll(x => x is UniScanC.Models.ModelDescription).Cast<UniScanC.Models.ModelDescription>().ToList();
            List<string> modelCategoryList = SystemConfig.Instance.ModelCategoryList;

            foreach (KeyValuePair<string, CategorySet> sortedCategoryPair in SortedCategoryDic)
            {
                sortedCategoryPair.Value.PropertyChanged -= CategorySet_PropertyChanged;
            }
            SortedCategoryDic.Clear();

            var tempSortedCategoryDic = new Dictionary<string, CategorySet>();
            foreach (string modelCategory in modelCategoryList)
            {
                var sortedList = new List<string>(modelDescriptionList.Select(x => x.CategoryDic.ContainsKey(modelCategory) ? x.CategoryDic[modelCategory] : ""));
                sortedList.RemoveAll(x => x == "");
                sortedList = sortedList.Distinct().ToList();
                sortedList.Sort();
                sortedList.Insert(0, "");
                var categorySet = new CategorySet() { CategoryList = sortedList, SelectedCategory = "" };
                categorySet.PropertyChanged += CategorySet_PropertyChanged;
                tempSortedCategoryDic.Add(modelCategory, categorySet);
            }
            SortedCategoryDic = tempSortedCategoryDic;
        }

        private void CategorySet_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedCategory")
            {
                OnPropertyChanged("Source");
            }
        }

        private void OnUserChanged(User user)
        {
            if (user.IsAuth(ERoleType.ModelManage))
            {
                IsAuthorized = true;
            }
            else
            {
                IsAuthorized = false;
            }
        }
        #endregion
    }
}
