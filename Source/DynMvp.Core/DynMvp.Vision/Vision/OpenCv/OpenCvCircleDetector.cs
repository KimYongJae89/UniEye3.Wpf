using DynMvp.Base;
using DynMvp.UI;
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

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvCircleDetector : CircleDetector
    {
        //public override CircleEq Detect(AlgoImage algoImage, DebugContext debugContext)
        //{
        //    OpenCvGreyImage openCvImage = algoImage as OpenCvGreyImage;

        //    CircleF[][] Circles = openCvImage.Image.HoughCircles(new Gray(200), new Gray(10), 1, (int)Param.OutterRadius, (int)Param.InnerRadius, (int)Param.OutterRadius);

        //    foreach (CircleF circle in Circles[0])
        //    {
        //        CircleEq circleEq = new CircleEq();

        //        circleEq.Center = new PointF((float)circle.Center.X, (float)circle.Center.Y);
        //        circleEq.Radius = (float)circle.Radius;

        //        return circleEq;
        //    }

        //    return null;
        //}

        public override CircleEq Detect(AlgoImage algoImage, DebugContext debugContext)
        {
            try
            {
                algoImage.Save("Input.bmp", debugContext);

                ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoImage);

                imageProcessing.Binarize(algoImage);

                imageProcessing.Dilate(algoImage, 1);
                imageProcessing.Erode(algoImage, 1);

                algoImage.Save("Processing.bmp", debugContext);

                var openCvImage = algoImage as OpenCvGreyImage;

                var hierachy = new Mat();
                var contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(openCvImage.Image, contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxNone);

                if (contours == null)
                {
                    return null;
                }

                for (int i = 0; i < contours.Size; i++)
                {
                    CircleF circle = CvInvoke.MinEnclosingCircle(contours[i]);

                    if (circle.Radius > Param.OutterRadius || circle.Radius < Param.InnerRadius)
                    {
                        continue;
                    }

                    return new CircleEq(circle.Center, circle.Radius);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OpenCvCircleDetector.Detect(AlgoImage algoImage, DebugContext debugContext)" + ex.InnerException.Message);

                ErrorManager.Instance().Report((int)ErrorSection.Process, (int)CommonError.FailToExecute, ErrorLevel.Fatal,
                    ErrorSection.Process.ToString(), CommonError.FailToExecute.ToString(), ex.InnerException.Message);
            }

            return null;
        }
    }
}
