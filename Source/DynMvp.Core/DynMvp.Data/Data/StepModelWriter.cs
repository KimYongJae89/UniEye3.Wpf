using DynMvp.Base;
using DynMvp.Data.Library;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Data
{
    public class StepModelWriterBuilder
    {
        public static StepModelWriter Create(float fileVersion = 0)
        {
            if (fileVersion == 0)
            {
                fileVersion = (float)1.0;  // Last Version
            }

            var modelWriter = new StepModelWriter();

            return modelWriter;
        }
    }

    public class StepModelWriter
    {
        protected XmlDocument xmlDocument;

        public void Write(StepModel model, string filePath, IReportProgress reportProgress)
        {
            xmlDocument = new XmlDocument();

            XmlElement modelElement = xmlDocument.CreateElement("", "Model", "");
            xmlDocument.AppendChild(modelElement);

            float versionNo = 1.0f;
            XmlHelper.SetAttributeValue(modelElement, "Version", versionNo.ToString("F1"));

            //모델의 LightParamSet을 저장 한다.
            XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
            modelElement.AppendChild(lightParamSetElement);
            model.LightParamSet.Save(lightParamSetElement);

            XmlElement exposureTimeListElement = xmlDocument.CreateElement("", "ExposureTimeList", "");
            modelElement.AppendChild(exposureTimeListElement);
            WriteExposureTime(model, exposureTimeListElement);

            WriteTargets(model, modelElement, reportProgress);

            XmlElement fiducialSetListElement = xmlDocument.CreateElement("", "FiducialSetList", "");
            modelElement.AppendChild(fiducialSetListElement);

            foreach (FiducialSet fiducialSet in model.FiducialSetList)
            {
                XmlElement fiducialSetElement = xmlDocument.CreateElement("", "FiducialSet", "");
                fiducialSetListElement.AppendChild(fiducialSetElement);

                WriteFiducialSet(fiducialSetElement, fiducialSet);
            }

            xmlDocument.Save(filePath);
        }

        public void Write(Template template, string filePath)
        {
            xmlDocument = new XmlDocument();

            XmlElement templateElement = xmlDocument.CreateElement("", "Template", "");
            xmlDocument.AppendChild(templateElement);

            XmlHelper.SetAttributeValue(templateElement, "Name", template.Name);
            XmlHelper.SetAttributeValue(templateElement, "Id", template.Id.ToString());

            float versionNo = 1.0f;
            XmlHelper.SetAttributeValue(templateElement, "Version", versionNo.ToString("F1"));
            XmlHelper.SetAttributeValue(templateElement, "Tag", template.GetTag());

            XmlElement targetElement = xmlDocument.CreateElement("", "Target", "");
            templateElement.AppendChild(targetElement);

            WriteTarget(targetElement, template.Target);

            xmlDocument.Save(filePath);
        }

        protected void WriteExposureTime(StepModel model, XmlElement exposureTimeListElement)
        {
            int cameraIndex = 0;
            foreach (int exposureTime in model.ExposureTimeUsList)
            {
                XmlElement exposureTimeElement = exposureTimeListElement.OwnerDocument.CreateElement("", "ExposureTime", "");
                exposureTimeListElement.AppendChild(exposureTimeElement);

                XmlHelper.SetAttributeValue(exposureTimeElement, "ExpsureTime", exposureTime.ToString());
                XmlHelper.SetAttributeValue(exposureTimeElement, "CameraIndex", cameraIndex.ToString());

                cameraIndex++;
            }
        }

        protected void WriteFiducialSet(XmlElement fiducialSetElement, FiducialSet fiducialSet)
        {
            XmlHelper.SetAttributeValue(fiducialSetElement, "Index", fiducialSet.Index.ToString());

            foreach (Probe probe in fiducialSet.Fiducials)
            {
                XmlElement fiducialElement = xmlDocument.CreateElement("", "Fiducial", "");
                fiducialSetElement.AppendChild(fiducialElement);

                XmlHelper.SetAttributeValue(fiducialElement, "TargetId", probe.Target.Id.ToString());
                XmlHelper.SetAttributeValue(fiducialElement, "ProbeId", probe.Id.ToString());
            }
        }

        protected void WriteTarget(XmlElement targetElement, Target target)
        {
            XmlHelper.SetAttributeValue(targetElement, "Id", target.Id.ToString());
            XmlHelper.SetAttributeValue(targetElement, "Name", target.Name);

            if (target.GetType() == target.GetType().BaseType)
            {
                XmlHelper.SetAttributeValue(targetElement, "ModuleNo", target.ModuleNo.ToString());
                XmlHelper.SetAttributeValue(targetElement, "Image", target.ImageEncodedString);
                XmlHelper.SetAttributeValue(targetElement, "LightTypeIndex", target.LightTypeIndexArr.ToString());
            }

            XmlHelper.SetAttributeValue(targetElement, "Type", target.TypeName);
            XmlHelper.SetAttributeValue(targetElement, "UseInspection", target.UseInspection.ToString());
            XmlHelper.SetAttributeValue(targetElement, "InspectionLogicType", target.InspectionLogicType.ToString());
            XmlHelper.SetAttributeValue(targetElement, "InspectOrder", target.InspectOrder.ToString());
            XmlHelper.SetAttributeValue(targetElement, "CameraIndex", target.CameraIndex.ToString());

            XmlHelper.SetValue(targetElement, "Region", target.BaseRegion);

            foreach (Probe probe in target.ProbeList)
            {
                XmlElement probeElement = xmlDocument.CreateElement("", "Probe", "");
                targetElement.AppendChild(probeElement);

                WriteProbe(probeElement, probe);
            }
        }

        protected void WriteProbe(XmlElement probeElement, Probe probe)
        {
            XmlHelper.SetAttributeValue(probeElement, "Id", probe.Id.ToString());
            XmlHelper.SetAttributeValue(probeElement, "Name", probe.Name.ToString());
            XmlHelper.SetAttributeValue(probeElement, "ProbeType", probe.ProbeType.ToString());
            XmlHelper.SetAttributeValue(probeElement, "FiducialProbeId", probe.FiducialProbeId.ToString());
            XmlHelper.SetAttributeValue(probeElement, "InverseResult", probe.InverseResult.ToString());
            XmlHelper.SetAttributeValue(probeElement, "ModelVerification", probe.ModelVerification.ToString());
            XmlHelper.SetAttributeValue(probeElement, "StepBlocker", probe.StepBlocker.ToString());
            //XmlHelper.SetAttributeValue(probeElement, "LightTypeIndex", probe.LightTypeIndex.ToString());

            XmlHelper.SetValue(probeElement, "Region", probe.BaseRegion);

            if (probe.CustomInfo != null)
            {
                probe.CustomInfo.Save(probeElement);
            }

            switch (probe.ProbeType)
            {
                case ProbeType.Vision:
                    WriteVisionProbe(probeElement, (VisionProbe)probe);
                    break;
                case ProbeType.Io:
                    WriteIoProbe(probeElement, (IoProbe)probe);
                    break;
                case ProbeType.Serial:
                    WriteSerialProbe(probeElement, (SerialProbe)probe);
                    break;
                case ProbeType.Daq:
                    WriteDaqProbe(probeElement, (DaqProbe)probe);
                    break;
                case ProbeType.Marker:
                    WriteMarkerProbe(probeElement, (MarkerProbe)probe);
                    break;
                default:
                    throw new InvalidTypeException();
            }
        }

        protected void WriteMarkerProbe(XmlElement probeElement, MarkerProbe markerProbe)
        {
            XmlHelper.SetAttributeValue(probeElement, "MarkerType", markerProbe.MarkerType.ToString());
            XmlHelper.SetAttributeValue(probeElement, "MergeSourceId", markerProbe.MergeSourceId);

            XmlHelper.SetValue(probeElement, "MergeOffset", markerProbe.MergeOffset);
        }

        protected void WriteIoProbe(XmlElement probeElement, IoProbe ioProbe)
        {
            XmlHelper.SetAttributeValue(probeElement, "PortNo", ioProbe.PortNo.ToString());
            XmlHelper.SetAttributeValue(probeElement, "DigitalIoName", ioProbe.DigitalIoName);
        }

        protected void WriteSerialProbe(XmlElement probeElement, SerialProbe serialprobe)
        {
            XmlHelper.SetAttributeValue(probeElement, "PortName", serialprobe.PortName);
            XmlHelper.SetAttributeValue(probeElement, "UpperValue", serialprobe.UpperValue.ToString());
            XmlHelper.SetAttributeValue(probeElement, "LowerValue", serialprobe.LowerValue.ToString());
            XmlHelper.SetAttributeValue(probeElement, "NumSerialReading", serialprobe.NumSerialReading.ToString());
            if (serialprobe is TensionSerialProbe)
            {
                XmlHelper.SetAttributeValue(probeElement, "TensionFilePath", ((TensionSerialProbe)serialprobe).TensionFilePath.ToString());
                XmlHelper.SetAttributeValue(probeElement, "UnitType", ((TensionSerialProbe)serialprobe).UnitType.ToString());
            }

            XmlHelper.SetValue(probeElement, "WorldRegion", serialprobe.WorldRegion);
        }

        protected void WriteDaqProbe(XmlElement probeElement, DaqProbe daqProbe)
        {
            string channelName = "";
            if (daqProbe.DaqChannel != null)
            {
                channelName = daqProbe.DaqChannel.Name;
            }

            XmlHelper.SetAttributeValue(probeElement, "ChannelName", channelName);
            XmlHelper.SetAttributeValue(probeElement, "UpperValue", daqProbe.UpperValue.ToString());
            XmlHelper.SetAttributeValue(probeElement, "LowerValue", daqProbe.LowerValue.ToString());
            XmlHelper.SetAttributeValue(probeElement, "NumSample", daqProbe.NumSample.ToString());
            XmlHelper.SetAttributeValue(probeElement, "UseLocalScaleFactor", daqProbe.UseLocalScaleFactor.ToString());
            XmlHelper.SetAttributeValue(probeElement, "LocalScaleFactor", daqProbe.LocalScaleFactor.ToString());
            XmlHelper.SetAttributeValue(probeElement, "ValueOffset", daqProbe.ValueOffset.ToString());
            XmlHelper.SetAttributeValue(probeElement, "MeasureType", daqProbe.MeasureType.ToString());
            XmlHelper.SetAttributeValue(probeElement, "FilterType", daqProbe.FilterType.ToString());
            XmlHelper.SetAttributeValue(probeElement, "Target1Name", daqProbe.Target1Name);
            XmlHelper.SetAttributeValue(probeElement, "Target2Name", daqProbe.Target2Name);
        }

        protected void WriteComputeProbe(XmlElement probeElement, SerialProbe serialprobe)
        {
            XmlHelper.SetAttributeValue(probeElement, "PortName", serialprobe.PortName);
            XmlHelper.SetAttributeValue(probeElement, "UpperValue", serialprobe.UpperValue.ToString());
            XmlHelper.SetAttributeValue(probeElement, "LowerValue", serialprobe.LowerValue.ToString());
        }

        protected void WriteTargets(StepModel model, XmlElement modelElement, IReportProgress reportProgress)
        {
            int count = 0;

            foreach (InspectStep inspectStep in model.InspectStepList)
            {
                XmlElement inspectionStepElement = xmlDocument.CreateElement("", "InspectionStep", "");
                modelElement.AppendChild(inspectionStepElement);

                WriteInspectionStep(inspectionStepElement, inspectStep);

                if (reportProgress != null)
                {
                    reportProgress.ReportProgress(count * 10, "");
                }

                count++;
            }
        }

        private void WriteInspectionStep(XmlElement inspectionStepElement, InspectStep inspectStep)
        {
            if (inspectStep.Position != null)
            {
                inspectStep.Position.SetValue(inspectionStepElement, "AxisPosition");
            }

            XmlHelper.SetAttributeValue(inspectionStepElement, "Name", inspectStep.Name);
            XmlHelper.SetValue(inspectionStepElement, "FovRect", inspectStep.FovRect);

            XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
            inspectionStepElement.AppendChild(lightParamSetElement);
            inspectStep.LightParamSet.Save(lightParamSetElement);

            foreach (Target target in inspectStep.TargetList)
            {
                XmlElement targetElement = xmlDocument.CreateElement("", "Target", "");
                inspectionStepElement.AppendChild(targetElement);

                WriteTarget(targetElement, target);
            }
        }

        protected void WriteVisionProbe(XmlElement probeElement, VisionProbe visionProbe)
        {
            XmlHelper.SetAttributeValue(probeElement, "AlgorithmType", visionProbe.InspAlgorithm.GetAlgorithmType().ToString());
            XmlHelper.SetValue(probeElement, "WorldRegion", visionProbe.WorldRegion);

            if (visionProbe.MaskFigures.FigureExist)
            {
                XmlElement maskFiguresElement = probeElement.OwnerDocument.CreateElement("", "MaskFigures", "");
                probeElement.AppendChild(maskFiguresElement);

                visionProbe.MaskFigures.Save(maskFiguresElement);
            }

            XmlElement algorithmElement = xmlDocument.CreateElement("", "Algorithm", "");
            probeElement.AppendChild(algorithmElement);

            visionProbe.InspAlgorithm.SaveParam(algorithmElement);
        }
    }
}
