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
    public abstract class Shape
    {
        public abstract void Draw();
    }

    public class CircleShape : Shape
    {
        private int step;
        public Point3d CenterPt { get; set; }
        public float Radius { get; set; }

        public CircleShape(int step)
        {
            this.step = step;
        }

        public void Draw(Point3d centerPt, float radius)
        {
            double delta = 2.0 * Math.PI / step;

#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.TriangleFan);
#pragma warning restore CS0618 // Type or member is obsolete
            {
                GL.Vertex3(centerPt.X, centerPt.Y, centerPt.Z);
                for (int i = 0; i <= step; i++)
                {
                    double x = radius * Math.Cos(delta * i);
                    double z = radius * Math.Sin(delta * i);
                    GL.Vertex3(centerPt.X + x, centerPt.Y, centerPt.Z + z);
                }
            }
            GL.End();
        }

        public override void Draw()
        {
            Draw(CenterPt, Radius);
        }
    }

    public class SphereShape : Shape
    {
        private Vertex[] SphereVertices;
        private ushort[] SphereElements;
        public Point3d CenterPt { get; set; }
        public RectangleF ScreenRect { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public SphereShape(float radius, float height, byte segments, byte rings)
        {
            SphereVertices = CalculateVertices2(radius, height, segments, rings);
            SphereElements = CalculateElements(radius, height, segments, rings);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            Draw(CenterPt);
        }

        public void Draw(TrackPos trackPos)
        {
            trackPos.UpdateMatrix();

            Draw(trackPos.GetPosition());

            trackPos.TrackRectangle = ScreenRect;
        }

        public void Draw(Point3d centerPt)
        {
            ScreenRect = RectangleF.Empty;

            Draw(new Vector3((float)centerPt.X, (float)centerPt.Y, (float)centerPt.Z));
        }

        public void Draw(Vector3 cneterPt)
        {
            GL.MatrixMode(MatrixMode.Modelview);

            GL.PushMatrix();
            GL.Translate(cneterPt);

            double[] surfaceModelVec = new double[16];
            double[] surfaceProjVec = new double[16];
            int[] surfaceViewportVec = new int[4];

            GL.GetDouble(GetPName.ModelviewMatrix, surfaceModelVec);
            GL.GetDouble(GetPName.ProjectionMatrix, surfaceProjVec);
            GL.GetInteger(GetPName.Viewport, surfaceViewportVec);

#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.TriangleFan);
#pragma warning restore CS0618 // Type or member is obsolete
            foreach (ushort element in SphereElements)
            {
                Vertex vertex = SphereVertices[element];
                GL.TexCoord2(vertex.TexCoord);
                GL.Normal3(vertex.Normal);
                GL.Vertex3(vertex.Position);

#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                OpenTK.Graphics.Glu.Project(vertex.Position, surfaceModelVec, surfaceProjVec, surfaceViewportVec, out Vector3 screenPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                float y = surfaceViewportVec[3] - screenPt.Y;

                if (ScreenRect == RectangleF.Empty)
                {
                    ScreenRect = RectangleF.FromLTRB(screenPt.X, y, screenPt.X, y);
                }
                else
                {
                    ScreenRect = RectangleF.Union(ScreenRect, RectangleF.FromLTRB(screenPt.X, y, screenPt.X, y));
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vertex[] CalculateVertices2(float radius, float height, byte segments, byte rings)
        {
            var data = new Vertex[segments * rings];

            int i = 0;

            for (double y = 0; y < rings; y++)
            {
                double phi = (y / (rings - 1)) * Math.PI; //was /2 
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;

                    var v = new Vector3()
                    {
                        X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
                        Y = (float)(height * Math.Cos(phi)),
                        Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
                    };
                    var n = Vector3.Normalize(v);
                    var uv = new Vector2()
                    {
                        X = (float)(x / (segments - 1)),
                        Y = (float)(y / (rings - 1))
                    };
                    // Using data[i++] causes i to be incremented multiple times in Mono 2.2 (bug #479506).
                    data[i] = new Vertex() { Position = v, Normal = n, TexCoord = uv };
                    i++;
                }
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ushort[] CalculateElements(float radius, float height, byte segments, byte rings)
        {
            int num_vertices = segments * rings;
            ushort[] data = new ushort[num_vertices * 6];

            ushort i = 0;

            for (byte y = 0; y < rings - 1; y++)
            {
                for (byte x = 0; x < segments - 1; x++)
                {
                    data[i++] = (ushort)((y + 0) * segments + x);
                    data[i++] = (ushort)((y + 1) * segments + x);
                    data[i++] = (ushort)((y + 1) * segments + x + 1);

                    data[i++] = (ushort)((y + 1) * segments + x + 1);
                    data[i++] = (ushort)((y + 0) * segments + x + 1);
                    data[i++] = (ushort)((y + 0) * segments + x);
                }
            }

            // Verify that we don't access any vertices out of bounds:
            foreach (int index in data)
            {
                if (index >= segments * rings)
                {
                    throw new IndexOutOfRangeException();
                }
            }

            return data;
        }

        /// <summary>
        /// mimic InterleavedArrayFormat.T2fN3fV3f
        /// </summary>
        public struct Vertex
        {
            public Vector2 TexCoord;
            public Vector3 Normal;
            public Vector3 Position;
        }
        //public class LineShape
        //{
        //    public abstract void Render();
        //}
    }

    internal class ArrowShape : Shape
    {
        private string name;

        public ArrowShape(string name)
        {
            this.name = name;
        }

        public override void Draw()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.LineLoop);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.Vertex3(0, 0, 0.5f);
            GL.Vertex3(0, 0, -0.5f);
            GL.Vertex3(2, 0, -0.5f);
            GL.Vertex3(2, 0, -1.0f);
            GL.Vertex3(3, 0, 0.0f);
            GL.Vertex3(2, 0, 1.0f);
            GL.Vertex3(2, 0, 0.5f);
            GL.End();

            float fSize = 2;
            float fOff = (fSize / 2.0f) * name.Length;

            GL.PushMatrix();
            GL.Translate(4 + fOff, 0, -0.5f);
            GL.Rotate(90, 1, 0, 0);
            GL.Scale(fSize, fSize, 0);

            var textShape = new TextShape();
            textShape.Initialize(name, 0, 0, 1, 1);
            textShape.Draw();

            GL.PopMatrix();
        }
    }

    internal class GridShape : Shape
    {
        private bool gridCreated;
        private int gridHandle;
        private SizeF gridSize;
        private float step;

        public GridShape(SizeF gridSize, float step)
        {
            this.gridSize = gridSize;
            this.step = step;
        }

        public override void Draw()
        {
            if (!gridCreated)
            {
                GL.LineWidth(1);

                float halfWidth = gridSize.Width / 2;
                float halfHeight = gridSize.Height / 2;

                gridHandle = GL.GenLists(1);

                GL.NewList(gridHandle, ListMode.Compile);
                GL.Disable(EnableCap.Lighting);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Quads);
#pragma warning restore CS0618 // Type or member is obsolete
                GL.Color4(1, 1, 1, 0.2f);
                GL.Vertex3(-halfWidth, 0, -halfHeight);
                GL.Vertex3(halfWidth, 0, -halfHeight);
                GL.Vertex3(halfWidth, 0, halfHeight);
                GL.Vertex3(-halfWidth, 0, halfHeight);
                GL.End();

                GL.Disable(EnableCap.Blend);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Lines);
#pragma warning restore CS0618 // Type or member is obsolete

                GL.Color3(0.3f, 0.3f, 0.3f);
                for (float i = -halfWidth; i <= halfWidth; i += step)
                {
                    GL.Vertex3(i, 0, -halfWidth);
                    GL.Vertex3(i, 0, halfWidth);
                }

                for (float i = -halfHeight; i <= halfHeight; i += step)
                {
                    GL.Vertex3(-halfHeight, 0, i);
                    GL.Vertex3(halfHeight, 0, i);
                }

                // x-axis
                GL.Color3(0.5f, 0.0f, 0.0f);
                GL.Vertex3(-halfWidth, 0, 0);
                GL.Vertex3(halfWidth, 0, 0);

                // z-axis
                GL.Color3(0.0f, 0.0f, 0.5f);
                GL.Vertex3(0, 0, -halfHeight);
                GL.Vertex3(0, 0, halfHeight);

                GL.End();
                GL.Color3(1.0f, 1.0f, 1.0f);

                var xArrow = new ArrowShape("X+");
                GL.PushMatrix();
                GL.Translate(-10, 0, 11);
                xArrow.Draw();
                GL.PopMatrix();

                var yArrow = new ArrowShape("Y+");
                GL.PushMatrix();
                GL.Rotate(90.0f, 0, 1, 0);
                GL.Translate(-10, 0, -11);
                yArrow.Draw();
                GL.PopMatrix();

                GL.Enable(EnableCap.Lighting);
                GL.EndList();

                gridCreated = true;
            }

            GL.CallList(gridHandle);
        }
    }
}
