using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Inspect
{
    public class InspectEventListener : IInspectEventListener
    {
        #region 필드
        private static InspectEventListener _instance;
        #endregion

        #region 생성자
        public InspectEventListener()
        {
            SystemManager.Instance().InspectRunner.InspectEventHandler.AddListener(this);
        }
        #endregion

        #region 대리자
        public delegate void UpdateResultEventDelegate(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null);
        public delegate void GeneralEventDelegate();
        #endregion

        #region 속성
        public static InspectEventListener Instance => (_instance ?? (_instance = new InspectEventListener()));

        public UpdateResultEventDelegate UpdateResult { get; set; }

        public GeneralEventDelegate StartDelegate { get; set; }

        public GeneralEventDelegate StopDelegate { get; set; }
        #endregion

        #region 메서드
        public bool EnterWaitInspection()
        {
            return true;
        }

        public void ExitWaitInspection()
        {
        }

        public void ProductBeginInspect(ProductResult productResult) { }

        public void ProductInspected(ProductResult productResult)
        {
            var productResults = new List<ProductResult>() { productResult };
            var task = Task.Run(() => UpdateResult(productResults));
            Task.WaitAll(task);
        }

        public void ProductEndInspect(ProductResult productResult) { }

        public void StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult) { }

        public void StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }

        public void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }

        public void TargetBeginInspect(Target target) { }

        public void TargetEndInspect(Target target, ProbeResultList probeResultList) { }

        public void TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList) { }

        public void ProbeBeginInspect() { }

        public void ProbeEndInspect() { }
        #endregion
    }
}
