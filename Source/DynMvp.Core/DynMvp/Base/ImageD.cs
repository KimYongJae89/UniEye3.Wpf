using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class ImageBase<T>
    {
        private T[] data;
        public T[] Data => data;
        public IntPtr DataPtr { get; private set; } = IntPtr.Zero;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Pitch { get; private set; }
        public int NumBand { get; private set; }
        public int DataSize { get; private set; }

        public void Initialize(int width, int height, int numBand, int pitch = 0, T[] data = null)
        {
            Width = width;
            Height = height;
            NumBand = numBand;
            Pitch = pitch;
            if (pitch == 0)
            {
                Pitch = (width * numBand + 3) / 4 * 4;   // 4바이트 배수
            }

            if (data != null)
            {
                this.data = (T[])data.Clone();
            }
            else
            {
                this.data = new T[Pitch * Height];
            }

            DataSize = System.Runtime.InteropServices.Marshal.SizeOf(this.data[0]);
        }

        public void Initialize(int width, int height, int numBand, int pitch, IntPtr dataPtr)
        {
            Width = width;
            Height = height;
            NumBand = numBand;
            if (pitch == 0)
            {
                Pitch = width * numBand;
            }
            else
            {
                Pitch = pitch;
            }

            DataPtr = dataPtr;

            var testData = new T[1];
            DataSize = System.Runtime.InteropServices.Marshal.SizeOf(testData[0]);
        }

        public void SetData(T[] data)
        {
            this.data = data;
        }

        public void SetDataPtr(IntPtr dataPtr)
        {
            DataPtr = dataPtr;
        }

        public void Free()
        {
            if (data != null)
            {
                Array.Resize(ref data, 0);
            }
            data = null;
            DataPtr = IntPtr.Zero;
            DataSize = 0;
        }

        public void Clear()
        {
            Array.Clear(data, 0, data.Length);
        }

        public ImageBase<T> GetLayer(int index)
        {
            var imageData = new ImageBase<T>();
            imageData.Initialize(Width, Height, 1);

            int size = Pitch * Height;
            Buffer.BlockCopy(data, size * index, imageData.Data, 0, size);

            return imageData;
        }

        public void Copy(T[] dataSrc)
        {
            int srcSize = dataSrc.Count() * DataSize;

            if (data == null || data.Count() != srcSize)
            {
                data = new T[srcSize];
            }

            Buffer.BlockCopy(dataSrc, 0, data, 0, srcSize * DataSize);
        }

        public void Copy(T[] dataSrc, Rectangle srcRect, int srcPitch, Point destPt)
        {
            for (int y = 0; y < srcRect.Height; y++)
            {
                Buffer.BlockCopy(dataSrc, (y + srcRect.Y) * srcPitch + srcRect.X * NumBand, data, (destPt.Y + y) * Pitch + destPt.X * NumBand, srcRect.Width * NumBand);
            }
        }

        public void Save(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Create);
            var binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(Width);
            binaryWriter.Write(Height);
            binaryWriter.Write(Pitch);
            binaryWriter.Write(NumBand);
            byte[] byteReserved = new byte[1000];

            byte[] byteBuffer = new byte[data.Length * DataSize];
            Buffer.BlockCopy(data, 0, byteBuffer, 0, byteBuffer.Length);

            binaryWriter.Write(byteBuffer, 0, byteBuffer.Length);
            fileStream.Close();
        }

        public void SaveRaw(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Create);
            var binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(Width);
            binaryWriter.Write(Height);
            binaryWriter.Write(NumBand);

            byte[] byteBuffer = new byte[data.Length * DataSize];
            Buffer.BlockCopy(data, 0, byteBuffer, 0, byteBuffer.Length);

            binaryWriter.Write(byteBuffer, 0, byteBuffer.Length);
        }

        public void Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                return;
            }

            var fileStream = new FileStream(fileName, FileMode.Open);
            var binaryReader = new BinaryReader(fileStream);

            Width = binaryReader.ReadInt32();
            Height = binaryReader.ReadInt32();
            Pitch = binaryReader.ReadInt32();
            NumBand = binaryReader.ReadInt32();
            //            byte[] byteReserved = binaryReader.ReadBytes(1000);

            data = new T[Width * Height * NumBand];
            DataSize = System.Runtime.InteropServices.Marshal.SizeOf(data[0]);

            int size = Width * Height * NumBand * DataSize;
            byte[] byteBuffer = binaryReader.ReadBytes(size);

            Buffer.BlockCopy(byteBuffer, 0, data, 0, size);

            DataPtr = System.Runtime.InteropServices.GCHandle.Alloc(data, GCHandleType.Pinned).AddrOfPinnedObject();

            binaryReader.Dispose();
            fileStream.Dispose();
        }

        public float[] GetRangeValue(Point point, int range = 5)
        {
            float[] sum = new float[NumBand];

            int count = 0;

            try
            {
                for (int y = point.Y - range; y <= point.Y + range; y++)
                {
                    for (int x = point.X - range; x <= point.X + range; x++)
                    {
                        int index = (y * Pitch) + x * NumBand;

                        sum[0] += float.Parse(data[index].ToString());
                        count++;

                        if (NumBand == 3)
                        {
                            sum[1] += float.Parse(data[index + 1].ToString());
                            sum[2] += float.Parse(data[index + 2].ToString());
                        }
                    }
                }
            }
            catch (FormatException)
            {

            }

            for (int i = 0; i < NumBand; i++)
            {
                sum[i] /= count;
            }

            return sum;
        }

        public void Clip(ImageBase<T> destImageData, Rectangle rectangle)
        {
            int xStart = (rectangle.Left < 0 ? 0 : rectangle.Left);
            int yStart = (rectangle.Top < 0 ? 0 : rectangle.Top);
            int xEnd = (rectangle.Right > Width ? Width : rectangle.Right);
            int yEnd = (rectangle.Bottom > Height ? Height : rectangle.Bottom);
            int realWidth = Math.Min(rectangle.Width, xEnd - xStart) * NumBand * DataSize;
            Parallel.For(yStart, yEnd, y =>
            {
                int srcIndex = y * Pitch + xStart * NumBand;
                int destIndex = (y - yStart) * destImageData.Pitch;
                Array.Copy(data, srcIndex, destImageData.Data, destIndex, realWidth);
            });
        }
    }

    public abstract class ImageD : IDisposable
    {
        public object Tag { get; set; }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract int Pitch { get; }
        public abstract int NumBand { get; }
        public abstract int DataSize { get; }

        public Size Size => new Size(Width, Height);
        public Rectangle Roi { get; set; } = Rectangle.Empty;

        public abstract void Initialize(int width, int height, int numBand, int pitch = 0);

        public abstract ImageD Clone();

        public abstract void CopyFrom(ImageD imageSrc);
        public abstract void CopyFrom(ImageD imageSrc, Rectangle srcRect, int srcPitch, Point destPt);
        public abstract Bitmap ToBitmap();
        public abstract void Clear();

        public abstract void ConvertFromDataPtr(int srcPitch = 0);
        public abstract void ConvertFromData();

        public abstract ImageD GetLayer(int index);

        public abstract void Save(string fileName);
        public abstract void Load(string fileName);

        public abstract void SaveImage(string fileName, ImageFormat imageFormat);
        public abstract void LoadImage(string fileName);

        public abstract ImageD ClipImage(RotatedRect rotatedRect);
        public abstract ImageD ClipImage(Rectangle rectangle);

        public abstract float GetAverage();
        public abstract float GetMax();
        public abstract float GetMin();

        public abstract ImageD Not();
        public abstract ImageD FlipX();
        public abstract ImageD FlipY();
        public abstract ImageD FlipXY();
        public abstract void RotateFlip(RotateFlipType rotateFlipType);

        public bool IsSame(ImageD imageD)
        {
            return imageD.Width == Width && imageD.Height == Height &&/* imageD.Pitch == Pitch &&*/ imageD.NumBand == NumBand && imageD.DataSize == DataSize;
        }

        //        public abstract float GetValue(Point point);
        public abstract float[] GetRangeValue(Point point, int range = 5);
        public abstract ImageD Resize(int destWidth, int destHeight);

        protected abstract void FreeImageData();

        protected virtual void Dispose(bool disposing)
        {
            //    if (!disposedValue)
            //    {
            if (disposing)
            {
                FreeImageData();
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            // set large fields to null.

            //    disposedValue = true;
            //}
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageD() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
    }


    public class Image2D : ImageD
    {
        private const int NUM_COLOR_BAND = 3;

        public ImageBase<byte> ImageData { get; private set; } = null;

        public override int Width => ImageData.Width;
        public override int Height => ImageData.Height;
        public override int Pitch => ImageData.Pitch;
        public override int NumBand => ImageData.NumBand;
        public override int DataSize => ImageData.DataSize;

        public byte[] Data => ImageData.Data;
        public IntPtr DataPtr => ImageData.DataPtr;

        public override void ConvertFromDataPtr(int srcPitch = 0)
        {
            if (ImageData.DataPtr != IntPtr.Zero)
            {
                if (srcPitch == 0)
                {
                    srcPitch = ImageData.Pitch;
                }

                int size = ImageData.Pitch * ImageData.Height;
                byte[] data = new byte[size];

                if (srcPitch == ImageData.Pitch)
                {
                    Marshal.Copy(ImageData.DataPtr, data, 0, size);
                }
                else
                {
                    for (int y = 0; y < ImageData.Height; y++)
                    {
                        int ptrOffset = y * srcPitch;
                        int idxOffset = y * ImageData.Pitch;
                        Marshal.Copy(ImageData.DataPtr + ptrOffset, data, idxOffset, ImageData.Pitch);
                    }
                }

                ImageData.SetData(data);
                ImageData.SetDataPtr(IntPtr.Zero);
            }
        }

        public override void ConvertFromData()
        {
            if (ImageData.Data != null)
            {
                var gcHandle = GCHandle.Alloc(ImageData.Data, GCHandleType.Pinned);
                ImageData.SetDataPtr(gcHandle.AddrOfPinnedObject());
                gcHandle.Free();
            }
        }

        public Image2D()
        {

        }

        public Image2D(int width, int height, int numBand, int pitch = 0)
        {
            Initialize(width, height, numBand, pitch);
        }

        public Image2D(string fileName)
        {
            if (File.Exists(fileName))
            {
                LoadImage(fileName);
            }
        }

        public override void Initialize(int width, int height, int numBand, int pitch = 0)
        {
            ImageData = new ImageBase<byte>();
            ImageData.Initialize(width, height, numBand, pitch);
        }

        public void Initialize(int width, int height, int numBand, int pitch, byte[] data)
        {
            ImageData = new ImageBase<byte>();
            ImageData.Initialize(width, height, numBand, pitch, data);
        }

        public void Initialize(int width, int height, int numBand, int pitch, IntPtr data)
        {
            ImageData = new ImageBase<byte>();
            ImageData.Initialize(width, height, numBand, pitch, data);
        }

        public bool IsUseIntPtr()
        {
            return ImageData.DataPtr != IntPtr.Zero;
        }

        protected override void FreeImageData()
        {
            ImageData.Free();
            ImageData = null;
        }

        public override ImageD Clone()
        {
            var cloneImage = new Image2D();
            if (ImageData.DataPtr != null)
            {
                cloneImage.Initialize(Width, Height, NumBand, Pitch, ImageData.DataPtr);
            }
            else
            {
                cloneImage.Initialize(Width, Height, NumBand, Pitch);
            }

            if (ImageData.Data != null)
            {
                cloneImage.ImageData.Copy(ImageData.Data);
            }

            cloneImage.Tag = Tag;

            return cloneImage;
        }

        public override void CopyFrom(ImageD imageSrc)
        {
            Debug.Assert(imageSrc is Image2D);
            Debug.Assert(IsSame(imageSrc));

            var imageSrc2d = (Image2D)imageSrc;

            if (Width != imageSrc2d.Width || Height != imageSrc2d.Height)
            {
                LogHelper.Warn(LoggerType.Operation, StringManager.GetString("Image Data size is not match."));
                return;
            }

            if (imageSrc2d.Pitch != Pitch)
            {
                ImageData.Initialize(Width, Height, NumBand, imageSrc2d.Pitch, null);
            }

            ImageData.SetDataPtr(imageSrc2d.DataPtr);
            if (imageSrc2d.Data != null)
            {
                ImageData.Copy(imageSrc2d.Data);

                imageSrc2d.FreeImageData();
            }
        }

        public override void CopyFrom(ImageD imageSrc, Rectangle srcRect, int srcPitch, Point destPt)
        {
            var imageSrc2d = (Image2D)imageSrc;
            if (ImageData.NumBand == imageSrc2d.NumBand)
            {
                ImageData.Copy(imageSrc2d.Data, srcRect, srcPitch, destPt);
            }
            else if (ImageData.NumBand == 3)
            {
                var colorImage = (Image2D)imageSrc2d.GetColorImage();
                ImageData.Copy(colorImage.Data, srcRect, colorImage.Pitch, destPt);
            }
            else
            {
                Image2D grayImage = imageSrc2d.GetGrayImage();
                ImageData.Copy(grayImage.Data, srcRect, grayImage.Pitch, destPt);
            }
        }

        public void SetData(IntPtr srcPtr)
        {
            if (ImageData != null)
            {
                Marshal.Copy(srcPtr, ImageData.Data, 0, Pitch * Height);
            }
        }

        public void SetDataPtr(IntPtr srcPtr)
        {
            if (ImageData != null)
            {
                ImageData.SetDataPtr(srcPtr);
            }
        }

        public override ImageD GetLayer(int index)
        {
            var layerImage = new Image2D();
            layerImage.Initialize(Width, Height, 1);
            layerImage.SetData(ImageData.GetLayer(index).Data);

            return layerImage;
        }

        public void SetData(byte[] dataSrc)
        {
            if (ImageData != null)
            {
                ImageData.Copy(dataSrc);
            }
        }

        public override void Clear()
        {
            if (ImageData != null)
            {
                ImageData.Clear();
            }
        }

        public override Bitmap ToBitmap()
        {
            if (ImageData == null)
            {
                return null;
            }

            Bitmap bitMap = null;

            if (IsUseIntPtr() == true)
            {
                bitMap = ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, DataPtr);
            }
            else
            {
                bitMap = ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, Data);
            }

            return bitMap;
        }

        public static Image2D FromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }

            int numBand = 1;

            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                numBand = 4;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                numBand = 3;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                numBand = 1;
            }
            else
            {
                //Debug.Assert(false, StringManager.GetString(this.GetType().FullName, "DynMvp is support only 8bit or 24bit bitmap"));
                return null;
            }

            //if ((stride % 4) != 0)
            //    stride = stride + (4 - stride % 4);//4바이트 배수가 아닐시..

            var image2d = new Image2D();
            image2d.Initialize(bitmap.Width, bitmap.Height, numBand);

            var srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData srcBmpData = bitmap.LockBits(srcRect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;
            //image2d.Initialize(bitmap.Width, bitmap.Height, numBand, stride, srcPtr);
            image2d.SetData(srcBmpData.Scan0);
            image2d.ConvertFromDataPtr(srcBmpData.Stride); //여기시 deep-copy

            bitmap.UnlockBits(srcBmpData);

            return image2d;
        }

        public static Image2D ToImage2D(string fileName)
        {
            return new Image2D(fileName);
        }

        public static Image2D ToImage2D(Bitmap bitmap, bool usePtr = true)
        {
            Debug.Assert(bitmap != null);

            int numBand = 1;
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                numBand = 4;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                numBand = 3;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                numBand = 1;
            }
            else
            {
                Debug.Assert(false, "The image format [{0}] is not supported.", bitmap.PixelFormat.ToString());
                return null;
            }

            var srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData srcBmpData = bitmap.LockBits(srcRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var image2d = new Image2D();
            if (usePtr)
            {
                image2d.Initialize(srcBmpData.Width, srcBmpData.Height, numBand, srcBmpData.Stride, srcBmpData.Scan0);
            }
            else
            {
                byte[] imageData = new byte[srcBmpData.Stride * srcBmpData.Height * numBand];
                Marshal.Copy(srcBmpData.Scan0, imageData, 0, imageData.Length);
                image2d.Initialize(srcBmpData.Width, srcBmpData.Height, numBand, srcBmpData.Stride, imageData);
            }

            bitmap.UnlockBits(srcBmpData);

            return image2d;
        }

        public override void SaveImage(string fileName, ImageFormat imageFormat)
        {
            Debug.Assert(ImageData != null);

            Bitmap bitmap = ToBitmap();
            ImageHelper.SaveImage(bitmap, fileName, imageFormat);
            bitmap.Dispose();
        }

        public override void LoadImage(string fileName)
        {
            LogHelper.Debug(LoggerType.Function, string.Format("Image2D::LoadImage - {0}", fileName));

            bool ok = false;

            var sr = new StreamReader(fileName);
            string extention = Path.GetExtension(fileName).ToLower();
            if (extention == ".bmp")
            {
                // Support Extrime Size
                long headerSize = 54;//sr.BaseStream.Length;
                sr.BaseStream.Position = 0;
                byte[] headerData = new byte[headerSize];
                sr.BaseStream.Read(headerData, 0, headerData.Length);

                if (headerData[0] == 'B' && headerData[1] == 'M')
                {
                    int width = headerData[21] << 24 | headerData[20] << 16 | headerData[19] << 8 | headerData[18];
                    int height = headerData[25] << 24 | headerData[24] << 16 | headerData[23] << 8 | headerData[22];
                    int bpp = headerData[29] << 8 | headerData[28];
                    int pitch = ((width * bpp + 31) / 32) * 4;
                    //pitch = (width * bpp / 8);

                    int dataStartIdx = 54 + (bpp == 8 ? 4 * 256 : 0);
                    long dataSize = sr.BaseStream.Length - dataStartIdx;
                    int imageSize = pitch * height;
                    byte[] imageData = new byte[imageSize];

                    sr.BaseStream.Position = dataStartIdx;
                    for (int h = 0; h < height; h++)
                    {
                        int offset = (height - 1 - h) * pitch;
                        //offset = h * pitch;
                        sr.BaseStream.Read(imageData, offset, pitch);
                    }

                    ImageData = new ImageBase<byte>();
                    ImageData.Initialize(width, height, bpp / 8, pitch, imageData);

                    ok = true;
                }
            }

            if (ok == false)
            {
                // Old Code
                var bitmap = Image.FromStream(sr.BaseStream, true, false) as Bitmap;
                //Bitmap bitmap = (Bitmap)Image.FromFile(fileName, false);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;

                ImageData = new ImageBase<byte>();
                ImageData.Initialize(bitmap.Width, bitmap.Height, bytesPerPixel);
                ImageData.SetData(ImageHelper.GetByte(bitmap));
                bitmap.Dispose();
            }

            sr.Close();
            //LogHelper.Debug(LoggerType.Function, "Image2D::LoadImage End");
        }

        public override void Save(string fileName)
        {
            ImageData.Save(fileName);
        }

        public override void Load(string fileName)
        {
            ImageData.Load(fileName);
        }

        public override ImageD ClipImage(RotatedRect rotatedRect)
        {
            var rect = Rectangle.Round(rotatedRect.GetBoundRect());
            var clipImage = (Image2D)ClipImage(rect);

            if (rotatedRect.Angle % 90 == 0)
            {
                return clipImage;
            }

            Image2D clipImage2 = clipImage.Rotate(-rotatedRect.Angle);
            var rect2 = Rectangle.Round(DrawingHelper.FromCenterSize(DrawingHelper.CenterPoint(rect), new SizeF(rotatedRect.Width, rotatedRect.Height)));
            var clipImage3 = (Image2D)ClipImage(rect2);

#if DEBUG == true
            //clipImage2.SaveImage(@"D:\ClipImage3.bmp", ImageFormat.Bmp);
            //clipImage3.SaveImage(@"D:\ClipImage4.bmp", ImageFormat.Bmp);
#endif
            return clipImage3;
        }

        public override ImageD ClipImage(Rectangle rectangle)
        {
            var imageRect = new Rectangle(0, 0, Width, Height);
            if (Rectangle.Intersect(imageRect, rectangle) != rectangle)
            {
                return null;
            }

            var image2d = new Image2D(rectangle.Width, rectangle.Height, NumBand);

            ImageData.Clip(image2d.ImageData, rectangle);

            return image2d;
        }

        public Image2D GetGrayImage()
        {
            if (NumBand == 1)
            {
                return (Image2D)Clone();
            }

            var grayImage = new Image2D(Width, Height, 1);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float rValue = (float)Data[y * Pitch + x * NumBand] / 255;
                    float gValue = (float)Data[y * Pitch + x * NumBand + 1] / 255;
                    float bValue = (float)Data[y * Pitch + x * NumBand + 2] / 255;

                    grayImage.Data[y * Width + x] = (byte)((0.299 * rValue + 0.587 * gValue + 0.114 * bValue) * 255);
                }
            }

            return grayImage;
        }

        public ImageD GetColorImage()
        {
            if (NumBand == NUM_COLOR_BAND)
            {
                return (Image2D)Clone();
            }

            var colorImage = new Image2D(Width, Height, NUM_COLOR_BAND);

            int colorImagePitch = colorImage.Pitch;
            Parallel.For(0, Height, y =>
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int k = 0; k < NUM_COLOR_BAND; k++)
                    {
                        colorImage.Data[y * colorImagePitch + x * NUM_COLOR_BAND + k] = Data[y * Pitch + x * NumBand];
                    }
                }
            });

            return colorImage;
        }

        public override float GetAverage()
        {
            if (NumBand != 1)
            {
                Debug.Assert(false, "[ ImageD.GetAverage ] Image format must be gray ");
                return 0;
            }

            return Data.Average(x => (float)x);
        }

        public override float GetMax()
        {
            if (NumBand != 1)
            {
                Debug.Assert(false, "[ ImageD.GetMax ] Image format must be gray ");
                return 0;
            }

            return Data.Max();
        }

        public override float GetMin()
        {
            if (NumBand != 1)
            {
                Debug.Assert(false, "[ ImageD.GetMin ] Image format must be gray ");
                return 0;
            }

            return Data.Min();
        }

        public override ImageD Not()
        {
            var cloneImage = (Image2D)Clone();

            if (NumBand != 1)
            {
                Debug.Assert(false, "[ ImageD.Not ] Image format must be gray ");
                return cloneImage;
            }

            Parallel.For(0, cloneImage.DataSize, index => cloneImage.Data[index] = (byte)(255 - cloneImage.Data[index]));

            return cloneImage;
        }

        public override float[] GetRangeValue(Point point, int range = 5)
        {
            return ImageData.GetRangeValue(point, range);
        }

        private float GetValue(PointF point)
        {
            int xStep = 0, yStep = 0;
            var ptLT = new Point((int)point.X, (int)point.X);
            if (point.X < Width)
            {
                xStep = 1;
            }

            if (point.Y < Height)
            {
                yStep = 1;
            }

            float ltValue = Data[ptLT.Y * Width + ptLT.X];
            float rtValue = Data[ptLT.Y * Width + ptLT.X + xStep];
            float lbValue = Data[(ptLT.Y + yStep) * Width + ptLT.X];
            float rbValue = Data[(ptLT.Y + yStep) * Width + ptLT.X + xStep];

            float topValue = ltValue + (rtValue - ltValue) * (point.X - (int)point.X);
            float bottomValue = lbValue + (rbValue - lbValue) * (point.X - (int)point.X);

            return topValue + (bottomValue - topValue) * (point.Y - (int)point.Y);
        }

        public override ImageD Resize(int destWidth, int destHeight)
        {
            var resizeImage = new Image2D(destWidth, destHeight, NumBand);

            for (int y = 0; y < destHeight; y++)
            {
                for (int x = 0; x < destWidth; x++)
                {
                    var point = new PointF((float)x / destWidth * Width, (float)y / destHeight * Height);
                    float value = GetValue(point);
                    resizeImage.Data[y * destWidth + x] = (byte)value;
                }
            }

            return resizeImage;
        }

        public override ImageD FlipX()
        {
            var flipImage = new Image2D(Width, Height, NumBand);

            Parallel.For(0, Height, y =>
            //for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int k = 0; k < NumBand; k++)
                    {
                        flipImage.Data[(y * Pitch) + ((Width - 1 - x) * NumBand) + k] = Data[(y * Pitch) + (x * NumBand) + k];
                    }
                }
            });

            return flipImage;
        }

        public override ImageD FlipY()
        {
            var flipImage = new Image2D(Width, Height, NumBand, Pitch);

            Parallel.For(0, Height, y =>
            {
                int srcOffset = y * Pitch;
                int dstOffset = (Height - y - 1) * Pitch;
                Buffer.BlockCopy(Data, srcOffset, flipImage.Data, dstOffset, Pitch);

            });

            return flipImage;
        }

        public override ImageD FlipXY()
        {
            var flipImage = new Image2D(Width, Height, NumBand);
            var flipImage_Temp = new Image2D(Width, Height, NumBand);

            Parallel.For(0, Height, y =>
            //for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int k = 0; k < NumBand; k++)
                    {
                        flipImage_Temp.Data[(y * Pitch) + ((Width - 1 - x) * NumBand) + k] = Data[(y * Pitch) + (x * NumBand) + k];
                    }
                }
            });

            Parallel.For(0, Height, y =>
            {
                int srcOffset = y * Pitch;
                int dstOffset = (Height - y - 1) * Pitch;
                Buffer.BlockCopy(flipImage_Temp.Data, srcOffset, flipImage.Data, dstOffset, Pitch);

            });

            return flipImage;
        }

        public override void RotateFlip(RotateFlipType rotateFlipType)
        {
            Image2D flipImage = null;
            switch (rotateFlipType)
            {
                case RotateFlipType.RotateNoneFlipX:
                    flipImage = (Image2D)FlipX();
                    break;
                case RotateFlipType.RotateNoneFlipY:
                    flipImage = (Image2D)FlipY();
                    break;
                case RotateFlipType.RotateNoneFlipXY:
                    flipImage = (Image2D)FlipXY();
                    break;
                case RotateFlipType.Rotate90FlipNone:
                    flipImage = Rotate(90);
                    break;
            }

            ImageData = flipImage.ImageData;
        }

        public Image2D Rotate(float angleDeg)
        {
            double angleRad = MathHelper.DegToRad(angleDeg);
            double Cos = Math.Cos(angleRad);
            double Sin = Math.Sin(angleRad);
            double Mcos = Math.Cos(-angleRad);
            double Msin = Math.Sin(-angleRad);

            var srcRotateCenter = new PointF(Width / 2.0f, Height / 2.0f);
            int width = (int)Math.Round(Width * Cos + Height * Sin);
            int height = (int)Math.Round(Height * Cos + Width * Sin);

            var destImage = new Image2D(width, height, NumBand);
            var dstRotateCenter = new PointF(destImage.Width / 2.0f, destImage.Height / 2.0f);

            for (int dstY = 0; dstY < destImage.Height; dstY++)
            {
                for (int dstX = 0; dstX < destImage.Width; dstX++)
                {
                    int dstIndex = dstY * destImage.Pitch + dstX * destImage.NumBand;

                    double srcDx = (dstX - dstRotateCenter.X) * Mcos + (dstY - dstRotateCenter.Y) * Msin + srcRotateCenter.X;
                    double srcDy = (dstY - dstRotateCenter.Y) * Mcos - (dstX - dstRotateCenter.X) * Msin + srcRotateCenter.Y;

                    int srcX = (int)srcDx;
                    int srcY = (int)srcDy;
                    int srcIndex = srcY * Pitch + srcX * NumBand;
                    if (srcX < 0 || srcX > Width - 1 || srcY < 0 || srcY > Height - 1)
                    {
                        continue;
                    }

                    double srcOx = 0;
                    int srcOxIndex = -1;
                    if (srcX < Width - 2)
                    {
                        srcOx = srcDx - srcX;
                        srcOxIndex = srcY * Pitch + (srcX + 1) * NumBand;
                    }

                    double srcOy = 0;
                    int srcOyIndex = -1;
                    if (srcY < Height - 2)
                    {
                        srcOy = srcDx - srcY;
                        srcOyIndex = (srcY + 1) * Pitch + srcX * NumBand;
                    }

                    for (int k = 0; k < destImage.NumBand; k++)
                    {
                        byte orgVal = ImageData.Data[srcIndex + k];
                        byte offsetX = orgVal;
                        if (srcOxIndex > 0)
                        {
                            offsetX = ImageData.Data[srcOxIndex + k];
                        }

                        byte offsetY = orgVal;
                        if (srcOyIndex > 0)
                        {
                            offsetY = ImageData.Data[srcOyIndex + k];
                        }

                        byte resVal = (byte)(orgVal + (offsetX - orgVal) * srcOx + (offsetX - offsetY) * srcOy);
                        destImage.Data[dstIndex + k] = resVal;
                    }
                }
            }
            return destImage;
        }
    }


    public class Image3D : ImageD
    {
        public ImageBase<float> ImageData { get; private set; } = null;
        public override int Width => ImageData.Width;
        public override int Height => ImageData.Height;
        public override int Pitch => ImageData.Pitch;
        public override int NumBand => ImageData.NumBand;
        public override int DataSize => ImageData.DataSize;
        public RectangleF MappingRect { get; set; }
        public Point3d[] PointArray { get; set; }

        public float[] Data => ImageData.Data;
        public IntPtr DataPtr => ImageData.DataPtr;

        public override void ConvertFromDataPtr(int srcPitch = 0)
        {
            if (ImageData.DataPtr != IntPtr.Zero)
            {
                float[] data = new float[ImageData.Pitch * ImageData.Height];
                Marshal.Copy(ImageData.DataPtr, data, 0, ImageData.Pitch * ImageData.Height);

                ImageData.SetData(data);
            }
        }

        public override void ConvertFromData()
        {
            throw new NotImplementedException();
        }

        public Image3D()
        {

        }

        public Image3D(string fileName)
        {
            Load(fileName);
        }

        public Image3D(int width, int height)
        {
            Initialize(width, height, 1);
        }

        public override void Initialize(int width, int height, int numBand, int pitch = 0)
        {
            ImageData = new ImageBase<float>();
            ImageData.Initialize(width, height, numBand, pitch);
        }

        protected override void FreeImageData()
        {
            ImageData.Free();
        }

        public override ImageD Clone()
        {
            var cloneImage = new Image3D();
            cloneImage.Initialize(Width, Height, NumBand, Pitch);
            cloneImage.CopyFrom(this);

            return cloneImage;
        }

        public override void CopyFrom(ImageD imageSrc)
        {
            Debug.Assert(imageSrc is Image3D);
            Debug.Assert(IsSame(imageSrc));

            var imageSrc3d = (Image3D)imageSrc;

            ImageData.Copy(imageSrc3d.Data);
            if (imageSrc3d.PointArray != null)
            {
                PointArray = (Point3d[])imageSrc3d.PointArray.Clone();
            }

            MappingRect = imageSrc3d.MappingRect;
        }

        public override void CopyFrom(ImageD imageSrc, Rectangle srcRect, int srcPitch, Point destPt)
        {
            Debug.Assert(imageSrc is Image3D);
            Debug.Assert(IsSame(imageSrc));

            var imageSrc3d = (Image3D)imageSrc;

            ImageData.Copy(imageSrc3d.Data);
            if (imageSrc3d.PointArray != null)
            {
                PointArray = (Point3d[])imageSrc3d.PointArray.Clone();
            }

            MappingRect = imageSrc3d.MappingRect;
        }

        public override void Clear()
        {
            ImageData.Clear();
        }

        public override ImageD GetLayer(int index)
        {
            var layerImage = new Image3D();
            layerImage.Initialize(Width, Height, 1);
            layerImage.SetData(ImageData.GetLayer(index).Data);

            return layerImage;
        }

        public void SetData(float[] dataSrc)
        {
            ImageData.Copy(dataSrc);
        }

        public void SetData(float[] dataSrc, float valueScale, float valueOffset)
        {
            Parallel.For(0, dataSrc.Count(), index => Data[index] = dataSrc[index] * valueScale + valueOffset);
        }

        public void SetData(byte[] dataSrc)
        {
            int size = Pitch * Height;
            Buffer.BlockCopy(dataSrc, 0, Data, 0, size);
        }

        public override Bitmap ToBitmap()
        {
            Debug.Assert(ImageData != null);

            byte[] byteData = ImageHelper.ConvertByteBuffer(Data);

            return ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, byteData);
        }

        public Image2D ToImage2D()
        {
            Debug.Assert(ImageData != null);

            byte[] byteData = ImageHelper.ConvertByteBuffer(Data);

            var image2d = new Image2D(Width, Height, NumBand, Pitch);
            image2d.SetData(byteData);

            return image2d;
        }

        public override void SaveImage(string fileName, ImageFormat imageFormat)
        {
            Debug.Assert(ImageData != null);

            Bitmap bitmap = ToBitmap();
            ImageHelper.SaveImage(bitmap, fileName, imageFormat);
            bitmap.Dispose();
        }

        public override void LoadImage(string fileName)
        {
            var bitmap = new Bitmap(fileName);

            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            byte[] byteImageData = ImageHelper.GetByte(bitmap);
            float[] floatImageData = new float[byteImageData.Count()];

            byteImageData.CopyTo(floatImageData, 0);

            ImageData = new ImageBase<float>();
            ImageData.Initialize(bitmap.Width, bitmap.Height, bytesPerPixel, bitmap.Width * bytesPerPixel, floatImageData);

            bitmap.Dispose();
        }

        public override void Save(string fileName)
        {
            ImageData.Save(fileName);
        }

        public override void Load(string fileName)
        {
            if (ImageData == null)
            {
                ImageData = new ImageBase<float>();
            }

            ImageData.Load(fileName);
        }

        public void SaveRaw(string fileName)
        {
            ImageData.SaveRaw(fileName);
        }

        public void LoadRaw(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                return;
            }

            var fileStream = new FileStream(fileName, FileMode.Open);
            var binaryReader = new BinaryReader(fileStream);

            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            int numBand = binaryReader.ReadInt32();

            ImageData = new ImageBase<float>();
            ImageData.Initialize(width, height, numBand);

            int size = width * height * numBand * ImageData.DataSize;
            byte[] byteBuffer = binaryReader.ReadBytes(size);

            Buffer.BlockCopy(byteBuffer, 0, Data, 0, size);
        }

        public override ImageD ClipImage(RotatedRect rotatedRect)
        {
            Bitmap tempImage = ToBitmap();
            Bitmap clipImage = ImageHelper.ClipImage(tempImage, rotatedRect);

            return Image2D.ToImage2D(clipImage);
        }

        public override ImageD ClipImage(Rectangle rectangle)
        {
            var image3d = new Image3D(rectangle.Width, rectangle.Height);

            for (int y = rectangle.Top; y < rectangle.Bottom; y++)
            {
                int srcIndex = y * Pitch + rectangle.Left;
                int destIndex = (y - rectangle.Top) * rectangle.Width;
                Array.Copy(Data, srcIndex, image3d.Data, destIndex, rectangle.Width);
            }

            return image3d;
        }

        public override float GetAverage()
        {
            return Data.Average();
        }

        public override float GetMax()
        {
            return Data.Max();
        }

        public override float GetMin()
        {
            return Data.Min();
        }

        public override ImageD Not()
        {
            Bitmap tempImage = ToBitmap();
            ImageHelper.Not(tempImage);

            return Image2D.ToImage2D(tempImage);
        }

        public override float[] GetRangeValue(Point point, int range = 5)
        {
            return ImageData.GetRangeValue(point, range);
        }

        private float GetValue(PointF point)
        {
            int xStep = 0, yStep = 0;
            var ptLT = new Point((int)point.X, (int)point.Y);
            if (point.X < (Width - 1))
            {
                xStep = 1;
            }

            if (point.Y < (Height - 1))
            {
                yStep = 1;
            }

            float ltValue = Data[ptLT.Y * Width + ptLT.X];
            float rtValue = Data[ptLT.Y * Width + ptLT.X + xStep];
            float lbValue = Data[(ptLT.Y + yStep) * Width + ptLT.X];
            float rbValue = Data[(ptLT.Y + yStep) * Width + ptLT.X + xStep];

            float topValue = ltValue + (rtValue - ltValue) * (point.X - (int)point.X);
            float bottomValue = lbValue + (rbValue - lbValue) * (point.X - (int)point.X);

            return topValue + (bottomValue - topValue) * (point.Y - (int)point.Y);
        }

        public override ImageD FlipX()
        {
            var flipImage = new Image3D(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    flipImage.Data[((Height - 1) - y) * Width + x] = Data[y * Width + x];
                }
            }

            return flipImage;
        }

        public override ImageD FlipY()
        {
            var flipImage = new Image3D(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    flipImage.Data[y * Width + ((Width - 1) - x)] = Data[y * Width + x];
                }
            }

            return flipImage;
        }

        public override ImageD FlipXY()
        {
            var flipImage = new Image3D(Width, Height);
            var flipImage_Temp = new Image3D(Width, Height);

            Parallel.For(0, Height, y =>
            //for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    //for (int k = 0; k < NumBand; k++)
                    flipImage_Temp.Data[(y * Pitch) + ((Width - 1 - x))] = Data[(y * Pitch) + (x)];
                }
            });

            Parallel.For(0, Height, y =>
            {
                int srcOffset = y * Pitch;
                int dstOffset = (Height - y - 1) * Pitch;
                Buffer.BlockCopy(flipImage_Temp.Data, srcOffset, flipImage.Data, dstOffset, Pitch);

            });

            return flipImage;
        }

        public override void RotateFlip(RotateFlipType rotateFlipType)
        {
            Image3D flipImage = null;
            switch (rotateFlipType)
            {
                case RotateFlipType.RotateNoneFlipX:
                    flipImage = (Image3D)FlipX();
                    break;
                case RotateFlipType.RotateNoneFlipY:
                    flipImage = (Image3D)FlipY();
                    break;
            }

            ImageData = flipImage.ImageData;
        }

        public override ImageD Resize(int destWidth, int destHeight)
        {
            var resizeImage = new Image3D(destWidth, destHeight);

            for (int y = 0; y < destHeight; y++)
            {
                for (int x = 0; x < destWidth; x++)
                {
                    var point = new PointF((float)x / destWidth * Width, (float)y / destHeight * Height);
                    resizeImage.Data[y * destWidth + x] = GetValue(point);
                }
            }

            return resizeImage;
        }

        public static Image3D Average(Image3D image3d1, Image3D image3d2)
        {
            var averageImage = new Image3D(image3d1.Width, image3d1.Height);

            for (int i = 0; i < image3d1.Width * image3d1.Height; i++)
            {
                averageImage.Data[i] = (image3d1.Data[i] + image3d2.Data[i]) / 2;
            }

            return averageImage;
        }
    }
}
