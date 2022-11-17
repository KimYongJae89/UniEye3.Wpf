using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Windows.Views;

namespace UniScanC.Controls.ViewModels
{
    public class DefectDetailVerticalControlViewModel : CustomizeControlViewModel, INotifyModelChanged, INotifySelectedDefectChanged
    {
        #region 생성자
        public DefectDetailVerticalControlViewModel() : base(typeof(DefectDetailVerticalControlViewModel))
        {
            CategoryTypeEditCommand = new RelayCommand<DefectDetailModel>(CategoryTypeEditCommandAction);
        }

        private void CategoryTypeEditCommandAction(DefectDetailModel model)
        {
            var editControl = new CategoryTypeEditWindow();
            editControl.CategoryType = model.CategoryType.Clone();
            if ((bool)editControl.ShowDialog())
            {
                model.CategoryType.CopyFrom(editControl.CategoryType);
                if (Model != null)
                {
                    foreach (VisionModel visionModel in Model.VisionModels)
                    {
                        DefectCategory findCategory = visionModel.DefectCategories.Find(x => x.Name == model.DefectCategory.Name);
                        if (findCategory != null)
                        {
                            findCategory.CopyFrom(model.DefectCategory);
                            Model.SaveModel();
                            break;
                        }
                    }
                }
                UpdateSelectedDefect(SelectedDefect);
            }
        }
        #endregion


        #region 속성(LayoutControlViewModel)
        #endregion


        #region 속성
        public SelectedDefectUpdateDelegate SelectedDefectUpdate { get; set; }

        private Defect selectedDefect;
        public Defect SelectedDefect
        {
            get => selectedDefect;
            set => Set(ref selectedDefect, value);
        }

        private bool isEditParmeter;
        public bool IsEditParmeter
        {
            get => isEditParmeter;
            set => Set(ref isEditParmeter, value);
        }

        private Model model;
        public Model Model
        {
            get => model;
            set
            {
                if (Set(ref model, value))
                {
                    UpdateSelectedDefect(SelectedDefect);
                }
            }
        }

        private DefectCategory defectCategory;
        public DefectCategory DefectCategory
        {
            get => defectCategory;
            set
            {
                if (Set(ref defectCategory, value))
                {
                    UpdateSelectedDefect(SelectedDefect);
                }
            }
        }

        private string defectType;
        public string DefectType
        {
            get => defectType;
            set => Set(ref defectType, value);
        }

        private IEnumerable<CategoryType> categoryTypeList;
        public IEnumerable<CategoryType> CategoryTypeList
        {
            get => categoryTypeList;
            set => Set(ref categoryTypeList, value);
        }

        private IEnumerable<DefectDetailModel> defectDetailModelList;
        public IEnumerable<DefectDetailModel> DefectDetailModelList
        {
            get => defectDetailModelList;
            set => Set(ref defectDetailModelList, value);
        }

        public ICommand CategoryTypeEditCommand { get; set; }
        #endregion


        #region 메서드
        public void OnUpdateModel(ModelBase modelBase)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateModel(modelBase)));
                return;
            }
            UpdateModel(modelBase);
        }

        public void UpdateModel(ModelBase modelBase)
        {
            Model = modelBase as Model;
        }

        public void OnUpdateSelectedDefect(object selectedDefect)
        {
            System.Windows.Threading.Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnUpdateSelectedDefect(selectedDefect)));
                return;
            }
            UpdateSelectedDefect(selectedDefect as Defect);
        }

        private void UpdateSelectedDefect(Defect defect)
        {
            if (defect == null)
            {
                return;
            }

            SelectedDefect = defect;

            DefectDetailModelList = null;
            DefectCategory defectCategory = null;
            if (Model != null)
            {
                var defectCategories = new List<DefectCategory>();
                foreach (VisionModel visionModel in Model.VisionModels)
                {
                    foreach (DefectCategory category in visionModel.DefectCategories)
                    {
                        if (defectCategories.Find(x => x.Name == category.Name) == null)
                        {
                            defectCategories.Add(new DefectCategory(category));
                        }
                    }
                }

                defectCategories.Add(DefectCategory.GetDefaultCategory());
                defectCategories.Add(DefectCategory.GetColorCategory());

                defectCategory = defectCategories.Find(x => x.Name == defect.DefectTypeName);
            }
            else if (DefectCategory != null)
            {
                defectCategory = DefectCategory;
            }

            DefectDetailModelList = DefectDetailModel.CreateModel(defect, defectCategory, IsEditParmeter);
        }

        public override UserControl CreateControlView()
        {
            return new DefectDetailVerticalControlView();
        }
        #endregion
    }
}
