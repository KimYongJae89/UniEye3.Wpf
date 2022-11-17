using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.Models
{
    public abstract class InspectResult
    {
        public Model Model { get; }
        public DateTime InspectTime { get; }

        public InspectResult(Model model)
        {
            Model = model;
            InspectTime = DateTime.Now;
        }
    }
}
