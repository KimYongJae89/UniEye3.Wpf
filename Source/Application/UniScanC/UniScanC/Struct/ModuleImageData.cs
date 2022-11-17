using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;

namespace UniScanC.Struct
{
    public class ImageData : InputOutputs<AlgoImage, Size>, IResultBufferItem
    {
        public AlgoImage Image { get => Item1; set => Item1 = value; }
        public Size Size { get => Item2; set => Item2 = value; }

        public ImageData() : base("Image", "Size")
        {
            SetValues(null, Size.Empty);
        }

        public ImageData(AlgoImage image, Size size) : this()
        {
            SetValues(image, size);
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            return true;
        }

        public void Return(InspectBufferPool bufferPool)
        {
        }

        public void CopyFrom(IResultBufferItem from)
        {
            var imageData = (ImageData)from;
            Image = imageData.Image.Clone();
            Size = imageData.Size;
        }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
        }
    }

    public class ImageDataByte : InputOutputs<byte[], Size>, IResultBufferItem
    {
        private DynMvp.Base.Image2D image2D = null; // 복사 전 이미지 데이터

        public byte[] Data { get => Item1; set => Item1 = value; }
        public Size Size { get => Item2; set => Item2 = value; }

        private ImageDataByte() : base("Data", "Size")
        {
            SetValues(null, Size.Empty);
        }

        private ImageDataByte(byte[] data, Size size) : this()
        {
            SetValues(data, size);
        }

        public ImageDataByte(DynMvp.Base.Image2D image2D, Size size) : base("Data", "Size")
        {
            SetValues(null, size);
            this.image2D = image2D;
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            if (image2D != null && Data == null)
            {
                byte[] bytes = bufferPool.RequestBytesBuffer();
                if (bytes == null)
                {
                    return false;
                }

                if (image2D.DataPtr == IntPtr.Zero)
                {
                    if (image2D.Data.Length == bytes.Length)
                    {
                        Buffer.BlockCopy(image2D.Data, 0, bytes, 0, image2D.Data.Length);
                    }
                    else
                    {
                        int pitch = (int)Math.Ceiling(Size.Width / 4.0) * 4;
                        int heigth = Math.Min(Size.Height, bufferPool.Size.Height);
                        for (int y = 0; y < heigth; y++)
                        {
                            Buffer.BlockCopy(image2D.Data, image2D.Pitch * y, bytes, Size.Width * y, Size.Width);
                        }
                    }
                }
                else
                {
                    //data = new byte[image.Width * image.Height];
                    var sw = Stopwatch.StartNew();
                    if (false)
                    // 900ms 이상 소요됨.
                    {
                        //for (var h = 0; h < image2D.Height; ++h)
                        //    Marshal.Copy(IntPtr.Add(image2D.DataPtr, image2D.Pitch * h), bytes, image2D.Width * h, image2D.Width);// 100ms
                    }
                    else
                    {
                        Marshal.Copy(image2D.DataPtr, bytes, 0, bytes.Length);
                    }
                    sw.Stop();
                    LogHelper.Debug(LoggerType.Inspection, $"ImageDataByte::Request - Marshal.Copy: {sw.ElapsedMilliseconds}[ms]");
                }
                SetValue<Byte[]>("Data", bytes);
                LogHelper.Debug(LoggerType.Inspection, $"ImageDataByte::Request - Data Copied");
            }
            return true;
        }

        public void Return(InspectBufferPool bufferPool)
        {
            bufferPool.ReturnBytesBuffer(Data);
            SetValue<byte[]>("Data", null);
        }

        public void CopyFrom(IResultBufferItem from)
        {
            var imageDataByte = (ImageDataByte)from;
            Data = (byte[])imageDataByte.Data.Clone();
            Size = imageDataByte.Size;
        }

        public void SaveDebugInfo(DebugContextC debugContext)
        {
        }
    }

    [AlgorithmBaseParam]
    public class ModuleImageDataByte : InputOutputs<ImageDataByte, int>, IResultBufferItem
    {
        public ImageDataByte ImageDataByte { get => Item1; set => Item1 = value; }
        public int FrameNo { get => Item2; set => Item2 = value; }

        public ModuleImageDataByte() : base("ImageDataByte", "FrameNo") { }

        public ModuleImageDataByte(ImageDataByte imageDataByte, int FrameNo) : this()
        {
            SetValues(imageDataByte, FrameNo);
        }

        public static string[] GetPropNames()
        {
            return new string[] { "ImageDataByte", "FrameNo" };
        }

        public static (string, Type)[] GetProps()
        {
            return new (string, Type)[] { ("ImageDataByte", typeof(ImageDataByte)), ("FrameNo", typeof(int)) };
        }

        public bool Request(InspectBufferPool bufferPool)
        {
            return ImageDataByte.Request(bufferPool);
        }

        public void Return(InspectBufferPool bufferPool)
        {
            ImageDataByte.Return(bufferPool);
        }

        public void CopyFrom(IResultBufferItem from)
        {
            var data = (ModuleImageDataByte)from;
            ImageDataByte.CopyFrom(data.ImageDataByte);
            FrameNo = data.FrameNo;
        }

        public void SaveDebugInfo(DebugContextC debugContext) { }
    }
}
