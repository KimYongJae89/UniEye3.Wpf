using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Data
{
    public enum EIntervalSetting
    {
        EA, VALUE,
    }

    public enum EThicknessTargetType
    {
        Thickness, Refraction,
    }

    public class ThicknessChartSetting : Observable
    {
        #region 필드
        private float maxValue;
        private float minValue;
        private float targetValue;
        private float minmaxPercentValue;
        private float startPos;
        private float endPos;
        private float validStart;
        private float validEnd;
        private float errorPercentValue;
        private float warningPercentValue;
        private float upperErrorValue;
        private float upperWarningValue;
        private float lowerWarningValue;
        private float lowerErrorValue;
        private float xIntervalSetting;
        private EIntervalSetting xIntervalMode;
        private float yIntervalSetting;
        private EIntervalSetting yIntervalMode;
        private int overlayCount;
        private int averageCount;
        private bool targetLine;
        private bool errorLine;
        private bool warningLine;
        private bool displayPercent;
        private bool autoTarget;
        private bool autoRange;
        private float graphScale;
        private string layerName;
        private EThicknessTargetType targetType;
        #endregion

        #region 생성자
        public ThicknessChartSetting()
        {
            maxValue = 1.05f;
            minValue = 0.95f;
            targetValue = 1;
            minmaxPercentValue = 5;
            errorPercentValue = 3;
            warningPercentValue = 1.5f;
            upperErrorValue = 1.03f;
            upperWarningValue = 1.015f;
            lowerWarningValue = 0.985f;
            lowerErrorValue = 0.97f;
            xIntervalSetting = 8;
            xIntervalMode = EIntervalSetting.EA;
            yIntervalSetting = 6;
            yIntervalMode = EIntervalSetting.EA;
            overlayCount = 5;
            averageCount = 0;
            targetLine = false;
            errorLine = false;
            warningLine = false;
            displayPercent = false;
            autoTarget = true;
            autoRange = true;
            graphScale = 0.6f;
            layerName = "";
            targetType = EThicknessTargetType.Thickness;
        }
        #endregion

        #region 속성
        public float MaxValue
        {
            get => maxValue;
            set => Set(ref maxValue, value);
        }

        public float MinValue
        {
            get => minValue;
            set => Set(ref minValue, value);
        }

        public float TargetValue
        {
            get => targetValue;
            set
            {
                Set(ref targetValue, value);
                MaxValue = (float)(Convert.ToDouble(targetValue) * (1 + (Convert.ToDouble(minmaxPercentValue) / 100)));
                MinValue = (float)(Convert.ToDouble(targetValue) * (1 - (Convert.ToDouble(minmaxPercentValue) / 100)));
                UpperErrorValue = (float)(Convert.ToDouble(targetValue) * (1 + (Convert.ToDouble(errorPercentValue) / 100)));
                LowerErrorValue = (float)(Convert.ToDouble(targetValue) * (1 - (Convert.ToDouble(errorPercentValue) / 100)));
                UpperWarningValue = (float)(Convert.ToDouble(targetValue) * (1 + (Convert.ToDouble(warningPercentValue) / 100)));
                LowerWarningValue = (float)(Convert.ToDouble(targetValue) * (1 - (Convert.ToDouble(warningPercentValue) / 100)));

                OnPropertyChanged("YInterval");
            }
        }

        public float MinmaxPercentValue
        {
            get => minmaxPercentValue;
            set
            {
                Set(ref minmaxPercentValue, value);
                MaxValue = (float)(Convert.ToDouble(targetValue) * (1 + (Convert.ToDouble(minmaxPercentValue) / 100)));
                MinValue = (float)(Convert.ToDouble(targetValue) * (1 - (Convert.ToDouble(minmaxPercentValue) / 100)));
            }
        }

        public float StartPos
        {
            get => startPos;
            set
            {
                Set(ref startPos, value);
                OnPropertyChanged("XInterval");
            }
        }

        public float EndPos
        {
            get => endPos;
            set
            {
                Set(ref endPos, value);
                OnPropertyChanged("XInterval");
            }
        }

        public float ValidStart
        {
            get => validStart;
            set => Set(ref validStart, value);
        }

        public float ValidEnd
        {
            get => validEnd;
            set => Set(ref validEnd, value);
        }

        public float ErrorPercentValue
        {
            get => errorPercentValue;
            set
            {
                Set(ref errorPercentValue, value);
                UpperErrorValue = (float)(Convert.ToDouble(targetValue) * (1 + (Convert.ToDouble(errorPercentValue) / 100)));
                LowerErrorValue = (float)(Convert.ToDouble(targetValue) * (1 - (Convert.ToDouble(errorPercentValue) / 100)));
            }
        }

        public float WarningPercentValue
        {
            get => warningPercentValue;
            set
            {
                Set(ref warningPercentValue, value);
                UpperWarningValue = (float)(Convert.ToDouble(targetValue) * (1 + (Convert.ToDouble(warningPercentValue) / 100)));
                LowerWarningValue = (float)(Convert.ToDouble(targetValue) * (1 - (Convert.ToDouble(warningPercentValue) / 100)));
            }
        }

        public float UpperErrorValue
        {
            get => upperErrorValue;
            set => Set(ref upperErrorValue, value);
        }

        public float UpperWarningValue
        {
            get => upperWarningValue;
            set => Set(ref upperWarningValue, value);
        }

        public float LowerWarningValue
        {
            get => lowerWarningValue;
            set => Set(ref lowerWarningValue, value);
        }

        public float LowerErrorValue
        {
            get => lowerErrorValue;
            set => Set(ref lowerErrorValue, value);
        }

        public float XInterval
        {
            get
            {
                switch (XIntervalMode)
                {
                    case EIntervalSetting.VALUE: return xIntervalSetting;
                    case EIntervalSetting.EA: return Convert.ToSingle((endPos - startPos) / xIntervalSetting);
                    default: return xIntervalSetting;
                }
            }
        }

        public EIntervalSetting XIntervalMode
        {
            get => xIntervalMode;
            set
            {
                Set(ref xIntervalMode, value);
                OnPropertyChanged("XInterval");
            }
        }

        public float XIntervalSetting
        {
            get => xIntervalSetting;
            set
            {
                Set(ref xIntervalSetting, value);
                OnPropertyChanged("XInterval");
            }
        }

        public float YInterval
        {
            get
            {
                switch (YIntervalMode)
                {
                    case EIntervalSetting.VALUE: return yIntervalSetting;
                    case EIntervalSetting.EA: return Convert.ToSingle((maxValue - minValue) / yIntervalSetting);
                    default: return yIntervalSetting;
                }
            }
        }

        public EIntervalSetting YIntervalMode
        {
            get => yIntervalMode;
            set
            {
                Set(ref yIntervalMode, value);
                OnPropertyChanged("YInterval");
            }
        }

        public float YIntervalSetting
        {
            get => yIntervalSetting;
            set
            {
                Set(ref yIntervalSetting, value);
                OnPropertyChanged("YInterval");
            }
        }

        public int OverlayCount
        {
            get => overlayCount;
            set => Set(ref overlayCount, value);
        }

        public int AverageCount
        {
            get => averageCount;
            set => Set(ref averageCount, value);
        }

        public bool TargetLine
        {
            get => targetLine;
            set => Set(ref targetLine, value);
        }

        public bool ErrorLine
        {
            get => errorLine;
            set => Set(ref errorLine, value);
        }

        public bool WarningLine
        {
            get => warningLine;
            set => Set(ref warningLine, value);
        }

        public bool DisplayPercent
        {
            get => displayPercent;
            set => Set(ref displayPercent, value);
        }

        public bool AutoTarget
        {
            get => autoTarget;
            set => Set(ref autoTarget, value);
        }

        public bool AutoRange
        {
            get => autoRange;
            set => Set(ref autoRange, value);
        }

        public float GraphScale
        {
            get => graphScale;
            set => Set(ref graphScale, value);
        }

        public string LayerName
        {
            get => layerName;
            set => Set(ref layerName, value);
        }

        public EThicknessTargetType TargetType
        {
            get => targetType;
            set => Set(ref targetType, value);
        }
        #endregion

        #region 메서드
        public void CopyFrom(ThicknessChartSetting chartSetting)
        {
            MaxValue = chartSetting.MaxValue;
            MinValue = chartSetting.MinValue;
            TargetValue = chartSetting.TargetValue;
            MinmaxPercentValue = chartSetting.MinmaxPercentValue;
            ValidStart = chartSetting.ValidStart;
            ValidEnd = chartSetting.ValidEnd;
            ErrorPercentValue = chartSetting.ErrorPercentValue;
            WarningPercentValue = chartSetting.WarningPercentValue;
            UpperErrorValue = chartSetting.UpperErrorValue;
            UpperWarningValue = chartSetting.UpperWarningValue;
            LowerWarningValue = chartSetting.LowerWarningValue;
            LowerErrorValue = chartSetting.LowerErrorValue;
            XIntervalSetting = chartSetting.XIntervalSetting;
            XIntervalMode = chartSetting.XIntervalMode;
            YIntervalSetting = chartSetting.YIntervalSetting;
            YIntervalMode = chartSetting.YIntervalMode;
            OverlayCount = chartSetting.OverlayCount;
            AverageCount = chartSetting.AverageCount;
            TargetLine = chartSetting.TargetLine;
            ErrorLine = chartSetting.ErrorLine;
            WarningLine = chartSetting.WarningLine;
            DisplayPercent = chartSetting.DisplayPercent;
            AutoTarget = chartSetting.AutoTarget;
            AutoRange = chartSetting.AutoRange;
            GraphScale = chartSetting.GraphScale;
            LayerName = chartSetting.LayerName;
            TargetType = chartSetting.TargetType;
        }

        public ThicknessChartSetting Clone()
        {
            var chartSetting = new ThicknessChartSetting();

            chartSetting.MaxValue = maxValue;
            chartSetting.MinValue = minValue;
            chartSetting.MinmaxPercentValue = minmaxPercentValue;
            chartSetting.TargetValue = targetValue;
            chartSetting.StartPos = startPos;
            chartSetting.EndPos = endPos;
            chartSetting.ValidStart = validStart;
            chartSetting.ValidEnd = validEnd;
            chartSetting.ErrorPercentValue = errorPercentValue;
            chartSetting.WarningPercentValue = warningPercentValue;
            chartSetting.UpperErrorValue = upperErrorValue;
            chartSetting.UpperWarningValue = upperWarningValue;
            chartSetting.LowerWarningValue = lowerWarningValue;
            chartSetting.LowerErrorValue = lowerErrorValue;
            chartSetting.XIntervalSetting = xIntervalSetting;
            chartSetting.XIntervalMode = xIntervalMode;
            chartSetting.YIntervalSetting = yIntervalSetting;
            chartSetting.YIntervalMode = yIntervalMode;
            chartSetting.OverlayCount = overlayCount;
            chartSetting.AverageCount = averageCount;
            chartSetting.TargetLine = targetLine;
            chartSetting.ErrorLine = errorLine;
            chartSetting.WarningLine = warningLine;
            chartSetting.DisplayPercent = displayPercent;
            chartSetting.AutoTarget = autoTarget;
            chartSetting.autoRange = autoRange;
            chartSetting.GraphScale = graphScale;
            chartSetting.LayerName = layerName;
            chartSetting.TargetType = targetType;

            return chartSetting;
        }
        #endregion
    }

    public class ThicknessChartSettingJsonConverter : JsonCreationConverter<ThicknessChartSetting>
    {
        #region 메서드
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        protected override ThicknessChartSetting Create(Type objectType, JObject jObject)
        {
            return new ThicknessChartSetting();
        }
        #endregion
    }
}
