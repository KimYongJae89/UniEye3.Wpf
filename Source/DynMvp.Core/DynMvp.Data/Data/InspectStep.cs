using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DynMvp.Data
{
    public enum LightParamSource
    {
        Global,
        Model,
        InspectionStep
    }

    public class InspectStep
    {
        public const int StepAll = 0xffff;
        public int StepNo { get; set; }
        public int InspectOrder { get; set; }
        public string Name { get; set; } = "";
        public List<Target> TargetList { get; } = new List<Target>();

        public int NumTargets => TargetList.Count;
        public AxisPosition Position { get; set; } = null;
        public LightParamSource LightParamSource { get; set; } = LightParamSource.Global;
        public LightParamSet LightParamSet { get; set; } = new LightParamSet();
        public StepModel OwnerModel { get; set; }
        public RectangleF FovRect { get; set; }
        public object Tag { get; set; }

        public InspectStep(int stepNo)
        {
            StepNo = stepNo;
        }

        public string GetStepName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return StepNo.ToString();
            }

            return Name;
        }

        public InspectStep Clone()
        {
            var inspectStep = new InspectStep(StepNo);
            inspectStep.Copy(this);

            return inspectStep;
        }

        public void Copy(InspectStep srcInspectionStep)
        {
            Clear();

            LightParamSet = srcInspectionStep.LightParamSet.Clone();

            foreach (Target target in srcInspectionStep.TargetList)
            {
                var newTarget = (Target)target.Clone();
                newTarget.InspectStep = this;
                TargetList.Add(newTarget);
            }
        }

        public void GetInspectOrderRange(out int min, out int max)
        {
            int minValue = int.MaxValue;
            int maxValue = 0;

            TargetList.ForEach(x => { minValue = Math.Min(x.InspectOrder, minValue); maxValue = Math.Max(x.InspectOrder, maxValue); });

            min = minValue;
            max = maxValue;
        }

        public void SyncParam(Probe probe, IProbeFilter probeFilter)
        {
            foreach (Target target in TargetList)
            {
                target.SyncParam(probe, probeFilter);
            }
        }

        public void GetProbe(IProbeFilter probeFilter, List<Probe> probeList)
        {
            foreach (Target target in TargetList)
            {
                target.GetProbe(probeFilter, probeList);
            }
        }

        public Probe GetProbe(string targetIdOrName, int probeId)
        {
            Target target = GetTarget(targetIdOrName);
            if (target == null)
            {
                return null;
            }

            if (probeId >= target.NumProbe)
            {
                return null;
            }

            return target[probeId];
        }

        public void Clear()
        {
            foreach (Target target in TargetList)
            {
                target.Clear();
            }

            TargetList.Clear();
        }

        public IEnumerator<Target> GetEnumerator()
        {
            return TargetList.GetEnumerator();
        }

        public void AddTarget(Target newTarget)
        {
            newTarget.InspectStep = this;

            int nextTargetId = 0;
            foreach (Target target in TargetList)
            {
                if (target.Id == nextTargetId)
                {
                    nextTargetId++;
                    if (nextTargetId >= int.MaxValue)
                    {
                        throw new TooManyItemsException();
                    }

                    continue;
                }
            }

            newTarget.Id = nextTargetId;
            TargetList.Add(newTarget);
        }

        public void RemoveTarget(Target targetRemoved)
        {
            TargetList.Remove(targetRemoved);
        }

        public List<Target> GetCamTargets(int cameraIndex)
        {
            var camTargetList = new List<Target>();

            foreach (Target target in TargetList)
            {
                if (target.CameraIndex == cameraIndex)
                {
                    camTargetList.Add(target);
                }
            }

            return camTargetList;
        }

        public virtual void OnPreInspection()
        {
            foreach (Target target in TargetList)
            {
                target.OnPreInspection();
            }
        }

        public virtual void OnPostInspection()
        {
            foreach (Target target in TargetList)
            {
                target.OnPostInspection();
            }
        }

        public Probe GetProbe(string probeFullIdOrName)
        {
            foreach (Target target in TargetList)
            {
                Probe probe = target.GetProbe(probeFullIdOrName);
                if (probe != null)
                {
                    return probe;
                }
            }

            return null;
        }

        public Target GetTarget(int index)
        {
            if (TargetList.Count > index)
            {
                return TargetList[index];
            }

            return null;
        }

        public Target GetTarget(string targetFullIdOrName)
        {
            foreach (Target target in TargetList)
            {
                if (target.FullId == targetFullIdOrName || target.Name == targetFullIdOrName)
                {
                    return target;
                }
            }

            return null;
        }

        public void GetTargets(List<Target> targetList)
        {
            foreach (Target target in TargetList)
            {
                targetList.Add(target);
            }
        }

        public List<Target> GetTargets(string targetType)
        {
            var targetList = new List<Target>();

            foreach (Target target in TargetList)
            {
                if (target.TypeName == targetType)
                {
                    targetList.Add(target);
                }
            }

            return targetList;
        }

        public List<Target> GetTargets(int cameraIndex, int inspectOrder)
        {
            var targetList = new List<Target>();

            foreach (Target target in TargetList)
            {
                if (target.CameraIndex == cameraIndex && target.InspectOrder == inspectOrder)
                {
                    targetList.Add(target);
                }
            }

            return targetList;
        }

        public List<Target> GetTargets(int cameraIndex)
        {
            var targetList = new List<Target>();

            foreach (Target target in TargetList)
            {
                if (target.CameraIndex == cameraIndex)
                {
                    targetList.Add(target);
                }
            }

            return targetList;
        }

        public void GetTargetTypes(List<string> targetTypeList)
        {
            foreach (Target target in TargetList)
            {
                if (string.IsNullOrEmpty(target.TypeName))
                {
                    continue;
                }

                int index = targetTypeList.IndexOf(target.TypeName);
                if (index == -1)
                {
                    targetTypeList.Add(target.TypeName);
                }
            }
        }

        public void GetValueProbes(List<Probe> probeList)
        {
            foreach (Target target in TargetList)
            {
                target.GetValueProbes(probeList);
            }
        }

        public int GetNumTarget()
        {
            var targetList = new List<Target>();
            GetTargets(targetList);

            return targetList.Count;
        }

        public bool IsEmpty()
        {
            return TargetList.Count() == 0;
        }

        public void CreateObjectFigures(FigureGroup figureGroup)
        {
            foreach (Target target in TargetList)
            {
                target.CreateObjectFigures(figureGroup);
            }
        }

        public void AddSchemaFigure(Schema schema)
        {
            var rectangleFigure = new RectangleFigure(FovRect, new Pen(Color.Gray), null);
            rectangleFigure.Id = "rectangle";

            var schemaFigure = new SchemaFigure();
            schemaFigure.Name = Name;//StepName;// "Step";
            schemaFigure.Selectable = true;
            schemaFigure.Movable = false;
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.Tag = StepNo.ToString();

            schema.AddFigure(schemaFigure/*, "Step"*/);

            foreach (Target target in TargetList)
            {
                target.AddSchemaFigure(schema);
            }
        }

        public LightParamSet GetLightParamSet()
        {
            switch (LightParamSource)
            {
                case LightParamSource.Global:
                    return LightConfig.Instance().LightParamSet;
                case LightParamSource.Model:
                    return OwnerModel.LightParamSet;
                default:
                case LightParamSource.InspectionStep:
                    return LightParamSet;
            }
        }

        public void SetLightParamSet(LightParamSet lightParamSet)
        {
            switch (LightParamSource)
            {
                case LightParamSource.Global:
                    LightConfig.Instance().LightParamSet = lightParamSet;
                    break;
                case LightParamSource.Model:
                    OwnerModel.LightParamSet = lightParamSet;
                    break;
                default:
                case LightParamSource.InspectionStep:
                    LightParamSet = lightParamSet;
                    break;
            }
        }

        public int[] GetLightTypeIndexArr(int cameraIndex = -1)
        {
            var lightTypeList = new List<int>();

            foreach (Target target in TargetList)
            {
                if (cameraIndex != -1 && target.CameraIndex == cameraIndex)
                {
                    continue;
                }

                List<int> lightTypeList2 = target.GetLightTypeIndexList();
                foreach (int lightType in lightTypeList2)
                {
                    if (lightTypeList.Contains(lightType) == false)
                    {
                        lightTypeList.Add(lightType);
                    }
                }
            }

            lightTypeList.Sort();

            return lightTypeList.ToArray();
        }

        public Calibration GetCalibration(int cameraIndex, ProbeResultList probeResultList)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.CameraIndex == cameraIndex && probeResult.TargetType == Target.TypeCalibration)
                {
                    var calibrationProbeResult = (VisionProbeResult)probeResult;
                    ResultValue resultValue = calibrationProbeResult.GetResultValue("Calibration");
                    if (resultValue == null)
                    {
                        return null;
                    }

                    return resultValue.Value as Calibration;
                }
            }

            return null;
        }

        public PositionAligner GetPositionAligner(int cameraIndex, ProbeResultList probeResultList)
        {
            var fiducialSet = new FiducialSet();

            foreach (Target target in TargetList)
            {
                if (target.CameraIndex == cameraIndex && target.TypeName == Target.TypeGlobalFiducial)
                {
                    fiducialSet.AddFiducial(target[0]);
                }
            }

            return fiducialSet.Calculate(probeResultList);
        }

        public void AppendFigures(FigureGroup workingFigures, FigureGroup backgroundFigures, CanvasPanel.Option option)
        {
            foreach (Target target in TargetList)
            {
                target.AppendFigures(null, workingFigures, backgroundFigures, option);
            }
        }

        public void AppendFigures(int cameraIdex, PositionAligner positionAligner, FigureGroup workingFigures, FigureGroup backgroundFigures, CanvasPanel.Option option)
        {
            foreach (Target target in TargetList)
            {
                if (target.CameraIndex == cameraIdex)
                {
                    target.AppendFigures(positionAligner, workingFigures, backgroundFigures, option);
                }
            }
        }
    }
}
