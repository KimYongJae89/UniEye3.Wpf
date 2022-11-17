using DynMvp.Base;
using DynMvp.Cad;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    internal class DepthViewPanel : SubPanel
    {
        private Vector3f[] normalVector;
        private static ushort[] linePattern = new ushort[] { 0x0f0f, 0x8787, 0xc3c3, 0xe1e1, 0xf0f0, 0x7878, 0x3c3c, 0x1e1e };

        protected float minValue;
        public float MinValue => minValue;
        public float MaxValue { get; private set; }
        public float RangeRatioMin { get; set; } = 0;
        public float RangeRatioMax { get; set; } = 1;
        public bool UseMultiTexture { get; set; }
        public TextureDimType TextureDimType { get; set; }

        private SurfaceMode surfaceMode;
        private TextureDimType currentTextureDimType;
        public float HeightScale { get; set; } = 1.0f;
        public float GridStep { get; set; } = 1.0f;

        internal void InitCoordinate(Rectangle viewport, Point3d planeSize, int gridStep)
        {
            this.viewport = viewport;
            this.planeSize = planeSize;
            GridStep = gridStep;
        }

        private SizeF pixelResolution;
        private Image3D depthData;
        private LightMap lightMap = new LightMap();
        public TextureColor TextureColor { get; set; }
        public CrossSection CrossSection { get; set; }

        public bool IsValid()
        {
            return depthData != null;
        }

        public DepthViewPanel()
        {
            eyePoint = new Point3d(0, 0, 25.0f);
            eyeAngle = new Point3d(25.0f, -35.0f, 0.0f);
        }

        internal void SetTextureColor(TextureColorType textureColorType)
        {
            if (TextureColor == null)
            {
                TextureColor = new TextureColor();
                lightMap.TextureColor = TextureColor;
            }

            const int NUM_COLOR_LEVEL = 512;

            switch (textureColorType)
            {
                case TextureColorType.Rainbow:
                    TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, RangeRatioMin, RangeRatioMax);
                    break;
                case TextureColorType.Hsv1:
                    TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, RangeRatioMin, RangeRatioMax, Color.FromArgb(0, 162, 232), Color.FromArgb(255, 128, 0));
                    break;
                case TextureColorType.Hsv2:
                    TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, RangeRatioMin, RangeRatioMax, Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
                    break;
                case TextureColorType.Gray:
                    TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, RangeRatioMin, RangeRatioMax, Color.FromArgb(0, 0, 0), Color.FromArgb(255, 0, 0));
                    break;
            }
        }

        public void SetDepthData(Image3D depthData, SizeF pixelResolution)
        {
            this.depthData = depthData;
            this.pixelResolution = pixelResolution;

            minValue = depthData.Data.Min();
            MaxValue = depthData.Data.Max();

            planeSize = new Point3d(depthData.Width * pixelResolution.Width, depthData.Height * pixelResolution.Height, MaxValue - minValue);

            Normalize();

            ResetDrawList();
        }

        private void Normalize()
        {
            float[] depthRawData = depthData.Data;
            float fMin;
            fMin = 0.0f;

            fMin = minValue;

            for (int y = 0; y < depthData.Height; y++)
            {
                for (int x = 0; x < depthData.Width; x++)
                {
                    float depthValue = depthRawData[y * depthData.Width + x];

                    depthValue -= minValue;
                    if (depthValue < fMin)
                    {
                        fMin = depthValue;
                    }

                    depthRawData[y * depthData.Width + x] = depthValue;
                }
            }

            minValue = Math.Min(fMin, minValue) - 1;

            CalcNormals();
        }

        private void CalcNormals()
        {
            int width = depthData.Width;
            int height = depthData.Height;

            float[] depthRawData = depthData.Data;
            var norm = new Vector3f[width * height];
            var outVec = new Vector3f();
            var inVec = new Vector3f();
            var leftVec = new Vector3f();
            var rightVec = new Vector3f();

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    var sum = new Vector3f();

                    float depthValue = depthRawData[index];

                    if (y > 0)
                    {
                        outVec = new Vector3f(0.0f, depthRawData[(y - 1) * width + x] - depthValue, -1.0f);
                    }

                    if (y < height - 1)
                    {
                        inVec = new Vector3f(0.0f, depthRawData[(y + 1) * width + x] - depthValue, 1.0f);
                    }

                    if (x > 0)
                    {
                        leftVec = new Vector3f(-1.0f, depthRawData[y * width + (x - 1)] - depthValue, 0.0f);
                    }

                    if (x < width - 1)
                    {
                        rightVec = new Vector3f(1.0f, depthRawData[y * width + (x + 1)] - depthValue, 0.0f);
                    }

                    if (x > 0 && y > 0)
                    {
                        sum = sum + outVec.Cross(leftVec).Normalize();
                    }

                    if (x > 0 && y < height - 1)
                    {
                        sum = sum + leftVec.Cross(inVec).Normalize();
                    }

                    if (x < width - 1 && y < height - 1)
                    {
                        sum = sum + inVec.Cross(rightVec).Normalize();
                    }

                    if (x < width - 1 && y > 0)
                    {
                        sum = sum + rightVec.Cross(outVec).Normalize();
                    }

                    norm[index] = sum;
                }
            });

            normalVector = new Vector3f[depthData.Width * depthData.Height];

            const float FALLOUT_RATIO = 1.0f;
            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    Vector3f sum = norm[index];

                    if (x > 0)
                    {
                        sum = sum + norm[y * width + (x - 1)] * FALLOUT_RATIO;
                    }

                    if (x < width - 1)
                    {
                        sum = sum + norm[y * width + (x + 1)] * FALLOUT_RATIO;
                    }

                    if (y > 0)
                    {
                        sum = sum + norm[(y - 1) * width + x] * FALLOUT_RATIO;
                    }

                    if (y < height - 1)
                    {
                        sum = sum + norm[(y + 1) * width + x] * FALLOUT_RATIO;
                    }

                    if (sum.Magnitude() == 0)
                    {
                        sum = new Vector3f(0.0f, 1.0f, 0.0f);
                    }

                    normalVector[y * width + x] = sum;
                }
            });
        }

        public void GetDisplayRange(out float rangeMin, out float rangeMax)
        {
            float valueRange = MaxValue - minValue;

            rangeMin = (valueRange * RangeRatioMin) + minValue;
            rangeMax = (valueRange * RangeRatioMax) + MaxValue;
        }

        public Point3d[] GetCrossSectionData(CrossSection crossSection)
        {
            if (depthData == null)
            {
                return null;
            }

            float[] depthRawData = depthData.Data;

            var profilePtList = new List<Point3d>();

            PointF startPt = crossSection.StartPoint;
            PointF endPt = crossSection.EndPoint;
            crossSection.GetStep(out float stepX, out float stepY);

            float x, y;
            for (y = startPt.Y, x = startPt.X; (Math.Abs(y - endPt.Y) > Math.Abs(stepY) * 2) && (Math.Abs(x - endPt.X) > Math.Abs(stepX)); y += stepY, x += stepX)
            {
                int index = depthData.Width * (int)y + (int)x;
                profilePtList.Add(new Point3d(x, y, depthRawData[index]));
            }

            crossSection.ProfilePtList = profilePtList;

            return profilePtList.ToArray();
        }

        public void SetSurfaceMode(SurfaceMode surfaceMode)
        {
            this.surfaceMode = surfaceMode;

            TextureDimType = TextureDimType.One;
            switch (surfaceMode)
            {
                case SurfaceMode.TextureColor: TextureDimType = TextureDimType.One; break;
                case SurfaceMode.Image: TextureDimType = TextureDimType.Two; break;
                case SurfaceMode.Mixed: TextureDimType = TextureDimType.Mixed; break;
            }
        }

        public override void DrawObject()
        {
            var gridShape = new GridShape(new SizeF((float)planeSize.X, (float)planeSize.Y), 10);
            gridShape.Draw();

            if (depthData == null)
            {
                return;
            }

            lightMap.Set(0, surfaceMode);

            float xScale = (float)viewport.Width / depthData.Width;
            float yScale = (float)viewport.Height / depthData.Height;
            float scale = Math.Max(xScale, yScale);

            //             GridShape gridShape = new GridShape(XRange, ZRange, (int)(XRange.Range / 10));

            float fScaleZ = (float)planeSize.Z * 0.005f;
            GetDisplayRange(out float rangeMin, out float rangeMax);

            // float offsetZ = -((valueRange * (rangeMin - minValue) / valueRange)) * fScaleZ;

            //float longSideSize = (Width > Height) ? Width : Height;
            //float fScale = coordSize / longSideSize;

            GL.Translate((-scale * planeSize.X) / 2, 0, -(scale * planeSize.Y) / 2);
            GL.Scale(scale, fScaleZ, scale);

            if (drawListCreated && UseMultiTexture == false)
            {
                if (currentTextureDimType != TextureDimType)
                {
                    ResetDrawList();
                }
            }

            if (drawListCreated == false)
            {
                CreateDrawList();
            }

            GL.CallList(drawList);
        }

        private void DrawCrossSectionLine()
        {
            //if (crossSection.Enabled) //  && ((clock() / 200) % 2 == 0))
            {
                Point3d[] profilePtArray = GetCrossSectionData(CrossSection);
                if (profilePtArray == null)
                {
                    return;
                }

                GL.Color4(0.14f, 0.14f, 0.14f, 50 / 100.0f);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Polygon);
#pragma warning restore CS0618 // Type or member is obsolete
                GL.Vertex3(CrossSection.StartPoint.X, 0, CrossSection.StartPoint.Y);
                GL.Vertex3(CrossSection.StartPoint.X, MaxValue, CrossSection.StartPoint.Y);
                GL.Vertex3(CrossSection.EndPoint.X, MaxValue, CrossSection.EndPoint.Y);
                GL.Vertex3(CrossSection.EndPoint.X, 0, CrossSection.EndPoint.Y);
                GL.End();

                GL.Color4(0.0f, 1.0f, 0.0f, 80 / 100.0f);
#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Polygon);
#pragma warning restore CS0618 // Type or member is obsolete
                GL.Vertex3(CrossSection.StartPoint.X, 0, CrossSection.StartPoint.Y);
                GL.Vertex3(CrossSection.StartPoint.X, MaxValue, CrossSection.StartPoint.Y);
                GL.Vertex3(CrossSection.EndPoint.X, MaxValue, CrossSection.EndPoint.Y);
                GL.Vertex3(CrossSection.EndPoint.X, 0, CrossSection.EndPoint.Y);
                GL.End();

                GL.Color3(1.0f, 1.0f, 1.0f);
                GL.LineWidth(3);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.LineStrip);
#pragma warning restore CS0618 // Type or member is obsolete

                foreach (Point3d profilePt in profilePtArray)
                {
                    GL.Vertex3(profilePt.X, profilePt.Z, profilePt.Y);
                }
                GL.End();
                GL.LineWidth(1);
            }
        }

        public void SelectPoint(int ptX, int ptY, bool controlPushed)
        {
            var point = new Vector3();
            point.X = ptX;
            point.Y = (float)viewportVec[3] - ptY;
            float zValue = 0;
            GL.ReadPixels((int)point.X, (int)point.Y, 1, 1, PixelFormat.DepthComponent, PixelType.Float, ref zValue);
            point.Z = zValue;

            if (point.Z != 1)
            {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                OpenTK.Graphics.Glu.UnProject(point, modelVec, projectionVec, viewportVec, out Vector3 destPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                if (controlPushed)
                {
                    CrossSection.EndPoint = new PointF(destPt.X, destPt.Z);
                }
                else
                {
                    CrossSection.StartPoint = new PointF(destPt.X, destPt.Z);
                }
            }
        }

        //void SectionClip(bool setFlag, CrossSection clipData)
        //{
        //    if (clipData == null)
        //        return;

        //    if (setFlag && clipData.Enabled == false)
        //        return;

        //    SectionClip(setFlag, CrossSectionAxis.X, clipData.GetClipPos(CrossSectionAxis.X));
        //    SectionClip(setFlag, CrossSectionAxis.Y, clipData.GetClipPos(CrossSectionAxis.Y));
        //    SectionClip(setFlag, CrossSectionAxis.Z, clipData.GetClipPos(CrossSectionAxis.Z));
        //}

        //void SectionClip(bool setFlag, CrossSectionAxis clipAxis, ClipPos clipPos)
        //{
        //    if (setFlag == false)
        //    {
        //        GL.Disable((EnableCap)clipPos.StartPlane);
        //        GL.Disable((EnableCap)clipPos.EndPlane);
        //        return;
        //    }

        //    float distance = 0.0f;
        //    int idx = -1;
        //    switch (clipAxis)
        //    {
        //        case CrossSectionAxis.X:
        //            distance = (float)depthData.Width;
        //            idx = 0;
        //            break;
        //        case CrossSectionAxis.Y:
        //            distance = (float)depthData.Height;
        //            idx = 2;
        //            break;
        //        case CrossSectionAxis.Z:
        //            distance = (maxValue - minValue) + 1;
        //            idx = 1;
        //            break;
        //        default:
        //            break;
        //    }

        //    if (distance == 0.0 || idx < 0)
        //        return;

        //    double[] equ1 = new double[4] { 0, 0, 0, -(distance * clipPos.StartRatio) };
        //    double[] equ2 = new double[4] { 0, 0, 0, (distance * (1 - clipPos.EndRatio)) };
        //    equ1[idx] = 1;
        //    equ2[idx] = -1;

        //    GL.ClipPlane(clipPos.StartPlane, equ1);
        //    GL.ClipPlane(clipPos.EndPlane, equ2);

        //    GL.Enable((EnableCap)clipPos.StartPlane);
        //    GL.Enable((EnableCap)clipPos.EndPlane);

        //    return;
        //}

        private void CreateDrawList()
        {
            float[] data = depthData.Data;
            int ymax = depthData.Height - 1;
            int xmax = depthData.Width - 1;

            float objectHeight = (MaxValue - minValue) + 1;

            drawList = GL.GenLists(1);

            GL.NewList(drawList, ListMode.Compile);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvModeCombine.Modulate);

            GL.Color3(1.0f, 1.0f, 1.0f);

            float fymax = (float)ymax - 1;
            float fxmax = (float)xmax - 1;
            for (int y = 0; y < ymax; y++)
            {
                if (polygonMode == PolygonMode.Point)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    GL.Begin(BeginMode.Points);
                }
#pragma warning restore CS0618 // Type or member is obsolete
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    GL.Begin(BeginMode.TriangleStrip);
                }
#pragma warning restore CS0618 // Type or member is obsolete

                float fy1 = y / fymax;
                float fy2 = (y + 1) / fymax;
                for (int x = 0; x < xmax; x++)
                {
                    if (polygonMode == PolygonMode.Fill)
                    {
                        int index = y * depthData.Width + x;

                        Vector3f normal = normalVector[index];
                        GL.Normal3(normal[0], normal[1], normal[2]);

                        float fx = (x / fxmax) / 1.0f;
                        switch (TextureDimType)
                        {
                            case TextureDimType.One: GL.TexCoord1(data[index] / objectHeight); break;
                            case TextureDimType.Two: GL.TexCoord2(fx, fy1); break;
                            case TextureDimType.Mixed: GL.TexCoord1(data[index] / objectHeight); break;
                        }

                        GL.Vertex3(x, data[index], y);

                        index = (y + 1) * depthData.Width + x;
                        normal = normalVector[index];
                        GL.Normal3(normal[0], normal[1], normal[2]);

                        switch (TextureDimType)
                        {
                            case TextureDimType.One: GL.TexCoord1(data[index] / objectHeight); break;
                            case TextureDimType.Two: GL.TexCoord2(fx, fy2); break;
                            case TextureDimType.Mixed: GL.TexCoord1(data[index] / objectHeight); break;
                        }
                        GL.Vertex3(x, data[index], y + 1);
                    }
                    else if (polygonMode == PolygonMode.Line)
                    {
                        int index = y * depthData.Width + x;

                        GL.Vertex3(x, data[index], y);

                        index = (y + 1) * depthData.Width + x;
                        GL.Vertex3(x, data[index], y + 1);
                    }
                    else
                    {
                        int index = y * depthData.Width + x;

                        GL.Vertex3(x, data[index], y);
                    }
                }
                GL.End();
            }
            GL.EndList();

            currentTextureDimType = TextureDimType;
            drawListCreated = true;
        }
    }
}
