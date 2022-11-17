using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class CharReaderParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<CharReader> charReaderList = new List<CharReader>();
        private bool onValueUpdate = false;

        public CharReaderParamControl()
        {
            InitializeComponent();

            groupBoxCharactorSize.Text = StringManager.GetString(groupBoxCharactorSize.Text);
            labelWidth.Text = StringManager.GetString(labelWidth.Text);
            labelHeight.Text = StringManager.GetString(labelHeight.Text);
            labelXOverlap.Text = StringManager.GetString(labelXOverlap.Text);
            labelThreshold.Text = StringManager.GetString(labelThreshold.Text);
            labelPolarity.Text = StringManager.GetString(labelPolarity.Text);
            addThresholdButton.Text = StringManager.GetString(addThresholdButton.Text);
            deleteThresholdButton.Text = StringManager.GetString(deleteThresholdButton.Text);
            labelNumCharacter.Text = StringManager.GetString(labelNumCharacter.Text);
            labelDesiredString.Text = StringManager.GetString(labelDesiredString.Text);
            autoTuneButton.Text = StringManager.GetString(autoTuneButton.Text);
            groupBoxFont.Text = StringManager.GetString(groupBoxFont.Text);
            labelFontFile.Text = StringManager.GetString(labelFontFile.Text);
            trainButton.Text = StringManager.GetString(trainButton.Text);
            extractFontButton.Text = StringManager.GetString(extractFontButton.Text);
            showFontbutton.Text = StringManager.GetString(showFontbutton.Text);
            thresholdList.Text = StringManager.GetString(thresholdList.Text);
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            charReaderList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "CharParamControl - SetSelectedProbe");

            probeList.Clear();
            charReaderList.Clear();

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == CharReader.TypeName)
            {
                probeList.AddProbe(visionProbe);
                charReaderList.Add((CharReader)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void UpdateProbeImage()
        {

        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            probeList.Clear();
            charReaderList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                probeList.AddProbe(visionProbe);
                charReaderList.Add((CharReader)visionProbe.InspAlgorithm);
            }

            UpdateData();
        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - UpdateData");

            if (charReaderList.Count == 0)
            {
                return;
            }

            onValueUpdate = true;

            CharReader charReader = charReaderList[0];

            var param = (CharReaderParam)charReader.Param;

            maxHeight.Value = param.CharacterMaxHeight;
            minHeight.Value = param.CharacterMinHeight;
            maxWidth.Value = param.CharacterMaxWidth;
            minWidth.Value = param.CharacterMinWidth;
            xOverlapRatio.Value = param.XOverlapRatio;
            polarity.SelectedIndex = (int)param.CharactorPolarity;
            desiredString.Text = param.DesiredString;
            desiredNumCharacter.Value = param.DesiredNumCharacter;

            fontFileName.Text = Path.GetFileName(param.FontFileName);
            charReader.Train(param.FontFileName);

            RefreshThresholdList();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void minWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - minWidth_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).CharacterMinWidth = (int)minWidth.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void maxWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - maxWidth_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).CharacterMaxWidth = (int)maxWidth.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void minHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - minHeight_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).CharacterMinHeight = (int)minHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void maxHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - maxHeight_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).CharacterMaxHeight = (int)maxHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void polarity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - polarity_SelectedIndexChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).CharactorPolarity = (CharactorPolarity)polarity.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void desiredString_TextChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - desiredString_TextChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).DesiredString = desiredString.Text;
                desiredNumCharacter.Value = desiredString.Text.Length;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void selectFontFile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        foreach (CharReader charReader in charReaderList)
                        {
                            charReader.Train(dlg.FileName);

                            AlgorithmParam newParam = charReader.Param.Clone();
                            ((CharReaderParam)newParam).FontFileName = dlg.FileName;

                            ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
                        }

                        fontFileName.Text = Path.GetFileName(dlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fail to CharReader training." + ex.Message);
                    }
                }
            }
        }

        private void trainButton_Click(object sender, EventArgs e)
        {
            foreach (CharReader charReader in charReaderList)
            {
                var param = (CharReaderParam)charReader.Param;
                charReader.Train(param.FontFileName);
            }
        }

        private void autoTuneButton_Click(object sender, EventArgs e)
        {
            if (desiredString.Text == "")
            {
                MessageBox.Show("Desired String is empty.");
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - autoTuneButton_Click");

            if (charReaderList.Count != 1)
            {
                return;
            }

            Probe probe = probeList[0];
            CharReader charReader = charReaderList[0];

            Target selectedTarget = probe.Target;
            ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                RotatedRect probeRotatedRect = probe.BaseRegion;

                probeRotatedRect.X -= targetRegion.Left;
                probeRotatedRect.Y -= targetRegion.Top;
                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), targetGroupImage, ImageType.Grey, ImageBandType.Luminance);

                charReader.AutoSegmentation(algoImage, probeRotatedRect, desiredString.Text);

                algoImage.Dispose();
            }
            else
            {
                MessageBox.Show(StringManager.GetString("Probe region is invalid."));
            }
        }

        private void xOverlapRatio_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - xOverlapRatio_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).XOverlapRatio = (int)xOverlapRatio.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void extractFontButton_Click(object sender, EventArgs e)
        {
            if (thresholdList.SelectedIndex == -1)
            {
                return;
            }

            int threshold = Convert.ToInt32(thresholdList.SelectedItem.ToString());

            if (charReaderList.Count != 1)
            {
                return;
            }

            Probe probe = probeList[0];
            CharReader charReader = charReaderList[0];

            Target selectedTarget = probe.Target;
            ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                fontGrid.Columns.Clear();
                fontGrid.Rows.Clear();

                RotatedRect probeRegionInFov = probe.BaseRegion;

                ImageD probeClipImage = targetGroupImage.ClipImage(probeRegionInFov);

                var debugContext = new DebugContext(true, BaseConfig.Instance().TempPath);
                AlgorithmResult algorithmResult = charReader.Extract(probeClipImage, probeRegionInFov, probeRegionInFov, threshold, debugContext);

                algorithmResult.Offset(-targetRegion.X, -targetRegion.Y);

                var charReaderResult = (CharReaderResult)algorithmResult;
                List<CharPosition> charPositionList = charReaderResult.CharPositionList;

                if (charPositionList.Count > 0)
                {
                    for (int i = 0; i < charPositionList.Count; i++)
                    {
                        var column = new DataGridViewImageColumn();
                        column.Width = 100;
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        column.ImageLayout = DataGridViewImageCellLayout.Zoom;
                        column.DefaultCellStyle.NullValue = null;
                        column.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
                        fontGrid.Columns.Add(column);
                    }

                    fontGrid.Rows.Add(1);

                    for (int i = 0; i < charPositionList.Count; i++)
                    {
                        RotatedRect position = charPositionList[i].Position;
                        position.Offset(-targetRegion.X, -targetRegion.Y);
                        //ImageD image = targetGroupImage.ClipImage(position);
                        Image image = targetGroupImage.ClipImage(position).ToBitmap();
                        fontGrid.Rows[0].Cells[i].Value = image;
                        fontGrid.Rows[0].Cells[i].Tag = charPositionList[i];
                    }
                }

                if (charReaderResult.IsNG())
                {
                    charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                    string pathName = Path.Combine(BaseConfig.Instance().TempPath, "OcrFailed");
                    var ocrDebugContext = new DebugContext(true, pathName);

                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    DebugHelper.SaveImage(probeClipImage, timeStamp + ".bmp", ocrDebugContext);
                }
            }
        }

        private void fontGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (charReaderList.Count == 0)
            {
                return;
            }

            var form = new InputForm("Charactor");
            if (form.ShowDialog() == DialogResult.OK)
            {
                var charPosition = (CharPosition)fontGrid.Rows[0].Cells[e.ColumnIndex].Tag;

                foreach (CharReader charReader in charReaderList)
                {
                    charReader.AddCharactor(charPosition, form.InputText[0]);
                }

                CharReader reader = charReaderList[0];

                var param = (CharReaderParam)reader.Param;

                if (param.FontFileName == "")
                {
                    var dlg = new SaveFileDialog();
                    if (dlg.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }

                    param.FontFileName = dlg.FileName;
                }

                reader.SaveFontFile(param.FontFileName);
            }
        }

        private void RefreshThresholdList()
        {
            thresholdList.Items.Clear();

            if (charReaderList.Count == 0)
            {
                return;
            }

            CharReader charReader = charReaderList[0];
            var param = (CharReaderParam)charReader.Param;

            foreach (int threshold in param.ThresholdList)
            {
                thresholdList.Items.Add(threshold);
            }
        }

        private void addThresholdButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - addThresholdButton_Click");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).ThresholdList.Add((int)threshold.Value);

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }

            RefreshThresholdList();
        }

        private void DeleteThresholdButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - deleteThresholdButton_Click");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).ThresholdList.Remove((int)threshold.Value);

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }

            RefreshThresholdList();
        }

        private void thresholdList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (thresholdList.SelectedIndex > -1)
            {
                threshold.Value = Convert.ToInt32(thresholdList.Items[thresholdList.SelectedIndex].ToString());
            }
        }

        private void desiredNumCharacter_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - desiredNumCharacter_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).DesiredNumCharacter = (int)desiredNumCharacter.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void showFontButton_Click(object sender, EventArgs e)
        {
            if (charReaderList.Count == 0)
            {
                return;
            }

            CharReader charReader = charReaderList[0];

            var fontView = new FontView();
            fontView.Initialize(charReader);
            fontView.ShowDialog();
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return CharReader.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
