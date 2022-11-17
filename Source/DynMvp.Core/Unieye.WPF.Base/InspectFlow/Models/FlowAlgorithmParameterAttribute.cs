using System;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FlowAlgorithmParameterAttribute : Attribute
    {
        public FlowAlgorithmParameterAttribute() { }
    }
}