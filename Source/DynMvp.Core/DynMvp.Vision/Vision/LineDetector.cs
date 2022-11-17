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
    public class LineDetectorParam : EdgeDetectorParam
    {
        public int NumEdgeDetector { get; set; }
        public int SearchLength { get; set; }
        public int ProjectionHeight { get; set; }
        public float SearchAngle { get; set; }

        public LineDetectorParam() : base()
        {
            NumEdgeDetector = 1;
            SearchLength = 60;
            ProjectionHeight = 10;
            SearchAngle = 90;
        }

        public override EdgeDetectorParam Clone()
        {
            var param = new LineDetectorParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(EdgeDetectorParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (LineDetectorParam)srcAlgorithmParam;

            NumEdgeDetector = param.NumEdgeDetector;
            ProjectionHeight = param.ProjectionHeight;
            SearchLength = param.SearchLength;
            SearchAngle = param.SearchAngle;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            NumEdgeDetector = Convert.ToInt32(XmlHelper.GetValue(paramElement, "NumEdgeDetector", "1"));
            SearchLength = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchLength", "60"));
            ProjectionHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ProjectionHeight", "10"));
            SearchAngle = Convert.ToSingle(XmlHelper.GetValue(paramElement, "SearchAngle", "90"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "NumEdgeDetector", NumEdgeDetector.ToString());
            XmlHelper.SetValue(paramElement, "SearchLength", SearchLength.ToString());
            XmlHelper.SetValue(paramElement, "ProjectionHeight", ProjectionHeight.ToString());
            XmlHelper.SetValue(paramElement, "SearchAngle", SearchAngle.ToString());
        }
    }

    public abstract class LineDetector
    {
        public LineDetectorParam Param { get; set; }

        public static string TypeName => "LineDetector";

        public void AppendLineDetectorFigures(FigureGroup figureGroup, PointF startPt, PointF endPt)
        {
            float xStep = (endPt.X - startPt.X) / Param.NumEdgeDetector;
            float yStep = (endPt.Y - startPt.Y) / Param.NumEdgeDetector;

            float detectorHalfHeight = Param.ProjectionHeight / 2;
            float detectorHalfWidth = Param.SearchLength / 2;

            float theta = (float)MathHelper.RadToDeg(MathHelper.arctan(startPt.Y - endPt.Y, endPt.X - startPt.X)) + 90 + (Param.SearchAngle - 90);

            for (int i = 0; i < Param.NumEdgeDetector; i++)
            {
                var centerPt = new PointF(startPt.X + xStep * (float)(i + 0.5), startPt.Y + yStep * (float)(i + 0.5));
                var rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight,
                                                    detectorHalfWidth * 2, detectorHalfHeight * 2, theta);
                figureGroup.AddFigure(new RectangleFigure(rectangle, new Pen(Color.Cyan, 1.0F)));

                PointF[] rectPos = rectangle.GetPoints();
                figureGroup.AddFigure(new LineFigure(rectPos[0], centerPt, new Pen(Color.Cyan, 1.0F)));
                figureGroup.AddFigure(new LineFigure(rectPos[3], centerPt, new Pen(Color.Cyan, 1.0F)));
            }
        }

        public abstract _1stPoliLineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext);
    }
}
