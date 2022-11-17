using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System.Collections.Generic;

namespace UniEye.Base.Inspect
{
    public class InspectEventHandler : IInspectEventListener
    {
        private List<IInspectEventListener> listenerList = new List<IInspectEventListener>();

        public InspectEventHandler()
        {
        }

        public void AddListener(IInspectEventListener inspectEventListener)
        {
            listenerList.Add(inspectEventListener);
        }

        public bool EnterWaitInspection()
        {
            bool result = true;

            listenerList.ForEach(x => result &= x.EnterWaitInspection());

            return result;
        }

        public void ExitWaitInspection()
        {
            listenerList.ForEach(x => x.ExitWaitInspection());
        }

        public void ProductBeginInspect(ProductResult productResult)
        {
            listenerList.ForEach(x => x.ProductBeginInspect(productResult));
        }

        public void ProductInspected(ProductResult inspectResult)
        {
            listenerList.ForEach(x =>
            {
                var bt = new BlockTracer("InspectEventHandler.ProductInspected : " + x.GetType().ToString());
                x.ProductInspected(inspectResult);
            });
        }

        public void ProductEndInspect(ProductResult productResult)
        {
            listenerList.ForEach(x => x.ProductEndInspect(productResult));
        }

        public void StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer)
        {
            foreach (IInspectEventListener listener in listenerList)
            {
                listener.StepBeginInspect(inspectStep, productResult, imageBuffer);
            }
        }

        public void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer)
        {
            listenerList.ForEach(x => x.StepEndInspect(inspectStep, productResult, imageBuffer));
        }

        public void StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult)
        {
            listenerList.ForEach(x => x.StepOrderEndInspect(model, inspectOrder, productResult));
        }

        public void TargetBeginInspect(Target target)
        {
            listenerList.ForEach(x => x.TargetBeginInspect(target));
        }

        public void TargetEndInspect(Target target, ProbeResultList probeResultList)
        {
            listenerList.ForEach(x => x.TargetEndInspect(target, probeResultList));
        }

        public void TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList)
        {
            listenerList.ForEach(x => x.TargetOrderEndInspect(inspectStep, inspectOrder, probeResultList));
        }

        public void ProbeBeginInspect()
        {
            listenerList.ForEach(x => x.ProbeBeginInspect());
        }

        public void ProbeEndInspect()
        {
            listenerList.ForEach(x => x.ProbeEndInspect());
        }
    }
}
