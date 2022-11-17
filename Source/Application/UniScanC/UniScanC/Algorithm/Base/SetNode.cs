using DynMvp.Base;
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
    public enum ESetNodeType { Union, Intersection };

    public class Input<T> : InputOutputs<List<List<T>>>
    {
        public List<List<T>> ListArray { get => Item1; set => Item1 = value; }

        public Input() : base("ListArray")
        {
            ListArray = new List<List<T>>();
        }

        public override void SetValue(string key, Type type, object data)
        {
            if (!(data is List<T>))
            {
                throw new InvalidCastException();
            }

            ListArray.Add((List<T>)data);
        }
    }

    public class Output<T> : InputOutputs<List<T>>, IResultBufferItem
    {
        public List<T> List { get => Item1; set => Item1 = value; }

        public Output() : base("List")
        {
            List = new List<T>();
        }

        public void CopyFrom(IResultBufferItem from)
        {
            var output = (Output<T>)from;
            List = new List<T>(output.List);
        }

        public bool Request(InspectBufferPool bufferPool) { return true; }

        public void Return(InspectBufferPool bufferPool) { }

        public void SaveDebugInfo(DebugContextC debugContext) { }
    }

    [AlgorithmBaseParam]
    public class SetNodeParam<T> : AlgorithmBaseParam<SetNode<T>, Input<T>, Output<T>>
    {
        public ESetNodeType NodeType { get; set; }

        //public Type InputType => typeof(Input<T>);
        //public Type OutputType => typeof(Output<T>);

        private (string, Type)[] PropNameTypes => typeof(T).GetProperties().Select(f => (f.Name, f.PropertyType)).ToArray();

        public SetNodeParam() { }

        public SetNodeParam(ESetNodeType nodeType)
        {
            NodeType = nodeType;
        }

        public SetNodeParam(string name, ESetNodeType nodeType) : this(nodeType)
        {
            Name = name;
        }

        public SetNodeParam(IAlgorithmBaseParam algorithmBaseParam) : base(algorithmBaseParam) { }

        public override INode BuildNode(ModuleInfo moduleInfo)
        {
            switch (NodeType)
            {
                case ESetNodeType.Union:
                    return new UnionNode<T>(moduleInfo, this);
                case ESetNodeType.Intersection:
                    return new IntersectionNode<T>(moduleInfo, this);
            }

            throw new Exception();
        }

        public override INodeParam Clone()
        {
            return new SetNodeParam<T>(this);
        }

        public override void SetVisionModel(VisionModel visionModel)
        {
        }

        public override void CopyFrom(IAlgorithmBaseParam algorithmBaseParam)
        {
            var setNodeParam = (SetNodeParam<T>)algorithmBaseParam;
            Name = setNodeParam.Name;
            NodeType = setNodeParam.NodeType;
        }
    }

    public abstract class SetNode<T> : AlgorithmBase<Input<T>, Output<T>>
    {
        public override int RequiredBufferCount => 0;

        public SetNode(ModuleInfo moduleInfo, IAlgorithmBaseParam param) : base(moduleInfo, param) { }
    }

    public class IntersectionNode<T> : SetNode<T>
    {
        public IntersectionNode(ModuleInfo moduleInfo, IAlgorithmBaseParam param) : base(moduleInfo, param) { }

        public override bool Run(Input<T> input, ref Output<T> output, AlgoImage[] workingBuffers)
        {
            try
            {
                IEnumerable<IEnumerable<T>> listList = input.ListArray;
                output.List = listList.Aggregate((f, g) => f.Intersect(g)).ToList();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"IntersectionNode::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }
    }

    public class UnionNode<T> : SetNode<T>
    {
        public UnionNode(ModuleInfo moduleInfo, IAlgorithmBaseParam param) : base(moduleInfo, param) { }

        public override bool Run(Input<T> input, ref Output<T> output, AlgoImage[] workingBuffers)
        {
            try
            {
                IEnumerable<IEnumerable<T>> listList = input.ListArray;
                output.List = listList.Aggregate((f, g) => f.Union(g)).ToList();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, $"UnionNode::Run - {ex.GetType().Name}{Environment.NewLine}{ex.StackTrace}");
                return false;
            }
        }
    }
}