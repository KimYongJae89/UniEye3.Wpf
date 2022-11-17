using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniScanC.Algorithm.Base;
using UniScanC.Data;
using UniScanC.Struct;

namespace UniScanC.AlgoTask
{
    public interface ILink
    {
        //Type Type { get; }

        ILink Clone();

        bool IsDestination(INodeParam nodeParam);
        bool IsDestination(IAlgoTask algoTask);
        bool IsDestination(IAlgorithmBaseParam param);

        bool IsResult();

        void SetData(TupleElement tupleElement, Type type, object data);
        (Type, object) GetData(InspectBufferSet inspectBufferSet);
    }

    public class Link : ILink
    {
        public int SrcUnit { get; set; }

        public int SrcPort { get; set; }

        public int DstUnit { get; set; }

        public int DstPort { get; set; }

        public Link(int fromUnitNo, int fromPortNo, int toUnitNo, int toPortNo)
        {
            SrcUnit = fromUnitNo;
            SrcPort = fromPortNo;
            DstUnit = toUnitNo;
            DstPort = toPortNo;
        }

        public virtual ILink Clone()
        {
            return new Link(SrcUnit, SrcPort, DstUnit, DstPort);
        }

        public virtual bool IsDestination(INodeParam nodeParam)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsDestination(IAlgoTask algoTask)
        {
            return DstUnit == algoTask.Index;
        }

        public virtual bool IsDestination(IAlgorithmBaseParam param)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsResult()
        {
            return DstUnit < 0;
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {SrcUnit}.{SrcPort} -> {DstUnit}.{DstPort}";
        }

        public void SetData(TupleElement tupleElement, Type type, object data)
        {
            tupleElement.SetValue(DstPort, type, data);
        }


        public (Type, object) GetData(InspectBufferSet inspectBufferSet)
        {
            IResultBufferItem taskResult = inspectBufferSet.GetTaskResult(SrcUnit);
            Type type = taskResult.GetType(SrcPort);
            object data = taskResult.GetValue<object>(SrcPort);
            return (type, data);
        }
    }

    public class LinkS : ILink
    {
        public string SrcUnitName { get; set; }
        public string SrcPortName { get; set; }
        public string DstUnitName { get; set; }
        public string DstPortName { get; set; }

        public LinkS(string srcUnitName, string srcPortName, string dstUnitName, string dstPortName)
        {
            SrcUnitName = srcUnitName;
            SrcPortName = srcPortName;
            DstUnitName = dstUnitName;
            DstPortName = dstPortName;
        }

        public bool IsDestination(IAlgoTask algoTask)
        {
            return DstUnitName == algoTask.Name;
        }

        public virtual bool IsDestination(INodeParam nodeParam)
        {
            return DstUnitName == nodeParam.Name;
        }

        public virtual bool IsDestination(IAlgorithmBaseParam param)
        {
            return DstUnitName == param.Name;
        }

        public bool IsResult()
        {
            return DstUnitName == typeof(InspectResult).Name;
        }

        public virtual ILink Clone()
        {
            return new LinkS((string)SrcUnitName.Clone(), (string)SrcPortName.Clone(), (string)DstUnitName.Clone(), (string)DstPortName.Clone());
        }

        public void SetData(TupleElement tupleElement, Type type, object data)
        {
            tupleElement.SetValue(DstPortName, type, data);
        }

        public virtual (Type, object) GetData(InspectBufferSet inspectBufferSet)
        {
            IResultBufferItem taskResult = inspectBufferSet.GetTaskResult(SrcUnitName);
            Type type = taskResult.GetType(SrcPortName);
            object data = taskResult.GetValue<object>(SrcPortName);
            return (type, data);
        }
    }

    //public class LinkEx<Tin, Tout> : LinkS
    //{
    //    Func<Tin, Tout> convertor;

    //    public LinkEx(string srcUnitName, string srcPortName, string dstUnitName, string dstPortName, Func<Tin, Tout> convertor) : base(srcUnitName, srcPortName, dstUnitName, dstPortName)
    //    {
    //        this.convertor = convertor;
    //    }

    //    public override ILink Clone()
    //    {
    //        return new LinkEx<Tin, Tout>((string)this.SrcUnitName.Clone(), (string)this.SrcPortName.Clone(), (string)this.DstUnitName.Clone(), (string)this.DstPortName.Clone(), convertor);
    //    }

    //    public override (Type, object) GetData(ResultBuffer inspectBufferSet)
    //    {
    //        (Type, object) data = base.GetData(inspectBufferSet);
    //        Type type = typeof(Tout);
    //        object converted = null;
    //        if (convertor != null)
    //            converted = convertor.Invoke((Tin)data.Item2);
    //        return (type, converted);
    //    }
    //}

    public class LinkEx : LinkS
    {
        public Type Tin { get; set; }
        public Type Tout { get; set; }

        public LinkEx(string srcUnitName, string srcPortName, string dstUnitName, string dstPortName, Type tin, Type tout) : base(srcUnitName, srcPortName, dstUnitName, dstPortName)
        {
            Tin = tin;
            Tout = tout;
        }

        public override ILink Clone()
        {
            return new LinkEx((string)SrcUnitName.Clone(), (string)SrcPortName.Clone(), (string)DstUnitName.Clone(), (string)DstPortName.Clone(), Tin, Tout);
        }

        public override (Type, object) GetData(InspectBufferSet inspectBufferSet)
        {
            //LogHelper.Debug(LoggerType.Inspection, $"LinkEx::GetData - {this.SrcUnitName}.{this.SrcPortName}({this.Tin.Name}) ->  {this.DstUnitName}.{this.DstPortName}{this.Tout}");

            (Type, object) data = base.GetData(inspectBufferSet);
            if (data.Item2 == null)
            {
                return (Tout, null);
            }

            object converted = AlgoLinkConverter.Convert(Tin, Tout, data.Item2);
            return (Tout, converted);
        }
    }

    public class LinkListAdd<T> : ILink
    {
        public (string, string)[] Tuples { get; set; }
        public string DstUnitName { get; set; }
        public string DstPortName { get; set; }

        public LinkListAdd() { }
        public LinkListAdd(string dstUnitName, string dstPortName, params (string, string)[] srcUnitPortNames)
        {
            int tupleLen = srcUnitPortNames.Length;
            Tuples = ((string, string)[])srcUnitPortNames.Clone();

            DstUnitName = dstUnitName;
            DstPortName = dstPortName;
        }

        public ILink Clone()
        {
            return new LinkListAdd<T>(DstUnitName, DstPortName, Tuples);
        }

        public (Type, object) GetData(InspectBufferSet inspectBufferSet)
        {
            var joinList = new List<T>();
            Array.ForEach(Tuples, f =>
            {
                List<T> list = inspectBufferSet.GetTaskResult(f.Item1).GetValue<List<T>>(f.Item2);
                joinList.AddRange(list);
            });

            return (typeof(List<T>), joinList);
        }


        public virtual bool IsDestination(INodeParam nodeParam)
        {
            return DstUnitName == nodeParam.Name;
        }

        public bool IsDestination(IAlgoTask algoTask)
        {
            return DstUnitName == algoTask.Name;
        }

        public bool IsDestination(IAlgorithmBaseParam param)
        {
            return DstUnitName == param.Name;
        }

        public bool IsResult()
        {
            return DstUnitName == typeof(InspectResult).Name;
        }

        public void SetData(TupleElement tupleElement, Type type, object data)
        {
            tupleElement.SetValue(DstPortName, type, data);
        }
    }
}
