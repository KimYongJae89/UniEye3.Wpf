using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.UI
{
    internal enum LightType { Point, Spot, Directional }
    public partial class GL3DViewerPanel : UserControl
    {
        protected Vector3 _position;
        protected Vector3 _rotation;
        protected Matrix4 _modelView;
        protected Vector3 _scale;
        private bool isMouseDown = false;

        //Vector3 oldDragPoint, newDragPoint;

        private Camera3D _camera3D;
        private TrackBallControlls _tracballCtrl;

        //DataController _dataCtrl = new DataController();
        //static GameWindow gWindow;

        private int mouse_x, mouse_y;
        private int linePos_X, linePos_Y;

        private enum ProfileLneAxis { X_Axis, Y_Axis, None }

        private enum MouseWheelMode { Zoom, ProfilePos, None }

        private ProfileLneAxis currentSelecteAxis = ProfileLneAxis.X_Axis;
        private MouseWheelMode currentMouseWheelMode = MouseWheelMode.Zoom;

        //int sampling = 1;
        //Bitmap IMAGE = null;// new Bitmap(OPEN.FileName);

        private int width;
        private int height;

        //VBO
        private uint indexBufferId;
        private uint vertexBufferId;
        private uint colorBufferId;

        private struct Byte4
        {
            public byte R, G, B, A;

            public Byte4(byte[] input)
            {
                R = input[0];
                G = input[1];
                B = input[2];
                A = input[3];
            }

            public uint ToUInt32()
            {
                byte[] temp = new byte[] { R, G, B, A };
                return BitConverter.ToUInt32(temp, 0);
            }

            public override string ToString()
            {
                return R + ", " + G + ", " + B + ", " + A;
            }
        }


        public GL3DViewerPanel()
        {
            InitializeComponent();

            width = height = 0;

            _position.X = _position.Y = _position.Z = 0;
            _rotation.X = _rotation.Y = _rotation.Z = 0;
            _modelView.ClearProjection();
            _modelView.ClearRotation();
            _modelView.ClearScale();
            _modelView.ClearTranslation();
            _scale.X = _scale.Y = _scale.Z = 0;

            _camera3D = new Camera3D();
            _camera3D.Pos = new Vector3(0, 0, 0);
            _camera3D.Up = new Vector3(0, 1, 0);
            _camera3D.LookAt(new Vector3(0, 0, 0));

            _tracballCtrl = new TrackBallControlls(_camera3D, new Rectangle(0, 0, glControl.Width, glControl.Height));

            _tracballCtrl.NoZoom = false;
            _tracballCtrl.NoRotate = false;
            _tracballCtrl.NoPan = false;

            var rand = new Random();



            //IMAGE = new Bitmap("C:\\work\\Img.BMP");
            /*
                        int i;
                        int w =  IMAGE.Width;
                        int h =  IMAGE.Height;

                        float[] data = new float[w * h];

                        int x, y;
                        int xx, yy;

                        int ww;
                        int hh;


                        using (FileStream file = File.OpenRead("C:\\work\\raw.dat"))
                        {
                            using (BinaryReader reader = new BinaryReader(file))
                            {
                                ww = reader.ReadInt32();
                                hh = reader.ReadInt32();
                                for (i = 0; i < (w * h); ++i)// foreach (int index in data)
                                {
                                    data[i] = reader.ReadSingle();
                                    //if (data[i] == 0) data[i] = float.NaN;
                                }
                            }
                        }
                        */
            /*
            for (y = 0; y < h; ++y)
            {
                for (x = 0; x < w; ++x)
                {                    
                    data[(y ) * w + (x )] = (float)Math.Sqrt(((float)x - w / 2.0f) * ((float)x - w / 2.0f) + ((float)y - h / 2.0f) * ((float)y - h / 2.0f)) + rand.Next(0, (int)((w + h) * 0.02)); 
                }
            }     

            //int xx = rand.Next(1, w-1);
            //int yy = rand.Next(1, h-1);
            for( i = 0; i < (w*h)*0.02; ++i)
            {
                x = rand.Next(1, w - 1);
                y = rand.Next(1, h - 1);
                data[(y) * w + (x)] = float.NaN;
            }
            
            linePos_X = 0;
            linePos_Y = 0;


            DataController.Use().SetData(w, h, data);
*/

        }

        public void setData(float[] data, int w, int h, Bitmap img)
        {
            //DataController.Use().SetData(w, h, data);
            //if (IMAGE == null)
            //    IMAGE = new Bitmap(img);
            //else
            //    IMAGE = img;
            if (img.Width != w || img.Height != h)
            {
                return;
            }

            int i;
            int x, y;
            width = w;
            height = h;
            //VBO
            // Set-up index buffer:
            uint[] indices = new uint[w * h];
            for (i = 0; i < (w * h); ++i)// foreach (int index in data)
            {
                indices[i] = (uint)i;
            }

            GL.GenBuffers(1, out indexBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);
            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                (IntPtr)(indices.Length * sizeof(uint)),
                indices,
                BufferUsageHint.StaticDraw);

            // Set-up vertex buffer:
            float[] vertexData = new float[w * h * 3];
            for (y = 0; y < h; ++y)
            {
                for (x = 0; x < w * 3; x += 3)
                {
                    vertexData[y * (w * 3) + x + 0] = -(w / 2 - x / 3);
                    vertexData[y * (w * 3) + x + 1] = h / 2 - y;
                    vertexData[y * (w * 3) + x + 2] = data[(y) * w + (x / 3)] * 0.2f; // - DataController.Use().zData.Min();
                }
            }

            GL.GenBuffers(1, out vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                (IntPtr)(vertexData.Length * sizeof(float)),
                vertexData,
                BufferUsageHint.StaticDraw);


            // Set-up color buffer:
            ///////////////////////////////////////////////
            // Create a new bitmap.         
            // Lock the bitmap's bits.  
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                img.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                img.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * img.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red.  
            // for (int counter = 2; counter < rgbValues.Length; counter += 3)
            //     rgbValues[counter] = 255;

            byte[] colorData = new byte[bytes];
            for (y = 0; y < h; ++y)
            {
                for (x = 0; x < w * 3; x += 3)
                {
                    colorData[y * (w * 3) + x + 0] = rgbValues[y * (w * 3) + x + 2];
                    colorData[y * (w * 3) + x + 1] = rgbValues[y * (w * 3) + x + 1];
                    colorData[y * (w * 3) + x + 2] = rgbValues[y * (w * 3) + x + 0];
                }
            }
            //Array.Copy(rgbValues, colorData, bytes);

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            // Unlock the bits.
            img.UnlockBits(bmpData);
            ///////////////////////////////



            GL.GenBuffers(1, out colorBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferId);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                (IntPtr)(colorData.Length * sizeof(byte)),
                colorData,
                BufferUsageHint.StaticDraw);

            GL.PointSize(1.0f);

            GL.ClearColor(Color.DarkGray);
            /*
                        GL.Enable(EnableCap.DepthTest);

                        GL.ShadeModel(ShadingModel.Smooth);// 정점 색 보간

                        GL.Enable(EnableCap.Blend);
                        //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                        GL.Enable(EnableCap.PointSmooth);
                        GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);

                        GL.Enable(EnableCap.LineSmooth);
                        GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);

                        */

            // Create lights
            float[] globa_ambient = { 0.1f, 0.1f, 0.1f, 1.0f };

            float[] light0_ambient = { 0.5f, 0.4f, 0.3f, 1.0f };
            float[] light0_diffuse = { 0.5f, 0.4f, 0.3f, 1.0f };
            float[] light0_specular = { 1.0f, 1.0f, 1.0f, 1.0f };

            float[] light0Pos = { 5000, 5000, 5000, 1 };
            float[] light1Pos = { -5000, 5000, 5000, 1 };

            float[] material_ambient = { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] material_diffuse = { 0.8f, 0.8f, 0.8f, 1.0f };
            float[] material_specular = { 0.0f, 0.0f, 0.7f, 1.0f };
            float[] material_shininess = { 25.0f };

            /*
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Ambient, light0_ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, light0_diffuse);
            GL.Light(LightName.Light0, LightParameter.Specular, light0_specular);
            GL.Light(LightName.Light0, LightParameter.Position, light0Pos);

            GL.Enable(EnableCap.Light1);
            GL.Light(LightName.Light1, LightParameter.Ambient, light0_ambient);
            GL.Light(LightName.Light1, LightParameter.Diffuse, light0_diffuse);
            GL.Light(LightName.Light1, LightParameter.Specular, light0_specular);
            GL.Light(LightName.Light1, LightParameter.Position, light1Pos);

            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, material_ambient);
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, material_diffuse);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, material_specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, material_shininess);

            GL.LightModel(LightModelParameter.LightModelAmbient, globa_ambient);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);           
            */
            _tracballCtrl._camObject._pos.X = 0;
            _tracballCtrl._camObject._pos.Y = 0;// (float)Math.Sqrt(w * w + h * h) * 1.4f;
            _tracballCtrl._camObject._pos.Z = (float)Math.Sqrt(w * w + h * h) * 1.4f;
            img.Dispose();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
            // Ensure that the viewport and projection matrix are set correctly.
            glControl_Resize(glControl, EventArgs.Empty);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (glControl.ClientSize.Height == 0)
            {
                glControl.ClientSize = new System.Drawing.Size(glControl.ClientSize.Width, 1);
            }

            GL.Viewport(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);

            float aspect_ratio = Width / (float)Height;
            var perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 40000);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);

            _tracballCtrl.Screen = new Rectangle(0, 0, glControl.ClientSize.Width, glControl.ClientSize.Height);
        }

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            _tracballCtrl.OpenGLTess_MouseDown(e);
            isMouseDown = true;
        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            _tracballCtrl.OpenGLTess_MouseUp(e);
            if (isMouseDown)
            {
                isMouseDown = false;
                //Render();
            }
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            _tracballCtrl.OpenGLTess_MouseMove(e);
            if (isMouseDown)
            {
                mouse_x = e.X;
                mouse_y = e.Y;
                //computeDragPoint(e.X, e.Y, ref newDragPoint);
                //rotateCamera(oldDragPoint, ref newDragPoint);
                //oldDragPoint = newDragPoint;

                //Render();
            }
        }
        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (currentMouseWheelMode == MouseWheelMode.Zoom)
            {
                _tracballCtrl.OpenGLTess_MouseWheel(e);
                //Render();
            }
            else if (currentMouseWheelMode == MouseWheelMode.ProfilePos)
            {
                if (currentSelecteAxis == ProfileLneAxis.X_Axis)
                {
                    linePos_X += e.Delta / width;

                    if (linePos_X <= -width / 2)
                    {
                        linePos_X = -width / 2;
                    }
                    else if (linePos_X >= width / 2)
                    {
                        linePos_X = width / 2 - 1;
                    }
                }
                else if (currentSelecteAxis == ProfileLneAxis.Y_Axis)
                {
                    linePos_Y += e.Delta / height;

                    if (linePos_Y <= -height / 2)
                    {
                        linePos_Y = -height / 2;
                    }
                    else if (linePos_Y >= height / 2)
                    {
                        linePos_Y = height / 2 - 1;
                    }
                }

                //Limjunwoo();
                //Render();
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (glControl.IsIdle)
            {
                //if (_tracballCtrl._state != State.NONE)
                {
                    Render();
                    Thread.Sleep(60);
                }
                //Thread.Sleep(100);
            }
        }



        private void Render()
        {
            /*
            Matrix4 lookat = Matrix4.LookAt(0, 20, 20, 0, 0, 0, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            */

            /*
            var t2 = Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);
            var r1 = Matrix4.CreateRotationX(_rotation.X);
            var r2 = Matrix4.CreateRotationY(_rotation.Y);
            var r3 = Matrix4.CreateRotationZ(_rotation.Z);
            var s = Matrix4.CreateScale(_scale);
            _modelView = r1 * r2 * r3 * s * t2;
            */
            // * camera.LookAtMatrix;
            //GL.UniformMatrix4(21, false, ref _modelView);


            /*
            GL.PushMatrix();
            GL.Translate(_position); // My object is moving along the X axis
            GL.Rotate(_rotation.X, 1.0f, 0.0f, 0.0f); // Also it rotates alongs its 
            GL.Rotate(_rotation.Y, 0.0f, 1.0f, 0.0f); // Also it rotates alongs its 
            GL.Rotate(_rotation.Z, 0.0f, 0.0f, 1.0f); // Also it rotates alongs its 
            */

            /**/
            _tracballCtrl.Update();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            Matrix4 lookat = _tracballCtrl._camObject.ViewMatrix;
            //GL.Scale(-1, 1, 1);            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);



            //DrawZData();
            DrawZDataVBO();
            //draw3DAxis(2000);
            //drawProfielLine(currentSelecteAxis);
            glControl.SwapBuffers();
        }

        private void DrawZDataVBO()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Bind vertex buffer:
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            // Bind color buffer:
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferId);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.ColorPointer(3, ColorPointerType.UnsignedByte, 0, IntPtr.Zero);

            // Bind index buffer:
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);

            // Draw:            
#pragma warning disable CS0618 // Type or member is obsolete
            GL.DrawElements(BeginMode.Points, width * height, DrawElementsType.UnsignedInt, IntPtr.Zero);
#pragma warning restore CS0618 // Type or member is obsolete
            GL.Flush();

            // Disable:
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);

            //SwapBuffers();
        }

    }
}
