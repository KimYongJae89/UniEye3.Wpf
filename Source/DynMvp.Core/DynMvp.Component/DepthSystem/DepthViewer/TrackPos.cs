using DynMvp.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public class TrackPos
    {
        private double[] surfaceModelVec = new double[16];
        private double[] surfaceProjVec = new double[16];
        private int[] surfaceViewportVec = new int[4];
        public float TrackHeight { get; set; }
        public RectangleF TrackRectangle { get; set; } = RectangleF.Empty;

        public bool IsInitialized()
        {
            return TrackRectangle != RectangleF.Empty;
        }

        public void UpdateMatrix()
        {
            GL.GetDouble(GetPName.ModelviewMatrix, surfaceModelVec);
            GL.GetDouble(GetPName.ProjectionMatrix, surfaceProjVec);
            GL.GetInteger(GetPName.Viewport, surfaceViewportVec);
        }

        public Vector3 GetPosition()
        {
            PointF centerPt = DrawingHelper.CenterPoint(TrackRectangle);
            var srcPt = new Vector3();
            srcPt.X = centerPt.X;
            srcPt.Y = surfaceViewportVec[3] - centerPt.Y;
            srcPt.Z = TrackHeight;

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            OpenTK.Graphics.Glu.UnProject(srcPt, surfaceModelVec, surfaceProjVec, surfaceViewportVec, out Vector3 destPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

            return destPt;
        }
    }
}
