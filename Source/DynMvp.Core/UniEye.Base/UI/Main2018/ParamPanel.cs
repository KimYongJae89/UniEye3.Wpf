using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.InspectData;
using DynMvp.Vision;
using Infragistics.Win.UltraWinDock;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.UI.ParamControl;

namespace UniEye.Base.UI.Main2018
{
    public partial class ParamPanel : UserControl, IParamControl, IModellerPane
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;
        private bool onValueUpdate = false;

        private CommandManager commandManager;

        private TeachHandler teachHandler;
        public TeachHandler TeachHandler
        {
            set
            {
                teachHandler = value;
                objectInfoPanel?.SetTeachHandler(value);
            }
        }

        private IAlgorithmParamControl selectedAlgorithmParamControl = null;

        private List<IAlgorithmParamControl> paramControlList = new List<IAlgorithmParamControl>();

        private ImageD targetGroupImage;
        private int lightTypeIndex = 0;
        private IObjectInfoPanel objectInfoPanel = null;

        public string Title => "Parameter";

        public PaneType PaneType => PaneType.Param;

        public Control Control => this;

        public DockedLocation DockedLocation => DockedLocation.DockedRight;

        public void OnPreSelectedInspect()
        {
        }

        public void OnPostSelectedInspect(ProbeResultList probeResultList)
        {
        }

        public void StepChanged(InspectStep inspectStep)
        {
        }

        public ParamPanel()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin ParamPanel-Ctor");

            InitializeComponent();

            SuspendLayout();

            UiManager.Instance().CreateTargetSubParamControl(paramControlList);

            foreach (IAlgorithmParamControl paramControl in paramControlList)
            {
                paramControl.ValueChanged = new AlgorithmValueChangedDelegate(ParamControl_ValueChanged);
            }

            objectInfoPanel = UiManager.Instance().CreateObjectInfoPanel();
            ((TargetInfoPanel)objectInfoPanel).ParamPanel = this;
            if (objectInfoPanel != null)
            {
                Controls.Add((UserControl)objectInfoPanel);
            }

            ResumeLayout(false);
            PerformLayout();

            LogHelper.Debug(LoggerType.Operation, "End ParamPanel-Ctor");
        }

        public void Init(TeachHandler teachHandler, CommandManager commandManager, AlgorithmValueChangedDelegate valueChanged)
        {
            this.teachHandler = teachHandler;
            objectInfoPanel.SetTeachHandler(teachHandler);

            ValueChanged = valueChanged;
            this.commandManager = commandManager;

            foreach (IAlgorithmParamControl paramControl in paramControlList)
            {
                paramControl.CommandManager = commandManager;
            }
        }

        public void UpdateTargetGroupImage(ImageD targetGroupImage, int lightTypeIndex)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - UpdateTargetImage(Bitmap targetImage)");

            this.targetGroupImage = targetGroupImage;
            this.lightTypeIndex = lightTypeIndex;

            objectInfoPanel?.UpdateTargetImage(targetGroupImage);

            TeachState.Instance().TargetGroupImage = targetGroupImage;

            LogHelper.Debug(LoggerType.Operation, "End TargetParamControl - UpdateTargetImage(Bitmap targetImage)");
        }

        public void SelectObject(ITeachObject teachObject)
        {
            if (teachObject is Target target)
            {
                SelectTarget(target);
                UpdateTargetImage(target.Image);

                if (selectedAlgorithmParamControl != null)
                {
                    selectedAlgorithmParamControl.Hide();
                }
            }
            else if (teachObject is Probe probe)
            {
                SelectProbe(probe);
            }
            else
            {
                ShowAlgorithmParamControl(null);
            }
        }

        public void SelectObject(List<ITeachObject> teachObjectList)
        {
            onValueUpdate = true;

            if (teachObjectList.Count == 1)
            {
                if (teachObjectList[0] is Probe probe)
                {
                    SelectProbe(probe);
                }
                else
                {
                    if (teachObjectList[0] is Target target)
                    {
                        SelectTarget(target);
                    }
                }

            }

            onValueUpdate = false;

            ShowAlgorithmParamControl(teachObjectList);
        }

        public void SelectProbe(Probe probe)
        {
            onValueUpdate = true;

            objectInfoPanel?.SelectProbe(probe);

            SelectTarget(probe.Target);

            onValueUpdate = false;

            ShowAlgorithmParamControl(new List<ITeachObject>() { probe });
        }

        public void SelectTarget(Target target)
        {
            objectInfoPanel?.SelectTarget(target);
            UpdateTargetImage(target.Image);
        }

        private void UpdateTargetImage(ImageD image)
        {
            objectInfoPanel?.UpdateTargetImage(image);
        }

        public void ClearProbeData()
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ClearProbeData");

            objectInfoPanel?.ClearProbeData();

            if (selectedAlgorithmParamControl != null)
            {
                selectedAlgorithmParamControl.Hide();
                selectedAlgorithmParamControl.ClearSelectedProbe();
                selectedAlgorithmParamControl = null;
            }
        }

        public void ClearTargetData()
        {

        }

        public void TestSelectObject()
        {
            var visionProbe = new VisionProbe();
            visionProbe.InspAlgorithm = new PatternMatching();
            visionProbe.Target = new Target();
            visionProbe.Target.InspectStep = new InspectStep(0);
            SelectProbe(visionProbe);
        }

        public void ShowAlgorithmParamControl(List<ITeachObject> teachObjectList)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ShowAlgorithmParamControl");

            IAlgorithmParamControl preSelectedAlgorithmParamControl = selectedAlgorithmParamControl;

            var probeList = new ProbeList();
            foreach (ITeachObject teachObject in teachObjectList)
            {
                if (!(teachObject is Probe probe))
                {
                    continue;
                }

                probeList.AddProbe(probe);
            }

            selectedAlgorithmParamControl = null;

            //probeList = probeList.GetFilteredList();
            if (probeList.Count > 0)
            {
                IAlgorithmParamControl curAlgorithmParamControl = null;
                foreach (Probe probe in probeList)
                {
                    foreach (IAlgorithmParamControl paramControl in paramControlList)
                    {
                        if (paramControl.GetTypeName() == probe.ProbeType.ToString())
                        {
                            curAlgorithmParamControl = paramControl;
                            break;
                        }
                    }

                    if (selectedAlgorithmParamControl == null)
                    {
                        selectedAlgorithmParamControl = curAlgorithmParamControl;
                    }
                    else if (selectedAlgorithmParamControl != curAlgorithmParamControl)
                    {
                        selectedAlgorithmParamControl = null;
                        break;
                    }
                }
            }

            if (selectedAlgorithmParamControl != preSelectedAlgorithmParamControl)
            {
                if (preSelectedAlgorithmParamControl != null)
                {
                    Controls.RemoveAt(0);
                    preSelectedAlgorithmParamControl.Hide();
                    preSelectedAlgorithmParamControl.ClearSelectedProbe();
                }

                if (selectedAlgorithmParamControl != null)
                {
                    Controls.Add((UserControl)selectedAlgorithmParamControl);
                    Controls.SetChildIndex((UserControl)selectedAlgorithmParamControl, 0);

                    selectedAlgorithmParamControl.Show();
                }
            }

            if (selectedAlgorithmParamControl != null)
            {
                selectedAlgorithmParamControl.SelectProbe(probeList);
            }
        }

        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam, bool modified)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ParamControl_ValueChanged");

            if (onValueUpdate == false)
            {
                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, modified);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
            selectedAlgorithmParamControl?.PointSelected(clickPos, ref processingCancelled);
        }

        private void TargetParamControl_Load(object sender, EventArgs e)
        {
            //this.targetInfoPanal = SystemManager.Instance().UiChanger.CreateTargetInfoPanel();
            //if (targetInfoPanal == null)
            //{
            //    this.panelTargetInfo.Visible = false;
            //}
            //else
            //{
            //    targetInfoPanal.SetTeachHandler(this.teachHandlerProbe);
            //    this.panelTargetInfo.Controls.Add((Control)targetInfoPanal);
            //}
        }

        public void SetLightParamSet(LightParamSet lightParamSet)
        {
            foreach (IAlgorithmParamControl paramControl in paramControlList)
            {
                if (paramControl is VisionParamControl visionParamControl)
                {
                    visionParamControl.LightParamSet = lightParamSet;
                    break;
                }
            }
        }

        public void Clear()
        {

        }

        public void PointSelected(PointF point, ref bool processingCancelled)
        {

        }

        public void UpdateImage(Image2D sourceImage2d, int lightTypeIndex)
        {

        }
    }
}
