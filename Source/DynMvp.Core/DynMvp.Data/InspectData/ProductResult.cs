using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DynMvp.InspectData
{
    public class ResultCount
    {
        public int numAccept;
        public int numReject;
        public int numFalseReject;
        public Judgment Judgment
        {
            get
            {
                if (numReject > 0)
                {
                    return Judgment.NG;
                }
                else if (numFalseReject > 0)
                {
                    return Judgment.Overkill;
                }
                else
                {
                    return Judgment.OK;
                }
            }
        }

        public Dictionary<string, int> numTargetTypeDefects = new Dictionary<string, int>();
        public Dictionary<string, int> numTargetDefects = new Dictionary<string, int>();
    }

    public class TargetImage
    {
        public string TargetId { get; }
        public int LightTypeIndex { get; }
        public Image2D Image { get; private set; }

        public TargetImage(string targetId, int lightTypeIndex, Image2D image)
        {
            TargetId = targetId;
            LightTypeIndex = lightTypeIndex;
            Image = image;
        }

        public void Clear()
        {
            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }
        }
    }

    public class AlignResult
    {
        public SizeF FiducialProbeOffset { get; set; }
        public float FiducialProbeAngle { get; set; }
        public RotatedRect FiducialProbeRect { get; set; }
    }

    public class ProductResult : ProbeResultList
    {
        public string MachineName { get; set; }
        public string ModelName { get; set; }
        public string InspectionNo { get; set; }
        public string InputBarcode { get; set; }
        public string OutputBarcode { get; set; }
        public DateTime InspectStartTime { get; set; }
        public DateTime InspectEndTime { get; set; }

        public TimeSpan InspectTimeSpan => (InspectEndTime - InspectStartTime);

        public static string GetInspectionNo()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        public string JobOperator { get; set; }
        public string ResultPath { get; set; }
        public string DbPath { get; set; }
        public bool RetryInspection { get; set; }
        public bool InspectionCancelled { get; set; }
        public int RepeatCount { get; set; }
        public int ResultViewIndex { get; set; }
        public bool ResultSent { get; set; } = false;

        protected List<int> targetGroupInspected = new List<int>();
        public List<int> TargetGroupInspected
        {
            get => targetGroupInspected;
            set => targetGroupInspected = value;
        }
        public string ModuleId { get; set; } = "";
        public string FileName { get; set; } = "";

        private Dictionary<string, object> additionalValue = new Dictionary<string, object>();

        public void ClearExtraResult()
        {
            additionalValue.Clear();
        }

        public void AddAdditionalValue(string name, object value)
        {
            additionalValue.Add(name, value);
        }

        public object GetAdditionalValue(string name)
        {
            if (additionalValue.TryGetValue(name, out object value) == false)
            {
                return null;
            }

            return value;
        }

        public ProductResult()
        {
            InspectStartTime = DateTime.Now;
        }

        private bool stepBlocked;
        public new bool StepBlocked
        {
            set => stepBlocked = value;
            get
            {
                if (stepBlocked == true)
                {
                    return true;
                }

                return base.StepBlocked;
            }
        }
        public int TriggerIndex { get; set; } = -1;

        public virtual void CopyFrom(ProductResult result)
        {
            MachineName = result.MachineName;
            ModelName = result.ModelName;
            InspectionNo = result.InspectionNo;
            InputBarcode = result.InputBarcode;
            OutputBarcode = result.OutputBarcode;
            InspectStartTime = result.InspectStartTime;
            InspectEndTime = result.InspectEndTime;
            JobOperator = result.JobOperator;
            ResultPath = result.ResultPath;
            DbPath = result.DbPath;
            RetryInspection = result.RetryInspection;
            InspectionCancelled = result.InspectionCancelled;
            RepeatCount = result.RepeatCount;
            ResultViewIndex = result.ResultViewIndex;
            ResultSent = result.ResultSent;

            targetGroupInspected.AddRange(result.TargetGroupInspected);

            ModuleId = result.ModuleId;
            FileName = result.FileName;

            foreach (KeyValuePair<string, object> pair in result.additionalValue)
            {
                AddAdditionalValue(pair.Key, pair.Value);
            }

            TriggerIndex = result.TriggerIndex;
        }


        ~ProductResult()
        {
        }

        public Image2D GetTargetImage(string targetId, int lightTypeIndex = 0)
        {
            return null;
        }

        public virtual string GetSummaryMessage()
        {
            var resultCount = new ResultCount();
            GetResultCount(resultCount);

            string summaryMessage = "";

            if (resultCount.numReject == 0)
            {
                summaryMessage = StringManager.GetString("The product is Good");
            }
            else
            {
                summaryMessage = string.Format("Defect Targets : {0}", resultCount.numTargetDefects.Count());

                if (DifferentProductDetected)
                {
                    summaryMessage += " / " + StringManager.GetString("Different Product is Detected");
                }

                if (StepBlocked)
                {
                    summaryMessage += " / " + StringManager.GetString("Step Blocked. Check the product.");
                }
            }

            return summaryMessage;
        }

        public string GetResultString()
        {
            throw new NotImplementedException();
        }
    }
}
