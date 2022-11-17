using DynMvp.Base;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DynMvp.Data
{
    public class AlignedRegionParam
    {
        public PositionAligner GlobalPositionAligner { get; set; }
        public PositionAligner LocalPositionAligner { get; set; }
        public Size Size { get; set; }
        public PointF RobotPos { get; set; }
        public bool WholeImageMode { get; set; }

        public AlignedRegionParam(InspectParam inspectParam)
        {
            GlobalPositionAligner = inspectParam.GlobalPositionAligner;
            LocalPositionAligner = inspectParam.LocalPositionAligner;
            RobotPos = inspectParam.InspectStepAlignedPos;
        }
    }

    public class VisionProbe : Probe
    {
        public Algorithm InspAlgorithm { get; set; }

        protected FigureGroup maskFigures = new FigureGroup();
        public FigureGroup MaskFigures
        {
            get => maskFigures;
            set => maskFigures = value;
        }

        protected FigureGroup alignedMaskFigures = new FigureGroup();
        public FigureGroup AlignedMaskFigures
        {
            get => alignedMaskFigures;
            set => alignedMaskFigures = value;
        }

        public override int[] LightTypeIndexArr => InspAlgorithm.LightTypeIndexArr;
        public int PreveiewLightTypeIndex { get; set; } = 0;

        public VisionProbe() : base()
        {

        }

        public override bool IsControllable()
        {
            if (InspAlgorithm as ObjectFinder != null)
            {
                return false;
            }

            if (InspAlgorithm as BoltChecker != null)
            {
                return false;
            }

            return true;
        }

        public override bool IsCalibration()
        {
            return InspAlgorithm is ICalibration;
        }

        public override List<ResultValue> GetResultValues()
        {
            return InspAlgorithm.GetResultValues();
        }

        public override object Clone()
        {
            var visionProbe = new VisionProbe();
            visionProbe.Copy(this);

            return visionProbe;
        }

        public override bool SyncParam(Probe srcProbe, IProbeFilter probeFilter)
        {
            var srcVisionProbe = (VisionProbe)srcProbe;

            if (srcVisionProbe.InspAlgorithm.GetAlgorithmType() == InspAlgorithm.GetAlgorithmType())
            {
                if (base.SyncParam(srcProbe, probeFilter) == false)
                {
                    return false;
                }

                InspAlgorithm.SyncParam(srcVisionProbe.InspAlgorithm);
            }

            return true;
        }

        public override object GetParamValue(string paramName)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }

            if (InspAlgorithm != null)
            {
                return InspAlgorithm.GetParamValue(paramName);
            }

            return "";
        }

        public override bool SetParamValue(string paramName, object value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, value);
                return true;
            }

            if (InspAlgorithm != null)
            {
                return InspAlgorithm.SetParamValue(paramName, value);
            }

            return false;
        }

        public override void Clear()
        {
            InspAlgorithm.Clear();
        }

        public override void PrepareInspection()
        {
            InspAlgorithm.PrepareInspection();
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            var visionProbe = (VisionProbe)probe;

            if (visionProbe.InspAlgorithm.IsAlgorithmPoolItem == true)
            {
                InspAlgorithm = visionProbe.InspAlgorithm;
            }
            else
            {
                InspAlgorithm = visionProbe.InspAlgorithm.Clone();
            }
        }

        public override string GetProbeTypeDetailed()
        {
            return InspAlgorithm.GetAlgorithmType();
        }

        public override string GetProbeTypeShortName()
        {
            return InspAlgorithm.GetAlgorithmTypeShort();
        }

        public override string[] GetPreviewNames()
        {
            return InspAlgorithm.GetPreviewNames();
        }

        public RotatedRect GetInspectRegion(AlignedRegionParam alignedRegionParam, SizeF imageSize, bool forceWholeImage = false)
        {
            RotatedRect inspectRegion;

            bool useWholeImage = false;
            if (this is Searchable searchable)
            {
                useWholeImage = searchable.UseWholeImage;
            }

            if (useWholeImage == false && forceWholeImage == false)
            {
                inspectRegion = GetAlignedRegion(alignedRegionParam);
                inspectRegion = InspAlgorithm.AdjustInspectRegion(inspectRegion);
            }
            else
            {
                inspectRegion = new RotatedRect(new Point(0, 0), imageSize, 0);
            }

            return inspectRegion;
        }

        public override void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {
            if (InspAlgorithm.Enabled == false)
            {
                return;
            }

            if (InspAlgorithm is Searchable searchable)
            {
                if (searchable.UseWholeImage == false)
                {
                    Size searchRange = searchable.GetSearchRangeSize();
                    var searchRangeRect = RotatedRect.Inflate(probeRect, searchRange.Width, searchRange.Height);

                    var pen = new Pen(Color.Cyan, 1.0F);
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    var figure = new RectangleFigure(searchRangeRect, pen);
                    figure.Selectable = false;
                    tempFigures.AddFigure(figure);
                }

                InspAlgorithm.BuildSelectedFigures(probeRect, tempFigures);
            }
        }

        public override void PreviewFilterResult(RotatedRect inspectRegion, ImageD imageCloned, int filterType)
        {
            imageCloned.Roi = inspectRegion.ToRectangle();

            InspAlgorithm.Filter(imageCloned, filterType);
        }

        //private RotatedRect AlignRegion(PositionAligner positionAligner)
        //{
        //    RotatedRect newRegion = BaseRegion;

        //    if (positionAligner != null)
        //    {
        //        newRegion = positionAligner.Align(BaseRegion, Target.TargetGroup.InspectionStep.AlignedPosition);
        //    }

        //    return newRegion;
        //}

        public override ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            if (InspAlgorithm.Enabled == false)
            {
                return new VisionProbeResult(this, "Algorithm is disabled");
            }

            if (AlgorithmFactory.Instance().IsAlgorithmEnabled(InspAlgorithm.GetAlgorithmType()) == false)
            {
                return new VisionProbeResult(this, string.Format("{0} Algorithm is not licensed.", InspAlgorithm.GetAlgorithmType()));
            }

            List<Image2D> imageList = inspectParam.ImageBuffer.CreateImageList(Target.CameraIndex, InspAlgorithm.LightTypeIndexArr);
            if (imageList.Count() == 0)
            {
                return new VisionProbeResult(this, "Image List is Empty");
            }

            bool useWholeImage = false;
            if (InspAlgorithm is Searchable searchable)
            {
                useWholeImage = searchable.UseWholeImage;
            }
            var sw = new Stopwatch();
            sw.Start();

            LogHelper.Debug(LoggerType.Inspection, string.Format("Insepct Probe - {0} [{1} ms]", FullId, sw.ElapsedMilliseconds, sw.ElapsedMilliseconds));

            bool saveDebugImage = InspectConfig.Instance().SaveDebugImage;
            string debugPath = string.Format("{0}\\{1}", BaseConfig.Instance().TempPath, string.IsNullOrEmpty(Name) ? FullId : Name);
            var debugContext = new DebugContext(saveDebugImage, debugPath);

            LogHelper.Debug(LoggerType.Inspection, string.Format("Insepct Probe - Align Probe : {0} [{1} ms]", FullId, sw.ElapsedMilliseconds));

            RotatedRect probeRegionInFov = GetAlignedRegion(new AlignedRegionParam(inspectParam));

            if (fiducialProbeId > 0)
            {
                var probeResult = probeResultList.GetProbeResult(fiducialProbeId) as VisionProbeResult;
                if (probeResult == null)
                {
                    if (probeResult.AlgorithmResult is SearchableResult pmResult)
                    {
                        probeRegionInFov.Offset(pmResult.OffsetFound);
                    }
                }
                else
                {
                    LogHelper.Warn(LoggerType.Inspection, "Can't find local fiducial result");
                }
            }

            Size imageSize = imageList[0].Size;

            var fovRegion = new RectangleF(new PointF(0, 0), imageSize);

            RotatedRect inspectRegionInFov;
            if (useWholeImage)
            {
                inspectRegionInFov = new RotatedRect(new PointF(0, 0), imageSize, 0);
            }
            else
            {
                inspectRegionInFov = InspAlgorithm.AdjustInspectRegion(probeRegionInFov);

                int clipExtendSize = InspectConfig.Instance().ClipExtendSize;
                if (clipExtendSize > 0)
                {
                    inspectRegionInFov.Inflate(clipExtendSize, clipExtendSize);
                }
            }

            if (fovRegion.Contains(inspectRegionInFov.ToRectangle()) == false)
            {
                LogHelper.Debug(LoggerType.Inspection, string.Format("Inspection Skipped. {0}", Target.FullId));
                return new VisionProbeResult(this, "Probe region is out of image");
            }

            LogHelper.Debug(LoggerType.Inspection, string.Format("Insepct Probe - Algorithm Inspect : {0} [{1} ms]", FullId, sw.ElapsedMilliseconds));

            var inspectImageList = new List<ImageD>();

            foreach (ImageD cameraImage in imageList)
            {
                ImageD inspectImage = cameraImage.ClipImage(inspectRegionInFov);

                inspectImageList.Add(inspectImage);
                DebugHelper.SaveImage(inspectImage, "InspectImage.bmp", debugContext);
            }

            var algorithmInspectParam = new AlgorithmInspectParam(inspectImageList, imageSize, probeRegionInFov, inspectRegionInFov, inspectParam.CameraCalibration, debugContext);

            // 검사
            AlgorithmResult algorithmResult = InspAlgorithm.Inspect(algorithmInspectParam);

            LogHelper.Debug(LoggerType.Inspection, string.Format("VisionProbe Inspect Time [{0} ms]", sw.ElapsedMilliseconds));

            if ((InspectConfig.Instance().SaveDefectImage && algorithmResult.IsNG()) || InspectConfig.Instance().SaveProbeImage)
            {
                if (string.IsNullOrEmpty(inspectParam.ResultPath) != true)
                {
                    foreach (Image2D inspectImage in inspectImageList)
                    {
                        string fileName = string.Format("{0}\\{1}_{2}.jpg", inspectParam.ResultPath, FullId, (algorithmResult.IsGood() ? "G" : "N"));

                        if (File.Exists(fileName) == true)
                        {
                            File.Delete(fileName);
                        }

                        inspectImage.SaveImage(fileName, ImageFormat.Jpeg);
                    }
                }
            }

            var visionProbeResult = new VisionProbeResult(this, algorithmResult);
            visionProbeResult.ProbeRegion = probeRegionInFov;
            visionProbeResult.InspectRegion = inspectRegionInFov;

            return visionProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new VisionProbeResult(this, "Empty");
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override string GetFigurePropertyName()
        {
            if (InspAlgorithm.Enabled == false)
            {
                return "Disabled";
            }
            else
            {
                return InspAlgorithm.AlgorithmName;
            }
        }
    }
}
