using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;

namespace DynMvp.Inspect
{
    public delegate bool EnterWaitInspectionDelegate();
    public delegate void ExitWaitInspectionDelegate();

    public delegate void ProductBeginInspectDelegate(ProductResult productResult);
    public delegate void ProductInspectedDelegate(ProductResult productResult);
    public delegate void ProductEndInspectDelegate(ProductResult productResult);

    public delegate void StepOrderEndInspectDelegate(ModelBase model, int inspectOrder, ProductResult productResult);

    public delegate void StepBeginInspectDelegate(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer);
    public delegate void StepEndInspectDelegate(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer);

    public delegate void TargetBeginInspectDelegate(Target target);
    public delegate void TargetEndInspectDelegate(Target target, ProbeResultList probeResultList);
    public delegate void TargetOrderEndInspectDelegate(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList);

    public delegate void ProbeBeginInspectDelegate();
    public delegate void ProbeEndInspectDelegate();

    public interface IInspectEventListener
    {
        bool EnterWaitInspection();
        void ExitWaitInspection();

        void ProductBeginInspect(ProductResult productResult);
        void ProductInspected(ProductResult productResult);
        void ProductEndInspect(ProductResult productResult);

        /// <summary>
        /// 보정 마크 계산 등을 수행한다.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="inspectOrder"></param>
        /// <param name="productResult"></param>
        void StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult);

        void StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer);
        void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer);

        void TargetBeginInspect(Target target);
        void TargetEndInspect(Target target, ProbeResultList probeResultList);
        void TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList);

        void ProbeBeginInspect();
        void ProbeEndInspect();
    }
}
