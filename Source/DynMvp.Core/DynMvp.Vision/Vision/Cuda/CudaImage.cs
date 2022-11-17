using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Cuda
{
    public abstract class CudaImage : AlgoImage
    {
        public static int CUDA_GPU_NO = 0;

        protected uint imageId;
        protected int width;
        protected int height;
        protected int pitch;
        protected int channel;

        public uint ImageID => imageId;
        public override int Width => width;
        public override int Height => height;
        public override int Pitch => pitch;

        public abstract void Alloc(int width, int height);
        public abstract void Alloc(int width, int height, IntPtr dataPtr);
        public abstract void Put(IntPtr intPtr);

        public static IntPtr ToIntPtr(Array buffer)
        {
            var pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            IntPtr temp = pinnedArray.AddrOfPinnedObject();

            pinnedArray.Free();

            return temp;
        }

        public CudaImage()
        {
            imageId = 0;
            width = 0;
            height = 0;
            pitch = 0;
            channel = 1;

            LibraryType = ImagingLibrary.Cuda;
        }

        protected static int SizeOfType(Type type)
        {
            var dm = new DynamicMethod("SizeOfType", typeof(int), new Type[] { });
            ILGenerator il = dm.GetILGenerator();
            il.Emit(OpCodes.Sizeof, type);
            il.Emit(OpCodes.Ret);
            return (int)dm.Invoke(null, null);
        }

        public override AlgoImage ChangeImageType(ImagingLibrary imagingLibrary)
        {
            return null;
        }

        public void SetRoiRect(RectangleF rect)
        {
            CudaMethods.CUDA_SET_ROI(imageId, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void ResetRoiRect()
        {
            CudaMethods.CUDA_RESET_ROI(imageId);
        }

        public void CreateProfile()
        {
            CudaMethods.CUDA_CREATE_PROFILE(imageId);
        }

        public void CreateLabelBuffer()
        {
            CudaMethods.CUDA_CREATE_LABEL_BUFFER(imageId);
        }

        public override byte[] CloneByte()
        {
            if (imageId == 0)
            {
                return null;
            }

            byte[] imageBuffer = new byte[width * height];
            var pinnedArray = GCHandle.Alloc(imageBuffer, GCHandleType.Pinned);

            IntPtr dataPtr = pinnedArray.AddrOfPinnedObject();
            CudaMethods.CUDA_GET_IMAGE(imageId, dataPtr);

            pinnedArray.Free();

            Marshal.Copy(dataPtr, imageBuffer, 0, imageBuffer.Length);

            return imageBuffer;
        }

        public override void CopyByte(byte[] imageBuffer)
        {
            if (imageId == 0)
            {
                return;
            }

            //byte[] imageBuffer = new byte[width * height];
            var pinnedArray = GCHandle.Alloc(imageBuffer, GCHandleType.Pinned);

            IntPtr dataPtr = pinnedArray.AddrOfPinnedObject();
            CudaMethods.CUDA_GET_IMAGE(imageId, dataPtr);

            pinnedArray.Free();

            // 복사가 또 필요한가? -> CudaMethods.CUDA_GET_IMAGE 에서 이미 복사 하지 않는가?
            //Marshal.Copy(dataPtr, imageBuffer, 0, imageBuffer.Length);

            //return imageBuffer;
        }


        protected abstract Array GetData();

        public override IntPtr GetPtr()
        {
            throw new NotImplementedException();
        }

        public Array CloneData()
        {
            if (imageId == 0)
            {
                return null;
            }

            return GetData();
        }

        public override void PutByte(byte[] data)
        {
            Put(CudaImage.ToIntPtr(data));
        }

        public override void Dispose()
        {
            Free();
        }

        public override Bitmap ToBitmap()
        {
            byte[] imageBuffer = CloneByte();
            if (imageBuffer == null)
            {
                return null;
            }

            Bitmap bitmap = ImageHelper.ByteArrayToBitmap(imageBuffer, width, height, channel);

            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    channel = 4;
                    break;
                case PixelFormat.Format24bppRgb:
                    channel = 3;
                    break;
                case PixelFormat.Format8bppIndexed:
                default:
                    channel = 1;
                    break;
            }

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            pitch = bmpData.Stride;
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }

        public override ImageD ToImageD()
        {
            var image = new Image2D();
            byte[] buffer = GetByte();

            var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            image.Initialize(width, height, 1, pitch, gcHandle.AddrOfPinnedObject());

            gcHandle.Free();

            return image;
        }

        public override void Save(string fileName, DebugContext debugContext = null)
        {
            ToBitmap()?.Save(fileName);
        }

        public override void Clear(byte initVal = 0)
        {
            CudaMethods.CUDA_CLEAR_IMAGE(imageId);
        }

        public void Free()
        {
            CudaMethods.CUDA_FREE_IMAGE(imageId);

            imageId = 0;
            width = 0;
            height = 0;
            pitch = 0;
            channel = 1;
        }
    }

    public class CudaDepthImage<T> : CudaImage
    {
        private T[] imageData;

        public CudaDepthImage() : base()
        {
            imageData = null;
        }

        protected override Array GetData()
        {
            if (imageData == null)
            {
                imageData = new T[width * height];
            }

            var pinnedArray = GCHandle.Alloc(imageData, GCHandleType.Pinned);

            IntPtr dataPtr = pinnedArray.AddrOfPinnedObject();
            CudaMethods.CUDA_GET_IMAGE(imageId, dataPtr);

            pinnedArray.Free();

            return imageData;
        }

        public override void Alloc(int width, int height)
        {
            this.width = width;
            this.height = height;

            pitch = width;
            channel = 1;

            imageId = CudaMethods.CUDA_CREATE_IMAGE(width, height, SizeOfType(typeof(T)));
        }

        public override void Alloc(int width, int height, IntPtr dataPtr)
        {
            Alloc(width, height);
            imageData = new T[width * height];
            var intPtr = CudaImage.ToIntPtr(imageData);
            Put(intPtr);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            var newImage = new CudaDepthImage<T>();
            newImage.Alloc(rectangle.Width, rectangle.Height);
            CudaMethods.CUDA_CLIP_IMAGE(imageId, newImage.ImageID, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            return newImage;
        }

        public override void Put(IntPtr intPtr)
        {
            CudaMethods.CUDA_SET_IMAGE(imageId, intPtr);
        }

        public override AlgoImage Clone()
        {
            var newImage = new CudaDepthImage<T>();
            newImage.Alloc(width, height);
            newImage.PutByte(CloneByte());

            return newImage;
        }

        public override AlgoImage GetChildImage(Rectangle rectangle)
        {
            return Clip(rectangle);
        }
    }
}
