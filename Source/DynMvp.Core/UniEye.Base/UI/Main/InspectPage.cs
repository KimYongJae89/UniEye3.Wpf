using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;

namespace UniEye.Base.UI.Main
{
    public partial class InspectPage : UserControl, IMainTabPage, IInspectEventListener, IModelEventListener
    {
        private List<ProductResult> inspectResultList = new List<ProductResult>();

        public IInspectPanel inspectPanel = null;

        private object drawingLock = new object();

        private string[] offlineImagePathList = null;

        public TabKey TabKey => TabKey.Inspect;

        public string TabName => "Inspect";

        public Bitmap TabIcon => Properties.Resources.inspection_gray_36;

        public Bitmap TabSelectedIcon => Properties.Resources.inspection_white_36;

        public Color TabSelectedColor => Color.FromArgb(248, 90, 50);

        public bool IsAdminPage => false;

        public Uri Uri => throw new NotImplementedException();

        public InspectPage()
        {
            InitializeComponent();
        }

        public void ChangeCaption()
        {
            labelModelName.Text = StringManager.GetString(labelModelName.Text);
            labelModuleId.Text = StringManager.GetString(labelModuleId.Text);
            labelInspTime.Text = StringManager.GetString(labelInspTime.Text);
            labelTotal.Text = StringManager.GetString(labelTotal.Text);
            labelAccept.Text = StringManager.GetString(labelAccept.Text);
            labelDefect.Text = StringManager.GetString(labelDefect.Text);
            buttonResetCount.Text = StringManager.GetString(buttonResetCount.Text);
        }

        public void Initialize()
        {
            inspectPanel = UiManager.Instance().CreateInspectionPanel();

            viewContainer.Controls.Add((UserControl)inspectPanel);
            ((UserControl)inspectPanel).Dock = DockStyle.Fill;
            inspectPanel.Initialize();
        }

        public void OnIdle()
        {

        }

        private void ShowCount()
        {

        }

        public void StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new StepBeginInspectDelegate(StepBeginInspect), inspectStep, productResult, imageBuffer);
                return;
            }

            inspectPanel.StepBeginInspect(inspectStep, productResult, imageBuffer);
        }

        public void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new StepEndInspectDelegate(StepEndInspect), inspectStep, productResult, imageBuffer);
                return;
            }

            inspectPanel.StepEndInspect(inspectStep, productResult, imageBuffer);
        }

        public void TargetEndInspect(Target target, ProbeResultList probeResultList)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TargetEndInspectDelegate(TargetEndInspect), target, probeResultList);
                return;
            }

            inspectPanel.TargetEndInspect(target, probeResultList);
        }

        private delegate void ProductBeginInspectDelegate(ProductResult productResult);
        public void ProductBeginInspect(ProductResult productResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductBeginInspectDelegate(ProductBeginInspect), productResult);
                return;
            }

            inspectNo.Text = productResult.InspectionNo;

            inspectPanel.ProductBeginInspect(productResult);
        }

        private void UpdateInspectResult(ProductResult productResult)
        {
            switch (productResult.Judgment)
            {
                default:
                case Judgment.OK:
                    UpdateStatusLabel("Good", Color.Black, Color.LimeGreen);
                    break;
                case Judgment.Overkill:
                    UpdateStatusLabel("Overkill", Color.Black, Color.Yellow);
                    break;
                case Judgment.NG:
                    UpdateStatusLabel("NG", Color.White, Color.Red);
                    break;
            }
        }

        private delegate void ProductInspectedDelegate(ProductResult productResult);
        public void ProductInspected(ProductResult productResult)
        {
            productResult.InspectEndTime = DateTime.Now;

            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Inspection, "Add InspectionResultList");

                inspectResultList.Add(productResult);

                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke ProductInspected");
                Invoke(new ProductInspectedDelegate(ProductInspected), productResult);

                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "Product Insepected");

            inspectPanel.ProductInspected(productResult);

            UpdateProductionInfo(productResult);

            buttonResetCount.Enabled = true;

            SaveCaptureResult(productResult.ResultPath);

            if (inspectResultList.IndexOf(productResult) > -1)
            {
                inspectResultList.Remove(productResult);
            }
        }

        private delegate void ProductEndInspectDelegate(ProductResult productResult);
        public void ProductEndInspect(ProductResult productResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ProductEndInspectDelegate(ProductEndInspect), productResult);
                return;
            }

            inspectPanel.ProductEndInspect(productResult);
        }

        private delegate void UpdateProductionInfoDelegate(ProductResult inspectionResult);
        private void UpdateProductionInfo(ProductResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateProductionInfoDelegate(UpdateProductionInfo), inspectionResult);
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "InspectionPage - UpdateProductionInfo");

            TimeSpan inspectionProcessTime = inspectionResult.InspectTimeSpan;
            inspTime.Text = string.Format("{0:00}:{1:00}.{2:000}", inspectionProcessTime.Minutes, inspectionProcessTime.Seconds, inspectionProcessTime.Milliseconds);
            if (inspectionResult.InspectionNo != null)
            {
                inspectNo.Text = inspectionResult.ModuleId;
            }

            Production production = ModelManager.Instance().CurrentModel.Production;

            if (inspectionResult.IsGood() == true)
            {
                production.ProduceGood();
            }
            else
            {
                production.ProduceNG();
            }

            productionNg.Text = production.Ng.ToString();
            productionGood.Text = production.Good.ToString();
            productionTotal.Text = production.Total.ToString();

            ModelManager.Instance().CurrentStepModel?.SaveProduction();
        }

        public bool EnterWaitInspection()
        {
            buttonResetCount.Enabled = false;
            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Wait);

            return inspectPanel.EnterWaitInspection();
        }

        public void ExitWaitInspection()
        {
            inspectPanel.ExitWaitInspection();

            buttonResetCount.Enabled = true;

            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Idle);
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            InspectRunner inspectRunner = SystemManager.Instance().InspectRunner;
            if (SystemState.Instance().OpState == OpState.Idle)
            {
                if (inspectRunner.EnterWaitInspection() == true)
                {
                    buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
                }
            }
            else
            {
                inspectRunner.ExitWaitInspection();
                repeatInspection.Checked = false;
                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
            }
        }

        //public void UpdateImage()
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "InspectionPage - UpdateImage");

        //    InspectionStep inspectStep = SystemManager.Instance().CurrentModel.GetInspectionStep(0);
        //    inspectStep.UpdateImageBuffer(imageAcquisition.ImageBuffer);

        //    if (lightCtrlHandler != null)
        //    {
        //        LightValueList lightValueList = imageAcquisition.ImageBuffer.GetLightValueList(0);
        //        lightCtrlHandler.TurnOn(lightValueList[0]);
        //    }

        //    imageAcquisition.Acquire(inspectStep.StepNo, 0);

        //    int numOfCameraImage = Math.Min(OperationSettings.Instance().NumOfResultView, MachineSettings.Instance().NumCamera);

        //    for (int i = 0; i < numOfCameraImage; i++)
        //    {
        //        Bitmap preBitmap = resultView[i].Image;

        //        ImageDevice imageDevice = SystemManager.Instance().Machine.ImageDeviceHandler.GetImageDevice(i);

        //        resultView[i].ZoomScale = -1;

        //        if (imageDevice.IsDepthScanner())
        //        {
        //            resultView[i].Image3d = imageAcquisition.ImageBuffer.GetImageBuffer3dItem(i).ResultImage;
        //            resultView[i].MouseDoubleClicked = resultView_mouseDoubleClicked;
        //        }
        //        else
        //        {
        //            resultView[i].Image = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(i, 0).Image.ToBitmap();
        //            resultView[i].MouseDoubleClicked = null;
        //        }

        //        if (preBitmap != null)
        //            preBitmap.Dispose();
        //    }
        //}

        private void ButtonResetCount_Click(object sender, EventArgs e)
        {
            string message = StringManager.GetString("Inspection count will be Reset. Continue?");
            DialogResult result = MessageForm.Show(ParentForm, message, MessageFormType.YesNo);
            if (result != DialogResult.Yes)
            {
                return;
            }

            Production production = ModelManager.Instance().CurrentStepModel.Production;

            production.Reset();
            productionNg.Text = "0";
            productionGood.Text = "0";
            productionTotal.Text = "0";
            SystemManager.Instance().InspectRunner.ResetState(); // 치약 바코드, 샴푸바코드는 IO와 검사 결과까지 모두 초기화 해줘야 한다.
        }

        private void UpdateStatusLabel(string text, Color foreColor, Color backColor)
        {
            labelStatus.BackColor = backColor;
            labelStatus.ForeColor = foreColor;
            labelStatus.Text = StringManager.GetString(text);
        }

        public void ChangeStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new OpStateChangedDelegate(ChangeStatus));
                return;
            }

            if (ErrorManager.Instance().IsAlarmed())
            {
                UpdateStatusLabel("Alarm", Color.White, Color.Red);
                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
            }
            else
            {
                if (SystemState.Instance().OpState == OpState.Idle)
                {
                    buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;
                }
                else
                {
                    buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
                }

                switch (SystemState.Instance().OpState)
                {
                    case OpState.Inspect:
                        switch (SystemState.Instance().InspectState)
                        {
                            case InspectState.Review:
                            case InspectState.Done:

                                break;
                            case InspectState.Run:
                                UpdateStatusLabel("Inspecting", Color.Black, Color.CornflowerBlue);
                                break;
                        }
                        break;
                    case OpState.Wait:
                        UpdateStatusLabel("Waitting", Color.Black, Color.CornflowerBlue);
                        break;
                    case OpState.Idle:
                        UpdateStatusLabel("Idle", Color.Black, Color.Gray);
                        break;
                }
            }
        }

        private void InspectionPage_Load(object sender, EventArgs e)
        {
            SystemState.Instance().OpStateChanged = ChangeStatus;
            ChangeStatus();
        }

        private void RepeatTimer_Tick(object sender, EventArgs e)
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                repeatInspection.Checked = false;
                return;
            }

            if (SystemState.Instance().OnInspection == false)
            {
                SystemManager.Instance().InspectRunner.Inspect();
            }
        }

        private void RepeatInspection_CheckedChanged(object sender, EventArgs e)
        {
            OperationConfig.Instance().RepeatInspection = repeatInspection.Checked;

            if (repeatInspection.Checked)
            {
                repeatTimer.Start();
            }
            else
            {
                repeatTimer.Stop();
            }
        }

        private void ButtonTrigger_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.Inspect();
        }

        public void EnableControls()
        {

        }

        public void TabPageVisibleChanged(bool visible)
        {

        }

        public void SaveCaptureResult(string resultPath)
        {
            var bmp = new Bitmap(viewContainer.Width, viewContainer.Height);
            viewContainer.DrawToBitmap(bmp, new Rectangle(0, 0, viewContainer.Width, viewContainer.Height));
            string path = Path.Combine(resultPath, "Result.jpg");
            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void LabelModelName_Click(object sender, EventArgs e)
        {
            SaveCaptureResult("D:\\");
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {

        }

        public void ModelListChanged()
        {

        }

        public void ModelOpen(ModelBase model)
        {
            ModelChanged();
        }

        public void ModelClosed(ModelBase model)
        {
            ModelChanged();
        }

        public void ModelChanged()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionPage - ModelChanged");

            StepModel currentModel = ModelManager.Instance().CurrentStepModel;
            if (currentModel == null)
            {
                modelName.Text = "";

                productionNg.Text = "";
                productionGood.Text = "";
                productionTotal.Text = "";
            }
            else if (modelName.Text != currentModel.Name)
            {
                modelName.Text = currentModel.Name;

                offlineImagePathList = new string[1] { Path.Combine(currentModel.ModelPath, "Image") };

                Production production = currentModel.Production;

                productionNg.Text = production.Ng.ToString();
                productionGood.Text = production.Good.ToString();
                productionTotal.Text = production.Total.ToString();

                inspectPanel.ClearPanel();
            }
        }

        // 구현 필요 없는 인터페이스
        public void StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult) { }
        public void TargetBeginInspect(Target target) { }
        public void TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList) { }
        public void ProbeBeginInspect() { }
        public void ProbeEndInspect() { }
    }
}
