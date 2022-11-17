using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class ModelCategorySettingWindowViewModel : Observable
    {
        #region 생성자
        public ModelCategorySettingWindowViewModel()
        {
            AddCategoryCommand = new RelayCommand(AddCategoryCommandAction);
            DeleteCategoryCommand = new RelayCommand(DeleteCategoryCommandAction);
            OkCommand = new RelayCommand<ChildWindow>(OkCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            var systemConfig = SystemConfig.Instance as SystemConfig;
            if (systemConfig.ModelCategoryList != null)
            {
                ModelCategoryList = new ObservableCollection<ObservableValue<string>>();
                foreach (string modelCategory in systemConfig.ModelCategoryList)
                {
                    ModelCategoryList.Add(new ObservableValue<string>(modelCategory));
                }
            }
        }
        #endregion


        #region 속성
        public ICommand AddCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<ObservableValue<string>> ModelCategoryList { get; set; } = new ObservableCollection<ObservableValue<string>>();

        private ObservableValue<string> selectedCategory;
        public ObservableValue<string> SelectedCategory
        {
            get => selectedCategory;
            set => Set(ref selectedCategory, value);
        }
        #endregion


        #region 메서드
        public void AddCategoryCommandAction()
        {
            ModelCategoryList.Add(new ObservableValue<string>("New Category"));
        }

        public void DeleteCategoryCommandAction()
        {
            ModelCategoryList.Remove(SelectedCategory);
        }

        public void OkCommandAction(ChildWindow wnd)
        {
            var systemConfig = SystemConfig.Instance as SystemConfig;

            systemConfig.ModelCategoryList.Clear();
            foreach (ObservableValue<string> category in ModelCategoryList)
            {
                systemConfig.ModelCategoryList.Add(category.Value);
            }

            SystemConfig.Instance.Save();

            wnd.Close();
        }

        public void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close();
        }
        #endregion        
    }
}
