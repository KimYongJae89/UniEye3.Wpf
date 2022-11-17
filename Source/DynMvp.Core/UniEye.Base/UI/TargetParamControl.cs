using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.UI.ParamControl;

namespace UniEye.Base.UI
{
    public interface ITargetParamControl
    {
        void SetLightParamSet(LightParamSet lightParamSet);

        void ClearProbeData();
        void Init(TeachHandler teachHandlerProbe, CommandManager commandManager, AlgorithmValueChangedDelegate valueChanged);
        void PointSelected(Point clickPos, ref bool processingCancelled);
        void SelectObject(List<ITeachObject> teachObjectList);
        void UpdateTargetGroupImage(ImageD targetGroupImage, int lightTypeIndex);
    }

    public partial class TargetParamControl : UserControl, IParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged = null;
        private bool onValueUpdate = false;

        private CommandManager commandManager;
        private TeachHandler teachHandler;

        private IAlgorithmParamControl selectedProbeParamControl = null;

        private List<IAlgorithmParamControl> probeParamControlList = new List<IAlgorithmParamControl>();

        private ImageD targetGroupImage;
        private int lightTypeIndex = 0;
        private IObjectInfoPanel objectInfoPanel = null;

        public TargetParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin TargetParamControl-Ctor");

            InitializeComponent();

            SuspendLayout();

            UiManager.Instance().CreateTargetSubParamControl(probeParamControlList);

            foreach (IAlgorithmParamControl paramControl in probeParamControlList)
            {
                paramControl.ValueChanged = new AlgorithmValueChangedDelegate(ParamControl_ValueChanged);
            }

            objectInfoPanel = UiManager.Instance().CreateObjectInfoPanel();
            if (objectInfoPanel != null)
            {
                Controls.Add((UserControl)objectInfoPanel);
            }

            ResumeLayout(false);
            PerformLayout();

            LogHelper.Debug(LoggerType.Operation, "End TargetParamControl-Ctor");
        }

        public void Init(TeachHandler teachHandler, CommandManager commandManager, AlgorithmValueChangedDelegate valueChanged)
        {
            this.teachHandler = teachHandler;
            objectInfoPanel?.SetTeachHandler(teachHandler);
            ValueChanged = valueChanged;
            this.commandManager = commandManager;

            foreach (IAlgorithmParamControl paramControl in probeParamControlList)
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

                if (selectedProbeParamControl != null)
                {
                    selectedProbeParamControl.Hide();
                }
            }
            else if (teachObject is Probe probe)
            {
                SelectProbe(probe);
            }
            else
            {
                ShowProbeParamControl(null);
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

            ShowProbeParamControl(teachObjectList);
        }

        public void SelectProbe(Probe probe)
        {
            onValueUpdate = true;

            objectInfoPanel?.SelectProbe(probe);

            SelectTarget(probe.Target);

            onValueUpdate = false;

            ShowProbeParamControl(new List<ITeachObject>() { probe });
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

            if (selectedProbeParamControl != null)
            {
                selectedProbeParamControl.Hide();
                selectedProbeParamControl.ClearSelectedProbe();
                selectedProbeParamControl = null;
            }
        }

        public void ClearTargetData()
        {

        }

        public void ShowProbeParamControl(List<ITeachObject> teachObjectList)
        {
            LogHelper.Debug(LoggerType.Operation, "TargetParamControl - ShowAlgorithmParamControl");

            IAlgorithmParamControl preSelectedAlgorithmParamControl = selectedProbeParamControl;

            var probeList = new ProbeList();
            foreach (ITeachObject teachObject in teachObjectList)
            {
                if (!(teachObject is Probe probe))
                {
                    continue;
                }

                probeList.AddProbe(probe);
            }

            selectedProbeParamControl = null;

            //probeList = probeList.GetFilteredList();
            if (probeList.Count > 0)
            {
                IAlgorithmParamControl curProbeParamControl = null;
                foreach (Probe probe in probeList)
                {
                    foreach (IAlgorithmParamControl probeParamControl in probeParamControlList)
                    {
                        if (probeParamControl.GetTypeName() == probe.ProbeType.ToString())
                        {
                            curProbeParamControl = probeParamControl;
                            break;
                        }
                    }

                    if (selectedProbeParamControl == null)
                    {
                        selectedProbeParamControl = curProbeParamControl;
                    }
                    else if (selectedProbeParamControl != curProbeParamControl)
                    {
                        selectedProbeParamControl = null;
                        break;
                    }
                }
            }

            if (selectedProbeParamControl != preSelectedAlgorithmParamControl)
            {
                if (preSelectedAlgorithmParamControl != null)
                {
                    Controls.RemoveAt(0);
                    preSelectedAlgorithmParamControl.Hide();
                    preSelectedAlgorithmParamControl.ClearSelectedProbe();
                }

                if (selectedProbeParamControl != null)
                {
                    Controls.Add((UserControl)selectedProbeParamControl);
                    Controls.SetChildIndex((UserControl)selectedProbeParamControl, 0);

                    selectedProbeParamControl.Show();
                }
            }

            if (selectedProbeParamControl != null)
            {
                selectedProbeParamControl.SelectProbe(probeList);
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
            selectedProbeParamControl?.PointSelected(clickPos, ref processingCancelled);
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
            foreach (IAlgorithmParamControl paramControl in probeParamControlList)
            {
                if (paramControl is VisionParamControl visionParamControl)
                {
                    visionParamControl.LightParamSet = lightParamSet;
                    break;
                }
            }
        }
    }
}
