using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniScanC.Struct;

namespace UniScanC.AlgoTask
{
    public static class AlgoLinkConverter
    {
        public static object Convert(Type Tin, Type Tout, object input)
        {
            LogHelper.Debug(LoggerType.Operation, $"AlgoLinkConverter::Convert - In: {Tin.Name}, Out: {Tout.Name}");
            System.Reflection.MethodInfo mothodInfo = typeof(AlgoLinkConverter).GetMethod($"Func{Tin.Name}To{Tout.Name}");
            return mothodInfo?.Invoke(null, new object[] { input });
        }

        public static bool IsExistConverter(Type Tin, Type Tout)
        {
            string methodName = $"Func{Tin.Name}To{Tout.Name}";
            System.Reflection.MethodInfo methodInfo = typeof(AlgoLinkConverter).GetMethod(methodName);
            return methodInfo != null;
        }

        public static SizeF FuncImageDataToSizeF(ImageData imageData)
        {
            return imageData.Size;
        }

        public static float FuncRoiMaskToSingle(RoiMask roiMask)
        {
            return roiMask.ROIs.Count() > 0 ? roiMask.ROIs.Min(g => g.Left) : 0;
        }

        public static BitmapSource FuncImageDataToBitmapSource(ImageData imageData)
        {
            LogHelper.Debug(LoggerType.Operation, $"AlgoLinkConverter::FuncImageDataToBitmapSource - imageData.Image: {imageData.Image.Size}, {imageData.Size}");
            if (imageData.Size.IsEmpty)
            {
                return imageData.Image.ToBitmapSource();
            }

            var rectangle = new Rectangle(Point.Empty, imageData.Size);
            using (DynMvp.Vision.AlgoImage childImage = imageData.Image.GetChildImage(rectangle))
            {
                return childImage.ToBitmapSource();
            }
        }
    }
}
