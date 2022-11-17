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
    internal class SurfaceRender
    {
        private Vector3f[] depthNormalVector;
        private Vector3[] cloudDepthPoint;
        private Vector3f[] cadDepthNormalVector;
        private Vector3[] cadDepthPoint;
        private static ushort[] linePattern = new ushort[] { 0x0f0f, 0x8787, 0xc3c3, 0xe1e1, 0xf0f0, 0x7878, 0x3c3c, 0x1e1e };
        private float depthMinValue;
        private float depthMaxValue;
        private float cadMinValue;
        private float cadMaxValue;

        public float MinValue
        {
            get
            {
                if (depthModel != null)
                {
                    return depthMinValue;
                }
                else if (cad3dModel != null)
                {
                    return cadMinValue;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float MaxValue
        {
            get
            {
                if (depthModel != null)
                {
                    return depthMaxValue;
                }
                else if (cad3dModel != null)
                {
                    return cadMaxValue;
                }
                else
                {
                    return 0;
                }
            }
        }
        public float RangeRatioMin { get; set; } = 0;
        public float RangeRatioMax { get; set; } = 1;
        public bool UseMultiTexture { get; set; }
        public bool NativeDraw { get; set; }

        public float cloudDepthDataWidth = 0;
        public float cloudDepthDataHeight = 0;
        public float cadDataWidth = 0;
        public float cadDataHeight = 0;

        public float Width
        {
            get
            {
                if (depthModel != null)
                {
                    return CloudDepthData.Width;
                }
                else
                {
                    return 20;
                }
            }
        }

        public float Height
        {
            get
            {
                if (depthModel != null)
                {
                    return CloudDepthData.Height;
                }
                else
                {
                    return 20;
                }
            }
        }

        public float DataWidth
        {
            get
            {
                if (depthModel != null)
                {
                    return cloudDepthDataWidth;
                }
                else if (cad3dModel != null)
                {
                    return cadDataWidth;
                }
                else
                {
                    return 20;
                }
            }
        }

        public float DataHeight
        {
            get
            {
                if (depthModel != null)
                {
                    return cloudDepthDataHeight;
                }
                else if (cad3dModel != null)
                {
                    return cadDataHeight;
                }
                else
                {
                    return 20;
                }
            }
        }
        public TextureDimType TextureDimType { get; set; }

        private PolygonMode polygonMode = PolygonMode.Fill;
        public PolygonMode PolygonMode
        {
            get => polygonMode;
            set
            {
                polygonMode = value;
                if (cloudDepthDrawListCreated)
                {
                    GL.DeleteLists(cloudDepthDrawList, 1);
                    cloudDepthDrawListCreated = false;
                }

                if (cadDrawListCreated)
                {
                    GL.DeleteLists(cadDrawList, 1);
                    cadDrawListCreated = false;
                }
            }
        }

        private int cloudDepthDrawList = 0;
        private int cadDepthDrawList = 0;
        private int cadDrawList = 0;
        private bool cloudDepthDrawListCreated = false;
        private bool cadDrawListCreated = false;
        private bool cadDepthDrawListCreated = false;
        private TextureDimType currentTextureDimType;
        public float HeightScale { get; set; } = 1.0f;
        public bool ShowDepth { get; set; } = true;
        public bool ShowCadDepth { get; set; } = true;
        public bool ShowCad { get; set; } = true;

        private float pixelResolution = 1.0f;
        public Image3D CloudDepthData { get; private set; }

        private Image3D cadDepthData;
        private Cad3dModel depthModel = null;
        private Cad3dModel cad3dModel;
        private Cad3dModel cadDepthModel;

        public bool IsValid()
        {
            return depthModel != null || cad3dModel != null;
        }

        public void ClearList()
        {
            if (cloudDepthDrawListCreated)
            {
                GL.DeleteLists(cloudDepthDrawList, 1);
                cloudDepthDrawListCreated = false;
            }

            if (cadDepthDrawListCreated)
            {
                GL.DeleteLists(cadDepthDrawList, 1);
                cadDepthDrawListCreated = false;
            }

            if (cadDrawListCreated)
            {
                GL.DeleteLists(cadDrawList, 1);
                cadDrawListCreated = false;
            }
        }
        public void SetDepthData(Image3D depthData, float pixelResolution)
        {
            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - SetDepthData 1");

            if (cloudDepthDrawListCreated)
            {
                GL.DeleteLists(cloudDepthDrawList, 1);
                cloudDepthDrawListCreated = false;
            }

            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - SetDepthData 2");

            CloudDepthData = depthData;
            if (depthData == null)
            {
                depthModel = null;
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - SetDepthData 3");

            depthMinValue = 0; //  depthData.Data.Min();
            depthMaxValue = Math.Max(50, CloudDepthData.Data.Max());
            cloudDepthDataWidth = CloudDepthData.Width * pixelResolution;
            cloudDepthDataHeight = CloudDepthData.Height * pixelResolution;
            RangeRatioMin = 0;
            RangeRatioMax = 1;

            this.pixelResolution = pixelResolution;

            Normalize(depthData);

            depthNormalVector = new Vector3f[CloudDepthData.Width * CloudDepthData.Height];
            cloudDepthPoint = new Vector3[CloudDepthData.Width * CloudDepthData.Height];

            CalcNormals(CloudDepthData, depthNormalVector, cloudDepthPoint);

            depthModel = CreateDepthModel(depthData, depthNormalVector);

            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - SetDepthData 4");
        }

        public void SetCadDepthData(Image3D cadDepthData)
        {
            if (cadDepthDrawListCreated)
            {
                GL.DeleteLists(cadDepthDrawList, 1);
                cadDepthDrawListCreated = false;
            }

            this.cadDepthData = cadDepthData;
            if (cadDepthData == null)
            {
                cadDepthModel = null;
                return;
            }

            cadDepthNormalVector = new Vector3f[cadDepthData.Width * cadDepthData.Height];
            cadDepthPoint = new Vector3[cadDepthData.Width * cadDepthData.Height];

            CalcNormals(cadDepthData, cadDepthNormalVector, cadDepthPoint);

            cadDepthModel = CreateDepthModel(cadDepthData, cadDepthNormalVector);
        }

        //internal void SetHeightValue(float minValue, float maxValue)
        //{
        //    depthMinValue = minValue;
        //    depthMaxValue = maxValue;
        //    cadMinValue = minValue;
        //    cadMaxValue = maxValue;

        //    if (depthDrawListCreated)
        //    {
        //        GL.DeleteLists(depthDrawList, 1);
        //        depthDrawListCreated = false;
        //    }

        //    depthModel = CreateDepthModel();
        //}

        private Vector3 ToVector3(Point3d point3d)
        {
            return new Vector3((float)point3d.X, (float)point3d.Y, (float)point3d.X);
        }

        public Cad3dModel CreateDepthModel(Image3D depthData, Vector3f[] normalVector)
        {
            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - CreateDepthModel");

            var depthModel = new Cad3dModel();

            float[] data = depthData.Data;

            Triangle triangle;

            int step = 1;
            for (int y = 0; y < depthData.Height - step; y += step)
            {
                for (int x = 0; x < depthData.Width - step; x += step)
                {
                    int index = y * depthData.Width + x;

                    triangle = new Triangle();

                    triangle.Vertex[0] = new Point3d(x, y, data[y * depthData.Width + x]);
                    triangle.Vertex[1] = new Point3d(x, y + step, data[(y + step) * depthData.Width + x]);
                    triangle.Vertex[2] = new Point3d(x + step, y + step, data[(y + step) * depthData.Width + (x + step)]);

                    triangle.NormalVector = new Point3d(normalVector[index][0], normalVector[index][1], normalVector[index][2]);

                    depthModel.TriangleList.Add(triangle);

                    triangle = new Triangle();

                    triangle.Vertex[0] = new Point3d(x, y, data[y * depthData.Width + x]);
                    triangle.Vertex[1] = new Point3d(x + step, y, data[y * depthData.Width + (x + step)]);
                    triangle.Vertex[2] = new Point3d(x + step, y + step, data[(y + step) * depthData.Width + (x + step)]);

                    triangle.NormalVector = new Point3d(normalVector[index][0], normalVector[index][1], normalVector[index][2]);

                    depthModel.TriangleList.Add(triangle);
                }
            }

            return depthModel;
        }

        public void SetCadData(Cad3dModel cad3dModel)
        {
            this.cad3dModel = cad3dModel;

            Point3d point3d = cad3dModel.CenterPt;
            cadMaxValue = cad3dModel.MaxValue;
            cadMinValue = cad3dModel.MinValue;
            cadDataWidth = cad3dModel.Size.Width;
            cadDataHeight = cad3dModel.Size.Height;

            var projectionMat = Matrix4.CreateTranslation(-(float)point3d.X, -(float)point3d.Y, -(float)point3d.Z);

            TransformCad(projectionMat);

            //minValue = 0; //  depthData.Data.Min();
            //maxValue = 50; //  depthData.Data.Max();

            if (cadDrawListCreated)
            {
                GL.DeleteLists(cadDrawList, 1);
                cadDrawListCreated = false;
            }
        }

        private void Normalize(Image3D depthData)
        {
            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - Normalize");

            float[] depthRawData = depthData.Data;
            float fMin;
            fMin = 0.0f;

            fMin = depthMinValue;

            for (int y = 0; y < depthData.Height; y++)
            {
                for (int x = 0; x < depthData.Width; x++)
                {
                    float depthValue = depthRawData[y * depthData.Width + x];

                    depthValue -= depthMinValue;
                    if (depthValue < fMin)
                    {
                        fMin = depthValue;
                    }

                    depthRawData[y * depthData.Width + x] = depthValue;
                }
            }

            depthMinValue = Math.Min(fMin, depthMinValue) - 1;
        }

        private void CalcNormals(Image3D depthData, Vector3f[] normalVector, Vector3[] depthPoint)
        {
            LogHelper.Debug(LoggerType.Operation, "SurfaceRender - CalcNormals");

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
                        outVec = new Vector3f(0.0f, depthRawData[(y - 1) * width + x] - depthValue, -1.0f * pixelResolution);
                    }

                    if (y < height - 1)
                    {
                        inVec = new Vector3f(0.0f, depthRawData[(y + 1) * width + x] - depthValue, 1.0f * pixelResolution);
                    }

                    if (x > 0)
                    {
                        leftVec = new Vector3f(-1.0f * pixelResolution, depthRawData[y * width + (x - 1)] - depthValue, 0.0f);
                    }

                    if (x < width - 1)
                    {
                        rightVec = new Vector3f(1.0f * pixelResolution, depthRawData[y * width + (x + 1)] - depthValue, 0.0f);
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
                        sum = new Vector3f(0.0f, 1.0f * pixelResolution, 0.0f);
                    }

                    normalVector[y * width + x] = sum;
                    depthPoint[y * width + x] = new Vector3(x * pixelResolution, y * pixelResolution, depthRawData[y * width + x]);
                }
            });
        }

        public void GetDisplayRange(out float rangeMin, out float rangeMax)
        {
            float valueRange = MaxValue - MinValue;

            rangeMin = (valueRange * RangeRatioMin) + MinValue;
            rangeMax = (valueRange * RangeRatioMax) + MinValue;
        }

        public Point3d[] GetCrossSectionData(CrossSection crossSection)
        {
            if (depthModel == null)
            {
                return new Point3d[0];
            }

            float[] depthRawData = CloudDepthData.Data;

            var profilePtList = new List<Point3d>();

            PointF startPt = crossSection.StartPoint;
            PointF endPt = crossSection.EndPoint;
            crossSection.GetStep(out float stepX, out float stepY);

            float x, y;
            for (y = startPt.Y, x = startPt.X; (Math.Abs(y - endPt.Y) > Math.Abs(stepY) * 2) && (Math.Abs(x - endPt.X) > Math.Abs(stepX)); y += stepY, x += stepX)
            {
                int index = CloudDepthData.Width * (int)(y / pixelResolution) + (int)(x / pixelResolution);
                profilePtList.Add(new Point3d(x, y, depthRawData[index]));
            }

            return profilePtList.ToArray();
        }

        public void DrawProfilePos(float baseSize, ProfileCoordinate profileCoordinate)
        {
            if (profileCoordinate.ScreenPosValid == false)
            {
                return;
            }

            float valueRange = MaxValue - MinValue;

            float size = (DataWidth > DataHeight) ? DataWidth : DataHeight;
            //float scaleXy = baseSize / size;

            //int sx = (int)((profileCoordinate.WorldPos.X + ((scaleXy * depthData.Width) / 2)) / scaleXy);
            //int sy = (int)((profileCoordinate.WorldPos.Z + ((scaleXy * depthData.Height) / 2)) / scaleXy);
            int sx = (int)(profileCoordinate.WorldPos.X);
            int sy = (int)(profileCoordinate.WorldPos.Z);

            Base.MathHelper.Bound(sx, 0, DataWidth);
            Base.MathHelper.Bound(sy, 0, DataHeight);
            GetDisplayRange(out float rangeMin, out float rangeMax);

            float offsetZ = 0;

            GL.Enable(EnableCap.LineStipple);
            GL.LineStipple(2, linePattern[0]); // (clock() / 100) % 8]);

            GL.PushMatrix();
            GL.Translate(-(DataWidth / 2), offsetZ, -(DataHeight / 2));

            float[] depthRawData = CloudDepthData.Data;

            float zoff = (MaxValue - MinValue) / 50;
            float fWid = size / 300;
            if (fWid < 1)
            {
                fWid = 0;
            }

            float fhWid = fWid / 2;
            float h = 0;
            int x, y;
            GL.Color3(0.0f, 0.0f, 0.0f);

            GL.LineWidth(fWid * 2);
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.LineStrip);
#pragma warning restore CS0618 // Type or member is obsolete

            if (profileCoordinate.Vertical)
            {
                for (y = 0; y < CloudDepthData.Height; y++)
                {
                    h = depthRawData[y * CloudDepthData.Width + sx] + zoff;
                    GL.Vertex3(sx, h, y);
                }
                GL.Vertex3(sx, h, y + fhWid);
            }
            else
            {
                for (x = 0; x < CloudDepthData.Width; x++)
                {
                    h = depthRawData[sy * CloudDepthData.Width + x] + zoff;
                    GL.Vertex3(x, h, sy);
                }
                GL.Vertex3(x + fhWid, h, sy);
            }

            GL.End();
            GL.PopMatrix();

            GL.Disable(EnableCap.LineStipple);
            GL.Disable(EnableCap.Lighting);
            GL.PushMatrix();

            GL.Color3(0.0f, 1.0f, 1.0f);
            if (profileCoordinate.Vertical)
            {
                GL.Translate(profileCoordinate.WorldPos.X, 0, 10);
                GL.Rotate(180, 0, 0, 1);
                GL.Rotate(-90.0f, 0, 1, 0);
                // s.Format(_T("%g"), ConvAbsScaleX((float)sx));
                // DrawText(pFont, s);
            }
            else
            {
                GL.Translate(10, 0, profileCoordinate.WorldPos.Z);
                GL.Rotate(180, 1, 0, 0);
                //                s.Format(_T("%g"), ConvAbsScaleY((float)sy));
                //                DrawText(pFont, s);
            }
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
        }

        public void DrawDepthData(float maxLength, CrossSection crossSection, float fBaseZ = 0)
        {
            if (CloudDepthData == null && cad3dModel == null)
            {
                return;
            }

            GL.PolygonMode(MaterialFace.FrontAndBack, polygonMode);

            float valueRange = MaxValue - MinValue;
            GetDisplayRange(out float rangeMin, out float rangeMax);

            float offsetZ = 0; // -((valueRange * (rangeMin - minValue) / valueRange)) * fScaleZ;

            GL.Translate(-(DataWidth / 2), offsetZ, -(DataHeight / 2));

            if (cloudDepthDrawListCreated && UseMultiTexture == false)
            {
                if (currentTextureDimType != TextureDimType)
                {
                    GL.DeleteLists(cloudDepthDrawList, 1);
                    cloudDepthDrawListCreated = false;
                }
            }

            if (cadDepthDrawListCreated && UseMultiTexture == false)
            {
                if (currentTextureDimType != TextureDimType)
                {
                    GL.DeleteLists(cadDepthDrawList, 1);
                    cadDepthDrawListCreated = false;
                }
            }

            if (cadDrawListCreated && UseMultiTexture == false)
            {
                if (currentTextureDimType != TextureDimType)
                {
                    GL.DeleteLists(cadDrawList, 1);
                    cadDrawListCreated = false;
                }
            }

            if (cloudDepthDrawListCreated == false && CloudDepthData != null)
            {
                cloudDepthDrawList = CreateDepthDrawList(CloudDepthData, cloudDepthPoint, depthNormalVector);
                cloudDepthDrawListCreated = true;
            }

            if (cadDepthDrawListCreated == false && cadDepthData != null)
            {
                cadDepthDrawList = CreateDepthDrawList(cadDepthData, cadDepthPoint, cadDepthNormalVector);
                cadDepthDrawListCreated = true;
            }

            if (cadDrawListCreated == false && cad3dModel != null)
            {
                CreateCadDrawList();
                cadDrawListCreated = true;
            }

            if (ShowDepth && cloudDepthDrawListCreated)
            {
                GL.CallList(cloudDepthDrawList);
            }

            if (ShowCadDepth && cadDepthDrawListCreated)
            {
                GL.CallList(cadDepthDrawList);
            }

            if (ShowCad && cadDrawListCreated)
            {
                GL.Color3(1.0f, 1.0f, 1.0f);
                GL.CallList(cadDrawList);
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

        private int CreateDepthDrawList(Image3D depthData, Vector3[] depthPoint, Vector3f[] normalVector)
        {
            int ymax = depthData.Height - 1;
            int xmax = depthData.Width - 1;

            float maxRatioHeight = (MaxValue - MinValue) * RangeRatioMax + MinValue;
            float minRatioHeight = (MaxValue - MinValue) * RangeRatioMin + MinValue;

            float objectHeight = (maxRatioHeight - minRatioHeight) + 1;

            int depthDrawList = GL.GenLists(1);

            GL.NewList(depthDrawList, ListMode.Compile);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvModeCombine.Modulate);
            GL.Enable(EnableCap.Texture1D);

            GL.Color3(1.0f, 1.0f, 1.0f);

            float fymax = (float)ymax - 8;
            float fxmax = (float)xmax - 8;
            for (int y = 7; y < ymax - 7; y++)
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
                for (int x = 7; x < xmax - 7; x++)
                {
                    if (polygonMode == PolygonMode.Fill)
                    {
                        int index = y * depthData.Width + x;

                        float textureValue = (depthPoint[index].Z - minRatioHeight) / objectHeight;

                        Vector3f normal = normalVector[index];
                        GL.Normal3(normal[0], normal[1], normal[2]);

                        float fx = (x / fxmax) / 1.0f;
                        if (UseMultiTexture && NativeDraw == false)
                        {
                            GL.MultiTexCoord1(TextureUnit.Texture0, textureValue);
                            GL.MultiTexCoord2(TextureUnit.Texture1, fx, fy1);
                        }
                        else
                        {
                            switch (TextureDimType)
                            {
                                case TextureDimType.One:
                                    GL.TexCoord1(textureValue);
                                    break;
                                case TextureDimType.Two:
                                    GL.TexCoord2(fx, fy1);
                                    break;
                                case TextureDimType.Mixed:
                                    GL.TexCoord1(textureValue);
                                    break;
                            }
                        }

                        GL.Vertex3(depthPoint[index].X, depthPoint[index].Z, depthPoint[index].Y);

                        index = (y + 1) * depthData.Width + x;
                        normal = normalVector[index];
                        GL.Normal3(normal[0], normal[1], normal[2]);

                        if (UseMultiTexture && NativeDraw == false)
                        {
                            GL.MultiTexCoord1(TextureUnit.Texture0, textureValue);
                            GL.MultiTexCoord2(TextureUnit.Texture1, fx, fy2);
                        }
                        else
                        {
                            switch (TextureDimType)
                            {
                                case TextureDimType.One:
                                    GL.TexCoord1(textureValue);
                                    break;
                                case TextureDimType.Two:
                                    GL.TexCoord2(fx, fy2);
                                    break;
                                case TextureDimType.Mixed:
                                    GL.TexCoord1(textureValue);
                                    break;
                            }
                        }
                        GL.Vertex3(depthPoint[index].X, depthPoint[index].Z, depthPoint[index].Y);

                    }
                    else if (polygonMode == PolygonMode.Line)
                    {
                        int index = y * depthData.Width + x;

                        GL.Vertex3(depthPoint[index].X, depthPoint[index].Z, depthPoint[index].Y);

                        index = (y + 1) * depthData.Width + x;
                        GL.Vertex3(depthPoint[index].X, depthPoint[index].Z, depthPoint[index].Y);
                    }
                    else
                    {
                        int index = y * depthData.Width + x;

                        GL.Vertex3(depthPoint[index].X, depthPoint[index].Z, depthPoint[index].Y);
                    }
                }
                GL.End();
            }
            GL.EndList();

            currentTextureDimType = TextureDimType;

            return depthDrawList;
        }

        private void CreateCadDrawList()
        {
            if (cad3dModel == null)
            {
                return;
            }

            float objectHeight = (MaxValue - MinValue) + 1;

            cadDrawList = GL.GenLists(1);

            GL.NewList(cadDrawList, ListMode.Compile);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvModeCombine.Modulate);

            GL.Color3(1.0f, 1.0f, 1.0f);

            foreach (Triangle triangle in cad3dModel.TriangleList)
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

                if (polygonMode == PolygonMode.Fill)
                {
                    //Point3d normal = triangle.NormalVector;

                    Vector3 point1 = ToVector3(triangle.Vertex[0]);
                    Vector3 point2 = ToVector3(triangle.Vertex[1]);
                    Vector3 point3 = ToVector3(triangle.Vertex[2]);

                    // Vector3 dir1 = Vector3.Cross(point2 - point1, point3 - point1);
                    var normal = Vector3.Normalize(Vector3.Cross(point2 - point1, point3 - point1));

                    GL.Normal3(normal.X, normal.Z, normal.Y);

                    GL.TexCoord1(triangle.Vertex[0].Z / objectHeight);
                    GL.Vertex3(triangle.Vertex[0].X, triangle.Vertex[0].Z, triangle.Vertex[0].Y);

                    GL.TexCoord1(triangle.Vertex[1].Z / objectHeight);
                    GL.Vertex3(triangle.Vertex[1].X, triangle.Vertex[1].Z, triangle.Vertex[1].Y);

                    GL.TexCoord1(triangle.Vertex[2].Z / objectHeight);
                    GL.Vertex3(triangle.Vertex[2].X, triangle.Vertex[2].Z, triangle.Vertex[2].Y);
                }
                else
                {
                    GL.Vertex3(triangle.Vertex[0].X, triangle.Vertex[0].Z, triangle.Vertex[0].Y);
                    GL.Vertex3(triangle.Vertex[1].X, triangle.Vertex[1].Z, triangle.Vertex[1].Y);
                    GL.Vertex3(triangle.Vertex[2].X, triangle.Vertex[2].Z, triangle.Vertex[2].Y);
                }
                GL.End();
            }
            GL.EndList();

            currentTextureDimType = TextureDimType;
            cadDrawListCreated = true;
        }

        //void CreateDepthDrawList()
        //{
        //    if (depthModel == null)
        //        return;

        //    float objectHeight = (depthMaxValue - depthMinValue) + 1;

        //    depthDrawList = GL.GenLists(1);

        //    GL.NewList(depthDrawList, ListMode.Compile);
        //    GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (Int32)TextureEnvModeCombine.Modulate);

        //    GL.Color3(1.0f, 1.0f, 1.0f);
        //    float zPos = 0;

        //    foreach (Triangle triangle in depthModel.TriangleList)
        //    {
        //        if (polygonMode == PolygonMode.Point)
        //            GL.Begin(PrimitiveType.Points);
        //        else
        //            GL.Begin(PrimitiveType.Triangles);

        //        if (polygonMode == PolygonMode.Fill)
        //        {
        //            Point3d normal = triangle.NormalVector;

        //            GL.Normal3(normal.X, normal.Z, normal.Y);

        //            GL.TexCoord1(triangle.Vertex[0].Z / objectHeight);
        //            GL.Vertex3(triangle.Vertex[0].X* pixelResolution, triangle.Vertex[0].Z, triangle.Vertex[0].Y * pixelResolution);

        //            GL.TexCoord1(triangle.Vertex[1].Z / objectHeight);
        //            GL.Vertex3(triangle.Vertex[1].X * pixelResolution, triangle.Vertex[1].Z, triangle.Vertex[1].Y * pixelResolution);

        //            GL.TexCoord1(triangle.Vertex[2].Z / objectHeight);
        //            GL.Vertex3(triangle.Vertex[2].X * pixelResolution, triangle.Vertex[2].Z, triangle.Vertex[2].Y * pixelResolution);
        //        }
        //        else
        //        {
        //            GL.Vertex3(triangle.Vertex[0].X * pixelResolution, triangle.Vertex[0].Z, triangle.Vertex[0].Y * pixelResolution);
        //            GL.Vertex3(triangle.Vertex[1].X * pixelResolution, triangle.Vertex[1].Z, triangle.Vertex[1].Y * pixelResolution);
        //            GL.Vertex3(triangle.Vertex[2].X * pixelResolution, triangle.Vertex[2].Z, triangle.Vertex[2].Y * pixelResolution);
        //        }
        //        GL.End();
        //    }
        //    GL.EndList();

        //    currentTextureDimType = textureDimType;
        //    depthDrawListCreated = true;
        //}

        internal void RotateCad(Matrix4 rotationMat)
        {
            cad3dModel.UpdateData();
            Point3d point3d = cad3dModel.CenterPt;

            var offsetZero = Matrix4.CreateTranslation(-(float)point3d.X, -(float)point3d.Y, -(float)point3d.Z);
            var offsetPos = Matrix4.CreateTranslation((float)point3d.X, (float)point3d.Y, (float)point3d.Z);

            var resultMat = Matrix4.Mult(Matrix4.Mult(offsetZero, rotationMat), offsetPos);

            TransformCad(resultMat);

            cad3dModel.UpdateData();
        }

        internal void TransformCad(Matrix4 projectionMat)
        {
            foreach (Triangle triangle in cad3dModel.TriangleList)
            {
                Point3d[] vertex = triangle.Vertex;
                TransformVertex(projectionMat, vertex[0]);
                TransformVertex(projectionMat, vertex[1]);
                TransformVertex(projectionMat, vertex[2]);

                var vector0 = new Vector3((float)vertex[0].X, (float)vertex[0].Y, (float)vertex[0].Z);
                var vector1 = new Vector3((float)vertex[1].X, (float)vertex[1].Y, (float)vertex[1].Z);
                var vector2 = new Vector3((float)vertex[2].X, (float)vertex[2].Y, (float)vertex[2].Z);

                var dir = Vector3.Cross(vector1 - vector0, vector2 - vector0);
                var norm = Vector3.Normalize(dir);
                triangle.NormalVector = new Point3d(norm.X, norm.Y, norm.Z);
            }

            cadDrawListCreated = false;
        }

        private void TransformVertex(Matrix4 projectionMat, Point3d vertex)
        {
            var vector4 = new Vector4((float)vertex.X, (float)vertex.Y, (float)vertex.Z, 1);
            vector4 = Vector4.Transform(vector4, projectionMat);

            vertex.X = vector4.X;
            vertex.Y = vector4.Y;
            vertex.Z = vector4.Z;
        }
    }
}
