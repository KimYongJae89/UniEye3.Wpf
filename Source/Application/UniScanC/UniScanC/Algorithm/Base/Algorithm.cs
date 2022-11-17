using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using UniScanC.Data;
using UniScanC.Models;
using UniScanC.Struct;

namespace UniScanC.Algorithm.Base
{
    public interface IAlgorithmBase : INode
    {
        int RequiredBufferCount { get; }
    }

    public abstract class AlgorithmBase<Tin, Tout> : IAlgorithmBase
        where Tin : InputOutputs, new()
        where Tout : InputOutputs, IResultBufferItem
    {
        public static Type InputType => typeof(Tin);
        public static Type OutputType => typeof(Tout);
        //public string Name => Param.Name;

        public IAlgorithmBaseParam Param { get; }
        public ModuleInfo ModuleInfo { get; }

        public abstract int RequiredBufferCount { get; }

        public AlgorithmBase(ModuleInfo moduleInfo, IAlgorithmBaseParam param)
        {
            Param = param;
            ModuleInfo = moduleInfo;
        }

        public virtual AlgoImage[] BuildBuffer()
        {
            Size size = ModuleInfo.Camera.ImageSize;
            var array = new AlgoImage[RequiredBufferCount];
            for (int i = 0; i < RequiredBufferCount; i++)
            {
                array[i] = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(ImageType.Grey, size.Width, size.Height);
            }

            return array;
        }

        public Type GetInputType()
        {
            return InputType;
        }

        public Type GetOutputType()
        {
            return OutputType;
        }

        public abstract bool Run(Tin input, ref Tout output, AlgoImage[] workingBuffers);
    }
}
