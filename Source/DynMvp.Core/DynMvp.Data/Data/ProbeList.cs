using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Data
{
    public class ProbeList : IEnumerable<Probe>
    {
        private List<Probe> probeList = new List<Probe>();

        public int Count => probeList.Count;

        public Probe this[int index] => probeList[index];

        public IEnumerator<Probe> GetEnumerator()
        {
            return probeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return probeList.GetEnumerator();
        }

        public void Clear()
        {
            probeList.Clear();
        }

        public void AddProbe(Probe probe)
        {
            probeList.Add(probe);
        }

        public void AddProbe(ProbeList newProbeList)
        {
            probeList.AddRange(newProbeList);
        }

        public ProbeList GetFilteredList()
        {
            var filterredProbeList = new ProbeList();

            foreach (Probe probe in probeList)
            {
                if (probeList.Count == 0)
                {
                    filterredProbeList.AddProbe(probe);
                }
                else if (probeList[0].ProbeType == probe.ProbeType)
                {
                    if (probe.ProbeType == ProbeType.Vision)
                    {
                        var visionProbe1 = (VisionProbe)probeList[0];
                        var visionProbe2 = (VisionProbe)probe;

                        if (visionProbe1.InspAlgorithm.GetAlgorithmType() == visionProbe2.InspAlgorithm.GetAlgorithmType())
                        {
                            filterredProbeList.AddProbe(probe);
                        }
                    }
                    else
                    {
                        filterredProbeList.AddProbe(probe);
                    }
                }
            }

            return filterredProbeList;
        }

        public object GetParamValue(string valueName)
        {
            object valueObj = null;

            foreach (Probe probe in probeList)
            {
                object curValue = probe.GetParamValue(valueName);
                if (valueObj == null)
                {
                    valueObj = curValue;
                }
                else
                {
                    var curEnumerable = curValue as IEnumerable;
                    if (valueObj is IEnumerable varEnumerable)
                    {
                        if (curEnumerable == null)
                        {
                            return null;
                        }

                        string[] array1 = curEnumerable.Cast<object>()
                            .Select(x => x.ToString()).ToArray();
                        string[] array2 = varEnumerable.Cast<object>()
                            .Select(x => x.ToString()).ToArray();

                        if (array1.Length != array2.Length)
                        {
                            return null;
                        }

                        for (int i = 0; i < array1.Length; i++)
                        {
                            if (array1[i] != array2[i])
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        if (valueObj != curValue)
                        {
                            return null;
                        }
                    }
                }
            }

            return valueObj;
        }

        public string GetParamValueStr(string valueName)
        {
            string valueStr = "";

            foreach (Probe probe in probeList)
            {
                string curValue = probe.GetParamValue(valueName).ToString();
                if (valueStr == "")
                {
                    valueStr = curValue;
                }
                else
                {
                    if (valueStr != curValue)
                    {
                        return "";
                    }
                }
            }

            return valueStr;
        }

        public CheckState GetCheckState(string valueName)
        {
            var checkState = new CheckState();
            checkState = CheckState.Indeterminate;

            foreach (Probe probe in probeList)
            {
                bool bValue = (bool)probe.GetParamValue(valueName);
                if (checkState == CheckState.Indeterminate)
                {
                    checkState = (bValue == true ? CheckState.Checked : CheckState.Unchecked);
                }
                else
                {
                    if (((checkState == CheckState.Checked) && bValue == false) || (checkState == CheckState.Unchecked) && bValue == true)
                    {
                        return CheckState.Indeterminate;
                    }
                }
            }

            return checkState;
        }

        public Target GetSingleTarget()
        {
            Target target = null;
            foreach (Probe probe in probeList)
            {
                if (target == null)
                {
                    target = probe.Target;
                }
                else if (target != probe.Target)
                {
                    return null;
                }
            }

            return target;
        }

        public string GetProbeType()
        {
            string probeType = "";
            foreach (Probe probe in probeList)
            {
                string curProbeType = "";
                if (probe is VisionProbe visionProbe)
                {
                    curProbeType = visionProbe.InspAlgorithm.GetAlgorithmType();
                }
                else
                {
                    curProbeType = probe.ProbeType.ToString();
                }

                if (probeType == "")
                {
                    probeType = curProbeType;
                }
                else if (probeType != curProbeType)
                {
                    probeType = "Multi Selected";
                    break;
                }
            }

            return probeType;
        }
    }
}
