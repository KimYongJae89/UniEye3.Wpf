using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;

namespace UniScanC.Struct
{
    public interface IResultBufferItem : TupleElement
    {
        bool Request(AlgoTask.InspectBufferPool bufferPool);

        void Return(AlgoTask.InspectBufferPool bufferPool);

        void CopyFrom(IResultBufferItem from);

        void SaveDebugInfo(DebugContextC debugContext);
    }

    public class InspectBufferSet
    {
        private Dictionary<string, IResultBufferItem> tempDic;

        //public byte[] RawData; // 카메라 그랩 데이터

        // for Debug...
        public Size RawDataSize => tempDic["ModuleImageDataByte"].GetValue<ImageDataByte>(0).Size;
        public int FrameNo => tempDic["ModuleImageDataByte"].GetValue<int>(1);

        public AlgoImage SrcBuffer => tempDic["LineCalibrator"].GetValue<ImageData>(0).Image; // 원본 이미지
        public AlgoImage MaskBuffer => tempDic["PatternSizeChecker"].GetValue<RoiMask>(0).Mask; // 마스킹 이미지
        public AlgoImage LabelBuffer => tempDic["PlainFilmChecker"].GetValue<AlgoImage>(0);  // 라벨링 이미지 버퍼

        //public AlgoImage RoiBuffer; // Roi 영역 드로잉 버퍼 (디버깅용)

        public RectangleF[] RoiRectangles => tempDic["PatternSizeChecker"].GetValue<RoiMask>(0).ROIs;
        public SizeF PatternSize => tempDic["PatternSizeChecker"].GetValue<SizeF>(1);

        public List<Defect> DefectListColor => tempDic["ColorChecker"].GetValue<List<Defect>>(1);
        public List<Defect> DefectList => tempDic["PlainFilmChecker"].GetValue<List<Defect>>(1);

        public (byte min, byte max, float average, float stdDev)[] Statistics = new (byte, byte, float, float)[0];

        public float[] colorDiffs => tempDic["PlainFilmChecker"].GetValue<float[]>(0);

        //public InspectBufferSet(byte[] RawData, Size RawDataSize, int FrameNo)
        //{
        //    this.tempDic = new Dictionary<string, IResultBufferItem >();
        //    this.tempDic.Add(typeof(ModuleImageDataByte).Name, new ModuleImageDataByte(new ImageDataByte(RawData, RawDataSize), FrameNo));
        //}

        public InspectBufferSet(ImageDataByte imageDataByte, int frameNo)
        {
            tempDic = new Dictionary<string, IResultBufferItem>();
            tempDic.Add(typeof(ModuleImageDataByte).Name, new ModuleImageDataByte(imageDataByte, frameNo));
        }

        public IResultBufferItem GetTaskResult(int taskNo)
        {
            if (taskNo < 0)
            {
                taskNo = -1;
            }

            string taskName = tempDic.ElementAt(taskNo).Key;
            return GetTaskResult(taskName);
        }

        public IResultBufferItem GetTaskResult(string taskName)
        {
            return tempDic[taskName];
        }

        public void SetTaskResult(int taskNo, IResultBufferItem item)
        {
            string taskName = tempDic.ElementAt(taskNo).Key;
            SetTaskResult(taskName, item);
        }

        public void SetTaskResult(string taskName, IResultBufferItem item)
        {
            if (!tempDic.ContainsKey(taskName))
            {
                tempDic.Add(taskName, item);
                return;
            }
            IResultBufferItem existItem = tempDic[taskName];
            existItem.CopyFrom(item);
        }

        public void Save(string path)
        {
            var debugContext = new DebugContextC(true, path, FrameNo);
            foreach (KeyValuePair<string, IResultBufferItem> pair in tempDic)
            {
                pair.Value.SaveDebugInfo(debugContext);
            }

            //this.SrcBuffer.Save($"{FrameNo}.bmp", new DebugContext(true, Path.Combine(path, "SrcBuffers")));
            //this.MaskBuffer.Save($"{FrameNo}.bmp", new DebugContext(true, Path.Combine(path, "MaskBuffers")));
            //this.LabelBuffer.Save($"{FrameNo}.bmp", new DebugContext(true, Path.Combine(path, "LabelBuffer")));

            using (AlgoImage algoImage = SrcBuffer.Clone())
            {
                algoImage.Clear();
                ImageProcessing ip = ImageProcessingFactory.CreateImageProcessing(algoImage.LibraryType);
                RoiMask roiMask = null;
                if (tempDic.ContainsKey("PatternSizeChecker"))
                {
                    roiMask = tempDic["PatternSizeChecker"].GetValue<RoiMask>(0);
                }
                else if (tempDic.ContainsKey("RoiFinder"))
                {
                    roiMask = tempDic["RoiFinder"].GetValue<RoiMask>(0);
                }

                if (roiMask != null)
                {
                    RectangleF[] rois = roiMask.ROIs;
                    Array.ForEach(rois, f => ip.DrawRect(algoImage, Rectangle.Round(f), 255, true));
                }
                algoImage.Save($"{FrameNo}.bmp", new DebugContext(true, Path.Combine(path, "RoiBuffers")));
            }
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            bool[] isGood = new bool[tempDic.Count];
            for (int i = 0; i < tempDic.Count; i++)
            {
                isGood[i] = tempDic.ElementAt(i).Value.Request(bufferPool);
            }

            if (!Array.TrueForAll(isGood, f => f))
            {
                ReturnAll(bufferPool);
                return false;
            }
            return true;
        }

        public void ReturnAll(InspectBufferPool bufferPool)
        {
            for (int i = 0; i < tempDic.Count; i++)
            {
                tempDic.ElementAt(i).Value.Return(bufferPool);
            }

            tempDic.Clear();
        }
    }
}
