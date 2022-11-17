using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.UniScanCM.Override
{
    public class ModelBuilder : UniScanC.Models.ModelBuilder
    {
        public override DynMvp.Data.ModelBase CreateModel()
        {
            return new UniScanC.Models.Model(SystemConfig.Instance.ImModuleList.Count);
        }
    }
}
