using DynMvp.Base;
using DynMvp.Cad;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    internal enum TrackType
    {
        None, ProfileLeft, ProfileRight, CrossSectionStart, CrossSectionEnd, LegendMin, LegendMax
    }

    public partial class DepthViewer : UserControl
    {
        private GL gl = new GL();
        private SurfaceRender surfaceRender = new SurfaceRender();
        public SubView SubView { get; set; }

        private float profileLeftPct = 0;
        private float profileRightPct = 100;
        private LightMap lightMap = new LightMap();
        public TextureColor TextureColor { get; set; }

        private float shortLength = 20;
        private float maxLength = 20;
        public float PixelResolution { get; set; } = 1.0f;

        private ProfileCoordinate profileCoordinate = new ProfileCoordinate();
        private CrossSection crossSection = new CrossSection();
        private ProjectionType projectionType = ProjectionType.Perspective;
        private SurfaceMode surfaceMode;
        private FittingMode fittingMode = FittingMode.None;
        private List<Point3d> fittingPointList = new List<Point3d>();
        private MouseMode mouseMode = MouseMode.Rotate;
        private Point mouseStartPos;
        private bool onMouseMoving = false;
        private bool onTrackProfilePoint = false;
        private bool onTrackCrossSectionPoint = false;
        private bool onTrackEndPoint = false;
        private float cameraPosX = 0;
        private float cameraPosY = 0;
        private float cameraDistance = 25.0f;
        private float cameraAngleX = 25.0f;
        private float cameraAngleY = -35.0f;
        private float cameraAngleZ = 0;
        private bool gridCreated = false;
        private int gridHandle = 0;
        private double[] surfaceModelVec = new double[16];
        private double[] surfaceProjVec = new double[16];
        private int[] surfaceViewportVec = new int[4];
        private bool showLegend = true;
        private Rectangle legendRect;
        private Rectangle subViewRect;
        private TrackType curTrackType;
        private TrackPos[] trackPos = new TrackPos[Enum.GetNames(typeof(TrackType)).Count()];

        public DepthViewer()
        {
            InitializeComponent();

            currentGL.MouseWheel += new MouseEventHandler(CurrentGL_MouseWheel);

            //rotateToolStripButton.Checked = true;

            Setup();
        }

        private void Setup()
        {
            if (!currentGL.Context.IsCurrent)
            {
                currentGL.MakeCurrent();
            }

            //chartGL.MakeCurrent();
            var font = new Font("Arial", 10);
            FontStorage.Instance.Initialize(font, true, null);

            SetupEnable();

            GL.ClearStencil(0);
            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Lequal);

            SetupLights();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            subViewRect = new Rectangle(0, 0, Width / 2, Height / 2);

            surfaceRender = new SurfaceRender();

            SetSurfaceMode(SurfaceMode.TextureColor);
            SetupTextureColor(TextureColorType.Rainbow);
        }

        private void SetupLights()
        {
            {
                GL.Light(LightName.Light0, LightParameter.Ambient, new float[4] { .0f, .0f, .0f, 1.0f });
                GL.Light(LightName.Light0, LightParameter.Diffuse, new float[4] { .8f, .8f, .8f, 1.0f });
                //                GL.Light(LightName.Light0, LightParameter.Specular, new float[4] { 1, 1, 1, 1 });
                GL.Light(LightName.Light0, LightParameter.Specular, new float[4] { 0, 0, 0, 0 });
                GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 0, 100, 10, 0 });

                GL.Enable(EnableCap.Light0);
            }

            {
                GL.Light(LightName.Light1, LightParameter.Ambient, new float[4] { .0f, .0f, .0f, 1.0f });
                GL.Light(LightName.Light1, LightParameter.Diffuse, new float[4] { 1.0f, 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light1, LightParameter.Specular, new float[4] { 1, 1, 1, 1 });
                GL.Light(LightName.Light1, LightParameter.Position, new float[4] { -10, 10, 10, 0 });

                GL.Enable(EnableCap.Light1);
            }
        }

        private void SetupTextureColor(TextureColorType textureColorType)
        {
            if (surfaceRender != null)
            {
                float displayRangeRatioMin = surfaceRender.RangeRatioMin;
                float displayRangeRatioMax = surfaceRender.RangeRatioMax;

                if (TextureColor == null)
                {
                    TextureColor = new TextureColor();
                    lightMap.TextureColor = TextureColor;
                }

                const int NUM_COLOR_LEVEL = 512;

                switch (textureColorType)
                {
                    case TextureColorType.Rainbow:
                        TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, displayRangeRatioMin, displayRangeRatioMax);
                        break;
                    case TextureColorType.Hsv1:
                        TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, displayRangeRatioMin, displayRangeRatioMax, Color.FromArgb(0, 162, 232), Color.FromArgb(255, 128, 0));
                        break;
                    case TextureColorType.Hsv2:
                        TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, displayRangeRatioMin, displayRangeRatioMax, Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 255));
                        break;
                    case TextureColorType.Gray:
                        TextureColor.Create(textureColorType, NUM_COLOR_LEVEL, displayRangeRatioMin, displayRangeRatioMax, Color.FromArgb(0, 0, 0), Color.FromArgb(255, 0, 0));
                        break;
                }
            }
        }

        private void SetupEnable()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.ScissorTest);
            GL.ShadeModel(ShadingModel.Smooth);
            //            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.VertexArray);
        }

        public void SetDepthData(Image3D depthData, float pixelResolution)
        {
            LogHelper.Debug(LoggerType.Operation, "DepthViewer - SetDepthData 1");

            PixelResolution = pixelResolution;

            if (depthData != null)
            {
                LogHelper.Debug(LoggerType.Operation, "DepthViewer - SetDepthData 2");

                AlgoImage algoImage = ImageBuilder.OpenCvImageBuilder.Build(depthData, ImageType.Depth);
                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);
                imageProcessing.Median(algoImage, null, 5);
                imageProcessing.Average(algoImage);

                LogHelper.Debug(LoggerType.Operation, "DepthViewer - SetDepthData 3");

                surfaceRender.SetDepthData((Image3D)algoImage.ToImageD(), pixelResolution);

                LogHelper.Debug(LoggerType.Operation, "DepthViewer - SetDepthData 4");

                maxLength = Math.Max(depthData.Width, depthData.Height) * pixelResolution;
                shortLength = Math.Min(depthData.Width, depthData.Height) * pixelResolution;

                cameraDistance = maxLength * 2;
            }
            else
            {
                shortLength = 20;
                maxLength = 20;

                cameraDistance = 25;

                surfaceRender.SetDepthData(null, pixelResolution);
            }

            gridCreated = false;

            currentGL.Invalidate();
        }

        private void SetCadData(Cad3dModel cad3dModel)
        {
            cad3dModel.UpdateData();

            surfaceRender.SetCadData(cad3dModel);

            if (surfaceRender.IsValid() == false)
            {
                maxLength = Math.Max(cad3dModel.Size.Width, cad3dModel.Size.Height);
                shortLength = Math.Max(cad3dModel.Size.Width, cad3dModel.Size.Height);

                cameraDistance = maxLength * 2;
            }

            gridCreated = false;

            currentGL.Invalidate();
        }

        private void CurrentGL_Paint(object sender, PaintEventArgs e)
        {
            if (!currentGL.Context.IsCurrent)
            {
                currentGL.MakeCurrent();
            }
            //chartGL.MakeCurrent();

            RenderScene(e);
        }

        private void RenderScene(PaintEventArgs e)
        {
            int width = currentGL.Width;
            int height = currentGL.Height;

            GL.Viewport(0, 0, width, height);
            GL.Scissor(0, 0, width, height);

            //            GL.CullFace(CullFaceMode.Back);
            DrawObject();

            if (crossSection.Enabled)
            {
                DrawProfileSubView();
            }

            if (showLegend)
            {
                DrawLegend();
            }

            currentGL.SwapBuffers();
        }

        public void SetSurfaceMode(SurfaceMode surfaceMode)
        {
            this.surfaceMode = surfaceMode;
            if (surfaceRender != null)
            {
                TextureDimType textureDimType = TextureDimType.One;
                switch (surfaceMode)
                {
                    case SurfaceMode.TextureColor: textureDimType = TextureDimType.One; break;
                    case SurfaceMode.Image: textureDimType = TextureDimType.Two; break;
                    case SurfaceMode.Mixed: textureDimType = TextureDimType.Mixed; break;
                }
                surfaceRender.TextureDimType = textureDimType;
            }
        }

        private void SetupObjectCoordinate()
        {
            int width = currentGL.Width;
            int height = currentGL.Height;

            float aspectRatio = width / (float)height;
            float halfLength = maxLength / 2;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 projectionMat;
            if (projectionType == ProjectionType.Perspective)
            {
                projectionMat = Matrix4.CreatePerspectiveFieldOfView(0.5f /*(float)(System.Math.PI / 3.5)*/, aspectRatio, 1, maxLength * 4);
            }
            else
            {
                if (width <= height)
                {
                    projectionMat = Matrix4.CreateOrthographicOffCenter(-halfLength, halfLength, -halfLength / aspectRatio, halfLength / aspectRatio, -maxLength * 4, maxLength * 4);
                }
                else
                {
                    projectionMat = Matrix4.CreateOrthographicOffCenter(-halfLength * aspectRatio, halfLength * aspectRatio, -halfLength, halfLength, -maxLength * 4, maxLength * 4);
                }
            }
            GL.LoadMatrix(ref projectionMat);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void DrawCrossSectionLine()
        {
            if (crossSection.Enabled) //  && ((clock() / 200) % 2 == 0))
            {
                Point3d[] profilePtArray = surfaceRender.GetCrossSectionData(crossSection);
                if (profilePtArray == null)
                {
                    return;
                }

                GL.Color4(0.14f, 0.14f, 0.14f, 50 / 100.0f);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Polygon);
#pragma warning restore CS0618 // Type or member is obsolete
                GL.Vertex3(crossSection.StartPoint.X, 0, crossSection.StartPoint.Y);
                GL.Vertex3(crossSection.StartPoint.X, surfaceRender.MaxValue, crossSection.StartPoint.Y);
                GL.Vertex3(crossSection.EndPoint.X, surfaceRender.MaxValue, crossSection.EndPoint.Y);
                GL.Vertex3(crossSection.EndPoint.X, 0, crossSection.EndPoint.Y);
                GL.End();

                GL.Color3(0.0f, 1.0f, 0.0f);

                //if (trackPos[(int)TrackType.CrossSectionStart].IsInitialized() == false)
                //    new Point3d(crossSection.StartPoint.X, surfaceRender.MaxValue, crossSection.StartPoint.Y);

                //SphereShape sphereShape = new SphereShape(5, 5, 32, 32);
                //sphereShape.Draw();
                //trackPos[(int)TrackType.CrossSectionStart] = sphereShape.ScreenRect;
                //sphereShape.Draw(new Point3d(crossSection.EndPoint.X, surfaceRender.MaxValue, crossSection.EndPoint.Y));
                //trackRectangle[(int)TrackType.CrossSectionEnd] = sphereShape.ScreenRect;

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

        private Vector3 GetTrackPos(RectangleF trackRect)
        {
            if (trackRect == RectangleF.Empty)
            {
                return new Vector3();
            }

            var point = new Vector3();

            PointF centerPt = DrawingHelper.CenterPoint(trackRect);
            point.X = centerPt.X;
            point.Y = surfaceViewportVec[3] - centerPt.Y;

            float zValue = 0;
            GL.ReadPixels((int)centerPt.X, (int)centerPt.Y, 1, 1, PixelFormat.DepthComponent, PixelType.Float, ref zValue);
            point.Z = zValue;

            if (point.Z != 1)
            {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                OpenTK.Graphics.Glu.UnProject(point, surfaceModelVec, surfaceProjVec, surfaceViewportVec, out Vector3 destPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                return destPt;
            }

            return new Vector3();
        }

        private void DisableTexture()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Disable(EnableCap.Texture1D);
            GL.Disable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Disable(EnableCap.Texture1D);
            GL.Disable(EnableCap.Texture2D);
        }

        private void DrawObject()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            GL.PushMatrix();

            SetupObjectCoordinate();

            GL.DepthFunc(DepthFunction.Lequal);
            GL.Translate(cameraPosX, cameraPosY, -cameraDistance);
            GL.Rotate(cameraAngleX, 1, 0, 0);
            GL.Rotate(cameraAngleY, 0, 1, 0);
            GL.Rotate(cameraAngleZ, 0, 0, 1);

            GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 0, 100, 10, 0 });

            DrawGrid(maxLength / 2, maxLength / 20);
            // Draw_All_Obj();

            lightMap.Set(0, surfaceMode);

            if (surfaceRender != null)
            {
                surfaceRender.DrawDepthData(maxLength, crossSection);

                DisableTexture();

                DrawCrossSectionLine();

                DrawPlaneFittingCircle();
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            GL.GetDouble(GetPName.ModelviewMatrix, surfaceModelVec);
            GL.GetDouble(GetPName.ProjectionMatrix, surfaceProjVec);
            GL.GetInteger(GetPName.Viewport, surfaceViewportVec);

            /*double[] matModel = new double[16];
            double[] matProj = new double[16];
            int[] view = new int[4];

            switch (subView)
            {
                case SubView.Profile:
                    surfaceRender.DrawProfilePos(fCoordSize, profileCoordinate);
                    break;
                default:
                    break;
            }*/

            GL.DepthFunc(DepthFunction.Always);

            //GL.PushMatrix();
            //Rectangle screenRect = new Rectangle(0, 0, Width, Height);

            //GL.Disable(EnableCap.Lighting);

            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadIdentity();
            //GL.Ortho(0, screenRect.Width, screenRect.Height, 0, -1, 1);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            //DrawHandle(screenRect);
            //DrawLegend(screenRect);

            //GL.Arb.ActiveTexture(TextureUnit.Texture0);
            //GL.Disable(EnableCap.Texture1D);
            //GL.Arb.ActiveTexture(TextureUnit.Texture1);
            //GL.Disable(EnableCap.Texture2D);
            //GL.PopMatrix();

            GL.PopMatrix();
        }

        private void DrawPlaneFittingCircle()
        {
            var sphereShape = new SphereShape(10, 10, 32, 32);
            foreach (Point3d point in fittingPointList)
            {
                sphereShape.Draw(point);
            }
        }

        private void DrawProfileSubView()
        {
            int width = currentGL.Width / 2;
            int height = currentGL.Height / 2;

            GL.Viewport(0, height - 1, width, height);
            GL.Scissor(0, height - 1, width, height);

            GL.PushMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 projectionMat;
            projectionMat = Matrix4.CreateOrthographicOffCenter(0, width, height, 0, -10.0f, 10.0f);

            GL.LoadMatrix(ref projectionMat);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            const int subViewTransparent = 50;

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.LineSmooth);

            float margin = 20;
            float f_l = margin * 2;
            float f_b = margin;
            float f_w = width - margin * 4;
            float f_h = height - margin * 4;

            GL.Color4(0.14f, 0.14f, 0.14f, subViewTransparent / 100.0f);
            GL.Rect(0, 0, width, height);

            Point3d[] pointArray = surfaceRender.GetCrossSectionData(crossSection);

            int pointArrayCount = pointArray.Count();

            var left = new Vector2();
            var right = new Vector2();
            if (pointArrayCount == 0)
            {
                GL.PopMatrix();
                left = new Vector2(width * 1 / 3, height - 50);
                right = new Vector2(width * 2 / 3, height - 50);
                return;
            }

            float maxValue = (float)pointArray.Max(x => x.Z);
            float minValue = (float)pointArray.Min(x => x.Z);
            float bound = maxValue - minValue;

            var leftValuePos = new PointF();
            var rightValuePos = new PointF();

            int profileLeftIndex = (int)((pointArrayCount - 1) * profileLeftPct / 100.0f);
            int profileRightIndex = (int)((pointArrayCount - 1) * profileRightPct / 100.0f);

            GL.LineWidth(2);
            GL.Color3(1.0f, 1.0f, 1.0f);
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.LineStrip);
#pragma warning restore CS0618 // Type or member is obsolete

            for (int i = 0; i < pointArrayCount; i++)
            {
                float f_x = f_l + f_w * (i / (float)pointArrayCount);
                float f_y = f_b + f_h * (1 - (((float)pointArray[i].Z - minValue) / bound));
                GL.Vertex2(f_x, f_y);

                if (i == profileLeftIndex)
                {
                    leftValuePos = new PointF(f_x, f_y);
                }
                else if (i == profileRightIndex)
                {
                    rightValuePos = new PointF(f_x, f_y);
                }
            }

            GL.End();

            var textShape = new TextShape();

            textShape.Initialize(pointArray[profileLeftIndex].Z.ToString("0.000"), leftValuePos.X, leftValuePos.Y, 1.5f, 1.2f, AlignFormat.Center);
            textShape.Draw();

            textShape.Initialize(pointArray[profileRightIndex].Z.ToString("0.000"), rightValuePos.X, rightValuePos.Y, 1.5f, 1.2f, AlignFormat.Center);
            textShape.Draw();

            GL.LineWidth(2);
            GL.Color3(1.0f, 1.0f, 0.0f);
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.Lines);
#pragma warning restore CS0618 // Type or member is obsolete

            GL.Vertex2(margin, margin);
            GL.Vertex2(margin, height - margin);
            GL.Vertex2(margin, height - margin);
            GL.Vertex2(width - margin, height - margin);

            GL.Color3(1.0f, 0.0f, 0.0f);

            GL.Vertex2(leftValuePos.X, margin);
            GL.Vertex2(leftValuePos.X, height - margin);

            GL.Vertex2(rightValuePos.X, margin);
            GL.Vertex2(rightValuePos.X, height - margin);

            GL.Color3(1.0f, 0.0f, 1.0f);
            GL.LineStipple(4, 0xAAAA);
            GL.Enable(EnableCap.LineStipple);

            GL.Vertex2(leftValuePos.X - margin, leftValuePos.Y);
            GL.Vertex2(rightValuePos.X, leftValuePos.Y);

            GL.Vertex2(leftValuePos.X - margin, rightValuePos.Y);
            GL.Vertex2(rightValuePos.X, rightValuePos.Y);

            GL.Disable(EnableCap.LineStipple);

            //GL.Vertex2(margin, margin);
            //GL.Vertex2(margin, 100);

            GL.End();

            //SphereShape sphereShape = new SphereShape(10, 10, 32, 32);
            //sphereShape.Draw(new Point3d(leftValuePos.X, margin, 0));
            //trackRectangle[(int)TrackType.ProfileLeft] = sphereShape.ScreenRect;
            //trackRectangle[(int)TrackType.ProfileLeft].Y += height;

            //sphereShape.Draw(new Point3d(rightValuePos.X, margin, 0));
            //trackRectangle[(int)TrackType.ProfileRight] = sphereShape.ScreenRect;
            //trackRectangle[(int)TrackType.ProfileRight].Y += height;

            float valueDiff = (float)Math.Abs(pointArray[profileLeftIndex].Z - pointArray[profileRightIndex].Z);
            float midYPos = (leftValuePos.Y + rightValuePos.Y) / 2;
            textShape.Initialize(valueDiff.ToString("0.000"), leftValuePos.X - margin, midYPos, 1.5f, 1.2f, AlignFormat.Center);
            textShape.Draw();

            var leftPt = new PointF((float)pointArray[profileLeftIndex].X, (float)pointArray[profileLeftIndex].Y);
            var rightPt = new PointF((float)pointArray[profileRightIndex].X, (float)pointArray[profileRightIndex].Y);

            float length = Base.MathHelper.GetLength(leftPt, rightPt);
            float midXPos = (leftValuePos.X + rightValuePos.X) / 2;
            float yPos = Math.Min(leftValuePos.Y, rightValuePos.Y);
            textShape.Initialize(length.ToString("0.000"), midXPos, yPos, 1.5f, 1.2f, AlignFormat.Center);
            textShape.Draw();

            GL.PopMatrix();

            //DrawMeasure(width, height, ref left, ref right);
        }

        private void DrawProfileValue()
        {
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

        private void DrawLegend()
        {
            if (!surfaceRender.IsValid())
            {
                return;
            }

            GL.PushMatrix();

            int width = currentGL.Width;
            int height = currentGL.Height;

            GL.Viewport(0, 0, width, height);
            GL.Scissor(0, 0, width, height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 projectionMat;
            projectionMat = Matrix4.CreateOrthographicOffCenter(0, width, height, 0, -10.0f, 10.0f);

            GL.LoadMatrix(ref projectionMat);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            const float boxWidthPercent = 0.05f;
            const float boxHeightPercent = 0.3f;

            int boxWidth = (int)(width * boxWidthPercent);
            boxWidth = Base.MathHelper.Bound(boxWidth, 5, 30);

            int boxHeight = (int)(height * boxHeightPercent);

            int fontSize = boxWidth / 3;
            int nGap = fontSize;

            int boxRight = width - nGap;
            int boxLeft = boxRight - boxWidth;
            int boxTop = nGap;
            int boxBottom = nGap + boxHeight;

            legendRect = Rectangle.FromLTRB(boxLeft, boxTop, boxRight, boxBottom);

            //            Rectangle boxRect = Rectangle.FromLTRB(10, 10, 20, 100);

            //BOOL m_bView = (m_nWorkSurfaceMode != ST_IMAGE && IsShowColorBar());
            //int nSize = rcScr.right - rc.left;
            //int nInc = (int)((m_fTmFrame * nSize) / 500);
            //if (nInc <= 0)
            //    nInc = 1;
            //if (m_bView)
            //{
            //    if (nSize > m_nLegendOffset)
            //    {
            //        m_nLegendOffset += nInc;
            //        if (nSize < m_nLegendOffset)
            //            m_nLegendOffset = nSize;
            //        rc.OffsetRect(nSize - m_nLegendOffset, 0);
            //    }
            //}
            //else
            //{
            //    if (m_nLegendOffset <= 0)
            //        return;
            //    m_nLegendOffset -= nInc;
            //    rc.OffsetRect(nSize - m_nLegendOffset, 0);
            //}

            //m_font.SetAlign(GFA_CENTER);

            float fmin = surfaceRender.MinValue;
            float fmax = surfaceRender.MaxValue;
            surfaceRender.GetDisplayRange(out float displayRangeMin, out float displayRangeMax);

            float fMinRate = surfaceRender.RangeRatioMin;
            float fMaxRate = surfaceRender.RangeRatioMax;
            //{
            //    surfaceRender.MinValue(&fmin, &fmax);
            //    surfaceRender.GetRangeMinMaxRate(&fMinRate, &fMaxRate);
            //}

            float fCen = (legendRect.Left + legendRect.Right) / 2.0f;
            float fSize = (float)fontSize;
            int nFrameThick = 4;

            //CString s;

            GL.Color3(0.0f, 0.0f, 0.0f);
            GL.Rect(legendRect.Left - nFrameThick, legendRect.Top - nFrameThick, legendRect.Right + nFrameThick, legendRect.Bottom + nFrameThick);

            //s.Format(_T("%4.0f"), ConvAbsZ(fmax));
            //DrawText(fCen, (float)boxRect.Top - nFrameThick, fSize, RGB(255, 255, 255), s);
            //s.Format(_T("%4.0f"), ConvAbsZ(fmin));
            //DrawText(fCen, (float)boxRect.Bottom + fSize / 2 + nFrameThick, fSize, RGB(255, 255, 255), s);

            GL.Color3(1.0f, 1.0f, 1.0f);
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.Polygon);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.Vertex2(legendRect.Left, legendRect.Top);
            GL.Vertex2(legendRect.Right, legendRect.Top);
            GL.Vertex2(legendRect.Right, legendRect.Bottom);
            GL.Vertex2(legendRect.Left, legendRect.Bottom);
            GL.End();

            GL.Enable(EnableCap.Texture1D);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
            TextureColor.Bind();

            float fPosMaxY = legendRect.Top + (legendRect.Height - (legendRect.Height * fMaxRate));
            float fPosMinY = legendRect.Top + (legendRect.Height - (legendRect.Height * fMinRate));

#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.Polygon);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.TexCoord1(1.0f);
            GL.Vertex2(legendRect.Left, fPosMaxY);
            GL.TexCoord1(1.0f);
            GL.Vertex2(legendRect.Right, fPosMaxY);
            GL.TexCoord1(0.0f);
            GL.Vertex2(legendRect.Right, fPosMinY);
            GL.TexCoord1(0.0f);
            GL.Vertex2(legendRect.Left, fPosMinY);
            GL.End();

            //GL_EXT(glActiveTextureARB(GL_TEXTURE0_ARB));
            GL.Disable(EnableCap.Texture1D);

            //fCen = boxRect.Right + fSize * 2;
            //float fSt = (float)(boxRect.Left - 20);
            //float fEd = (float)(boxRect.Right + 10);
            //float fH = (float)boxRect.Height;

            //m_font.SetAlign(GFA_RIGHT);
            //float fHalfFont = fSize / 2;


            //GL.PushMatrix();

            //SphereShape sphereShape = new SphereShape(fSize, fSize, 32, 32);
            //sphereShape.Draw(new Point3d(boxRect.Left - fSize, fPosMaxY, 0));
            //trackRectangle[(int)TrackType.LegendMax] = sphereShape.ScreenRect;
            //sphereShape.Draw(new Point3d(boxRect.Left - fSize, fPosMinY, 0));
            //trackRectangle[(int)TrackType.LegendMin] = sphereShape.ScreenRect;

            var textShape = new TextShape();

            textShape.Initialize(displayRangeMax.ToString("0.000"), legendRect.Left - fSize * 3, fPosMaxY - fSize, 1.5f, 1.2f, AlignFormat.Right);
            textShape.Draw();

            textShape.Initialize(displayRangeMin.ToString("0.000"), legendRect.Left - fSize * 3, fPosMinY - fSize, 1.5f, 1.2f, AlignFormat.Right);
            textShape.Draw();

            //            GL.Translate(fSt, fPosMaxY, 0);
            //            GL.Rotate(180.0f, 1.0f, 0.0f, 0.0f);
            //            GL.Scale(fSize, fSize * 1.5f, 1.0f);
            //            GL.Color3(0.0f, 1.0f, 1.0f);
            ////            int nMaxTextWid = (int)(m_font.Print(_T("%4.0f"), ConvAbsZ(fmaxSet)) * fSize);
            //            GL.PopMatrix();


            //GL.PushMatrix();

            //GL.Color3(1.0f, 1.0f, 1.0f);
            //GL.Begin( PrimitiveType.Triangles);
            //GL.Vertex3(fSt + 1, fPosMinY, 0);
            //GL.Vertex3((float)boxRect.Left, fPosMinY, 0);
            //GL.Vertex3(fSt + 1, fPosMinY + fSize / 2, 0);
            //GL.End();
            //GL.Translate(fSt, fPosMinY + fSize - 1, 0);
            //GL.Rotate(180.0f, 1.0f, 0.0f, 0.0f);
            //GL.Scale(fSize, fSize * 1.5f, 1);
            //GL.Color3(1.0f, 0.0f, 1.0f);
            ////            int nMinTextWid = (int)(m_font.Print(_T("%4.0f"), ConvAbsZ(fminSet)) * fSize);

            //GL.PopMatrix();

            //m_font.SetAlign(GFA_CENTER);

            //CSingleLock sync(&m_csPos, TRUE );
            //m_rcUIPos[UID_COLOR_MAX].SetRect((int)fSt - nMaxTextWid, (int)(fPosMaxY - fSize), rc.left, (int)(fPosMaxY));
            //m_rcUIPos[UID_COLOR_MIN].SetRect((int)fSt - nMinTextWid, (int)(fPosMinY), rc.left, (int)(fPosMinY + fSize));
            //m_rcColorBar.CopyRect(rc);
            //sync.Unlock();

            GL.PopMatrix();
        }

        private void DrawHandle(Rectangle screenRect)
        {
            //            CRect rc;

            //            int nBoxWidth = PERCENT(m_nWindowWidth, HDL_BOX_WIDTH);
            //            int nBoxHeight = 40;

            //            int nFontSize = nBoxHeight / 2;
            //            int nGap = 20;
            //            int nFrameThick = 10;

            //            rc.left = (rcScr.Width() - nBoxWidth) / 2;
            //            rc.right = m_nWindowWidth - (nBoxHeight * 4);   // rc.left + nBoxWidth;
            //            rc.top = rcScr.bottom - (nGap + nBoxHeight);
            //            rc.bottom = rc.top + nBoxHeight;


            //            BOOL bView = m_Clip.IsEnable();
            //            static int m_nHandleOffset = 0;
            //            int nSize = rcScr.bottom - rc.top;
            //            int nInc = (int)((m_fTmFrame * nSize) / 500);
            //            if (nInc <= 0) nInc = 1;
            //            if (bView)
            //            {
            //                if (nSize > m_nHandleOffset)
            //                {
            //                    m_nHandleOffset += nInc;
            //                    if (nSize < m_nHandleOffset)
            //                        m_nHandleOffset = nSize;
            //                    rc.OffsetRect(0, nSize - m_nHandleOffset);
            //                }
            //            }
            //            else
            //            {
            //                if (m_nHandleOffset <= 0)
            //                    return;
            //                m_nHandleOffset -= nInc;
            //                rc.OffsetRect(0, nSize - m_nHandleOffset);
            //            }

            //            int yCen = (rc.top + rc.bottom) / 2;

            //            CRect rcGrp(rc);
            //            rcGrp.bottom = rcGrp.top + rc.Height() / 2;

            //            GL_RGBA_COLOR(63, 82, 204, 128);
            //            glRecti(rcGrp.left - nFrameThick, rcGrp.top - nFrameThick, rcGrp.right + nFrameThick, rcGrp.bottom + nFrameThick);


            //            CRect rcText(rcGrp);
            //            rcText.top = rcGrp.top - nFrameThick / 2;
            //            rcText.bottom = rcGrp.bottom + nFrameThick / 2;
            //            rcText.left = rc.right + nFrameThick * 3 / 2;
            //            rcText.right = rcText.left + rcText.Height();
            //            m_font.SetAlign(GFA_CENTER);
            //            float fSize = (float)rcText.Height();
            //            float fGap = fSize / 5;

            //            CSingleLock sync(&m_csPos, TRUE );

            //            CLIP_AXIS axis = m_Clip.GetCurAxis();

            //            COLORREF colSel = RGB(255, 0, 0);
            //            COLORREF colNor = RGB(255, 255, 255);

            //            GL_RGBA_COLOR(0, 0, 232, 128);
            //            glRecti(rcText.left, rcText.top, rcText.right, rcText.bottom);
            //            DrawText((float)(rcText.left + rcText.right) / 2, (float)rcText.top + fSize - fGap, fSize,
            //                (axis == CAX_X) ? colSel : colNor, _T("X"));
            //            m_rcUIPos[UID_CLIP_X].CopyRect(rcText);

            //            rcText.OffsetRect(rcText.Width() + 3, 0);
            //            GL_RGBA_COLOR(0, 0, 232, 128);
            //            glRecti(rcText.left, rcText.top, rcText.right, rcText.bottom);
            //            DrawText((float)(rcText.left + rcText.right) / 2, (float)rcText.top + fSize - fGap, fSize,
            //                (axis == CAX_Y) ? colSel : colNor, _T("Y"));
            //            m_rcUIPos[UID_CLIP_Y].CopyRect(rcText);

            //            rcText.OffsetRect(rcText.Width() + 3, 0);
            //            GL_RGBA_COLOR(0, 0, 232, 128);
            //            glRecti(rcText.left, rcText.top, rcText.right, rcText.bottom);
            //            DrawText((float)(rcText.left + rcText.right) / 2, (float)rcText.top + fSize - fGap, fSize,
            //                (axis == CAX_Z) ? colSel : colNor, _T("Z"));
            //            m_rcUIPos[UID_CLIP_Z].CopyRect(rcText);

            //            rcText.OffsetRect(rcText.Width() + 3, 0);
            //            GL_RGBA_COLOR(185, 122, 87, 128);
            //            glRecti(rcText.left, rcText.top, rcText.right, rcText.bottom);
            //            DrawText((float)(rcText.left + rcText.right) / 2, (float)rcText.top + fSize - fGap, fSize,
            //                RGB(255, 255, 255), _T("R"));
            //            m_rcUIPos[UID_CLIP_RESET].CopyRect(rcText);

            //            rcText.left = rcGrp.left - nFrameThick * 3 / 2;
            //            rcText.right = rcText.left + rcText.Height();
            //            rcText.OffsetRect(-(rcText.Width() + 3), 0);

            //            GL_RGBA_COLOR(0, 0, 232, 128);
            //            glRecti(rcText.left, rcText.top, rcText.right, rcText.bottom);
            //            DrawText((float)(rcText.left + rcText.right) / 2, (float)rcText.top + fSize - fGap, fSize,
            //                m_Clip.IsEnableProfile() ? colSel : colNor, _T("P"));
            //            m_rcUIPos[UID_CLIP_PROFILE].CopyRect(rcText);

            //            sync.Unlock();


            //            GL_RGBA_COLOR(0, 0, 255, 192);
            //            glRecti(rcGrp.left, rcGrp.top, rcGrp.right, rcGrp.bottom);

            //            CRect rcSe(rcGrp);

            //            float fWid = (float)rcGrp.Width();


            //            AxisClipData* pAxis = m_Clip.GetAxisClipData();
            //            float fSt = pAxis->fSt;
            //            float fEd = pAxis->fEd;

            //            rcSe.left = rcGrp.left + (int)(fWid * fSt);
            //            rcSe.right = rcGrp.right - (int)(fWid * fEd);

            //            GL_RGBA_COLOR(255, 201, 14, 192);
            //            glRecti(rcSe.left, rcSe.top, rcSe.right, rcSe.bottom);

            //            GL_RGBA_COLOR(255, 255, 255, 128);
            //            CRect rcBox1, rcBox2;

            //            rcBox1.left = rcBox1.right = rcSe.left;
            //            rcBox1.top = rcSe.bottom;
            //            rcBox1.bottom = rcBox1.top + nFontSize;

            //            int nTabSize = nFontSize / 3;
            //            rcBox1.InflateRect(nTabSize, 0);

            //            glBegin(GL_TRIANGLE_FAN);
            //            glVertex2i(rcBox1.left, rcBox1.top + nTabSize);
            //            glVertex2i(rcBox1.left, rcBox1.bottom);
            //            glVertex2i(rcBox1.right, rcBox1.bottom);
            //            glVertex2i(rcBox1.right, rcBox1.top + nTabSize);
            //            glVertex2i(rcSe.left, rcBox1.top);
            //            glEnd();

            //            rcBox2.CopyRect(rcBox1);
            //            rcBox2.left = rcBox2.right = rcSe.right;
            //            rcBox2.InflateRect(nTabSize, 0);

            //            glBegin(GL_TRIANGLE_FAN);
            //            glVertex2i(rcBox2.left, rcBox2.top + nTabSize);
            //            glVertex2i(rcBox2.left, rcBox2.bottom);
            //            glVertex2i(rcBox2.right, rcBox2.bottom);
            //            glVertex2i(rcBox2.right, rcBox2.top + nTabSize);
            //            glVertex2i(rcSe.right, rcBox2.top);
            //            glEnd();

            //            CSingleLock lock (&m_csPos, TRUE );
            //            m_rcSection.CopyRect(rcGrp);
            //            m_rcUIPos[UID_CLIP_MIN].CopyRect(rcBox1);
            //            m_rcUIPos[UID_CLIP_MAX].CopyRect(rcBox2);
        }

        private void DrawGrid(float size, float step)
        {
            if (!gridCreated)
            {
                GL.LineWidth(1);

                gridHandle = GL.GenLists(1);

                GL.NewList(gridHandle, ListMode.Compile);
                GL.Disable(EnableCap.Lighting);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Quads);
#pragma warning restore CS0618 // Type or member is obsolete
                GL.Color4(1, 1, 1, 0.2f);
                GL.Vertex3(-size, 0, -size);
                GL.Vertex3(size, 0, -size);
                GL.Vertex3(size, 0, size);
                GL.Vertex3(-size, 0, size);
                GL.End();

                GL.Disable(EnableCap.Blend);

#pragma warning disable CS0618 // Type or member is obsolete
                GL.Begin(BeginMode.Lines);
#pragma warning restore CS0618 // Type or member is obsolete

                GL.Color3(0.3f, 0.3f, 0.3f);
                for (float i = step; i <= size; i += step)
                {
                    GL.Vertex3(-size, 0, i);
                    GL.Vertex3(size, 0, i);
                    GL.Vertex3(-size, 0, -i);
                    GL.Vertex3(size, 0, -i);

                    GL.Vertex3(i, 0, -size);
                    GL.Vertex3(i, 0, size);
                    GL.Vertex3(-i, 0, -size);
                    GL.Vertex3(-i, 0, size);
                }

                // x-axis
                GL.Color3(0.5f, 0.0f, 0.0f);
                GL.Vertex3(-size, 0, 0);
                GL.Vertex3(size, 0, 0);

                // z-axis
                GL.Color3(0.0f, 0.0f, 0.5f);
                GL.Vertex3(0, 0, -size);
                GL.Vertex3(0, 0, size);

                GL.End();
                GL.Color3(1.0f, 1.0f, 1.0f);

                GL.PushMatrix();
                GL.Translate(-size, 0, size + step);
                DrawArrow("X+", step);
                GL.PopMatrix();

                GL.PushMatrix();
                GL.Rotate(90.0f, 0, 1, 0);
                GL.Translate(-size, 0, -size - step);
                DrawArrow("Y+", step);
                GL.PopMatrix();

                GL.Enable(EnableCap.Lighting);
                GL.EndList();

                gridCreated = true;
            }

            GL.PushMatrix();
            //            GL.Translate(0, 0, size);
            GL.CallList(gridHandle);
            GL.PopMatrix();
        }

        private void DrawArrow(string axisName, float size)
        {
            float halfSize = size / 2;
#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.LineLoop);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.Vertex3(0, 0, 0.5f * halfSize);
            GL.Vertex3(0, 0, -0.5f * halfSize);
            GL.Vertex3(2 * halfSize, 0, -0.5f * halfSize);
            GL.Vertex3(2 * halfSize, 0, -1.0f * halfSize);
            GL.Vertex3(3 * halfSize, 0, 0.0f);
            GL.Vertex3(2 * halfSize, 0, 1.0f * halfSize);
            GL.Vertex3(2 * halfSize, 0, 0.5f * halfSize);
            GL.End();

            float fOff = (size / 2.0f) * axisName.Length;

            GL.PushMatrix();
            GL.Translate(1 + fOff, 0, -0.5f * halfSize);
            GL.Rotate(90, 1, 0, 0);
            GL.Scale(size, size, 0);

            var textShape = new TextShape(axisName, 0, 0, 0.05f, 0.05f);
            textShape.Draw();

            //            m_font.Print(sStr);
            GL.PopMatrix();
        }

        public bool UnProject(Vector4 srcPt, Matrix4 modelMatrix, Matrix4 projMatrix,
                                  float[] viewport, ref Vector4 destPt)
        {
            var multiMat = Matrix4.Mult(modelMatrix, projMatrix);
            var finalMatrix = Matrix4.Invert(multiMat);

            //' Map x and y from window coordinates 
            srcPt.X = (srcPt.X - viewport[0]) / viewport[2];
            srcPt.Y = (srcPt.Y - viewport[1]) / viewport[3];
            //' Map to range -1 to 1 
            srcPt.X = srcPt.X * 2.0f - 1.0f;
            srcPt.Y = srcPt.Y * 2.0f - 1.0f;
            srcPt.Z = srcPt.Z * 2.0f - 1.0f;
            srcPt.W = 1.0f;
            destPt = Vector4.Transform(srcPt, finalMatrix);

            if (destPt.W == 0)
            {
                return false;
            }

            destPt.X /= destPt.W;
            destPt.Y /= destPt.W;
            destPt.Z /= destPt.W;

            return true;
        }

        private void CurrentGL_MouseDown(object sender, MouseEventArgs e)
        {
            onMouseMoving = true;

            if (e.Button == MouseButtons.Right)
            {
                mouseStartPos = new Point(e.X, e.Y);
            }
            else if (e.Button == MouseButtons.Left)
            {
                curTrackType = TrackType.None;
                //for (int i=0; i<trackRectangle.Count(); i++)
                //{
                //    if (trackRectangle[i].Contains(e.X, e.Y))
                //    {
                //        curTrackType = (TrackType)i;
                //        mouseStartPos = new Point(e.X, e.Y);
                //    }
                //}

                if (curTrackType == TrackType.None)
                {
                    if (fittingMode != FittingMode.None)
                    {
                        AddFittingPoint(e);
                    }
                    else if (legendRect.Contains(e.X, e.Y))
                    {
                        if (ModifierKeys == Keys.Control)
                        {
                            surfaceRender.RangeRatioMin = (float)(legendRect.Bottom - e.Y) / legendRect.Height;
                        }
                        else
                        {
                            surfaceRender.RangeRatioMax = (float)(legendRect.Bottom - e.Y) / legendRect.Height;
                        }

                        surfaceRender.ClearList();
                    }
                    else if (crossSection.Enabled)
                    {
                        onTrackEndPoint = ModifierKeys == Keys.Control;

                        if (e.X < Width / 2 && e.Y < Height / 2)
                        {
                            onTrackProfilePoint = SelectProfilePoint(e);
                        }
                        else
                        {
                            onTrackCrossSectionPoint = SelectCrossSectionPoint(e);
                        }
                    }
                }
            }
            currentGL.Refresh();
        }

        private bool SelectProfilePoint(MouseEventArgs e)
        {
            int width = Width / 2;
            int height = Height / 2;

            if (e.X < width && e.Y < height)
            {
                float margin = 20;
                float f_l = margin * 2;
                float f_w = width - margin * 4;

                if (e.X >= f_l && e.X <= (f_l + f_w))
                {
                    if (onTrackEndPoint)
                    {
                        profileRightPct = (e.X - f_l) * 100 / f_w;
                    }
                    else
                    {
                        profileLeftPct = (e.X - f_l) * 100 / f_w;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool SelectCrossSectionPoint(MouseEventArgs e)
        {
            var point = new Vector3();
            point.X = e.X;
            point.Y = (float)surfaceViewportVec[3] - e.Y;
            float zValue = 0;
            GL.ReadPixels((int)point.X, (int)point.Y, 1, 1, PixelFormat.DepthComponent, PixelType.Float, ref zValue);
            point.Z = zValue;

            if (point.Z != 1)
            {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                OpenTK.Graphics.Glu.UnProject(point, surfaceModelVec, surfaceProjVec, surfaceViewportVec, out Vector3 destPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                if (ModifierKeys == Keys.Control)
                {
                    crossSection.EndPoint = new PointF(destPt.X, destPt.Z);
                }
                else
                {
                    crossSection.StartPoint = new PointF(destPt.X, destPt.Z);
                }

                return true;
            }

            return false;
        }

        public static float CalculateAngle(Vector3 first, Vector3 second)
        {
            return (float)System.Math.Acos(Math.Min((Vector3.Dot(first, second)) / (first.Length * second.Length), 1.0f));
        }

        private bool AddFittingPoint(MouseEventArgs e)
        {
            var point = new Vector3();
            point.X = e.X;
            point.Y = (float)surfaceViewportVec[3] - e.Y;
            float zValue = 0;
            GL.ReadPixels((int)point.X, (int)point.Y, 1, 1, PixelFormat.DepthComponent, PixelType.Float, ref zValue);
            point.Z = zValue;

            if (point.Z != 1)
            {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                OpenTK.Graphics.Glu.UnProject(point, surfaceModelVec, surfaceProjVec, surfaceViewportVec, out Vector3 destPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

                int numFittingPoint = (fittingMode == FittingMode.Cad ? 6 : 3);
                if (fittingPointList.Count() == numFittingPoint)
                {
                    fittingPointList.Clear();
                }

                var fittingPoint = new Point3d(destPt.X, destPt.Y, destPt.Z);
                fittingPointList.Add(fittingPoint);

                return true;
            }

            return false;
        }

        private void CurrentGL_MouseMove(object sender, MouseEventArgs e)
        {
            if (onMouseMoving)
            {
                if (e.Button == MouseButtons.Right)
                {
                    bool controlPushed = (ModifierKeys == Keys.Control);
                    switch (mouseMode)
                    {
                        case MouseMode.Move:
                            cameraPosX -= (mouseStartPos.X - e.X) * (maxLength / Width);
                            cameraPosY += (mouseStartPos.Y - e.Y) * (maxLength / Height);
                            break;
                        case MouseMode.Rotate:
                            {
                                if (controlPushed)
                                {
                                    cameraAngleZ -= e.Delta / 20;// * (maxLength / 100);
                                }
                                else
                                {
                                    int stepX = Math.Abs(e.X - mouseStartPos.X);
                                    int stepY = Math.Abs(e.Y - mouseStartPos.Y);

                                    if (stepX > stepY)
                                    {
                                        cameraAngleY += e.X - mouseStartPos.X;
                                    }
                                    else
                                    {
                                        cameraAngleX += e.Y - mouseStartPos.Y;
                                    }
                                }
                            }
                            projectionType = ProjectionType.Perspective;
                            break;
                        case MouseMode.MoveCad:
                            {
                                Matrix4 projectionMat;
                                if (controlPushed)
                                {
                                    float stepZ = e.Delta * (maxLength / 1000);

                                    projectionMat = Matrix4.CreateTranslation(0, 0, stepZ);
                                }
                                else
                                {
                                    float stepX = -(mouseStartPos.X - e.X) * (maxLength / Width);
                                    float stepY = -(mouseStartPos.Y - e.Y) * (maxLength / Height);

                                    if (projectionType == ProjectionType.Ortho)
                                    {
                                        if (cameraAngleX == 0 && cameraAngleY == 0)
                                        {
                                            projectionMat = Matrix4.CreateTranslation(stepX, 0, -stepY);
                                        }
                                        else if (cameraAngleX == 0 && cameraAngleY == 90)
                                        {
                                            projectionMat = Matrix4.CreateTranslation(0, stepX, -stepY);
                                        }
                                        else
                                        {
                                            projectionMat = Matrix4.CreateTranslation(stepX, stepY, 0);
                                        }
                                    }
                                    else
                                    {
                                        projectionMat = Matrix4.CreateTranslation(stepX, stepY, 0);
                                    }
                                }
                                surfaceRender.TransformCad(projectionMat);
                            }
                            break;
                        case MouseMode.RotateCad:
                            {
                                Matrix4 projectionMat;
                                if (controlPushed)
                                {
                                    projectionMat = Matrix4.CreateRotationZ(e.X - mouseStartPos.X);
                                }
                                else
                                {
                                    int stepY = Math.Abs(e.Y - mouseStartPos.Y) / 10;
                                    int stepX = Math.Abs(e.X - mouseStartPos.X) / 10;

                                    if (projectionType == ProjectionType.Ortho)
                                    {
                                        if (cameraAngleX == 0 && cameraAngleY == 0)
                                        {
                                            if (stepY > stepX)
                                            {
                                                projectionMat = Matrix4.CreateRotationX(-(e.Y - mouseStartPos.Y) / 50.0f);
                                            }
                                            else
                                            {
                                                projectionMat = Matrix4.CreateRotationZ((e.X - mouseStartPos.X) / 50.0f);
                                            }
                                        }
                                        else if (cameraAngleX == 0 && cameraAngleY == 90)
                                        {
                                            if (stepY > stepX)
                                            {
                                                projectionMat = Matrix4.CreateRotationY(-(e.Y - mouseStartPos.Y) / 50.0f);
                                            }
                                            else
                                            {
                                                projectionMat = Matrix4.CreateRotationZ((e.X - mouseStartPos.X) / 50.0f);
                                            }
                                        }
                                        else
                                        {
                                            if (stepY > stepX)
                                            {
                                                projectionMat = Matrix4.CreateRotationX(-(e.Y - mouseStartPos.Y) / 50.0f);
                                            }
                                            else
                                            {
                                                projectionMat = Matrix4.CreateRotationY((e.X - mouseStartPos.X) / 50.0f);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (stepY > stepX)
                                        {
                                            projectionMat = Matrix4.CreateRotationX(-(e.Y - mouseStartPos.Y) / 50.0f);
                                        }
                                        else
                                        {
                                            projectionMat = Matrix4.CreateRotationY((e.X - mouseStartPos.X) / 50.0f);
                                        }
                                    }

                                    surfaceRender.RotateCad(projectionMat);
                                }
                            }
                            break;
                    }

                    mouseStartPos = new Point(e.X, e.Y);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    //if (curTrackType != TrackType.None)
                    //{
                    //    trackPos[(int)curTrackType].Offset((mouseStartPos.X - e.X), (mouseStartPos.Y - e.Y));
                    //    mouseStartPos = new Point(e.X, e.Y);
                    //}
                    //else
                    if (legendRect.Contains(e.X, e.Y))
                    {
                        if (ModifierKeys == Keys.Control)
                        {
                            surfaceRender.RangeRatioMin = (float)(legendRect.Bottom - e.Y) / legendRect.Height;
                        }
                        else
                        {
                            surfaceRender.RangeRatioMax = (float)(legendRect.Bottom - e.Y) / legendRect.Height;
                        }

                        surfaceRender.ClearList();
                    }
                    else
                    {
                        if (onTrackCrossSectionPoint)
                        {
                            SelectCrossSectionPoint(e);
                        }
                        else if (onTrackProfilePoint)
                        {
                            SelectProfilePoint(e);
                        }
                    }
                }

                currentGL.Invalidate();
            }
        }

        private void CurrentGL_MouseUp(object sender, MouseEventArgs e)
        {
            onMouseMoving = false;
            onTrackCrossSectionPoint = false;
            onTrackProfilePoint = false;
        }

        private void CurrentGL_MouseWheel(object sender, MouseEventArgs e)
        {
            cameraDistance += e.Delta * (maxLength / 1000);
            cameraDistance = Base.MathHelper.Bound(cameraDistance, 0.01f, maxLength * 3);

            currentGL.Invalidate();
        }

        private void LoadToolStripButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "3D Data Files (.3d)|*.3d|STL Files (.stl)|*.stl|All Files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileExt = Path.GetExtension(dialog.FileName);

                if (fileExt == ".3d")
                {
                    var depthData = new Image3D();
                    depthData.Load(dialog.FileName);

                    SetDepthData(depthData, PixelResolution);
                }
                else if (fileExt == ".stl")
                {
                    CadImporter cadImporter = CadImporterFactory.Create(CadType.STL);
                    Cad3dModel cad3dModel = cadImporter.Import(dialog.FileName);

                    SetCadData(cad3dModel);
                }

                currentGL.Invalidate();
            }
        }

        private Image3D GetDepthImage()
        {
            int width = currentGL.Width;
            int height = currentGL.Height;

            int xOffset = 0;
            int yOffset = 0;

            int pixelLength = 0;
            if (width >= height)
            {
                xOffset = (width - height) / 2;
                pixelLength = height;
            }
            else
            {
                yOffset = (height - width) / 2;
                pixelLength = Width;
            }

            float pelRes = maxLength / pixelLength;

            if (surfaceRender.DataWidth > surfaceRender.DataHeight)
            {
                yOffset += (int)((surfaceRender.DataWidth - surfaceRender.DataHeight) / 2 / pelRes);
            }
            else
            {
                xOffset += (int)((surfaceRender.DataHeight - surfaceRender.DataWidth) / 2 / pelRes);
            }

            int pointWidth = (int)(surfaceRender.DataWidth / pelRes);
            int pointHeight = (int)(surfaceRender.DataHeight / pelRes);
            float[] zValue = new float[width * height];
            GL.ReadPixels<float>(0, 0, width, height, PixelFormat.DepthComponent, PixelType.Float, zValue);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (zValue[y * width + x] < 1 && zValue[y * width + x] > 0)
                    {
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                        OpenTK.Graphics.Glu.UnProject(new Vector3(x, y, zValue[y * width + x]), surfaceModelVec, surfaceProjVec, surfaceViewportVec, out Vector3 destPt);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                        zValue[y * width + x] = destPt.Y;
                    }
                }
            }

            var image3d = new Image3D();
            image3d.Initialize(width, height, 1);
            image3d.SetData(zValue);

            var clipImage = (Image3D)image3d.ClipImage(new Rectangle(xOffset, yOffset, pointWidth, pointHeight));
            clipImage = (Image3D)clipImage.FlipX();

            return (Image3D)clipImage.Resize((int)surfaceRender.Width, (int)surfaceRender.Height);
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "3D Files| *.3d|3D Projection Files| *.3dp|All Files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileExt = Path.GetExtension(dialog.FileName);
                if (fileExt == ".3d")
                {
                    if (surfaceRender.CloudDepthData != null)
                    {
                        surfaceRender.CloudDepthData.Save(dialog.FileName);
                    }
                }
                else if (fileExt == ".3dp")
                {
                    Image3D depthImage = GetDepthImage();
                    depthImage.Save(dialog.FileName);
                }
            }
        }

        private void PlaneXyTtoolStripButton_Click(object sender, EventArgs e)
        {
            projectionType = ProjectionType.Ortho;

            cameraAngleX = 90;
            cameraAngleY = 0;
            cameraPosX = 0;
            cameraPosY = 0;

            cameraDistance = maxLength * 2;

            currentGL.Invalidate();
        }

        private void PlaneXzToolStripButton_Click(object sender, EventArgs e)
        {
            projectionType = ProjectionType.Ortho;
            cameraAngleX = 0;
            cameraAngleY = 0;
            cameraPosX = 0;
            cameraPosY = 0;

            cameraDistance = maxLength * 2;

            currentGL.Invalidate();
        }

        private void PlaneYzToolStripButton_Click(object sender, EventArgs e)
        {
            projectionType = ProjectionType.Ortho;
            cameraAngleX = 0;
            cameraAngleY = 90;
            cameraPosX = 0;
            cameraPosY = 0;

            cameraDistance = maxLength * 2;

            currentGL.Invalidate();
        }

        private void CheckMouseModeButton(MouseMode mouseMode)
        {
            this.mouseMode = mouseMode;
            //rotateToolStripButton.Checked = (mouseMode == MouseMode.Rotate);
            //panToolStripButton.Checked = (mouseMode == MouseMode.Pan);
            //moveCadXYToolStripButton.Checked = (mouseMode == MouseMode.MoveCadXY);
            //moveCadZToolStripButton.Checked = (mouseMode == MouseMode.MoveCadZ);
            //rotateCadXToolStripButton.Checked = (mouseMode == MouseMode.RotateCadX);
            //rotateCadYToolStripButton.Checked = (mouseMode == MouseMode.RotateCadY);
            //rotateCadZToolStripButton.Checked = (mouseMode == MouseMode.RotateCadZ);
        }

        private Vector3 GetCenterVector(Vector3 pt1, Vector3 pt2, Vector3 pt3)
        {
            var vector = new Vector3();
            vector.X = (pt1.X + pt2.X + pt3.X) / 3;
            vector.Y = (pt1.Y + pt2.Y + pt3.Y) / 3;
            vector.Z = (pt1.Z + pt2.Z + pt3.Z) / 3;

            return vector;
        }

        private void PlaneFitting()
        {
            if (fittingMode == FittingMode.Plane)
            {
                fittingMode = FittingMode.None;

                if (fittingPointList.Count == 3)
                {
                    var pts = new Vector3[3];

                    int idx = 0;
                    foreach (Point3d pt in fittingPointList)
                    {
                        pts[idx++] = new Vector3((float)pt.X, (float)pt.Z, (float)pt.Y);
                    }

                    Vector3 center = GetCenterVector(pts[0], pts[1], pts[2]);

                    var offsetMat1 = Matrix4.CreateTranslation(Vector3.Multiply(center, -1));


                    var pts2 = new Vector3[3];
                    for (int i = 0; i < 3; i++)
                    {
                        pts2[i] = Vector4.Transform(new Vector4(pts[i], 1.0f), offsetMat1).Xyz;
                    }

                    var dir1 = Vector3.Cross(pts2[1] - pts2[0], pts2[2] - pts2[0]);
                    var norm1 = Vector3.Normalize(dir1);

                    //Vector3 dir2 = Vector3.Cross(new Vector3(1, 0, 0), new Vector3(0, 0, 1));
                    //Vector3 norm2 = Vector3.Normalize(dir2);

                    var norm2 = new Vector3(0, 0, 1);

                    var rotateAxis = Vector3.Cross(norm1, norm2);
                    float rotateAngle = CalculateAngle(norm1, norm2);
                    var rotateMat1 = Matrix4.CreateFromAxisAngle(rotateAxis, rotateAngle);

                    var pts3 = new Vector3[3];
                    for (int i = 0; i < 3; i++)
                    {
                        pts3[i] = Vector4.Transform(new Vector4(pts2[i], 1.0f), rotateMat1).Xyz;
                    }

                    surfaceRender.TransformCad(offsetMat1);
                    surfaceRender.TransformCad(rotateMat1);
                }
            }
            else
            {
                fittingMode = FittingMode.Plane;
            }

            fittingPointList.Clear();
        }

        private void CadFitting1()
        {
            if (fittingMode == FittingMode.Cad)
            {
                fittingMode = FittingMode.None;

                if (fittingPointList.Count == 6)
                {
                    var pts = new List<Vector3>();
                    foreach (Point3d pt in fittingPointList)
                    {
                        pts.Add(new Vector3((float)pt.X, (float)pt.Z, (float)pt.Y));
                    }

                    Vector3 center1 = GetCenterVector(pts[0], pts[1], pts[2]);
                    Vector3 center2 = GetCenterVector(pts[3], pts[4], pts[5]);

                    // List<Vector3> pts2 = new List<Vector3>();
                    var offsetMat1 = Matrix4.CreateTranslation(Vector3.Multiply(center2, -1));

                    surfaceRender.TransformCad(offsetMat1);

                    var pts2 = new Vector3[3];
                    for (int i = 0; i < 3; i++)
                    {
                        pts2[i] = Vector4.Transform(new Vector4(pts[i + 3], 1.0f), offsetMat1).Xyz;
                    }

                    var dir1 = Vector3.Cross(pts[1] - pts[0], pts[2] - pts[0]);
                    var norm1 = Vector3.Normalize(dir1);

                    var dir2 = Vector3.Cross(pts2[1] - pts2[0], pts2[2] - pts2[0]);
                    var norm2 = Vector3.Normalize(dir2);

                    var rotateAxis = Vector3.Cross(norm1, norm2);
                    float rotateAngle = CalculateAngle(norm1, norm2);
                    var rotateMat1 = Matrix4.CreateFromAxisAngle(rotateAxis, -rotateAngle);

                    surfaceRender.TransformCad(rotateMat1);
                    var pts3 = new Vector3[3];
                    for (int i = 0; i < 3; i++)
                    {
                        pts3[i] = Vector4.Transform(new Vector4(pts2[i], 1.0f), rotateMat1).Xyz;
                    }

                    var offsetMat2 = Matrix4.CreateTranslation(Vector3.Multiply(center1, 1));

                    var pts4 = new Vector3[3];
                    for (int i = 0; i < 3; i++)
                    {
                        pts4[i] = Vector4.Transform(new Vector4(pts3[i], 1.0f), offsetMat2).Xyz;
                    }

                    var orgPt1 = Vector3.Subtract(pts[0], center1);
                    var orgPt2 = Vector3.Subtract(pts4[0], center1);
                    float rotateAngle2 = Vector3.CalculateAngle(orgPt1, orgPt2);
                    var rotateMat2 = Matrix4.CreateFromAxisAngle(norm1, rotateAngle2);

                    surfaceRender.TransformCad(rotateMat2);
                    surfaceRender.TransformCad(offsetMat2);

                    projectionType = ProjectionType.Ortho;

                    currentGL.Invalidate();
                }
            }
            else
            {
                fittingMode = FittingMode.Cad;
            }

            fittingPointList.Clear();
        }

        private void CadFitting2()
        {
            projectionType = ProjectionType.Ortho;

            cameraAngleX = 90;
            cameraAngleY = 0;
            cameraPosX = 0;
            cameraPosY = 0;

            cameraDistance = maxLength * 2;

            showLegend = false;
            surfaceRender.ShowCad = true;
            surfaceRender.ShowCadDepth = false;
            surfaceRender.ShowDepth = false;
            currentGL.Invalidate();
            currentGL.Update();

            Image3D cadDepthData = GetDepthImage();
            surfaceRender.SetCadDepthData(cadDepthData);

            cadDepthData.Save(@"D:\Test.3d");

            showLegend = true;
            surfaceRender.ShowCad = false;
            surfaceRender.ShowCadDepth = true;
            surfaceRender.ShowDepth = false;

            currentGL.Invalidate();

            Image3D alignedCadData = null; // (Image3D)Mil3dMeasure.Alignment(surfaceRender.CloudDepthData, cadDepthData).FlipX();

            surfaceRender.SetCadDepthData(alignedCadData);
        }

        private void FillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surfaceRender.PolygonMode = PolygonMode.Fill;
            fillToolStripMenuItem.Checked = true;
            wireframeToolStripMenuItem.Checked = false;
            pointCloudToolStripMenuItem.Checked = false;

            currentGL.Invalidate();
        }

        private void WireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surfaceRender.PolygonMode = PolygonMode.Line;
            fillToolStripMenuItem.Checked = false;
            wireframeToolStripMenuItem.Checked = true;
            pointCloudToolStripMenuItem.Checked = false;

            currentGL.Invalidate();
        }

        private void PointCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surfaceRender.PolygonMode = PolygonMode.Point;
            fillToolStripMenuItem.Checked = false;
            wireframeToolStripMenuItem.Checked = false;
            pointCloudToolStripMenuItem.Checked = true;

            currentGL.Invalidate();
        }

        private void RainbowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupTextureColor(TextureColorType.Rainbow);
            currentGL.Invalidate();
        }

        private void Hsv1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupTextureColor(TextureColorType.Hsv1);
            currentGL.Invalidate();
        }

        private void Hsv2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupTextureColor(TextureColorType.Hsv2);
            currentGL.Invalidate();
        }

        private void GrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupTextureColor(TextureColorType.Gray);
            currentGL.Invalidate();
        }

        private void DefaultHeightScaleToolStripButton_Click(object sender, EventArgs e)
        {
            surfaceRender.HeightScale = 1.0f;
            currentGL.Invalidate();
        }

        private void IncreaseHeightScaleToolStripButton_Click(object sender, EventArgs e)
        {
            surfaceRender.HeightScale *= 1.1f;
            currentGL.Invalidate();
        }

        private void DecreaseHeightScaleToolStripButton_Click(object sender, EventArgs e)
        {
            surfaceRender.HeightScale /= 1.1f;
            currentGL.Invalidate();
        }

        private void DrawCircle(int n)
        {
            double delta = 2.0 * Math.PI / n;
            GL.PushMatrix();

#pragma warning disable CS0618 // Type or member is obsolete
            GL.Begin(BeginMode.TriangleFan);
#pragma warning restore CS0618 // Type or member is obsolete
            {
                GL.Vertex2(Vector2.Zero);
                for (int i = 0; i <= n; i++)
                {
                    double x = Math.Cos(delta * i);
                    double y = Math.Sin(delta * i);
                    GL.Vertex2(x, y);
                }
            }
            GL.End();
            GL.PopMatrix();
        }

        private void ShowCrossSectionToolStripButton_Click(object sender, EventArgs e)
        {
            crossSection.Enabled = !crossSection.Enabled;
            showCrossSectionToolStripButton.Checked = crossSection.Enabled;

            currentGL.Invalidate();
        }

        private void CurrentGL_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            mouseActionRadialMenu.Expanded = true;
            mouseActionRadialMenu.Show(ParentForm, new Point(e.X, e.Y));
        }

        private void MouseActionRadialMenu_ToolClick(object sender, Infragistics.Win.UltraWinRadialMenu.RadialMenuToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "Move":
                    CheckMouseModeButton(MouseMode.Move);
                    break;
                case "Rotate":
                    CheckMouseModeButton(MouseMode.Rotate);
                    break;
                case "MoveCad":
                    CheckMouseModeButton(MouseMode.MoveCad);
                    break;
                case "RotateCad":
                    CheckMouseModeButton(MouseMode.RotateCad);
                    break;
                case "PlaneFitting":
                    PlaneFitting();
                    break;
                case "CadFitting1":
                    CadFitting1();
                    break;
                case "CadFitting2":
                    CadFitting2();
                    break;
            }

            mouseActionRadialMenu.Hide();
        }

        private void ShowDepthToolStripButton_Click(object sender, EventArgs e)
        {
            surfaceRender.ShowDepth = (surfaceRender.ShowDepth == false);

            currentGL.Invalidate();
        }

        private void ShowCadToolStripButton_Click(object sender, EventArgs e)
        {
            surfaceRender.ShowCad = (surfaceRender.ShowCad == false);

            currentGL.Invalidate();
        }

        private void ShowCadDepthToolStripButton_Click(object sender, EventArgs e)
        {
            surfaceRender.ShowCadDepth = (surfaceRender.ShowCadDepth == false);

            currentGL.Invalidate();
        }
    }
}
