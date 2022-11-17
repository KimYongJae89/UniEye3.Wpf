using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.FilterForm;
using DynMvp.Data.Forms;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI.ParamControl
{
    public partial class VisionParamControl : UserControl, IAlgorithmParamControl
    {
        private ProbeList probeList = new ProbeList();

        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        // 컨트롤의 값을 프로그램적으로 갱신하고 있는 동안, 부품 이미지의 갱신을 하지 않도록 하기 위해 사용하는 파라미터
        // 갑이 갱신될 때, 각 갱신 이벤트에 이미지 갱신을 하는 함수를 호출하고 있어, 이 플랙이 없을 경우 반복적으로 갱신이 수행됨.
        private bool onValueUpdate = false;

        private IAlgorithmParamControl selectedAlgorithmParamControl = null;
        private IFilterParamControl selectedFilterParamControl = null;
        public List<IAlgorithmParamControl> AlgorithmParamControlList { get; set; } = new List<IAlgorithmParamControl>();
        private List<IFilterParamControl> filterParamControlList = new List<IFilterParamControl>();
        private LightParamSet lightParamSet;
        public LightParamSet LightParamSet
        {
            set => lightParamSet = value;
        }

        public VisionParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin VisionParamControl-Ctor");

            InitializeComponent();

            SuspendLayout();

            // Filter Param Control
            AddFilterParamControl(new BinarizationFilterParamControl());
            AddFilterParamControl(new EdgeExtractionFilterParamControl());
            AddFilterParamControl(new MorphologyFilterParamControl());
            AddFilterParamControl(new NoParamFilterParamControl());
            //AddFilterParamControl(new SubtractionFilterParamControl());
            AddFilterParamControl(new MaskFilterParamControl());

            ResumeLayout(false);
            PerformLayout();

            //change language begin
            labelPos.Text = StringManager.GetString(labelPos.Text);
            labelSize.Text = StringManager.GetString(labelSize.Text);
            inverseResult.Text = StringManager.GetString(inverseResult.Text);
            labelW.Text = StringManager.GetString(labelW.Text);
            labelH.Text = StringManager.GetString(labelH.Text);
            imageBandLabel.Text = StringManager.GetString(imageBandLabel.Text);
            labelFiducialProbe.Text = StringManager.GetString(labelFiducialProbe.Text);
            buttonAddFilter.Text = StringManager.GetString(buttonAddFilter.Text);
            buttonDeleteFilter.Text = StringManager.GetString(buttonDeleteFilter.Text);
            imageBandLabel.Text = StringManager.GetString(imageBandLabel.Text);
            //change language end

            contextMenuStripAddFilter.Items.Clear();
            string[] filterTypeNames = Enum.GetNames(typeof(FilterType));
            foreach (string filterTypeName in filterTypeNames)
            {
                ToolStripItem filterToolStripItem = contextMenuStripAddFilter.Items.Add(StringManager.GetString(filterTypeName));
                filterToolStripItem.Tag = filterTypeName;
                filterToolStripItem.Click += filterToolStripItem_Click;
            }

            UpdateLightTypeCombo();

            probeHeight.Minimum = probeWidth.Minimum = 0;
            probeHeight.Maximum = probeWidth.Maximum = int.MaxValue;

            probePosX.Minimum = probePosY.Minimum = 0;
            probePosX.Maximum = probePosY.Maximum = int.MaxValue;

            probePosR.Maximum = 360;
            probePosR.Minimum = 0;

            LogHelper.Debug(LoggerType.Operation, "End VisionParamControl-Ctor");
        }

        public string GetTypeName()
        {
            return "Vision";
        }

        public void AddAlgorithmParamControl(IAlgorithmParamControl paramControl)
        {
            var userControl = (UserControl)paramControl;

            userControl.Name = "algorithmParamControl";
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;
            userControl.TabIndex = 26;
            userControl.Hide();
            paramControl.ValueChanged += VisionParamControl_AlgorithmValueChanged;

            AlgorithmParamControlList.Add(paramControl);
        }

        public void AddFilterParamControl(IFilterParamControl paramControl)
        {
            var userControl = (UserControl)paramControl;

            userControl.Name = "filterParamControl";
            userControl.Location = new System.Drawing.Point(0, 0);
            userControl.Size = new System.Drawing.Size(10, 10);
            userControl.Dock = System.Windows.Forms.DockStyle.Fill;
            userControl.TabIndex = 26;
            userControl.Hide();
            paramControl.SetValueChanged(new FilterParamValueChangedDelegate(FilterParamControl_ValueChanged));

            filterParamControlList.Add(paramControl);
        }

        private void UpdateLightTypeCombo()
        {
            LogHelper.Debug(LoggerType.Operation, "InitLightTypeList");
            bool preOnValueUpdate = onValueUpdate;
            onValueUpdate = true;

            if (comboBoxLightType.Items.Count == 0)
            {
                foreach (LightParam lightParam in LightConfig.Instance().LightParamSet)
                {
                    comboBoxLightType.Items.Add(lightParam.Name);
                }
            }
            else
            {
                comboBoxLightType.Text = "";
            }

            object lightTypeIndex = probeList.GetParamValue("LightTypeIndex");
            if (lightTypeIndex != null)
            {
                var lightTypeIndexEnumerable = lightTypeIndex as IEnumerable;
                foreach (object index in lightTypeIndexEnumerable)
                {
                    comboBoxLightType.SelectedIndex = (int)index;
                    break;
                }
            }

            onValueUpdate = preOnValueUpdate;
            //onValueUpdate = false;
        }

        private void FilterParamControl_ValueChanged()
        {
            VisionParamControl_AlgorithmValueChanged(ValueChangedType.ImageProcessing);
        }

        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            Point pt = buttonAddFilter.PointToScreen(new Point(buttonAddFilter.Bounds.Left, buttonAddFilter.Bounds.Bottom));
            contextMenuStripAddFilter.Show(pt);
        }

        private void UpdateFilterButton(bool flag)
        {
            buttonAddFilter.Enabled = flag;
            buttonDeleteFilter.Enabled = flag;
            buttonFilterUp.Enabled = flag;
            buttonFilterDown.Enabled = flag;
        }

        private void filterToolStripItem_Click(object sender, EventArgs e)
        {
            if (probeList.Count != 1)
            {
                UpdateFilterButton(false);
                return;
            }

            if (!(probeList[0] is VisionProbe visionProbe))
            {
                UpdateFilterButton(false);
                return;
            }

            UpdateFilterButton(true);

            Algorithm inspAlgorithm = visionProbe.InspAlgorithm;

            var filterToolStripItem = (ToolStripItem)sender;
            var filterType = (FilterType)Enum.Parse(typeof(FilterType), (string)filterToolStripItem.Tag);
            IFilter filter = null;

            foreach (IFilterParamControl filterParamControl in filterParamControlList)
            {
                if (filterType == filterParamControl.GetFilterType())
                {
                    filter = filterParamControl.CreateFilter();
                    break;
                }
            }

            if (filter != null)
            {
                AddFilter(filter);
                UpdateData();
            }
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();

            if (selectedAlgorithmParamControl != null)
            {
                ((UserControl)selectedAlgorithmParamControl).Visible = false;
                selectedAlgorithmParamControl.ClearSelectedProbe();
                selectedAlgorithmParamControl = null;
            }
        }

        //public void AddSelectedProbe(Probe probe)
        //{
        //    if (ShowAlgorithmParamControl((VisionProbe)probe) == true)
        //    {
        //        selectedProbeList.Clear();
        //    }

        //    selectedProbeList.Add((VisionProbe)probe);

        //    if (selectedTarget == null && selectedProbeList.Count == 1)
        //        selectedTarget = ((VisionProbe)probe).Target;
        //    else if (selectedTarget != ((VisionProbe)probe).Target)
        //        selectedTarget = null;

        //    if (selectedProbeList.Count != 0)
        //    {
        //        UpdateData();
        //    }
        //    else
        //    {
        //       EnableControls(false);
        //    }
        //}

        public void SelectProbe(ProbeList selectedProbeList)
        {
            probeList.Clear();
            probeList.AddProbe(selectedProbeList);

            UpdateData();

            if (probeList.Count > 0)
            {
                if (ShowAlgorithmParamControl(probeList) == false)
                {
                    probeList.Clear();
                }
                else
                {
                    ((IAlgorithmParamControl)selectedAlgorithmParamControl).SelectProbe(probeList);
                }
            }

            if (probeList.Count != 0)
            {
                UpdateData();
            }
            else
            {
                EnableControls(false);
            }
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (selectedFilterParamControl != null)
            {
                ((UserControl)selectedFilterParamControl).Hide();
            }

            if (probeList.Count == 0)
            {
                if (selectedAlgorithmParamControl != null)
                {
                    ((UserControl)selectedAlgorithmParamControl).Visible = false;
                    selectedAlgorithmParamControl.ClearSelectedProbe();
                    selectedAlgorithmParamControl = null;
                }
                EnableControls(false);
                return;
            }

            EnableControls(true);

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - UpdateData");

            onValueUpdate = true;

            probePosX.Text = probeList.GetParamValueStr("X");
            probePosY.Text = probeList.GetParamValueStr("Y");
            probePosR.Text = probeList.GetParamValueStr("Angle");
            probeWidth.Text = probeList.GetParamValueStr("Width");
            probeHeight.Text = probeList.GetParamValueStr("Height");

            UpdateFilterListBox();
            UpdateLightTypeCombo();

            comboFiducialProbe.Items.Clear();
            comboFiducialProbe.Items.Add("None");

            Target target = probeList.GetSingleTarget();

            if (target != null)
            {
                comboFiducialProbe.Enabled = true;
                foreach (Probe probe in target)
                {
                    if (probe is VisionProbe visionProbe && visionProbe.InspAlgorithm is Searchable == true)
                    {
                        comboFiducialProbe.Items.Add(probe);
                    }
                }
            }
            else
            {
                comboFiducialProbe.Enabled = false;
                comboFiducialProbe.Items.Clear();
            }

            string fidProbeId = probeList.GetParamValueStr("FiducialProbeId");
            if (fidProbeId == "-1")
            {
                comboFiducialProbe.Text = "None";
            }
            else if (fidProbeId != "")
            {
                comboFiducialProbe.Text = "Probe " + fidProbeId;
            }
            else
            {
                comboFiducialProbe.Text = "";
            }

            inverseResult.CheckState = probeList.GetCheckState("InverseResult");

            string sourceImageType = probeList.GetParamValueStr("SourceImageType");

            if (sourceImageType != "")
            {
                if (sourceImageType == "Grey")
                {
                    imageBand.Text = "Luminance";
                    imageBand.Enabled = false;
                }
                else
                {
                    imageBand.Text = probeList.GetParamValueStr("ImageBand");
                    imageBand.Enabled = true;
                }
            }

            onValueUpdate = false;

            //VisionParamControl_AlgorithmValueChanged(ValueChangedType.ImageProcessing, null, null, false);
        }

        private void EnableControls(bool enable)
        {
            probePosX.Enabled = enable;
            probePosY.Enabled = enable;
            probeWidth.Enabled = enable;
            probeHeight.Enabled = enable;
            buttonAddFilter.Enabled = enable;
            buttonDeleteFilter.Enabled = enable;
            inverseResult.Enabled = enable;
            imageBand.Enabled = enable;
            comboFiducialProbe.Enabled = enable;

            if (enable == false)
            {
                if (selectedAlgorithmParamControl != null)
                {
                    ((UserControl)selectedAlgorithmParamControl).Hide();
                    selectedAlgorithmParamControl.ClearSelectedProbe();
                    selectedAlgorithmParamControl = null;
                }
            }
        }

        private bool ShowAlgorithmParamControl(ProbeList probeList)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - ShowAlgorithmParamControl");

            IAlgorithmParamControl preSelectedAlgorithmParamControl = selectedAlgorithmParamControl;

            selectedAlgorithmParamControl = null;

            foreach (Probe probe in probeList)
            {
                if (!(probe is VisionProbe visionProbe))
                {
                    selectedAlgorithmParamControl = null;
                    break;
                }

                string algorithmType = visionProbe.InspAlgorithm.GetAlgorithmType();

                IAlgorithmParamControl curAlgorithmParamControl = null;
                foreach (IAlgorithmParamControl algorithmParamControl in AlgorithmParamControlList)
                {
                    if (algorithmType == algorithmParamControl.GetTypeName())
                    {
                        curAlgorithmParamControl = algorithmParamControl;
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

            if (selectedAlgorithmParamControl != preSelectedAlgorithmParamControl)
            {
                if (preSelectedAlgorithmParamControl != null)
                {
                    ((UserControl)preSelectedAlgorithmParamControl).Hide();
                    preSelectedAlgorithmParamControl.ClearSelectedProbe();
                }

                groupboxInspection.Controls.Clear();

                if (selectedAlgorithmParamControl != null)
                {
                    var userControl = (UserControl)selectedAlgorithmParamControl;
                    groupboxInspection.Controls.Add(userControl);

                    userControl.Show();
                }
            }

            return (selectedAlgorithmParamControl != null);
        }

        public void VisionParamControl_AlgorithmValueChanged(ValueChangedType valueChangedType, Algorithm algorithm = null, AlgorithmParam newParam = null, bool modified = true)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_PositionUpdated");

                if (algorithm != null && CommandManager != null)
                {
                    AlgorithmParam oldParam = algorithm.Param.Clone();
                    CommandManager.Execute(new ChangeParameterCommand(algorithm, oldParam, newParam));
                }

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, modified);
            }
        }

        private void inverseResult_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - inverseResult_CheckedChanged");

            foreach (Probe probe in probeList)
            {
                probe.InverseResult = inverseResult.Checked;

                VisionParamControl_AlgorithmValueChanged(ValueChangedType.None);
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == probePosX)
            {
                valueName = StringManager.GetString("Position X");
            }
            else if (sender == probePosY)
            {
                valueName = StringManager.GetString("Position Y");
            }
            else if (sender == probeWidth)
            {
                valueName = StringManager.GetString("Width");
            }
            else if (sender == probeHeight)
            {
                valueName = StringManager.GetString("Height");
            }

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void colorBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - colorBand_SelectedIndexChanged");

            foreach (Probe probe in probeList)
            {
                if (probe is VisionProbe visionProbe)
                {
                    visionProbe.InspAlgorithm.Param.ImageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), imageBand.Text);

                    VisionParamControl_AlgorithmValueChanged(ValueChangedType.ImageProcessing);
                }
            }
        }

        private void probePosX_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probePosX_ValueChanged");

            foreach (Probe probe in probeList)
            {
                probe.X = (int)probePosX.Value;
                VisionParamControl_AlgorithmValueChanged(ValueChangedType.Position);
            }
        }

        private void probePosY_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probePosY_ValueChanged");

            foreach (Probe probe in probeList)
            {
                probe.Y = (int)probePosY.Value;
                VisionParamControl_AlgorithmValueChanged(ValueChangedType.Position);
            }
        }

        private void probePosR_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probePosR_ValueChanged");

            if (onValueUpdate == true)
            {
                return;
            }

            foreach (Probe probe in probeList)
            {
                probe.Angle = (int)probePosR.Value;
                VisionParamControl_AlgorithmValueChanged(ValueChangedType.Position);
            }
        }

        private void probeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probeWidth_ValueChanged");

            foreach (Probe probe in probeList)
            {
                probe.Width = (int)probeWidth.Value;
                VisionParamControl_AlgorithmValueChanged(ValueChangedType.Position);
            }
        }

        private void probeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - probeHeight_ValueChanged");

            foreach (Probe probe in probeList)
            {
                probe.Height = (int)probeHeight.Value;
                VisionParamControl_AlgorithmValueChanged(ValueChangedType.Position);
            }
        }

        private void comboFiducialProbe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - comboFiducialProbe_SelectedIndexChanged");

            foreach (Probe probe in probeList)
            {
                if (comboFiducialProbe.SelectedIndex == 0)
                {
                    probe.FiducialProbe = null;
                }
                else
                {
                    probe.FiducialProbe = (Probe)comboFiducialProbe.SelectedItem;
                }

                VisionParamControl_AlgorithmValueChanged(ValueChangedType.None);
            }
        }

        private void filterListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - ShowAlgorithmParamControl");

            if (selectedFilterParamControl != null)
            {
                ((UserControl)selectedFilterParamControl).Hide();
                selectedFilterParamControl.ClearSelectedFilter();
            }

            var filter = (IFilter)filterListBox.SelectedItem;
            if (filter == null)
            {
                return;
            }

            foreach (IFilterParamControl filterParamControl in filterParamControlList)
            {
                if (filter.GetFilterType() == filterParamControl.GetFilterType())
                {
                    selectedFilterParamControl = filterParamControl;
                    break;
                }
            }

            groupboxFilter.Controls.Clear();
            groupboxFilter.Controls.Add((UserControl)selectedFilterParamControl);

            ((UserControl)selectedFilterParamControl).Show();

            selectedFilterParamControl.AddSelectedFilter(filter);
        }

        private void ButtonDeleteFilter_Click(object sender, EventArgs e)
        {
            var filter = (IFilter)filterListBox.SelectedItem;
            if (filter == null)
            {
                return;
            }

            RemoveFilter(filter);
            UpdateData();
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
            if (selectedAlgorithmParamControl == null)
            {
                return;
            } ((IAlgorithmParamControl)selectedAlgorithmParamControl).PointSelected(clickPos, ref processingCancelled);
        }

        private void AddFilter(IFilter filter)
        {
            foreach (Probe probe in probeList)
            {
                if (probe is VisionProbe visionProbe)
                {
                    visionProbe.InspAlgorithm.FilterList.Add(filter);
                }
            }
            UpdateFilterListBox();
        }

        private void RemoveFilter(IFilter filter)
        {
            foreach (Probe probe in probeList)
            {
                if (probe is VisionProbe visionProbe)
                {
                    visionProbe.InspAlgorithm.FilterList.Remove(filter);
                }
            }
            UpdateFilterListBox();
        }

        private void RemoveFilter(int index)
        {
            foreach (Probe probe in probeList)
            {
                if (probe is VisionProbe visionProbe)
                {
                    visionProbe.InspAlgorithm.FilterList.RemoveAt(index);
                }
            }
            UpdateFilterListBox();
        }

        private void UpdateFilterListBox()
        {
            filterListBox.Items.Clear();

            if (probeList.Count == 0)
            {
                return;
            }

            if (!(probeList[0] is VisionProbe visionProbe))
            {
                return;
            }

            filterListBox.Items.AddRange(visionProbe.InspAlgorithm.FilterList.ToArray());
        }

        private void buttonFilterUp_Click(object sender, EventArgs e)
        {
            int selectedFilterIndex = filterListBox.SelectedIndex;
            if (selectedFilterIndex < 0)
            {
                return;
            }

            int targetFilterIndex = selectedFilterIndex - 1;

            SwapFilter(selectedFilterIndex, targetFilterIndex);
            UpdateData();
        }

        private void buttonFilterDown_Click(object sender, EventArgs e)
        {
            int selectedFilterIndex = filterListBox.SelectedIndex;
            if (selectedFilterIndex < 0)
            {
                return;
            }

            int targetFilterIndex = selectedFilterIndex + 1;

            SwapFilter(selectedFilterIndex, targetFilterIndex);
            UpdateData();
        }

        private void SwapFilter(int selectedFilterIndex, int targetFilterIndex)
        {
            if (probeList.Count == 0)
            {
                return;
            }

            if (!(probeList[0] is VisionProbe visionProbe))
            {
                return;
            }

            List<IFilter> filterList = visionProbe.InspAlgorithm.FilterList;
            if (selectedFilterIndex < 0 || filterList.Count <= selectedFilterIndex
                || targetFilterIndex < 0 || filterList.Count <= targetFilterIndex)
            {
                return;
            }

            IFilter buffer = filterList[selectedFilterIndex];
            filterList[selectedFilterIndex] = filterList[targetFilterIndex];
            filterList[targetFilterIndex] = buffer;

            UpdateFilterListBox();
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            // No need implement
        }

        private void VisionParamControl_Load(object sender, EventArgs e)
        {
            UiManager.Instance().SetupVisionParamControl(this);
        }

        public void UpdateLockMoveState(bool movable)
        {
            probeHeight.Enabled = movable;
            probeWidth.Enabled = movable;
            probePosX.Enabled = movable;
            probePosY.Enabled = movable;
            probePosR.Enabled = movable;
        }

        private void comboBoxLightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - comboFiducialProbe_SelectedIndexChanged");

            foreach (Probe probe in probeList)
            {
                if (probe is VisionProbe visionProbe)
                {
                    visionProbe.LightTypeIndexArr[0] = comboBoxLightType.SelectedIndex;
                }
            }

            VisionParamControl_AlgorithmValueChanged(ValueChangedType.Light);
        }
    }
}
