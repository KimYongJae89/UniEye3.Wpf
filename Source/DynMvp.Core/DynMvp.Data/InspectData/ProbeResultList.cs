using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Linq;

namespace DynMvp.InspectData
{
    public class ProbeResultList
    {
        private List<ProbeResult> probeResultList = new List<ProbeResult>();

        public virtual string GetGoodNgStr()
        {
            return (IsGood() ? StringManager.GetString("OK") : StringManager.GetString("NG"));
        }

        public IEnumerator<ProbeResult> GetEnumerator()
        {
            return probeResultList.GetEnumerator();
        }

        public IEnumerable<ProbeResult> GetEnumerable()
        {
            return probeResultList;
        }

        public virtual Judgment Judgment
        {
            get
            {
                if (probeResultList.Count == 0)
                {
                    return Judgment.Skip;
                }

                bool falseRejectExist = false;
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Result.Judgement == Judgment.NG)
                    {
                        return Judgment.NG;
                    }

                    if (probeResult.Result.Judgement == Judgment.Overkill)
                    {
                        falseRejectExist = true;
                    }
                }

                return (falseRejectExist ? Judgment.Overkill : Judgment.OK);
            }
        }

        public virtual bool IsGood()
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.IsNG())
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool IsOverkill()
        {
            bool overkillExist = false;
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.IsNG())
                {
                    return false;
                }

                if (probeResult.IsOverkill())
                {
                    overkillExist = true;
                }
            }

            return overkillExist == true;
        }

        public bool DifferentProductDetected
        {
            get
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.DifferentProductDetected == true)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool StepBlocked
        {
            get
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    if (probeResult.Probe.StepBlocker == true && probeResult.IsNG())
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Clear()
        {
            lock (probeResultList)
            {
                probeResultList.Clear();
            }
        }

        public void AddProbeResult(ProbeResult probeResult)
        {
            lock (probeResultList)
            {
                probeResultList.Add(probeResult);
            }
        }

        public void AddProbeResult(ProbeResultList probeResultList)
        {
            lock (probeResultList)
            {
                foreach (ProbeResult probeResult in probeResultList)
                {
                    AddProbeResult(probeResult);
                }
            }
        }

        public void ArrangeResultList()
        {
            lock (probeResultList)
            {
                probeResultList.Sort((f, g) => f.StepNo.CompareTo(g.StepNo));
            }
        }

        public virtual void SetGood()
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.IsNG())
                {
                    probeResult.SetOverkill();
                }
            }
        }

        public virtual void SetDefect()
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                probeResult.Result.SetResult(false);
            }
        }

        public int Count()
        {
            return probeResultList.Count;
        }

        public void AppendResultFigures(FigureGroup figureGroup, ResultImageType resultImageType)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                probeResult.AppendResultFigures(figureGroup, resultImageType);
            }
        }

        public void AppendResultMessage(Message resultMessage)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                probeResult.AppendResultMessage(resultMessage);
            }
        }


        public void GetCamResult(string inspectionStepName, int cameraIndex, ProbeResultList camResult, bool includeAccept = false)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe.Target.InspectStep.Name == inspectionStepName &&
                    probeResult.Probe.Target.CameraIndex == cameraIndex && (includeAccept || probeResult.IsNG()))
                {
                    camResult.AddProbeResult(probeResult);
                }
            }
        }

        public bool IsDefected(Target target)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe.Target == target)
                {
                    if (probeResult.IsNG())
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public bool IsDefected(Probe probe)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe == probe)
                {
                    if (probeResult.IsNG())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsDefected()
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.IsNG())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsDefected(string stepName, int cameraIndex)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                Target target = probeResult.Probe.Target;
                if (target.CameraIndex == cameraIndex && target.InspectStep.Name == stepName)
                {
                    if (probeResult.IsNG())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsInspected(string stepName, int cameraIndex)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                Target target = probeResult.Probe.Target;
                if (target.CameraIndex == cameraIndex && target.InspectStep.Name == stepName)
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetResult(string name, out object value)
        {
            value = null;

            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.TargetName == name)
                {
                    ProbeResultList targetResult = GetTargetResult(name);
                    value = targetResult.IsGood();
                    return true;
                }
                else if (probeResult.ProbeName == name)
                {
                    value = probeResult.IsGood();
                    return true;
                }
                else if (name.Contains(probeResult.ProbeName) == true)
                {
                    string[] words = name.Split(new char[] { '.' });
                    string resultValueName = words[words.Count() - 1];

                    ResultValue probeResultValue = probeResult.GetResultValue(resultValueName);
                    if (probeResultValue != null)
                    {
                        value = probeResultValue.Value;
                        return true;
                    }
                }
            }

            return false;
        }

        public ProbeResultList GetTargetResult(Target target)
        {
            var targetResult = new ProbeResultList();

            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe.Target == target)
                {
                    targetResult.AddProbeResult(probeResult);
                }
            }

            return targetResult;
        }

        public ProbeResultList GetTargetResult(string targetIdOrName)
        {
            var targetResult = new ProbeResultList();

            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.TargetName == targetIdOrName)
                {
                    targetResult.AddProbeResult(probeResult);
                }
                else if (probeResult.Probe != null)
                {
                    if (probeResult.Probe.Target.Name == targetIdOrName || probeResult.Probe.Target.FullId == targetIdOrName)
                    {
                        targetResult.AddProbeResult(probeResult);
                    }
                }
            }

            return targetResult;
        }

        public ProbeResult this[int index] => probeResultList[index];

        public ProbeResult GetProbeResult(int probeIndex)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe.Id == probeIndex)
                {
                    return probeResult;
                }
            }

            return null;
        }

        public ProbeResult GetProbeResult(Probe probe)
        {
            if (probe == null)
            {
                return null;
            }

            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe == probe)
                {
                    return probeResult;
                }
            }

            return null;
        }

        public ProbeResult GetProbeResult(string probeIdOrName)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.Probe != null)
                {
                    if (probeResult.Probe.FullId == probeIdOrName || probeResult.Probe.Name == probeIdOrName)
                    {
                        return probeResult;
                    }
                }
                if (probeResult.ProbeName == probeIdOrName)
                {
                    return probeResult;
                }
            }

            return null;
        }

        public ProbeResult GetProbeResult(string targetIdOrName, int probeIndex)
        {
            ProbeResultList targetResult = GetTargetResult(targetIdOrName);
            if (targetResult.Count() > 0)
            {
                return targetResult.GetProbeResult(probeIndex);
            }

            return null;
        }

        public void GetResultCount(ResultCount resultCount)
        {
            foreach (ProbeResult probeResult in probeResultList)
            {
                if (probeResult.IsNG())
                {
                    string typeName = probeResult.TargetType;
                    if (string.IsNullOrEmpty(typeName) == false)
                    {
                        if (resultCount.numTargetTypeDefects.TryGetValue(typeName, out int count))
                        {
                            resultCount.numTargetTypeDefects[typeName]++;
                        }
                        else
                        {
                            resultCount.numTargetTypeDefects[typeName] = 1; ;
                        }
                    }

                    string targetName = probeResult.TargetName;
                    if (string.IsNullOrEmpty(targetName) == false)
                    {
                        if (resultCount.numTargetDefects.TryGetValue(targetName, out int count))
                        {
                            resultCount.numTargetDefects[targetName]++;
                        }
                        else
                        {
                            resultCount.numTargetDefects[targetName] = 1;
                        }
                    }
                    resultCount.numReject++;
                }
                else if (probeResult.IsOverkill())
                {
                    resultCount.numFalseReject++;
                }
                else
                {
                    resultCount.numAccept++;
                }
            }
        }

        public int GetTargetDefectCount()
        {
            var resultCount = new ResultCount();
            GetResultCount(resultCount);

            return resultCount.numTargetDefects.Count();
        }

        public int GetProbeDefectCount()
        {
            var resultCount = new ResultCount();
            GetResultCount(resultCount);

            return resultCount.numReject;
        }
    }
}
