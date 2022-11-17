using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data.UI
{
    public interface IInspectionResultVisualizer
    {
        void Update(StepModel stepModel);
        void ResetResult();
        void UpdateResult(Target target, ProductResult targetResult);
    }
}
