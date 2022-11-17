using Authentication.Core;
using Authentication.Core.Datas;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.InspectData;
using DynMvp.Vision;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniEye.Base.UI.InspectionPanel;
using UniEye.Base.UI.Main;
using UniEye.Base.UI.Main2018;
using UniEye.Base.UI.ParamControl;

namespace UniEye.Base.UI
{
    public class UiManager
    {
        protected IMainForm mainForm;
        public IMainForm MainForm => mainForm;

        private ToolStripStatusLabel statusLabelMessage;
        private ToolStripStatusLabel statusLabelUser;
        private ToolStripStatusLabel statusLabelLicense;
        private ToolStripStatusLabel statusLabelMouseOffset;
        private ToolStripStatusLabel statusLabelMousePos;
        private ToolStripStatusLabel statusLabelRobotPos;
        private ToolStripStatusLabel statusCPUTemperature;
        private ToolStripStatusLabel statusSystemTemperature;
        private Computer computerHardware;
        private static UiManager _instance = null;
        public static UiManager Instance()
        {
            if (_instance == null)
            {
                _instance = new UiManager();
            }

            return _instance;
        }

        public static void SetInstance(UiManager uiManager)
        {
            _instance = uiManager;
            UserHandler.Instance.OnUserChanged = uiManager.OnUserChanged;
        }

        public UiManager()
        {
            computerHardware = new Computer();
            computerHardware.Open();
            computerHardware.CPUEnabled = true;
            computerHardware.HDDEnabled = true;
        }

        public void ChangeOpMode(OpMode opMode)
        {
            mainForm.ChangeOpMode(opMode);
        }

        public OpMode GetOpMode()
        {
            if (mainForm == null)
            {
                return OpMode.Idle;
            }

            return mainForm.GetOpMode();
        }

        public virtual void SetupVisionParamControl(VisionParamControl visionParamControl)
        {
            var algorithmFactory = DynMvp.Vision.AlgorithmFactory.Instance();

            if (algorithmFactory.IsAlgorithmEnabled(PatternMatching.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new PatternMatchingParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(BinaryCounter.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new BinaryCounterParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(BrightnessChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new BrightnessCheckerParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(ColorChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new ColorCheckerParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(WidthChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new WidthCheckerParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(EdgeChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new EdgeCheckerParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(CircleChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new CircleCheckeParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(BlobChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new BlobCheckeParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(BarcodeReader.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new BarcodeReaderParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(CharReader.TypeName) == true)
            {
                if (OperationConfig.Instance().ImagingLibrary == ImagingLibrary.MatroxMIL)
                {
                    visionParamControl.AddAlgorithmParamControl(new MilCharReaderParamControl());
                }
                else
                {
                    visionParamControl.AddAlgorithmParamControl(new CharReaderParamControl());
                }
            }
            if (algorithmFactory.IsAlgorithmEnabled(RectChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new RectCheckerParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(DepthChecker.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new DepthCheckerParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(BlobSubtractor.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new BlobSubtractorParamControl());
            }

            if (algorithmFactory.IsAlgorithmEnabled(GridSubtractor.TypeName) == true)
            {
                visionParamControl.AddAlgorithmParamControl(new GridSubtractorParamControl());
            }
        }

        public virtual ITeachPanel CreateTeachPanel()
        {
            var teachPanel = new TeachPanel();

            teachPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            teachPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            teachPanel.Location = new System.Drawing.Point(3, 3);
            teachPanel.Name = "targetImage";
            teachPanel.Size = new System.Drawing.Size(409, 523);
            teachPanel.TabIndex = 8;
            teachPanel.TabStop = false;
            teachPanel.Enable = true;

            return teachPanel;
        }

        public virtual List<IModellerPane> CreateModellerPane()
        {
            return new List<IModellerPane>();
        }

        /// <summary>
        /// 사용자가 변경될 때 호출됨.
        /// </summary>
        public virtual void OnUserChanged(User user)
        {

        }

        public virtual void ChangeModellerMenu(IModellerPage modellerPage)
        {

        }

        public virtual IObjectInfoPanel CreateObjectInfoPanel()
        {
            //TargetInfoPanel targetInfoPanel = new TargetInfoPanel();

            //targetInfoPanel.Name = "DefaultTargetInfoPanel";
            //targetInfoPanel.Location = new System.Drawing.Point(0, 0);
            //targetInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            //targetInfoPanel.TabIndex = 26;
            //return targetInfoPanel;

            //ProbeInfoPanel proboInfoPanel = new ProbeInfoPanel();

            //proboInfoPanel.Name = "DefaultProbeInfoPanel";
            //proboInfoPanel.Location = new System.Drawing.Point(0, 0);
            //proboInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            //proboInfoPanel.TabIndex = 26;

            return null;
        }

        public virtual ModelForm CreateModelForm()
        {
            return new ModelForm();
        }

        public virtual string[] GetProbeNames()
        {
            return null;
        }

        public virtual IMainForm CreateMainForm()
        {
            var mainForm = new UniEye.Base.UI.Main.MainForm();
            ModelManager.Instance().AddListener(mainForm);

            this.mainForm = mainForm;

            return mainForm;
        }

        public virtual void CreateTargetSubParamControl(List<IAlgorithmParamControl> paramControlList)
        {
            var visionParamControl = new VisionParamControl();
            var daqParamControl = new DaqParamControl();
            var markerParamControl = new MarkerParamControl();

            visionParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            //visionParamControl.Hide();
            paramControlList.Add(visionParamControl);

            daqParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            daqParamControl.Hide();
            paramControlList.Add(daqParamControl);

            markerParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            markerParamControl.Hide();
            paramControlList.Add(markerParamControl);
        }

        public virtual IRightPanel CreateRightPanel()
        {
            return null;
        }

        public virtual IInspectPanel CreateInspectionPanel()
        {
            var inspectionPanel = new SingleStepInspectionPanel();
            return inspectionPanel;
        }

        public virtual IReportPanel CreateReportPanel()
        {
            return new ReportResultPanel();
        }

        public virtual List<IMainTabPage> CreateMainTabPage()
        {
            var inspectPage = new Main.InspectPage();
            SystemManager.Instance().InspectRunner.InspectEventHandler.AddListener(inspectPage);

            var tabPageList = new List<IMainTabPage>();
            tabPageList.Add(new ModelPage());
            tabPageList.Add(inspectPage);
            tabPageList.Add(new Main.ModellerPage());
            tabPageList.Add(new SettingPage());

            return tabPageList;
        }

        public virtual IInspectionTimeExpender CreateInspectionTimeExpenter()
        {
            return null;
        }

        public virtual ILightParamForm CreateLightParamForm()
        {
            var lightParamForm = new LightParamForm();
            // 단순 Light Param Form을 사용할 경우, 아래 라인을 사용한다.
            // LightParamSimpleForm lightParamForm = new LightParamSimpleForm();
            lightParamForm.Location = new System.Drawing.Point(96, 95);
            lightParamForm.Name = "LightParamForm";
            lightParamForm.TabIndex = 0;
            lightParamForm.Visible = false;

            lightParamForm.TopMost = true;

            return lightParamForm;
        }

        public virtual List<Control> GetModellerDockControlList()
        {
            var controlList = new List<Control>();

            var modelTreePanel = new ModelTreePanel();
            controlList.Add(modelTreePanel);

            var fiducialPanel = new FiducialPanel();
            controlList.Add(fiducialPanel);

            return controlList;
        }

        public virtual IParamControl CreateParamControl()
        {
            var targetParamControl = new TargetParamControl();

            targetParamControl.Location = new System.Drawing.Point(96, 95);
            targetParamControl.Name = "targetParamControl";
            targetParamControl.Size = new System.Drawing.Size(74, 101);
            targetParamControl.TabIndex = 0;
            targetParamControl.Dock = System.Windows.Forms.DockStyle.Fill;

            return targetParamControl;
        }

        public virtual IModellerToolbar CreateModellerToolbar()
        {
            var modellerToolBar = new Main.ModellerToolbar();

            modellerToolBar.BackColor = System.Drawing.SystemColors.ButtonFace;
            modellerToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            modellerToolBar.Location = new System.Drawing.Point(0, 313);
            modellerToolBar.Name = "modellerToolBar";
            modellerToolBar.Size = new System.Drawing.Size(466, 359);
            modellerToolBar.TabIndex = 0;

            return modellerToolBar;
        }

        private ToolStripButton CreateToolStripButton(string text, string imageName, string commandName, TextImageRelation textImageRelation = TextImageRelation.ImageAboveText)
        {
            var toolStripButton = new ToolStripButton();
            toolStripButton.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageName);
            toolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton.Name = commandName;
            toolStripButton.Size = new System.Drawing.Size(78, 59);
            toolStripButton.Text = "Property";
            toolStripButton.TextImageRelation = textImageRelation;
            return toolStripButton;
        }

        public virtual ModellerPageExtender CreateModellerPageExtender()
        {
            return new ModellerPageExtender();
        }

        public virtual IModelFormExtraProperty CreateModelExtraPropertyPanel()
        {
            return null;
        }

        public virtual IDefectReportPanel CreateDefectReportPanel()
        {
            var defectReportPanel = new DefectReportPanel();

            defectReportPanel.Location = new System.Drawing.Point(620, 47);
            defectReportPanel.Name = "DefectReportPanel";
            defectReportPanel.Size = new System.Drawing.Size(683, 489);
            defectReportPanel.TabIndex = 177;
            defectReportPanel.TabStop = false;

            return defectReportPanel;
        }

        public virtual void BuildAdditionalAlgorithmTypeMenu(IModellerPage modellerPage, ToolStripItemCollection dropDownItems)
        {

        }

        public virtual void SetupStatusStrip(StatusStrip statusStrip)
        {
            statusLabelMessage = new ToolStripStatusLabel();
            statusLabelMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            statusLabelMessage.TextAlign = ContentAlignment.MiddleLeft;
            statusLabelMessage.Spring = true;
            statusLabelMessage.Text = "Ready";
            statusStrip.Items.Add(statusLabelMessage);

            statusLabelUser = new ToolStripStatusLabel();
            statusLabelUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusLabelUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            statusLabelUser.Text = "User";
            statusStrip.Items.Add(statusLabelUser);

            statusLabelLicense = new ToolStripStatusLabel();
            statusLabelLicense.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusLabelLicense.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            statusLabelLicense.Text = "License";
            statusStrip.Items.Add(statusLabelLicense);

            statusLabelMousePos = new ToolStripStatusLabel();
            statusLabelMousePos.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusLabelMousePos.Image = global::UniEye.Base.Properties.Resources.Mouse_32;
            statusLabelMousePos.Name = "statusLabelMousePos";
            statusLabelMousePos.Text = "(0, 0)";
            statusStrip.Items.Add(statusLabelMousePos);

            statusLabelMouseOffset = new ToolStripStatusLabel();
            statusLabelMouseOffset.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusLabelMouseOffset.Image = global::UniEye.Base.Properties.Resources.pos;
            statusLabelMouseOffset.Name = "statusLabelMouseOffset";
            statusLabelMouseOffset.Text = "(0, 0)";
            statusStrip.Items.Add(statusLabelMouseOffset);

            statusLabelRobotPos = new ToolStripStatusLabel();
            statusLabelRobotPos.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusLabelRobotPos.Image = global::UniEye.Base.Properties.Resources.Manipulator_32;
            statusLabelRobotPos.Name = "statusLabelRobotPos";
            statusLabelRobotPos.Text = "(0, 0)";
            statusStrip.Items.Add(statusLabelRobotPos);

            statusCPUTemperature = new ToolStripStatusLabel();
            statusCPUTemperature.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusCPUTemperature.Name = "statusCPUTemperature";
            statusCPUTemperature.Text = "CPU : 0°C";
            statusStrip.Items.Add(statusCPUTemperature);

            statusSystemTemperature = new ToolStripStatusLabel();
            statusSystemTemperature.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            statusSystemTemperature.Name = "statusSystemTemperature";
            statusSystemTemperature.Text = "System : 0°C";
            statusStrip.Items.Add(statusSystemTemperature);
        }

        public virtual void UpdateStatusStrip()
        {
            statusLabelLicense.Text = OperationConfig.Instance().ImagingLibrary.ToString().Substring(0, 1) + AlgorithmFactory.Instance()?.LicenseErrorCount.ToString();
            statusLabelUser.Text = UserHandler.Instance.CurrentUser.UserId;

            foreach (IHardware hardware in computerHardware.Hardware)
            {
                hardware.Update();

                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.Name.Contains("CPU Package"))
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            statusCPUTemperature.Text = "CPU : " + sensor.Value.ToString() + "°C";
                        }
                        if (sensor.SensorType == SensorType.Power)
                        {
                            int sensorValue = Convert.ToInt32(sensor.Value);
                            statusSystemTemperature.Text = "System : " + sensorValue.ToString() + "°C";
                        }
                    }
                }
            }

            if (DeviceManager.Instance().RobotStage != null)
            {
                AxisPosition axisPosition = DeviceManager.Instance().RobotStage.GetActualPos();
                statusLabelRobotPos.Text = axisPosition.ToString();
            }

            /*
             * 상태바 업데이트
            MachineInterface.MachineIf machineIf = SystemManager.Instance().MachineIf;

            statusMachineAlive.BackColor = machineIf.MachineAlive ? Color.LimeGreen : Color.White;
            statusMachineTrigger.BackColor = machineIf.Trigger ? Color.LimeGreen : Color.White;
            statusMachineDone.BackColor = machineIf.CommandDone ? Color.LimeGreen : Color.White;
            statusVisionAlive.BackColor = machineIf.VisionAlive ? Color.LimeGreen : Color.White;
            statusVisionReady.BackColor = machineIf.Ready ? Color.LimeGreen : Color.White;
            statusVisionWorking.BackColor = machineIf.OnWorking ? Color.LimeGreen : Color.White;
            statusVisionComplete.BackColor = machineIf.Complete ? Color.LimeGreen : Color.White;
            */
        }
    }
}
