using DynMvp.Base;
using DynMvp.Data.Library;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Data
{
    public delegate ProbeCustomInfo CreateCustomInfoDelegate();
    public delegate void OnModelLoaded();

    public class StepModelReaderBuilder
    {
        public static StepModelReader Create(string modelPath)
        {
            if (System.IO.File.Exists(modelPath) == false)
            {
                return new StepModelReader();
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(modelPath);

            XmlElement modelElement = xmlDocument.DocumentElement;

            float fileVersion = Convert.ToSingle(XmlHelper.GetAttributeValue(modelElement, "Version", "1.0"));

            StepModelReader modelReader;
            switch (fileVersion)
            {
                default:
                case 1:
                    modelReader = new StepModelReader();
                    break;
            }

            return modelReader;
        }
    }

    public class StepModelReader
    {
        protected CreateCustomInfoDelegate CreateCustomInfo;

        protected StringBuilder errorLog = new StringBuilder();

        public void Initialize(CreateCustomInfoDelegate CreateCustomInfo)
        {
            this.CreateCustomInfo = CreateCustomInfo;
        }

        public void Load(StepModel model, string modelPath, IReportProgress reportProgress)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(modelPath);

            XmlElement modelElement = xmlDocument.DocumentElement;

            model.FileVersion = Convert.ToSingle(XmlHelper.GetAttributeValue(modelElement, "Version", "1.0"));

            //model.LightParamSource = (LightParamSource)Enum.Parse(typeof(LightParamSource), XmlHelper.GetAttributeValue(modelElement, "LightParamSource", "Model"));
            // 모델의 LightParamSet을 불러온다.
            XmlElement lightParamSetElement = modelElement["LightParamSet"];
            if (lightParamSetElement != null)
            {
                model.LightParamSet.Load(lightParamSetElement);
            }

            XmlElement exposureTimeListElement = modelElement["ExposureTimeList"];
            if (exposureTimeListElement != null)
            {
                LoadExposureTime(model, exposureTimeListElement);
            }

            LoadTargets(model, modelElement, reportProgress);

            XmlElement fiducialSetListElement = modelElement["FiducialSetList"];
            if (fiducialSetListElement != null)
            {
                foreach (XmlElement fiducialSetElement in fiducialSetListElement)
                {
                    if (fiducialSetElement.Name == "FiducialSet")
                    {
                        var fiducialSet = new FiducialSet();
                        LoadFiducialSet(fiducialSet, fiducialSetElement);
                    }
                }
            }
        }

        public bool Load(Template template, string templatePath)
        {
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(templatePath);

                XmlElement templateElement = xmlDocument.DocumentElement;

                template.FileVersion = Convert.ToSingle(XmlHelper.GetAttributeValue(templateElement, "Version", "1.0"));
                template.Path = templatePath;
                template.Name = XmlHelper.GetAttributeValue(templateElement, "Name", "");
                template.SetTag(XmlHelper.GetAttributeValue(templateElement, "Tag", ""));
                string guid = XmlHelper.GetAttributeValue(templateElement, "Id", "");
                template.Id = new Guid(guid);
                template.FileInfo = new FileInfo(templatePath);

                XmlElement targetElement = templateElement["Target"];
                if (targetElement != null)
                {
                    var target = new Target();
                    LoadTarget(target, targetElement);

                    template.Target = target;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Load(TemplatePool templatePool, string templatePoolPath)
        {
            string[] dirs = Directory.GetDirectories(templatePoolPath);

            foreach (string dir in dirs)
            {
                var templateGroup = new TemplateGroup(Path.GetFileName(dir));

                Load(templateGroup, dir);

                templatePool.AddTemplateGroup(templateGroup);
            }
        }

        public void Load(TemplateGroup templateGroup, string templateGroupPath)
        {
            string[] dirs = Directory.GetFiles(templateGroupPath);

            foreach (string dir in dirs)
            {
                var template = new Template(Path.GetFileName(dir));

                Load(template, dir);

                templateGroup.AddTemplate(template);
            }
        }

        protected void LoadExposureTime(StepModel model, XmlElement exposureTimeListElement)
        {
            foreach (XmlElement exposureTimeElement in exposureTimeListElement)
            {
                if (exposureTimeElement.Name == "ExposureTime")
                {
                    int cameraIndex = Convert.ToInt32(XmlHelper.GetAttributeValue(exposureTimeElement, "CameraIndex", "0"));
                    int exposureTime = Convert.ToInt32(XmlHelper.GetAttributeValue(exposureTimeElement, "Value", "10000"));

                    if (model.ExposureTimeUsList.Count > cameraIndex)
                    {
                        model.ExposureTimeUsList[cameraIndex] = exposureTime;
                    }
                }
            }
        }

        protected void LoadFiducialSet(FiducialSet fiducialSet, XmlElement fiducialSetElement)
        {
            fiducialSet.Index = Convert.ToInt32(XmlHelper.GetAttributeValue(fiducialSetElement, "Index", "0"));

            foreach (XmlElement fiducialElement in fiducialSetElement)
            {
                if (fiducialElement.Name == "Fiducial")
                {
                    var fiducialInfo = new FiducialInfo();
                    fiducialInfo.StepNo = Convert.ToInt32(XmlHelper.GetAttributeValue(fiducialElement, "StepNo", "1"));
                    fiducialInfo.TargetId = Convert.ToInt32(XmlHelper.GetAttributeValue(fiducialElement, "TargetId", "1"));
                    fiducialInfo.ProbeId = Convert.ToInt32(XmlHelper.GetAttributeValue(fiducialElement, "ProbeId", "1"));

                    fiducialSet.FiducialInfoList.Add(fiducialInfo);
                }
            }
        }

        protected void LoadTarget(Target target, XmlElement targetElement)
        {
            if (targetElement == null)
            {
                return;
            }

            target.Id = Convert.ToInt32(XmlHelper.GetAttributeValue(targetElement, "Id", "-1"));
            if (target.Id == -1)
            {
                throw new Base.InvalidDataException();
            }

            target.ImageEncodedString = XmlHelper.GetAttributeValue(targetElement, "Image", "");
            target.Name = XmlHelper.GetAttributeValue(targetElement, "Name", "");
            target.ModuleNo = Convert.ToInt32(XmlHelper.GetAttributeValue(targetElement, "ModuleNo", "0"));
            target.TypeName = XmlHelper.GetAttributeValue(targetElement, "Type", "");
            target.UseInspection = Convert.ToBoolean(XmlHelper.GetAttributeValue(targetElement, "UseInspection", "True"));
            target.InspectionLogicType = (InspectionLogicType)Enum.Parse(typeof(InspectionLogicType), XmlHelper.GetAttributeValue(targetElement, "InspectionLogicType", "And"));
            target.InspectOrder = Convert.ToInt32(XmlHelper.GetAttributeValue(targetElement, "InspectOrder", "0"));
            target.CameraIndex = Convert.ToInt32(XmlHelper.GetAttributeValue(targetElement, "CameraIndex", "0"));

            var targetRegion = new RotatedRect();
            XmlHelper.GetValue(targetElement, "Region", ref targetRegion);
            target.BaseRegion = targetRegion;

            foreach (XmlElement probeElement in targetElement)
            {
                if (probeElement.Name == "Probe")
                {
                    string probeTypeStr = XmlHelper.GetAttributeValue(probeElement, "ProbeType", "");
                    if (probeTypeStr == "")
                    {
                        throw new Base.InvalidDataException();
                    }

                    var probeType = (ProbeType)Enum.Parse(typeof(ProbeType), probeTypeStr);

                    try
                    {
                        Probe probe = ProbeFactory.Create(probeType);

                        LoadProbe(probe, probeElement);

                        target.AddProbe(probe);
                    }
                    catch (Base.InvalidDataException ex)
                    {
                        errorLog.AppendFormat("Some error occurred on Target {0}\n", target.FullId);
                        errorLog.AppendLine(ex.Message);
                    }
                }
            }

            target.LinkFiducialProbe();
        }

        protected void LoadProbe(Probe probe, XmlElement probeElement)
        {
            if (probeElement == null)
            {
                return;
            }

            probe.Id = Convert.ToInt32(XmlHelper.GetAttributeValue(probeElement, "Id", "-1"));
            if (probe.Id == -1)
            {
                throw new Base.InvalidDataException("Probe ID 0 is invalid");
            }

            probe.Name = XmlHelper.GetAttributeValue(probeElement, "Name", "");
            probe.FiducialProbeId = Convert.ToInt32(XmlHelper.GetAttributeValue(probeElement, "FiducialProbeId", "-1"));
            probe.InverseResult = Convert.ToBoolean(XmlHelper.GetAttributeValue(probeElement, "InverseResult", "False"));
            probe.ModelVerification = Convert.ToBoolean(XmlHelper.GetAttributeValue(probeElement, "ModelVerification", "False"));
            probe.StepBlocker = Convert.ToBoolean(XmlHelper.GetAttributeValue(probeElement, "StepBlocker", "False"));

            if (CreateCustomInfo != null)
            {
                probe.CustomInfo = CreateCustomInfo();
                probe.CustomInfo.Load(probeElement);
            }

            //probe.IsRectFigure = Convert.ToBoolean(XmlHelper.GetAttributeValue(probeElement, "IsRectFigure","false"));
            //probe.PointList.Add(new System.Drawing.PointF(0.0f, 0.5f));
            //probe.PointList.Add(new System.Drawing.PointF(1.0f, 0.5f));

            var probeRegion = new RotatedRect();
            XmlHelper.GetValue(probeElement, "Region", ref probeRegion);
            probe.BaseRegion = probeRegion;

            switch (probe.ProbeType)
            {
                case ProbeType.Vision:
                    LoadVisionProbe((VisionProbe)probe, probeElement);
                    break;
                case ProbeType.Io:
                    LoadIoProbe((IoProbe)probe, probeElement);
                    break;
                case ProbeType.Serial:
                    LoadSerialProbe((SerialProbe)probe, probeElement);
                    break;
                case ProbeType.Daq:
                    LoadDaqProbe((DaqProbe)probe, probeElement);
                    break;
                case ProbeType.Marker:
                    LoadMarkerProbe((MarkerProbe)probe, probeElement);
                    break;
                default:
                    throw new InvalidTypeException(string.Format("Invalid probe type : {0}", probe.ProbeType.ToString()));
            }

        }

        protected void LoadMarkerProbe(MarkerProbe markerProbe, XmlElement probeElement)
        {
            markerProbe.MarkerType = (MarkerType)Enum.Parse(typeof(MarkerType), XmlHelper.GetAttributeValue(probeElement, "MarkerType", MarkerType.MergeSource.ToString()));
            markerProbe.MergeSourceId = XmlHelper.GetAttributeValue(probeElement, "MergeSourceId", "");

            var mergeOffset = new Point3d();
            XmlHelper.GetValue(probeElement, "MergeOffset", ref mergeOffset);
            markerProbe.MergeOffset = mergeOffset;
        }

        protected void LoadIoProbe(IoProbe ioProbe, XmlElement probeElement)
        {
            ioProbe.DigitalIoName = XmlHelper.GetAttributeValue(probeElement, "DigitalIoName", "Default");
            ioProbe.PortNo = Convert.ToInt32(XmlHelper.GetAttributeValue(probeElement, "PortNo", "0"));
        }

        protected void LoadSerialProbe(SerialProbe serialprobe, XmlElement probeElement)
        {
            var worldRegion = new RotatedRect();
            XmlHelper.GetValue(probeElement, "WorldRegion", ref worldRegion);
            serialprobe.WorldRegion = worldRegion;

            serialprobe.PortName = XmlHelper.GetAttributeValue(probeElement, "PortName", "");
            serialprobe.UpperValue = Convert.ToSingle(XmlHelper.GetAttributeValue(probeElement, "UpperValue", "0.0"));
            serialprobe.LowerValue = Convert.ToSingle(XmlHelper.GetAttributeValue(probeElement, "LowerValue", "0.0"));
            serialprobe.NumSerialReading = Convert.ToInt32(XmlHelper.GetAttributeValue(probeElement, "NumSerialReading", "0"));
            serialprobe.InspectionSerialPort = SerialPortManager.Instance().GetSerialPort(serialprobe.PortName);
            if (serialprobe is TensionSerialProbe)
            {
                ((TensionSerialProbe)serialprobe).TensionFilePath = XmlHelper.GetAttributeValue(probeElement, "TensionFilePath", "");
                ((TensionSerialProbe)serialprobe).UnitType = (TensionUnitType)Enum.Parse(typeof(TensionUnitType), XmlHelper.GetAttributeValue(probeElement, "UnitType", "Newton"));
            }
        }

        protected void LoadDaqProbe(DaqProbe daqProbe, XmlElement probeElement)
        {
            string channelName = XmlHelper.GetAttributeValue(probeElement, "ChannelName", "");
            daqProbe.DaqChannel = DaqChannelManager.Instance().GetDaqChannel(channelName);

            daqProbe.UpperValue = Convert.ToSingle(XmlHelper.GetAttributeValue(probeElement, "UpperValue", "0.0"));
            daqProbe.LowerValue = Convert.ToSingle(XmlHelper.GetAttributeValue(probeElement, "LowerValue", "0.0"));
            daqProbe.NumSample = Convert.ToInt32(XmlHelper.GetAttributeValue(probeElement, "NumSample", "100"));
            daqProbe.UseLocalScaleFactor = Convert.ToBoolean(XmlHelper.GetAttributeValue(probeElement, "UseLocalScaleFactor", "False"));
            daqProbe.LocalScaleFactor = Convert.ToSingle(XmlHelper.GetAttributeValue(probeElement, "LocalScaleFactor", "0.0"));
            daqProbe.ValueOffset = Convert.ToSingle(XmlHelper.GetAttributeValue(probeElement, "ValueOffset", "0.0"));
            daqProbe.MeasureType = (DaqMeasureType)Enum.Parse(typeof(DaqMeasureType), XmlHelper.GetAttributeValue(probeElement, "MeasureType", "Absolute"));
            daqProbe.FilterType = (DaqFilterType)Enum.Parse(typeof(DaqFilterType), XmlHelper.GetAttributeValue(probeElement, "FilterType", "Average"));
            daqProbe.Target1Name = XmlHelper.GetAttributeValue(probeElement, "Target1Name", "");
            daqProbe.Target2Name = XmlHelper.GetAttributeValue(probeElement, "Target2Name", "");
        }

        public void LoadTargets(StepModel model, XmlElement modelElement, IReportProgress reportProgress)
        {
            int count = 0;

            foreach (XmlElement inspectionStepElement in modelElement)
            {
                if (inspectionStepElement.Name == "InspectionStep")
                {
                    var inspectStep = new InspectStep(count);
                    inspectStep.OwnerModel = model;

                    model.AddInspectStep(inspectStep);

                    LoadInspectionStep(inspectStep, inspectionStepElement);

                    count++;
                }
            }
        }

        private void LoadInspectionStep(InspectStep inspectStep, XmlElement inspectionStepElement)
        {
            if (inspectionStepElement["AxisPosition"] != null)
            {
                var basePosition = new AxisPosition();
                basePosition.GetValue(inspectionStepElement, "AxisPosition");

                inspectStep.Position = basePosition;
            }

            inspectStep.Name = XmlHelper.GetAttributeValue(inspectionStepElement, "Name", "");

            XmlElement lightParamSetElement = inspectionStepElement["LightParamSet"];
            if (lightParamSetElement != null)
            {
                if (inspectStep.LightParamSet.NumLightType != 0)
                {
                    inspectStep.LightParamSet.Load(lightParamSetElement);
                }
            }

            foreach (XmlElement subElement in inspectionStepElement)
            {
                if (subElement.Name == "Target")
                {
                    var target = new Target();
                    inspectStep.AddTarget(target);

                    LoadTarget(target, subElement);
                }
            }
        }

        protected void LoadVisionProbe(VisionProbe visionProbe, XmlElement probeElement)
        {
            if (probeElement == null)
            {
                return;
            }

            var worldRegion = new RotatedRect();
            XmlHelper.GetValue(probeElement, "WorldRegion", ref worldRegion);
            visionProbe.WorldRegion = worldRegion;

            XmlElement maskFiguresElement = probeElement["MaskFigures"];
            if (maskFiguresElement != null)
            {
                visionProbe.MaskFigures.Load(maskFiguresElement);
            }

            XmlElement algorithmElement = probeElement["Algorithm"];
            if (algorithmElement == null)
            {
                return;
            }

            //string algorithmType = XmlHelper.GetAttributeValue(algorithmElement, "AlgorithmType", "");
            string algorithmType = XmlHelper.GetValue(algorithmElement, "AlgorithmType", "");
            Algorithm algorithm = AlgorithmFactory.Instance().CreateAlgorithm(algorithmType);
            if (algorithm == null)
            {
                return;
            }

            algorithm.LoadParam(algorithmElement);

            visionProbe.InspAlgorithm = algorithm;
        }
    }
}
