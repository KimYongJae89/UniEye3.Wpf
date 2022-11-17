using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public partial class FigurePropertyForm : Form
    {
        public Figure Figure { get; set; } = null;

        private Font font = null;
        private Pen pen = null;
        private SolidBrush brush = null;
        private Color textColor;
        private StringAlignment alignment;

        public FigurePropertyForm()
        {
            InitializeComponent();

            labelPosition.Text = StringManager.GetString(labelPosition.Text);
            labelWidth.Text = StringManager.GetString(labelWidth.Text);
            labelLineColor.Text = StringManager.GetString(labelLineColor.Text);
            lineThicknessLabel.Text = StringManager.GetString(lineThicknessLabel.Text);
            labelBackgroundColor.Text = StringManager.GetString(labelBackgroundColor.Text);
            labelTextColor.Text = StringManager.GetString(labelTextColor.Text);
            labelFont.Text = StringManager.GetString(labelFont.Text);
            alignmentLabel.Text = StringManager.GetString(alignmentLabel.Text);
            okButton.Text = StringManager.GetString(okButton.Text);
            cancelButton.Text = StringManager.GetString(cancelButton.Text);

        }

        private void GetRectProperty(Figure figure)
        {
            RotatedRect rectangle = figure.GetRectangle();
            txtWidth.Text = rectangle.Width.ToString();
            txtHeight.Text = rectangle.Height.ToString();
            txtPositionX.Text = rectangle.X.ToString();
            txtPositionY.Text = rectangle.Y.ToString();
        }

        private void GetProperty(Figure figure)
        {
            FigureProperty figureProperty = figure.FigureProperty;
            if (figure is TextFigure)
            {
                var textFigure = figure as TextFigure;

                if (font == null)
                {
                    font = (Font)figureProperty.Font.Clone();
                    textColor = figureProperty.TextColor;
                    alignment = figureProperty.Alignment;

                }
            }
            else if ((figure is LineFigure) || (figure is RectangleFigure) || (figure is EllipseFigure))
            {
                if (pen == null)
                {
                    pen = (Pen)figureProperty.Pen.Clone();
                }

                if ((figure is RectangleFigure) || (figure is EllipseFigure))
                {
                    if (brush == null)
                    {
                        brush = (SolidBrush)figureProperty.Brush.Clone();
                    }
                }
            }
        }

        private void SetProperty(Figure figure)
        {
            FigureProperty figureProperty = figure.FigureProperty;
            if (figure is TextFigure)
            {
                var textFigure = figure as TextFigure;

                figureProperty.Font = (Font)font.Clone();
                figureProperty.TextColor = textColor;
                figureProperty.Alignment = alignment;
            }
            else if ((figure is LineFigure) || (figure is RectangleFigure) || (figure is EllipseFigure))
            {
                figureProperty.Pen = (Pen)pen.Clone();

                if ((figure is RectangleFigure) || (figure is EllipseFigure))
                {
                    figureProperty.Brush = (SolidBrush)brush.Clone();
                }
            }
        }
        private void SetRectProperty(Figure figure)
        {
            if (txtWidth.Text.ToString() != "" && txtHeight.Text.ToString() != "" && txtPositionX.Text.ToString() != "" && txtPositionY.Text.ToString() != "")
            {
                RotatedRect rectangle = figure.GetRectangle();
                rectangle.Width = float.Parse(txtWidth.Text.ToString());
                rectangle.Height = float.Parse(txtHeight.Text.ToString());
                rectangle.X = float.Parse(txtPositionX.Text.ToString());
                rectangle.Y = float.Parse(txtPositionY.Text.ToString());
                figure.SetRectangle(rectangle);
            }
        }

        private void FigurePropertyForm_Load(object sender, EventArgs e)
        {
            if (Figure is FigureGroup)
            {
                var figureGroup = Figure as FigureGroup;
                foreach (Figure subFigure in figureGroup)
                {
                    GetProperty(subFigure);
                }
            }
            else
            {
                GetProperty(Figure);
                GetRectProperty(Figure);
            }

            if (font != null)
            {
                switch (alignment)
                {
                    case StringAlignment.Near:
                        alignmentCombo.SelectedIndex = 0;
                        break;
                    case StringAlignment.Center:
                        alignmentCombo.SelectedIndex = 1;
                        break;
                    case StringAlignment.Far:
                        alignmentCombo.SelectedIndex = 2;
                        break;
                }
                textColorBox.BackColor = textColor;
                labelSampleText.Font = font;
            }
            else
            {
                textColorBox.Enabled = false;
                buttonSelectFont.Enabled = false;
                alignmentCombo.Enabled = false;
            }

            if (pen != null)
            {
                lineThicknessEdit.Value = (int)pen.Width;
                lineColorBox.BackColor = pen.Color;
            }
            else
            {
                lineThicknessEdit.Enabled = false;
                lineColorBox.Enabled = false;
            }

            if (brush != null)
            {
                backgroundColorBox.BackColor = brush.Color;
            }
            else
            {
                backgroundColorBox.Enabled = false;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (font != null)
            {
                font = (Font)font.Clone(); //  new Font(font.FontFamily, (float)fontSizeEdit.Value, font.Style);

                switch (alignmentCombo.SelectedIndex)
                {
                    case 0:
                        alignment = StringAlignment.Near;
                        break;
                    case 1:
                        alignment = StringAlignment.Center;
                        break;
                    case 2:
                        alignment = StringAlignment.Far;
                        break;
                }

                textColor = textColorBox.BackColor;
            }

            if (pen != null)
            {
                pen = new Pen(lineColorBox.BackColor, (float)lineThicknessEdit.Value);
            }

            if (brush != null)
            {
                brush = new SolidBrush(backgroundColorBox.BackColor);
            }

            if (Figure is FigureGroup)
            {
                var figureGroup = Figure as FigureGroup;
                foreach (Figure subFigure in figureGroup)
                {
                    SetProperty(subFigure);
                    SetRectProperty(subFigure);
                }
            }
            else
            {
                SetProperty(Figure);
                SetRectProperty(Figure);
            }

            Close();
        }



        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lineColorButton_Click(object sender, EventArgs e)
        {
            var dlg = new ColorDialog();
            dlg.Color = lineColorBox.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lineColorBox.BackColor = dlg.Color;
            }
        }

        private void backgroundColorButton_Click(object sender, EventArgs e)
        {
            var dlg = new ColorDialog();
            dlg.Color = backgroundColorBox.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                backgroundColorBox.BackColor = dlg.Color;
            }
        }

        private void textColorButton_Click(object sender, EventArgs e)
        {
            var dlg = new ColorDialog();
            dlg.Color = textColorBox.BackColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textColorBox.BackColor = dlg.Color;
            }
        }

        private void buttonSelectFont_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                return;
            }

            var dialog = new FontDialog();
            dialog.ShowColor = false;
            dialog.ShowEffects = false;
            dialog.AllowVerticalFonts = false;
            dialog.Font = font;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                font = dialog.Font;
                labelSampleText.Font = font;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
