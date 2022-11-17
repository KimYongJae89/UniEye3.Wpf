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
    internal enum TextureDimType
    {
        One, Two, Mixed
    }

    internal abstract class SubPanel
    {
        protected Point3d eyePoint;
        internal Point3d EyePoint
        {
            get => eyePoint;
            set => eyePoint = value;
        }

        protected Point3d eyeAngle;
        internal Point3d EyeAngle
        {
            get => eyeAngle;
            set => eyeAngle = value;
        }

        protected Rectangle viewport;
        internal Rectangle Viewport
        {
            get => viewport;
            set => viewport = value;
        }

        protected ProjectionType projectionType = ProjectionType.Perspective;
        internal ProjectionType ProjectionType
        {
            get => projectionType;
            set => projectionType = value;
        }

        protected Point3d planeSize;
        internal Point3d PlaneSize
        {
            get => planeSize;
            set => planeSize = value;
        }

        protected double[] modelVec = new double[16];
        protected double[] projectionVec = new double[16];
        protected int[] viewportVec = new int[4];

        protected int drawList = 0;
        protected bool drawListCreated = false;

        protected PolygonMode polygonMode = PolygonMode.Fill;
        internal PolygonMode PolygonMode
        {
            get => polygonMode;
            set
            {
                polygonMode = value;
                ResetDrawList();
            }
        }

        internal void ResetDrawList()
        {
            if (drawListCreated)
            {
                GL.DeleteLists(drawList, 1);
                drawListCreated = false;
            }
        }

        internal virtual void SetupCoordinate()
        {
            GL.PushMatrix();

            GL.Viewport(viewport.X, viewport.Y, viewport.Width, viewport.Height);
            GL.Scissor(viewport.X, viewport.Y, viewport.Width, viewport.Height);

            SetupProjectionTransform();

            SetupEyePoint();

            GL.PolygonMode(MaterialFace.FrontAndBack, polygonMode);
        }

        internal virtual void SetupProjectionTransform()
        {
            int width = viewport.Width;
            int height = viewport.Height;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspectRatio = width / (float)height;

            Matrix4 projectionMat;
            if (projectionType == ProjectionType.Perspective)
            {
                projectionMat = Matrix4.CreatePerspectiveFieldOfView(0.8f /*(float)(System.Math.PI / 3.5)*/, aspectRatio, 1, 100);
                //                projectionMat = Matrix4.CreatePerspectiveOffCenter(xRange.Min, xRange.Max, yRange.Min, yRange.Max, (float)50/*eyePoint.Z*/, 100);
            }
            else
            {
                float halfWidth = (float)planeSize.X / 2;

                if (width <= height)
                {
                    projectionMat = Matrix4.CreateOrthographicOffCenter(-halfWidth, halfWidth, -halfWidth / aspectRatio, halfWidth / aspectRatio, -100, 100);
                }
                else
                {
                    projectionMat = Matrix4.CreateOrthographicOffCenter(-halfWidth * aspectRatio, halfWidth * aspectRatio, -halfWidth, halfWidth, -100, 100);
                }

                //                projectionMat = Matrix4.CreateOrthographicOffCenter(xRange.Min, xRange.Max, yRange.Min, yRange.Max, zRange.Min, zRange.Max);
            }
            GL.LoadMatrix(ref projectionMat);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        internal virtual void SetupEyePoint()
        {
            GL.Translate(eyePoint.X, eyePoint.Y, -eyePoint.Z);
            GL.Rotate(eyeAngle.X, 1, 0, 0);
            GL.Rotate(eyeAngle.Y, 0, 1, 0);
        }

        internal virtual void PostDraw()
        {
            GL.GetDouble(GetPName.ModelviewMatrix, modelVec);
            GL.GetDouble(GetPName.ProjectionMatrix, projectionVec);
            GL.GetInteger(GetPName.Viewport, viewportVec);

            GL.PopMatrix();
        }

        internal virtual void Draw()
        {
            SetupCoordinate();

            GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 0, 100, 10, 0 });

            DrawObject();

            PostDraw();
        }

        internal void SetPlaneXy()
        {
            projectionType = ProjectionType.Ortho;

            eyeAngle.X = 90;
            eyeAngle.Y = 0;
            eyePoint.X = 0;
            eyePoint.Z = 0;
        }

        internal void SetPlaneXz()
        {
            projectionType = ProjectionType.Ortho;

            eyeAngle.X = 0;
            eyeAngle.Y = 0;
            eyePoint.X = 0;
            eyePoint.Z = 0;
        }

        internal void SetPlaneYz()
        {
            projectionType = ProjectionType.Ortho;

            eyeAngle.X = 0;
            eyeAngle.Y = 90;
            eyePoint.X = 0;
            eyePoint.Z = 0;
        }

        public abstract void DrawObject();
    }
}
