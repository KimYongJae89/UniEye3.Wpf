using Unieye.WPF.Base.Helpers;
using WPF.UniScanIM.Manager;

namespace WPF.UniScanIM.Override
{
    public class SystemManager : Unieye.WPF.Base.Override.SystemManager
    {
        public TeachingManager TeachingManager { get; }

        public SystemManager() : base()
        {
            TeachingManager = new TeachingManager();
        }

        public static new SystemManager Instance()
        {
            return instance as SystemManager;
        }

        public override void Release()
        {
            base.Release();

            CommManager.Instance().Disconnect();

            //foreach (var module in SystemConfig.Instance.ModuleList)
            //{
            //    module.CommManager?.Disconnect();
            //}
        }
    }
}
