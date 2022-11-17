using DynMvp.Base;
using DynMvp.Component.DepthSystem.DepthViewer;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;

namespace UniEye.Base.UI.InspectionPanel
{
    public partial class SingleStepInspectionPanel : UserControl, IInspectPanel
    {
        private List<ProductResult> inspectionResultList = new List<ProductResult>();
        private int fontSize = 40;

        private DrawBox[] resultView;
        private CameraHandler cameraHandler;
        private List<int> camIdList = new List<int>();

        public SingleStepInspectionPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor Inspection Page");

            InitializeComponent();
            InitResultViewPanel();

            LogHelper.Debug(LoggerType.StartUp, "End Constructor Inspection Page");
        }

        public void InitResultViewPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init Result View Panel");

            int numOfResultView = camIdList.Count();

            resultView = new DrawBox[numOfResultView];

            resultViewPanel.ColumnStyles.Clear();
            resultViewPanel.RowStyles.Clear();

            resultViewPanel.ColumnCount = numOfResultView;
            resultViewPanel.RowCount = 1;


            for (int i = 0; i < resultViewPanel.ColumnCount; i++)
            {
                resultViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / resultViewPanel.ColumnCount));
            }

            for (int i = 0; i < resultViewPanel.RowCount; i++)
            {
                resultViewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / resultViewPanel.RowCount));
            }

            for (int i = 0; i < numOfResultView; i++)
            {
                resultView[i] = new DrawBox();

                resultView[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                resultView[i].Dock = System.Windows.Forms.DockStyle.Fill;
                resultView[i].Location = new System.Drawing.Point(3, 3);
                resultView[i].Name = "targetImage";
                resultView[i].Size = new System.Drawing.Size(409, 523);
                resultView[i].TabIndex = 8;
                resultView[i].TabStop = false;
                resultView[i].Enable = false;
                resultView[i].MeasureMode = true;
                resultView[i].ShowTooltip = true;

                int rowIndex = i / numOfResultView;

                int colIndex = i % numOfResultView;

                resultViewPanel.Controls.Add(resultView[i], colIndex, rowIndex);
            }
        }

        void IInspectPanel.Initialize()
        {
            cameraHandler = DeviceManager.Instance().CameraHandler;

            foreach (Camera camera in cameraHandler)
            {
                camIdList.Add(camera.Index);
            }

            InitResultViewPanel();
        }

        void IInspectPanel.Initialize(int[] camIdArr)
        {
            cameraHandler = DeviceManager.Instance().CameraHandler;

            camIdList.AddRange(camIdArr);

            InitResultViewPanel();
        }

        void IInspectPanel.ClearPanel()
        {
            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPanel - ClearPanel");

            int numOfResultView = camIdList.Count();

            for (int i = 0; i < numOfResultView; i++)
            {
                resultView[i].TempFigureGroup.Clear();
                ImageHelper.Clear(resultView[i].Image, 0);
            }
        }

        private delegate void ProductBeginInspectDelegate(ProductResult productResult);
        public void ProductBeginInspect(ProductResult productResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductBeginInspectDelegate(ProductBeginInspect), productResult);
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ProductBeginInspect");

            for (int i = 0; i < resultView.Count(); i++)
            {
                ImageHelper.Clear(resultView[i].Image, 0);

                var tempFigureGroup = new FigureGroup();

                var inspectionTextFigure = new TextFigure("Inspecting...", new Point(Parent.Width - fontSize, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Orange);


                resultView[i].ShowCenterGuide = UiConfig.Instance().ShowCenterGuide;
                resultView[i].TempFigureGroup = tempFigureGroup;

                resultView[i].Invalidate();
            }
        }

        private delegate void StepEndInspectDelegate(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer);
        public void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke StepEndInspect");
                BeginInvoke(new StepEndInspectDelegate(StepEndInspect), inspectStep, productResult, imageBuffer);
                return;
            }

            ShowResult(inspectStep, productResult, imageBuffer);
        }

        private delegate void ProductInspectedDelegate(ProductResult inspectionResult);
        public void ProductInspected(ProductResult inspectionResult)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Inspection, "Add InspectionResultList");

                inspectionResultList.Add(inspectionResult);

                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke ProductInspected");
                BeginInvoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);

                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ProductInspected");

            if (OperationConfig.Instance().UseDefectReview == true)
            {
                if (inspectionResult.IsGood() == false)
                {
                    var form = new DefectReviewForm();
                    form.DefectReportPanel = UiManager.Instance().CreateDefectReportPanel();
                    form.Initialize(inspectionResult);
                    form.Location = PointToScreen(Location);
                    form.Size = Size;

                    form.ShowDialog(this);
                }
            }
        }

        private void ShowResult(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer)
        {
            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ShowResult");

            int numOfResultView = camIdList.Count();

            for (int viewIndex = 0; viewIndex < numOfResultView; viewIndex++)
            {
                var targetInspectResult = new ProbeResultList();
                productResult.GetCamResult(inspectStep.GetStepName(), camIdList[viewIndex], targetInspectResult, true);

                // 0번 lightType은 반드시 영상을 촬영한다.
                ShowImageResult(targetInspectResult, viewIndex, imageBuffer.GetImage(camIdList[viewIndex], 0));

                if (InspectConfig.Instance().SaveResultFigure)
                {
                    string fileName = Path.Combine(productResult.ResultPath,
                        string.Format(InspectConfig.Instance().ImageNameFormat, camIdList[viewIndex], inspectStep.StepNo, 0, "R"));
                    resultView[viewIndex].SaveImage(fileName);
                }
            }
        }

        private void ShowImageResult(ProbeResultList targetInspectResult, int viewIndex, ImageD grabImage)
        {
            if (grabImage == null)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ShowImageResult");

            resultView[viewIndex].UpdateImage(grabImage.ToBitmap());
            resultView[viewIndex].ZoomFit();

            var tempFigureGroup = new FigureGroup();
            targetInspectResult.AppendResultFigures(tempFigureGroup, ResultImageType.Camera);

            TextFigure overallResultTextFigure = null;

            if (targetInspectResult.Count() == 0)
            {
                overallResultTextFigure = new TextFigure("Not Trained", new Point(Parent.Width - fontSize, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Orange);
            }
            else if (targetInspectResult.IsGood())
            {
                overallResultTextFigure = new TextFigure("Good", new Point(Parent.Width - fontSize + 800, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.LightGreen);
            }
            else if (targetInspectResult.IsOverkill())
            {
                overallResultTextFigure = new TextFigure("Overkill", new Point(Parent.Width - fontSize + 450, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Yellow);
            }
            else
            {
                overallResultTextFigure = new TextFigure("NG", new Point(Parent.Width - fontSize + 800, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Red);
            }
            //tempFigureGroup.AddFigure(overallResultTextFigure);

            LogHelper.Debug(LoggerType.Inspection, string.Format("Result[{0}] image Text is \"{1}\"", viewIndex, overallResultTextFigure.Text));

            if (UiConfig.Instance().ShowScore)
            {
                foreach (ProbeResult probeResult in targetInspectResult)
                {
                    Probe probe = probeResult.Probe;
                    ResultValue probeResultValue = probeResult.GetResultValue("Value");
                    string valueStr = probeResultValue.Value.ToString();

                    var penColor = new Color();
                    if (probeResult.IsGood())
                    {
                        penColor = Color.LightGreen;
                    }
                    else if (probeResult.IsOverkill())
                    {
                        penColor = Color.Yellow;
                    }
                    else
                    {
                        penColor = Color.Red;
                    }

                    var tempTextFigure = new TextFigure(valueStr, new Point((int)probe.BaseRegion.X, (int)probe.BaseRegion.Y - 70),
                                                                    new Font(FontFamily.GenericSansSerif, 40, FontStyle.Bold), penColor);
                    tempFigureGroup.AddFigure(tempTextFigure);
                }
            }

            resultView[viewIndex].ShowCenterGuide = UiConfig.Instance().ShowCenterGuide;
            resultView[viewIndex].TempFigureGroup = tempFigureGroup;

            resultView[viewIndex].Invalidate();
            resultView[viewIndex].Update();
        }

        private void resultView_mouseDoubleClicked(DrawBox senderView)
        {
            var form = new DepthViewForm();
            form.Image3d = senderView.Image3d;
            form.ShowDialog();
        }

        bool IInspectEventListener.EnterWaitInspection() { return true; }
        void IInspectEventListener.ExitWaitInspection() { }
        void IInspectEventListener.ProductEndInspect(ProductResult productResult) { }
        void IInspectEventListener.StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult) { }
        void IInspectEventListener.StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }
        void IInspectEventListener.TargetBeginInspect(Target target) { }
        void IInspectEventListener.TargetEndInspect(Target target, ProbeResultList probeResultList) { }
        void IInspectEventListener.TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList) { }
        void IInspectEventListener.ProbeBeginInspect() { }
        void IInspectEventListener.ProbeEndInspect() { }
    }
}
