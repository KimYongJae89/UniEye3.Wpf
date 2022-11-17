using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.UI
{
    public class ObjectCreator
    {
        public IModellerPage ModellerPage { get; set; }

        public ObjectCreator(IModellerPage modellerPage)
        {
            ModellerPage = modellerPage;
        }

        public VisionProbe CreateVisionProbe(RotatedRect rect = new RotatedRect())
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateVisionProbe");

            if (rect == RotatedRect.Empty)
            {
                rect = ModellerPage.GetDefaultProbeRegion();
                if (rect.IsEmpty)
                {
                    return null;
                }
            }

            var visionProbe = (VisionProbe)ProbeFactory.Create(ProbeType.Vision);
            visionProbe.BaseRegion = rect;

            return visionProbe;
        }

        public VisionProbe CreateVisionProbe(string algorithmType, RotatedRect rect = new RotatedRect())
        {
            VisionProbe probe = CreateVisionProbe(rect);
            if (probe == null)
            {
                return null;
            }

            probe.InspAlgorithm = AlgorithmFactory.Instance().CreateAlgorithm(algorithmType);

            Image2D clipImage = ModellerPage.GetClipImage(probe.BaseRegion);

            if (clipImage.NumBand == 1)
            {
                probe.InspAlgorithm.Param.SourceImageType = ImageType.Grey;
            }
            else
            {
                probe.InspAlgorithm.Param.SourceImageType = ImageType.Color;
            }

            probe.InspAlgorithm.Enabled = true;

            return probe;
        }

        public VisionProbe CreateVisionProbe(Algorithm algorithm, RotatedRect rect = new RotatedRect())
        {
            VisionProbe probe = CreateVisionProbe(rect);
            if (probe == null)
            {
                return null;
            }

            probe.InspAlgorithm = algorithm;

            return probe;
        }

        public Probe CreatePatternMatching(RotatedRect rect = new RotatedRect())
        {
            VisionProbe probe = CreateVisionProbe(rect);
            if (probe == null)
            {
                return null;
            }

            var patternMatching = new PatternMatching();

            Pattern pattern = AlgorithmFactory.Instance().CreatePattern();

            Image2D clipImage = ModellerPage.GetClipImage(probe.BaseRegion);

            patternMatching.AddPattern(clipImage);

            probe.InspAlgorithm = patternMatching;

            return probe;
        }

        public ComputeProbe CreateComputeProbe(RotatedRect rect = new RotatedRect())
        {
            var computeProbe = (ComputeProbe)ProbeFactory.Create(ProbeType.Compute);

            return computeProbe;
        }

        public DaqProbe CreateDaqProbe(RotatedRect rect = new RotatedRect())
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - CreateDaqProbe");

            if (rect == null)
            {
                rect = ModellerPage.GetDefaultProbeRegion();
                if (rect.IsEmpty)
                {
                    return null;
                }
            }

            var daqProbe = (DaqProbe)ProbeFactory.Create(ProbeType.Daq);
            daqProbe.BaseRegion = rect;

            return daqProbe;
        }
    }
}
