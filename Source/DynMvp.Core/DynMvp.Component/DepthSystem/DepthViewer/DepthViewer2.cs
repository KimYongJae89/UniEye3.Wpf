using DynMvp.Base;
using DynMvp.Cad;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public partial class DepthViewer2 : UserControl
    {
        private GL gl = new GL();
        private DepthViewPanel depthViewPanel;
        private CadViewPanel cadViewPanel;
        private ProfilePanel profilePanel;
        private CrossSection crossSection;
        private MouseMode mouseMode = MouseMode.Rotate;
        private Point mouseStartPos;
        private Point nowMousePos;
        private bool onMouseMoving = false;
        private bool showProfile;

        public DepthViewer2()
        {
            InitializeComponent();

            cadViewPanel = new CadViewPanel();
            currentGL.MouseWheel += new MouseEventHandler(currentGL_MouseWheel);

            rotateToolStripButton.Checked = true;

            Setup();
        }

        private void Setup()
        {
            if (!currentGL.Context.IsCurrent)
            {
                currentGL.MakeCurrent();
            }

            SetupEnable();

            GL.ClearStencil(0);
            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Lequal);

            SetupLights();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

            crossSection = new CrossSection();

            depthViewPanel = new DepthViewPanel();
            depthViewPanel.Viewport = new Rectangle(0, 0, Width, Height);
            depthViewPanel.CrossSection = crossSection;
            depthViewPanel.InitCoordinate(new Rectangle(0, 0, Width, Height), new Point3d(100, 100, 100), 10);
            //cadViewPanel = new CadViewPanel();
            //cadViewPanel.Viewport = new Rectangle(0, 0, Width, Height);

            profilePanel = new ProfilePanel();
            profilePanel.Viewport = new Rectangle(0, 0, Width / 2, Height / 2);
            profilePanel.CrossSection = crossSection;

            depthViewPanel.SetSurfaceMode(SurfaceMode.TextureColor);
            depthViewPanel.SetTextureColor(TextureColorType.Rainbow);
        }

        private void SetupLights()
        {
            {
                GL.Light(LightName.Light0, LightParameter.Ambient, new float[4] { .0f, .0f, .0f, 1.0f });
                GL.Light(LightName.Light0, LightParameter.Diffuse, new float[4] { .8f, .8f, .8f, 1.0f });
                GL.Light(LightName.Light0, LightParameter.Specular, new float[4] { 1, 1, 1, 1 });

                // position the light
                float[] lightPos = new float[4] { 0, 100, 10, 0 };
                GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 0, 100, 10, 0 });

                GL.Enable(EnableCap.Light0);
            }

            {
                GL.Light(LightName.Light1, LightParameter.Ambient, new float[4] { .0f, .0f, .0f, 1.0f });
                GL.Light(LightName.Light1, LightParameter.Diffuse, new float[4] { 1.0f, 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light1, LightParameter.Specular, new float[4] { 1, 1, 1, 1 });

                // position the light
                float[] lightPos = new float[4] { 0, 100, 10, 0 };
                GL.Light(LightName.Light1, LightParameter.Position, new float[4] { -10, 10, 10, 0 });

                GL.Enable(EnableCap.Light1);
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

        public void SetDepthData(Image3D depthData, SizeF pixelResolution)
        {
            depthViewPanel.SetDepthData(depthData, pixelResolution);
        }

        private void currentGL_Paint(object sender, PaintEventArgs e)
        {
            if (!currentGL.Context.IsCurrent)
            {
                currentGL.MakeCurrent();
            }

            SetupEnable();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            GL.DepthFunc(DepthFunction.Lequal);

            depthViewPanel.Draw();

            DisableTexture();

            //            profilePanel.Draw();

            GL.DepthFunc(DepthFunction.Always);

            currentGL.SwapBuffers();
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

        private void currentGL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                onMouseMoving = true;
                mouseStartPos = new Point(e.X, e.Y);
            }

            if (e.Button == MouseButtons.Left)
            {
                depthViewPanel.SelectPoint(e.X, e.Y, ModifierKeys == Keys.Control);
            }

            currentGL.Refresh();
        }

        private void currentGL_MouseMove(object sender, MouseEventArgs e)
        {
            if (onMouseMoving)
            {
                if (mouseMode == MouseMode.Move)
                {
                    depthViewPanel.EyePoint.X -= ((10.0f * (mouseStartPos.X - e.X) / (Width / 2.0f)));
                    depthViewPanel.EyePoint.Z += ((10.0f * (mouseStartPos.Y - e.Y) / (Height / 2.0f)));
                }
                else
                {
                    depthViewPanel.EyeAngle.X += (e.X - mouseStartPos.X);
                    depthViewPanel.EyeAngle.Y += (e.Y - mouseStartPos.Y);

                    crossSection.Enabled = false;
                    depthViewPanel.ProjectionType = ProjectionType.Perspective;
                }

                if (e.Button == MouseButtons.Right)
                {
                    Tracker(mouseStartPos);
                }

                mouseStartPos = new Point(e.X, e.Y);

                currentGL.Invalidate();
            }
        }

        private void Tracker(Point mousePoint)
        {
            if ((mousePoint.X < (currentGL.Width / 2) && mousePoint.Y < (currentGL.Height / 2)) == false)
            {
                return;
            }

            nowMousePos = mousePoint;
        }

        private void currentGL_MouseUp(object sender, MouseEventArgs e)
        {
            onMouseMoving = false;
        }

        private void currentGL_MouseWheel(object sender, MouseEventArgs e)
        {
            depthViewPanel.EyePoint.Y += e.Delta * 0.01f;
            depthViewPanel.EyePoint.Y = Base.MathHelper.Bound((float)depthViewPanel.EyePoint.Y, 0.01f, 100);
            currentGL.Invalidate();
        }

        private void loadToolStripButton_Click(object sender, EventArgs e)
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

                    depthViewPanel.SetDepthData(depthData, new SizeF(0.3f, 0.3f));
                }
                else if (fileExt == ".stl")
                {
                    CadImporter cadImporter = CadImporterFactory.Create(CadType.STL);
                    cadViewPanel.SetCadData(cadImporter.Import(dialog.FileName));
                }

                currentGL.Invalidate();
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void planeXyTtoolStripButton_Click(object sender, EventArgs e)
        {
            depthViewPanel.SetPlaneXy();

            currentGL.Invalidate();
        }

        private void planeXzToolStripButton_Click(object sender, EventArgs e)
        {
            depthViewPanel.SetPlaneXz();

            currentGL.Invalidate();
        }

        private void planeYzToolStripButton_Click(object sender, EventArgs e)
        {
            currentGL.Invalidate();

            depthViewPanel.SetPlaneYz();
        }

        private void panToolStripButton_Click(object sender, EventArgs e)
        {
            mouseMode = MouseMode.Move;
            rotateToolStripButton.Checked = false;
            panToolStripButton.Checked = true;
        }

        private void rotateToolStripButton_Click(object sender, EventArgs e)
        {
            mouseMode = MouseMode.Rotate;
            rotateToolStripButton.Checked = true;
            panToolStripButton.Checked = false;
        }

        private void fillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.PolygonMode = PolygonMode.Fill;
            fillToolStripMenuItem.Checked = true;
            wireframeToolStripMenuItem.Checked = false;
            pointCloudToolStripMenuItem.Checked = false;

            currentGL.Invalidate();
        }

        private void wireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.PolygonMode = PolygonMode.Line;
            fillToolStripMenuItem.Checked = false;
            wireframeToolStripMenuItem.Checked = true;
            pointCloudToolStripMenuItem.Checked = false;

            currentGL.Invalidate();
        }

        private void pointCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.PolygonMode = PolygonMode.Point;
            fillToolStripMenuItem.Checked = false;
            wireframeToolStripMenuItem.Checked = false;
            pointCloudToolStripMenuItem.Checked = true;

            currentGL.Invalidate();
        }

        private void rainbowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.SetTextureColor(TextureColorType.Rainbow);
            currentGL.Invalidate();
        }

        private void hsv1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.SetTextureColor(TextureColorType.Hsv1);
            currentGL.Invalidate();
        }

        private void hsv2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.SetTextureColor(TextureColorType.Hsv2);
            currentGL.Invalidate();
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            depthViewPanel.SetTextureColor(TextureColorType.Gray);
            currentGL.Invalidate();
        }

        private void defaultHeightScaleToolStripButton_Click(object sender, EventArgs e)
        {
            depthViewPanel.HeightScale = 1.0f;
            currentGL.Invalidate();
        }

        private void increaseHeightScaleToolStripButton_Click(object sender, EventArgs e)
        {
            depthViewPanel.HeightScale *= 1.1f;
            currentGL.Invalidate();
        }

        private void decreaseHeightScaleToolStripButton_Click(object sender, EventArgs e)
        {
            depthViewPanel.HeightScale /= 1.1f;
            currentGL.Invalidate();
        }

        private void currentGL_Resize(object sender, EventArgs e)
        {
            if (depthViewPanel != null)
            {
                depthViewPanel.Viewport = new Rectangle(0, 0, Width, Height);
                currentGL.Invalidate();
            }
        }

        private void showCrossSectionToolStripButton_Click(object sender, EventArgs e)
        {
            showProfile = !showProfile;
            currentGL.Invalidate();
        }
    }
}
