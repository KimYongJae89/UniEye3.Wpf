using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public interface ILightParamForm
    {
        //        LightParamSet LightParamSet { get; }
        LightTypeChangedDelegate LightTypeChanged { get; set; }
        LightValueChangedDelegate LightValueChanged { get; set; }

        void InitControls();
        void SetLightValues(StepModel currentModel, InspectStep curInspectionStep, int cameraIndex);
        void ToggleVisible();
    }

    public delegate void LightTypeChangedDelegate(int lightTypeIndex);
    public delegate void LightValueChangedDelegate(bool imageUpdateRequired);

    public partial class LightParamForm : Form, ILightParamForm
    {
        private StepModel curModel;
        private InspectStep curInspectionStep;
        private int curCameraIndex;
        public LightParamSet LightParamSet { get; set; }

        private bool onValueUpdate = false;
        public LightTypeChangedDelegate LightTypeChanged { get; set; }
        public LightValueChangedDelegate LightValueChanged { get; set; }

        //public int LightTypeIndex
        //{
        //    get { return lightTypeCombo.SelectedIndex; }
        //}

        public LightParamForm()
        {
            InitializeComponent();

            labelLightSource.Text = StringManager.GetString(labelLightSource.Text);
            labelLightType.Text = StringManager.GetString(labelLightType.Text);
            labelExposure.Text = StringManager.GetString(labelExposure.Text);

            applyLightButton.Text = StringManager.GetString(applyLightButton.Text);
            applyAllLightButton.Text = StringManager.GetString(applyAllLightButton.Text);
            advanceButton.Text = StringManager.GetString(advanceButton.Text);

            saveToDefaultToolStripMenuItem.Text = StringManager.GetString(saveToDefaultToolStripMenuItem.Text);
            loadFromDefaultToolStripMenuItem.Text = StringManager.GetString(loadFromDefaultToolStripMenuItem.Text);
        }

        public void Initialize()
        {

        }

        public void InitControls()
        {
            InitLightList();
            UpdateLightTypeCombo(LightConfig.Instance().LightParamSet);
        }

        private void UpdateLightTypeCombo(LightParamSet lightParamSet)
        {
            LogHelper.Debug(LoggerType.Operation, "InitLightTypeList");

            int numLightType = LightConfig.Instance().NumLightType;

            int curLightType = lightTypeCombo.SelectedIndex;
            int curCompositeSrc1 = comboCompositeSrc1.SelectedIndex;
            int curCompositeSrc2 = comboCompositeSrc2.SelectedIndex;

            onValueUpdate = true;

            comboCompositeSrc1.Items.Clear();
            comboCompositeSrc2.Items.Clear();
            lightTypeCombo.Items.Clear();
            for (int i = 0; i < LightConfig.Instance().NumLightType; i++)
            {
                string lightTypeName = lightParamSet[i].Name;
                lightTypeCombo.Items.Add(lightTypeName);

                if (numLightType >= 3)
                {
                    comboCompositeSrc1.Items.Add(lightTypeName);
                    comboCompositeSrc2.Items.Add(lightTypeName);
                }
            }

            lightTypeCombo.SelectedIndex = MathHelper.Bound(curLightType, 0, lightTypeCombo.Items.Count);
            if (comboCompositeSrc1.Items.Count > 0)
            {
                comboCompositeSrc1.SelectedIndex = MathHelper.Bound(curCompositeSrc1, 0, comboCompositeSrc1.Items.Count);
            }

            if (comboCompositeSrc2.Items.Count > 0)
            {
                comboCompositeSrc2.SelectedIndex = MathHelper.Bound(curCompositeSrc2, 0, comboCompositeSrc2.Items.Count);
            }

            onValueUpdate = false;

            if (lightTypeCombo.Items.Count == 1)
            {
                lightTypeCombo.Enabled = false;
            }
        }

        private void InitLightList()
        {
            LogHelper.Debug(LoggerType.Operation, "InitLightList");

            LightParamSet = LightConfig.Instance().LightParamSet;
            string[] lightDeviceNameList = LightConfig.Instance().LightDeviceNameList;

            lightValueGrid.Rows.Clear();
            for (int i = 0; i < lightDeviceNameList.Length; i++)
            {
                lightValueGrid.Rows.Add(lightDeviceNameList[i], 0);
            }
        }

        private void LightParamPanel_Load(object sender, EventArgs e)
        {

        }

        public void SetLightValues(StepModel model, InspectStep inspectStep, int cameraIndex)
        {
            curModel = model;
            curInspectionStep = inspectStep;
            curCameraIndex = cameraIndex;

            LightParamSet = inspectStep.GetLightParamSet().Clone();

            comboLightParamSource.SelectedIndex = (int)inspectStep.LightParamSource;
            UpdateLightTypeCombo(LightParamSet);
            UpdateLightValues();
        }

        private void UpdateLightValues()
        {
            if (curInspectionStep != null)
            {
                comboLightParamSource.SelectedIndex = (int)curInspectionStep.LightParamSource;
            }

            if (LightParamSet.NumLightType <= 0)
            {
                return;
            }

            onValueUpdate = true;

            int lightTypeIndex = lightTypeCombo.SelectedIndex;

            LightParam lightParam = LightParamSet[lightTypeIndex];
            paramTab.SelectedTab = paramTab.Tabs[(int)lightParam.LightParamType];

            if (lightParam.LightParamType == LightParamType.Value)
            {
                if (lightValueGrid.Rows.Count > 0)
                {
                    for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                    {
                        lightValueGrid.Rows[i].Cells[1].Value = lightParam.LightValue.Value[i];
                    }
                }

                exposureTimeMs.Value = curModel.ExposureTimeUsList[curCameraIndex] / 1000;
                if (exposureTimeMs.Value == 0)
                {
                    exposureTimeMs.Value = DeviceConfig.Instance().DefaultExposureTimeMs;
                }
            }
            else
            {
                comboCompositeSrc1.SelectedIndex = lightParam.FirstImageIndex;
                comboCompositeSrc2.SelectedIndex = lightParam.SecondImageIndex;
                comboCompositeType.SelectedIndex = (int)lightParam.OperationType;
            }

            onValueUpdate = false;
        }

        private void comboLightParamSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            curInspectionStep.LightParamSource = (LightParamSource)comboLightParamSource.SelectedIndex;

            LightParamSet lightParamSet = curInspectionStep.GetLightParamSet().Clone();
            UpdateLightTypeCombo(lightParamSet);
            UpdateLightValues();
            LightTypeChanged?.Invoke(lightTypeCombo.SelectedIndex);
        }

        public void SetLightTypeIndex(int lightTypeIndex)
        {
            lightTypeCombo.SelectedIndex = lightTypeIndex;
        }

        private void lightTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            UpdateLightValues();

            LightTypeChanged?.Invoke(lightTypeCombo.SelectedIndex);
        }

        private void applyLightButton_Click(object sender, EventArgs e)
        {
            bool ok = ApplyLight();
            if (!ok)
            {
                string message = "Light param is invailed";
                MessageBox.Show(message);
                return;
            }

            UpdateLightTypeCombo(LightParamSet);

            //if (curTargetGroup == null)
            //    return;

            //LightParamSource lightParamSource = (LightParamSource)comboLightParamSource.SelectedIndex;
            //curTargetGroup.LightParamSource = lightParamSource;


            //switch (lightParamSource)
            //{
            //    case LightParamSource.Model:
            //        curModel.LightParamSet = lightParamSet.Clone();
            //        break;
            //    case LightParamSource.InspectionStep:
            //        curInspectionStep.LightParamSet = lightParamSet.Clone();
            //        break;
            //    case LightParamSource.TargetGroup:
            //        curTargetGroup.LightParamSet = lightParamSet.Clone();
            //        break;
            //}

            LightValueChanged?.Invoke(true);
        }

        private void applyAllLightButton_Click(object sender, EventArgs e)
        {
            bool ok = ApplyLight();
            if (!ok)
            {
                string message = StringManager.GetString("Light param is invailed");
                MessageForm.Show(ParentForm, message);
                return;
            }

            string message2 = StringManager.GetString("Apply this light parameter into every step?");
            DialogResult res = MessageForm.Show(ParentForm, message2, MessageFormType.YesNo);
            if (res == DialogResult.No)
            {
                return;
            }

            List<InspectStep> inspectStepList = curModel.GetInspectStepList();
            foreach (InspectStep inspectStep in inspectStepList)
            {
                if (inspectStep.LightParamSource == LightParamSource.InspectionStep)
                {
                    inspectStep.LightParamSet = LightParamSet.Clone();
                }
            }

            LightValueChanged?.Invoke(false);
        }

        private bool ApplyLight()
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - exposureTime_ValueChanged");

            try
            {
                int lightTypeIndex = lightTypeCombo.SelectedIndex;

                LightParam lightParam = LightParamSet[lightTypeIndex];
                lightParam.Name = lightTypeCombo.Text;
                lightParam.LightParamType = (LightParamType)paramTab.ActiveTab.Index;

                curModel.ExposureTimeUsList[curCameraIndex] = Convert.ToInt32(exposureTimeMs.Value * 1000);

                if (lightParam.LightParamType == LightParamType.Value)
                {
                    string[] lightDeviceNameList = LightConfig.Instance().LightDeviceNameList;
                    for (int i = 0; i < lightValueGrid.Rows.Count; i++)
                    {
                        lightParam.LightValue.Value[i] = Convert.ToInt32(lightValueGrid.Rows[i].Cells[1].Value);
                        lightDeviceNameList[i] = (string)lightValueGrid.Rows[i].Cells[0].Value;
                    }
                }
                else
                {
                    if (comboCompositeSrc1.SelectedIndex < 0
                        || comboCompositeSrc2.SelectedIndex < 0
                        || lightTypeIndex == lightParam.FirstImageIndex || lightTypeIndex == lightParam.SecondImageIndex)
                    {
                        return false;
                    }
                    lightParam.FirstImageIndex = comboCompositeSrc1.SelectedIndex;
                    lightParam.SecondImageIndex = comboCompositeSrc2.SelectedIndex;
                    lightParam.OperationType = (ImageOperationType)comboCompositeType.SelectedIndex;
                }

                if (curInspectionStep != null)
                {
                    curInspectionStep.LightParamSource = (LightParamSource)comboLightParamSource.SelectedIndex;
                    curInspectionStep.SetLightParamSet(LightParamSet.Clone());
                }

                LightConfig.Instance().LightParamSet = LightParamSet.Clone();
                LightConfig.Instance().Save();

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        private void editTypeNameButton_Click(object sender, EventArgs e)
        {
            var form = new InputForm(StringManager.GetString("Edit Light Type Name"), lightTypeCombo.Items[lightTypeCombo.SelectedIndex].ToString());
            form.StartPosition = FormStartPosition.CenterParent;
            form.TopLevel = true;
            form.TopMost = true;

            if (form.ShowDialog() == DialogResult.OK)
            {
                LightParamSet[lightTypeCombo.SelectedIndex].Name = form.InputText;
                UpdateLightTypeCombo(LightParamSet);
                ApplyLight();
                //lightTypeCombo.Items[lightTypeCombo.SelectedIndex] = form.InputText;
                //UpdateLightTypeCombo(this.lightParamSet);
            }
        }

        private void panelValue_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LightParamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;
        }

        private void comboComposite_SelectedIndexChanged(object sender, EventArgs e)
        {
            int lightTypeIndex = lightTypeCombo.SelectedIndex;

            if (LightParamSet[lightTypeIndex].LightParamType == LightParamType.Calculation)
            {
                var comboBox = (ComboBox)sender;
                if (comboBox.SelectedIndex == lightTypeCombo.SelectedIndex)
                {
                    string message = "Can not select itself.";
                    MessageBox.Show(message);
                    comboBox.SelectedIndex = -1;
                }
            }
        }

        private void advanceButton_Click(object sender, EventArgs e)
        {
            //Point pt = this.PointToScreen(new Point(advanceButton.Bounds.Left, advanceButton.Bounds.Bottom));
            //advancedContextMenuStrip.Show(pt);

            advancedContextMenuStrip.Show(Cursor.Position);
        }

        private void saveToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = StringManager.GetString("Save parameters to default setting?");
            DialogResult res = MessageForm.Show(ParentForm, message, MessageFormType.YesNo);
            if (res == DialogResult.Yes)
            {
                LightConfig.Instance().LightParamSet = LightParamSet.Clone();
                LightConfig.Instance().Save();
            }
        }

        private void loadFromDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = StringManager.GetString("Load parameters from default setting?");
            DialogResult res = MessageForm.Show(ParentForm, message, MessageFormType.YesNo);
            if (res == DialogResult.Yes)
            {
                LightParamSet = LightConfig.Instance().LightParamSet.Clone();

                UpdateLightTypeCombo(LightParamSet);
                UpdateLightValues();
            }
        }

        public void ToggleVisible()
        {
            Visible = !Visible;
        }
    }
}
