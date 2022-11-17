using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public enum ImageOperationType
    {
        Add, Subtract
    }

    public class ImageOperation
    {
        public static void Operate(ImageOperationType imageOperationType, Image2D image1, Image2D image2, Image2D imageDest)
        {
            Debug.Assert(image1.Width == image2.Width && image1.Width == imageDest.Width);
            Debug.Assert(image1.Height == image2.Height && image1.Height == imageDest.Height);
            Debug.Assert(image1.NumBand == 1 && image2.NumBand == 1 && imageDest.NumBand == 1);
            Debug.Assert(image1.Pitch == image2.Pitch && image1.Pitch == imageDest.Pitch);

            switch (imageOperationType)
            {
                case ImageOperationType.Add:
                    Parallel.For(0, image1.Height, y =>
                    {
                        for (int x = 0; x < image1.Width; x++)
                        {
                            int index = y * image1.Pitch + x;
                            int value = image1.Data[index] + image2.Data[index];
                            if (value > 255)
                            {
                                imageDest.Data[index] = 255;
                            }
                            else
                            {
                                imageDest.Data[index] = (byte)value;
                            }
                        }
                    });
                    break;
                case ImageOperationType.Subtract:
                    Parallel.For(0, image1.Height, y =>
                    {
                        for (int x = 0; x < image1.Width; x++)
                        {
                            int index = y * image1.Pitch + x;
                            int value = image1.Data[index] - image2.Data[index];
                            if (value < 0)
                            {
                                imageDest.Data[index] = 0;
                            }
                            else
                            {
                                imageDest.Data[index] = (byte)value;
                            }
                        }
                    });
                    break;
            }
        }
    }
}
