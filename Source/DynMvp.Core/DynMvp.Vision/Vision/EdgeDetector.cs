using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class EdgeDetectorParam
    {
        public EdgeType EdgeType { get; set; }
        public int GausianFilterSize { get; set; }
        public int MedianFilterSize { get; set; }
        public int Threshold { get; set; }
        public int MorphologyFilterSize { get; set; }
        public int AverageCount { get; set; }
        public string EdgeDirection { get; set; }

        public EdgeDetectorParam()
        {
            EdgeType = EdgeType.LightToDark;
            GausianFilterSize = 3;
            Threshold = 200;
            MorphologyFilterSize = 20;
            EdgeDirection = "Horizontal";
            MedianFilterSize = 3;
            AverageCount = 20;
        }

        public virtual EdgeDetectorParam Clone()
        {
            var param = new EdgeDetectorParam();

            param.Copy(this);

            return param;
        }

        public virtual object GetParamValue(string paramName)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }

            return "";
        }

        public virtual bool SetParamValue(string paramName, object value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, value);
                return true;
            }

            return false;
        }

        public virtual void Copy(EdgeDetectorParam srcParam)
        {
            EdgeType = srcParam.EdgeType;
            GausianFilterSize = srcParam.GausianFilterSize;
            Threshold = srcParam.Threshold;
            MorphologyFilterSize = srcParam.MorphologyFilterSize;
            EdgeDirection = srcParam.EdgeDirection;
            MedianFilterSize = srcParam.MedianFilterSize;
            AverageCount = srcParam.AverageCount;
        }

        public virtual void LoadParam(XmlElement paramElement)
        {
            EdgeType = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(paramElement, "EdgeType", "LightToDark"));
            GausianFilterSize = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FilterSize", "3"));
            Threshold = Convert.ToInt32(XmlHelper.GetValue(paramElement, "EdgeThreshold", "200"));
            MorphologyFilterSize = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MorphologyFilterSize", "20"));
            EdgeDirection = XmlHelper.GetValue(paramElement, "EdgeDirection", "Horizontal");
            MedianFilterSize = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MedianFilterSize", "3"));
            AverageCount = Convert.ToInt32(XmlHelper.GetValue(paramElement, "AverageCount", "20"));
        }

        public virtual void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "EdgeType", EdgeType.ToString());
            XmlHelper.SetValue(paramElement, "FilterSize", GausianFilterSize.ToString());
            XmlHelper.SetValue(paramElement, "EdgeThreshold", Threshold.ToString());
            XmlHelper.SetValue(paramElement, "MorphologyFilterSize", MorphologyFilterSize.ToString());
            XmlHelper.SetValue(paramElement, "EdgeDirection", EdgeDirection.ToString());
            XmlHelper.SetValue(paramElement, "MedianFilterSize", MedianFilterSize.ToString());
            XmlHelper.SetValue(paramElement, "AverageCount", AverageCount.ToString());
        }
    }

    public abstract class EdgeDetector
    {
        public EdgeDetectorParam Param { get; set; }


        public static string TypeName => "EdgeDetector";

        public abstract EdgeDetectionResult Detect(AlgoImage algoImage, RotatedRect rotatedRect, DebugContext debugContext);
        public abstract AlgoImage GetEdgeImage(AlgoImage algoImage);
    }

    public class EdgeDetectionResult
    {
        public PointF FallingEdgePosition { get; set; } = new PointF();
        public PointF RealFallingEdgePosition { get; set; } = new PointF();
        public PointF RissingEdgePosition { get; set; } = new PointF();
        public PointF RealRissingEdgePosition { get; set; } = new PointF();
        public bool Result { get; set; } = false;
        public float YOffset { get; set; }
    }
}
