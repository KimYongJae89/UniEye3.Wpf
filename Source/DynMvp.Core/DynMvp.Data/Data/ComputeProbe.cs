using DynMvp.Base;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DynMvp.Data
{
    public enum ComputeType
    {
        Add, Subtract, Multiply, Divide, Equal, NotEqual, Greater, GreaterEqual, Less, LessEq, Not, And, Or, Distance
    }

    public class ComputeItem
    {
        public string Name { get; set; }
        public ComputeType Type { get; set; }
        public string Operand1 { get; set; }
        public string Operand2 { get; set; }

        public bool Compute(ComputeResultList computeResultList, out object value)
        {

            value = 0;

            bool result = false;
            if (computeResultList.GetValue(Operand1, out object value1) == true &&
                    computeResultList.GetValue(Operand2, out object value2) == true)
            {
                if (MathHelper.IsBoolean(value1) && MathHelper.IsBoolean(value2))
                {
                    result = ComputeBoolean(value1, value2, out value);
                }
                else if (MathHelper.IsNumeric(value1) && MathHelper.IsNumeric(value2))
                {
                    result = ComputeNumeric(value1, value2, out value);
                }
                else if (value1 is string && value2 is string)
                {
                    result = ComputeString(value1, value2, out value);
                }
                else if (value1 is PointF && value2 is PointF)
                {
                    result = ComputeDistance(value1, value2, out value);
                }
            }

            return result;
        }

        private bool ComputeString(object value1, object value2, out object value)
        {
            float fValue1 = Convert.ToSingle(value1);
            float fValue2 = Convert.ToSingle(value2);

            value = 0;

            if (Type == ComputeType.Add)
            {
                string resultString = value1.ToString() + value2.ToString();
                value = resultString;

                return true;
            }
            else if (Type == ComputeType.Equal)
            {
                bool bResult = (value1.ToString() == value2.ToString());
                value = bResult;

                return true;
            }

            return false;
        }

        private bool ComputeDistance(object value1, object value2, out object value)
        {
            var fValue1 = (PointF)value1;
            var fValue2 = (PointF)value2;

            bool bResult = true;

            value = 0;
            if (Type == ComputeType.Distance)
            {
                value = CalculateDistance(fValue1, fValue2);
            }

            return bResult;
        }
        private double CalculateDistance(PointF point1, PointF point2)
        {
            double resultX = Math.Pow(point2.X - (double)point1.X, 2);
            double resultY = Math.Pow(point2.Y - (double)point1.Y, 2);
            double length = Math.Sqrt(resultX + resultY);

            return length;
        }

        private bool ComputeBoolean(object value1, object value2, out object value)
        {
            bool bValue1 = Convert.ToBoolean(value1);
            bool bValue2 = Convert.ToBoolean(value2);

            value = 0;

            bool bResult = true;
            if (Type == ComputeType.And)
            {
                value = bValue1 && bValue2;
            }
            else if (Type == ComputeType.Or)
            {
                value = bValue1 || bValue2;
            }
            else if (Type == ComputeType.Not)
            {
                value = !bValue1;
            }
            else if (Type == ComputeType.Equal)
            {
                value = (bValue1 == bValue2);
            }
            else if (Type == ComputeType.NotEqual)
            {
                value = (bValue1 != bValue2);
            }
            else
            {
                bResult = false;
            }

            return bResult;
        }

        private bool ComputeNumeric(object value1, object value2, out object value)
        {
            float fValue1 = Convert.ToSingle(value1);
            float fValue2 = Convert.ToSingle(value2);

            value = 0;

            bool bResult = true;
            switch (Type)
            {
                case ComputeType.Add:
                    value = (fValue1 + fValue2);
                    break;
                case ComputeType.Subtract:
                    value = (fValue1 - fValue2);
                    break;
                case ComputeType.Multiply:
                    value = (fValue1 * fValue2);
                    break;
                case ComputeType.Divide:
                    if (fValue2 == 0)
                    {
                        LogHelper.Error("The operand2 must have none zero value when computeType is Divide.");
                        bResult = false;
                    }
                    else
                    {
                        value = fValue1 / fValue2;
                    }
                    break;
                case ComputeType.Equal:
                    value = (fValue1 == fValue2);
                    break;
                case ComputeType.NotEqual:
                    value = (fValue1 != fValue2);
                    break;
                case ComputeType.Greater:
                    value = (fValue1 > fValue2);
                    break;
                case ComputeType.GreaterEqual:
                    value = (fValue1 >= fValue2);
                    break;
                case ComputeType.Less:
                    value = (fValue1 < fValue2);
                    break;
                case ComputeType.LessEq:
                    value = (fValue1 <= fValue2);
                    break;
                default:
                    bResult = false;
                    break;
            }

            return bResult;
        }
    }

    public class ComputeResult
    {
        public string Name { get; }
        public object Value { get; }

        public ComputeResult(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ComputeResultList
    {
        private List<ComputeResult> computeResultList = new List<ComputeResult>();

        public void Clear()
        {
            computeResultList.Clear();
        }

        public void Add(ComputeResult computeResult)
        {
            computeResultList.Add(computeResult);
        }

        public bool GetValue(string name, out object value)
        {
            value = 0;
            ComputeResult computeResult = computeResultList.Find(x => x.Name == name);
            if (computeResult != null)
            {
                value = computeResult.Value;
            }

            return computeResult != null;
        }
    }

    public class ComputeProbe : Probe
    {
        public List<ComputeItem> ComputeItemList { get; set; } = new List<ComputeItem>();
        public string ResultComputeItemName { get; set; }

        public override int[] LightTypeIndexArr => null;

        public override object Clone()
        {
            var computeProbe = new ComputeProbe();
            computeProbe.Copy(this);

            return computeProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            var computeProbe = (ComputeProbe)probe;

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

        private ComputeItem GetComputeItem(string name)
        {
            foreach (ComputeItem computeItem in ComputeItemList)
            {
                if (computeItem.Name == name)
                {
                    return computeItem;
                }
            }

            return null;
        }

        public List<string> GetOperandList()
        {
            var operandList = new List<string>();
            foreach (ComputeItem computeItem in ComputeItemList)
            {
                operandList.Add(computeItem.Operand1);
                operandList.Add(computeItem.Operand2);
            }

            return operandList;
        }

        public override ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            var computeResultList = new ComputeResultList();
            computeResultList.Clear();

            List<string> operandList = GetOperandList();

            object value = null;

            foreach (string operandName in operandList)
            {
                bool bResult = probeResultList.GetResult(operandName, out value);
                if (bResult == true)
                {
                    computeResultList.Add(new ComputeResult(operandName, value));
                }
            }

            bool result;
            foreach (ComputeItem computeItem in ComputeItemList)
            {
                result = computeItem.Compute(computeResultList, out value);
                if (result == true && value != null)
                {
                    computeResultList.Add(new ComputeResult(computeItem.Name, value));
                }
            }

            result = computeResultList.GetValue(ResultComputeItemName, out value);

            var computeProbeResult = new ComputeProbeResult(this, ResultComputeItemName, (result == true ? value : 0));
            if (result == true)
            {
                if (MathHelper.IsBoolean(value) == true)
                {
                    computeProbeResult.AddResultValue(new ResultValue("Result", "", Convert.ToBoolean(value)));
                }
                else if (value is string)
                {
                    computeProbeResult.AddResultValue(new ResultValue("Result", "", value.ToString()));
                }
                else
                {
                    computeProbeResult.AddResultValue(new ResultValue("Result", "", 0, 0, Convert.ToSingle(value)));
                }
            }

            return computeProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new ComputeProbeResult(this, "Error", false);
        }
    }

    public class ComputeProbeResult : ProbeResult
    {
        public ComputeProbeResult(Probe probe, string resultName, object value)
        {
            SetResult(true);
            if (value is bool && Convert.ToBoolean(value) == false)
            {
                SetResult(false);
            }
            else
            {
                SetResult(true);
            }

            Probe = probe;
        }

        public override string ToString()
        {
            return string.Format("result value = {0}", GetResultValue(0).Value);
        }
    }
}
