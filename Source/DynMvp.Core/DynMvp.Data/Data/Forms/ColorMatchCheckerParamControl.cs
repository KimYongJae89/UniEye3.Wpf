using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Data.Forms
{
    public partial class ColorMatchCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<ColorMatchChecker> colorMatchCheckerList = new List<ColorMatchChecker>();
        private Image2D targetImage = null;
        public Image2D TargetImage
        {
            set => targetImage = value;
        }

        private bool onValueUpdate = false;

        public ColorMatchCheckerParamControl()
        {
            InitializeComponent();

            buttonAdd.Text = StringManager.GetString(buttonAdd.Text);
            buttonDelete.Text = StringManager.GetString(buttonDelete.Text);
            buttonReset.Text = StringManager.GetString(buttonReset.Text);
            labelScore.Text = StringManager.GetString(labelScore.Text);
            labelSmoothing.Text = StringManager.GetString(labelSmoothing.Text);
            labelMatchColor.Text = StringManager.GetString(labelMatchColor.Text);
            checkUseColorSet.Text = StringManager.GetString(checkUseColorSet.Text);
            buttonSave.Text = StringManager.GetString(buttonSave.Text);

        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            colorMatchCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - SetSelectedProbe");

            probeList.Clear();
            colorMatchCheckerList.Clear();

            var visionProbe = (VisionProbe)probe;

            probeList.AddProbe(visionProbe);
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == ColorMatchChecker.TypeName)
            {
                colorMatchCheckerList.Add((ColorMatchChecker)visionProbe.InspAlgorithm);

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
            colorMatchCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                probeList.AddProbe(visionProbe);
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == ColorMatchChecker.TypeName)
                {
                    colorMatchCheckerList.Add((ColorMatchChecker)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (colorMatchCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - UpdateData");

            onValueUpdate = true;

            var colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchCheckerList[0].Param;

            matchScore.Value = colorMatchCheckerParam.MatchScore;
            matchColor.Text = colorMatchCheckerParam.MatchColor;

            UpdateGirdData();

            onValueUpdate = false;
        }


        //public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        //{
        //    if (onValueUpdate == false)
        //    {
        //        LogHelper.Debug(LoggerType.OpDebug, "ColorMatchCheckerParamControl - ParamControl_ValueChanged");

        //        if (ValueChanged != null)
        //            ValueChanged(valueChangedType, true);
        //    }
        //}

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "ColorCheckerParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void AddColorPattern()
        {
            if (probeList.Count != 1)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "ColorMatchParamControl - AddPattern");

            Probe probe = probeList[0];
            Target selectedTarget = probe.Target;

            RectangleF targetRegion = selectedTarget.BaseRegion.GetBoundRect();
            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, targetRegion))
            {
                ColorMatchChecker colorMatchChecker = colorMatchCheckerList[0];

                RotatedRect probeRotatedRect = probe.BaseRegion;

                probeRotatedRect.X -= targetRegion.Left;
                probeRotatedRect.Y -= targetRegion.Top;

                var clipImage = (Image2D)targetImage.ClipImage(probeRotatedRect);

                AlgoImage algoImage = ImageBuilder.Build(colorMatchChecker.GetAlgorithmType(), clipImage, ImageType.Color, ImageBandType.Luminance);

                string tempColorName = "";
                tempColorName = GetColorName();
                if (tempColorName == "")
                {
                    return;
                }

                if (string.IsNullOrEmpty(tempColorName))
                {
                    return;
                }

                var colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchChecker.Param;

                ColorPattern colorPattern = colorMatchCheckerParam.AddColorPattern(tempColorName, clipImage);
                colorPattern.Image = clipImage;
                ((ColorMatchCheckerParam)colorMatchChecker.Param).Smoothing = Convert.ToInt32(txtSmoothing.Value);
                colorMatchChecker.Train();

                colorPatternGrid.Rows.Add(colorPattern.Name, colorPattern.Image);
                colorPatternGrid.Rows[0].Tag = colorPattern;
                colorPatternGrid.Rows[0].Height = Math.Min(colorPattern.Image.Height, colorPatternGrid.RowTemplate.Height);
                colorPatternGrid.Rows[0].Selected = true;
            }
            else
            {
                MessageBox.Show(StringManager.GetString("Probe region is invalid."));
            }
        }
        private string GetColorName()
        {
            string name;

            var form = new InputForm("Input Color Name");
            var point = new Point(Screen.PrimaryScreen.Bounds.Width - Screen.PrimaryScreen.Bounds.Width / 4, Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.Bounds.Height / 4);
            form.ChangeLocation(point);

            if (form.ShowDialog(this) == DialogResult.OK && form.InputText != "")
            {
                name = form.InputText;
                for (int i = 0; i < colorPatternGrid.Rows.Count; i++)
                {
                    if (colorPatternGrid.Rows[i].Cells[0].Value.ToString() == form.InputText)
                    {
                        MessageBox.Show("The colorname is already exist.");
                        name = "";
                    }
                }
            }
            else
            {
                name = "";
            }

            return name;
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddColorPattern();
            UpdateData();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (colorMatchCheckerList.Count != 1)
            {
                return;
            }

            if (colorPatternGrid.SelectedRows.Count > 0)
            {
                ColorMatchChecker colorMatchChecker = colorMatchCheckerList[0];

                string deleteColorName = colorPatternGrid.SelectedRows[0].Cells[0].Value.ToString();
                colorPatternGrid.Rows.Remove(colorPatternGrid.SelectedRows[0]);

                var colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchChecker.Param;

                colorMatchCheckerParam.DeleteColorPattern(deleteColorName);
            }

            colorPatternGrid.Refresh();
            UpdateData();
        }

        private void buttonSelectColorPatternFile_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                colorPatternGrid.Controls.Clear();
            }
        }

        private void matchScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - matchScore_ValueChanged");

            foreach (ColorMatchChecker colorMatchChecker in colorMatchCheckerList)
            {
                var colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchChecker.Param;

                AlgorithmParam newParam = colorMatchCheckerParam.Clone();
                ((ColorMatchCheckerParam)newParam).MatchScore = (int)matchScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
            }
        }

        private void matchColor_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - matchColor_TextChanged");

            foreach (ColorMatchChecker colorMatchChecker in colorMatchCheckerList)
            {
                var colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchChecker.Param;

                AlgorithmParam newParam = colorMatchCheckerParam.Clone();
                ((ColorMatchCheckerParam)newParam).MatchColor = matchColor.Text;

                ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
            }
        }

        private void useColorPatternFile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void colorPatternGrid_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void ColorMatchCheckerParamControl_Load(object sender, EventArgs e)
        {
            if (colorMatchCheckerList.Count != 1)
            {
                return;
            }

            ColorMatchChecker colorMatchChecker = colorMatchCheckerList[0];

            if (colorMatchChecker != null)
            {
                UpdateData();
            }
        }

        private void colorPatternGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (colorPatternGrid.SelectedRows.Count > 0)
            {
                DeleteDuplicateMatchColor();
            }
            UpdateData();
        }

        private void DeleteDuplicateMatchColor()
        {
            string addTargetName = colorPatternGrid.SelectedRows[0].Cells[0].Value.ToString();
            if (string.IsNullOrEmpty(addTargetName) == false)
            {
                string[] text = matchColor.Text.Split(';');
                for (int i = 0; i < text.Length; i++)
                {
                    if (addTargetName == text[i])
                    {
                        MessageBox.Show("Alreay added target name");
                        return;
                    }

                }
                matchColor.Text += addTargetName + ';';
            }
        }

        private void comboIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colorFolderPath = string.Format(@"{0}\..\Config", Environment.CurrentDirectory);
            if (comboIndex.SelectedIndex == 0 || checkUseColorSet.Checked == false)
            {
                return;
            }
            else
            {
                string fileName = string.Format(@"{0}\ColorSet{1}.xml", colorFolderPath, comboIndex.SelectedItem.ToString());

                if (File.Exists(fileName))
                {
                    if (MessageBox.Show("Do you want to change the color list?", "Caution", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        LoadData(fileName);
                        UpdateGirdData();
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void LoadData(string fileName)
        {
            if (colorMatchCheckerList.Count != 1)
            {
                return;
            }

            ColorMatchChecker ColorMatchChecker = colorMatchCheckerList[0];

            var colorMatchCheckerParam = (ColorMatchCheckerParam)ColorMatchChecker.Param;

            //칼라 데이터 초기화
            colorPatternGrid.Rows.Clear();
            colorMatchCheckerParam.RemoveAllColorPattern();

            //칼라 데이터 추가
            var xmlDocument = new XmlDocument();

            XmlElement colorElement = xmlDocument.CreateElement("", "Color", "");
            xmlDocument.AppendChild(colorElement);
            string tempName = fileName + ".xml";
            colorMatchCheckerParam.ColorPatternFileName = fileName;
            colorMatchCheckerParam.LoadParam(colorElement);
        }

        private void UpdateGirdData()
        {
            if (colorMatchCheckerList.Count != 1)
            {
                return;
            }

            ColorMatchChecker ColorMatchChecker = colorMatchCheckerList[0];

            var colorMatchCheckerParam = (ColorMatchCheckerParam)ColorMatchChecker.Param;

            colorPatternGrid.Rows.Clear();
            if (colorMatchCheckerParam.ColorPatternList.Count > 0)
            {
                foreach (ColorPattern colorPattern in colorMatchCheckerParam.ColorPatternList)
                {
                    if (string.IsNullOrEmpty(colorPattern.Name) == false)
                    {
                        colorPatternGrid.Rows.Add(colorPattern.Name, colorPattern.Image);
                    }
                }
            }
        }

        private void comboIndex_Validating(object sender, CancelEventArgs e)
        {
            if (e.Cancel == true)
            {
                MessageBox.Show("Color load Cancel");
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string colorFolderPath = string.Format(@"{0}\..\Config", Environment.CurrentDirectory);
            string fileName = string.Format(@"{0}\ColorSet{1}", colorFolderPath, comboIndex.SelectedItem.ToString());
            if (comboIndex.SelectedIndex == 0 || checkUseColorSet.Checked == false)
            {
                return;
            }
            else
            {
                if (colorPatternGrid.Rows.Count >= 0)
                {
                    if (MessageBox.Show("Do you want save the color list?", "Caution", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        if (colorMatchCheckerList.Count != 1)
                        {
                            return;
                        }

                        ColorMatchChecker ColorMatchChecker = colorMatchCheckerList[0];

                        var colorMatchCheckerParam = (ColorMatchCheckerParam)ColorMatchChecker.Param;

                        var xmlDocument = new XmlDocument();

                        XmlElement colorElement = xmlDocument.CreateElement("", "Color", "");
                        xmlDocument.AppendChild(colorElement);
                        string tempName = fileName + ".xml";
                        colorMatchCheckerParam.UseColorPatternFile = true;
                        colorMatchCheckerParam.ColorPatternFileName = tempName;
                        colorMatchCheckerParam.SaveParam(colorElement);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void checkUseColorSet_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "ColorMatchCheckerParamControl - matchColor_TextChanged");

            foreach (ColorMatchChecker colorMatchChecker in colorMatchCheckerList)
            {
                var colorMatchCheckerParam = (ColorMatchCheckerParam)colorMatchChecker.Param;

                AlgorithmParam newParam = colorMatchCheckerParam.Clone();
                ((ColorMatchCheckerParam)newParam).UseColorPatternFile = checkUseColorSet.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, colorMatchChecker, newParam);
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResetColor();
        }

        private void ResetColor()
        {
            colorPatternGrid.Rows.Clear();
            matchColor.Text = "";
            txtSmoothing.Value = 0;
            matchScore.Value = 0;

            foreach (ColorMatchChecker colorMatchChecker in colorMatchCheckerList)
            {
                ((ColorMatchCheckerParam)colorMatchChecker.Param).RemoveAllColorPattern();
                colorMatchChecker.Clear();
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return ColorMatchChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
