using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace DynMvp.Data
{
    public enum TeachError
    {
        InvalidState,
        Merge3DState,
    }

    public abstract class ModelBase
    {
        public string Name => modelDescription.Name;

        protected float fileVersion;
        public float FileVersion
        {
            get => fileVersion;
            set => fileVersion = value;
        }

        protected bool modified;
        public bool Modified
        {
            get => modified;
            set => modified = value;
        }

        protected string modelPath;
        public string ModelPath
        {
            get => modelPath;
            set => modelPath = value;
        }

        protected ModelDescription modelDescription = new ModelDescription();
        public ModelDescription ModelDescription
        {
            get => modelDescription;
            set => modelDescription = value;
        }

        protected Production production = new Production();
        public Production Production => production;

        public virtual void CopyFrom(ModelBase model)
        {
            model.modelDescription.CopyTo(modelDescription);
            modelPath = model.modelPath;
            fileVersion = model.fileVersion;
            modified = model.modified;
            production = model.production.Clone();
        }

        public abstract ModelBase Clone();
        public abstract bool IsEmpty();
        public abstract void Clear();
        public abstract void BuildModel();

        public void LoadProduction()
        {
            string filePath = string.Format("{0}\\Production.xml", modelPath);
            if (File.Exists(filePath))
            {
                production.Load(filePath);
            }
        }

        public void SaveProduction()
        {
            string filePath = string.Format("{0}\\Production.xml", modelPath);
            if (File.Exists(filePath))
            {
                production.Save(filePath);
            }
        }

        public virtual bool OpenModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        {
            try
            {
                LoadProduction();

                string filePath = string.Format("{0}\\ModelDescription.xml", ModelPath);
                modelDescription.Load(filePath);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Model Save Error : " + ex.Message);
            }

            return false;
        }

        public virtual void SaveModel(IReportProgress reportProgress = null)
        {
            try
            {
                string filePath = string.Format("{0}\\ModelDescription.xml", modelPath);
                modelDescription.Save(filePath);

                modified = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Model Save Error : " + ex.Message);
            }
        }

        public virtual void Import(ValueTable<string> valueTable)
        {

        }

        public virtual void Export(ValueTable<string> valueTable)
        {

        }
    }

    public class StepModel : ModelBase
    {
        public string GetImagePath()
        {
            return Path.Combine(modelPath, "Image");
        }
        public LightParamSet LightParamSet { get; set; } = new LightParamSet();
        public List<int> ExposureTimeUsList { get; set; } = new List<int>();

        public int NumInspectStep => inspectStepList.Count;
        public Schema ModelSchema { get; set; } = new Schema();

        protected List<InspectStep> inspectStepList = new List<InspectStep>();
        public List<InspectStep> InspectStepList => inspectStepList;
        public List<FiducialSet> FiducialSetList { get; } = new List<FiducialSet>();

        public StepModel()
        {
            ExposureTimeUsList = new List<int>();

            foreach (Camera camera in DeviceManager.Instance().CameraHandler)
            {
                for (int i = 0; i < DeviceManager.Instance().CameraHandler.NumCamera; i++)
                {
                    ExposureTimeUsList.Add((int)camera.GetExposureTime());
                }
            }
        }

        public override ModelBase Clone()
        {
            var stepModel = new StepModel();
            stepModel.CopyFrom(this);

            return stepModel;
        }

        public override void CopyFrom(ModelBase model)
        {
            base.CopyFrom(model);

            var stepModel = new StepModel();

            production = stepModel.production.Clone();
            LightParamSet = stepModel.LightParamSet.Clone();

            ModelSchema = stepModel.ModelSchema.Clone();

            foreach (InspectStep inspectStep in stepModel.inspectStepList)
            {
                inspectStepList.Add(inspectStep.Clone());
            }
        }

        public override void Clear()
        {
            inspectStepList.ForEach(x => x.Clear());
            inspectStepList.Clear();
        }

        public IEnumerator<InspectStep> GetEnumerator()
        {
            return inspectStepList.GetEnumerator();
        }

        /// <summary>
        /// 각 Target을 순회하면서 TargetType에 따른 우선 순위를 찾고, 찾아진 우선 순위값 중 제일 큰 값을 반환한다.
        /// 우선 순위가 낮은 값을 가진 Target이 먼저 검사 된다.
        /// 기본적인 모델은 모든 Target의 우선 순위가 같기 때문에 0을 반환한다. 상속된 모델에서 구현해야 한다.
        /// </summary>
        /// <returns></returns>
        public void GetInspectOrderRange(out int min, out int max)
        {
            int minValue = int.MaxValue;
            int maxValue = 0;

            inspectStepList.ForEach(x => { minValue = Math.Min(x.InspectOrder, minValue); maxValue = Math.Max(x.InspectOrder, maxValue); });

            min = minValue;
            max = maxValue;
        }

        public List<InspectStep> GetInspectStepList(int inspectOrder = -1)
        {
            if (inspectOrder == -1)
            {
                return inspectStepList;
            }

            return inspectStepList.FindAll(x => x.InspectOrder == inspectOrder);
        }

        public List<InspectStep> GetInspectStepList()
        {
            return inspectStepList;
        }

        public virtual void UpdateInspectOrder()
        {

        }

        public virtual bool IsTaught()
        {
            return inspectStepList.Sum(x => x.TargetList.Count()) > 0;
        }

        public override bool IsEmpty()
        {
            return inspectStepList.Sum(x => x.TargetList.Count()) == 0;
        }

        public void AddInspectStep(InspectStep inspectStep)
        {
            inspectStepList.Add(inspectStep);
        }

        public void RemoveInspectStep(InspectStep inspectStep)
        {
            inspectStepList.Remove(inspectStep);

            UpdateStepNo();

            CleanImage();
        }

        public void UpdateStepNo()
        {
            int index = 0;
            inspectStepList.ForEach(x => x.StepNo = index++);
        }

        public void CreateInspectionStepList(int numInspectionStep)
        {
            inspectStepList.Clear();

            for (int i = 0; i < numInspectionStep; i++)
            {
                var inspectStep = new InspectStep(i);
                inspectStep.OwnerModel = this;

                inspectStepList.Add(inspectStep);
            }
        }

        public InspectStep CreateInspectionStep()
        {
            var inspectStep = new InspectStep(inspectStepList.Count);
            inspectStep.OwnerModel = this;

            inspectStepList.Add(inspectStep);

            return inspectStep;
        }

        public void OnPreInspection()
        {
            inspectStepList.ForEach(x => x.OnPreInspection());
        }

        public void OnPostInspection()
        {
            inspectStepList.ForEach(x => x.OnPostInspection());
        }

        public void LinkFiducial()
        {
            foreach (FiducialSet fiducialSet in FiducialSetList)
            {
                fiducialSet.LinkFiducial(this);
            }
        }

        public InspectStep GetInspectStep(int stepNo)
        {
            return inspectStepList.Find(x => x.StepNo == stepNo);
        }

        public InspectStep GetInspectStep(int inspectionStepNo, bool fCreateOnEmpty = false)
        {
            foreach (InspectStep inspectStep in inspectStepList)
            {
                if (inspectStep.StepNo == inspectionStepNo)
                {
                    return inspectStep;
                }
            }

            if (fCreateOnEmpty)
            {
                var inspectStep = new InspectStep(inspectionStepNo);
                inspectStep.OwnerModel = this;

                inspectStepList.Add(inspectStep);

                return inspectStep;
            }

            return null;
        }

        public virtual Probe GetProbe(string probeFullIdOrName)
        {
            foreach (InspectStep inspectStep in inspectStepList)
            {
                Probe probe = inspectStep.GetProbe(probeFullIdOrName);
                if (probe != null)
                {
                    return probe;
                }
            }

            return null;
        }

        public void GetProbe(IProbeFilter filter, List<Probe> probeList)
        {
            inspectStepList.ForEach(x => x.GetProbe(filter, probeList));
        }

        public Target GetTarget(string targetFullIdOrName)
        {
            foreach (InspectStep inspectStep in inspectStepList)
            {
                Target target = inspectStep.GetTarget(targetFullIdOrName);
                if (target != null)
                {
                    return target;
                }
            }

            return null;
        }

        public void GetTargets(List<Target> targetList)
        {
            inspectStepList.ForEach(x => x.GetTargets(targetList));
        }

        public void GetTargetTypes(List<string> targetTypeList)
        {
            inspectStepList.ForEach(x => x.GetTargetTypes(targetTypeList));
        }

        public void GetValueProbes(List<Probe> probeList)
        {
            inspectStepList.ForEach(x => x.GetValueProbes(probeList));
        }

        public int GetNumTarget()
        {
            var targetList = new List<Target>();
            GetTargets(targetList);

            return targetList.Count;
        }

        protected virtual void OnPostLoadModel()
        {

        }

        public override void BuildModel()
        {
            LightParamSet = LightConfig.Instance().LightParamSet.Clone();
        }

        public override void SaveModel(IReportProgress reportProgress = null)
        {
            string tempModelFilePath = string.Format("{0}\\~Model.xml", ModelPath);

            StepModelWriter modelWriter = StepModelWriterBuilder.Create((float)2.0);
            modelWriter.Write(this, tempModelFilePath, null);

            string modelFilePath = string.Format("{0}\\Model.xml", ModelPath);
            string bakModelFilePath = string.Format("{0}\\Model.xml.bak", ModelPath);

            FileHelper.SafeSave(tempModelFilePath, bakModelFilePath, modelFilePath);

            SaveModelSchema();

            base.SaveModel(reportProgress);
        }

        public override bool OpenModel(IReportProgress reportProgress, CreateCustomInfoDelegate CreateCustomInfo = null)
        {
            Debug.Assert(string.IsNullOrEmpty(modelPath) == false, "Model Path must be set in child class");

            string modelFilePath = string.Format("{0}\\Model.xml", ModelPath);
            if (File.Exists(modelFilePath))
            {
                StepModelReader modelReader = StepModelReaderBuilder.Create(modelFilePath);

                modelReader.Initialize(CreateCustomInfo);
                modelReader.Load(this, modelFilePath, reportProgress);

                LoadModelSchema();

                base.OpenModel(reportProgress, CreateCustomInfo);

                return true;
            }

            return false;
        }

        public void LoadModelSchema()
        {
            string filePath = string.Format("{0}\\ModelSchema.xml", ModelPath);
            if (File.Exists(filePath))
            {
                ModelSchema.Load(filePath);
            }
        }

        public void SaveModelSchema()
        {
            string filePath = string.Format("{0}\\ModelSchema.xml", ModelPath);
            ModelSchema.Save(filePath);
        }

        public void LinkSchemaFigures(Schema schema = null)
        {
            if (schema == null)
            {
                schema = ModelSchema;
            }

            List<InspectStep> inspectStepList = GetInspectStepList();

            foreach (InspectStep inspectStep in inspectStepList)
            {
                foreach (Target target in inspectStep)
                {
                    if (target.SchemaFigures == null)
                    {
                        target.SchemaFigures = schema.GetFigureByName(target.Name);
                    }

                    foreach (Probe probe in target.ProbeList)
                    {
                        if (probe.SchemaFigures == null)
                        {
                            probe.SchemaFigures = schema.GetFigureByName(probe.Name);
                        }
                    }
                }
            }
        }

        public void UnlinkSchemaFigures()
        {
            List<InspectStep> inspectStepList = GetInspectStepList();

            foreach (InspectStep inspectStep in inspectStepList)
            {
                foreach (Target target in inspectStep)
                {
                    target.SchemaFigures = null;

                    foreach (Probe probe in target.ProbeList)
                    {
                        probe.SchemaFigures = null;
                    }
                }
            }
        }

        public void CleanImage()
        {
            string imagePath = Path.Combine(ModelPath, "Image");
            if (Directory.Exists(imagePath) == true)
            {
                Directory.Delete(imagePath, true);
            }
        }

        public RotatedRect GetDefaultProbeRegion(Calibration cameraCalibration)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - GetDefaultProbeRegion");

            var rectangle = new Rectangle(0, 0, cameraCalibration.ImageSize.Width, cameraCalibration.ImageSize.Height);

            float centerX = cameraCalibration.ImageSize.Width / 2;
            float centerY = cameraCalibration.ImageSize.Height / 2;

            float width = cameraCalibration.ImageSize.Width / 4;
            float height = cameraCalibration.ImageSize.Height / 4;

            float left = centerX - width / 2;
            float top = centerY - height / 2;
            return new RotatedRect(left, top, width, height, 0);
        }

        protected IFilter AutoAddFilter(FilterType filterType)
        {
            IFilter filter = null;

            switch (filterType)
            {
                case FilterType.Binarize:
                    filter = new BinarizeFilter(BinarizationType.SingleThreshold, 40, 40);
                    break;
                case FilterType.Average:
                    filter = new AverageFilter();
                    break;
                case FilterType.EdgeExtraction:
                    filter = new EdgeExtractionFilter(3);
                    break;
                case FilterType.HistogramEqualization:
                    filter = new HistogramEqualizationFilter();
                    break;
                case FilterType.Morphology:
                    filter = new MorphologyFilter(MorphologyType.Erode, 3);
                    break;
            }
            return filter;
        }

        public void GetGlobalFiducialStep(FiducialSet fiducialSet)
        {
            GetFiducialStep(fiducialSet, Target.TypeGlobalFiducial);
        }

        public void GetLocalFiducialStep(FiducialSet fiducialSet)
        {
            GetFiducialStep(fiducialSet, Target.TypeLocalFiducial);
        }

        public void GetFiducialStep(FiducialSet fiducialSet, string typeName)
        {
            Debug.Assert(false);

            fiducialSet.Clear();

            foreach (InspectStep inspectStep in InspectStepList)
            {
                Target target = inspectStep.GetTarget(typeName);

                Probe probe = target[0];
                if (probe != null && probe is VisionProbe)
                {
                    fiducialSet.AddFiducial(probe);
                }
            }
        }

        public Schema AutoSchema(CancellationToken cancellationToken)
        {
            RectangleF unionRect = RectangleF.Empty;

            var schema = new Schema();

            foreach (InspectStep inspectStep in inspectStepList)
            {
                RectangleF rectangle = inspectStep.FovRect;

                if (unionRect == RectangleF.Empty)
                {
                    unionRect = rectangle;
                }
                else
                {
                    unionRect = RectangleF.Union(rectangle, unionRect);
                }

                inspectStep.AddSchemaFigure(schema);

                cancellationToken.ThrowIfCancellationRequested();
            }

            schema.Region = unionRect;
            schema.InvertY = true;

            return schema;
        }

    }
}
