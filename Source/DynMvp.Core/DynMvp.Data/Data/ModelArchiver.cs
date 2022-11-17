using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.Device;
using DynMvp.Device.Dio;
using DynMvp.Device.Comm;
using DynMvp.Device.Daq;

namespace DynMvp.Data
{
    public class ModelArchiver
    {
        private DigitalIo digitalIo = null;
        private XmlDocument xmlDocument = null;

        public void SetEnvironment(DigitalIo digitalIo)
        {
            this.digitalIo = digitalIo;
        }

        public void Load(Model model, string modelPath, IReportProgress reportProgress)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(modelPath);

            XmlElement modelElement = xmlDocument.DocumentElement;

            model.FileVersion = Convert.ToSingle(XmlHelper.GetValue(modelElement, "Version", "1.0"));

            int count = 0;

            XmlElement imageBufferFileListElement = modelElement["ImageBufferFileList"];
            if (imageBufferFileListElement != null)
            {
                model.ImageBufferPathList.Clear();
                foreach (XmlElement imageBufferFileElement in imageBufferFileListElement)
                {
                    model.ImageBufferPathList.Add(imageBufferFileElement.InnerText);
                }
            }

            foreach (XmlElement targetGroupElement in modelElement)
            {
                if (targetGroupElement.Name == "TargetGroup")
                {
                    TargetGroup targetGroup = new TargetGroup();
                    model.AddTargetGroup(targetGroup);

                    LoadTargetGroup(targetGroup, targetGroupElement);

                    if (reportProgress != null)
                        reportProgress.ReportProgress(count * 10, "");

                    count++;
                }
            }
        }

        private void LoadFiducialSet(TargetGroup targetGroup, XmlElement fiducialSetElement)
        {
            if (fiducialSetElement == null)
                return;

            FiducialSet fiducialSet = targetGroup.FiducialSet;

            foreach (XmlElement fiducialElement in fiducialSetElement)
            {
                if (fiducialElement.Name == "Fiducial")
                {
                    int targetId = Convert.ToInt32(XmlHelper.GetValue(fiducialElement, "TargetId", ""));
                    int probeId = Convert.ToInt32(XmlHelper.GetValue(fiducialElement, "ProbeId", ""));
                    Target target = targetGroup.GetTarget(targetId);
                    if (target != null)
                    {
                        Probe probe = target.GetProbe(probeId);
                        fiducialSet.AddFiducial(probe);
                    }
                }
            }
        }

        private void LoadTargetGroup(TargetGroup targetGroup, XmlElement targetGroupElement)
        {
            targetGroup.CamId = Convert.ToUInt32(XmlHelper.GetValue(targetGroupElement, "CamId", "0"));

            foreach (XmlElement subElement in targetGroupElement)
            {
                if (subElement.Name == "Target")
                {
                    Target target = new Target();
                    targetGroup.AddTarget(target);

                    LoadTarget(target, subElement);
                }
                else if (subElement.Name == "LightParam")
                {
                    int lightParamIndex = Convert.ToInt32(XmlHelper.GetValue(subElement, "Index", "0"));
                    if (targetGroup.LightParamList.Count > lightParamIndex)
                    {
                        LightParam lightParam = targetGroup.LightParamList[lightParamIndex];
                        lightParam.Load(subElement);
                    }
                }
            }

            LoadFiducialSet(targetGroup, targetGroupElement["FiducialSet"]);
        }

        private void LoadTarget(Target target, XmlElement targetElement)
        {
            if (targetElement == null)
                return;

            target.Id = Convert.ToInt32(XmlHelper.GetValue(targetElement, "Id", "0"));
            if (target.Id == 0)
            {
                throw new InvalidDataException();
            }

            target.InspectionStep = Convert.ToInt32(XmlHelper.GetValue(targetElement, "InspectionStep", "0"));
            target.ImageEncodedString = XmlHelper.GetValue(targetElement, "Image", "");
            target.Name = XmlHelper.GetValue(targetElement, "Name", "");
            target.TypeName = XmlHelper.GetValue(targetElement, "Type", "");
            target.UseInspection = Convert.ToBoolean(XmlHelper.GetValue(targetElement, "UseInspection", "True"));

            Rectangle targetRegion = new Rectangle();
            XmlHelper.GetValue(targetElement, "Region", ref targetRegion);
            target.Region = targetRegion;

            foreach (XmlElement probeElement in targetElement)
            {
                if (probeElement.Name == "Probe")
                {
                    string probeTypeStr = XmlHelper.GetValue(probeElement, "ProbeType", "");
                    if (probeTypeStr == "")
                        throw new InvalidDataException();

                    ProbeType probeType = (ProbeType)Enum.Parse(typeof(ProbeType), probeTypeStr);

                    Probe probe = ProbeFactory.Create(probeType);
                    target.AddProbe(probe);

                    LoadProbe(probe, probeElement);
                }
            }
        }

        private void LoadProbe(Probe probe, XmlElement probeElement)
        {
            if (probeElement == null)
                return;

            probe.Id = Convert.ToInt32(XmlHelper.GetValue(probeElement, "Id", "0"));
            if (probe.Id == 0)
                throw new InvalidDataException();

            probe.ActAsFiducialProbe = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "ActAsFiducialProbe", "False"));
            probe.FiducialProbeId = Convert.ToInt32(XmlHelper.GetValue(probeElement, "FiducialProbeId", "0"));
            probe.InverseResult = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "InverseResult", "False"));
            probe.ModelVerification = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "ModelVerification", "False"));
            probe.StepBlocker = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "StepBlocker", "False"));

            Rectangle probeRegion = new Rectangle();
            XmlHelper.GetValue(probeElement, "Region", ref probeRegion);
            probe.Region = probeRegion;

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
                default:
                    throw new InvalidTypeException();
            }

        }

        private void LoadVisionProbe(VisionProbe visionProbe, XmlElement probeElement)
        {
            if (probeElement == null)
                return;

            visionProbe.SourceImageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(probeElement, "SourceImageType", "Grey"));
            visionProbe.ImageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), XmlHelper.GetValue(probeElement, "ImageBand", "Luminance"));
            visionProbe.UseSobelFilter = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "UseSobelFilter", "False"));
            visionProbe.UseAverageFilter = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "UseAverageFilter", "False"));
            visionProbe.UseHistogramEqualization = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "UseHistogramEqualization", "False"));
            visionProbe.UseBinarization = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "UseBinarization", "False"));
            visionProbe.BinarizationType = (BinarizationType)Enum.Parse(typeof(BinarizationType), XmlHelper.GetValue(probeElement, "BinarizationType", "SingleThreshold"));
            visionProbe.ThresholdLower = Convert.ToInt32(XmlHelper.GetValue(probeElement, "ThresholdLower", "128"));
            visionProbe.ThresholdUpper = Convert.ToInt32(XmlHelper.GetValue(probeElement, "ThresholdUpper", "128"));
            visionProbe.UseEdgeExtraction = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "UseEdgeExtraction", "False"));

            XmlElement algorithmElement = probeElement["Algorithm"];

            Algorithm algorithm;
            AlgorithmArchiver.LoadAlgorithm(algorithmElement, out algorithm);
            visionProbe.InspAlgorithm = algorithm;
        }

        private void LoadIoProbe(IoProbe ioProbe, XmlElement probeElement)
        {
            ioProbe.PortNo = Convert.ToInt32(XmlHelper.GetValue(probeElement, "PortNo", "0"));
            ioProbe.DigitalIo = digitalIo;
        }

        private void LoadSerialProbe(SerialProbe serialprobe, XmlElement probeElement)
        {
            serialprobe.PortName = XmlHelper.GetValue(probeElement, "PortName", "");
            serialprobe.UpperValue = (float)Convert.ToDouble(XmlHelper.GetValue(probeElement, "UpperValue", "0.0"));
            serialprobe.LowerValue = (float)Convert.ToDouble(XmlHelper.GetValue(probeElement, "LowerValue", "0.0"));

            serialprobe.InspectionSerialPort = (InspectionSerialPort)SerialPortManager.Instance().GetSerialPort(serialprobe.PortName);
        }

        private void LoadDaqProbe(DaqProbe daqprobe, XmlElement probeElement)
        {
            string channelName = XmlHelper.GetValue(probeElement, "ChannelName", "");
            daqprobe.DaqChannel = DaqChannelManager.Instance().GetDaqChannel(channelName);

            daqprobe.UpperValue = (float)Convert.ToDouble(XmlHelper.GetValue(probeElement, "UpperValue", "0.0"));
            daqprobe.LowerValue = (float)Convert.ToDouble(XmlHelper.GetValue(probeElement, "LowerValue", "0.0"));
            daqprobe.NumSample = Convert.ToInt32(XmlHelper.GetValue(probeElement, "NumSample", "100"));
        }

        public void Save(Model model, string filePath)
        {
            xmlDocument = new XmlDocument();

            XmlElement modelElement = xmlDocument.CreateElement("", "Model", "");
            xmlDocument.AppendChild(modelElement);

            XmlHelper.SetValue(modelElement, "Version", "1.0");

            XmlElement imageBufferFileListElement = xmlDocument.CreateElement("", "ImageBufferFileList", "");
            modelElement.AppendChild(imageBufferFileListElement);

            foreach (string fileName in model.ImageBufferPathList)
            {
                XmlHelper.SetValue(imageBufferFileListElement, "ImageBufferFile", fileName);
            }

            foreach (TargetGroup targetGroup in model.TargetGroupList)
            {
                XmlElement targetGroupElement = xmlDocument.CreateElement("", "TargetGroup", "");
                modelElement.AppendChild(targetGroupElement);

                SaveTargetGroup(targetGroupElement, targetGroup);
            }

            xmlDocument.Save(filePath);
        }

        private void SaveFiducialSet(XmlElement fiducialSetElement, FiducialSet fiducialSet)
        {
            foreach (Probe probe in fiducialSet.Fiducials)
            {
                XmlElement fiducialElement = xmlDocument.CreateElement("", "Fiducial", "");
                fiducialSetElement.AppendChild(fiducialElement);

                XmlHelper.SetValue(fiducialElement, "TargetId", probe.Target.Id.ToString());
                XmlHelper.SetValue(fiducialElement, "ProbeId", probe.Id.ToString());
            }
        }

        private void SaveTargetGroup(XmlElement targetGroupElement, TargetGroup targetGroup)
        {
            XmlHelper.SetValue(targetGroupElement, "CamId", targetGroup.CamId.ToString());

            XmlElement fiducialSetElement = xmlDocument.CreateElement("", "FiducialSet", "");
            targetGroupElement.AppendChild(fiducialSetElement);

            SaveFiducialSet(fiducialSetElement, targetGroup.FiducialSet);

            foreach (Target target in targetGroup.TargetList)
            {
                XmlElement targetElement = xmlDocument.CreateElement("", "Target", "");
                targetGroupElement.AppendChild(targetElement);

                SaveTarget(targetElement, target);
            }

            int index = 0;
            foreach (LightParam lightParam in targetGroup.LightParamList)
            {
                XmlElement lightParamElement = xmlDocument.CreateElement("", "LightParam", "");
                targetGroupElement.AppendChild(lightParamElement);

                XmlHelper.SetValue(lightParamElement, "Index", index.ToString());
                lightParam.Save(lightParamElement);

                index++;
            }
        }

        private void SaveTarget(XmlElement targetElement, Target target)
        {
            XmlHelper.SetValue(targetElement, "Id", target.Id.ToString());
            XmlHelper.SetValue(targetElement, "Name", target.Name);
            XmlHelper.SetValue(targetElement, "Type", target.TypeName);
            XmlHelper.SetValue(targetElement, "Image", target.ImageEncodedString);
            XmlHelper.SetValue(targetElement, "InspectionStep", target.InspectionStep.ToString());
            XmlHelper.SetValue(targetElement, "UseInspection", target.UseInspection.ToString());

            XmlHelper.SetValue(targetElement, "Region", target.Region);

            foreach (Probe probe in target.ProbeList)
            {
                XmlElement probeElement = xmlDocument.CreateElement("", "Probe", "");
                targetElement.AppendChild(probeElement);

                SaveProbe(probeElement, probe);
            }
        }

        private void SaveProbe(XmlElement probeElement, Probe probe)
        {
            XmlHelper.SetValue(probeElement, "Id", probe.Id.ToString());
            XmlHelper.SetValue(probeElement, "ProbeType", probe.ProbeType.ToString());
            XmlHelper.SetValue(probeElement, "ActAsFiducialProbe", probe.ActAsFiducialProbe.ToString());
            XmlHelper.SetValue(probeElement, "FiducialProbeId", probe.FiducialProbeId.ToString());
            XmlHelper.SetValue(probeElement, "InverseResult", probe.InverseResult.ToString());
            XmlHelper.SetValue(probeElement, "ModelVerification", probe.ModelVerification.ToString());
            XmlHelper.SetValue(probeElement, "Region", probe.Region);
            XmlHelper.SetValue(probeElement, "StepBlocker", probe.StepBlocker.ToString());

            switch (probe.ProbeType)
            {
                case ProbeType.Vision:
                    SaveVisionProbe(probeElement, (VisionProbe)probe);
                    break;
                case ProbeType.Io:
                    SaveIoProbe(probeElement, (IoProbe)probe);
                    break;
                case ProbeType.Serial:
                    SaveSerialProbe(probeElement, (SerialProbe)probe);
                    break;
                case ProbeType.Daq:
                    SaveDaqProbe(probeElement, (DaqProbe)probe);
                    break;
                default:
                    throw new InvalidTypeException();
            }
        }

        private void SaveVisionProbe(XmlElement probeElement, VisionProbe visionProbe)
        {
            XmlHelper.SetValue(probeElement, "AlgorithmType", visionProbe.InspAlgorithm.GetAlgorithmType().ToString());
            XmlHelper.SetValue(probeElement, "SourceImageType", visionProbe.SourceImageType.ToString());
            XmlHelper.SetValue(probeElement, "ImageBand", visionProbe.ImageBand.ToString());
            XmlHelper.SetValue(probeElement, "UseSobelFilter", visionProbe.UseSobelFilter.ToString());
            XmlHelper.SetValue(probeElement, "UseAverageFilter", visionProbe.UseAverageFilter.ToString());
            XmlHelper.SetValue(probeElement, "UseHistogramEqualization", visionProbe.UseHistogramEqualization.ToString());
            XmlHelper.SetValue(probeElement, "UseBinarization", visionProbe.UseBinarization.ToString());
            XmlHelper.SetValue(probeElement, "BinarizationType", visionProbe.BinarizationType.ToString());
            XmlHelper.SetValue(probeElement, "ThresholdLower", visionProbe.ThresholdLower.ToString());
            XmlHelper.SetValue(probeElement, "ThresholdUpper", visionProbe.ThresholdUpper.ToString());
            XmlHelper.SetValue(probeElement, "UseEdgeExtraction", visionProbe.UseEdgeExtraction.ToString());

            XmlElement algorithmElement = xmlDocument.CreateElement("", "Algorithm", "");
            probeElement.AppendChild(algorithmElement);

            AlgorithmArchiver.SaveAlgorithm(algorithmElement, visionProbe.InspAlgorithm);
        }

        private void SaveIoProbe(XmlElement probeElement, IoProbe ioProbe)
        {
            XmlHelper.SetValue(probeElement, "PortNo", ioProbe.PortNo.ToString());
        }

        private void SaveSerialProbe(XmlElement probeElement, SerialProbe serialprobe)
        {
            XmlHelper.SetValue(probeElement, "PortName", serialprobe.PortName);
            XmlHelper.SetValue(probeElement, "UpperValue", serialprobe.UpperValue.ToString());
            XmlHelper.SetValue(probeElement, "LowerValue", serialprobe.LowerValue.ToString());
        }

        private void SaveDaqProbe(XmlElement probeElement, DaqProbe daqprobe)
        {
            string channelName = "";
            if (daqprobe.DaqChannel != null)
                channelName = daqprobe.DaqChannel.Name;

            XmlHelper.SetValue(probeElement, "ChannelName", channelName);
            XmlHelper.SetValue(probeElement, "UpperValue", daqprobe.UpperValue.ToString());
            XmlHelper.SetValue(probeElement, "LowerValue", daqprobe.LowerValue.ToString());
            XmlHelper.SetValue(probeElement, "NumSample", daqprobe.NumSample.ToString());
        }
    }
}
