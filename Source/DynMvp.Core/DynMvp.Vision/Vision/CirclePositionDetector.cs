using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.OpenCv;
using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class CirclePositionDetectorParam
    {
        public float InnerRadius { get; set; }
        public float OutterRadius { get; set; }
        public float Accumulate { get; set; }
        public int Closing { get; set; }
        public float CenterDistance { get; set; }

        public CirclePositionDetectorParam()
        {
            InnerRadius = 0;
            OutterRadius = 0;
            Accumulate = 10;
            Closing = 1;
            CenterDistance = 30;
        }

        public void Copy(CirclePositionDetectorParam param)
        {
            InnerRadius = param.InnerRadius;
            OutterRadius = param.OutterRadius;
            Accumulate = param.Accumulate;
            Closing = param.Closing;
            CenterDistance = param.CenterDistance;
        }

        public void LoadParam(XmlElement paramElement)
        {
            InnerRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "InnerRadius", "0"));
            OutterRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "OutterRadius", "0"));
            Accumulate = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Accumulate", "10"));
            Closing = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Closing", "3"));
            CenterDistance = Convert.ToSingle(XmlHelper.GetValue(paramElement, "centerDistance", "30"));

        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "InnerRadius", InnerRadius.ToString());
            XmlHelper.SetValue(paramElement, "OutterRadius", OutterRadius.ToString());
            XmlHelper.SetValue(paramElement, "Accumulate", Accumulate.ToString());
            XmlHelper.SetValue(paramElement, "Closing", Closing.ToString());
            XmlHelper.SetValue(paramElement, "centerDistance", CenterDistance.ToString());
        }
    }

    public class CirclePositionDetector
    {
        public CirclePositionDetectorParam Param { get; set; } = new CirclePositionDetectorParam();

        public CirclePositionDetector()
        {
        }

        public static string TypeName => "CirclePositionDetector";

        public CircleEq Detect(AlgoImage probeClipImage, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(probeClipImage);

            imageProcessing.Binarize(probeClipImage);

            imageProcessing.Dilate(probeClipImage, Param.Closing);
            imageProcessing.Erode(probeClipImage, Param.Closing);

            AlgoImage roiImage = probeClipImage.Clone();

            CircleEq resultCircle = null;
            var circles = new List<CircleEq>();

            circles = getMinEnclosingCircle(roiImage, debugContext);

            var circleOffset = new Point();
            bool finded = false;

            if (circles != null && circles.Count != 0)
            {
                resultCircle = circles.OrderByDescending(x => x.Radius).First();
                circles.Remove(resultCircle);

                foreach (CircleEq circle in circles)
                {
                    if (Math.Sqrt(Math.Pow(resultCircle.Center.X - circle.Center.X, 2) + Math.Pow(resultCircle.Center.Y - circle.Center.Y, 2)) < Param.CenterDistance)
                    {
                        resultCircle = new CircleEq(new PointF((resultCircle.Center.X + circle.Center.X) / 2, (resultCircle.Center.Y + circle.Center.Y) / 2), (resultCircle.Radius + circle.Radius) / 2);
                        finded = true;
                    }
                }
            }

            if (finded == false)
            {
                if (resultCircle != null)
                {
                    int margin = 10;

                    circleOffset = new Point((int)(resultCircle.Center.X - resultCircle.Radius - margin), (int)(resultCircle.Center.Y - resultCircle.Radius - margin));
                    var size = new Size((int)((resultCircle.Radius + margin) * 2), (int)((resultCircle.Radius + margin) * 2));

                    if (circleOffset.X < 0)
                    {
                        circleOffset.X = 0;
                    }

                    if (circleOffset.Y < 0)
                    {
                        circleOffset.Y = 0;
                    }

                    if (circleOffset.X + size.Width > probeClipImage.Width)
                    {
                        size.Width -= ((circleOffset.X + size.Width) - probeClipImage.Width);
                    }

                    if (circleOffset.Y + size.Height > probeClipImage.Height)
                    {
                        size.Height -= ((circleOffset.Y + size.Height) - probeClipImage.Height);
                    }

                    roiImage = probeClipImage.Clip(new Rectangle(circleOffset, size));
                }
                else
                {
                    roiImage = probeClipImage.Clone();
                }

                resultCircle = getHoughCircle(roiImage, circleOffset, resultCircle, debugContext);
            }

            return resultCircle;
        }

        private List<CircleEq> getMinEnclosingCircle(AlgoImage roiImage, DebugContext debugContext)
        {
            var openCvImage = roiImage as OpenCvGreyImage;

            var hierachy = new Mat();
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(openCvImage.Image, contours, hierachy, RetrType.Tree, ChainApproxMethod.ChainApproxNone);

            if (contours == null)
            {
                return null;
            }

            var circles = new List<CircleEq>();

            for (int i = 0; i < contours.Size; i++)
            {
                CircleF circle = Emgu.CV.CvInvoke.MinEnclosingCircle(contours[i]);

                if (circle.Radius > Param.OutterRadius || circle.Radius < Param.InnerRadius)
                {
                    continue;
                }

                circles.Add(new CircleEq(circle.Center, circle.Radius));
            }

            roiImage.Save("Circle.bmp", debugContext);

            return circles;
        }

        private CircleEq getHoughCircle(AlgoImage roiImage, Point offset, CircleEq resultCircle, DebugContext debugContext)
        {
            var openCvImage = roiImage as OpenCvGreyImage;

            CircleF[][] Circles = openCvImage.Image.HoughCircles(new Gray(200), new Gray(Param.Accumulate), 1, (int)Param.CenterDistance, (int)Param.InnerRadius, (int)Param.OutterRadius);

            var findedCircles = new List<CircleF>();

            var subCenters = new List<PointF>();
            var subRadius = new List<float>();
            var subCircleNum = new List<int>();

            if (resultCircle != null)
            {
                subCircleNum.Add(1);

                subCenters.Add(resultCircle.Center);
                subRadius.Add(resultCircle.Radius);
            }

            foreach (CircleF circle in Circles[0])
            {
                bool selected = false;
                var selectCenter = new PointF();
                float prevDistance = Param.CenterDistance;

                foreach (PointF center in subCenters)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(circle.Center.X - center.X, 2) + Math.Pow(circle.Center.Y - center.Y, 2));

                    if (distance < prevDistance)
                    {
                        prevDistance = distance;
                        selectCenter = center;
                        selected = true;
                    }
                }

                if (selected == false)
                {
                    subCircleNum.Add(1);

                    subCenters.Add(circle.Center);
                    subRadius.Add(circle.Radius);
                }
                else
                {
                    int index = subCenters.IndexOf(selectCenter);

                    subCenters[index] = new PointF(((subCenters[index].X * subCircleNum[index]) + circle.Center.X) / (subCircleNum[index] + 1),
                                                    ((subCenters[index].Y * subCircleNum[index]) + circle.Center.Y) / (subCircleNum[index] + 1));
                    subRadius[index] = ((subRadius[index] * subCircleNum[index]) + circle.Radius) / (subCircleNum[index] + 1);

                    subCircleNum[index]++;
                }
            }

            if (subCircleNum.Count == 0)
            {
                return null;
            }

            int resultIndex = 0;
            int prevCircleNum = 0;

            for (int index = 0; index < subCircleNum.Count; index++)
            {
                if (subCircleNum[index] > prevCircleNum)
                {
                    prevCircleNum = subCircleNum[index];
                    resultIndex = index;
                }
            }

            if (prevCircleNum < 5)
            {
                return null;
            }

            var resultCenter = new PointF(subCenters[resultIndex].X + offset.X, subCenters[resultIndex].Y + offset.Y);

            return new CircleEq(resultCenter, subRadius[resultIndex]);
        }
    }
}