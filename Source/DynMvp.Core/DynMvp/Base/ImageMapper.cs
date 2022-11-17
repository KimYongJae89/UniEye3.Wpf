using DynMvp.Base;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class ImageMapper
    {
        private int width = 0;
        private int height = 0;
        private double pixelResMm;
        private RectangleF mappingRect;
        public Image3D Image3d { get; set; }
        public Image3D ImageWeight { get; set; }

        public ImageMapper()
        {

        }

        public void Initialize(RectangleF mappingRect, double pixelResMm)
        {
            this.mappingRect = mappingRect;
            width = (int)(mappingRect.Width / pixelResMm);
            height = (int)(mappingRect.Height / pixelResMm);
            this.pixelResMm = pixelResMm;

            Image3d = new Image3D();
            Image3d.Initialize(width, height, 1);

            ImageWeight = new Image3D();
            ImageWeight.Initialize(width, height, 1);
        }

        public void Mapping(Point3d[] pointArray, bool useSeparateBottom = false)
        {
            LogHelper.Debug(LoggerType.Grab, "Mapping");

            int limHeight = 5;

            Image3d.Clear();
            ImageWeight.Clear();

            bool[,] mask = new bool[width, height];
            bool[,] mask2 = new bool[width, height];
            bool[,] mask3 = new bool[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    mask[w, h] = false;
                    mask2[w, h] = false;
                    mask3[w, h] = false;
                }
            }

            double centerX = width / 2.0 - 0.5;
            double centerY = height / 2.0 - 0.5;
            double widthMetric = mappingRect.Width;
            double heightMetric = mappingRect.Height;
            double halfWidthMetric = widthMetric / 2.0;
            double halfHeightMetric = heightMetric / 2.0;

            for (int i = 0; i < pointArray.Length; i++)
            {
                if (mappingRect.Contains((float)pointArray[i].X, (float)pointArray[i].Y) == false)
                {
                    continue;
                }

                double fHeight = pointArray[i].Z;
                if (fHeight <= limHeight)   // Top only
                {
                    continue;
                }

                double fpixX = (pointArray[i].X - mappingRect.Left) / pixelResMm;
                double fpixY = (mappingRect.Bottom - pointArray[i].Y) / pixelResMm;//상하 반전

                int pixX = (int)Math.Floor(fpixX);
                int pixY = (int)Math.Floor(fpixY);

                if (pixX < 0 || pixX >= width)
                {
                    continue;
                }

                if (pixY < 0 || pixY >= height)
                {
                    continue;
                }

                double pixRemainX = fpixX - pixX;
                double pixRemainY = fpixY - pixY;

                //00
                {
                    int pixIndex = (pixY) * width + (pixX);
                    double fWeight = (1.0 - pixRemainX) * (1.0 - pixRemainY);
                    Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                    ImageWeight.Data[pixIndex] += (float)fWeight;
                    mask[(pixX), (pixY)] = true;
                    //bmpMask.SetPixel(pixX, pixY, Color.White);
                }

                //X++
                if (pixX < width - 1)
                {
                    int pixIndex = (pixY) * width + (pixX + 1);
                    double fWeight = (pixRemainX) * (1.0 - pixRemainY);
                    Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                    ImageWeight.Data[pixIndex] += (float)fWeight;
                    mask[(pixX + 1), (pixY)] = true;
                    //bmpMask.SetPixel(pixX+1, pixY, Color.White);
                }

                //Y++
                if (pixY < height - 1)
                {
                    int pixIndex = (pixY + 1) * width + (pixX);
                    double fWeight = (1.0 - pixRemainX) * (pixRemainY);
                    Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                    ImageWeight.Data[pixIndex] += (float)fWeight;
                    mask[(pixX), (pixY + 1)] = true;
                    //bmpMask.SetPixel(pixX, pixY + 1, Color.White);
                }

                //X++, Y++
                if ((pixX < width - 1) && (pixY < height - 1))
                {
                    int pixIndex = (pixY + 1) * width + (pixX + 1);
                    double fWeight = (pixRemainX) * (pixRemainY);
                    Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                    ImageWeight.Data[pixIndex] += (float)fWeight;
                    mask[(pixX + 1), (pixY + 1)] = true;
                    //bmpMask.SetPixel(pixX + 1, pixY + 1, Color.White);
                }
            }
            // );

            if (useSeparateBottom)
            {

                for (int i = 0; i < pointArray.Length; i++)
                //            Parallel.For(0, pointArray.Length, i =>
                {
                    if (pointArray[i].X > halfWidthMetric || pointArray[i].X < -halfWidthMetric)
                    {
                        continue;
                    }

                    if (pointArray[i].Y > halfHeightMetric || pointArray[i].Y < -halfHeightMetric)
                    {
                        continue;
                    }

                    double fHeight = pointArray[i].Z;
                    if (fHeight > limHeight)   // bottom only
                    {
                        continue;
                    }

                    double fpixX = (pointArray[i].X / pixelResMm) + centerX;
                    double fpixY = (-pointArray[i].Y / pixelResMm) + centerY;//상하 반전

                    int pixX = (int)Math.Floor(fpixX);
                    int pixY = (int)Math.Floor(fpixY);

                    if (pixX < 0 || pixX >= width)
                    {
                        continue;
                    }

                    if (pixY < 0 || pixY >= height)
                    {
                        continue;
                    }

                    double pixRemainX = fpixX - pixX;
                    double pixRemainY = fpixY - pixY;

                    //00
                    {
                        int pixIndex = (pixY) * width + (pixX);
                        double fWeight = (1.0 - pixRemainX) * (1.0 - pixRemainY);
                        if (mask[(pixX), (pixY)] == false)
                        {
                            Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                            ImageWeight.Data[pixIndex] += (float)fWeight;
                            //bmpMask2.SetPixel(pixX, pixY, Color.White);
                        }
                        else
                        {
                            //bmpMask3.SetPixel(pixX, pixY, Color.White);
                        }
                    }

                    //X++
                    if (pixX < width - 1)
                    {
                        int pixIndex = (pixY) * width + (pixX + 1);
                        double fWeight = (pixRemainX) * (1.0 - pixRemainY);
                        if (mask[(pixX + 1), (pixY)] == false)
                        {
                            Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                            ImageWeight.Data[pixIndex] += (float)fWeight;
                            //bmpMask2.SetPixel(pixX + 1, pixY, Color.White);
                        }
                        else
                        {
                            //bmpMask3.SetPixel(pixX + 1, pixY, Color.White);
                        }

                    }

                    //Y++
                    if (pixY < height - 1)
                    {
                        int pixIndex = (pixY + 1) * width + (pixX);
                        double fWeight = (1.0 - pixRemainX) * (pixRemainY);
                        if (mask[(pixX), (pixY + 1)] == false)
                        {
                            Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                            ImageWeight.Data[pixIndex] += (float)fWeight;
                            //bmpMask2.SetPixel(pixX, pixY + 1, Color.White);
                        }
                        else
                        {
                            //bmpMask3.SetPixel(pixX, pixY + 1, Color.White);
                        }
                    }

                    //X++, Y++
                    if ((pixX < width - 1) && (pixY < height - 1))
                    {
                        int pixIndex = (pixY + 1) * width + (pixX + 1);
                        double fWeight = (pixRemainX) * (pixRemainY);
                        if (mask[(pixX + 1), (pixY + 1)] == false)
                        {
                            Image3d.Data[pixIndex] += (float)(fHeight * fWeight);
                            ImageWeight.Data[pixIndex] += (float)fWeight;
                            //bmpMask2.SetPixel(pixX + 1, pixY + 1, Color.White);
                        }
                        else
                        {
                            //bmpMask3.SetPixel(pixX + 1, pixY + 1, Color.White);
                        }
                    }
                }
            }

            LogHelper.Debug(LoggerType.Grab, "Mapping - 2");

            double ftemp = 0;
            double fmin = 0;
            double fmax = 0;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int pixIndex = j * width + i;

                    if (ImageWeight.Data[pixIndex] == 0)
                    {
                        Image3d.Data[pixIndex] = 0;
                        continue;
                    }

                    ftemp = Image3d.Data[pixIndex] / ImageWeight.Data[pixIndex];
                    Image3d.Data[pixIndex] = (float)ftemp;

                    if (ftemp < -999 || ftemp > 999)
                    {
                        Image3d.Data[pixIndex] = 0;
                        continue;
                    }

                    if (ftemp > fmax)
                    {
                        fmax = ftemp;
                    }

                    if (ftemp < fmin)
                    {
                        fmin = ftemp;
                    }
                }
            }
            double fdiff = fmax - fmin;

            LogHelper.Debug(LoggerType.Grab, "End Mapping");
            //image3d.SaveImage("d:\\TRANSFORM\\2_Height.bmp", ImageFormat.Bmp);
            //imageWeight.SaveImage("d:\\TRANSFORM\\2_Weight.bmp", ImageFormat.Bmp);
        }
    }
}
