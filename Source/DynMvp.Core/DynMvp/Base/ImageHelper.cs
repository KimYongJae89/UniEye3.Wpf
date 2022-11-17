using DynMvp.UI;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Matrix = System.Drawing.Drawing2D.Matrix;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using WpfColor = System.Windows.Media.Color;
using WpfPixelFormat = System.Windows.Media.PixelFormat;

namespace DynMvp.Base
{
    public class ImageHelper
    {
        private static object lockObject = new object();

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public static string BitmapToBase64String(Bitmap image)
        {
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Bmp);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            string base64String = Convert.ToBase64String(imageByte);

            imageByte = null;

            return base64String;
        }

        public static string ImageToBase64String(Image image, ImageFormat imageFormat)
        {
            var memoryStream = new MemoryStream();
            image.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            string base64String = Convert.ToBase64String(imageByte);

            imageByte = null;

            return base64String;
        }

        public static Bitmap Base64StringToBitmap(string base64string)
        {
            if (string.IsNullOrEmpty(base64string))
            {
                return null;
            }

            byte[] imageByte = Convert.FromBase64String(base64string);

            var memoryStream = new MemoryStream(imageByte);
            memoryStream.Position = 0;

            var bitmap = new Bitmap(memoryStream);

            return bitmap;
        }

        public static Image Base64StringToImage(string base64string, ImageFormat imageFormat)
        {
            if (base64string == "")
            {
                return null;
            }

            byte[] imageByte = Convert.FromBase64String(base64string);

            var memoryStream = new MemoryStream(imageByte);
            memoryStream.Position = 0;

            var image = Image.FromStream(memoryStream);

            return image;
        }

        public static Image LoadImage(string fileName)
        {
            try
            {
                //return Image.FromFile(fileName);

                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var image = Image.FromStream(fs);
                fs.Flush();
                fs.Close();
                fs.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                LogHelper.Warn(LoggerType.Operation, StringManager.GetString("Fail to load image. ") + ex.Message);
            }

            return null;
        }

        public static void SaveImage(Image image, string fileName)
        {
            SaveImage(image, fileName, ImageFormat.Bmp);
        }

        public static void SaveImage(Image image, string fileName, ImageFormat imageFormat)
        {
            if (image == null)
            {
                return;
            }

            try
            {
                var memoryStream = new MemoryStream();
                image.Save(memoryStream, imageFormat);

                memoryStream.Position = 0;
                byte[] imageByte = memoryStream.ToArray();

                memoryStream.Dispose();
                File.WriteAllBytes(fileName, imageByte);
            }
            catch (ExternalException)
            {
                Console.WriteLine("ExternalException From ImageHelper::SaveImage");
            }
            catch (IOException)
            {
                Console.WriteLine("IOException From ImageHelper::SaveImage");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("UnauthorizedAccessException From ImageHelper::SaveImage");
            }
        }

        public static void SaveImage(float[] floatData, int width, int height, string fileName)
        {
            int i, j, j0;
            byte[] byteData = new byte[width * height];

            float minValue = floatData.Min();
            float maxValue = floatData.Max();

            float diffValue = Math.Abs(maxValue - minValue);

            for (j = 0; j < height; j++)
            {
                j0 = j * width;
                for (i = 0; i < width; i++)
                {
                    byteData[j0 + i] = (byte)(Math.Abs(floatData[j0 + i] - minValue) / diffValue * 255);
                }
            }

            SaveImage(byteData, width, height, fileName);
        }

        public static void SaveImage(byte[] byteData, int width, int height, string fileName)
        {
            int stride = width;
            if ((width % 4) != 0)
            {
                stride = width + (4 - width % 4);//4바이트 배수가 아닐시..
            }

            IntPtr imageBuffer = Marshal.AllocHGlobal(byteData.Length);
            Marshal.Copy(byteData, 0, imageBuffer, byteData.Length);

            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format8bppIndexed, imageBuffer);

            ColorPalette colorPalette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = colorPalette;

            bitmap.Save(fileName);

            bitmap.Dispose();
            Marshal.FreeHGlobal(imageBuffer);
        }

        public static Bitmap ClipImage(Bitmap bitmap, RectangleF clipRect)
        {
            return ClipImage(bitmap, new Rectangle((int)clipRect.X, (int)clipRect.Y, (int)clipRect.Width, (int)clipRect.Height));
        }

        public static Bitmap ClipImage(Bitmap bitmap, Rectangle clipRect)
        {
            var bmpRect = new Rectangle(new Point(0, 0), bitmap.Size);
            clipRect = Rectangle.Intersect(bmpRect, clipRect);
            if (bmpRect.Contains(clipRect) == false)
            {
                return null;
            }

            Bitmap cloneBitmap = bitmap.Clone(clipRect, bitmap.PixelFormat);

            return cloneBitmap;
        }

        public static Bitmap ClipImage(Bitmap bitmap, RotatedRect clipRect)
        {
            Rectangle boundRect = DrawingHelper.ToRect(clipRect.GetBoundRect());

            Bitmap boundImage = ClipImage(bitmap, boundRect);

            if (BaseConfig.Instance().SaveDebugImage)
            {
                ImageHelper.SaveImage(boundImage, Path.Combine(BaseConfig.Instance().TempPath, "BoundImage.bmp"));
            }

            if (clipRect.Angle != 0)
            {
                var rotatedBitmap = new Bitmap(boundRect.Width, boundRect.Height);
                using (var g = Graphics.FromImage(rotatedBitmap))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    //Transformation matrix
                    var m = new Matrix();
                    m.RotateAt(clipRect.Angle, new PointF(boundImage.Width / 2.0f, boundImage.Height / 2.0f));

                    g.Transform = m;
                    g.DrawImage(boundImage, 0, 0);

                    g.Dispose();
                }

                if (BaseConfig.Instance().SaveDebugImage)
                {
                    ImageHelper.SaveImage(rotatedBitmap, Path.Combine(BaseConfig.Instance().TempPath, "RotatedImage.bmp"));
                }

                var angle0Rect = new Rectangle((int)(clipRect.X - boundRect.X), (int)(clipRect.Y - boundRect.Y), (int)clipRect.Width, (int)clipRect.Height);
                Bitmap clipImage = ClipImage(rotatedBitmap, angle0Rect);

                if (BaseConfig.Instance().SaveDebugImage)
                {
                    ImageHelper.SaveImage(clipImage, Path.Combine(BaseConfig.Instance().TempPath, "ClipImage.bmp"));
                }

                rotatedBitmap.Dispose();

                return clipImage;

            }
            else
            {
                return boundImage;
            }
        }

        public static Bitmap CloneImage(Bitmap bitmap)
        {
            var bmpRect = new Rectangle(new Point(0, 0), bitmap.Size);
            return bitmap.Clone(bmpRect, bitmap.PixelFormat);
        }

        public static Image CloneImage(Image image)
        {
            return (Image)image.Clone();
        }

        public static void Copy(Bitmap srcImage, Point srcPt, Bitmap destImage, Point destPt, Size size)
        {
            if (destPt.X < 0 || (destPt.X + srcImage.Width) > destImage.Width ||
                    destPt.Y < 0 || (destPt.Y + srcImage.Height) > destImage.Height)
            {
                Debug.Assert(false, "Destination is invalid");
                return;
            }

            if (srcImage.PixelFormat != PixelFormat.Format8bppIndexed && srcImage.PixelFormat != PixelFormat.Format24bppRgb)
            {
                Debug.Assert(false, "Source image format is not 8 bit gray image or 24 bit color image");
                return;
            }

            if (destImage.PixelFormat != PixelFormat.Format8bppIndexed && destImage.PixelFormat != PixelFormat.Format24bppRgb)
            {
                Debug.Assert(false, "Destination image format is not 8 bit gray image or 24 bit color image");
                return;
            }

            lock (srcImage)
            {
                lock (destImage)
                {
                    unsafe
                    {
                        var srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
                        var destRect = new Rectangle(0, 0, destImage.Width, destImage.Height);

                        BitmapData srcBmpData = srcImage.LockBits(srcRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, srcImage.PixelFormat);
                        BitmapData destBmpData = destImage.LockBits(destRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, destImage.PixelFormat);

                        IntPtr srcPtr = srcBmpData.Scan0;
                        IntPtr destPtr = destBmpData.Scan0;

                        if (destImage.PixelFormat == srcImage.PixelFormat)
                        {
                            int srcPixelSize = 1;
                            if (srcImage.PixelFormat == PixelFormat.Format24bppRgb)
                            {
                                srcPixelSize = 3;
                            }

                            if ((srcImage.Width == destImage.Width) && (srcImage.Height == destImage.Height) && (destPt.X == 0 && destPt.Y == 0) && (srcBmpData.Stride == destBmpData.Stride))
                            {
                                LogHelper.Debug(LoggerType.Operation, "Imagehelper - Copy - 6");

                                CopyMemory(destPtr, srcPtr, (uint)(srcBmpData.Stride * srcBmpData.Height * srcPixelSize));
                            }
                            else
                            {
                                for (int y = 0; y < srcImage.Height; y++)
                                {
                                    CopyMemory(destPtr + destPt.X * srcPixelSize + (destPt.Y + y) * destBmpData.Stride, srcPtr + y * srcBmpData.Stride, (uint)(srcBmpData.Width * srcPixelSize));
                                }
                            }
                        }
                        else
                        {
                            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;
                            int destImageSize = destBmpData.Stride * destBmpData.Height;

                            byte[] srcImageBuffer = new byte[srcImageSize];
                            byte[] destImageBuffer = new byte[destImageSize];

                            Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);
                            Marshal.Copy(destPtr, destImageBuffer, 0, destImageSize);

                            int srcPixelSize = 1;
                            int pixelStep1 = 1;
                            int pixelStep2 = 2;
                            int pixelStep3 = 3;
                            if (srcImage.PixelFormat == PixelFormat.Format32bppRgb)
                            {
                                srcPixelSize = 4;
                            }
                            else if (srcImage.PixelFormat == PixelFormat.Format24bppRgb)
                            {
                                srcPixelSize = 3;
                            }
                            else  // 8bit
                            {
                                pixelStep1 = 0;
                                pixelStep2 = 0;
                                pixelStep3 = 0;
                            }

                            Parallel.For(0, srcImage.Height, y =>
                            {
                                for (int x = 0; x < srcImage.Width; x++)
                                {
                                    if (destImage.PixelFormat == PixelFormat.Format32bppRgb)
                                    {
                                        destImageBuffer[(destPt.X + x) * 4 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 1 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep1 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 2 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep2 + (y * srcBmpData.Stride)];
                                    }
                                    else if (destImage.PixelFormat == PixelFormat.Format32bppArgb)
                                    {
                                        destImageBuffer[(destPt.X + x) * 4 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 1 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep1 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 2 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep2 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 3 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep3 + (y * srcBmpData.Stride)];
                                    }
                                    else if (destImage.PixelFormat == PixelFormat.Format24bppRgb)
                                    {
                                        destImageBuffer[(destPt.X + x) * 3 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 3 + 1 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep1 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 3 + 2 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep2 + (y * srcBmpData.Stride)];
                                    }
                                    else
                                    {
                                        destImageBuffer[(destPt.X + x) + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                    }
                                }
                            });

                            Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);
                            Marshal.Copy(destImageBuffer, 0, destPtr, destImageSize);
                        }

                        srcImage.UnlockBits(srcBmpData);

                        destImage.UnlockBits(destBmpData);

                        if (destImage.PixelFormat == PixelFormat.Format8bppIndexed)
                        {
                            ColorPalette colorPalette = destImage.Palette;
                            for (int i = 0; i < 256; i++)
                            {
                                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                            }
                            destImage.Palette = colorPalette;
                        }
                    }
                }
            }
        }

        public static void Add(Bitmap srcImage, int value)
        {
            var srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            for (int index = 0; index < srcImage.Height; index++)
            {
                srcImageBuffer[index] = (byte)(srcImageBuffer[index] + value);
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            ColorPalette colorPalette = srcImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            srcImage.Palette = colorPalette;
        }

        public static Bitmap MakeColor(Bitmap original)
        {
            var newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    //create the color object
                    var newColor = Color.FromArgb(originalColor.A, originalColor.R, originalColor.G, originalColor.B);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        public static Bitmap ConvertGrayImage(Bitmap original)
        {
            original.Save(string.Format("{0}\\{1}.bmp", BaseConfig.Instance().TempPath, "MakeGrayscale"), ImageFormat.Bmp);
            var imageRect = new Rectangle(0, 0, original.Width, original.Height);

            var greyImage = new Bitmap(original.Width, original.Height, PixelFormat.Format8bppIndexed);
            BitmapData greyBmpData = greyImage.LockBits(imageRect, ImageLockMode.ReadWrite, greyImage.PixelFormat);
            IntPtr grayImagePtr = greyBmpData.Scan0;
            int grayImageSize = greyBmpData.Stride * greyBmpData.Height;
            byte[] grayImageBuffer = new byte[grayImageSize];

            BitmapData srcBmpData = original.LockBits(imageRect, ImageLockMode.ReadWrite, original.PixelFormat);

            IntPtr srcImagePtr = srcBmpData.Scan0;
            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;
            byte[] srcImageBuffer = new byte[srcImageSize];
            System.Runtime.InteropServices.Marshal.Copy(srcImagePtr, srcImageBuffer, 0, srcImageSize);

            int width = original.Width;
            Parallel.For(0, original.Height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    int mean = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        mean += srcImageBuffer[x * 3 + y * srcBmpData.Stride + k];
                    }

                    grayImageBuffer[x + y * greyBmpData.Stride] = (byte)(mean / 3);
                }
            });

            //for (int i = 0; i < original.Width; i++)
            //{
            //    for (int j = 0; j < original.Height; j++)
            //    {
            //        imageBuffer[i + j * greyBmpData.Stride] =
            //         (byte)((original.GetPixel(i, j).R + original.GetPixel(i, j).G + original.GetPixel(i, j).B) / 3);
            //    }
            //}

            System.Runtime.InteropServices.Marshal.Copy(grayImageBuffer, 0, grayImagePtr, grayImageSize);

            greyImage.UnlockBits(greyBmpData);
            original.UnlockBits(srcBmpData);

            ColorPalette cp = greyImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                cp.Entries[i] = Color.FromArgb(i, i, i);
            }

            greyImage.Palette = cp;

            return greyImage;
        }

        public static ImageD GetRotateMask(int width, int height, RotatedRect rotatedRect)
        {
            RectangleF boundRect = rotatedRect.GetBoundRect();
            rotatedRect.Offset(-boundRect.X, -boundRect.Y);

            var rectangleFigure = new RectangleFigure(rotatedRect, new System.Drawing.Pen(Color.White), new SolidBrush(Color.White));
            var rotatedMask = new Bitmap(width, height);

            var g = Graphics.FromImage(rotatedMask);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, width, height));
            rectangleFigure.Draw(g, new CoordTransformer(), true);
            g.Dispose();

            Bitmap grayImage = ImageHelper.ConvertGrayImage(rotatedMask);

            rotatedMask.Dispose();

            return Image2D.ToImage2D(grayImage);
        }

        public static byte[] GetByte(Bitmap bitmap)
        {
            var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bmpData = bitmap.LockBits(bitmapRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr bmpPtr = bmpData.Scan0;
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            //int imageSize = bmpData.Stride * bitmap.Height * bytesPerPixel;
            int imageSize = bmpData.Stride * bitmap.Height;

            byte[] imageData = new byte[imageSize];

            Parallel.For(0, bitmap.Height, y => Marshal.Copy(bmpPtr + y * bmpData.Stride, imageData, y * bmpData.Stride, bmpData.Stride));

            return imageData;
        }

        public static Bitmap CreateBitmap(int width, int height, int pitch, int numBand, byte[] imageData)
        {
            Debug.Assert(imageData != null);

            Color[] pallet = null;
            PixelFormat pixelFormat;
            if (numBand == 4)
            {
                pixelFormat = PixelFormat.Format32bppPArgb;
            }
            else if (numBand == 3)
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            }
            else if (numBand == 2)
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppRgb565;
            }
            else
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;

                pallet = new Color[256];
                for (int i = 0; i < 256; i++)
                {
                    pallet[i] = Color.FromArgb(i, i, i);
                }
            }

            var bitmap = new Bitmap(width, height, pixelFormat);
            if (pallet != null)
            {
                ColorPalette cp = bitmap.Palette;
                Array.Copy(pallet, cp.Entries, Math.Min(pallet.Length, bitmap.Palette.Entries.Length));
                bitmap.Palette = cp;
            }

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat);
            for (int y = 0; y < height; y++)
            {
                Marshal.Copy(imageData, y * pitch, new IntPtr(bmpData.Scan0.ToInt64() + y * bmpData.Stride), bmpData.Width);
            }

            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        private static void Yuv2Rgb(int Y, int U, int V, ref int R, ref int G, ref int B)
        {
            B = (int)(1.164 * (Y - 16.0) + 2.018 * (U - 128.0));
            G = (int)(1.164 * (Y - 16.0) - 0.813 * (V - 128.0) - 0.391 * (U - 128.0));
            R = (int)(1.164 * (Y - 16) + 1.596 * (V - 128));

            B = asByte(B);
            G = asByte(G);
            R = asByte(R);
        }

        private static byte asByte(int value)
        {
            //return (byte)value;
            if (value > 255)
            {
                return 255;
            }
            else if (value < 0)
            {
                return 0;
            }
            else
            {
                return (byte)value;
            }
        }

        public static Bitmap CreateBitmap(int width, int height, int pitch, int numBand, IntPtr dataPtr)
        {
            Debug.Assert(dataPtr != null);

            int stride = width * numBand;
            if ((stride % 4) != 0)
            {
                stride = stride + (4 - stride % 4);//4바이트 배수가 아닐시..
            }

            Bitmap bmpImage;

            if (numBand == 3)
            {
                bmpImage = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, dataPtr);
            }
            else
            {
                bmpImage = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, dataPtr);

                ColorPalette cp = bmpImage.Palette;
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }

                bmpImage.Palette = cp;
            }

            return bmpImage;
        }

        public static byte[] ConvertByteBuffer(float[] floatData)
        {
            byte[] byteData = new byte[floatData.Count()];

            float fmin = floatData.Min();
            float fmax = floatData.Max();
            float fdiff = Math.Abs(fmax - fmin);

            if (fdiff > 0)
            {
                Parallel.For(0, floatData.Count(), index => byteData[index] = Convert.ToByte(Math.Abs(floatData[index] - fmin) / fdiff * 255));
            }
            else
            {
                Array.Clear(byteData, 0, byteData.Length);
            }

            return byteData;
        }

        public static void Clear(Bitmap srcImage, byte value)
        {
            if (srcImage == null)
            {
                return;
            }

            var srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];
            Array.Clear(srcImageBuffer, 0, srcImageBuffer.Length);
            //System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            if (value != 0)
            {
                for (int yIndex = 0; yIndex < srcImage.Height; yIndex++)
                {
                    for (int xIndex = 0; xIndex < srcImage.Width; xIndex++)
                    {
                        int index = yIndex * srcBmpData.Stride + xIndex;
                        srcImageBuffer[index] = value;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            if (srcImage.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = srcImage.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
            }
        }

        public static void Not(Bitmap srcImage)
        {
            var srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            for (int index = 0; index < srcImage.Height * srcImage.Width; index++)
            {
                srcImageBuffer[index] = (byte)(255 - srcImageBuffer[index]);
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            ColorPalette colorPalette = srcImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
            }
            srcImage.Palette = colorPalette;
        }

        public static Bitmap ConvertTo24bpp(Bitmap bitmap)
        {
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                return bitmap;
            }

            var bitmapConverted = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bitmapConverted))
            {
                gr.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }

            return bitmapConverted;
        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int channel = 1;
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb || bitmap.PixelFormat == PixelFormat.Format32bppPArgb || bitmap.PixelFormat == PixelFormat.Format32bppRgb)
            {
                channel = 4;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                channel = 3;
            }

            int rawWidth = bitmapData.Width * channel;
            rawWidth = rawWidth % 4 == 0 ? rawWidth : rawWidth + (4 - (rawWidth % 4));
            int bitmapSize = rawWidth * bitmapData.Height;
            byte[] rawImage = new byte[bitmapSize];
            Marshal.Copy(bitmapData.Scan0, rawImage, 0, bitmapSize);
            bitmap.UnlockBits(bitmapData);
            return NonPaddedByteArray(rawImage, bitmap.Width, bitmap.Height, channel);
        }

        public static Bitmap ByteArrayToBitmap(byte[] byteArray, int width, int height, int channel)
        {
            byte[] newByteArray = PaddedByteArray(byteArray, width, height, channel);
            PixelFormat pixelFormat = PixelFormat.Format8bppIndexed;
            if (channel == 4)
            {
                pixelFormat = PixelFormat.Format32bppArgb;
            }
            else if (channel == 3)
            {
                pixelFormat = PixelFormat.Format24bppRgb;
            }

            var bitmap = new Bitmap(width, height, pixelFormat);
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(newByteArray, 0, bmpData.Scan0, newByteArray.Length);
            if (channel == 1)
            {
                ColorPalette palette = bitmap.Palette;
                for (int i = 0, color = 0; i < 256; i++, color += 1)
                {
                    palette.Entries[i] = Color.FromArgb(color, color, color);
                }

                bitmap.Palette = palette;
            }
            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        private static byte[] PaddedByteArray(byte[] byteArray, int width, int height, int channel)
        {
            int margin = (width * channel) % 4;
            if (margin == 0)
            {
                return byteArray;
            }

            int padByte = 4 - margin;
            int nonPaddedWidth = width * channel;
            int paddedWidth = width * channel + padByte;
            byte[] newByteArray = new byte[byteArray.Length + height * padByte];
            for (int h = 0; h < height; ++h)
            {
                Array.Copy(byteArray, h * nonPaddedWidth, newByteArray, h * paddedWidth, nonPaddedWidth);
            }

            return newByteArray;
        }

        private static byte[] NonPaddedByteArray(byte[] byteArray, int width, int height, int channel)
        {
            int margin = (width * channel) % 4;
            if (margin == 0)
            {
                return byteArray;
            }

            int padByte = 4 - margin;
            int nonPaddedWidth = width * channel;
            int paddedWidth = width * channel + padByte;
            byte[] newByteArray = new byte[byteArray.Length - height * padByte];
            for (int h = 0; h < height; ++h)
            {
                Array.Copy(byteArray, h * paddedWidth, newByteArray, h * nonPaddedWidth, nonPaddedWidth);
            }

            return newByteArray;
        }

        public static Mat ByteArrayToMat(byte[] data, int width, int height, int channel)
        {
            var mat = new Mat(new Size(width, height), Emgu.CV.CvEnum.DepthType.Cv8U, channel);
            Marshal.Copy(data, 0, mat.DataPointer, data.Length);
            return mat;
        }

        public static Mat FloatArrayToMat(float[] data, int width, int height, int channel)
        {
            var mat = new Mat(new Size(width, height), Emgu.CV.CvEnum.DepthType.Cv32F, channel);
            Marshal.Copy(data, 0, mat.DataPointer, data.Length);
            return mat;
        }

        public static byte[] MatToByteArray(Mat mat)
        {
            byte[] byteArray = new byte[mat.Width * mat.Height * mat.NumberOfChannels];
            Marshal.Copy(mat.DataPointer, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        public static float[] MatToFloatArray(Mat mat)
        {
            float[] floatArray = new float[mat.Width * mat.Height * mat.NumberOfChannels];
            Marshal.Copy(mat.DataPointer, floatArray, 0, floatArray.Length);
            return floatArray;
        }

        public static Mat BitmapToMat(Bitmap bitmap, bool convertGray = false)
        {
            Bitmap bitmap24bpp = ConvertTo24bpp(bitmap);
            byte[] byteArray = BitmapToByteArray(bitmap24bpp);
            Mat mat = ByteArrayToMat(byteArray, bitmap.Width, bitmap.Height, 3);
            if (convertGray && mat.NumberOfChannels == 3)
            {
                CvInvoke.CvtColor(mat, mat, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            }

            return mat;
        }

        public static Bitmap MatToBitmap(Mat mat)
        {
            byte[] byteArray = MatToByteArray(mat);
            return new Bitmap(ByteArrayToBitmap(byteArray, mat.Width, mat.Height, mat.NumberOfChannels));
        }

        public static Mat CvRotate(Mat image, float rotateAngle, bool interpolation)
        {
            var center = new PointF(image.Width / 2.0f - 0.5f, image.Height / 2.0f - 0.5f);
            var trMat = new Matrix<double>(2, 3);
            CvInvoke.GetRotationMatrix2D(center, -rotateAngle, 1, trMat);
            //Rectangle bbox = new RotatedRect(center, new SizeF(image.Width, image.Height), rotateAngle).MinAreaRect();
            //trMat[0, 2] = trMat[0, 2] + image.Width / 2.0 - center.X;
            //trMat[1, 2] = trMat[1, 2] + image.Height / 2.0 - center.Y;
            var imageRotated = new Mat();
            CvInvoke.WarpAffine(image, imageRotated, trMat, new Size(image.Width, image.Height), interpolation ? Emgu.CV.CvEnum.Inter.Linear : Emgu.CV.CvEnum.Inter.Nearest);
            return imageRotated;
        }

        public static Mat CvRoi(Mat image, Rectangle roi)
        {
            if (roi.X < 0)
            {
                roi.X = 0;
            }

            if (roi.Y < 0)
            {
                roi.Y = 0;
            }

            if (roi.X + roi.Width > image.Width)
            {
                roi.Width = (image.Width - roi.X - 1);
            }

            if (roi.Y + roi.Height > image.Height)
            {
                roi.Height = (image.Height - roi.Y - 1);
            }

            var newMat = new Mat(image, roi);
            return newMat.Clone();
        }

        public static Mat CvRotatedRoi(Mat image, Emgu.CV.Structure.RotatedRect roi, bool interpolation = true)
        {
            Rectangle boundRect = roi.MinAreaRect();
            if (boundRect.X < 0 || boundRect.Y < 0 || boundRect.X >= image.Width || boundRect.Y >= image.Height ||
                boundRect.X + boundRect.Width >= image.Width || boundRect.Y + boundRect.Height >= image.Height)
            {
                return new Mat(image, new Rectangle(0, 0, 1, 1));
            }

            Mat boundImage = new Mat(image, boundRect).Clone();
            roi.Center.X -= boundRect.X;
            roi.Center.Y -= boundRect.Y;

            Mat trMat = new Mat(), rotated = new Mat(), cropped = new Mat();
            float angle = roi.Angle;
            var roiSize = Size.Ceiling(roi.Size);
            if (roi.Angle < -45f)
            {
                angle += 90f;
                int temp = roiSize.Width;
                roiSize.Width = roiSize.Height;
                roiSize.Height = temp;
            }
            CvInvoke.GetRotationMatrix2D(roi.Center, angle, 1, trMat);
            CvInvoke.WarpAffine(boundImage, rotated, trMat, boundImage.Size, interpolation ? Emgu.CV.CvEnum.Inter.Linear : Emgu.CV.CvEnum.Inter.Nearest);
            CvInvoke.GetRectSubPix(rotated, roiSize, roi.Center, cropped);
            return cropped;
        }

        public static Bitmap BitmapSourceToBitmap(BitmapSource source, PixelFormat pixelFormat)
        {
            ////convert image format
            //var src = new FormatConvertedBitmap();
            //src.BeginInit();
            //src.Source = source;
            //if (pixelFormat == PixelFormat.Format8bppIndexed)
            //    src.DestinationFormat = PixelFormats.Gray8;
            //else
            //    src.DestinationFormat = PixelFormats.Bgra32;
            //src.EndInit();

            ////copy to bitmap
            //Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, pixelFormat);
            //var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.WriteOnly, pixelFormat);
            //src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            //bitmap.UnlockBits(data);

            //return bitmap;

            var bmp = new Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                pixelFormat);

            BitmapData data = bmp.LockBits(
              new Rectangle(Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              pixelFormat);

            ColorPalette colorPalette = bmp.Palette;
            Color[] colors = colorPalette.Entries;
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.FromArgb(255, i, i, i);
            }

            bmp.Palette = colorPalette;

            source.CopyPixels(
              System.Windows.Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);

            return bmp;
        }

        public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
               bitmapData.Width, bitmapData.Height, 96, 96, ConvertPixelFormat(bitmap.PixelFormat), null,
               bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            bitmapSource.Freeze();

            return bitmapSource;
        }

        // Buffer에 이미지 헤더까지 포함되어 있을 경우 이 함수로 변환한다.
        public static BitmapSource ByteArrayToBitmapSource(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();

                image.Freeze();

                return image;
            }
        }

        // Buffer에 Pixel 값만 있을 경우 이 함수로 변환한다.
        public static BitmapSource ByteArrayToBitmapSource(int width, int height, WpfPixelFormat pixelFormat, byte[] buffer)
        {
            double dpiX = 96d;
            double dpiY = 96d;

            int bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8;
            int stride = bytesPerPixel * width;

            var bitmapSource = BitmapSource.Create(width, height, dpiX, dpiY, pixelFormat, null, buffer, stride);

            bitmapSource.Freeze();

            return bitmapSource;
        }

        public static byte[] BitmapSourceToByteArray(BitmapSource bitmapSource, ImageFormat imageFormat)
        {
            if (bitmapSource == null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();

            BitmapEncoder bitmapEncoder;

            if (imageFormat == ImageFormat.Jpeg)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = 80;
                bitmapEncoder = encoder;
            }
            else //if (ImageSaveType == ImageFormat.Bmp)
            {
                bitmapEncoder = new BmpBitmapEncoder();
            }

            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            bitmapEncoder.Save(memoryStream);

            return memoryStream.ToArray();
        }

        public static WpfPixelFormat ConvertPixelFormat(PixelFormat pixelFormat)
        {
            WpfPixelFormat wpfPixelFormat = PixelFormats.Gray8;

            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    wpfPixelFormat = PixelFormats.Gray8;
                    break;
                case PixelFormat.Format24bppRgb:
                    wpfPixelFormat = PixelFormats.Rgb24;
                    break;
                case PixelFormat.Format32bppRgb:
                    wpfPixelFormat = PixelFormats.Bgr32;
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    wpfPixelFormat = PixelFormats.Bgra32;
                    break;
            }

            return wpfPixelFormat;
        }
    }
}
