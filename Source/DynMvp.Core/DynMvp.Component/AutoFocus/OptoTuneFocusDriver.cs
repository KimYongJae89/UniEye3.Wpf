using DynMvp.Base;
using DynMvp.Component.AutoFocus;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Component.AutoFocus
{
    public class OptoTuneFocusDriverSetting
    {
        public enum Params
        {
            COM,
            MinCurrent,
            MaxCurrent,
            CoarseCurrentStep,
            MiddleCurrentStep,
            FineCurrentStep,
            PARAM_MAX
        }
        public int ComPort { get; set; } = 3;
        public double MinFocusCurrent { get; set; } = 0.0f;
        public double MaxFocusCurrent { get; set; } = 200.0f;
        public double CoarseCurrentStep { get; set; } = 1.0f;
        public double MiddleCurrentStep { get; set; } = 0.5f;
        public double FineCurrentStep { get; set; } = 0.1f;

        public string GetParamName(int iIdx)
        {
            string strName = "";

            switch ((Params)iIdx)
            {
                case Params.COM: strName = "COM"; break;
                case Params.MinCurrent: strName = "MinimumCurrent"; break;
                case Params.MaxCurrent: strName = "MaximumCurrent"; break;
                case Params.CoarseCurrentStep: strName = "CoarseStep"; break;
                case Params.MiddleCurrentStep: strName = "MiddleStep"; break;
                case Params.FineCurrentStep: strName = "FineStep"; break;
            }

            return strName;
        }

        public string GetParamValue(string strParamName)
        {
            string strVal = "";
            string strName = "";
            bool bFind = false;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strName = GetParamName(i);

                if (strName == strParamName)
                {
                    bFind = true;
                    switch ((Params)i)
                    {
                        case Params.COM: strVal = ComPort.ToString(); break;
                        case Params.MinCurrent: strVal = MinFocusCurrent.ToString(); break;
                        case Params.MaxCurrent: strVal = MaxFocusCurrent.ToString(); break;
                        case Params.CoarseCurrentStep: strVal = CoarseCurrentStep.ToString(); break;
                        case Params.MiddleCurrentStep: strVal = MiddleCurrentStep.ToString(); break;
                        case Params.FineCurrentStep: strVal = FineCurrentStep.ToString(); break;
                        default: bFind = false; break;
                    }
                    break;
                }
            }
            if (bFind == false)
            {
                throw new InvalidOperationException();
            }

            return strVal;
        }

        public void SetParamValue(string strParamName, string strVal)
        {
            if (strVal == "")
            {
                return;
            }

            string strName = "";
            bool bFind = false;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strName = GetParamName(i);

                if (strName == strParamName)
                {
                    bFind = true;
                    switch ((Params)i)
                    {
                        case Params.COM: ComPort = Convert.ToInt32(strVal); break;
                        case Params.MinCurrent: MinFocusCurrent = Convert.ToDouble(strVal); break;
                        case Params.MaxCurrent: MaxFocusCurrent = Convert.ToDouble(strVal); break;
                        case Params.CoarseCurrentStep: CoarseCurrentStep = Convert.ToDouble(strVal); break;
                        case Params.MiddleCurrentStep: MiddleCurrentStep = Convert.ToDouble(strVal); break;
                        case Params.FineCurrentStep: FineCurrentStep = Convert.ToDouble(strVal); break;
                        default: bFind = false; break;
                    }
                    break;
                }
            }
            if (bFind == false)
            {
                throw new InvalidOperationException();
            }
        }

        public void Load(XmlElement xmlElement)
        {
            string strParamName;
            string strParamVal;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strParamName = GetParamName(i);
                strParamVal = XmlHelper.GetValue(xmlElement, strParamName, "");
                if (strParamVal != "")
                {
                    SetParamValue(strParamName, strParamVal);
                }
            }
        }

        public void Save(XmlElement xmlElement)
        {
            string strParamName = "";
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strParamName = GetParamName(i);
                XmlHelper.SetValue(xmlElement, strParamName, GetParamValue(strParamName));
            }
        }

    }
    public class CameraSetting
    {

    }
    public class OptoTuneFocusDriver : FocusDriver
    {
        //OptoTuneSerial optoTune;
        private dynamic optoTune;
        private OptoTuneFocusDriverSetting setting;

        public void Initialize(OptoTuneSerial optoTune, OptoTuneFocusDriverSetting setting)
        {
            this.optoTune = optoTune;
            this.setting = setting;
        }
        public void Initialize(OptoTuneLAN optoTune, OptoTuneFocusDriverSetting setting)
        {
            this.optoTune = optoTune;
            this.setting = setting;
        }
        public override double GetCurPos()
        {
            return optoTune.GetCurrent();
        }

        public override double GetMinPos()
        {
            return setting.MinFocusCurrent;
        }

        public override double GetMaxPos()
        {
            return setting.MaxFocusCurrent;
        }

        public override bool IsOnLowerLimit()
        {
            return (GetCurPos() == GetMinPos());
        }

        public override bool IsOnUpperLimit()
        {
            return (GetCurPos() == GetMaxPos());
        }

        public override bool IsOnLimit()
        {
            return IsOnLowerLimit() || IsOnUpperLimit();
        }

        public override void MoveTo(double fVal)
        {
            //StepType stepSize = StepType.Good;
            //StepDirection stepDirection = StepDirection.Forward;

            //while (true)
            //{
            //    double fCurDif = fVal - optoTune.GetCurrent();
            //    double fAbsCurDif = Math.Abs(fCurDif);

            //    if (fCurDif >= 0)
            //    {
            //        stepDirection = StepDirection.Forward;
            //    }
            //    else
            //    {
            //        stepDirection = StepDirection.Backward;
            //    }

            //    if (fAbsCurDif >= setting.CoarseCurrentStep)
            //    {
            //        stepSize = StepType.Coarse;
            //    }
            //    else if (fAbsCurDif >= setting.MiddleCurrentStep)
            //    {
            //        stepSize = StepType.Middle;
            //    }
            //    else if (fAbsCurDif >= setting.FineCurrentStep)
            //    {
            //        stepSize = StepType.Fine;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //    Step(stepSize, stepDirection);
            //    Thread.Sleep(100);
            //}
            if (fVal < setting.MinFocusCurrent)
            {
                fVal = setting.MinFocusCurrent;
            }

            if (fVal > setting.MaxFocusCurrent)
            {
                fVal = setting.MaxFocusCurrent;
            }
            optoTune.SetCurrent(fVal);
        }

        public override void Step(StepSize stepType, StepDirection direction)
        {
            double current = optoTune.GetCurrent();

            switch (stepType)
            {
                case StepSize.Coarse:
                    current += (direction == StepDirection.Forward) ? setting.CoarseCurrentStep : -setting.CoarseCurrentStep;
                    break;
                case StepSize.Middle:
                    current += (direction == StepDirection.Forward) ? setting.MiddleCurrentStep : -setting.MiddleCurrentStep;
                    break;
                case StepSize.Fine:
                    current += (direction == StepDirection.Forward) ? setting.FineCurrentStep : -setting.FineCurrentStep;
                    break;
                case StepSize.Good:
                    return;
            }

            optoTune.SetCurrent(current);

        }

        public override void Test()
        {
            double current = optoTune.GetCurrent() + setting.CoarseCurrentStep;
            if (current > setting.MaxFocusCurrent)
            {
                current = 0;
            }
            optoTune.SetCurrent(current);
        }

        public void SetSetting(OptoTuneFocusDriverSetting setting)
        {
            if (setting == null)
            {
                setting = new OptoTuneFocusDriverSetting();
            }

            this.setting = setting;
        }
    }
}
