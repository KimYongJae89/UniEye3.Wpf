using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Module;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Service;
using WPF.UniScanCM.Windows.ViewModels;

namespace WPF.UniScanCM.Controls.ViewModels
{
    public class ModelParameterViewModel : Observable
    {
        private TeachingService TeachingService { get; set; } = new TeachingService();

        // 모델을 오픈할 때 마다 복사해서 가지고 있는 모델
        // Model를 복사해서 열어 놓는다.
        // 파라미터를 바꾸고 싶지 않은 경우 저장을 하지 않고 빠져 나올 수 있도록
        private UniScanC.Models.Model Model { get; set; }

        private Dictionary<string, BitmapSource> InspectImageDic { get; set; } = new Dictionary<string, BitmapSource>();

        private AlgoTaskManagerSettingDictionary AlgoTaskManagerSettingDictionary => SystemConfig.Instance.AlgoTaskManagerSettingDictionary;

        public List<InspectModuleInfo> ModuleList => SystemConfig.Instance.ImModuleList;

        private InspectModuleInfo selectedModule;
        public InspectModuleInfo SelectedModule
        {
            get => selectedModule;
            set
            {
                Set(ref selectedModule, value);
                {
                    if (Model != null)
                    {
                        VisionModel = Model.VisionModels[value.ModuleNo];
                        if (AlgoTaskManagerSettingDictionary.ContainsKey(value.ModuleTopic))
                        {
                            VisionModel.NodeParams = AlgoTaskManagerSettingDictionary[value.ModuleTopic].ParamList;
                        }

                        if (InspectImageDic.ContainsKey(value.ModuleTopic))
                        {
                            InspectImage = InspectImageDic[value.ModuleTopic];
                        }

                        if (VisionModel.NodeParams.FindAll(x => x is IAlgorithmBaseParam).Cast<IAlgorithmBaseParam>().Where(x => x.AlgorithmType.Name == "ColorChecker").Count() > 0)
                        {
                            UseColorChecker = true;
                        }
                        else
                        {
                            UseColorChecker = false;
                        }
                    }
                }
            }
        }

        private bool onGrab;
        public bool OnGrab
        {
            get => onGrab;
            set => Set(ref onGrab, value);
        }

        private VisionModel visionModel;
        public VisionModel VisionModel
        {
            get => visionModel;
            set => Set(ref visionModel, value);
        }

        private BitmapSource inspectImage;
        public BitmapSource InspectImage
        {
            get => inspectImage;
            set => Set(ref inspectImage, value);
        }

        private bool isDetailParamShow = false;
        public bool IsDetailParamShow
        {
            get => isDetailParamShow;
            set => Set(ref isDetailParamShow, value);
        }

        private bool isDetailTeachingAuthorized = false;
        public bool IsDetailTeachingAuthorized
        {
            get => isDetailTeachingAuthorized;
            set => Set(ref isDetailTeachingAuthorized, value);
        }

        private bool useColorChecker = false;
        public bool UseColorChecker
        {
            get => useColorChecker;
            set => Set(ref useColorChecker, value);
        }

        public System.Windows.Input.ICommand SaveCommand { get; }
        public System.Windows.Input.ICommand BatchSettingCommand { get; }
        public System.Windows.Input.ICommand InspectCommand { get; }
        public System.Windows.Input.ICommand GrabCommand { get; }
        public System.Windows.Input.ICommand LightValueChangedCommand { get; }

        public ModelParameterViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandAction);
            BatchSettingCommand = new RelayCommand(BatchSettingCommandAction);
            InspectCommand = new RelayCommand(InspectCommandAction);
            GrabCommand = new RelayCommand(GrabCommandAction);
            LightValueChangedCommand = new RelayCommand(LightValueChangedCommandAction);

            if (ModuleList.Count > 0)
            {
                SelectedModule = ModuleList.FirstOrDefault();
            }

            TeachingService.OnUpdateImage = OnUpdateImage;
            UserHandler.Instance.OnUserChanged += OnUserChanged;
        }

        private async void SaveCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Model Parameter Save");
            string header = TranslationHelper.Instance.Translate("Save");
            string message = TranslationHelper.Instance.Translate("SAVE_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                await SaveSettings();
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Model Parameter Save Complete");

                var commMgr = CommManager.Instance() as CommManager;
                var modelPath = new DirectoryInfo(UniScanC.Models.ModelManager.Instance().ModelPath);
                if (Model != null && await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, Model.Name, modelPath.Name))
                {
                }
                else
                {
                    await MessageWindowHelper.ShowMessageBox(
                        TranslationHelper.Instance.Translate("NOTIFICATION"),
                        TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                        System.Windows.MessageBoxButton.OK);
                }

                ModelEventListener.Instance.OnModelOpened.Invoke(Model);
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Cancel Model Parameter Save");
            }
        }

        private async void BatchSettingCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Model Parameter Batch Setting");
            string header = TranslationHelper.Instance.Translate("BATCH_SETTING");
            string message = TranslationHelper.Instance.Translate("BATCH_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                foreach (VisionModel visionModel in Model.VisionModels)
                {
                    if (visionModel != VisionModel)
                    {
                        visionModel.CopyParametersFrom(VisionModel);
                    }
                }

                await SaveSettings();
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Model Parameter Batch Setting Complete");

                var commMgr = CommManager.Instance() as CommManager;
                var modelPath = new DirectoryInfo(UniScanC.Models.ModelManager.Instance().ModelPath);
                if (Model != null && await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, Model.Name, modelPath.Name))
                {
                }
                else
                {
                    await MessageWindowHelper.ShowMessageBox(
                        TranslationHelper.Instance.Translate("NOTIFICATION"),
                        TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                        System.Windows.MessageBoxButton.OK);
                }
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Cancel Model Parameter Batch Setting");
            }

            ModelEventListener.Instance.OnModelOpened.Invoke(Model);
        }

        private async void InspectCommandAction()
        {
            // 티칭 전용 모델저장
            var actionList = new List<Action>();
            actionList.Add(() => Model.SaveTeachModel());

            var source = new ProgressSource();
            source.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            await MessageWindowHelper.ShowProgress(TranslationHelper.Instance.Translate("Setting"),
                TranslationHelper.Instance.Translate("Save_the_configuration_file") + ("..."),
                actionList, true, source);

            await TeachingService.Inspect();
            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Inspect");
        }

        private async void GrabCommandAction()
        {
            OnGrab = await TeachingService.Grab();
            if (OnGrab)
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Model, false);
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, false);
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Report, false);
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Setting, false);
            }
            else
            {
                if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.ModelPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Model, true);
                }

                if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.InspectPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, true);
                }

                if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.ReportPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Report, true);
                }

                if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.SettingPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Setting, true);
                }
            }

            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Grab [{OnGrab}]");
        }

        private void LightValueChangedCommandAction()
        {
            if (OnGrab)
            {
                TeachingService.SetLight(true, VisionModel.TopLightValue, VisionModel.BottomLightValue);
            }
        }

        public async Task SaveSettings()
        {
            var actionList = new List<Action>();
            Model.ModelDescription.ModifiedDate = DateTime.Now;
            actionList.Add(() => Model.SaveModel());

            var source = new ProgressSource();
            source.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            await MessageWindowHelper.ShowProgress(TranslationHelper.Instance.Translate("Setting"),
                TranslationHelper.Instance.Translate("Save_the_configuration_file") + ("..."),
                actionList, true, source);
        }

        public void SetModel(ModelBase model)
        {
            // VisionModel을 복사해서 열어 놓는다.
            // 파라미터를 바꾸고 싶지 않은 경우 저장을 하지 않고 빠져 나올 수 있도록
            Model = TeachingService.Model = model.Clone() as UniScanC.Models.Model;
            if (Model != null)
            {
                if (SelectedModule != null)
                {
                    VisionModel = TeachingService.VisionModel = Model.VisionModels[SelectedModule.ModuleNo];
                    if (AlgoTaskManagerSettingDictionary.ContainsKey(SelectedModule.ModuleTopic))
                    {
                        VisionModel.NodeParams = AlgoTaskManagerSettingDictionary[SelectedModule.ModuleTopic].ParamList;
                    }

                    InspectImageDic.Clear();
                    foreach (InspectModuleInfo module in ModuleList)
                    {
                        InspectImageDic.Add(module.ModuleTopic, null);
                    }
                    if (VisionModel.NodeParams.FindAll(x => x is IAlgorithmBaseParam).Cast<IAlgorithmBaseParam>().Where(x => x.AlgorithmType.Name == "ColorChecker").Count() > 0)
                    {
                        UseColorChecker = true;
                    }
                    else
                    {
                        UseColorChecker = false;
                    }
                }
            }
        }

        public void ClearModel()
        {
            VisionModel = TeachingService.VisionModel = null;
            InspectImageDic.Clear();
        }

        private void OnUpdateImage(string imName, BitmapSource source)
        {
            InspectImageDic[imName] = source;

            if (SelectedModule == null)
            {
                return;
            }

            if (SelectedModule.ModuleTopic == imName)
            {
                InspectImage = InspectImageDic[imName];
            }
        }

        private void OnUserChanged(User user)
        {
            if (user.IsAuth(ERoleType.DetailTeaching))
            {
                IsDetailTeachingAuthorized = true;
            }
            else
            {
                IsDetailTeachingAuthorized = false;
            }
        }
    }
}
