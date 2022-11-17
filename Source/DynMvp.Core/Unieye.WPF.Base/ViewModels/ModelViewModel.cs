using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Services;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Translation.Helpers;

namespace Unieye.WPF.Base.ViewModels
{
    public class ModelViewModel : Observable
    {
        public ModelViewModel()
        {
            UserChanged(UserHandler.Instance.CurrentUser);
            UserHandler.Instance.OnUserChanged += OnUserChanged;
        }

        private void UserChanged(User user)
        {
            if (user != null)
            {
                IsModelUser = user.IsAuth(ERoleType.ModelPage);
            }
            else
            {
                IsModelUser = false;
            }
        }

        private void OnUserChanged(User user)
        {
            UserChanged(user);
        }

        private ModelDescription selectedModelDescription;
        public ModelDescription SelectedModelDescription
        {
            get => selectedModelDescription;
            set => Set(ref selectedModelDescription, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                Set(ref _searchText, value);
                OnPropertyChanged("Source");
            }
        }

        private bool isModelUser;
        public bool IsModelUser
        {
            get => isModelUser;
            set => Set(ref isModelUser, value);
        }

        private ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand(Remove));

        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(Add));

        private ICommand _selectCommand;
        public ICommand SelectCommand => _selectCommand ?? (_selectCommand = new RelayCommand(Select));

        private ICommand _copyCommand;
        public ICommand CopyCommand => _copyCommand ?? (_copyCommand = new RelayCommand(Copy));

        public IEnumerable<ModelDescription> Source => ModelManager.Instance().ModelDescriptionList.Where(model => model.Name.Contains(_searchText)).OrderByDescending(model => model.CreatedDate);

        private async void Remove()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            string header = TranslationHelper.Instance.Translate("Remove");
            string message = TranslationHelper.Instance.Translate("MODEL_DELETE_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                ModelManager.Instance().DeleteModel(SelectedModelDescription.Name);
                OnPropertyChanged("Source");
            }
        }

        private void Select()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            string header = UniEye.Translation.Helpers.TranslationHelper.Instance.Translate("Select");

            //var result = await MessageWindowHelper.ShowMessage(this, header, "모델을 선택하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);

            //if (result == MessageDialogResult.Affirmative)
            ModelManager.Instance().OpenModel(SelectedModelDescription, null);
        }

        private async void Add()
        {
            ModelWindowResult result = await Application.Current.MainWindow.ShowChildWindowAsync<ModelWindowResult>(new ModelWindow(), ChildWindowManager.OverlayFillBehavior.FullWindow);

            if (result != null)
            {
                var modelMgr = ModelManager.Instance();
                if (modelMgr.ModelDescriptionList.Any(m => m.Name == result.Name) == false)
                {
                    ModelDescription desc = modelMgr.CreateModelDescription();
                    desc.Name = result.Name;
                    desc.CreatedDate = DateTime.Now;
                    desc.ModifiedDate = DateTime.Now;
                    desc.Description = result.Description;

                    modelMgr.AddModel(desc);

                    OnPropertyChanged("Source");
                }
                else
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    string message = TranslationHelper.Instance.Translate("MODEL_OVERLAP_MESSAGE");
                    await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                }
            }
        }

        private async void Copy()
        {
            if (SelectedModelDescription == null)
            {
                return;
            }

            ModelWindowResult result = await Application.Current.MainWindow.ShowChildWindowAsync<ModelWindowResult>(new ModelWindow(), ChildWindowManager.OverlayFillBehavior.FullWindow);

            if (result != null)
            {
                var modelMgr = ModelManager.Instance();
                if (modelMgr.ModelDescriptionList.Any(m => m.Name == result.Name) == false)
                {
                    ModelDescription desc = SelectedModelDescription.Clone();
                    desc.Name = result.Name;
                    desc.CreatedDate = DateTime.Now;
                    desc.ModifiedDate = DateTime.Now;
                    desc.Description = result.Description;

                    modelMgr.CopyModelDescription(SelectedModelDescription, desc);
                    modelMgr.CopyModelData(SelectedModelDescription.Name, desc.Name);

                    ModelBase srcModel = modelMgr.LoadModel(SelectedModelDescription);
                    ModelBase dstModel = modelMgr.LoadModel(result.Name);
                    if (srcModel != null && dstModel != null)
                    {
                        dstModel.CopyFrom(srcModel);
                        dstModel.ModelPath = modelMgr.GetModelPath(result.Name);
                        dstModel.ModelDescription = desc;
                        dstModel.SaveModel();
                    }

                    modelMgr.ModelDescriptionList.Add(desc);
                    modelMgr.ModelListChanged();

                    OnPropertyChanged("Source");
                }
                else
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    string message = TranslationHelper.Instance.Translate("MODEL_OVERLAP_MESSAGE");
                    await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                }
            }
        }
    }
}
