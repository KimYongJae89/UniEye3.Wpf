using DynMvp.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision
{
    public class ResultValue
    {
        public string Name { get; set; }

        protected string shortResultMessage;
        public string ShortResultMessage
        {
            get => shortResultMessage;
            set => shortResultMessage = value;
        }
        public List<string> ResultMessageList { get; set; } = new List<string>();
        public string ValueType { get; set; }
        public object Value { get; set; }
        public string Unit { get; set; }
        public bool Good { get; set; }
        public object DesiredValue { get; set; }
        public float Ucl { get; set; }
        public float Lcl { get; set; }
        public string DesiredString { get; set; }
        public RotatedRect? ResultRect { get; set; }
        public bool Visible { get; set; } = true;

        // Object Tree에 항목 표시를 위해 사용됨
        public ResultValue(string name)
        {
            Name = name;
        }

        public ResultValue(string name, string unit, object value, bool good = true)
        {
            Name = name;
            Value = value;
            Good = good;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public ResultValue(string name, string unit, object value, RotatedRect resultRect, bool good = true)
        {
            Name = name;
            Value = value;
            ResultRect = resultRect;
            Good = good;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public ResultValue(string name, string unit, object desiredValue, object value, bool good = true)
        {
            Name = name;
            DesiredValue = desiredValue;
            Value = value;
            Good = good;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public ResultValue(string name, string unit, float ucl, float lcl, float desiredValue, float value, bool good = true)
        {
            Name = name;
            Ucl = ucl;
            Lcl = lcl;
            Value = value;
            Good = good;
            DesiredValue = desiredValue;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public ResultValue(string name, string unit, float ucl, float lcl, float value, bool good = true)
        {
            Name = name;
            Ucl = ucl;
            Lcl = lcl;
            Value = value;
            Good = good;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public ResultValue(string name, string unit, string desiredString, string value)
        {
            Name = name;
            DesiredString = desiredString;
            Value = value;
            Good = true;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public ResultValue(string name, string unit, string desiredString, List<string> value)
        {
            Name = name;
            DesiredString = desiredString;
            Value = value;
            Good = true;
            Unit = unit;
            ValueType = value[0].GetType().Name;
        }

        public ResultValue(string name, string unit, string value = "")
        {
            Name = name;
            Value = value;
            Unit = unit;
            ValueType = value.GetType().Name;
        }

        public void CopyFrom(ResultValue resultValue)
        {
            Name = resultValue.Name;
            shortResultMessage = resultValue.shortResultMessage;
            ResultMessageList.AddRange(resultValue.ResultMessageList);
            ValueType = resultValue.ValueType;
            Value = resultValue.Value;
            Unit = resultValue.Unit;
            Good = resultValue.Good;
            DesiredValue = resultValue.DesiredValue;
            Ucl = resultValue.Ucl;
            Lcl = resultValue.Lcl;
            DesiredString = resultValue.DesiredString;
            ResultRect = resultValue.ResultRect;
            Visible = resultValue.Visible;
        }

        public void AppendResultFigure(FigureGroup figureGroup)
        {
            if (ResultRect != null)
            {
                var pen = new Pen(Good ? Color.Lime : Color.Red);
                figureGroup.AddFigure(new RectangleFigure(ResultRect.Value, pen));
            }
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        public void FromJsonString(string dataString)
        {
            ResultValue resultValue = JsonConvert.DeserializeObject<ResultValue>(dataString);
            CopyFrom(resultValue);
        }
    }

    public class ResultValueList : List<ResultValue>
    {
        public ResultValue GetResultValue(string name)
        {
            return Find(x => x.Name == name);
        }

        public void AppendResultFigure(FigureGroup figureGroup)
        {
            foreach (ResultValue resultValue in this)
            {
                resultValue.AppendResultFigure(figureGroup);
            }
        }

        public float GetResultValueUcl(string valueName)
        {
            ResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
            {
                return resultValue.Ucl;
            }

            return 0;
        }

        public float GetResultValueLcl(string valueName)
        {
            ResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
            {
                return resultValue.Lcl;
            }

            return 0;
        }

        public float GetResultValueFloat(string valueName)
        {
            float floatValue = 0;
            ResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
            {
                floatValue = Convert.ToSingle(resultValue.Value);
            }

            return floatValue;
        }

        public string GetResultValueString(string valueName)
        {
            ResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
            {
                return resultValue.Value.ToString();
            }

            return "";
        }

        public bool GetResultValueGood(string valueName)
        {
            ResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
            {
                return resultValue.Good;
            }

            return true;
        }

        public object GetResultValueDesired(string valueName)
        {
            ResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
            {
                return resultValue.DesiredValue;
            }

            return true;
        }
    }
}
