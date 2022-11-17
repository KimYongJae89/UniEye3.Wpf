using DynMvp.Base;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;

namespace DynMvp.Data
{
    public enum MarkerType
    {
        MergeSource, MergeTarget
    }

    public class MarkerProbe : Probe
    {
        public MarkerType MarkerType { get; set; }
        public string MergeSourceId { get; set; } = "";
        public Point3d MergeOffset { get; set; } = new Point3d();

        public override int[] LightTypeIndexArr => null;

        public override object Clone()
        {
            var markerProbe = new MarkerProbe();
            markerProbe.Copy(this);

            return markerProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            var markerProbe = (MarkerProbe)probe;
            MergeSourceId = markerProbe.MergeSourceId;
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override bool IsControllable()
        {
            return false;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Result"));

            return resultValues;
        }

        public override ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            var markerProbeResult = new ProbeResult(this);

            return markerProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new ProbeResult(this);
        }
    }
}
