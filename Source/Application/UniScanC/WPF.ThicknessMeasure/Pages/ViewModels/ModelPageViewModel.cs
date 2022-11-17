using DynMvp.Data;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using WPF.ThicknessMeasure.Override;
using WPF.ThicknessMeasure.Windows.ViewModels;
using WPF.ThicknessMeasure.Windows.Views;

namespace WPF.ThicknessMeasure.Pages.ViewModels
{
    public class ModelPageViewModel : Observable
    {
        #region 필드
        private DataTable source;
        private int selectedIndex;
        private ModelDescription selectedModelDescription;
        private string _searchText = string.Empty;

        private ICommand _selectCommand;
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _editCommand;
        private ICommand _copyCommand;
        #endregion

        #region 생성자
        public ModelPageViewModel() { }
        #endregion

        #region 속성
        public DataTable Source { get => UpdateDataGrid(); set => Set(ref source, value); }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                Set(ref selectedIndex, value);
                if (selectedIndex != -1)
                {
                    SelectedModelDescription = ModelManager.Instance().ModelDescriptionList.Find(x => x.Name == Source.Rows[selectedIndex]["Name"].ToString());
                }
            }
        }

        public ModelDescription SelectedModelDescription { get => selectedModelDescription; set => Set(ref selectedModelDescription, value); }

        public string SearchText
        {
            get => _searchText;
            set
            {
                Set(ref _searchText, value);
                OnPropertyChanged("Source");
            }
        }

        public ICommand SelectCommand => _selectCommand ?? (_selectCommand = new RelayCommand(Select));

        public ICommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(Add));

        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand(Remove));

        public ICommand EditCommand => _editCommand ?? (_editCommand = new RelayCommand(Edit));

        public ICommand CopyCommand => _copyCommand ?? (_copyCommand = new RelayCommand(Copy));
        #endregion

        #region 메서드
        private DataTable UpdateDataGrid()
        {
            List<ModelDescription> modelDescriptions = ModelManager.Instance().ModelDescriptionList.FindAll(model => model.Name.Contains(_searchText));
            List<string> layerNameList = SystemConfig.Instance.SpectrometerProperty.LayerNameList;

            var table = new DataTable();

            //table.Columns.Add("No", typeof(int));
            table.Columns.Add("Name", typeof(string));
            foreach (string layerName in layerNameList)
            {
                table.Columns.Add(layerName, typeof(string));
            }

            table.Columns.Add("Width", typeof(string));
            table.Columns.Add("Sensor", typeof(string));

            int i = 1;
            foreach (Model.ModelDescription modelDesc in modelDescriptions)
            {
                DataRow row = table.NewRow();
                //row["No"] = i;
                row["Name"] = modelDesc.Name;
                foreach (string layerName in layerNameList)
                {
                    if (modelDesc.LayerParamList.ContainsKey(layerName) == true)
                    {
                        row[layerName] = modelDesc.LayerParamList[layerName];
                    }
                }

                row["Width"] = modelDesc.ScanWidth;
                row["Sensor"] = modelDesc.SensorType;

                table.Rows.Add(row);
                i++;
            }

            return table;
        }

        private void Select()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            ModelManager.Instance().OpenModel(SelectedModelDescription, null);
        }

        private async void Add()
        {
            ModelWindowResult result = await Application.Current.MainWindow.ShowChildWindowAsync<ModelWindowResult>(new ModelWindowView(), ChildWindowManager.OverlayFillBehavior.FullWindow);

            if (result != null)
            {
                var modelMgr = ModelManager.Instance();
                if (modelMgr.ModelDescriptionList.Any(m => m.Name == result.Name) == false)
                {
                    var desc = modelMgr.CreateModelDescription() as Model.ModelDescription;
                    desc.Name = result.Name;
                    desc.CreatedDate = DateTime.Now;
                    desc.ModifiedDate = DateTime.Now;
                    desc.Description = result.Description;

                    desc.LayerParamList = result.LayerParamList;
                    desc.ScanWidth = result.ScanWidth;
                    desc.SensorType = result.SensorType;

                    modelMgr.AddModel(desc);

                    OnPropertyChanged("Source");
                }
                else
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    await MessageWindowHelper.ShowMessage(this, header, "이미 등록된 모델입니다.", MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void Edit()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            var desc = SelectedModelDescription as Model.ModelDescription;
            ModelWindowResult modelResult = desc.ConvertToModelWindowResult();
            ModelWindowResult result = await Application.Current.MainWindow.ShowChildWindowAsync<ModelWindowResult>(new ModelWindowView(modelResult), ChildWindowManager.OverlayFillBehavior.FullWindow);

            if (result != null)
            {
                var modelMgr = ModelManager.Instance();
                desc.ModifiedDate = DateTime.Now;
                desc.Description = result.Description;

                desc.LayerParamList = result.LayerParamList;
                desc.ScanWidth = result.ScanWidth;
                desc.SensorType = result.SensorType;

                if (modelMgr.ModelDescriptionList.Any(m => m.Name == result.Name) == true)
                {
                    desc.Name = result.Name;
                    modelMgr.EditModel(desc);
                }
                else
                {
                    modelMgr.DeleteModel(selectedModelDescription.Name);
                    desc.Name = result.Name;
                    modelMgr.AddModel(desc);
                }
                OnPropertyChanged("Source");
            }
        }

        private async void Copy()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            var modelDesc = SelectedModelDescription as Model.ModelDescription;
            ModelWindowResult modelResult = modelDesc.ConvertToModelWindowResult();
            ModelWindowResult result = await Application.Current.MainWindow.ShowChildWindowAsync<ModelWindowResult>(new ModelWindowView(modelResult), ChildWindowManager.OverlayFillBehavior.FullWindow);

            if (result != null)
            {
                var modelMgr = ModelManager.Instance();
                if (modelMgr.ModelDescriptionList.Any(m => m.Name == result.Name) == false)
                {
                    var desc = modelMgr.CreateModelDescription() as Model.ModelDescription;
                    desc.Name = result.Name;
                    desc.CreatedDate = DateTime.Now;
                    desc.ModifiedDate = DateTime.Now;
                    desc.Description = result.Description;

                    desc.LayerParamList = result.LayerParamList;
                    desc.ScanWidth = result.ScanWidth;
                    desc.SensorType = result.SensorType;

                    modelMgr.AddModel(desc);

                    OnPropertyChanged("Source");
                }
                else
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    await MessageWindowHelper.ShowMessage(this, header, "이미 등록된 모델입니다.", MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void Remove()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            string header = TranslationHelper.Instance.Translate("Remove");
            MessageDialogResult result = await MessageWindowHelper.ShowMessage(this, header, "모델을 삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                ModelManager.Instance().DeleteModel(SelectedModelDescription.Name);
                OnPropertyChanged("Source");
            }
        }
        #endregion
    }
}
