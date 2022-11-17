using DynMvp.Base;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public enum CrossSectionAxis
    {
        X, Y, Z
    }

    public enum SelectedPos
    {
        Start, End
    }

    public class ClipPos
    {
        public ClipPlaneName StartPlane { get; set; }
        public float StartRatio { get; set; }
        public ClipPlaneName EndPlane { get; set; }
        public float EndRatio { get; set; }
    }

    internal class ProfileCoordinate
    {
        public bool Vertical { get; set; } = false;
        public bool ScreenPosValid { get; private set; } = false;
        public bool WorldPosValid { get; private set; } = false;
        public PointF ScreenPos { get; private set; } = Point.Empty;
        public Point3d WorldPos { get; private set; } = new Point3d();

        public ProfileCoordinate()
        {
        }

        private void Reset()
        {
            ScreenPosValid = false;
            WorldPosValid = false;
            ScreenPos = Point.Empty;
            WorldPos = new Point3d();
        }

        private void SetScrrenPos(float x, float y)
        {
            ScreenPos = new PointF(x, y);
            ScreenPosValid = true;
        }
    }

    public class CrossSection
    {
        public bool Enabled { get; set; }
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        public List<Point3d> ProfilePtList { get; set; } = new List<Point3d>();

        public CrossSection()
        {

        }

        public void ClearProfileData()
        {

        }
        public void GetStep(out float stepX, out float stepY, int numStep = 0)
        {
            float lengthX = EndPoint.X - StartPoint.X;
            float lengthY = EndPoint.Y - StartPoint.Y;

            if (numStep == 0)
            {
                float length = MathHelper.GetLength(StartPoint, EndPoint);
                stepX = lengthX / length;
                stepY = lengthY / length;
            }
            else
            {
                stepX = lengthX / numStep;
                stepY = lengthY / numStep;
            }
        }
    }
}
