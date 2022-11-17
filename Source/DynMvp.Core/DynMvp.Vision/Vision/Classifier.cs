using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CNTK;
using CNTKTool;

namespace DynMvp.Vision.Vision
{
    abstract class ClassifierImage : AlgoImage, IData
    {
        public List<float[]> GenerateRaw()
        {
            float[] rawArray = Array.ConvertAll(GetByte(), x => (float)x);
            return new List<float[]> { rawArray };
        }

        public void LoadFromFile(string filePath)
        {
            using (var tempImage = new Bitmap(filePath))
                SetByte(BitmapTool.GetRawImage(tempImage));
        }
    }

    abstract class ClassifierLabel : IData
    {
        public string ClassName { get; internal set; }


        public List<float[]> GenerateRaw()
        {
            float[] id = { ClassNameToId(ClassName) };
            return new List<float[]> { id };
        }

        public void LoadFromFile(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                ClassName = file.ReadLine();
                try
                {
                    ClassNameToId(ClassName);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + ", 파일이름: " + filePath);
                }
            }
        }

        internal abstract int ClassNameToId(string className);
    }

    class ClassifierNet : NeuralNetwork
    {
        public ClassifierNet(IList<int> deviceIds, int inputWidth, int inputHeight, int inputChannel,
            int classNumber)
            : base(deviceIds)
        {
            InputWidth = inputWidth;
            InputWidth = inputHeight;
            InputWidth = inputChannel;
            ClassNumber = classNumber;
        }

        public int InputWidth { get; private set; } = -1;
        public int InputHeight { get; private set; } = -1;
        public int InputChannel { get; private set; } = -1;
        public int ClassNumber { get; private set; } = -1;
        public NDShape InputShape { get => new int[] { InputChannel, InputWidth, InputHeight }; }
        public NDShape LabelShape { get => new int[] { ClassNumber }; }
        internal Variable Input { get; set; } = null;
        internal Variable Label { get; set; } = null;
        public IList<float> ClassBalanceVector { get; private set; } = null;


        protected override Function BuildLoss()
        {
            Label = CNTKLib.InputVariable(LabelShape, DataType.Float, "label");
            Variable labelOneHot = CNTKLib.Reshape(OneHot(Label, ClassNumber, new Axis(0)), LabelShape);
            return CrossEntropyLoss(Model.Output, labelOneHot, new Axis(0), ClassBalanceVector?.ToArray(), name: "loss");
        }

        protected override Function BuildModel(string modelPath)
        {
            if (modelPath != "")
                return Function.Load(modelPath, Device[0]);
            else
                return BuildResnet();
        }

        protected override Trainer BuildTrainer()
        {
            return CNTKLib.CreateTrainer(Model, Loss, new LearnerVector() { Adam(Model, 1E-05) });
        }

        internal Function BuildInceptionV4()
        {
            Input = CNTKLib.InputVariable(InputShape, DataType.Float, "input");
            Variable inputCHW = TransposeAxes(TransposeAxes(Input, 0, 2), 0, 1); // HWC 포맷 -> CHW 포맷
            Variable inputScaled = CNTKLib.ElementTimes(Constant.Scalar(0.00390625f, Device[0]), inputCHW); // 정규화

            var local = InceptionStem(inputScaled);
            local = InceptionBlockA(local);
            local = InceptionBlockA(local);
            local = InceptionBlockA(local);
            local = InceptionBlockA(local);
            local = InceptionReductionA(local);
            local = InceptionBlockB(local);
            local = InceptionBlockB(local);
            local = InceptionBlockB(local);
            local = InceptionBlockB(local);
            local = InceptionBlockB(local);
            local = InceptionBlockB(local);
            local = InceptionBlockB(local);
            local = InceptionReductionB(local);
            local = InceptionBlockC(local);
            local = InceptionBlockC(local);
            local = InceptionBlockC(local, name:"output");
            return local;
        }

        internal Function BuildResnet()
        {
            Input = CNTKLib.InputVariable(InputShape, DataType.Float, "input");
            Variable inputCHW = TransposeAxes(TransposeAxes(Input, 0, 2), 0, 1); // HWC 포맷 -> CHW 포맷
            Variable inputScaled = CNTKLib.ElementTimes(Constant.Scalar(0.00390625f, Device[0]), inputCHW); // 정규화

            var local = Conv(inputScaled, inputScaled.Shape[2], 32);
            local = ResBlock(local);
            local = MaxPool(Conv(ReLU(BN(local, true)), 32, 64)); // 128 x 128
            local = ResBlock(local);
            local = MaxPool(Conv(ReLU(BN(local, true)), 64, 128)); // 64 x 64
            local = ResBlock(local);
            local = MaxPool(Conv(ReLU(BN(local, true)), 128, 256)); // 32 x 32
            local = ResBlock(local);
            local = MaxPool(Conv(ReLU(BN(local, true)), 256, 512)); // 16 x 16
            local = ResBlock(local);
            local = MaxPool(Conv(ReLU(BN(local, true)), 512, 1024)); // 8 x 8
            local = ResBlock(local);
            local = ResBlock(local);
            local = MaxPool(Conv(ReLU(BN(local, true)), 1024, 2048)); // 4 x 4
            local = ResBlock(local);
            local = ResBlock(local);
            local = AvgPool(Conv(ReLU(BN(local, true)), 2048, 4096), 4, 4); // 1 x 1
            local = FC(local, 4096, 4096);
            local = FC(local, 4096, ClassNumber);
            local = Softmax(ReLU(BN(local, true)), new Axis(0), "output");
            return local;
        }
    }
}
