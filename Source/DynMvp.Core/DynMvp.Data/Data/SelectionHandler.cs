using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public class SelectionHandler
    {
        public List<Target> TargetList { get; set; } = new List<Target>();
        public List<Probe> ProbeList { get; set; } = new List<Probe>();

        public void AddTarget(Target target)
        {
            target.Id = CreateTargetId();
            TargetList.Add(target);
            ProbeList.Clear();
        }

        private int CreateTargetId()
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (GetTarget(i) == null)
                {
                    return i;
                }
            }
            return 99999;
        }

        public Target GetTarget(int id)
        {
            foreach (Target target in TargetList)
            {
                if (target.Id == id)
                {
                    return target;
                }
            }
            return null;
        }

        public void ClearTargetList()
        {
            TargetList.Clear();
            ProbeList.Clear();
        }

        public bool IsTargetSelected()
        {
            return TargetList.Count > 0;
        }

        public bool IsTargetSingleSelected()
        {
            return TargetList.Count == 1;
        }

        public Target GetSingleTarget()
        {
            if (TargetList.Count == 1)
            {
                return TargetList[0];
            }

            return null;
        }

        public void AddProbe(Probe probe)
        {
            ProbeList.Add(probe);
        }

        public void ClearProbeList()
        {
            ProbeList.Clear();
        }

        public bool IsProbeSelected()
        {
            return ProbeList.Count > 0;
        }

        public bool IsProbeSingleSelected()
        {
            return ProbeList.Count == 1;
        }

        public Probe GetSingleProbe()
        {
            if (ProbeList.Count == 1)
            {
                return ProbeList[0];
            }

            return null;
        }
    }
}
