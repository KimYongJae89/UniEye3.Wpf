using OpenTK;
using OpenTK.Input;
/**
* Initial @author Eberhard Graether / http://egraether.com/
* C# port by Michael Ivanov (sasmaster) 
*
* This utility was written for OpenTK SDK.
* It wasn't tested with other C# based graphics libraries.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DynMvp.UI
{
    internal class Light
    {
        public Light(Vector3 position, Vector3 color, float diffuseintensity = 1.0f, float ambientintensity = 1.0f)
        {
            Position = position;
            Color = color;

            DiffuseIntensity = diffuseintensity;
            AmbientIntensity = ambientintensity;

            Type = LightType.Point;
            Direction = new Vector3(0, 0, 1);
            ConeAngle = 15.0f;
        }

        public Vector3 Position;
        public Vector3 Color;
        public float DiffuseIntensity;
        public float AmbientIntensity;

        public LightType Type;
        public Vector3 Direction;
        public float ConeAngle;
    }





    public enum State
    {
        NONE,
        ROTATE,
        ZOOM,
        PAN
    };

    public class Camera3D
    {
        public Vector3 Up { get; set; }
        public Vector3 _pos;

        public Vector3 Pos
        {
            get => _pos;
            set => _pos = value;
        }

        public Matrix4 ViewMatrix { get; set; }

        public Camera3D()
        {
            _pos = Vector3.Zero;
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            ViewMatrix = Matrix4.Identity;
        }
        public void LookAt(Vector3 target)
        {
            ViewMatrix = Matrix4.LookAt(_pos, target, Up);
        }
    }

    public class TrackBallControlls
    {
        private const double SQRT1_2 = 0.7071067811865476;
        public Camera3D _camObject;
        public State _state, _prevState;

        public float RotateSpeed { get; set; }

        public float ZoomSpeed { get; set; }

        public float PanSpeed { get; set; }

        public bool NoRotate { get; set; }

        public bool NoZoom { get; set; }

        public bool NoPan { get; set; }

        public bool NoRoll { get; set; }


        public bool StaticMoving { get; set; }
        public bool Enabled { get; set; }

        public float DynamicDampingFactor { get; set; }

        public float MinDistance { get; set; }

        public float MaxDistance { get; set; }

        public Rectangle Screen { get; set; }

        private Vector3 _target;
        private Vector3 _eye;
        private Vector3 _rotateStart, _rotateEnd;
        private Vector2 _zoomStart, _zoomEnd;
        private Vector2 _panStart, _panEnd;
        private Vector3 lastPosition;
        private List<int> _keys;

        public TrackBallControlls(Camera3D camObject, Rectangle screenSize, GameWindow win)
        {
            _camObject = camObject;
            Enabled = true;


            Screen = screenSize;

            RotateSpeed = 0.8f;
            ZoomSpeed = 1.2f;
            PanSpeed = 0.3f;

            NoRotate = false;
            NoZoom = false;
            NoPan = false;
            NoRoll = false;

            StaticMoving = false;
            DynamicDampingFactor = 0.2f;

            MinDistance = 0.0f;
            MaxDistance = float.PositiveInfinity;

            _target = Vector3.Zero;

            lastPosition = Vector3.Zero;

            _state = State.NONE;
            _prevState = State.NONE;

            _eye = Vector3.Zero;

            _rotateStart = Vector3.Zero;
            _rotateEnd = Vector3.Zero;

            _zoomStart = Vector2.Zero;
            _zoomEnd = Vector2.Zero;

            _panStart = Vector2.Zero;
            _panEnd = Vector2.Zero;

            _keys = new List<int> { 65 /*A*/, 83 /*S*/, 68 /*D*/};

            win.MouseMove += OpenGLTess_MouseMove;
            win.MouseUp += OpenGLTess_MouseUp;
            win.MouseDown += OpenGLTess_MouseDown;
            win.MouseWheel += OpenGLTess_MouseWheel;
            win.KeyUp += OpenGLTess_KeyUp;
            win.KeyDown += OpenGLTess_KeyDown;
        }

        public TrackBallControlls(Camera3D camObject, Rectangle screenSize)
        {
            _camObject = camObject;
            Enabled = true;


            Screen = screenSize;

            RotateSpeed = 1.0f;
            ZoomSpeed = 1.2f;
            PanSpeed = 0.3f;

            NoRotate = false;
            NoZoom = false;
            NoPan = false;
            NoRoll = false;

            StaticMoving = false;
            DynamicDampingFactor = 0.6f;

            MinDistance = 0.0f;
            MaxDistance = float.PositiveInfinity;

            _target = Vector3.Zero;

            lastPosition = Vector3.Zero;

            _state = State.NONE;
            _prevState = State.NONE;

            _eye = Vector3.Zero;

            _rotateStart = Vector3.Zero;
            _rotateEnd = Vector3.Zero;

            _zoomStart = Vector2.Zero;
            _zoomEnd = Vector2.Zero;

            _panStart = Vector2.Zero;
            _panEnd = Vector2.Zero;

            _keys = new List<int> { 65 /*A*/, 83 /*S*/, 68 /*D*/};
        }

        private Vector2 GetMouseOnScreen(int clientX, int clientY)
        {
            return new Vector2(
                (clientX - Screen.Left) / (float)Screen.Width,
                (clientY - Screen.Top) / (float)Screen.Height
                );
        }

        private Vector3 GetMouseProjectionOnBall(int clientX, int clientY)
        {
            var mouseOnBall = new Vector3(
              (clientX - Screen.Width * 0.5f) / (Screen.Width * 0.5f),
               (Screen.Height * 0.5f - clientY) / (Screen.Height * 0.5f),
               0.0f
            );

            double length = mouseOnBall.Length;

            if (NoRoll)
            {
                if (length < SQRT1_2)
                {
                    mouseOnBall.Z = (float)Math.Sqrt(1.0 - length * length);
                }
                else
                {
                    mouseOnBall.Z = (float)(0.5 / length);
                }
            }
            else if (length > 1.0)
            {
                mouseOnBall.Normalize();
            }
            else
            {
                mouseOnBall.Z = (float)Math.Sqrt(1.0 - length * length);
            }

            Vector3 camPos = _camObject.Pos;

            _eye = Vector3.Subtract(camPos, _target);


            var upClone = new Vector3(_camObject.Up);
            Vector3 projection;//object.up.clone().normalize().scale(mouseOnBall.y);
            upClone.Normalize();

            projection = Vector3.Multiply(upClone, mouseOnBall.Y);

            //  projection.add(object.up.cross(_eye).normalize().scale(mouseOnBall.x));
            var cross = Vector3.Cross(_camObject.Up, _eye);
            cross.Normalize();
            cross = Vector3.Multiply(cross, mouseOnBall.X);
            projection = Vector3.Add(projection, cross);

            //  projection.add(_eye.normalize().scale(mouseOnBall.z));
            var eyeClone = new Vector3(_eye);
            eyeClone.Normalize();
            projection = Vector3.Add(projection, Vector3.Multiply(eyeClone, mouseOnBall.Z));

            return projection;

        }

        private void RotateCamera()
        {
            float angle = (float)Math.Acos(Vector3.Dot(_rotateStart, _rotateEnd) / _rotateStart.Length / _rotateEnd.Length);

            if (!float.IsNaN(angle) && angle != 0.0f)
            {
                var axis = Vector3.Cross(_rotateStart, _rotateEnd); //_rotateStart.cross(_rotateEnd).normalize();
                axis.Normalize();
                if (float.IsNaN(axis.X))
                {
                    axis = Vector3.Zero; /// a hack,sometimes NAN comes from "axis" and fucks up everything. Zeroing of it resolves the issue.
                }

                Quaternion quaternion = Quaternion.Identity;

                angle *= RotateSpeed;

                //  quaternion.setAxisAngle(axis, angle);
                quaternion = Quaternion.FromAxisAngle(axis, -angle);

                //quaternion.rotate(_eye);
                _eye = Vector3.Transform(_eye, quaternion);

                //  quaternion.rotate(object.up);
                _camObject.Up = Vector3.Transform(_camObject.Up, quaternion);
                //  quaternion.rotate(_rotateEnd);
                _rotateEnd = Vector3.Transform(_rotateEnd, quaternion);


                if (StaticMoving)
                {
                    _rotateStart = new Vector3(_rotateEnd);
                }
                else
                {
                    quaternion = Quaternion.FromAxisAngle(axis, angle * (DynamicDampingFactor - 1.0f));
                    _rotateStart = Vector3.Transform(_rotateStart, quaternion);
                }
            }
        }

        private void ZoomCamera()
        {
            float factor = 1.0f + (_zoomEnd.Y - _zoomStart.Y) * ZoomSpeed;
            if (factor != 1.0f && factor > 0.0f)
            {
                //  _eye.scale( factor );
                _eye = Vector3.Multiply(_eye, factor);

                if (StaticMoving)
                {
                    _zoomStart = new Vector2(_zoomEnd.X, _zoomEnd.Y);
                }
                else
                {
                    _zoomStart.Y += (_zoomEnd.Y - _zoomStart.Y) * DynamicDampingFactor;
                }
            }
        }

        private void PanCamera()
        {
            Vector2 mouseChange = _panEnd - _panStart;
            if (mouseChange.Length != 0.0f)
            {
                // mouseChange.scale( _eye.Length * _panSpeed );
                mouseChange = Vector2.Multiply(mouseChange, _eye.Length * PanSpeed);

                //   Vector3 pan = _eye.cross( object.up ).normalize().scale( mouseChange.x );
                var pan = Vector3.Cross(_eye, _camObject.Up);
                pan.Normalize();
                pan = Vector3.Multiply(pan, mouseChange.X);

                // pan += object.up.clone().normalize().scale( mouseChange.Y );
                var camUpClone = new Vector3(_camObject.Up);
                camUpClone.Normalize();
                camUpClone = Vector3.Multiply(camUpClone, mouseChange.Y);
                pan += camUpClone;

                //object.position.add( pan );
                _camObject._pos = Vector3.Add(_camObject.Pos, pan);

                //  target.add( pan );
                _target = Vector3.Add(_target, pan);
                if (StaticMoving)
                {
                    _panStart = _panEnd;
                }
                else
                {
                    Vector2 diff = _panEnd - _panStart;
                    diff = Vector2.Multiply(diff, DynamicDampingFactor);
                    _panStart += diff;// (_panEnd - _panStart).scale(_dynamicDampingFactor);
                }
            }
        }

        private void CheckDistances()
        {
            if (!NoZoom || !NoPan)
            {
                if (_camObject.Pos.LengthSquared > MaxDistance * MaxDistance)
                {
                    _camObject._pos.Normalize();
                    _camObject._pos = Vector3.Multiply(_camObject._pos, MaxDistance);
                }

                if (_eye.LengthSquared < MinDistance * MinDistance)
                {
                    // object.position = target + _eye.normalize().scale(minDistance);
                    _eye.Normalize();
                    _eye = Vector3.Multiply(_eye, MinDistance);

                    _camObject._pos = _target + _eye;
                }
            }
        }

        public void Update()
        {
            //   _eye.setFrom( object.position ).sub( target );
            _eye = new Vector3(_camObject.Pos);
            _eye = Vector3.Subtract(_eye, _target);
            if (!NoRotate)
            {
                RotateCamera();
            }

            if (!NoZoom)
            {
                ZoomCamera();
            }

            if (!NoPan)
            {
                PanCamera();
            }

            // object.position =  target + _eye;
            _camObject._pos = _target + _eye;
            CheckDistances();
            // object.lookAt( target );
            _camObject.LookAt(_target);

            // distanceToSquared
            if ((lastPosition - _camObject.Pos).LengthSquared > 0.0f)
            {
                //
                //   dispatchEvent( changeEvent );
                lastPosition = new Vector3(_camObject.Pos);
            }

            /*
            string str = "";
            str += _camObject.Pos.X.ToString() + ",";
            str += _camObject.Pos.Y.ToString() + ",";
            str += _camObject.Pos.Z.ToString();
            Debug.WriteLine(str);
            */
        }

        ///////////////////event listeners///////////////////////////////////////
        private void OpenGLTess_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (!Enabled) { return; }

            _state = _prevState;
        }
        public void OpenGLTess_KeyUp()
        {
            if (!Enabled) { return; }

            _state = _prevState;
        }

        private void OpenGLTess_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            _prevState = _state;

            KeyboardState state = OpenTK.Input.Keyboard.GetState();

            if (_state != State.NONE)
            {
                return;
            }
            else if (e.Key == Key.A/* event.keyCode == keys[ STATE.ROTATE ]*/ && !NoRotate)
            {
                _state = State.ROTATE;
            }
            else if (e.Key == Key.S /* event.keyCode == keys[ STATE.ZOOM ]*/ && !NoZoom)
            {
                _state = State.ZOOM;
            }
            else if (e.Key == Key.D /* event.keyCode == keys[ STATE.PAN ]*/ && !NoPan)
            {
                _state = State.PAN;
            }
        }
        public void OpenGLTess_KeyDown(KeyboardKeyEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            _prevState = _state;

            KeyboardState state = OpenTK.Input.Keyboard.GetState();

            if (_state != State.NONE)
            {
                return;
            }
            else if (e.Key == Key.A/* event.keyCode == keys[ STATE.ROTATE ]*/ && !NoRotate)
            {
                _state = State.ROTATE;
            }
            else if (e.Key == Key.S /* event.keyCode == keys[ STATE.ZOOM ]*/ && !NoZoom)
            {
                _state = State.ZOOM;
            }
            else if (e.Key == Key.D /* event.keyCode == keys[ STATE.PAN ]*/ && !NoPan)
            {
                _state = State.PAN;
            }
        }

        private void OpenGLTess_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            if (!Enabled) { return; }

            float delta = 0.0f;

            if (e.Delta != 0)
            { // Firefox
                delta = -e.Delta / 3.0f;
            }
            _zoomStart.Y += delta * 0.05f;
        }

        public void OpenGLTess_MouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            if (!Enabled) { return; }

            float delta = 0.0f;

            if (e.Delta != 0)
            { // Firefox
                delta = -e.Delta / 300.0f;
            }
            _zoomStart.Y += delta * 0.05f;
        }

        private void OpenGLTess_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (!Enabled) { return; }

            if (_state == State.NONE)
            {
                if (OpenTK.Input.Mouse.GetState()[MouseButton.Right])
                {
                    _state = State.PAN;
                }
                else
                {
                    _state = State.ROTATE;
                }
                //   _state = e.Button;//  event.button;
            }

            if (_state == State.ROTATE && !NoRotate)
            {
                _rotateStart = GetMouseProjectionOnBall(e.X, e.Y);
                _rotateEnd = _rotateStart;
            }
            else if (_state == State.ZOOM && !NoZoom)
            {
                _zoomStart = GetMouseOnScreen(e.X, e.Y);
                _zoomEnd = _zoomStart;
            }
            else if (_state == State.PAN && !NoPan)
            {
                _panStart = GetMouseOnScreen(e.X, e.Y);
                _panEnd = _panStart;
            }
        }
        public void OpenGLTess_MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (!Enabled) { return; }

            if (_state == State.NONE)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    _state = State.PAN;
                }
                else
                {
                    _state = State.ROTATE;
                }
                //   _state = e.Button;//  event.button;
            }

            if (_state == State.ROTATE && !NoRotate)
            {
                _rotateStart = GetMouseProjectionOnBall(e.X, e.Y);
                _rotateEnd = _rotateStart;
            }
            else if (_state == State.ZOOM && !NoZoom)
            {
                _zoomStart = GetMouseOnScreen(e.X, e.Y);
                _zoomEnd = _zoomStart;
            }
            else if (_state == State.PAN && !NoPan)
            {
                _panStart = GetMouseOnScreen(e.X, e.Y);
                _panEnd = _panStart;
            }
        }

        private void OpenGLTess_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (!Enabled) { return; }
            _state = State.NONE;
        }

        public void OpenGLTess_MouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (!Enabled) { return; }
            _state = State.NONE;
        }

        private void OpenGLTess_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            if (!Enabled) { return; }

            if (_state == State.ROTATE && !NoRotate)
            {
                _rotateEnd = GetMouseProjectionOnBall(e.X, e.Y);
            }
            else if (_state == State.ZOOM && !NoZoom)
            {
                _zoomEnd = GetMouseOnScreen(e.X, e.Y);
            }
            else if (_state == State.PAN && !NoPan)
            {
                _panEnd = GetMouseOnScreen(e.X, e.Y);
            }
        }

        public void OpenGLTess_MouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (!Enabled) { return; }

            if (_state == State.ROTATE && !NoRotate)
            {
                _rotateEnd = GetMouseProjectionOnBall(e.X, e.Y);
            }
            else if (_state == State.ZOOM && !NoZoom)
            {
                _zoomEnd = GetMouseOnScreen(e.X, e.Y);
            }
            else if (_state == State.PAN && !NoPan)
            {
                _panEnd = GetMouseOnScreen(e.X, e.Y);
            }
        }
    }
}
