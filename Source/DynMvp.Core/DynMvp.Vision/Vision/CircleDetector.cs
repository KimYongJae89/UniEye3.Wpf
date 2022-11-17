using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class CircleDetectorParam
    {
        public PointF CenterPosition { get; set; }
        public float InnerRadius { get; set; }
        public float OutterRadius { get; set; }
        public EdgeType EdgeType { get; set; } = EdgeType.Any;
        public float MaxAssociationDistance { get; set; }
        public float MinEdgeValue { get; set; }
        public bool Smallest { get; set; }

        public CircleDetectorParam()
        {
            CenterPosition = new PointF();
            InnerRadius = 0;
            OutterRadius = 0;
            EdgeType = EdgeType.Any;
        }

        public CircleDetectorParam Clone()
        {
            var param = new CircleDetectorParam();
            param.Copy(this);

            return param;
        }

        public void Copy(CircleDetectorParam srcParam)
        {
            CenterPosition = srcParam.CenterPosition;
            InnerRadius = srcParam.InnerRadius;
            OutterRadius = srcParam.OutterRadius;
            EdgeType = srcParam.EdgeType;
        }

        public void LoadParam(XmlElement paramElement)
        {
            float x = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CenterPosition.X", "0"));
            float y = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CenterPosition.Y", "0"));
            CenterPosition = new PointF(x, y);
            InnerRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "InnerRadius", "0"));
            OutterRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "OutterRadius", "0"));
            EdgeType = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(paramElement, "EdgeType", "Any"));
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "CenterPosition.X", CenterPosition.X.ToString());
            XmlHelper.SetValue(paramElement, "CenterPosition.Y", CenterPosition.Y.ToString());
            XmlHelper.SetValue(paramElement, "InnerRadius", InnerRadius.ToString());
            XmlHelper.SetValue(paramElement, "OutterRadius", OutterRadius.ToString());
            XmlHelper.SetValue(paramElement, "EdgeType", EdgeType.ToString());
        }
    }

    public abstract class CircleDetector
    {
        public CircleDetectorParam Param { get; set; }

        public static string TypeName => "CircleDetector";

        public abstract CircleEq Detect(AlgoImage algoImage, DebugContext debugContext);
    }
}