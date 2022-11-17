using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.Matrox
{
    public abstract class MilImage : AlgoImage
    {
        ~MilImage()
        {
            Dispose();
            System.Diagnostics.Debug.Assert(image == 0);
        }

        public void Free()
        {
            if (image != MIL.M_NULL)
            {
                MIL.MbufFree(image);
                image = MIL.M_NULL;
            }
        }

        public override void Dispose()
        {
            Free();
        }

        public override void Clear(byte initVal)
        {
            MIL.MbufClear(image, initVal);
        }

        public override AlgoImage Clone()
        {
            return Clip(new Rectangle(0, 0, width, height));
        }

        public void Copy(AlgoImage srcImage)
        {
            Copy(srcImage, new Rectangle(0, 0, width, height));
        }

        public void Save(string fileName)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilImage.Save]");
            }

            MIL_INT fileFormat = MIL.M_BMP;
            if (fileName.Contains(".jpg") == true || fileName.Contains(".jpeg") == true)
            {
                fileFormat = MIL.M_JPEG2000_LOSSLESS;
            }

            MIL.MbufExport(fileName, fileFormat, image);
        }

        protected MIL_ID image = MIL.M_NULL;
        public MIL_ID Image
        {
            get => image;
            set => image = value;
        }

        protected int width;
        public override int Width => width;

        protected int height;
        public override int Height => height;

        public override int Pitch
        {
            get
            {
                if (image == MIL.M_NULL)
                {
                    throw new InvalidObjectException("[MilImage.Pitch]");
                }

                MIL_INT pitch = MIL.MbufInquire(image, MIL.M_PITCH);
                return (int)pitch;
                //return Width;
            }
        }

        public static MilGreyImage CheckGreyImage(AlgoImage algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
            {
                throw new InvalidSourceException(string.Format("[{0}] {1} Image is null", functionName, imageName));
            }

            try
            {
                var milImage = algoImage as MilGreyImage;
                if (milImage.Image == MIL.M_NULL)
                {
                    //throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
                }
                return milImage;
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(string.Format("[{0}] {1} Image must be gray image", functionName, imageName));
            }
        }

        public static MilColorImage CheckColorImage(AlgoImage algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
            {
                throw new InvalidSourceException(string.Format("[{0}] {1} Image is null", functionName, imageName));
            }

            try
            {
                var milImage = algoImage as MilColorImage;
                if (milImage.Image == MIL.M_NULL)
                {
                    throw new InvalidTargetException(string.Format("[{0}] {1} Image Object is null", functionName, imageName));
                }

                return milImage;
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(string.Format("[{0}] {1} Image must be color image", functionName, imageName));
            }
        }

        public void Alloc(int width, int height)
        {
            Alloc(width, height, IntPtr.Zero, 0);
        }

        public abstract void Alloc(int width, int height, IntPtr dataPtr, int pitch);
        public abstract void Copy(AlgoImage srcImage, Rectangle rectangle);
        public abstract void Put(byte[] userArrayPtr);
        public abstract void Get(byte[] userArrayPtr);
        public abstract void Put(float[] userArrayPtr);
        public abstract void Get(float[] userArrayPtr);

        public override IntPtr GetPtr()
        {
            if (image == MIL.M_NULL)
            {
                return IntPtr.Zero;
            }

            return MIL.MbufInquire(image, MIL.M_HOST_ADDRESS);
        }

        public override void Save(string fileName, DebugContext debugContext)
        {
            string filePath = null;

            if (debugContext != null)
            {
                if (debugContext.SaveDebugImage == false)
                {
                    return;
                }

                Directory.CreateDirectory(debugContext.Path);
                filePath = string.Format("{0}\\{1}", debugContext.Path, fileName);
            }
            else
            {
                filePath = fileName;
            }
            Save(filePath);
        }

        public override byte[] CloneByte()
        {
            byte[] data = new byte[Width * Height];
            Get(data);

            return data;
        }
        public override void CopyByte(byte[] imageBuffer)
        {
        }
        public override void PutByte(byte[] data)
        {
            Put(data);
        }
    }

    public class MilGreyImage : MilImage
    {
        public MilGreyImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Grey;
        }

        public MilGreyImage(int width, int height)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Grey;

            Alloc(width, height);
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilGreyImage.Alloc]");
            }

            this.width = width;
            this.height = height;

            if (dataPtr == IntPtr.Zero)
            {
                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            }
            else
            {
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr, MIL.M_NULL);
            }
            //image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_HOST_ADDRESS, MIL.M_DEFAULT, (ulong)dataPtr, MIL.M_NULL);

            if (image == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilGreyImage.Alloc]");
            }
        }

        public void AllocInteger(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilGreyImage.Alloc]");
            }

            this.width = width;
            this.height = height;
            if (dataPtr == IntPtr.Zero)
            {
                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 16, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            }
            else
            {
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_UNSIGNED + 16, MIL.M_IMAGE + MIL.M_PROC, MIL.M_HOST_ADDRESS + MIL.M_PITCH, MIL.M_DEFAULT, (ulong)dataPtr, MIL.M_NULL);
            }

            if (image == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilGreyImage.Alloc]");
            }
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle rectangle)
        {
            MilImage.CheckGreyImage(srcImage, "MilGreyImage.Copy", "Source");

            var milSrcImage = (MilGreyImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, rectangle.Left, rectangle.Top, MIL.M_ALL_BAND, 0, 0, rectangle.Width, rectangle.Height);

            milSrcImage.FilteredList = FilteredList;
        }

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilGreyImage.Put]");
            }

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilGreyImage.Get]");
            }

            MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            return Clip(new Rectangle(0, 0, width, height));
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            var milChildImage = new MilGreyImage();

            var wholeRect = new Rectangle(0, 0, width, height);
            if (rectangle.IntersectsWith(wholeRect))
            {
                rectangle.Intersect(wholeRect);
            }

            milChildImage.image = MIL.MbufChild2d(image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, MIL.M_NULL);

            milChildImage.width = rectangle.Width;
            milChildImage.height = rectangle.Height;

            return milChildImage;
        }

        public override Bitmap ToBitmap()
        {
            byte[] data = new byte[Width * Height];
            Get(data);

            return ImageHelper.CreateBitmap(Width, Height, Width, 1, data);
        }
    }

    public class MilDepthImage : MilImage
    {
        public MilDepthImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Depth;
        }

        public MilDepthImage(int width, int height)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Depth;

            Alloc(width, height);
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilDepthImage.Alloc]");
            }

            this.width = width;
            this.height = height;
            if (dataPtr == IntPtr.Zero)
            {
                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_FLOAT + 32, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            }
            else
            {
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_FLOAT + 32, MIL.M_IMAGE + MIL.M_PROC, MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr.ToInt64(), MIL.M_NULL);
            }

            if (image == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilDepthImage.Alloc]");
            }
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle rectangle)
        {
            MilImage.CheckGreyImage(srcImage, "MilDepthImage.Copy", "Source");

            var milSrcImage = (MilGreyImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, rectangle.Left, rectangle.Top, MIL.M_ALL_BAND, 0, 0, rectangle.Width, rectangle.Height);

            milSrcImage.FilteredList = FilteredList;
        }

        public override void Put(byte[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilDepthImage.Put] Byte data type is not support.");
        }

        public override void Get(byte[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilDepthImage.Get] Byte data type is not support.");
        }

        public override void Put(float[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilGreyImage.Put]");
            }

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(float[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilGreyImage.Get]");
            }

            MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            return Clip(new Rectangle(0, 0, width, height));
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override Bitmap ToBitmap()
        {
            throw new NotImplementedException();
        }
    }

    public class MilColorImage : MilImage
    {
        public MilColorImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Color;
        }

        public MilColorImage(int width, int height)
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Color;

            Alloc(width, height);
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilColorImage.Alloc] Already Allocated.");
            }

            this.width = width;
            this.height = height;
            if (dataPtr == IntPtr.Zero)
            {
                image = MIL.MbufAllocColor(MIL.M_DEFAULT_HOST, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
            }
            else
            {
                image = MIL.MbufCreateColor(MIL.M_DEFAULT_HOST, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_PITCH_BYTE, pitch, ref dataPtr, MIL.M_NULL);
            }

            if (image == MIL.M_NULL)
            {
                throw new AllocFailedException("[MilColorImage.Alloc]");
            }
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            return cloneImage;
        }

        private MIL_INT GetBand(ImageBandType imageBandType)
        {
            MIL_INT band;
            switch (imageBandType)
            {
                case ImageBandType.Red: band = MIL.M_RED; break;
                case ImageBandType.Blue: band = MIL.M_BLUE; break;
                case ImageBandType.Green: band = MIL.M_GREEN; break;
                default:
                    throw new ArgumentException("Invalid Image Band Type");
            }

            return band;
        }

        public AlgoImage Clone(ImageBandType imageBandType)
        {
            return Clone(new Rectangle(0, 0, width, height), imageBandType);
        }

        public AlgoImage Clone(Rectangle rectangle, ImageBandType imageBandType)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilColorImage.Clone] Image is not allocated ");
            }

            var cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);

            if (imageBandType == ImageBandType.Luminance)
            {
                MIL.MimConvert(image, cloneImage.Image, MIL.M_RGB_TO_L);
            }
            else
            {
                MIL_INT band = GetBand(imageBandType);

                MIL.MbufCopyColor2d(image, cloneImage.Image, band, rectangle.Left, rectangle.Top, MIL.M_ALL_BAND, 0, 0, rectangle.Width, rectangle.Height);
            }

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle rectangle)
        {
            MilImage.CheckColorImage(srcImage, "MilColorImage.Copy", "Source");

            var milSrcImage = (MilColorImage)srcImage;
            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, rectangle.Left, rectangle.Top, MIL.M_ALL_BAND, 0, 0, rectangle.Width, rectangle.Height);

            FilteredList = milSrcImage.FilteredList;
        }

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilColorImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilColorImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilColorImage.Put]");
            }

            MIL.MbufPutColor(image, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
            {
                throw new InvalidObjectException("[MilColorImage.Get]");
            }

            MIL.MbufGetColor(image, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override Bitmap ToBitmap()
        {
            throw new NotImplementedException();
        }
    }
}
