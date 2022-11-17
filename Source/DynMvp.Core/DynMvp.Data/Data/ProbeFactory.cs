using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public class ProbeFactory
    {
        public static Probe Create(ProbeType probeType)
        {
            Probe probe = null;
            switch (probeType)
            {
                case ProbeType.Vision:
                    probe = new VisionProbe();
                    break;
                case ProbeType.Io:
                    probe = new IoProbe();
                    break;
                case ProbeType.Serial:
                    probe = new SerialProbe();
                    break;
                case ProbeType.Daq:
                    probe = new DaqProbe();
                    break;
                case ProbeType.Compute:
                    probe = new ComputeProbe();
                    break;
                case ProbeType.Marker:
                    probe = new MarkerProbe();
                    break;
                default:
                    throw new InvalidTypeException();
            }

            if (probe != null)
            {
                probe.ProbeType = probeType;
                return probe;
            }
            else
            {
                throw new OutOfMemoryException();
            }
        }
    }
}
