using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class MilCharReaderParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<CharReader> charReaderList = new List<CharReader>();
        public Image2D TargetGroupImage { get; set; }

        private bool onValueUpdate = false;

        public MilCharReaderParamControl()
        {
            InitializeComponent();

            //change language
            labelDesiredString.Text = StringManager.GetString(labelDesiredString.Text);
            labelFontFileName.Text = StringManager.GetString(labelFontFileName.Text);
            labelNumCharacter.Text = StringManager.GetString(labelNumCharacter.Text);
            labelMinScore.Text = StringManager.GetString(labelMinScore.Text);
            groupBoxFont.Text = StringManager.GetString(groupBoxFont.Text);
            labelStringCalibration.Text = StringManager.GetString(labelStringCalibration.Text);
            CalibrationButton.Text = StringManager.GetString(CalibrationButton.Text);
            extractFontButton.Text = StringManager.GetString(extractFontButton.Text);
            showFontbutton.Text = StringManager.GetString(showFontbutton.Text);
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            charReaderList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "MilCharReaderParamControl - SetSelectedProbe");

            probeList.Clear();
            charReaderList.Clear();

            var visionProbe = (VisionProbe)probe;
            probeList.AddProbe(visionProbe);
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == CharReader.TypeName)
            {
                charReaderList.Add((CharReader)visionProbe.InspAlgorithm);
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
            charReaderList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                probeList.AddProbe(visionProbe);
                charReaderList.Add((CharReader)visionProbe.InspAlgorithm);
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "MilCharReaderParamControl - UpdateData");

            if (charReaderList.Count == 0)
            {
                return;
            }

            onValueUpdate = true;

            CharReader charReader = charReaderList[0];

            var param = (CharReaderParam)charReader.Param;

            fontFileName.Text = Path.GetFileName(param.FontFileName);
            desiredString.Text = param.DesiredString;
            minScore.Value = (int)param.MinScore;
            desiredNumCharacter.Value = param.DesiredNumCharacter;

            calibrationString.Clear();
            fontGrid.Columns.Clear();
            fontGrid.Rows.Clear();

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

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }

            desiredNumCharacter.Value = desiredString.Text.Length;
        }

        private void selectFontFile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Mil Font File (*.mfo) | *.mfo";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    foreach (CharReader charReader in charReaderList)
                    {
                        var param = (CharReaderParam)charReader.Param;

                        AlgorithmParam newParam = charReader.Param.Clone();
                        ((CharReaderParam)newParam).FontFileName = dlg.FileName;

                        ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
                    }

                    fontFileName.Text = dlg.SafeFileName;
                }
            }
        }

        private void minScore_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CharReaderParamControl - minScore_ValueChanged");

            foreach (CharReader charReader in charReaderList)
            {
                AlgorithmParam newParam = charReader.Param.Clone();
                ((CharReaderParam)newParam).MinScore = (float)minScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, charReader, newParam);
            }
        }

        private void ShowFontList()
        {
            if (charReaderList.Count != 1)
            {
                return;
            }

            Probe probe = probeList[0];
            CharReader charReader = charReaderList[0];

            Target selectedTarget = probe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                fontGrid.Columns.Clear();
                fontGrid.Rows.Clear();

                RotatedRect probeRegionInFov = probe.BaseRegion;

                ImageD clipImage = TargetGroupImage.ClipImage(probeRegionInFov);

                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), clipImage, ImageType.Grey, ImageBandType.Luminance);

                var clipRect = new RectangleF(0, 0, probeRegionInFov.Width, probeRegionInFov.Height);

                var debugContext = new DebugContext(false, "");
                AlgorithmResult algorithmResult = charReader.Read(algoImage, clipRect, debugContext);

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
                        ImageD charImage = TargetGroupImage.ClipImage(position);
                        fontGrid.Rows[0].Cells[i].Value = charImage.ToBitmap();
                        fontGrid.Rows[0].Cells[i].Tag = charPositionList[i];
                    }
                }

                if (charReaderResult.IsNG())
                {
                    charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                    string pathName = Path.Combine(BaseConfig.Instance().TempPath, "OcrFailed");
                    var ocrDebugContext = new DebugContext(true, pathName);

                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    algoImage.Save(timeStamp + ".bmp", ocrDebugContext);

                    clipImage.SaveImage(Path.Combine(pathName, timeStamp + "_0.bmp"), ImageFormat.Bmp);
                }

                algoImage.Dispose();
            }
        }

        private void showFontbutton_Click(object sender, EventArgs e)
        {
            if (charReaderList.Count != 1)
            {
                return;
            }

            CharReader charReader = charReaderList[0];

            var fontView = new FontView();
            fontView.Initialize(charReader);
            fontView.ShowDialog();
        }

        private void extractFontButton_Click(object sender, EventArgs e)
        {
            if (charReaderList.Count != 1)
            {
                return;
            }

            Probe probe = probeList[0];
            CharReader charReader = charReaderList[0];

            Target selectedTarget = probe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                fontGrid.Columns.Clear();
                fontGrid.Rows.Clear();

                RotatedRect probeRegionInFov = probe.BaseRegion;

                ImageD probeClipImage = TargetGroupImage.ClipImage(probeRegionInFov);

                AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), probeClipImage, ImageType.Grey, ImageBandType.Luminance);

                var clipRect = new RectangleF(0, 0, probeRegionInFov.Width, probeRegionInFov.Height);
                var debugContext = new DebugContext(true, BaseConfig.Instance().TempPath);
                AlgorithmResult algorithmResult = charReader.Extract(algoImage, clipRect, 0, debugContext);

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
                        ImageD charImage = TargetGroupImage.ClipImage(position);
                        fontGrid.Rows[0].Cells[i].Value = charImage.ToBitmap();
                        fontGrid.Rows[0].Cells[i].Tag = charPositionList[i];
                    }
                }

                if (charReaderResult.IsNG())
                {
                    charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                    string pathName = Path.Combine(BaseConfig.Instance().TempPath, "OcrFailed");
                    var ocrDebugContext = new DebugContext(true, pathName);

                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    algoImage.Save(timeStamp + ".bmp", ocrDebugContext);
                }

                algoImage.Dispose();
            }
        }

        private void fontGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var form = new InputForm("Charactor");
            if (form.ShowDialog() == DialogResult.OK)
            {
                var charImage = (Bitmap)fontGrid.Rows[0].Cells[e.ColumnIndex].Value;

                foreach (CharReader charReader in charReaderList)
                {
                    AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), Image2D.ToImage2D(charImage), ImageType.Grey, ImageBandType.Luminance);
                    charReader.AddCharactor(algoImage, form.InputText[0].ToString());
                    algoImage.Dispose();
                }
            }
        }

        private void calibrationButton_Click(object sender, EventArgs e)
        {
            if (charReaderList.Count != 1)
            {
                return;
            }

            Probe probe = probeList[0];
            CharReader charReader = charReaderList[0];
            var param = (CharReaderParam)charReader.Param;

            if (File.Exists(param.FontFileName))
            {
                Target selectedTarget = probe.Target;

                RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
                RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
                if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
                {
                    fontGrid.Columns.Clear();
                    fontGrid.Rows.Clear();

                    RotatedRect probeRegionInFov = probe.BaseRegion;

                    var searchRegionInFov = new RotatedRect(probeRegionInFov);

                    RectangleF imageRegionInFov = searchRegionInFov.GetBoundRect();

                    RectangleF imageClipRegion = imageRegionInFov;
                    imageClipRegion.X -= targetRegion.Left;
                    imageClipRegion.Y -= targetRegion.Top;

                    ImageD probeClipImage = TargetGroupImage.ClipImage(Rectangle.Ceiling(imageClipRegion));

                    AlgoImage algoImage = ImageBuilder.Build(charReader.GetAlgorithmType(), probeClipImage, ImageType.Grey, ImageBandType.Luminance);

                    charReader.Filter(algoImage);
                    int count = charReader.CalibrateFont(algoImage, calibrationString.Text);

                    algoImage.Dispose();

                    if (calibrationString.Text.Count() == count)
                    {
                        MessageBox.Show(string.Format("Success!! (Desired Calibration String Num : {0}, Blob String Num : {1})", calibrationString.Text.Length, count));
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Fail.. (Desired Calibration String Num : {0}, Blob String Num : {1})", calibrationString.Text.Length, count));
                    }
                }
            }
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