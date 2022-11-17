using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DynMvp.Data
{
    public struct FiducialInfo
    {
        public int StepNo { get; set; }
        public int TargetId { get; set; }
        public int ProbeId { get; set; }
    }

    public class FiducialSet : ICloneable
    {
        public int Index { get; set; }

        public bool Valid => Fiducials.Count > 0;
        public List<FiducialInfo> FiducialInfoList { get; set; } = new List<FiducialInfo>();
        public List<Probe> Fiducials { get; set; } = new List<Probe>();

        public int Count => Fiducials.Count;

        public void Clear()
        {
            Fiducials.Clear();
        }

        public object Clone()
        {
            var fiducialSet = new FiducialSet();
            fiducialSet.Copy(this);

            return fiducialSet;
        }

        public bool IsEmpty()
        {
            return Fiducials.Count == 0;
        }

        public StepModel CreateStepModel()
        {
            var stepModel = new StepModel();

            foreach (Probe probe in Fiducials)
            {
                stepModel.AddInspectStep(probe.Target.InspectStep);
            }

            return stepModel;
        }

        public void LinkFiducial(StepModel model)
        {
            Fiducials.Clear();

            foreach (FiducialInfo fiducialInfo in FiducialInfoList)
            {
                InspectStep inspectStep = model.GetInspectStep(fiducialInfo.StepNo);
                if (inspectStep == null)
                {
                    continue;
                }

                Target target = inspectStep.GetTarget(fiducialInfo.TargetId);
                if (target != null)
                {
                    Probe probe = target[fiducialInfo.ProbeId];
                    AddFiducial(probe);
                }
            }
        }

        public void Copy(FiducialSet fiducialSet)
        {
            foreach (Probe probe in fiducialSet.Fiducials)
            {
                AddFiducial(probe);
            }
        }

        public void AddFiducial(Probe probe)
        {
            if (Fiducials.IndexOf(probe) == -1)
            {
                Fiducials.Add(probe);
                if (Fiducials.Count > 2)
                {
                    Fiducials.RemoveAt(0);
                }
            }
        }

        public bool FiducialExist(Probe probe)
        {
            int index = Fiducials.IndexOf(probe);
            if (index == -1)
            {
                return false;
            }
            return true;
        }

        public void RemoveFiducial(Probe probe)
        {
            Fiducials.Remove(probe);
        }

        public void AppendFigures(FigureGroup figureGroup)
        {
            int index = 1;
            foreach (Probe probe in Fiducials)
            {
                var figure = new TextFigure(index.ToString(), new Point((int)probe.BaseRegion.Left, (int)probe.BaseRegion.Top),
                                        new Font("Arial", 30, FontStyle.Bold), Color.Red);
                index++;
                figure.Selectable = false;

                figureGroup.AddFigure(figure);
            }
        }

        public bool IsContained(Target target)
        {
            foreach (Probe probe in Fiducials)
            {
                if (target == probe.Target)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsFiducialInspected(ProbeResultList probeResultList)
        {
            if (Fiducials.Count() == 0)
            {
                return false;
            }

            for (int i = 0; i < Fiducials.Count(); i++)
            {
                if (!(probeResultList.GetProbeResult(Fiducials[i]) is VisionProbeResult fidProbeResult))
                {
                    return false;
                }
            }

            return true;
        }

        public PositionAligner Calculate(ProbeResultList probeResultList)
        {
            var positionAligner = new PositionAligner();

            if (Fiducials.Count() == 0)
            {
                return positionAligner;
            }

            PointF firstFiducialPos = DrawingHelper.CenterPoint(Fiducials[0].BaseRegion);

            try
            {
                VisionProbeResult fid1ProbeResult;
                fid1ProbeResult = probeResultList.GetProbeResult(Fiducials[0]) as VisionProbeResult;
                if (fid1ProbeResult == null || fid1ProbeResult.IsNG())
                {
                    positionAligner.Fid1Error = true;
                    return positionAligner;
                }

                var searchableResult1 = fid1ProbeResult.AlgorithmResult as SearchableResult;

                positionAligner.Offset = searchableResult1.OffsetFound;
                positionAligner.Angle = -searchableResult1.AngleFound;
                positionAligner.RotationCenter = firstFiducialPos + positionAligner.Offset;

                if (Fiducials.Count > 1)
                {
                    if (!(probeResultList.GetProbeResult(Fiducials[1]) is VisionProbeResult fid2ProbeResult) || fid2ProbeResult.IsNG())
                    {
                        positionAligner.Fid2Error = true;
                        return positionAligner;
                    }

                    var searchableResult2 = fid2ProbeResult.AlgorithmResult as SearchableResult;

                    PointF secondFiducialOrgPos = DrawingHelper.CenterPoint(Fiducials[1].BaseRegion);
                    PointF secondFiducialOffsetPos = secondFiducialOrgPos + positionAligner.Offset;
                    PointF secondFiducialPosFinded = secondFiducialOrgPos + searchableResult2.OffsetFound.ToSize();

                    positionAligner.Angle = (float)MathHelper.GetAngle(positionAligner.RotationCenter, secondFiducialOffsetPos, secondFiducialPosFinded);
                }
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }

        // 일반 버전용
        public PositionAligner Calculate2Fid(ProbeResultList probeResultList, Calibration calibration, Camera camera)
        {
            var positionAligner = new PositionAligner();

            if (Fiducials.Count() < 2)
            {
                return positionAligner;
            }

            positionAligner.FovCenter = calibration.GetFovCenterPos();

            try
            {
                if (!(probeResultList.GetProbeResult(Fiducials[0].FullId) is VisionProbeResult fid1ProbeResult) || fid1ProbeResult.IsNG())
                {
                    positionAligner.Fid1Error = true;
                    return positionAligner;
                }

                if (!(probeResultList.GetProbeResult(Fiducials[1].FullId) is VisionProbeResult fid2ProbeResult) || fid2ProbeResult.IsNG())
                {
                    positionAligner.Fid2Error = true;
                    return positionAligner;
                }

                PointF fid1Pos = DrawingHelper.CenterPoint(fid1ProbeResult.Probe.BaseRegion);
                PointF fid2Pos = DrawingHelper.CenterPoint(fid2ProbeResult.Probe.BaseRegion);

                float fidDistance = MathHelper.GetLength(fid1Pos, fid2Pos);
                positionAligner.DesiredFiducialDistance = fidDistance;

                var searchableResult1 = fid1ProbeResult.AlgorithmResult as SearchableResult;

                positionAligner.Offset = searchableResult1.RealOffsetFound;
                positionAligner.Offset = new SizeF(positionAligner.Offset.Width, -positionAligner.Offset.Height);

                var fid1OffsetPos = PointF.Add(fid1Pos, positionAligner.Offset);
                var fid2OffsetPos = PointF.Add(fid2Pos, positionAligner.Offset);

                var searchableResult2 = fid2ProbeResult.AlgorithmResult as SearchableResult;

                var fid2Offset = new SizeF(searchableResult2.RealOffsetFound.Width - positionAligner.Offset.Width,
                                              -searchableResult2.RealOffsetFound.Height - positionAligner.Offset.Height);
                var fid2FoundPos = PointF.Add(fid2OffsetPos, fid2Offset);

                positionAligner.FiducialDistance = MathHelper.GetLength(fid1OffsetPos, fid2FoundPos);

                positionAligner.RotationCenter = fid1OffsetPos;
                positionAligner.Angle = (float)MathHelper.GetAngle(fid1OffsetPos, fid2OffsetPos, fid2FoundPos);
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }

        public PositionAligner Calculate2Fov(ProbeResultList probeResultList, Calibration calibration, Camera camera)
        {
            var positionAligner = new PositionAligner();

            if (Fiducials.Count() < 2)
            {
                return positionAligner;
            }

            positionAligner.FovCenter = new PointF(camera.ImageSize.Width / 2, camera.ImageSize.Height / 2);

            try
            {
                if (!(probeResultList.GetProbeResult(Fiducials[0].FullId) is VisionProbeResult fid1ProbeResult) || fid1ProbeResult is VisionProbeResult == false || fid1ProbeResult.IsNG())
                {
                    positionAligner.Fid1Error = true;
                    return positionAligner;
                }

                if (!(probeResultList.GetProbeResult(Fiducials[1].FullId) is VisionProbeResult fid2ProbeResult) || fid2ProbeResult is VisionProbeResult == false || fid2ProbeResult.IsNG())
                {
                    positionAligner.Fid2Error = true;
                    return positionAligner;
                }

                PointF imageCenter = calibration.PixelToWorld(new PointF(camera.CameraInfo.Width / 2, camera.CameraInfo.Height / 2));
                PointF fid1CenterPos = calibration.PixelToWorld(DrawingHelper.CenterPoint(fid1ProbeResult.Probe.BaseRegion));
                PointF fid2CenterPos = calibration.PixelToWorld(DrawingHelper.CenterPoint(fid2ProbeResult.Probe.BaseRegion));

                PointF fid1CenterOffset = DrawingHelper.Subtract(fid1CenterPos, imageCenter);
                PointF fid2CenterOffset = DrawingHelper.Subtract(fid2CenterPos, imageCenter);

                fid1CenterOffset = new PointF(fid1CenterOffset.X, fid1CenterOffset.Y * (-1));
                fid2CenterOffset = new PointF(fid2CenterOffset.X, fid2CenterOffset.Y * (-1));

                var searchableResult1 = fid1ProbeResult.AlgorithmResult as SearchableResult;
                SizeF realOffset1 = searchableResult1.RealOffsetFound;
                realOffset1 = new SizeF(realOffset1.Width, realOffset1.Height * (-1));

                var searchableResult2 = fid2ProbeResult.AlgorithmResult as SearchableResult;
                SizeF realOffset2 = searchableResult2.RealOffsetFound;
                realOffset2 = new SizeF(realOffset2.Width, realOffset2.Height * (-1));

                positionAligner.Offset = realOffset1;

                var fid1RobotPos = fid1ProbeResult.Probe.Target.InspectStep.Position.ToPointF();
                PointF fid1TeachPos = DrawingHelper.Add(fid1RobotPos, fid1CenterOffset);

                var fid2RobotPos = fid2ProbeResult.Probe.Target.InspectStep.Position.ToPointF();
                PointF fid2TeachPos = DrawingHelper.Add(fid2RobotPos, fid2CenterOffset);

                positionAligner.DesiredFiducialDistance = MathHelper.GetLength(fid2TeachPos, fid1TeachPos);

                var fid1Pos = PointF.Add(fid1TeachPos, realOffset1);
                var fid2Pos = PointF.Add(fid2TeachPos, realOffset2);

                positionAligner.FiducialDistance = MathHelper.GetLength(fid1Pos, fid2Pos);
                positionAligner.FiducialDistanceDiff = positionAligner.FiducialDistance - positionAligner.DesiredFiducialDistance;

                positionAligner.RotationCenter = fid1Pos;

                SizeF fid2ActualOffset = DrawingHelper.Subtract(realOffset2, realOffset1);
                var fid2OffsetPos = PointF.Add(fid2TeachPos, fid2ActualOffset);

                positionAligner.Angle = (float)MathHelper.GetAngle(fid1TeachPos, fid2TeachPos, fid2OffsetPos);
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }
    }
}
