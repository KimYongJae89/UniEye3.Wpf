using DynMvp.Base;
using DynMvp.Data.Library;
using DynMvp.Data.UI;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DynMvp.Data
{
    public enum InspectionLogicType
    {
        And, Or, Nand, Nor, Custom
    }

    public class Target : ICloneable, ITrackTarget, ITeachObject
    {
        // TypeName 정의
        public const string TypeGlobalFiducial = "GlobalFiducial";
        public const string TypeLocalFiducial = "LocalFiducial";
        public const string TypeCalibration = "Calibration";
        public int Id { get; set; }
        public int ModuleNo { get; set; }
        public int CameraIndex { get; set; }

        public int[] LightTypeIndexArr
        {
            get
            {
                var lightTypeIndex = new SortedSet<int>();
                foreach (Probe probe in ProbeList)
                {
                    if (probe.LightTypeIndexArr != null)
                    {
                        foreach (int index in probe.LightTypeIndexArr)
                        {
                            lightTypeIndex.Add(index);
                        }
                    }
                }

                return lightTypeIndex.ToArray();
            }
        }

        public string FullId => string.Format("{0:00}.{1:000}", InspectStep.StepNo, Id);
        public InspectionLogicType InspectionLogicType { get; set; }

        private string name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    return FullId;
                }

                return name;
            }
            set => name = value;
        }
        public string TypeName { get; set; }
        public int InspectOrder { get; set; }
        public InspectStep InspectStep { get; set; }
        public Figure ShapeFigure { get; set; }
        public List<Figure> SchemaFigures { get; set; }
        public RotatedRect BaseRegion { get; set; }
        public bool UseInspection { get; set; } = true;
        public bool Selected { get; set; } = false;
        public Probe FiducialProbe { get; set; }
        public Image2D Image { get; set; } = null;

        public string ImageEncodedString
        {
            get
            {
                if (Image == null)
                {
                    return null;
                }

                var bitmap = Image.ToBitmap();
                string resultString = ImageHelper.BitmapToBase64String(bitmap);
                bitmap.Dispose();

                return resultString;
            }
            set
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(value);
                if (bitmap != null)
                {
                    Image = Image2D.ToImage2D(bitmap);
                    bitmap.Dispose();
                }
            }
        }
        public List<Probe> ProbeList { get; } = new List<Probe>();

        public Probe this[int index] => ProbeList[index];

        public int NumProbe => ProbeList.Count;
        public Template Template { get; set; }

        public IEnumerator<Probe> GetEnumerator()
        {
            return ProbeList.GetEnumerator();
        }

        public Target()
        {

        }

        public void Clear()
        {
            foreach (Probe probe in ProbeList)
            {
                probe.Clear();
            }

            ProbeList.Clear();

            if (Image != null)
            {
                Image.Dispose();
            }
        }

        public object Clone()
        {
            var target = new Target();
            target.Copy(this);

            return target;
        }

        public int GetNumBand()
        {
            if (Image != null)
            {
                return Image.NumBand;
            }

            return 1;
        }

        public void UpdateTargetImage(Image2D targetImage)
        {
            Image = targetImage;
        }

        public void Copy(Target target)
        {
            InspectionLogicType = target.InspectionLogicType;
            name = target.Name;
            TypeName = target.TypeName;
            BaseRegion = target.BaseRegion;
            if (target.Image != null)
            {
                Image = (Image2D)target.Image.Clone();
            }
            else
            {
                Image = null;
            }

            UseInspection = target.UseInspection;

            foreach (Probe probe in target.ProbeList)
            {
                var cloneProbe = (Probe)probe.Clone();
                AddProbe(cloneProbe);
            }

            UpdateProbeId();

            LinkFiducialProbe();

            InspectStep = target.InspectStep;
        }

        public void SyncParam(Probe srcProbe, IProbeFilter probeFilter)
        {
            for (int i = 0; i < ProbeList.Count; i++)
            {
                if (srcProbe.GetType() == ProbeList[i].GetType())
                {
                    ProbeList[i].SyncParam(srcProbe, probeFilter);
                }
            }
        }

        public bool Add(object obj)
        {
            if (obj is Probe probe)
            {
                return AddProbe((Probe)obj);
            }

            return false;
        }

        internal void LinkFiducialProbe()
        {
            for (int i = 0; i < NumProbe; i++)
            {
                if (ProbeList[i].FiducialProbeId == i)
                {
                    this[i].FiducialProbe = this[ProbeList[i].FiducialProbeId];
                }
            }
        }

        public bool AddProbe(List<Probe> probeList)
        {
            foreach (Probe probe in probeList)
            {
                if (IsAddable(probe) == false)
                {
                    return false;
                }
            }

            foreach (Probe probe in probeList)
            {
                AddProbe(probe);
            }
            UpdateProbeId();

            return true;
        }

        /// <summary>
        /// Calibration Probe 등 특수 기능을 하는 Probe는 Target내에 하나만 존재할 수 있기 때문에
        /// Probe를 Target에 추가하기 전에 추가 가능한지 여부를 검사한다.
        /// </summary>
        /// <param name="probe"></param>
        /// <returns></returns>
        public bool IsAddable(Probe probe)
        {
            if (probe.IsCalibration() == true)
            {
                if (GetCalibrationProbe() != null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 새로운 Probe를 추가한다. Probe가 추가된 후 Probe의 ID가 갱신되어야 
        /// </summary>
        /// <param name="probe"></param>
        /// <returns></returns>
        public bool AddProbe(Probe probe)
        {
            if (IsAddable(probe) == false)
            {
                return false;
            }

            probe.FiducialProbe = FiducialProbe;
            probe.Target = this;
            probe.Id = ProbeList.Count;
            ProbeList.Add(probe);

            // Target의 BaseRegion을 갱신해주어야 한다.

            return true;
        }

        public void GetValueProbes(List<Probe> probeList)
        {
            foreach (Probe probe in ProbeList)
            {
                if (probe.ProbeResultType == ProbeResultType.Value)
                {
                    probeList.Add(probe);
                }
            }
        }

        public Probe GetProbe(int index)
        {
            if (ProbeList.Count > index)
            {
                return ProbeList[index];
            }

            return null;
        }

        public void GetProbe(IProbeFilter probeFilter, List<Probe> selectedProbeList)
        {
            foreach (Probe probe in ProbeList)
            {
                if (probeFilter.IsValid(probe))
                {
                    selectedProbeList.Add(probe);
                }
            }
        }

        public Probe GetProbe(string probeFullIdOrName)
        {
            foreach (Probe probe in ProbeList)
            {
                if (probe.FullId == probeFullIdOrName || probe.Name == probeFullIdOrName)
                {
                    return probe;
                }
            }

            return null;
        }

        public Probe GetCalibrationProbe()
        {
            foreach (Probe probe in ProbeList)
            {
                if (probe.IsCalibration())
                {
                    return probe;
                }
            }

            return null;
        }

        public void UpdateProbeId()
        {
            for (int i = 0; i < ProbeList.Count; i++)
            {
                ProbeList[i].Id = i;
            }
        }

        public void RemoveProbe(Probe probeRemoved)
        {
            probeRemoved.Clear();

            ProbeList.Remove(probeRemoved);

            UpdateProbeId();
        }

        public void CreateObjectFigures(FigureGroup figureGroup)
        {
            Figure targetFigure = new RectangleFigure(BaseRegion, new Pen(Color.Cyan));
            targetFigure.Tag = this;
            figureGroup.AddFigure(targetFigure);

            foreach (Probe probe in ProbeList)
            {
                Figure probeFigure = new RectangleFigure(probe.WorldRegion, new Pen(Color.Yellow));
                probeFigure.Tag = this;
                figureGroup.AddFigure(probeFigure);
            }
        }

        /// <summary>
        /// Target내의 모든 Probe가 같이 사용할 보정 Probe를 설정한다.
        /// </summary>
        /// <param name="fiducial">보정 Probe</param>
        /// <returns></returns>
        public bool SetFiducialProbe(Probe fiducialProbe)
        {
            if (fiducialProbe.Target != this)
            {
                return false;
            }

            if (fiducialProbe is Searchable == false)
            {
                return false;
            }

            FiducialProbe = fiducialProbe;

            foreach (Probe probe in ProbeList)
            {
                if (probe == fiducialProbe)
                {
                    continue;
                }

                probe.FiducialProbe = fiducialProbe;
            }

            return true;
        }

        /// <summary>
        /// Target내의 Probe에 설정된 보정 Probe 정보를 초기화 한다.
        /// </summary>
        public void ResetFiducial()
        {
            foreach (Probe probe in ProbeList)
            {
                probe.FiducialProbe = null;
            }
        }

        /// <summary>
        /// Target내에 설정된 보정 Probe 목록을 반환한다.
        /// 검사를 할 때 이 목록에 있는 Probe를 먼저 검사한다.
        /// </summary>
        public List<Probe> GetFiducialProbeList()
        {
            var fiducialList = new List<Probe>();

            foreach (Probe probe in ProbeList)
            {
                if (probe is VisionProbe visionProbe)
                {
                    if (visionProbe.InspAlgorithm is Searchable)
                    {
                        fiducialList.Add(visionProbe);
                    }
                }
            }

            return fiducialList;
        }

        public void UpdateRegion()
        {
            RectangleF newRegion = RectangleF.Empty;
            foreach (Probe probe in ProbeList)
            {
                if (newRegion == RectangleF.Empty)
                {
                    newRegion = probe.BaseRegion.GetBoundRect();
                }
                else
                {
                    newRegion = RectangleF.Union(newRegion, probe.BaseRegion.GetBoundRect());
                }
            }

            BaseRegion = new RotatedRect(newRegion, 0);
        }

        public void UpdateRegion(RotatedRect newBaseRegion)
        {
            PointF centerFrom = DrawingHelper.CenterPoint(BaseRegion);
            PointF centerTo = DrawingHelper.CenterPoint(newBaseRegion);

            var offset = new SizeF(DrawingHelper.Subtract(centerTo, centerFrom));
            Offset(offset);
        }

        public void Offset(SizeF offset)
        {
            foreach (Probe probe in ProbeList)
            {
                probe.Offset(offset);
            }

            UpdateRegion();
        }

        public RotatedRect GetAlignedRegion(PositionAligner positionAligner, RotatedRect rotatedRect)
        {
            if (positionAligner != null)
            {
                PointF stepPos = positionAligner.Align(InspectStep.Position.ToPointF());
                return positionAligner.AlignFov(rotatedRect, stepPos);
            }

            return rotatedRect;
        }

        /// <summary>
        /// 화면 표시를 위한 Figure 객체를 생성한다.
        /// Visitor Pattern.
        /// </summary>
        /// <param name="figureGroup">Figure 객체를 수집하는 객체</param>
        /// <param name="option"></param>
        public void AppendFigures(PositionAligner positionAligner, FigureGroup workingFigures, FigureGroup backgroundFigures, CanvasPanel.Option option)
        {
            RotatedRect targetRegion = BaseRegion;

            if (positionAligner != null)
            {
                targetRegion = GetAlignedRegion(positionAligner, targetRegion);
            }

            Pen pen = option.Pen;
            if (option.IncludeProbe)
            {
                targetRegion.Inflate(10, 10);

                if (pen == null)
                {
                    pen = new Pen(Color.LightGreen, 0F);
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }

                var workingFigure = new RectangleFigure(targetRegion, pen);
                workingFigure.Tag = this;
                workingFigure.ObjectLevel = 2;
                workingFigure.Selectable = true;
                workingFigure.Resizable = false;

                workingFigures.AddFigure(workingFigure);

                foreach (Probe probe in ProbeList)
                {
                    probe.AppendFigures(positionAligner, workingFigures, backgroundFigures, option);
                }
            }
            else
            {
                if (pen == null)
                {
                    pen = new Pen(Color.LightGreen, 0F);
                }

                var workingFigure = new RectangleFigure(targetRegion, pen);
                workingFigure.Tag = this;
                workingFigure.Selectable = true;
                workingFigure.ObjectLevel = 2;

                workingFigures.AddFigure(workingFigure);
            }
        }

        public MarkerProbe GetMarkerProbe(MarkerType markerType)
        {
            foreach (Probe probe in ProbeList)
            {
                if (probe is MarkerProbe markerProbe)
                {
                    if (markerProbe != null && markerProbe.MarkerType == markerType)
                    {
                        return markerProbe;
                    }
                }
            }

            return null;
        }

        public List<int> GetLightTypeIndexList()
        {
            var lightTypeList = new List<int>();
            foreach (Probe probe in ProbeList)
            {
                if (probe is VisionProbe)
                {
                    lightTypeList.AddRange(((VisionProbe)probe).LightTypeIndexArr);
                }
            }
            lightTypeList.Sort();
            return lightTypeList;
        }

        internal void AddSchemaFigure(Schema schema)
        {
            var targetFigure = new RectangleFigure(BaseRegion, new Pen(Color.LightGray), null);
            targetFigure.Id = "TargetRect";

            var targetSchemaFigure = new SchemaFigure();
            targetSchemaFigure.Name = name;
            targetSchemaFigure.Deletable = false;
            targetSchemaFigure.Movable = false;
            targetSchemaFigure.AddFigure(targetFigure);
            targetSchemaFigure.Tag = FullId;

            schema.AddFigure(targetSchemaFigure);

            foreach (Probe probe in ProbeList)
            {
                var probeFigure = new RectangleFigure(probe.WorldRegion, (Pen)schema.DefaultFigureProperty.Pen.Clone(), null);
                probeFigure.Id = "ProbeRect";

                var probeSchemaFigure = new SchemaFigure();
                probeSchemaFigure.Name = probe.Name;
                probeSchemaFigure.Deletable = false;
                probeSchemaFigure.Movable = false;
                probeSchemaFigure.AddFigure(probeFigure);
                probeSchemaFigure.Tag = probe.FullId;

                schema.AddFigure(probeSchemaFigure);
            }
        }

        private List<Probe> GetComputeProbeList()
        {
            var computeProbeList = new List<Probe>();

            foreach (Probe probe in ProbeList)
            {
                if (probe is ComputeProbe)
                {
                    computeProbeList.Add(probe);
                }
            }

            return computeProbeList;
        }

        private List<Probe> GetSortedProbeList()
        {
            return GetSortedProbeList(ProbeList);
        }

        private List<Probe> GetSortedProbeList(List<Probe> probeList)
        {
            var sortedProbeList = new List<Probe>();

            sortedProbeList.AddRange(GetFiducialProbeList());

            Probe calibrationProbe = GetCalibrationProbe();
            if (calibrationProbe != null)
            {
                sortedProbeList.Add(calibrationProbe);
            }

            foreach (Probe probe in probeList)
            {
                if (probe is ComputeProbe || probe is MarkerProbe || probe == calibrationProbe)
                {
                    continue;
                }

                if (probe is VisionProbe visionProbe)
                {
                    if (visionProbe.InspAlgorithm is Searchable)
                    {
                        continue;
                    }
                }

                sortedProbeList.Add(probe);
            }

            return sortedProbeList;
        }

        public void OnPreInspection()
        {
            foreach (Probe probe in ProbeList)
            {
                probe.OnPreInspection();
            }
        }

        public void OnPostInspection()
        {
            foreach (Probe probe in ProbeList)
            {
                probe.OnPostInspection();
            }
        }

        protected RotatedRect GetAlignedRect(InspectParam inspectParam)
        {
            RotatedRect targetAlignedRect = BaseRegion;
            if (inspectParam.GlobalPositionAligner != null)
            {
                targetAlignedRect = inspectParam.GlobalPositionAligner.AlignFov(targetAlignedRect, inspectParam.InspectStepAlignedPos);
            }

            if (inspectParam.LocalPositionAligner != null)
            {
                targetAlignedRect = inspectParam.LocalPositionAligner.Align(targetAlignedRect);
            }

            return targetAlignedRect;
        }

        public bool Inspect(InspectParam inspectParam)
        {
            if (UseInspection == false)
            {
                LogHelper.Debug(LoggerType.Inspection, string.Format("Inspection Skipped. {0} {1}", FullId, Name));
                return false;
            }

            if (BaseRegion.IsEmpty)
            {
                LogHelper.Debug(LoggerType.Inspection, string.Format("Target Region is Empty. {0} {1}", FullId, Name));
                return false;
            }

            if (inspectParam.InspectEventHandler != null)
            {
                inspectParam.InspectEventHandler.TargetBeginInspect(this);
            }

            List<Probe> sortedProbeList = GetSortedProbeList();

            var targetResult = new ProbeResultList();

            bool fiducialGood = true;
            bool good = true;
            if (InspectionLogicType == InspectionLogicType.Or)
            {
                good = false;
            }

            RotatedRect alignedRect = GetAlignedRect(inspectParam);

            foreach (Probe probe in sortedProbeList)
            {
                ProbeResult probeResult = probe.Inspect(inspectParam, targetResult);

                bool curGood = (probeResult.IsGood());

                if (probe is Searchable)
                {
                    fiducialGood &= curGood;
                }
                else if (InspectionLogicType == InspectionLogicType.Or)
                {
                    good |= curGood;
                }
                else
                {
                    good &= curGood;
                }

                targetResult.AddProbeResult(probeResult);

                probeResult.TargetRegion = alignedRect;
            }

            good &= fiducialGood;

            if (good && InspectionLogicType == InspectionLogicType.Or)
            {
                foreach (ProbeResult probeResult in targetResult)
                {
                    if (probeResult.IsNG())
                    {
                        probeResult.SetOverkill();
                    }
                }
            }

            string resultPath = inspectParam.ResultPath;
            if (InspectConfig.Instance().SaveTargetImage && string.IsNullOrEmpty(resultPath) != true)
            {
                int lightTypeIndex = LightTypeIndexArr[0];
                Image2D cameraImage = inspectParam.ImageBuffer.GetImage(CameraIndex, lightTypeIndex);

                if (cameraImage != null)
                {
                    var targetImage = (Image2D)cameraImage.ClipImage(Rectangle.Ceiling(alignedRect.GetBoundRect()));
                    if (targetImage != null)
                    {
                        string fileName = string.Format("{0}\\{1}_{2}.jpg", resultPath, FullId, (good == true ? "G" : "N"));

                        if (File.Exists(fileName) == true)
                        {
                            File.Delete(fileName);
                        }

                        targetImage.SaveImage(fileName, ImageFormat.Jpeg);
                    }
                }
            }

            inspectParam.ProbeResultList.AddProbeResult(targetResult);

            if (inspectParam.InspectEventHandler != null)
            {
                inspectParam.InspectEventHandler.TargetEndInspect(this, targetResult);
            }

            return good;
        }

        public bool Compute(InspectParam inspectParam, ProductResult inspectionResult)
        {
            if (UseInspection == false)
            {
                return false;
            }

            List<Probe> computeProbeList = GetComputeProbeList();

            var targetResult = new ProductResult();
            bool result = true;
            bool exist = false;
            foreach (Probe probe in computeProbeList)
            {
                ProbeResult probeResult = probe.Inspect(inspectParam, targetResult);
                result &= probeResult.IsGood();
                exist = true;
            }

            if (exist == true)
            {
                inspectParam.InspectEventHandler.TargetEndInspect(this, targetResult);
            }

            inspectionResult.AddProbeResult(targetResult);

            //LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Result. {0} {1} {2}", FullId, Name, result));

            return result;
        }

        public bool IsSizable()
        {
            return false;
        }

        public bool IsRotatable()
        {
            return false;
        }

        public bool IsContainer()
        {
            return true;
        }

        public void AddToParent()
        {
            InspectStep.AddTarget(this);
        }
    }
}
