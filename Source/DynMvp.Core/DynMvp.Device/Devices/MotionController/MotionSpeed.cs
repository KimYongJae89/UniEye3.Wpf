using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.MotionController
{
    public class MovingParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(MovingParam))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(string) &&
                 value is MovingParam)
            {

                var movingParam = (MovingParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(MovingParamConverter))]
    public class MovingParam
    {
        public string Name { get; set; }
        public double StartVelocity { get; set; }
        public double AccelerationTimeMs { get; set; }
        public double DecelerationTimeMs { get; set; }
        public double MaxVelocity { get; set; }
        public double SCurveTimeMs { get; set; }

        public MovingParam()
        {
            Name = "";
            StartVelocity = 0.0;
            AccelerationTimeMs = 100;
            DecelerationTimeMs = 100;
            MaxVelocity = 2000.0;
            SCurveTimeMs = 0;
        }

        public MovingParam(string name, float startVelocity, float accelerationTimeMs, float decelerationTimeMs, float maxVelocity, float sCurveTimeMs)
        {
            Name = name;
            StartVelocity = startVelocity;
            AccelerationTimeMs = accelerationTimeMs;
            DecelerationTimeMs = decelerationTimeMs;
            MaxVelocity = maxVelocity;
            SCurveTimeMs = sCurveTimeMs;
        }

        public MovingParam(MovingParam srcMovingParam)
        {
            Name = srcMovingParam.Name;
            StartVelocity = srcMovingParam.StartVelocity;
            AccelerationTimeMs = srcMovingParam.AccelerationTimeMs;
            DecelerationTimeMs = srcMovingParam.DecelerationTimeMs;
            MaxVelocity = srcMovingParam.MaxVelocity;
            SCurveTimeMs = srcMovingParam.SCurveTimeMs;
        }

        public void Load(XmlElement movingParamElement)
        {
            if (movingParamElement == null)
            {
                return;
            }

            Name = XmlHelper.GetValue(movingParamElement, "Name", Name);
            StartVelocity = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "StartVelocity", StartVelocity.ToString()));
            AccelerationTimeMs = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "AccelerationTimeMs", AccelerationTimeMs.ToString()));
            DecelerationTimeMs = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "DecelerationTimeMs", DecelerationTimeMs.ToString()));
            MaxVelocity = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "MaxVelocity", MaxVelocity.ToString()));
            SCurveTimeMs = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "SCurveTimeMs", SCurveTimeMs.ToString()));
        }

        public MovingParam Clone()
        {
            return new MovingParam(this);
        }

        public void Save(XmlElement movingParamElement)
        {
            if (movingParamElement == null)
            {
                return;
            }

            XmlHelper.SetValue(movingParamElement, "Name", Name);
            XmlHelper.SetValue(movingParamElement, "StartVelocity", StartVelocity.ToString());
            XmlHelper.SetValue(movingParamElement, "AccelerationTimeMs", AccelerationTimeMs.ToString());
            XmlHelper.SetValue(movingParamElement, "DecelerationTimeMs", DecelerationTimeMs.ToString());
            XmlHelper.SetValue(movingParamElement, "MaxVelocity", MaxVelocity.ToString());
            XmlHelper.SetValue(movingParamElement, "SCurveTimeMs", SCurveTimeMs.ToString());
        }
    }

    public class HomeParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(HomeParam))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(string) &&
                 value is HomeParam)
            {

                var homeSpeed = (HomeParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public enum HomeMode
    {
        PosEndLimit, NegEndLimit, HomeSensor, ZPhase, PosEndLimit_EZ, NegEndLimit_EZ // Low_Home, Low_Home_EZ, High_Home, High_Home_EZ, Low_Home_Limit, Low_Home_Limit_EZ, High_Home_Limit, High_Home_Limit_EZ, Low_Home_EX, CurPos
    }

    public enum MoveDirection
    {
        CW, CCW
    }

    public enum HomeSignal
    {
        Low, High
    }

    [TypeConverterAttribute(typeof(HomeParamConverter))]
    public class HomeParam
    {
        public HomeMode HomeMode { get; set; }
        public MoveDirection HomeDirection { get; set; }
        public HomeSignal HomeSignal { get; set; }
        public MovingParam HighSpeed { get; set; } = new MovingParam();
        public MovingParam MediumSpeed { get; set; } = new MovingParam();
        public MovingParam FineSpeed { get; set; } = new MovingParam();

        public void Load(XmlElement homeMovingSpeedElement)
        {
            if (homeMovingSpeedElement == null)
            {
                return;
            }

            HomeMode = (HomeMode)Enum.Parse(typeof(HomeMode), XmlHelper.GetValue(homeMovingSpeedElement, "HomeMode", "NegEndLimit"));
            HomeDirection = (MoveDirection)Enum.Parse(typeof(MoveDirection), XmlHelper.GetValue(homeMovingSpeedElement, "MoveDirection", "CW"));
            HomeSignal = (HomeSignal)Enum.Parse(typeof(HomeSignal), XmlHelper.GetValue(homeMovingSpeedElement, "HomeSignal", "High"));

            XmlElement highSpeedElement = homeMovingSpeedElement["HighSpeed"];
            if (highSpeedElement != null)
            {
                HighSpeed.Load(highSpeedElement);
            }

            XmlElement mediumSpeedElement = homeMovingSpeedElement["MediumSpeed"];
            if (mediumSpeedElement != null)
            {
                MediumSpeed.Load(mediumSpeedElement);
            }

            XmlElement fineSpeedElement = homeMovingSpeedElement["FineSpeed"];
            if (fineSpeedElement != null)
            {
                FineSpeed.Load(fineSpeedElement);
            }
        }

        public void Save(XmlElement homeMovingSpeedElement)
        {
            if (homeMovingSpeedElement == null)
            {
                return;
            }

            XmlHelper.SetValue(homeMovingSpeedElement, "HomeMode", HomeMode.ToString());
            XmlHelper.SetValue(homeMovingSpeedElement, "MoveDirection", HomeDirection.ToString());
            XmlHelper.SetValue(homeMovingSpeedElement, "HomeSignal", HomeSignal.ToString());

            XmlElement highSpeedElement = homeMovingSpeedElement.OwnerDocument.CreateElement("", "HighSpeed", "");
            homeMovingSpeedElement.AppendChild(highSpeedElement);

            HighSpeed.Save(highSpeedElement);

            XmlElement mediumSpeedElement = homeMovingSpeedElement.OwnerDocument.CreateElement("", "MediumSpeed", "");
            homeMovingSpeedElement.AppendChild(mediumSpeedElement);

            MediumSpeed.Save(mediumSpeedElement);

            XmlElement fineSpeedElement = homeMovingSpeedElement.OwnerDocument.CreateElement("", "FineSpeed", "");
            homeMovingSpeedElement.AppendChild(fineSpeedElement);

            FineSpeed.Save(fineSpeedElement);
        }
    }

    // 축별로 세부적인 속도 제어가 필요한 상황에서 사용할 클래스
    public class AxisSpeedConfig
    {
        public List<MovingParam> MovingParamList { get; set; } = new List<MovingParam>();

        public void AddMovingParam(MovingParam movingParam)
        {
            MovingParamList.Add(movingParam);
        }

        public MovingParam GetMovingParam(string name)
        {
            return MovingParamList.Find(delegate (MovingParam movingParam) { return movingParam.Name == name; });
        }

        public void Load(XmlElement axisSpeedConfigElement)
        {
            foreach (XmlElement movingParamElement in axisSpeedConfigElement)
            {
                if (movingParamElement.Name == "MovingParam")
                {
                    var movingParam = new MovingParam();
                    movingParam.Load(movingParamElement);

                    MovingParamList.Add(movingParam);
                }
            }
        }

        public void Save(XmlElement axisSpeedConfigElement)
        {
            foreach (MovingParam movingParam in MovingParamList)
            {
                XmlElement movingParamElement = axisSpeedConfigElement.OwnerDocument.CreateElement("", "MovingParam", "");
                axisSpeedConfigElement.AppendChild(movingParamElement);

                movingParam.Save(movingParamElement);
            }
        }
    }

    public class MotionSpeedConfig
    {
        private string fileName = string.Empty;
        private AxisSpeedConfig[] axisSpeedConfig = null;

        protected void Initialize(int numAxis)
        {
            axisSpeedConfig = new AxisSpeedConfig[numAxis];

            fileName = string.Format(@"{0}\..\Config\MotionSpeedConfig.xml", Environment.CurrentDirectory);
            if (File.Exists(fileName))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);

                int axisId = 0;

                XmlElement motionSpeedConfigElement = xmlDocument.DocumentElement;
                foreach (XmlElement axisSpeedConfigElement in motionSpeedConfigElement)
                {
                    if (axisSpeedConfigElement.Name == "AxisSpeedConfig")
                    {
                        axisSpeedConfig[axisId].Load(axisSpeedConfigElement);
                        axisId++;
                    }
                }
            }
        }

        public AxisSpeedConfig GetAxisSpeedConfig(int axisNo)
        {
            return axisSpeedConfig[axisNo];
        }

        public void SetAxisSpeedConfig(int axisNo, AxisSpeedConfig axisSpeedConfig)
        {
            this.axisSpeedConfig[axisNo] = axisSpeedConfig;
        }

        public void Save()
        {
            if (fileName != string.Empty)
            {
                var xmlDocument = new XmlDocument();

                XmlElement motionSpeedConfigElement = xmlDocument.CreateElement("", "MotionSpeedConfig", "");
                xmlDocument.AppendChild(motionSpeedConfigElement);

                foreach (AxisSpeedConfig axisSpeedConfig in axisSpeedConfig)
                {
                    XmlElement axisSpeedConfigElement = xmlDocument.CreateElement("", "AxisSpeedConfig", "");
                    motionSpeedConfigElement.AppendChild(axisSpeedConfigElement);

                    axisSpeedConfig.Save(axisSpeedConfigElement);
                }
            }
        }
    }
}
