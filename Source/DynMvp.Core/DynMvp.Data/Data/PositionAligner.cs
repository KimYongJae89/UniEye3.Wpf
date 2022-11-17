using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public class PositionAligner
    {
        public bool Fid1Error { get; set; }
        public bool Fid2Error { get; set; }

        public bool IsFidError()
        {
            return Fid1Error || Fid2Error;
        }
        public SizeF Offset { get; set; }
        public float Angle { get; set; }
        public float DesiredFiducialDistance { get; set; }
        public float FiducialDistance { get; set; }
        public float FiducialDistanceDiff { get; set; }
        public PointF RotationCenter { get; set; }
        public PointF FovCenter { get; set; }
        public Calibration Calibration { get; set; }

        //bool globalCoordination;
        //public bool GlobalCoordination
        //{
        //    get { return globalCoordination; }
        //    set { globalCoordination = value; }
        //}

        //Size imageSize;
        //public Size ImageSize
        //{
        //    get { return imageSize; }
        //    set { imageSize = value; }
        //}

        //SizeF scale = new SizeF(1, 1);
        //public SizeF Scale
        //{
        //    get { return scale; }
        //    set { scale = value; }
        //}

        //public bool StretchInspect()
        //{
        //    fiducialDistanceDiff = Math.Abs(desiredFiducialDistance - fiducialDistance);

        //    if (fiducialDistanceDiff > fiducialDistanceDiffTol)
        //        stretchResult = false;
        //    else
        //        stretchResult = true;

        //    return stretchResult;
        //}

        public PointF AlignFov(PointF position, PointF robotPos)
        {
            // 카메라 중심 기준의 
            var posFromCenter = PointF.Subtract(position, new SizeF(robotPos));
            var posFromLeftTop = PointF.Add(posFromCenter, new SizeF(FovCenter));

            return Calibration.WorldToPixel(posFromLeftTop);
        }

        public RotatedRect AlignFov(RotatedRect rect, PointF robotPos)
        {
            RotatedRect alignedRect = Align(rect);
            PointF alignedCenter = DrawingHelper.CenterPoint(alignedRect);
            PointF alignedCenterPel;
            PointF pelSize;

            if (Calibration != null)
            {
                alignedCenterPel = AlignFov(alignedCenter, robotPos);
                pelSize = Calibration.WorldToPixel(new PointF(rect.Width, rect.Height));
            }
            else
            {
                alignedCenterPel = alignedCenter;
                pelSize = new PointF(0, 0);
            }

            return new RotatedRect(alignedCenterPel, new SizeF(pelSize), alignedRect.Angle);
        }

        public PointF Align(PointF position)
        {
            var newPosition = PointF.Subtract(position, Offset);
            return MathHelper.Rotate(newPosition, RotationCenter, Angle);
        }

        public RotatedRect Align(RotatedRect rect)
        {
            PointF centerPoint = Align(DrawingHelper.CenterPoint(rect));

            var halfSize = new SizeF(rect.Width / 2, rect.Height / 2);
            var rectangleF = new RectangleF(centerPoint.X - halfSize.Width, centerPoint.Y - halfSize.Height, rect.Width, rect.Height);

            return new RotatedRect(rectangleF, (rect.Angle + Angle) % 360);
        }
    }
}
