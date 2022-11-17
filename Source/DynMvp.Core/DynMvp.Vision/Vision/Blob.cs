using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Vision
{
    public class BlobRect
    {
        public float Area { get; set; }
        public float Circularity { get; set; }
        public RectangleF BoundingRect { get; set; }
        public PointF CenterPt { get; set; }
        public int LabelNumber { get; set; }
        public float SigmaValue { get; set; }
        public float MaxValue { get; set; }
        public float MinValue { get; set; }
        public float MeanValue { get; set; }
        public float Compactness { get; set; }
        public float Rectangularity { get; set; }
        public int NumberOfHoles { get; set; }
        public float ConvexFillRatio { get; set; }
        public float AspectRetio { get; set; }
        public List<PointF> ConvexHullPointList { get; set; }

        public void CopyFrom(BlobRect srcDefect)
        {
            Area = srcDefect.Area;
            Circularity = srcDefect.Circularity;
            BoundingRect = srcDefect.BoundingRect;
            CenterPt = srcDefect.CenterPt;
            LabelNumber = srcDefect.LabelNumber;
            SigmaValue = srcDefect.SigmaValue;
            MaxValue = srcDefect.MaxValue;
            MinValue = srcDefect.MinValue;
            MeanValue = srcDefect.MeanValue;
            Compactness = srcDefect.Compactness;
            Rectangularity = srcDefect.Rectangularity;
            NumberOfHoles = srcDefect.NumberOfHoles;
            ConvexFillRatio = srcDefect.ConvexFillRatio;
            AspectRetio = srcDefect.AspectRetio;
            if (srcDefect.ConvexHullPointList != null)
            {
                ConvexHullPointList.AddRange(srcDefect.ConvexHullPointList);
            }
        }
    }

    public class BlobRectList : IEnumerable<BlobRect>, IDisposable
    {
        private HashSet<BlobRect> blobRectList = new HashSet<BlobRect>();

        public int Count => blobRectList.Count;

        public List<BlobRect> GetList()
        {
            return blobRectList.ToList();
        }

        public void AddBlobRect(BlobRect blobRect)
        {
            blobRectList.Add(blobRect);
        }

        public void AddBlobRect(BlobRectList blobRectList)
        {
            foreach (BlobRect b in blobRectList)
            {
                this.blobRectList.Add(b);
            }
        }

        public void Offset(float offsetX, float offsetY)
        {
            foreach (BlobRect b in this)
            {
                var rect = new RectangleF();
                rect.X = b.BoundingRect.X + offsetX;
                rect.Y = b.BoundingRect.Y + offsetY;
                rect.Width = b.BoundingRect.Width;
                rect.Height = b.BoundingRect.Height;
                b.BoundingRect = rect;

                var pt = new PointF();
                pt.X = b.CenterPt.X + offsetX;
                pt.Y = b.CenterPt.Y + offsetY;
                b.CenterPt = pt;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return blobRectList.GetEnumerator();
        }

        public IEnumerator<BlobRect> GetEnumerator()
        {
            return blobRectList.GetEnumerator();
        }

        public void Clear()
        {
            blobRectList.Clear();
        }

        public BlobRect GetMaxAreaBlob()
        {
            if (blobRectList.Count == 0)
            {
                return null;
            }

            return blobRectList.OrderByDescending(x => x.Area).First();
        }

        public RectangleF GetUnionRect()
        {
            BlobRect maxBlobRect = GetMaxAreaBlob();
            if (maxBlobRect == null)
            {
                return new RectangleF();
            }

            RectangleF unionRect = maxBlobRect.BoundingRect;

            foreach (BlobRect blobRect in blobRectList)
            {
                unionRect = RectangleF.Union(unionRect, blobRect.BoundingRect);
            }

            return unionRect;
        }

        public virtual void Dispose()
        {

        }
    }

    public class DrawBlobOption
    {
        public bool SelectBlob { get; set; } = false;
        public bool SelectBlobContour { get; set; } = false;
        public bool SelectHoles { get; set; } = false;
        public bool SelectHolesContour { get; set; } = false;
    }

    public class BlobParam
    {
        public int MaxCount { get; set; }
        public bool SelectWholeImage { get; set; } = false;
        public bool SelectArea { get; set; } = true;
        public bool SelectBoundingRect { get; set; } = true;
        public bool SelectCenterPt { get; set; } = true;
        public bool SelectLabelValue { get; set; }
        public bool SelectNumberOfHoles { get; set; }
        public double AreaMin { get; set; }
        public double AreaMax { get; set; }
        public double BoundingRectMinX { get; set; }
        public double BoundingRectMinY { get; set; }
        public double BoundingRectMaxX { get; set; }
        public double BoundingRectMaxY { get; set; }
        public double BoundingRectMinWidth { get; set; }
        public double BoundingRectMaxWidth { get; set; }
        public double BoundingRectMinHeight { get; set; }
        public double BoundingRectMaxHeight { get; set; }
        public bool SaveContour { get; set; }
        public bool SelectMinValue { get; set; }
        public bool SelectMeanValue { get; set; }
        public bool SelectMaxValue { get; set; }
        public bool SelectSigmaValue { get; set; }
        public bool SelectCompactness { get; set; }
        public bool SelectAspectRatio { get; set; }
        public bool SelectRectangularity { get; set; }
        public bool SelectConvexHull { get; set; }
        public bool SelectMinAreaBox { get; set; }
        public bool SelectConvexFillRatio { get; set; }
        public bool EraseBorderBlob { get; set; }
        public bool GroupSameLabelAndTouchingBlobs { get; set; }

        public BlobParam()
        {
            SelectWholeImage = false;
            SelectArea = true;
            SelectBoundingRect = true;
            SelectCenterPt = false;
            SelectLabelValue = false;
            SaveContour = false;
            SelectCompactness = false;
            SelectConvexHull = false;
            SelectMinValue = false;
            SelectMaxValue = false;
            SelectMeanValue = false;
            SelectConvexFillRatio = false;
            EraseBorderBlob = false;
            SelectAspectRatio = false;
            SelectRectangularity = false;
            SelectMinAreaBox = false;

            GroupSameLabelAndTouchingBlobs = false;

            BoundingRectMinWidth = 0;
            BoundingRectMaxWidth = 0;

            BoundingRectMinHeight = 0;
            BoundingRectMaxHeight = 0;

        }
    }
}
