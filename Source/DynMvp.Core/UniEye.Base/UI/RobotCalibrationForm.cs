using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class RobotCalibrationForm : Form
    {
        private DrawBox cameraBox;
        private DrawBox robotMapBox;
        private TeachBox teachBox;
        public RobotMapper RobotAligner { get; set; } = new RobotMapper();

        private Thread workingThread;

        // Calibration cameraCalibrarion;

        private bool stopFlag = false;
        private bool onCalibrating = false;
        private bool darkBlob = true;
        private BlobChecker blobChecker;
        private BlobCheckerParam blobCheckerParam;
        private ImageD image;

        public RobotCalibrationForm()
        {
            InitializeComponent();

            cameraBox = new DrawBox();
            robotMapBox = new DrawBox();
            teachBox = new TeachBox();
        }

        public void Initialize()
        {
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(0);

            cameraBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            cameraBox.Dock = System.Windows.Forms.DockStyle.Fill;
            cameraBox.Location = new System.Drawing.Point(3, 3);
            cameraBox.Name = "targetImage";
            cameraBox.Size = new System.Drawing.Size(409, 523);
            cameraBox.TabIndex = 8;
            cameraBox.TabStop = false;
            cameraBox.Enable = false;
            cameraBox.MeasureMode = true;
            tabPageCamera.Controls.Add(cameraBox);

            cameraBox.Enable = false;
            cameraBox.ShowCenterGuide = true;
            cameraBox.SetCamera(camera);
            cameraBox.LockLiveUpdate = false;
            cameraBox.Calibration = SystemManager.Instance().CameraCalibrationList[0];
            cameraBox.ImageProcessing = cameraBox_ImageProcessing;
            blobChecker = new BlobChecker();
            blobCheckerParam = new BlobCheckerParam();
            UpdateFovRect();
            //camera.GrabMulti();

            LightParamSet lightParamSet = LightConfig.Instance().LightParamSet;
            foreach (LightParam lightParam in lightParamSet)
            {
                comboBoxLightType.Items.Add(lightParam.Name);
            }

            if (comboBoxLightType.Items.Count > 1)
            {
                comboBoxLightType.SelectedIndex = 1;
            }

            numericUpDownExpose.Value = (decimal)(camera.GetExposureTime() / 1000.0f);

            RobotAligner = robotStage.RobotAligner;
            robotStage.OnEndMove += robotStage_RobotMoved;

            UpdateGrid();

            var joystick = new Joystick2AxisControl();
            joystick.Initialize(robotStage);
            joystick.SetMovableCheckDelegate(joystick_MovableCheck);
            joystickPanel.Controls.Add(joystick);
        }

        private Bitmap cameraBox_ImageProcessing(Bitmap bitmap)
        {
            var image2D = Image2D.ToImage2D(bitmap);
            image2D.SaveImage(@"d:\tt.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            ImagingLibrary imagingLibrary = OperationConfig.Instance().ImagingLibrary;
            AlgoImage algoImage = ImageBuilder.GetInstance(imagingLibrary).Build(image2D, ImageType.Grey);

            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(imagingLibrary);
            imageProcessing.Binarize(algoImage, Convert.ToInt32(numericUpDownThreshold.Value), darkBlob);

            image2D = (Image2D)algoImage.ToImageD();
            image2D.SaveImage(@"d:\tt2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            return image2D.ToBitmap();
        }

        private bool joystick_MovableCheck()
        {
            return !IsCalibrating();
        }

        private bool IsCalibrating()
        {
            return onCalibrating == true;
        }

        private void robotStage_RobotMoved(AxisPosition axisPosition)
        {
            if (IsCalibrating() == true)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new RobotEventDelegate(robotStage_RobotMoved), axisPosition);
                return;
            }

            if (IsCalibrating() == false)
            {
                Thread.Sleep(500);
            }
        }

        private void UpdateGrid()
        {
            if (RobotAligner.RefPoints == null)
            {
                return;
            }

            for (int y = 0; y < RobotAligner.RefPoints.GetLength(1); y++)
            {
                for (int x = 0; x < RobotAligner.RefPoints.GetLength(0); x++)
                {
                    float len = (float)Math.Sqrt(Math.Pow(RobotAligner.Offset[x, y].X, 2) + Math.Pow(RobotAligner.Offset[x, y].Y, 2));
                    AddDataGrid(string.Format("P{0}-{1}", x + 1, y + 1),
                        string.Format("{0},{1}", RobotAligner.RefPoints[x, y].X, RobotAligner.RefPoints[x, y].Y),
                        string.Format("{0},{1}", RobotAligner.RealPoints[x, y].X, RobotAligner.RealPoints[x, y].Y),
                        RobotAligner.Offset[x, y].X.ToString(), RobotAligner.Offset[x, y].Y.ToString(),
                       string.Format("{0}", Math.Sqrt(Math.Pow(RobotAligner.Offset[x, y].X, 2) + Math.Pow(RobotAligner.Offset[x, y].X, 2))));
                }
            }
        }

        private void UpdateFovRect()
        {
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(0);

            //AxisPosition axisPosition = robotStage.GetActualPos();
            //cameraBox.DisplayRect = DrawingHelper.FromCenterSize(axisPosition.ToPointF(), camera.FovSize);

            //RectangleF circleRect = DrawingHelper.FromCenterSize(axisPosition.ToPointF(), new SizeF((float)circleDiameter.Value, (float)circleDiameter.Value));

            //FigureGroup tempFigureGroup = new FigureGroup();
            //tempFigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Red)));
            //cameraBox.FigureGroup = tempFigureGroup;

            cameraBox.Invalidate();
        }

        private void CalibrationProc()
        {
            onCalibrating = true;

            LogHelper.Debug(LoggerType.Operation, "Start CalibrationProc.");

            AxisHandler robotStage = DeviceManager.Instance().RobotStage;

            Axis xAxis = robotStage.GetAxis("X-Axis");
            Axis yAxis = robotStage.GetAxis("Y-Axis");

            float xStepUm = Convert.ToSingle(txtXStep.Text);
            float yStepUm = Convert.ToSingle(txtYStep.Text);

            int countX = Convert.ToInt32(numPosX.Value);
            int countY = Convert.ToInt32(numPosY.Value);
            var refPoints = new PointF[countX, countY];
            var offsets = new PointF[countX, countY];

            float yPos = Convert.ToSingle(txtStartY.Text);
            float xPos = Convert.ToSingle(txtStartX.Text);
            var startPoint = new PointF(xPos, yPos);

            string tempPath = string.Format(@"{0}\RobotCalibration\", BaseConfig.Instance().TempPath);
            if (Directory.Exists(tempPath) == false)
            {
                Directory.CreateDirectory(tempPath);
            }

            RobotAligner.Clear();

            int[] version = StringHelper.GetVersionNo(OperationConfig.Instance().SystemVersion);

            Calibration cameraCalibration = SystemManager.Instance().CameraCalibrationList[0];
            PointF markSize = cameraCalibration.WorldToPixel(new PointF((float)circleDiameter.Value, (float)circleDiameter.Value));
            //markSize = (new PointF((float)circleDiameter.Value, (float)circleDiameter.Value));

            try
            {
                for (int y = 0; y < countY; y++)
                {
                    if (stopFlag)
                    {
                        break;
                    }

                    for (int x = 0; x < countX; x++)
                    {
                        if (stopFlag)
                        {
                            break;
                        }

                        int realX = x;
                        //if ((y % 2) == 1)
                        //{
                        //    realX = countX - x - 1;
                        //}

                        if (realX == 0 && y == 0)
                        {
                            int tryCount = 0;
                            while (true)
                            {
                                robotStage.Move(new AxisPosition(new float[] { xPos, yPos }));
                                robotStage.WaitMoveDone();
                                Thread.Sleep(100);

                                ErrorManager.Instance().ThrowIfAlarm();

                                AxisPosition axisPosition = robotStage.GetCommandPos();
                                Grab();

                                image.SaveImage(string.Format(@"{0}\{1}_{2}_{3}.bmp", tempPath, realX, y, tryCount), System.Drawing.Imaging.ImageFormat.Bmp);

                                // Found Point의 offset. [Pixel]
                                if (FindEdgeCenter(image, out PointF foundOffset) == false)
                                {
                                    MessageForm.Show(this, StringManager.GetString("Fail to Found Marker"));
                                    stopFlag = true;
                                    break;
                                }

                                // Found Point의 offset.
                                PointF worldPos = cameraBox.Calibration.PixelToWorld(foundOffset);
                                var axisPositionOffset = new AxisPosition(new float[] { worldPos.X, worldPos.Y });

                                xPos = axisPosition.Position[0] - axisPositionOffset.Position[0];
                                //xPos = axisPosition.Position[0] + axisPositionOffset.Position[0];

                                if (version[0] == 1)
                                {
                                    yPos = axisPosition.Position[1] - axisPositionOffset.Position[1];
                                }
                                else if (version[0] == 2)
                                {
                                    yPos = axisPosition.Position[1] + axisPositionOffset.Position[1];
                                }

                                //yPos = axisPosition.Position[1] - axisPositionOffset.Position[1];

                                const int dis = 5;
                                if ((Math.Abs(axisPositionOffset[0]) < dis && Math.Abs(axisPositionOffset[1]) < dis))
                                {
                                    break;
                                }

                                if (stopFlag)
                                {
                                    break;
                                }

                                tryCount++;
                            }

                            AxisPosition alignedPosition = robotStage.GetCommandPos();
                            startPoint = refPoints[0, 0] = new PointF(alignedPosition[0], alignedPosition[1]);
                            AddDataGrid(string.Format("P{0}-{1}", realX + 1, y + 1), string.Format("{0},{1}", startPoint.X, startPoint.Y)
                                , string.Format("{0},{1}", startPoint.X, startPoint.Y)
                                , "0", "0", "0");
                        }
                        else
                        {
                            xPos = startPoint.X + xStepUm * (-realX);
                            //xPos = startPoint.X + xStepUm * (+realX);

                            if (version[0] == 1)
                            {
                                yPos = startPoint.Y + yStepUm * (-y);
                            }
                            else if (version[0] == 2)
                            {
                                yPos = startPoint.Y + yStepUm * (+y);
                            }

                            //yPos = startPoint.Y + yStepUm * (+y);

                            var axisPosition = new AxisPosition(new float[] { xPos, yPos });
                            robotStage.Move(axisPosition);
                            robotStage.WaitMoveDone();
                            Thread.Sleep(100);

                            ErrorManager.Instance().ThrowIfAlarm();

                            Grab();
                            image.SaveImage(string.Format(@"{0}\{1}_{2}.bmp", tempPath, realX, y), System.Drawing.Imaging.ImageFormat.Bmp);

                            // Found Point의 offset. [Pixel]
                            if (FindEdgeCenter(image, out PointF foundOffset) == false)
                            {
                                MessageForm.Show(this, StringManager.GetString("Fail to Found Marker"));
                                stopFlag = true;
                                break;
                            }

                            // Found Point의 offset. [um]
                            PointF worldPos = cameraBox.Calibration.PixelToWorld(foundOffset);
                            var axisPositionOffset = new AxisPosition(new float[] { worldPos.X, worldPos.Y });

                            // 찾은 Point를 Figure로 그림. DrawBox의 Figure Unitd은 pixel 좌표.
                            var imageCenter = (new PointF(image.Width / 2, image.Height / 2));
                            var offsetPt = new PointF(imageCenter.X - foundOffset.X, imageCenter.Y - foundOffset.Y);
                            RectangleF circleRect = DrawingHelper.FromCenterSize(offsetPt, new SizeF(markSize));
                            cameraBox.FigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Yellow, 2)));
                            cameraBox.Invalidate();

                            var offset = new PointF(-axisPositionOffset.Position[0], axisPositionOffset.Position[1]);
                            offsets[realX, y] = offset;
                            refPoints[realX, y] = new PointF(xPos, yPos);
                            AddDataGrid(string.Format("P{0}-{1}", realX + 1, y + 1),
                                string.Format("{0},{1}", xPos, yPos),
                                string.Format("{0},{1}", xPos + offset.X, yPos + offset.Y),
                                offset.X.ToString(), offset.Y.ToString(),
                                string.Format("{0}", Math.Sqrt(Math.Pow(offset.X, 2) + Math.Pow(offset.Y, 2))));
                        }
                    }
                }
            }
            catch (AlarmException)
            {
                stopFlag = true;
            }
            finally
            {
                // 임의 정지시 저장 안 함
                if (!stopFlag)
                {
                    RobotAligner.Calculate(refPoints, offsets);
                    RobotAligner.Save(BaseConfig.Instance().ConfigPath);
                }

                onCalibrating = false;
            }
        }

        private delegate void AddDataGridDelegate(string pointIdx, string refPos, string realPos, string offsetX, string offsetY, string length);
        private void AddDataGrid(string pointIdx, string refPos, string realPos, string offsetX, string offsetY, string length)
        {
            if (InvokeRequired)
            {
                Invoke(new AddDataGridDelegate(AddDataGrid), pointIdx, refPos, realPos, offsetX, offsetY, length);
                return;
            }

            int idx = dataGridView1.Rows.Add(pointIdx, refPos, realPos, offsetX, offsetY, length);
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private bool FindEdgeCenter(ImageD image, out PointF conterPoint)
        {
            string tempPath = string.Format(@"{0}\RobotCalibration\", BaseConfig.Instance().TempPath);
            var debugContext = new DebugContext(true, tempPath);

            conterPoint = new PointF(0, 0);

            var position = new AxisPosition();
            var grabImage = (Image2D)image;


            AlgoImage clipImage = ImageBuilder.Build(BlobChecker.TypeName, grabImage, ImageType.Grey, ImageBandType.Luminance);
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(clipImage);

            imageProcessing.Binarize(clipImage);

            if (darkBlob)
            {
                imageProcessing.Not(clipImage, clipImage);
            }

            clipImage.Save("clipImage.bmp", debugContext);

            var blobParam = new BlobParam();
            blobParam.SelectCenterPt = true;
            //blobParam.SelectBoundingRect = false;

            BlobRectList blobRectList = imageProcessing.Blob(clipImage, blobParam);
            BlobRect blobRect = blobRectList.GetMaxAreaBlob();
            if (blobRect == null)
            {
                return false;
            }
            //PointF imageCenterPoint = new PointF(imageCenterPoint)
            //float x = DrawingHelper.CenterPoint(grabImage.Roi).X + blobRect.CenterPt.X;
            //float y = DrawingHelper.CenterPoint(grabImage.Roi).Y + blobRect.CenterPt.Y;

            float x = (grabImage.Width / 2) - blobRect.CenterPt.X;
            float y = (grabImage.Height / 2) - blobRect.CenterPt.Y;
            clipImage.Dispose();

            conterPoint = new PointF(x, y);
            return true;
        }

        private void UpdateMapFigure()
        {

        }

        private void Inspect()
        {
            //ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(cameraIndex);

            //teachBox.Inspect(deviceImageSet, false, null, null, null);

            //UpdateImageFigure();

            //InspectionResult lastSelectedResult = null;

            //if (lastInspectionResult != null)
            //{
            //    lastSelectedResult = new InspectionResult();
            //    foreach (ITeachObject teachObject in teachHandlerProbe.SelectedObjs)
            //    {
            //        Probe probe = teachObject as Probe;
            //        if (probe != null)
            //            lastSelectedResult.AddProbeResult(lastInspectionResult.GetProbeResult(probe));
            //    }
            //}

            //tryInspectionResultView.AddResult(teachBox.InspectionResultSelected, lastSelectedResult);
        }


        private void FindCircle()
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (IsCalibrating())
            {
                return;
            }

            if (MessageBox.Show("the old data will be removed. continue?", "UniEye", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            stopFlag = false;
            dataGridView1.Rows.Clear();

            workingThread = new Thread(new ThreadStart(CalibrationProc));
            workingThread.IsBackground = true;
            workingThread.Start();
        }

        private void UpdateImageFigure()
        {
            teachBox.UpdateFigure();
        }

        private void RobotCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (workingThread != null)
            {
                workingThread.Abort();
                workingThread = null;
            }

            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            robotStage.OnEndMove -= robotStage_RobotMoved;

            Close();
        }

        private void buttonTestGrab_Click(object sender, EventArgs e)
        {
            if (IsCalibrating())
            {
                return;
            }

            stopFlag = false;

            dataGridView1.Rows.Clear();

            workingThread = new Thread(new ThreadStart(TestProc));
            workingThread.IsBackground = true;
            workingThread.Start();
        }

        private void TestProc()
        {
            onCalibrating = true;

            LogHelper.Debug(LoggerType.Operation, "Start Test.");

            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            Axis xAxis = robotStage.GetAxis("X-Axis");
            Axis yAxis = robotStage.GetAxis("Y-Axis");

            float xStepUm = Convert.ToSingle(txtXStep.Text);
            float yStepUm = Convert.ToSingle(txtYStep.Text);

            int countX = Convert.ToInt32(numPosX.Value);
            int countY = Convert.ToInt32(numPosY.Value);

            float yPos = Convert.ToSingle(txtStartY.Text);
            float xPos = Convert.ToSingle(txtStartX.Text);
            var startPoint = new PointF(xPos, yPos);

            int[] version = StringHelper.GetVersionNo(OperationConfig.Instance().SystemVersion);
            Calibration cameraCalibration = SystemManager.Instance().CameraCalibrationList[0];
            PointF markSize = cameraCalibration.WorldToPixel(new PointF((float)circleDiameter.Value, (float)circleDiameter.Value));

            for (int y = 0; y < countY; y++)
            {
                if (stopFlag)
                {
                    break;
                }

                for (int x = 0; x < countX; x++)
                {
                    if (stopFlag)
                    {
                        break;
                    }

                    int realX = x;
                    //if ((y % 2) == 1)
                    //{
                    //    realX = countX - x - 1;
                    //}

                    if (realX == 0 && y == 0)
                    {
                        while (true)
                        {
                            robotStage.Move(new AxisPosition(new float[] { xPos, yPos }));
                            robotStage.WaitMoveDone();
                            Thread.Sleep(100);

                            AxisPosition axisPosition = robotStage.GetCommandPos();
                            Grab();

                            // Found Point의 offset. [Pixel]
                            if (FindEdgeCenter(image, out PointF foundOffset) == false)
                            {
                                MessageForm.Show(this, StringManager.GetString("Fail to Found Marker"));
                                stopFlag = true;
                                break;
                            }

                            // Found Point의 offset. [um]
                            PointF worldPos = cameraBox.Calibration.PixelToWorld(foundOffset);
                            var axisPositionOffset = new AxisPosition(new float[] { worldPos.X, worldPos.Y });

                            // 찾은 Point를 Figure로 그림. DrawBox의 Figure Unit이 um 인가??
                            var centerPt = new PointF(-worldPos.X, -worldPos.Y);
                            RectangleF circleRect = DrawingHelper.FromCenterSize(foundOffset, new SizeF(markSize));
                            cameraBox.FigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Yellow, 2)));
                            cameraBox.Invalidate();

                            xPos = axisPosition.Position[0] - axisPositionOffset.Position[0];
                            if (version[0] == 1)
                            {
                                yPos = axisPosition.Position[1] - axisPositionOffset.Position[1];
                            }
                            else if (version[0] == 2)
                            {
                                yPos = axisPosition.Position[1] + axisPositionOffset.Position[1];
                            }

                            const int dis = 10;
                            if ((Math.Abs(axisPositionOffset[0]) < dis && Math.Abs(axisPositionOffset[1]) < dis))
                            {
                                break;
                            }

                            if (stopFlag)
                            {
                                return;
                            }
                        }

                        AxisPosition alignedPosition = robotStage.GetCommandPos();
                        startPoint = new PointF(alignedPosition[0], alignedPosition[1]);
                        AddDataGrid(string.Format("P{0}-{1}", realX + 1, y + 1),
                            string.Format("{0},{1}", startPoint.X, startPoint.Y),
                            string.Format("{0},{1}", startPoint.X, startPoint.Y), "0", "0", "0");
                    }
                    else
                    {
                        xPos = startPoint.X + xStepUm * (-realX);
                        if (version[0] == 1)
                        {
                            yPos = startPoint.Y + yStepUm * (-y);
                        }
                        else if (version[0] == 2)
                        {
                            yPos = startPoint.Y + yStepUm * (+y);
                        }

                        var axisPosition = new AxisPosition(new float[] { xPos, yPos });
                        robotStage.Move(axisPosition);
                        robotStage.WaitMoveDone();
                        Thread.Sleep(100);

                        Grab();

                        // Found Point의 offset. [Pixel]
                        if (FindEdgeCenter(image, out PointF foundOffset) == false)
                        {
                            MessageForm.Show(this, StringManager.GetString("Fail to Found Marker"));
                            stopFlag = true;
                            break;
                        }

                        // Found Point의 offset. [um]
                        PointF worldPos = cameraBox.Calibration.PixelToWorld(foundOffset);
                        var axisPositionOffset = new AxisPosition(new float[] { worldPos.X, worldPos.Y });

                        // 찾은 Point를 Figure로 그림.
                        var centerPt = new PointF(-worldPos.X, -worldPos.Y);
                        RectangleF circleRect = DrawingHelper.FromCenterSize(foundOffset, new SizeF(markSize));
                        cameraBox.FigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Yellow, 2)));
                        cameraBox.Invalidate();

                        var offset = new PointF(-axisPositionOffset.Position[0], axisPositionOffset.Position[1]);
                        //offsets[realX, y] = offset;
                        //refPoints[realX, y] = new PointF(xPos, yPos);
                        AddDataGrid(string.Format("P{0}-{1}", realX + 1, y + 1),
                            string.Format("{0},{1}", xPos, yPos),
                            string.Format("{0},{1}", xPos + offset.X, yPos + offset.Y),
                            offset.X.ToString(), offset.Y.ToString(),
                            string.Format("{0}", Math.Sqrt(Math.Pow(offset.X, 2) + Math.Pow(offset.Y, 2))));
                    }
                }
            }

            onCalibrating = false;
        }

        public void Grab()
        {
            //AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(0);

            //LightParam lightParam = LightSettings.Instance().LightParamSet.LightParamList[lightParamSimpleForm.LightTypeIndex];

            int numLight = DeviceManager.Instance().LightCtrlHandler.NumLight;

            var lightParam = new LightParam(0, "Grab", numLight);

            for (int i = 0; i < numLight; i++)
            {
                lightParam.LightValue.Value[i] = Convert.ToInt32(dataGridViewLightVlaue.Rows[i].Cells[1].Value.ToString());
            }

            int exposureTimeUs = Convert.ToInt32(numericUpDownExpose.Value) * 1000;
            camera.SetExposureTime(exposureTimeUs);

            ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
            image = imageAcquisition.Acquire(0, lightParam);
            image.SaveImage(@"D:\ttt.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            Calibration cameraCalibration = SystemManager.Instance().CameraCalibrationList[0];
            PointF markSize = cameraCalibration.WorldToPixel(new PointF((float)circleDiameter.Value, (float)circleDiameter.Value));
            var centerPt = (new PointF(image.Width / 2, image.Height / 2));

            //RectangleF circleRect = DrawingHelper.FromCenterSize(PointF.Empty, new SizeF(markSize));
            RectangleF circleRect = DrawingHelper.FromCenterSize(centerPt, new SizeF(markSize));

            CoordTransformer coordTransformer = cameraBox.GetCoordTransformer();

            cameraBox.FigureGroup.Clear();
            cameraBox.FigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Red, 2)));
            //cameraBox.FigureGroup.AddFigure(new EllipseFigure(new RectangleF(100,100,500,500), new Pen(Color.Black, 2)));
            //cameraBox.Update();
            cameraBox.ZoomFit();

            //imageAcquisition.AcquireCalibation(0, 0, machine.LightCtrlHandler);
            //ImageD image = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(0, 0).Image;
            //camera.UpdateImage(image);

        }

        private void applyLightButton_Click(object sender, EventArgs e)
        {
            Grab();
        }

        private void buttonOrigin_Click(object sender, EventArgs e)
        {
            if (IsCalibrating())
            {
                return;
            }

            onCalibrating = true;

            AxisHandler robotStage = DeviceManager.Instance().RobotStage;

            var cancellationTokenSource = new CancellationTokenSource();
            var form = new SimpleProgressForm("Origin...");

            form.Show(new Action(() => robotStage.HomeMove(cancellationTokenSource.Token)), cancellationTokenSource);

            onCalibrating = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (IsCalibrating())
            {
                return;
            }

            if (dataGridView1.SelectedCells.Count == 0)
            {
                return;
            }

            if (Visible == false)
            {
                return;
            }

            int rowId = dataGridView1.SelectedCells[0].RowIndex;
            int colId = dataGridView1.SelectedCells[0].ColumnIndex;
            string refPositionStr = (string)dataGridView1.Rows[rowId].Cells[1].Value;
            string[] split = refPositionStr.Split(new char[] { ',' });
            var refPosition = new PointF(float.Parse(split[0]), float.Parse(split[1]));

            // 아래를 살리면 Align을 두 번 함...
            //if (colId == 2)
            //{
            //    refPosition = robotAligner.Align(refPosition);
            //}
            //AxisPosition tt = robotStage.GetActualPos();
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            robotStage.Move(new AxisPosition(refPosition.X, refPosition.Y));
            //Grab();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            RobotAligner.Save(BaseConfig.Instance().ConfigPath);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopFlag = true;
        }

        private void buttonUpdateStartPos_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            var curPos = robotStage.GetCommandPos().ToPointF();

            txtStartX.Text = curPos.X.ToString();
            txtStartY.Text = curPos.Y.ToString();
        }

        private void buttonMove_Click(object sender, EventArgs e)
        {
            if (IsCalibrating())
            {
                return;
            }

            joystickPanel.Enabled = false;
            AxisHandler robotStage = DeviceManager.Instance().RobotStage;
            float xPos = float.Parse(txtStartX.Text);
            float yPos = float.Parse(txtStartY.Text);
            robotStage.Move(new AxisPosition(new float[] { xPos, yPos }));
            joystickPanel.Enabled = true;
        }

        private void buttonExportTestData_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var dataStr = new StringBuilder();
                dataStr.Append("Point, Ref.Position X, Ref.Position Y, Real Position X, Real Position Y, Offset X, Offset Y, Length");
                dataStr.AppendLine();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                    {
                        dataStr.Append(dataGridView1.Rows[i].Cells[j].Value + ", ");
                    }

                    dataStr.AppendLine();
                }

                File.WriteAllText(dlg.FileName, dataStr.ToString());
            }
        }

        private void RobotCalibrationForm_Load(object sender, EventArgs e)
        {
            panelLight.Hide();

            circleDiameter.Minimum = 1;
            circleDiameter.Maximum = int.MaxValue;
            circleDiameter.Value = 1000;

            cmbJigType.SelectedIndex = 0;

        }

        private void cmbJigType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbJigType.SelectedIndex == 0)
            {
                txtStartX.Text = "-5634";
                txtStartY.Text = "-5342";
                txtXStep.Text = "20000";
                txtYStep.Text = "20000";
                numPosX.Value = 19;
                numPosY.Value = 20;
                darkBlob = false;
            }
            else if (cmbJigType.SelectedIndex == 1)
            {
                txtStartX.Text = "-13708.22";
                txtStartY.Text = "15741.61";
                txtXStep.Text = "20000";
                txtYStep.Text = "20000";
                numPosX.Value = 25;
                numPosY.Value = 26;
                darkBlob = true;

            }
        }

        private void buttonLight_Click(object sender, EventArgs e)
        {
            if (panelLight.Visible == true)
            {
                panelLight.Hide();
            }
            else
            {
                panelLight.Show();
            }
        }

        private void lightParamPanel_LightTypeChanged()
        {
        }

        private void comboBoxLightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewLightVlaue.Rows.Clear();

            int selType = comboBoxLightType.SelectedIndex;
            var lightConfig = LightConfig.Instance();
            LightParam lightParam = lightConfig.LightParamSet[selType];

            for (int i = 0; i < lightParam.LightValue.NumLight; i++)
            {
                dataGridViewLightVlaue.Rows.Add(lightConfig.LightDeviceNameList[i], lightParam.LightValue.Value[i]);
            }
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {


            Grab();
        }
    }
}
