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
    internal class ProfilePanel : SubPanel
    {
        public CrossSection CrossSection { get; set; }

        internal override void SetupCoordinate()
        {
            base.SetupCoordinate();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 projectionMat;
            projectionMat = Matrix4.CreateOrthographicOffCenter(0, viewport.Width, 0, viewport.Height, -1.0f, 1.0f);

            GL.LoadMatrix(ref projectionMat);
        }

        public override void DrawObject()
        {
            const int subViewTransparent = 50;

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.LineSmooth);

            float margin = 10;
            float f_l = margin;
            float f_b = margin;
            float f_w = viewport.Width - margin * 2;
            float f_h = viewport.Height - margin * 2;

            GL.Color4(0.14f, 0.14f, 0.14f, subViewTransparent / 100.0f);
            GL.Rect(0, 0, viewport.Width, viewport.Height);

            Point3d[] pointArray = CrossSection.ProfilePtList.ToArray();
            var left = new Vector2();
            var right = new Vector2();
            if (pointArray == null || pointArray.Count() == 0)
            {
                GL.PopMatrix();
                left = new Vector2(viewport.Width * 1 / 3, viewport.Height - 50);
                right = new Vector2(viewport.Width * 2 / 3, viewport.Height - 50);
                return;
            }

            float maxValue = (float)pointArray.Max(x => x.Z);
            float minValue = (float)pointArray.Min(x => x.Z);
            float bound = maxValue - minValue;

            GL.LineWidth(2);
            GL.Color3(1.0f, 1.0f, 1.0f);
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.LineStrip);
#pragma warning restore CS0618 // Type or member is obsolete

            int index = 0;
            foreach (Point3d point in pointArray)
            {
                float f_x = f_l + f_w * (index / (float)pointArray.Count());
                float f_y = f_b + f_h * (((float)point.Z - minValue) / bound);
                GL.Vertex2(f_x, f_y);

                index++;
            }

            GL.End();

            DrawMeasure(viewport.Width, viewport.Height, ref left, ref right);
        }

        private void DrawMeasure(int width, int height, ref Vector2 left, ref Vector2 right)
        {
            if (left.X >= (width / 2) || left.X <= 0)
            {
                left = new Vector2(width * 1 / 3, height - 50);
            }

            if (right.X >= (width / 2) || right.X <= width)
            {
                right = new Vector2(width * 2 / 3, height - 50);
            }

            GL.PushMatrix();
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.Lines);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.LineWidth(3);
            GL.Color3(1.0f, 0.0f, 1.0f);
            GL.LoadName((int)ObjName.Left);
            GL.Vertex2(left.X, 0f);
            GL.Vertex2(left.X, height - (height / 4));
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.Lines);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.LineWidth(10);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.LoadName((int)ObjName.Right);
            GL.Vertex2(right.X, 0f);
            GL.Vertex2(right.X, height - (height / 4));
            GL.End();
            GL.PopMatrix();
        }
    }
}
