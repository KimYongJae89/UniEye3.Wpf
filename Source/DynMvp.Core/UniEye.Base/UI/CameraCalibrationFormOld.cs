using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class CameraCalibrationFormOld : Form
    {
        private LightCtrlHandler lightCtrlHandler;
        private CameraHandler cameraHandler;
        private Calibration curCameraCalibration;
        private List<Calibration> cameraCalibrationList;

        public CameraCalibrationFormOld()
        {
            InitializeComponent();

            labelCamera.Text = StringManager.GetString(labelCamera.Text);
            calibrationTypeSingleScale.Text = StringManager.GetString(calibrationTypeSingleScale.Text);
            calibrationTypeGrid.Text = StringManager.GetString(calibrationTypeGrid.Text);
            calibrationTypeChessboard.Text = StringManager.GetString(calibrationTypeChessboard.Text);
            labelNumRow.Text = StringManager.GetString(labelNumRow.Text);
            labelRowSpace.Text = StringManager.GetString(labelRowSpace.Text);
            labelNumCol.Text = StringManager.GetString(labelNumCol.Text);
            labelColSpace.Text = StringManager.GetString(labelColSpace.Text);
            buttonGrab.Text = StringManager.GetString(buttonGrab.Text);
            buttonCalibrate.Text = StringManager.GetString(buttonCalibrate.Text);
            buttonSaveCalibration.Text = StringManager.GetString(buttonSaveCalibration.Text);
            buttonLoadCalibration.Text = StringManager.GetString(buttonLoadCalibration.Text);

        }

        public void Initialize()
        {
            lightCtrlHandler = DeviceManager.Instance().LightCtrlHandler;
            cameraHandler = DeviceManager.Instance().CameraHandler;
            cameraCalibrationList = SystemManager.Instance().CameraCalibrationList;
        }

        private void CameraCalibrationForm_Load(object sender, EventArgs e)
        {
            foreach (Camera camera in cameraHandler)
            {
                comboBoxCamera.Items.Add(string.Format("Camera {0}", camera.Index));
            }

            comboBoxCamera.SelectedIndex = 0;
            comboLightType.SelectedIndex = 0;
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
            {
                return;
            }

            int cameraIndex = Convert.ToInt32(comboBoxCamera.Text.Substring(7));

            Camera camera = cameraHandler.GetCamera(cameraIndex);

            if (camera != null)
            {
                if (lightCtrlHandler != null)
                {
                    LightParamSet lightParamSet = LightConfig.Instance().LightParamSet;
                    LightParam lightParam = lightParamSet[comboLightType.SelectedIndex];
                    LightValue lightValue = lightParam.LightValue;
                    lightCtrlHandler.TurnOn(lightValue);
                }

                camera.GrabOnce();
                cameraHandler.WaitGrabDone();

                ImageD grabImage = camera.GetGrabbedImage();

                calibrationImage.Image = grabImage.ToBitmap();
            }
        }

        private void buttonCalibrate_Click(object sender, EventArgs e)
        {
            if (curCameraCalibration == null)
            {
                return;
            }

            int cameraIndex = Convert.ToInt32(comboBoxCamera.Text.Substring(7));
            Camera camera = cameraHandler.GetCamera(cameraIndex);
            ImageD grabImage = camera.GetGrabbedImage();

            if (calibrationTypeSingleScale.Checked == true)
            {
                curCameraCalibration.CalibrationType = CalibrationType.SingleScale;
                curCameraCalibration.Calibrate(Convert.ToSingle(pelWidth.Text), Convert.ToSingle(pelHeight.Text));
            }
            //else if (calibrationTypeGrid.Checked == true)
            //{
            //    if (curCameraCalibration.Calibrate(grabImage, (int)numRow.Value, (int)numCol.Value, Convert.ToSingle(rowSpace.Text), Convert.ToSingle(colSpace.Text)) == false)
            //    {
            //        MessageBox.Show("Calibration Fail");
            //        return;
            //    }

            //    curCameraCalibration.TransformImage(grabImage);

            //    calibrationImage.Image = grabImage.ToBitmap();
            //    //calibrationImage.Image = null;
            //}
            //else if (calibrationTypeChessboard.Checked == true)
            //{
            //    var bitmap = (Bitmap)calibrationImage.Image;
            //    ImageD image = Image2D.ToImage2D(bitmap);

            //    curCameraCalibration.CalibrationType = CalibrationType.ChessBoard;
            //    if (curCameraCalibration.Calibrate(image, (int)numRow.Value, (int)numCol.Value, Convert.ToSingle(rowSpace.Text), Convert.ToSingle(colSpace.Text)) == false)
            //    {
            //        MessageBox.Show("Calibration Fail");
            //        return;
            //    }

            //    curCameraCalibration.TransformImage(image);

            //    calibrationImage.Image = image.ToBitmap();
            //}
        }

        private void buttonSaveCalibration_Click(object sender, EventArgs e)
        {
            if (curCameraCalibration != null)
            {
                curCameraCalibration.Save();

                int cameraIndex = Convert.ToInt32(comboBoxCamera.Text.Substring(7));
                Camera camera = cameraHandler.GetCamera(cameraIndex);

                camera.UpdateFovSize(curCameraCalibration.PelSize);
            }
        }

        private void buttonLoadCalibration_Click(object sender, EventArgs e)
        {
            if (curCameraCalibration != null)
            {
                curCameraCalibration.Load();
                if (curCameraCalibration.CalibrationType == CalibrationType.SingleScale)
                {
                    calibrationTypeSingleScale.Checked = true;
                }
                else if (curCameraCalibration.CalibrationType == CalibrationType.Grid)
                {
                    calibrationTypeGrid.Checked = true;
                }
                //else if (curCameraCalibration.CalibrationType == CalibrationType.ChessBoard)
                //{
                //    calibrationTypeChessboard.Checked = true;
                //}
            }
        }

        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
            {
                return;
            }

            int cameraIndex = Convert.ToInt32(comboBoxCamera.Text.Substring(7));

            Camera camera = cameraHandler.GetCamera(cameraIndex);

            Calibration cameraCalibration = cameraCalibrationList.Find(f => f.CameraIndex == camera.Index);
            if (cameraCalibration == null)
            {
                return;
            }

            curCameraCalibration = cameraCalibration;

            pelWidth.Text = curCameraCalibration.PelSize.Width.ToString();
            pelHeight.Text = curCameraCalibration.PelSize.Height.ToString();
            numRow.Value = curCameraCalibration.PatternCount.Width;
            numCol.Value = curCameraCalibration.PatternCount.Height;
            rowSpace.Text = curCameraCalibration.PatternSize.Width.ToString();
            colSpace.Text = curCameraCalibration.PatternSize.Height.ToString();

            if (curCameraCalibration.CalibrationType == CalibrationType.SingleScale)
            {
                calibrationTypeSingleScale.Checked = true;
            }
            else if (curCameraCalibration.CalibrationType == CalibrationType.Grid)
            {
                calibrationTypeGrid.Checked = true;
            }
            //else if (curCameraCalibration.CalibrationType == CalibrationType.ChessBoard)
            //{
            //    calibrationTypeChessboard.Checked = true;
            //}

            return;
        }

        private void CalibrationTypeChanged(object sender, EventArgs e)
        {
            if (curCameraCalibration == null)
            {
                return;
            }

            if (calibrationTypeSingleScale.Checked == true)
            {
                curCameraCalibration.CalibrationType = CalibrationType.SingleScale;
                pelWidth.Enabled = true;
                pelHeight.Enabled = true;
                numRow.Enabled = false;
                numCol.Enabled = false;
                rowSpace.Enabled = false;
                colSpace.Enabled = false;
            }
            else if (calibrationTypeGrid.Checked == true)
            {
                curCameraCalibration.CalibrationType = CalibrationType.Grid;
                pelWidth.Enabled = false;
                pelHeight.Enabled = false;
                numRow.Enabled = true;
                numCol.Enabled = true;
                rowSpace.Enabled = true;
                colSpace.Enabled = true;
            }
            //else if (calibrationTypeChessboard.Checked == true)
            //{
            //    curCameraCalibration.CalibrationType = CalibrationType.ChessBoard;
            //    pelWidth.Enabled = false;
            //    pelHeight.Enabled = false;
            //    numRow.Enabled = true;
            //    numCol.Enabled = true;
            //    rowSpace.Enabled = true;
            //    colSpace.Enabled = true;
            //}
        }
        private void calibrationTypeSingleScale_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
