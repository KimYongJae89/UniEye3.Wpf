using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class BlobSubtractorParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<BlobSubtractor> blobSubtractorList = new List<BlobSubtractor>();
        private ProbeList probeList = new ProbeList();
        private bool onValueUpdate = false;

        public BlobSubtractorParamControl()
        {
            InitializeComponent();
        }

        public void ClearSelectedProbe()
        {
            blobSubtractorList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - AddSelectedProbe");

            var selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == BlobSubtractor.TypeName)
            {
                blobSubtractorList.Add((BlobSubtractor)selectedProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void AddSelectedProbe(ProbeList selectedProbeList)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - AddSelectedProbe");

            blobSubtractorList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == BlobSubtractor.TypeName)
                {
                    blobSubtractorList.Add((BlobSubtractor)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - SelectProbe");

            probeList.Clear();
            blobSubtractorList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                probeList.AddProbe(visionProbe);
                blobSubtractorList.Add((BlobSubtractor)visionProbe.InspAlgorithm);
            }

            UpdateData();
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return BlobSubtractor.TypeName;
        }

        private void UpdateData()
        {
            if (blobSubtractorList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - UpdateData");

            onValueUpdate = true;

            var blobSubtractorParam = (BlobSubtractorParam)blobSubtractorList[0].Param;

            edgeThreshold.Value = blobSubtractorParam.EdgeThreshold;
            maxPixelCount.Value = blobSubtractorParam.MaxPixelCount;
            minPixelCount.Value = blobSubtractorParam.MinPixelCount;
            numAreaMin.Value = blobSubtractorParam.AreaMin;

            UpdateMaskImageSelector();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void addMaskButton_Click(object sender, EventArgs e)
        {
            if (blobSubtractorList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - addMaskButton_Click");

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                AlgorithmParam newParam = blobSubtractor.Param.Clone();

                AddMask((BlobSubtractorParam)newParam);

                UpdateMaskImageSelector();

                ParamControl_ValueChanged(ValueChangedType.None, blobSubtractor, newParam);
            }
        }

        private void AddMask(BlobSubtractorParam blobSubtractorParam)
        {
            if (probeList.Count == 0)
            {
                return;
            }

            if (blobSubtractorList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - AddMask");

            Probe probe = probeList[0];
            BlobSubtractor blobSubtractor = blobSubtractorList[0];

            Target selectedTarget = probe.Target;
            ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;

            var imageRegion = new RectangleF(0, 0, targetGroupImage.Width, targetGroupImage.Height);

            RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
            if (probeRegion == RectangleF.Intersect(probeRegion, imageRegion))
            {
                RotatedRect probeRotatedRect = probe.BaseRegion;

                ImageD clipImage = targetGroupImage.ClipImage(probeRotatedRect);
#if DEBUG
                //clipImage.ToBitmap().Save(@"D:\blobClipImage.bmp");
#endif 
                ImageD filterredImage = blobSubtractor.Filter(clipImage, 0);
#if DEBUG
                //filterredImage.ToBitmap().Save(@"D:\blobfilterredImage.bmp");
#endif 

                blobSubtractorParam.AddMaskImage(filterredImage);
            }
            else
            {
                MessageBox.Show(StringManager.GetString("Probe region is invalid."));
            }
        }

        private void refreshMaskButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - refreshMaskButton_Click");

            BlobSubtractor blobSubtractor = blobSubtractorList[0];

            maskImageSelector.Rows.Clear();
            var newParam = (BlobSubtractorParam)blobSubtractor.Param.Clone();
            var oldParam = (BlobSubtractorParam)blobSubtractor.Param;

            oldParam.RemoveAllMaskImage();
            newParam.RemoveAllMaskImage();

            AddMask(newParam);

            UpdateMaskImageSelector();

            ParamControl_ValueChanged(ValueChangedType.None, blobSubtractor, newParam);
        }

        private void DeleteMaskButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - deleteMaskButton_Click");

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                if (maskImageSelector.SelectedRows.Count > 0)
                {
                    int index = maskImageSelector.SelectedRows[0].Index;
                    if (index > -1)
                    {
                        var newParam = (BlobSubtractorParam)blobSubtractor.Param.Clone();

                        newParam.RemoveMaskImage(index);

                        maskImageSelector.Rows.RemoveAt(index);

                        ParamControl_ValueChanged(ValueChangedType.None, blobSubtractor, newParam);
                    }
                }
            }
        }

        private void minPixelCount_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            if (blobSubtractorList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - minPixelCount_ValueChanged");

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                AlgorithmParam newParam = blobSubtractor.Param.Clone();
                ((BlobSubtractorParam)newParam).MinPixelCount = (int)minPixelCount.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobSubtractor, newParam);
            }
        }

        private void maxPixelCount_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - maxPixelCount_ValueChanged");

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                AlgorithmParam newParam = blobSubtractor.Param.Clone();
                ((BlobSubtractorParam)newParam).MaxPixelCount = (int)maxPixelCount.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobSubtractor, newParam);
            }
        }

        private void edgeThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - edgeThreshold_ValueChanged");

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                AlgorithmParam newParam = blobSubtractor.Param.Clone();
                ((BlobSubtractorParam)newParam).EdgeThreshold = (int)edgeThreshold.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobSubtractor, newParam);
            }
        }

        private void UpdateMaskImageSelector()
        {
            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - UpdateMaskImageSelector");

            maskImageSelector.Rows.Clear();

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                var blobSubtractorParam = (BlobSubtractorParam)blobSubtractor.Param;//.Clone();

                foreach (AlgoImage maskImage in blobSubtractorParam.MaskImageList)
                {
                    int index = maskImageSelector.Rows.Add(maskImage.ToImageD().ToBitmap());

                    maskImageSelector.Rows[index].Height = maskImageSelector.Rows[index].Cells[0].ContentBounds.Height;
                    if (maskImageSelector.Rows[index].Height > maskImageSelector.Height - maskImageSelector.ColumnHeadersHeight)
                    {
                        maskImageSelector.Rows[index].Height = (maskImageSelector.Height - maskImageSelector.ColumnHeadersHeight);
                    }
                }

                if (maskImageSelector.Rows.Count > 0)
                {
                    maskImageSelector.Rows[0].Selected = true;
                }
            }
        }

        private void numAreaMin_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobSubtractorParamControl - numAreaMin_ValueChanged");

            foreach (BlobSubtractor blobSubtractor in blobSubtractorList)
            {
                AlgorithmParam newParam = blobSubtractor.Param.Clone();
                ((BlobSubtractorParam)newParam).AreaMin = (int)numAreaMin.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobSubtractor, newParam);
            }
        }
    }
}
