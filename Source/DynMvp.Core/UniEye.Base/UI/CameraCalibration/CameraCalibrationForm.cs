using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.Vision;
using Infragistics.Win.DataVisualization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.UI.CameraCalibration
{
    public partial class CameraCalibrationForm : Form, IMultiLanguageSupport
    {
        #region 생성자
        public CameraCalibrationForm()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            drawBox = new DrawBox();
            drawBox.Dock = DockStyle.Fill;
            drawBox.Enable = false;
            drawBox.BackColor = Control.DefaultBackColor;
            drawBox.AutoFitStyle = AutoFitStyle.FitAll;
            //drawBox.PictureBox.MouseMove += DrawBox_MouseMove;
            //drawBox.PictureBox.MouseLeave += DrawBox_MouseLeave;
            panelDrawBox.Controls.Add(drawBox);

            LightParamSet = LightConfig.Instance().LightParamSet.Clone();

            drawBoxToolTip = new ToolTip
            {
                ShowAlways = true
            };

            imageGrabbedDone = new ManualResetEvent(true);

            var calibrationConstant = new CalibrationConstant
            {
                //calibrationConstant.ChangeLanguage();
                Dock = DockStyle.Fill
            };

            var calibrationGrid = new CalibrationGrid
            {
                //calibrationGrid.ChangeLanguage();
                Dock = DockStyle.Fill
            };

            var calibrationRuler = new CalibrationRuler
            {
                //calibrationRuler.ChangeLanguage();
                Dock = DockStyle.Fill
            };

            tabPageContant.Controls.Add(calibrationConstant);
            tabPageGrid.Controls.Add(calibrationGrid);
            tabPageRuler.Controls.Add(calibrationRuler);
        }
        #endregion


        #region 속성
        private LightCtrlHandler LightCtrlHandler { get; set; }

        private LightParam lightParam { get; set; }

        private LightParamSet LightParamSet { get; set; }

        private CameraHandler CameraHandler { get; set; }

        private Calibration curCameraCalibration { get; set; }

        private List<Calibration> CameraCalibrationList { get; set; }

        private DrawBox drawBox { get; set; } = null;

        private ToolTip drawBoxToolTip { get; set; } = null;

        private CalibrationResult lastCalibrationResult { get; set; } = null;

        private ManualResetEvent imageGrabbedDone { get; set; } = null;

        private CameraCalibrationPanel curCameraCalibrationPanel { get; set; } = null;

        private Task asyncProcessTask { get; set; } = null;

        private object lockObject { get; set; } = new object();
        #endregion


        #region 이벤트

        private void CameraCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
            {
                return;
            }

            int cameraIndex = Convert.ToInt32(comboBoxCamera.SelectedIndex);
            var camera = CameraHandler.GetCamera(cameraIndex) as Camera;

            if (camera == null)
            {
                return;
            }

            if (camera.IsOnLive() == true)
            {
                camera.Stop();
                camera.ImageGrabbed -= ImageGrabbed;
            }

            var form = new SimpleProgressForm();
            form.Show(() => ResetCalibrationGrab(camera));
        }

        private void numericUpDownExternalExpose_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
            {
                return;
            }

            int cameraIndex = Convert.ToInt32(comboBoxCamera.SelectedIndex);

            var camera = CameraHandler.GetCamera(cameraIndex) as Camera;
            camera.SetExposureTime((float)numericUpDownExternalExpose.Value * 1000);

        }

        private void tabControlCalibrationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (curCameraCalibration == null)
            {
                return;
            }

            curCameraCalibrationPanel = (CameraCalibrationPanel)tabControlCalibrationType.SelectedTab.Controls[0];
            if (tabControlCalibrationType.SelectedIndex == 0)
            {
                curCameraCalibration.CalibrationType = CalibrationType.SingleScale;
            }
            else if (tabControlCalibrationType.SelectedIndex == 1)
            {
                curCameraCalibration.CalibrationType = CalibrationType.Grid;
            }
            else if (tabControlCalibrationType.SelectedIndex == 2)
            {
                curCameraCalibration.CalibrationType = CalibrationType.Ruler;
            }
        }

        private void comboLightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = comboLightType.SelectedIndex;
            if (idx < 0)
            {
                return;
            }

            var item = (ComboBoxItem)comboLightType.SelectedItem;
            if (radioDeviceLightType.Checked)
            {
                lightParam = item.lightParam;
            }
            else
            {
                lightParam = LightParamSet.LightParamList[item.ctrlId];
                int value = lightParam.LightValue.Value[item.ctrlCh];

                int maxValue = LightCtrlHandler.GetLightCtrl((uint)item.ctrlId).GetMaxLightLevel();

                trackBar1.Value = (int)Math.Round(value * 100.0f / maxValue);
                label13.Text = trackBar1.Value.ToString() + "%";

            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (radioDeviceLightType.Checked)
            {
                return;
            }

            int selIdx = comboLightType.SelectedIndex;
            if (selIdx < 0)
            {
                return;
            }

            var item = (ComboBoxItem)comboLightType.SelectedItem;
            int maxValue = LightCtrlHandler.GetLightCtrl((uint)item.ctrlId).GetMaxLightLevel();
            lightParam.LightValue.Value[item.ctrlCh] = (int)Math.Round(trackBar1.Value / 100f * maxValue);
            label13.Text = trackBar1.Value.ToString() + "%";
        }

        //private void UpdateValue()
        //{
        //    this.propRulerWidth = (float)propertyWidth.Value / 100f;
        //    this.propRulerHeight = (float)propertyHeight.Value / 100f;
        //    this.propRulerScale = (float)propertyScale.Value;
        //    this.propRulerReagionNum = (int)regionNum.Value;
        //    this.propRulerThreshold = (int)propertyThreshold.Value;
        //    this.propRulerPartial = (float)propertyPartial.Value / 100f;
        //}

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (drawBox.Image == null)
            {
                return;
            }

            var dlg = new SaveFileDialog
            {
                DefaultExt = ".bmp",
                FileName = string.Format("Calibration_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss")),
                Filter = "Bitmap(BMP)|*.bmp"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drawBox.Image.Save(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageForm.Show(null, ex.Message);
                }
            }
        }

        private void imageContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            drawBox.ZoomFit();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "bmp"
            };
            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            var bitmap = new Bitmap(dlg.FileName);
            var image2D = Image2D.FromBitmap(bitmap);

            lastCalibrationResult = CalibrateFunc(image2D);

            drawBox.UpdateImage(bitmap);
            drawBox.ZoomFit();
            if (lastCalibrationResult != null)
            {
                lastCalibrationResult.AppendFigure(drawBox.FigureGroup);
            }
        }

        private void buttonLightOn_Click(object sender, EventArgs e)
        {
            TurnOnLight();
        }

        private void buttonLightOff_Click(object sender, EventArgs e)
        {
            TurnOffLight();
        }

        private void CameraCalibrationForm_Resize(object sender, EventArgs e)
        {
            drawBox?.ZoomFit();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            var camera = CameraHandler.GetCamera(comboBoxCamera.SelectedIndex) as Camera;
            if (camera == null)
            {
                return;
            }

            var grabbedImage = camera.GetGrabbedImage() as Image2D;
            if (grabbedImage != null && grabbedImage.IsUseIntPtr())
            {
                var image2DD = (Image2D)grabbedImage.Clone();
                image2DD.ConvertFromDataPtr();
                grabbedImage = image2DD;
            }

            if (grabbedImage != null && camera is CameraVirtual)
            {
                grabbedImage = (Image2D)grabbedImage.ClipImage(new System.Drawing.Rectangle(0, 0, grabbedImage.Width, Math.Min(grabbedImage.Height, 256)));
            }

            if (curCameraCalibration == null)
            {
                return;
            }

            lastCalibrationResult = curCameraCalibrationPanel.Calibrate(curCameraCalibration, grabbedImage);
            UpdateResult(grabbedImage);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (Apply())
            {
                MessageForm.Show(null, "Calibrate OK");
            }
            else
            {
                MessageForm.Show(null, "Calibrate Fail");
            }
        }

        private void buttonSaveCalibration_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                MessageForm.Show(null, "Save OK");
            }
            else
            {
                MessageForm.Show(null, "Save Fail");
            }
        }

        private void buttonLoadCalibration_Click(object sender, EventArgs e)
        {
            if (curCameraCalibration != null)
            {
                curCameraCalibration.Load();
                UpdateCalibrateMenu();
            }
        }

        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraSelectedChanged(comboBoxCamera.SelectedIndex);

            label10.Text = curCameraCalibration.PelSize.Width.ToString("0.000");
            label3.Text = curCameraCalibration.PelSize.Height.ToString("0.000");
        }

        private void DrawBox_MouseLeave(object sender, EventArgs e)
        {
            drawBoxToolTip.Hide(drawBox);
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawBox.Image == null)
            {
                return;
            }

            CoordTransformer coordTransformer = drawBox.GetCoordTransformer();
            System.Drawing.Point imagePoint = coordTransformer.InverseTransform(e.Location);
            float fx = imagePoint.X * 100.0f / drawBox.Image.Width;
            float fy = imagePoint.Y * 100.0f / drawBox.Image.Height;

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("X: {0}px ({1}%)", imagePoint.X, fx));
            sb.AppendLine(string.Format("Y: {0}px ({1}%)", imagePoint.Y, fy));
            drawBoxToolTip.Show(sb.ToString(), drawBox, e.Location.X + 10, e.Location.Y + 10);
        }

        private void CameraCalibrationForm_Load(object sender, EventArgs e)
        {
            // Init Camera Combo
            InitCameraMenu();

            // Init Lights
            InitLightMenu();

            // Init Chart
            InitChartMenu();

            // init Calibrate Menu
            UpdateCalibrateMenu();
        }

        private void radioDeviceLight_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = (RadioButton)sender;
            if (radioDeviceLightType.Checked)
            {
                ChangeLightType();
            }

            if (radioDeviceLightValue.Checked)
            {
                ChangeLightValue();
            }
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
            {
                return;
            }

            var camera = CameraHandler.GetCamera(comboBoxCamera.SelectedIndex) as Camera;
            if (camera == null)
            {
                return;
            }

            if (camera.IsOnLive() == true)
            {
                return;
            }

            Grab(camera);
        }

        private void buttonGrabMulti_Click(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
            {
                return;
            }

            var camera = CameraHandler.GetCamera(comboBoxCamera.SelectedIndex) as Camera;

            if (camera == null)
            {
                return;
            }

            if (camera.IsOnLive() == false)
            {
                StartGrabMulti(camera);
            }
            else
            {
                StopGrab(camera);
            }
        }

        private void CameraSelectedChanged(int selIndex)
        {
            if (selIndex < 0)
            {
                return;
            }

            var camera = (Camera)CameraHandler.GetCamera(selIndex);

            float exposureTimeMs = camera.GetExposureTime();
            bool enable = !float.IsNaN(exposureTimeMs) && (exposureTimeMs >= 0);
            numericUpDownExternalExpose.Enabled = enable;
            if (enable)
            {
                numericUpDownExternalExpose.Value = (decimal)Math.Max(0, exposureTimeMs);
            }

            Calibration cameraCalibration = CameraCalibrationList.Find(f => f.CameraIndex == camera.Index);
            if (cameraCalibration == null)
            {
                return;
            }
            curCameraCalibration = cameraCalibration;


            UpdateCalibrateMenu();
        }
        #endregion


        #region 메서드
        public void Initialize()
        {
            LightCtrlHandler = DeviceManager.Instance().LightCtrlHandler;
            CameraHandler = DeviceManager.Instance().CameraHandler;
            CameraCalibrationList = SystemManager.Instance().CameraCalibrationList;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //labelCamera.Text = StringManager.GetString(this.GetType().FullName,labelCamera.Text);
            //calibrationTypeGrid.Text = StringManager.GetString(this.GetType().FullName,calibrationTypeGrid.Text);
            //calibrationTypeChessboard.Text = StringManager.GetString(this.GetType().FullName,calibrationTypeChessboard.Text);
            //labelNumRow.Text = StringManager.GetString(this.GetType().FullName,labelNumRow.Text);
            //labelRowSpace.Text = StringManager.GetString(this.GetType().FullName,labelRowSpace.Text);
            //labelNumCol.Text = StringManager.GetString(this.GetType().FullName,labelNumCol.Text);
            //labelColSpace.Text = StringManager.GetString(this.GetType().FullName,labelColSpace.Text);
            //buttonGrab.Text = StringManager.GetString(this.GetType().FullName,buttonGrab.Text);
            //buttonCalibrate.Text = StringManager.GetString(this.GetType().FullName,buttonCalibrate.Text);
            //buttonSaveCalibration.Text = StringManager.GetString(this.GetType().FullName,buttonSaveCalibration.Text);
            //buttonLoadCalibration.Text = StringManager.GetString(this.GetType().FullName,buttonLoadCalibration.Text);
        }

        private void InitCameraMenu()
        {
            foreach (Camera camera in CameraHandler)
            {
                if (camera != null)
                {
                    string name = camera.Name;
                    if (string.IsNullOrEmpty(name))
                    {
                        name = string.Format("Camera {0}", camera.Index);
                    }

                    comboBoxCamera.Items.Add(name);

                    SetCalibrationGrab(camera);
                }
            }

            //if (comboBoxCamera.Items.Count > 0)
            //{
            //    comboBoxCamera.SelectedIndex = 0;
            //}
        }

        private void InitLightMenu()
        {
            int numLightType = LightConfig.Instance().NumLightType;
            if (numLightType == 0)
            {
                ChangeLightValue();
                radioDeviceLightType.Enabled = false;
            }
            else
            {
                ChangeLightType();
            }
        }

        private void InitChartMenu()
        {

        }

        private void UpdateCalibrateMenu()
        {
            if (curCameraCalibration == null)
            {
                return;
            }

            if (curCameraCalibration.CalibrationType == CalibrationType.SingleScale)
            {
                tabControlCalibrationType.SelectedIndex = 0;
            }
            else if (curCameraCalibration.CalibrationType == CalibrationType.Grid)
            {
                tabControlCalibrationType.SelectedIndex = 1;
            }
            else if (curCameraCalibration.CalibrationType == CalibrationType.Ruler)
            {
                tabControlCalibrationType.SelectedIndex = 2;
            }

            curCameraCalibrationPanel = (CameraCalibrationPanel)tabControlCalibrationType.SelectedTab.Controls[0];
            //curCameraCalibrationPanel.ChangeLanguage();
            curCameraCalibrationPanel.UpdateData(curCameraCalibration);
            return;
        }


        private void ChangeLightValue()
        {
            radioDeviceLightValue.Checked = true;
            comboLightType.Items.Clear();

            for (int i = 0; i < LightCtrlHandler.Count; i++)
            {
                LightCtrl lightCtrl = LightCtrlHandler.GetLightCtrl((uint)i);
                string contorllorName = lightCtrl.Name;
                if (string.IsNullOrEmpty(contorllorName))
                {
                    contorllorName = string.Format("Ctrl. {0}", i);
                }

                for (int j = 0; j < lightCtrl.NumChannel; j++)
                {
                    string channelName = string.Format("Ch. {0}", j);
                    string text = string.Format("{0} - {1}", contorllorName, channelName);
                    comboLightType.Items.Add(new ComboBoxItem(text, i, j));
                }
            }

            if (comboLightType.Items.Count > 0)
            {
                comboLightType.SelectedIndex = 0;
            }
        }

        private void ChangeLightType()
        {
            radioDeviceLightType.Checked = true;
            comboLightType.Items.Clear();

            for (int i = 0; i < LightParamSet.LightParamList.Count; i++)
            {
                string name = LightParamSet.LightParamList[i].Name;
                if (string.IsNullOrEmpty(name))
                {
                    name = string.Format("Type {0}", i);
                }

                comboLightType.Items.Add(new ComboBoxItem(name, LightParamSet.LightParamList[i]));
            }

            if (comboLightType.Items.Count > 0)
            {
                comboLightType.SelectedIndex = 0;
            }
        }


        private void Grab(Camera camera)
        {
            LockButtons();

            camera.ImageGrabbed += ImageGrabbed;
            imageGrabbedDone.Reset();

            camera.SetTriggerMode(TriggerMode.Software, TriggerType.RisingEdge);

            DeviceManager.Instance().LightCtrlHandler.TurnOn();
            Thread.Sleep(10);

            camera.GrabOnce();
            camera.WaitGrabDone();
            new SimpleProgressForm("Processing").Show(() => imageGrabbedDone.WaitOne(5000));
            //imageGrabbedDone.WaitOne(5000);

            camera.Stop();
            camera.ImageGrabbed -= ImageGrabbed;

            DeviceManager.Instance().LightCtrlHandler.TurnOff();

            ReleaseButtons();
        }

        private void StartGrabMulti(Camera camera)
        {
            camera.SetTriggerMode(TriggerMode.Software);

            LockButtons();
            buttonGrabMulti.Text = "STOP";
            buttonGrabMulti.Enabled = true;

            DeviceManager.Instance().LightCtrlHandler.TurnOn();
            drawBox.DisplayRect = RectangleF.Empty;
            camera.ImageGrabbed += ImageGrabbed;
            camera.GrabMulti();
        }

        private void StopGrab(Camera camera)
        {
            camera.ImageGrabbed -= ImageGrabbed;
            camera.Stop();
            DeviceManager.Instance().LightCtrlHandler.TurnOff();

            if (asyncProcessTask != null && (asyncProcessTask.IsCompleted == false))
            {
                var simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() => asyncProcessTask.Wait());
            }

            buttonGrabMulti.Text = "Grab Multi";
            ReleaseButtons();
        }

        private void ImageGrabbed(Camera camera)
        {
            //if (asyncProcessTask == null || asyncProcessTask.IsCompleted)
            //{
            //    asyncProcessTask?.Dispose();
            //    asyncProcessTask = new Task(new Action(() =>
            //    {
                    try
                    {
                        LogHelper.Debug(LoggerType.Function, "CameraCalibrationForm::ImageGrabbed - asyncProcessTask Start");

                        // Get Grabbed Image
                        var grabbedImage = (Image2D)camera.GetGrabbedImage();
                        if (grabbedImage.IsUseIntPtr())
                        {
                            var image2DD = (Image2D)grabbedImage.Clone();
                            image2DD.ConvertFromDataPtr();
                            grabbedImage = image2DD;
                        }
                        //grabbedImage.SaveImage(@"D:\temp\tt.bmp");

                        if (camera is CameraVirtual)
                        {
                            grabbedImage = (Image2D)grabbedImage.ClipImage(new System.Drawing.Rectangle(0, 0, grabbedImage.Width, 256));
                        }
                        // Draw Chart
                        //draw_Chart_ms(image2D);

                        // Calibration Func
                        lastCalibrationResult = CalibrateFunc(grabbedImage);

                        //// Draw Image and Overlay Figure
                        //if (this.lastCalibrationResult != null && this.lastCalibrationResult.image != null)
                        //{
                        //    Image2D binalImage = this.lastCalibrationResult.binalImage;
                        //    image2D.CopyFrom(binalImage, new System.Drawing.Rectangle(System.Drawing.Point.Empty, binalImage.Size), binalImage.Pitch, this.lastCalibrationResult.calibRect.Location);
                        //}

                        //this.drawBox.FigureGroup.Clear();
                        //this.lastCalibrationResult.AppendFigure(this.drawBox.FigureGroup);

                        //Image image = image2D.ToBitmap();
                        //this.drawBox.UpdateImage(image);
                        //image.Dispose();

                        UpdateResult(grabbedImage);
                        imageGrabbedDone.Set();

                        LogHelper.Debug(LoggerType.Function, "CameraCalibrationForm::ImageGrabbed - asyncProcessTask End");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Debug(LoggerType.Function, $"CameraCalibrationForm::ImageGrabbed - {ex.Message} / {ex.StackTrace}");
                    }
                    finally
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }
            //    }));
            //    asyncProcessTask.Start();
            //}
        }


        private CalibrationResult CalibrateFunc(Image2D image2D)
        {
            CalibrationResult result = curCameraCalibrationPanel.Calibrate(curCameraCalibration, image2D);
            //switch (curCameraCalibration.CalibrationType)
            //{
            //    case CalibrationType.SingleScale:

            //        break;
            //    case CalibrationType.Grid:
            //        
            //        break;
            //    case CalibrationType.Ruler:

            //        break;
            //}

            return result;
        }

        private void ReleaseButtons()
        {
            UpdateBottons(true);

            int numLightType = LightConfig.Instance().NumLightType;
            if (numLightType == 0)
            {
                radioDeviceLightType.Enabled = false;
            }
        }

        private void UpdateBottons(bool v)
        {
            comboBoxCamera.Enabled = v;
            numericUpDownExternalExpose.Enabled = v;
            radioDeviceLightType.Enabled = v;
            radioDeviceLightValue.Enabled = v;
            comboLightType.Enabled = v;
            trackBar1.Enabled = v;
            buttonGrab.Enabled = v;
            buttonGrabMulti.Enabled = v;
            buttonSave.Enabled = v;
            buttonLoad.Enabled = v;
        }

        private void LockButtons()
        {
            UpdateBottons(false);
        }

        private void TurnOnLight()
        {
            if (LightCtrlHandler == null)
            {
                return;
            }

            LightCtrlHandler.TurnOn(lightParam);
            Thread.Sleep(100);
        }

        private void TurnOffLight()
        {
            if (LightCtrlHandler == null)
            {
                return;
            }

            LightCtrlHandler.TurnOff();
        }

        private bool Apply()
        {
            if (curCameraCalibration == null)
            {
                return false;
            }

            if (comboBoxCamera.SelectedIndex < 0)
            {
                return false;
            }

            if (lastCalibrationResult == null)
            {
                return false;
            }

            curCameraCalibration.PelSize = lastCalibrationResult.pelSize;
            return true;
        }

        private bool Save()
        {
            if (curCameraCalibration == null)
            {
                return false;
            }

            if (comboBoxCamera.SelectedIndex < 0)
            {
                return false;
            }

            var camera = (Camera)CameraHandler.GetCamera(comboBoxCamera.SelectedIndex);
            camera.UpdateFovSize(curCameraCalibration.PelSize);

            curCameraCalibration.Save();
            return true;
        }

        private void SetCalibrationGrab(Camera camera)
        {
            /*
            if (camera is CameraGenTL)
            {
                CameraGenTL cameraGenTL = (CameraGenTL)camera;
                cameraGenTL.SetAreaMode();
                cameraGenTL.SetDeviceExposure((float)numericUpDownExternalExpose.Value);
            }*/


            //camera.SetScanMode(ScanMode.Area);
            camera.SetExposureTime((float)numericUpDownExternalExpose.Value * 1000);
        }

        private void ResetCalibrationGrab(Camera camera)
        {
            /*
            if (camera is CameraGenTL)
            {
                CameraGenTL cameraGenTL = (CameraGenTL)camera;
                cameraGenTL.SetLineScanMode();
                //cameraGenTL.SetDeviceExposure((float)numericUpDownExternalExpose.Value);
            }*/
            //camera.SetScanMode(ScanMode.Line);
        }


        private void draw_Histogram(ImageD image)//ms
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraCalibrationForm::draw_Histogram"));

            var image2D = image as Image2D;
            AlgoImage fullImage = ImageBuilder.Build(OperationConfig.Instance().ImagingLibrary, image2D, ImageType.Grey);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(OperationConfig.Instance().ImagingLibrary);

            //1. Revision Histogram
            long[] histo = imageProcessing.Histogram(fullImage);
            RevisionHistogram(histo);


            //2. Revision MTF-Chart 
            int range = 100;
            int count = fullImage.Width / 2 - range / 2;
            float[] fullMFT = new float[fullImage.Width];
            byte[] linedata = new byte[fullImage.Width];
            float[] left = new float[count];
            float[] right = new float[count];
            byte[] temp = new byte[range];

            byte[] imageData = fullImage.GetByte();
            int offset = fullImage.Height / 2 * fullImage.Width;
            for (int i = 0; i < fullImage.Width; i++)
            {
                linedata[i] = imageData[offset + i];
            }

            Array.Clear(fullMFT, 0, fullMFT.Length);
            for (int i = range / 2; i < fullImage.Width - range / 2; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    int index = i - range / 2 + j;
                    temp[j] = linedata[index];
                }
                fullMFT[i] = getMTF(temp);
            }
            //left
            for (int i = 0; i < count; i++)
            {
                left[i] = fullMFT[fullImage.Width / 2 - i - 1];
            }
            //right
            for (int i = 0; i < count; i++)
            {
                right[i] = fullMFT[fullImage.Width / 2 + i];
            }

            RevisionMTF(left, right);

            fullImage.Dispose();
        }

        private float getMTF(byte[] data)
        {
            float fmax = data.Max();
            float fmin = data.Min();
            if (fmax == 0)
            {
                return 0;
            }

            return (fmax - fmin) / (fmax + fmin);
        }

        private delegate void RevisionHistogramDelegate(long[] histo);
        private void RevisionHistogram(long[] histo)
        {
            if (chart_Histogram.InvokeRequired)
            {
                BeginInvoke(new RevisionHistogramDelegate(RevisionHistogram), histo);
                return;
            }

            chart_Histogram.Series[0].Points.Clear();//logarithm은 0또는 음수를 허용하지 않음
            chart_Histogram.Series[1].Points.Clear();
            foreach (long data in histo)
            {
                chart_Histogram.Series[0].Points.Add(data + 1);//logarithm은 0또는 음수를 허용하지 않음
                chart_Histogram.Series[1].Points.Add(data);
            }
        }

        private delegate void RevisionMTFDelegate(float[] left, float[] right);
        private void RevisionMTF(float[] left, float[] right)
        {
            if (chart_MTF.InvokeRequired)
            {
                BeginInvoke(new RevisionMTFDelegate(RevisionMTF), left, right);
                return;
            }
            chart_MTF.Series[0].Points.Clear();
            chart_MTF.Series[1].Points.Clear();
            foreach (float data in left)
            {
                chart_MTF.Series[0].Points.Add(data);
            }
            foreach (float data in right)
            {
                chart_MTF.Series[1].Points.Add(data);
            }
        }

        private void draw_Chart_ms(ImageD imageD)//ms
        {
            LogHelper.Debug(LoggerType.Grab, string.Format("CameraCalibrationForm::draw_Chart_ms"));
            //Image2D image2D = Image2D.ToImage2D(drawBox.Image);
            var image2D = imageD as Image2D;

            AlgorithmStrategy algorithmStrategy = AlgorithmFactory.Instance().GetStrategy(Calibration.TypeName);
            AlgoImage fullImage = ImageBuilder.Build(algorithmStrategy.LibraryType, image2D, ImageType.Grey);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algorithmStrategy.LibraryType);

            //1. Revision Histogram
            long[] histo = imageProcessing.Histogram(fullImage);
            RevisionHistogram(histo);

            //2. Revision MTF-Chart 
            int range = 100;
            int count = fullImage.Width / 2 - range / 2;
            float[] fullMFT = new float[fullImage.Width];
            byte[] linedata = new byte[fullImage.Width];
            float[] left = new float[count];
            float[] right = new float[count];
            byte[] temp = new byte[range];

            byte[] imageData = fullImage.GetByte();
            int offset = fullImage.Height / 2 * fullImage.Width;
            for (int i = 0; i < fullImage.Width; i++)
            {
                linedata[i] = imageData[offset + i];
            }

            Array.Clear(fullMFT, 0, fullMFT.Length);
            for (int i = range / 2; i < fullImage.Width - range / 2; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    int index = i - range / 2 + j;
                    temp[j] = linedata[index];
                }
                fullMFT[i] = getMTF(temp);
            }
            //left
            for (int i = 0; i < count; i++)
            {
                left[i] = fullMFT[fullImage.Width / 2 - i - 1];
            }
            //right
            for (int i = 0; i < count; i++)
            {
                right[i] = fullMFT[fullImage.Width / 2 + i];
            }

            RevisionMTF(left, right);

            fullImage.Dispose();
        }

        // 흔적... 일단 살려주세요
        //private AlgoImage CreateMaskImage(Size referenceSize, System.Drawing.Rectangle measureReagion)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);
        //    AlgoImage maskImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, measureReagion.Width, measureReagion.Height);
        //    imageProcessing.Clear(maskImage, 255);
        //    foreach (PointF skip in this.propRulerSkipRegionList)
        //    {
        //        int ss = (int)Math.Round(referenceSize.Width * skip.X);
        //        int ee = (int)Math.Round(referenceSize.Width * skip.Y);
        //        System.Drawing.Rectangle skipRect = System.Drawing.Rectangle.FromLTRB(ss, measureReagion.Top, ee, measureReagion.Bottom);
        //        skipRect.Intersect(measureReagion);
        //        if (skipRect.Width <= 0)
        //            continue;
        //        skipRect.Offset(-measureReagion.Location.X, -measureReagion.Location.Y);
        //        imageProcessing.Clear(maskImage, skipRect, Color.Black);
        //    }

        //    return maskImage;
        //}

        private void UpdateResult(ImageD grabbedImage)
        {
            UpdateResult(grabbedImage, lastCalibrationResult);
            UpdateChart(grabbedImage);

            curCameraCalibrationPanel.UpdateResult(lastCalibrationResult, -1);

            for (int i = 0; i < lastCalibrationResult.subCalibrationResult.Count; i++)
            {
                CalibrationResult calibrationResult = lastCalibrationResult.subCalibrationResult[i];
                curCameraCalibrationPanel.UpdateResult(calibrationResult, i);
            }
        }

        private void UpdateResult(ImageD grabbedImage, CalibrationResult result)
        {
            // Draw Image and Overlay Figure
            if (grabbedImage != null)
            {
                var image2D = (Image2D)grabbedImage.Clone();
                if (lastCalibrationResult != null && lastCalibrationResult.clipImage != null)
                {
                    Image2D binalImage = lastCalibrationResult.binalImage;
                    try
                    {
                        image2D.CopyFrom(binalImage, new System.Drawing.Rectangle(System.Drawing.Point.Empty, binalImage.Size), binalImage.Pitch, lastCalibrationResult.calibRect.Location);
                    }
                    catch (Exception)
                    {

                    }
                }

                var image = image2D.ToBitmap();
                drawBox.UpdateImage(image);
                //image.Dispose();
                image2D.Dispose();
            }
            drawBox.FigureGroup.Clear();
            lastCalibrationResult.AppendFigure(drawBox.FigureGroup);

            var labelList = new Label[] { label7, label8, label11, label10, label3, label52 };
            SetLabelText(labelList[0], result.avgBrightness < 0 ? "---.-" : result.avgBrightness.ToString("0.0"));
            SetLabelText(labelList[1], result.minBrightness < 0 ? "---.-" : result.minBrightness.ToString("0.0"));
            SetLabelText(labelList[2], result.maxBrightness < 0 ? "---.-" : result.maxBrightness.ToString("0.0"));
            SetLabelText(labelList[3], result.pelSize.Width < 0 ? "---.---" : result.pelSize.Width.ToString("0.000"));
            SetLabelText(labelList[4], result.pelSize.Height < 0 ? "---.---" : result.pelSize.Height.ToString("0.000"));
            SetLabelText(labelList[5], result.focusValue < 0 ? "---.---" : result.focusValue.ToString("0.000"));
        }

        private delegate void SetRLabelTextDelegate(Label label, string message);
        private void SetLabelText(Label label, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new SetRLabelTextDelegate(SetLabelText), label, message);
                return;
            }

            label.Text = message;
        }

        private void UpdateChart(ImageD grabbedImage)
        {
            if (grabbedImage != null)
            {
                draw_Chart_ms(grabbedImage);
            }

            // Histogram

            // MTF
        }
        #endregion


        #region 클래스
        public class ComboBoxItem
        {
            public string text = "";
            public LightParam lightParam = null;
            public int ctrlId = -1;
            public int ctrlCh = -1;
            public ComboBoxItem(string text, int ctrlId, int ctrlCh)
            {
                this.text = text;
                this.ctrlId = ctrlId;
                this.ctrlCh = ctrlCh;
            }
            public ComboBoxItem(string text, LightParam lightParam)
            {
                this.text = text;
                this.lightParam = lightParam;
            }
            public override string ToString()
            {
                return text;
            }
        }
        #endregion
    }
}

