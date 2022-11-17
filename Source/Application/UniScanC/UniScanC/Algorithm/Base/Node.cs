using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.Base
{
    public interface INodeParam
    {
        string Name { get; set; }

        //Type InputType { get; }
        //Type OutputType { get; }

        (string, Type)[] InPropNameTypes { get; }
        (string, Type)[] OutPropNameTypes { get; }

        INode BuildNode(ModuleInfo moduleInfo);
        INodeParam Clone();
    }

    public interface INode
    {
        Type GetInputType();
        Type GetOutputType();
    }
}
