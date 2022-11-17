using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    public class InspectionFlowModelHandler
    {
        private static InspectionFlowModelHandler _instance;
        public static InspectionFlowModelHandler Instance => _instance ?? (_instance = new InspectionFlowModelHandler());

        public List<InspectionFlowModel> InspectionFlowModelList { get; set; } = new List<InspectionFlowModel>();
    }
}
