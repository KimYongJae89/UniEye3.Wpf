using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AlgorithmBaseParamAttribute : Attribute
    {
        public AlgorithmBaseParamAttribute() { }
    }

    public interface IAlgorithmBaseParam : INodeParam
    {
        Type AlgorithmType { get; }

        void SetVisionModel(VisionModel visionModel);

        //IAlgorithmBase BuildAlgorithm(ModuleInfo moduleInfo);
    }


    public abstract class AlgorithmBaseParam<Talgo, Tin, Tout> : IAlgorithmBaseParam
            where Tin : InputOutputs, new()
            where Tout : InputOutputs, IResultBufferItem
    {
        public abstract void SetVisionModel(VisionModel visionModel);

        public static Type GetAlgorithmType()
        {
            return typeof(Talgo);
        }

        public static Type GetInputType()
        {
            return typeof(Tin);
        }

        public static Type GetOutputType()
        {
            return typeof(Tout);
        }

        public string Name { get; set; }

        public Type AlgorithmType => GetAlgorithmType();
        //public Type InputType => GetInputType();
        //public Type OutputType => GetOutputType();

        public (string, Type)[] InPropNameTypes => typeof(Tin).GetProperties().Select(f => (f.Name, f.PropertyType)).ToArray();

        public (string, Type)[] OutPropNameTypes => typeof(Tout).GetProperties().Select(f => (f.Name, f.PropertyType)).ToArray();

        public AlgorithmBaseParam()
        {
            Type t = typeof(Talgo);
            Name = t.Name;
        }

        public AlgorithmBaseParam(IAlgorithmBaseParam algorithmBaseParam) : this()
        {
            CopyFrom(algorithmBaseParam);
        }

        public AlgorithmBaseParam(string name)
        {
            Name = name;
        }

        public AlgorithmBaseParam(VisionModel visionModel) : this()
        {
            SetVisionModel(visionModel);
        }

        public AlgorithmBaseParam(string name, VisionModel visionModel) : this(name)
        {
            SetVisionModel(visionModel);
        }

        public virtual INode BuildNode(ModuleInfo moduleInfo)
        {
            Type type = typeof(Talgo);
            var algorithmBase = (IAlgorithmBase)Activator.CreateInstance(type, moduleInfo, this);
            return algorithmBase;
        }

        public abstract INodeParam Clone();
        public abstract void CopyFrom(IAlgorithmBaseParam algorithmBaseParam);
    }

}
