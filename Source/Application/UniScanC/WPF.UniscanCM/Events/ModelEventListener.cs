using Authentication.Core;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using System.IO;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.Data;
using UniEye.Translation.Helpers;
using UniScanC.Enums;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Windows.Views;
using CommManager = WPF.UniScanCM.Override.CommManager;

namespace WPF.UniScanCM.Events
{
    public delegate void ModelEventDelegate(ModelBase model);
    public class ModelEventListener : IModelEventListener
    {
        private static ModelEventListener _instance;
        public static ModelEventListener Instance => (_instance ?? (_instance = new ModelEventListener()));

        public ModelEventDelegate OnModelOpened;
        public VoidDelegate OnModelClosed;

        public async void ModelOpen(ModelBase model)
        {
            Authentication.Core.Datas.User curUser = UserHandler.Instance.CurrentUser;

            var commMgr = CommManager.Instance() as CommManager;
            var modelPath = new DirectoryInfo(ModelManager.Instance().ModelPath);
            // AlgoTask를 모델 경로에 저장
            string modelFilePath = Path.Combine(modelPath.FullName, model.Name);
            SystemConfig.Instance.AlgoTaskManagerSettingDictionary.Save(modelFilePath);

            if (model != null && await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, model.Name, modelPath.Name))
            {
                if (curUser.IsAuth(ERoleType.InspectPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, true);
                }

                if (curUser.IsAuth(ERoleType.TeachPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, true);
                }

                UiManager.Instance.ShowTab(UniEye.Base.UI.TabKey.Inspect);

                LogHelper.Info(LoggerType.Operation, $"[Model Page] Model Open : {model.Name}");
                await Task.Run(() => OnModelOpened?.Invoke(model));

                SystemState.Instance().SetIdle();
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Model Page] Failed to Model Open : {model?.Name}");
                DynMvp.Data.ModelManager.Instance().CloseModel();

                if (curUser.IsAuth(ERoleType.InspectPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, false);
                }

                if (curUser.IsAuth(ERoleType.TeachPage))
                {
                    UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, false);
                }

                await MessageWindowHelper.ShowMessageBox(
                    TranslationHelper.Instance.Translate("NOTIFICATION"),
                    TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                    System.Windows.MessageBoxButton.OK);

                SystemState.Instance().SetIdle();
            }
        }

        public void ModelClosed(ModelBase model) { }

        public void ModelListChanged() { }
    }
}
