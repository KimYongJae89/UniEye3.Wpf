using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace UniScanC.Data
{
    public class DefectGroup : List<Defect>
    {
        public DefectGroup(Defect defect)
        {
            Add(defect);
        }

        public bool IsIntersect(DefectGroup dg)
        {
            return (this.Intersect(dg).Count() > 0);
        }

        public void Add(DefectGroup defectGroup)
        {
            AddRange(defectGroup);
        }

        public void AddRange(IEnumerable<DefectGroup> defectGroups)
        {
            foreach (DefectGroup defectGroup in defectGroups)
            {
                Add(defectGroup);
            }
        }

        public Defect ToDefect()
        {
            RectangleF accRect = RectangleF.Empty;
            var imageList = new List<(RectangleF, BitmapSource)>();

            var orderdList = this.OrderBy(f => f.FrameIndex).ToList();
            while (orderdList.Count > 0)
            {
                int frameIndex = orderdList[0].FrameIndex;
                List<Defect> inFrameDefectList = orderdList.FindAll(f => f.FrameIndex == frameIndex);
                orderdList.RemoveAll(f => inFrameDefectList.Contains(f));

                List<(RectangleF, BitmapSource)> rectList =
                    inFrameDefectList.Select(f => (DrawingHelper.Offset(f.ImageClipRect, new SizeF(0, accRect.Bottom)), f.DefectImage)).ToList();
                imageList.AddRange(rectList);

                RectangleF unionRect = inFrameDefectList.Select(f => f.BoundingRect).Aggregate((f, g) => RectangleF.Union(f, g));
                unionRect.Offset(0, accRect.Bottom);
                if (accRect.IsEmpty)
                {
                    accRect = unionRect;
                }

                accRect = RectangleF.Union(unionRect, accRect);
            }

            // 한줄이 길다. 길면 기차.
            var imageRect = Rectangle.Round(imageList.Select(f => f.Item1).Aggregate((f, g) => RectangleF.Union(f, g)));
            var image2D = new Image2D(imageRect.Width, imageRect.Height, 1);
            byte[] imageBytes = image2D.Data;
            imageList.ForEach(f =>
            {
                var rect = Rectangle.Round(f.Item1);
                rect.Offset(-imageRect.Left, -imageRect.Top);

                byte[] bytes = new byte[rect.Width * rect.Height];
                f.Item2?.CopyPixels(bytes, rect.Width, 0);

                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    int srcOffset = (y - rect.Top) * rect.Width;
                    int dstOffset = y * image2D.Pitch + rect.Left;
                    Array.Copy(bytes, srcOffset, imageBytes, dstOffset, rect.Width);
                }
            });

            // 망할 4바이트 배수가 아니면 어떻게 될까?
            BitmapSource bitmapSource = null;
            using (var bitmap = image2D.ToBitmap())
            {
                IntPtr hBitmap = bitmap.GetHbitmap();
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                bitmapSource.Freeze();
            }
            image2D.Dispose();

            Defect lastDefect = this.First();
            var defect = new Defect(lastDefect)
            {
                BoundingRect = accRect,
                MinGv = this.Min(f => f.MinGv),
                MaxGv = this.Max(f => f.MaxGv),
                AvgGv = (int)this.Average(f => f.AvgGv),
                DefectImage = bitmapSource
            };

            return defect;
        }

        public static implicit operator Defect(DefectGroup d)
        {
            return d.First();
        }
    }
}
