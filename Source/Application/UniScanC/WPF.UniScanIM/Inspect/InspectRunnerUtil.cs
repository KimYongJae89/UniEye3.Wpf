using DynMvp.Base;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniScanC.Data;

namespace WPF.UniScanIM.Inspect
{
    public static class InspectRunnerUtil
    {
        public static BitmapSource CreateImageSource(Image2D image2D)
        {
            BitmapSource image = null;
            if (image2D.Data != null)
            {
                image = BitmapSource.Create(image2D.Width, image2D.Height, 96, 96, PixelFormats.Gray8, null, image2D.Data, image2D.Pitch);
            }
            else if (image2D.DataPtr != System.IntPtr.Zero)
            {
                image = BitmapSource.Create(image2D.Width, image2D.Height, 96, 96, PixelFormats.Gray8, null, image2D.DataPtr, image2D.Pitch * image2D.Height, image2D.Pitch);
            }

            image?.Freeze();
            return image;
        }

        public static BitmapSource CreateImageSource(int width, int height, byte[] data)
        {
            var image = BitmapSource.Create(width, height, 96, 96, PixelFormats.Gray8, null, data, width);
            image.Freeze();
            return image;
        }

        public static ImageSource GetDefectImage(int width, int height, IEnumerable<Defect> defects)
        {
            var visual = new DrawingVisual();
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                foreach (Defect defect in defects)
                {
                    var brush = new SolidColorBrush(Colors.Red);

                    var streamGeometry = new StreamGeometry();


                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        var points = new System.Windows.Point[]
                        {
                            new System.Windows.Point(defect.BoundingRect.Left, defect.BoundingRect.Top),
                            new System.Windows.Point(defect.BoundingRect.Right, defect.BoundingRect.Top),
                            new System.Windows.Point(defect.BoundingRect.Right, defect.BoundingRect.Bottom),
                            new System.Windows.Point(defect.BoundingRect.Left, defect.BoundingRect.Bottom),
                        };

                        geometryContext.BeginFigure(points[0], true, true);
                        geometryContext.PolyLineTo(points, true, true);
                    }

                    drawingContext.DrawGeometry(brush, new System.Windows.Media.Pen(brush, 1), streamGeometry);
                }
            }

            var renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);

            renderTargetBitmap.Freeze();

            return renderTargetBitmap;
        }
    }
}
