using DynMvp.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DynMvp.GerberCvt.GerbPad
{
    public class GerberData : GerberCvt.GerberData
    {
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float OffsetXFromRightBottom { get; set; }
        public float OffsetYFromRightBottom { get; set; }
        public float BmpOriginX { get; set; }
        public float BmpOriginY { get; set; }
        public float BmpSizeX { get; set; }
        public float BmpSizeY { get; set; }
        public bool CombineArray { get; set; }
        public int NumModule { get; set; }
        public int NumFiducial { get; set; }
        public int NumBadMark { get; set; }
        public int NumPattern { get; set; }
        public int NumPad { get; set; }
        public int NumFov { get; set; }
        public List<Pad> PadList { get; } = new List<Pad>();

        public void AddFiducial(Fiducial fiducial)
        {
            fiducialList.Add(fiducial);
        }

        public void AddBadMark(BadMark badMark)
        {
            badMarkList.Add(badMark);
        }

        public void AddPad(Pad pad)
        {
            PadList.Add(pad);
        }

        public void AddFov(Fov fov)
        {
            fovList.Add(fov);
        }

        public PatternEdge GetLastPatternEdge()
        {
            if (patternEdgeList.Count > 0)
            {
                return patternEdgeList.Last();
            }

            return null;
        }

        public override RectangleF BoardRectangleF()
        {
            var leftBottom = new PointF(-BoardSize.Width - OffsetXFromRightBottom, -OffsetYFromRightBottom);
            return new RectangleF(leftBottom, BoardSize);
        }

        public override void Offset(float offsetX, float offsetY)
        {
            foreach (Pad pad in PadList)
            {
                pad.Offset(offsetX, offsetY);
            }

            foreach (Component component in componentList)
            {
                component.Offset(offsetX, offsetY);
            }

            foreach (Fov fov in fovList)
            {
                fov.Offset(offsetX, offsetY);
            }

            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.OffsetPos(offsetX, offsetY);
            }

            foreach (BadMark badMark in badMarkList)
            {
                badMark.OffsetPos(offsetX, offsetY);
            }

            foreach (ModuleGroup moduleGroup in moduleGroupList)
            {
                moduleGroup.Offset(offsetX, offsetY);
            }
        }

        public override void FlipX(float centerX)
        {
            foreach (Pad pad in PadList)
            {
                pad.FlipX(centerX);
            }

            foreach (Component component in componentList)
            {
                component.FlipX(centerX);
            }

            foreach (Fov fov in fovList)
            {
                fov.FlipX(centerX);
            }

            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.FlipX(centerX);
            }

            foreach (BadMark badMark in badMarkList)
            {
                badMark.FlipX(centerX);
            }

            foreach (ModuleGroup moduleGroup in moduleGroupList)
            {
                moduleGroup.FlipX(centerX);
            }
        }

        public override void FlipY(float centerY)
        {
            foreach (Pad pad in PadList)
            {
                pad.FlipY(centerY);
            }

            foreach (Component component in componentList)
            {
                component.FlipY(centerY);
            }

            foreach (Fov fov in fovList)
            {
                fov.FlipY(centerY);
            }

            foreach (Fiducial fiducial in fiducialList)
            {
                fiducial.FlipY(centerY);
            }

            foreach (BadMark badMark in badMarkList)
            {
                badMark.FlipY(centerY);
            }

            foreach (ModuleGroup moduleGroup in moduleGroupList)
            {
                moduleGroup.FlipY(centerY);
            }
        }

        public override List<Fov> GetFovList(SizeF fovSize, float fovMargin, bool rebuild)
        {
            if (fovList.Count != 0)
            {
                return fovList;
            }

            //GenerateFovListForPad(ref fovList, fovSize, fovMargin);
            GenerateFovListForComponent(ref fovList, fovSize, fovMargin);

            if (fovList.Count == 0)
            {
                return fovList;
            }

            PointF startPt = fovList[0].CenterPos;
            if (fiducialList.Count != 0)
            {
                startPt = fiducialList[fiducialList.Count - 1].Pos;
            }

            if (badMarkList.Count != 0)
            {
                startPt = badMarkList[badMarkList.Count - 1].Pos;
            }

            return TravellingSalesmanPath(startPt, fovList, rebuild);
        }

        public override List<Pad> GetPadList()
        {
            return PadList;
        }

        public override Figure CreateFiducialFigure(Fiducial fiducial)
        {
            return fiducial.CreateFigure();
        }

        public override Figure CreateBadMarkFigure(BadMark badMark)
        {
            return badMark.CreateFigure();
        }

        /*
        public override Figure CreateComponentFigure(Component component)
        {
            return component.CreateFigure();
        }
        */
    }
}
