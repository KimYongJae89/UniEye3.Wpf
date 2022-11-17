using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspectData;
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
    public partial class TargetView : DrawBox
    {
        private ProductResult targetInspectionResult = new ProductResult();
        public Target LinkedTarget { get; set; } = null;
        public int TargetIndex { get; set; }

        public TargetView()
        {
            InitializeComponent();
        }

        public void SetTarget(Target target, CanvasPanel.Option option, bool fUpdate = true)
        {
            LinkedTarget = target;
            UpdateImage(LinkedTarget.Image.ToBitmap());

            var workingFigures = new FigureGroup();
            var backgroundFigures = new FigureGroup();
            target.AppendFigures(null, workingFigures, backgroundFigures, option);

            workingFigures.SetSelectable(false);
            workingFigures.Offset(-target.BaseRegion.Left, -target.BaseRegion.Top);

            FigureGroup = workingFigures;

            if (fUpdate)
            {
                Invalidate();
            }
        }

        public void UpdteTargetView(ProductResult targetInspectionResult)
        {
            LogHelper.Debug(LoggerType.Grab, "Start UpdteTargetView");

            Image2D targetImage = targetInspectionResult.GetTargetImage(LinkedTarget.FullId);
            if (targetImage != null)
            {
                UpdateImage(targetImage.ToBitmap());
            }
            else
            {
                UpdateImage(LinkedTarget.Image.ToBitmap());
            }

            targetInspectionResult.AppendResultFigures(TempFigureGroup, ResultImageType.Target);

            LogHelper.Debug(LoggerType.Grab, "End UpdteTargetView");

            Invalidate();
        }

        public void UpdateTargetView(ProbeResult probeResult, Image2D targetImage, bool useWholeImage = false)
        {
            LogHelper.Debug(LoggerType.Grab, "Start UpdteTargetView");

            if (targetImage != null)
            {
                UpdateImage(targetImage.ToBitmap());
            }
            else
            {
                UpdateImage(Properties.Resources.Image);
            }

            TempFigureGroup.Clear();

            probeResult.AppendResultFigures(TempFigureGroup, ResultImageType.Target);

            if (probeResult.Probe != null)
            {
                LinkedTarget = probeResult.Probe.Target;
                if (useWholeImage == false)
                {
                    TempFigureGroup.Offset(-LinkedTarget.BaseRegion.X, -LinkedTarget.BaseRegion.Y);
                }
            }

            LogHelper.Debug(LoggerType.Grab, "End UpdteTargetView");

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Call base class, invoke Paint handlers
            base.OnPaint(e);

            if (LinkedTarget != null)
            {
                var font = new Font("Arial", 10);
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;

                Brush fontBrush = new SolidBrush(Color.Green);

                e.Graphics.DrawString(LinkedTarget.Name, font, fontBrush, (float)5, (float)5, stringFormat);
            }
        }
    }
}
