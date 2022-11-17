using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class JoystickAxisForm : Form
    {
        private bool initialized = false;
        public Camera Camera { get; set; }

        private LightParamSet lightParamSet = null;
        private IJoystickControl joystickControl;

        public MovableCheckDelegate MovableCheck
        {
            get => joystickControl.GetMovableCheckDelegate();
            set => joystickControl.SetMovableCheckDelegate(value);
        }

        public JoystickAxisForm(AxisHandler axisHandler)
        {
            InitializeComponent();
            int numAxis = 0;
            if (axisHandler != null)
            {
                numAxis = axisHandler.NumUniqueAxis;
            }

            switch (numAxis)
            {
                case 2:
                    joystickControl = new Joystick2AxisControl();
                    break;
                case 3:
                    joystickControl = new Joystick3AxisControl();
                    break;
                default:
                    joystickControl = null;
                    break;
            }

            if (joystickControl != null)
            {
                joystickControl.InitControl();
                joystickControl.Initialize(axisHandler);
                panelJoystick.Controls.Add((UserControl)joystickControl);

                axisHandler.OnEndMove += axisHandler_RobotMoved;

                initialized = true;
            }

            comboLightType.SelectedIndex = 0;
        }

        public void ToggleView(IWin32Window parentWnd, LightParamSet lightParamSet)
        {
            if (Visible)
            {
                Hide();
            }
            else
            {
                this.lightParamSet = lightParamSet;
                Show(parentWnd);
            }
        }

        private void axisHandler_RobotMoved(AxisPosition axisPosition)
        {
            if (Visible == false || lightParamSet == null)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new RobotEventDelegate(axisHandler_RobotMoved), axisPosition);
                return;
            }

            if (Camera != null)
            {
                LightCtrlHandler lightCtrlHandler = DeviceManager.Instance().LightCtrlHandler;

                LightParam lightParam = lightParamSet[comboLightType.SelectedIndex];
                LightValue lightValue = lightParam.LightValue;
                lightCtrlHandler.TurnOn(lightValue);

                Camera.GrabOnce();
                Camera.WaitGrabDone();

                ImageD grabImage = Camera.GetGrabbedImage();

                pictureImage.Image = grabImage.ToBitmap();
                //cameraView.Invalidate();

                lightCtrlHandler.TurnOff();
            }
        }

        //public void Initialize(AxisHandler axisHandler, ImageDevice imageDevice)
        //{
        //    cameraView.Tag = imageDevice;
        //    cameraView.SetImageDevice(imageDevice);

        //    this.joystickControl.Initialize(axisHandler);
        //    this.initialized = true;
        //}

        private void joystickControl_RobotMoved(AxisPosition axisPosition)
        {
            throw new NotImplementedException();
        }

        private void JoystickAxisForm_Load(object sender, EventArgs e)
        {
            if (!initialized)
            {
                Close();
                return;
            }
        }

        private void JoystickAxisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
