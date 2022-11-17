using DynMvp;
using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace DynMvp.Data
{
    public enum ProbeType
    {
        Vision, Io, Serial, Daq, Compute, Marker
    }

    public enum ProbeResultType
    {
        Judge, Value
    }

    public interface IProbeFilter : ICloneable
    {
        bool IsValid(Probe probe);
    }

    public abstract class ProbeCustomInfo
    {
        public abstract ProbeCustomInfo Clone();
        public abstract void Save(XmlElement xmlElement);
        public abstract void Load(XmlElement xmlElement);
    }

    public abstract class Probe : ICloneable, ITeachObject
    {
        [BrowsableAttribute(false)]
        public int Id { get; set; }

        public override string ToString()
        {
            return "Probe " + Id;
        }

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

        public abstract int[] LightTypeIndexArr { get; }

        protected Target target;
        [BrowsableAttribute(false)]
        public Target Target
        {
            get => target;
            set => target = value;
        }
        [BrowsableAttribute(false)]
        public ProbeType ProbeType { get; set; }
        public ProbeResultType ProbeResultType { get; set; }
        public bool StepBlocker { get; set; }

        public virtual string GetProbeTypeDetailed()
        {
            return ProbeType.ToString();
        }

        public virtual string GetProbeTypeShortName()
        {
            return ProbeType.ToString();
        }
        public bool Selected { get; set; } = false;
        public bool IsSelectable { get; set; } = true;
        public bool IsRotatable { get; set; } = true;

        /// <summary>
        /// probe의 위치를 보정할 기준 fiducial probe
        /// </summary>
        private Probe fiducialProbe = null;
        public Probe FiducialProbe
        {
            get => fiducialProbe;
            set
            {
                fiducialProbe = value;
                if (fiducialProbe == null)
                {
                    fiducialProbeId = -1;
                }
                else
                {
                    fiducialProbeId = fiducialProbe.Id;
                }
            }
        }

        /// <summary>
        /// probe의 위치를 보정할 기준 fiducial probe의 id
        /// </summary>
        protected int fiducialProbeId = -1;
        public int FiducialProbeId
        {
            get
            {
                if (fiducialProbe != null)
                {
                    return fiducialProbe.Id;
                }
                else
                {
                    return fiducialProbeId;
                }
            }
            set => fiducialProbeId = value;
        }
        public Figure ShapeFigure { get; set; }
        public List<Figure> SchemaFigures { get; set; }
        public RotatedRect WorldRegion { get; set; }

        [BrowsableAttribute(false)]
        protected RotatedRect baseRegion;
        public RotatedRect BaseRegion
        {
            get => baseRegion;
            set => baseRegion = value;
        }

        public float X { get => baseRegion.X; set => baseRegion.X = value; }
        public float Y { get => baseRegion.Y; set => baseRegion.Y = value; }
        public float Width { get => baseRegion.Width; set => baseRegion.Width = value; }
        public float Height { get => baseRegion.Height; set => baseRegion.Height = value; }
        public float Angle { get => baseRegion.Angle; set => baseRegion.Angle = value; }
        public bool InverseResult { get; set; }
        public bool ModelVerification { get; set; }

        public abstract bool IsControllable();
        public abstract List<ResultValue> GetResultValues();

        [BrowsableAttribute(false)]
        public string FullId => string.Format("{0}.{1:000}", Target.FullId, Id);
        public ProbeCustomInfo CustomInfo { get; set; } = null;

        public virtual bool IsCalibration()
        {
            return false;
        }

        public abstract object Clone();

        public virtual void Clear()
        {

        }

        public virtual void Copy(Probe probe)
        {
            Id = probe.Id;
            target = probe.target;
            ProbeType = probe.ProbeType;
            StepBlocker = probe.StepBlocker;
            fiducialProbe = probe.fiducialProbe;
            fiducialProbeId = probe.fiducialProbeId;
            baseRegion = new RotatedRect(probe.baseRegion);
            InverseResult = probe.InverseResult;
            ModelVerification = probe.ModelVerification;

            if (probe.CustomInfo != null)
            {
                CustomInfo = probe.CustomInfo.Clone();
            }
        }

        public virtual object GetParamValue(string paramName)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }

            return "";
        }

        public virtual bool SetParamValue(string paramName, object value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, value);
                return true;
            }

            return false;
        }

        public virtual bool SyncParam(Probe srcProbe, IProbeFilter probeFilter)
        {
            if (probeFilter?.IsValid(this) == false)
            {
                return false;
            }

            StepBlocker = srcProbe.StepBlocker;
            InverseResult = srcProbe.InverseResult;
            ModelVerification = srcProbe.ModelVerification;
            //lightTypeIndex = srcProbe.LightTypeIndex;

            //if (figureProperty != null)
            //    figureProperty = srcProbe.figureProperty.Clone();

            if (CustomInfo != null)
            {
                CustomInfo = srcProbe.CustomInfo.Clone();
            }

            return true;
        }

        public void UpdateTargetImage(Image2D targetImage)
        {
            target.Image = targetImage;
        }

        public void Offset(SizeF offset)
        {
            baseRegion.Offset(offset.Width, offset.Height);
        }

        public virtual string GetFigurePropertyName()
        {
            return ProbeType.ToString();
        }

        public virtual RotatedRect GetAlignedRegion(AlignedRegionParam alignedRegionParam)
        {
            RotatedRect probeAlignedRect = baseRegion;
            if (alignedRegionParam.GlobalPositionAligner != null)
            {
                probeAlignedRect = alignedRegionParam.GlobalPositionAligner.AlignFov(probeAlignedRect, alignedRegionParam.RobotPos);
            }

            if (alignedRegionParam.LocalPositionAligner != null)
            {
                probeAlignedRect = alignedRegionParam.LocalPositionAligner.Align(probeAlignedRect);
            }

            return probeAlignedRect;
        }

        public RotatedRect GetAlignedRegion(PositionAligner positionAligner, RotatedRect rotatedRect)
        {
            if (positionAligner != null)
            {
                InspectStep inspectStep = target.InspectStep;
                PointF stepPos = positionAligner.Align(inspectStep.Position.ToPointF());
                return positionAligner.AlignFov(rotatedRect, stepPos);
            }

            return rotatedRect;
        }

        public void AppendFigures(PositionAligner positionAligner, FigureGroup workingFigures, FigureGroup backgroundFigures, CanvasPanel.Option option)
        {
            RotatedRect probeRect = baseRegion;
            if (positionAligner != null)
            {
                probeRect = GetAlignedRegion(positionAligner, probeRect);
            }

            BuildFigures(probeRect, workingFigures, backgroundFigures, option);

            if (option.ShowProbeNumber)
            {
                var textFigure = new TextFigure(Id.ToString(), new Point((int)probeRect.X, (int)probeRect.Y),
                    new Font("Arial", option.ProbeNumberSize), Color.Red);

                backgroundFigures.AddFigure(textFigure);
            }
        }

        private void BuildFigures(RotatedRect probeRect, FigureGroup workingFigures, FigureGroup backgroundFigures, CanvasPanel.Option option)
        {
            RectangleFigure workingFigure;
            if (option.Pen == null)
            {
                string figurePropertyName = GetFigurePropertyName();
                workingFigure = new RectangleFigure(probeRect, figurePropertyName);
            }
            else
            {
                workingFigure = new RectangleFigure(probeRect, option.Pen);
            }

            workingFigure.Selectable = IsSelectable;
            workingFigure.Tag = this;
            workingFigure.ObjectLevel = 1;
            workingFigure.Selectable = true;

            if (Selected == true)
            {
                BuildSelectedFigures(probeRect, backgroundFigures);
            }

            workingFigures.AddFigure(workingFigure);
        }

        public virtual void BuildSelectedFigures(RotatedRect probeRect, FigureGroup backgroundFigures)
        {

        }

        public void UpdateRegion(RotatedRect newRegion)
        {
            baseRegion = newRegion;

            target.UpdateRegion();
        }

        public virtual string[] GetPreviewNames()
        {
            return new string[] { "None" };
        }

        public virtual void PreviewFilterResult(RotatedRect inspectRegion, ImageD imageCloned, int filterType)
        {

        }

        public ProbeResult Inspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            ProbeResult probeResult;
#if DEBUG
            probeResult = DoInspect(inspectParam, probeResultList);
            if (probeResult == null)
            {
                LogHelper.Warn(LoggerType.Inspection, string.Format("Invalid Inspection. Probe Result is null. Probe - {0}.{1}", Target.FullId, Id));
                return CreateDefaultResult();
            }
#else
            try
            {
                probeResult = DoInspect(inspectParam, probeResultList);
                if (probeResult == null)
                {
                    LogHelper.Warn(LoggerType.Inspection, String.Format("Invalid Inspection. Probe Result is null. Probe - {0}.{1}", Target.FullId, Id));
                    return CreateDefaultResult();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(String.Format("Inspection Exception :. Probe - {0}.{1} / Msg - {2}", Target.FullId, Id, ex.ToString()));
                probeResult = CreateDefaultResult();
                probeResultList.AddProbeResult(probeResult);
                return probeResult;
            }
#endif
            if ((probeResult is VisionProbeResult) == false)
            {
                probeResult.ProbeRegion = baseRegion;
                probeResult.InspectRegion = new RotatedRect(0, 0, baseRegion.Width, baseRegion.Height, 0);
            }

            if (ProbeResultType == ProbeResultType.Judge)
            {
                if (InverseResult)
                {
                    probeResult.InvertJudgment();
                }

                if (ModelVerification && probeResult.IsNG())
                {
                    probeResult.DifferentProductDetected = true;
                }
            }

            return probeResult;
        }

        public virtual void PrepareInspection()
        {

        }

        public abstract void OnPreInspection();
        public abstract ProbeResult CreateDefaultResult();
        public abstract ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList);
        public abstract void OnPostInspection();

        // ID가 같으면 같은 프로브이다.
        public override bool Equals(object obj)
        {
            if (!(obj is Probe))
            {
                return false;
            }

            var probe = (Probe)obj;
            return FullId == probe.FullId;
        }

        public override int GetHashCode()
        {
            return 1877310944 + Id.GetHashCode();
        }

        public void AddToParent()
        {
            target.AddProbe(this);
        }
    }
}
