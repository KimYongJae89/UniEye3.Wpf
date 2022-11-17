using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class PatternMatchingParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<PatternMatching> patternMatchingList = new List<PatternMatching>();
        private bool onValueUpdate = false;

        public PatternMatchingParamControl()
        {
            InitializeComponent();

            // language change
            labelSize.Text = StringManager.GetString(labelSize.Text);
            labelScore.Text = StringManager.GetString(labelScore.Text);
            labelW.Text = StringManager.GetString(labelW.Text);
            labelH.Text = StringManager.GetString(labelH.Text);
            addPatternButton.Text = StringManager.GetString(addPatternButton.Text);
            deletePatternButton.Text = StringManager.GetString(deletePatternButton.Text);
            refreshPatternButton.Text = StringManager.GetString(refreshPatternButton.Text);
            editMaskButton.Text = StringManager.GetString(editMaskButton.Text);
            ColumnPatternImage.HeaderText = StringManager.GetString(ColumnPatternImage.HeaderText);
            labelAngle.Text = StringManager.GetString(labelAngle.Text);
            labelAngleMin.Text = StringManager.GetString(labelAngleMin.Text);
            labelAngleMax.Text = StringManager.GetString(labelAngleMax.Text);
            labelScale.Text = StringManager.GetString(labelScale.Text);
            labelScaleMax.Text = StringManager.GetString(labelScaleMax.Text);
            labelScaleMin.Text = StringManager.GetString(labelScaleMin.Text);
            useWholeImage.Text = StringManager.GetString(useWholeImage.Text);
            useAllMatching.Text = StringManager.GetString(useAllMatching.Text);
            centerOffset.Text = StringManager.GetString(centerOffset.Text);
            labelW.Text = StringManager.GetString(labelW.Text);
            labelH.Text = StringManager.GetString(labelH.Text);

            AlgorithmStrategy algorithmStrategy = AlgorithmFactory.Instance().GetStrategy(PatternMatching.TypeName);
            try
            {
                if (algorithmStrategy.LibraryType != ImagingLibrary.CognexVisionPro)
                {
                    labelAngle.Visible = false;
                    minAngle.Visible = false;
                    maxAngle.Visible = false;
                    labelAngleMin.Visible = false;
                    labelAngleMax.Visible = false;
                    labelScale.Visible = false;
                    labelScaleMax.Visible = false;
                    labelScaleMin.Visible = false;
                    minScale.Visible = false;
                    maxScale.Visible = false;
                }
            }
            catch (NullReferenceException)
            {
                LogHelper.Debug(LoggerType.StartUp, "LibraryType is null. check your algorithm license key.");
                MessageBox.Show(StringManager.GetString("LibraryType is null. Please check your algorithm license key."));
            }

            var newCollection = new List<object>();
            foreach (object item in patternType.Items)
            {
                newCollection.Add(StringManager.GetString(item.ToString()));
            }
            patternType.Items.Clear();
            patternType.Items.AddRange(newCollection.ToArray());

            patternImageSelector.RowTemplate.Height = patternImageSelector.Height - 20;
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            patternMatchingList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - SetSelectedProbe");

            var visionProbe = (VisionProbe)probe;
            probeList.AddProbe(visionProbe);
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == PatternMatching.TypeName)
            {
                patternMatchingList.Add((PatternMatching)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            probeList.Clear();
            patternMatchingList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                probeList.AddProbe(visionProbe);
                patternMatchingList.Add((PatternMatching)visionProbe.InspAlgorithm);
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdatePatternImageSelector()
        {
            patternImageSelector.Rows.Clear();

            bool controlEnable = (patternMatchingList.Count == 1);

            addPatternButton.Enabled = controlEnable;
            deletePatternButton.Enabled = controlEnable;
            refreshPatternButton.Enabled = controlEnable;
            editMaskButton.Enabled = controlEnable;
            patternType.Enabled = controlEnable;

            if (controlEnable == false)
            {
                return;
            }

            PatternMatching patternMatching = patternMatchingList[0];

            var patternMatchingParam = (PatternMatchingParam)patternMatching.Param;//.Clone();

            foreach (Pattern pattern in patternMatchingParam.PatternList)
            {
                //ImageD patternImage = pattern.GetMaskedImage();
                ImageD patternImage = pattern.PatternImage;
                int index = patternImageSelector.Rows.Add(patternImage.ToBitmap());
#if DEBUG
                //patternImage.SaveImage(string.Format("{0}\\{1}.bmp", BaseConfig.Instance().TempPath, "patternImage"), ImageFormat.Bmp);
                //pattern.PatternImage.SaveImage(string.Format("{0}\\{1}.bmp", BaseConfig.Instance().TempPath, "patternImage2"), ImageFormat.Bmp);
#endif
                patternImageSelector.Rows[index].Tag = pattern;


                patternImageSelector.Rows[index].Height = patternImageSelector.Rows[index].Cells[0].ContentBounds.Height;
                if (patternImageSelector.Rows[index].Height > patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight)
                {
                    patternImageSelector.Rows[index].Height = (patternImageSelector.Height - patternImageSelector.ColumnHeadersHeight);
                }
            }

            if (patternImageSelector.Rows.Count > 0)
            {
                patternImageSelector.Rows[0].Selected = true;
                patternType.SelectedIndex = (int)((Pattern)patternImageSelector.Rows[0].Tag).PatternType;
            }
        }

        private void UpdateData()
        {
            if (patternMatchingList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - UpdateData");

            onValueUpdate = true;

            UpdatePatternImageSelector();

            searchRangeWidth.Text = probeList.GetParamValueStr("SearchRangeWidth");
            searchRangeHeight.Text = probeList.GetParamValueStr("SearchRangeHeight");

            numToleranceX.Text = probeList.GetParamValueStr("ToleranceX");
            numToleranceY.Text = probeList.GetParamValueStr("ToleranceY");

            minAngle.Text = probeList.GetParamValueStr("MinAngle");
            maxAngle.Text = probeList.GetParamValueStr("MaxAngle");
            minScale.Text = probeList.GetParamValueStr("MinScale");
            maxScale.Text = probeList.GetParamValueStr("MaxScale");

            matchScore.Text = probeList.GetParamValueStr("MatchScore");
            numToFind.Text = probeList.GetParamValueStr("NumToFind");

            useWholeImage.CheckState = probeList.GetCheckState("UseWholeImage");
            useAllMatching.CheckState = probeList.GetCheckState("UseAllMatching");
            centerOffset.CheckState = probeList.GetCheckState("UseImageCenter");

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - VisionParamControl_PositionUpdated");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - searchRangeWidth_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).SearchRangeWidth = (int)searchRangeWidth.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, patternMatching, newParam);
            }
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - searchRangeHeight_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).SearchRangeHeight = (int)searchRangeHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, patternMatching, newParam);
            }
        }

        private void matchScore_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - matchScore_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).MatchScore = (int)matchScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void numToFind_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - numToFind_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).NumToFind = (int)numToFind.Value;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == searchRangeHeight)
            {
                valueName = StringManager.GetString("Search Range Height");
            }
            else if (sender == searchRangeWidth)
            {
                valueName = StringManager.GetString("Search Range Width");
            }
            else if (sender == matchScore)
            {
                valueName = StringManager.GetString("Match Score");
            }

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void addPatternButton_Click(object sender, EventArgs e)
        {
            if (patternMatchingList.Count != 1)
            {
                return;
            }

            PatternMatching patternMatching = patternMatchingList[0];

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - addPatternButton_Click");

            AlgorithmParam newParam = patternMatching.Param.Clone();

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            AddPattern((PatternMatchingParam)newParam);

            UpdatePatternImageSelector();
        }

        private void AddPattern(PatternMatchingParam patternMatchingParam)
        {
            if (probeList.Count != 1)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - AddPattern");

            Probe probe = probeList[0];
            PatternMatching patternMatching = patternMatchingList[0];

            Target selectedTarget = probe.Target;
            ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;

            var imageRegion = new RectangleF(0, 0, targetGroupImage.Width, targetGroupImage.Height);

            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, imageRegion))
            {
                RotatedRect probeRotatedRect = probe.BaseRegion;

                ImageD clipImage = targetGroupImage.ClipImage(probeRotatedRect);

                ImageD filterredImage = patternMatching.Filter(clipImage, 0);

                Pattern pattern = patternMatchingParam.AddPattern((Image2D)filterredImage);

                patternType.SelectedIndex = (int)pattern.PatternType;
            }
            else
            {
                MessageBox.Show(StringManager.GetString("Probe region is invalid."));
            }
        }

        private void DeletePatternButton_Click(object sender, EventArgs e)
        {
            if (patternMatchingList.Count != 1)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - deletePatternButton_Click");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    var pattern = (Pattern)patternImageSelector.Rows[index].Tag;

                    PatternMatching patternMatching = patternMatchingList[0];

                    var newParam = (PatternMatchingParam)patternMatching.Param.Clone();
                    newParam.RemovePattern(index);

                    ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);

                    patternImageSelector.Rows.RemoveAt(index);
                }
            }
        }

        private void refreshPatternButton_Click(object sender, EventArgs e)
        {
            if (patternMatchingList.Count != 1)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - refreshPatternButton_Click");

            patternImageSelector.Rows.Clear();

            PatternMatching patternMatching = patternMatchingList[0];

            var newParam = (PatternMatchingParam)patternMatching.Param.Clone();

            ((PatternMatchingParam)patternMatching.Param).RemoveAllPatterns();
            newParam.RemoveAllPatterns();
            AddPattern(newParam);

            ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);

            UpdatePatternImageSelector();
        }

        private void editMaskButton_Click(object sender, EventArgs e)
        {
            if (patternMatchingList.Count != 1)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - editMaskButton_Click");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    PatternMatching patternMatching = patternMatchingList[0];

                    var newParam = (PatternMatchingParam)patternMatching.Param.Clone();

                    Pattern pattern = newParam.GetPattern(index);

                    var maskEditor = new MaskEditor();
                    maskEditor.SetImage(pattern.PatternImage);
                    maskEditor.SetMaskFigures(pattern.MaskFigures);
                    if (maskEditor.ShowDialog(this) == DialogResult.OK)
                    {
                        pattern.UpdateMaskImage();
                        patternImageSelector.Rows[index].Cells[0].Value = pattern.GetMaskedImage();

                        ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
                    }
                }
            }
        }

        private void patternImageSelector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - patternImageSelector_CellClick");

            if (e.RowIndex > -1)
            {
                var pattern = (Pattern)patternImageSelector.Rows[e.RowIndex].Tag;
                if (pattern == null)
                {
                    LogHelper.Error("PatternMatchingParamControl - pattern image is null.");
                    return;
                }

                patternType.SelectedIndex = (int)pattern.PatternType;
            }
        }

        private void patternType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - patternType_SelectedIndexChanged");

            if (patternImageSelector.SelectedRows.Count > 0)
            {
                int index = patternImageSelector.SelectedRows[0].Index;
                if (index > -1)
                {
                    foreach (PatternMatching patternMatching in patternMatchingList)
                    {
                        var newParam = (PatternMatchingParam)patternMatching.Param.Clone();

                        Pattern pattern = newParam.GetPattern(index);
                        if (pattern == null)
                        {
                            LogHelper.Error("PatternMatchingParamControl - pattern image is null.");
                            return;
                        }

                        pattern.PatternType = (PatternType)patternType.SelectedIndex;

                        ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
                    }
                }
            }
        }

        private void minAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - minAngle_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).MinAngle = (int)minAngle.Value;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void maxAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - maxAngle_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).MaxAngle = (int)maxAngle.Value;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void minScale_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - minScale_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).MinScale = (float)minScale.Value;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void maxScale_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - maxScale_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).MaxScale = (float)maxScale.Value;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void centerOffset_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - minScale_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).UseImageCenter = centerOffset.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void useWholeImage_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).UseWholeImage = useWholeImage.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        private void useAllMatching_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).UseAllMatching = useAllMatching.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, patternMatching, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return PatternMatching.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }

        private void numToleranceY_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - numToleranceY_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).ToleranceY = (int)numToleranceY.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, patternMatching, newParam);
            }
        }

        private void numToleranceX_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "PatternMatchingParamControl - numToleranceX_ValueChanged");

            foreach (PatternMatching patternMatching in patternMatchingList)
            {
                AlgorithmParam newParam = patternMatching.Param.Clone();
                ((PatternMatchingParam)newParam).ToleranceX = (int)numToleranceX.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, patternMatching, newParam);
            }
        }
    }
}
