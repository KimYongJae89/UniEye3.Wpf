using DynMvp.Data;
using System.Threading.Tasks;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Model
{
    public class ModelEventListener : IModelEventListener
    {
        #region 필드
        private static ModelEventListener _instance;
        #endregion

        #region 생성자
        public ModelEventListener() { }
        #endregion

        #region 대리자
        public delegate void ModelOpenEventDelegate(ModelBase model);
        public delegate void ModelCloseEventDelegate();
        #endregion

        #region 속성
        public static ModelEventListener Instance => (_instance ?? (_instance = new ModelEventListener()));

        public ModelOpenEventDelegate OnModelOpened { get; set; }

        public ModelCloseEventDelegate OnModelClosed { get; set; }
        #endregion

        #region 메서드
        public void ModelOpen(ModelBase model)
        {
            var task = Task.Run(() => OnModelOpened(model));
            Task.WaitAll(task);

            bool enable = true;
            UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, enable);
            UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, enable);

            UiManager.Instance.ShowTab(UniEye.Base.UI.TabKey.Inspect);
        }

        public void ModelClosed(ModelBase model)
        {
            var task = Task.Run(() => OnModelClosed());
            Task.WaitAll(task);

            bool enable = false;
            UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, enable);
            UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Inspect, enable);
        }

        public void ModelListChanged()
        {

        }
        #endregion
    }
}
