using Cognex.VisionPro;
using Cognex.VisionPro.CompositeColorMatch;
using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Vision.Cognex
{
    public class CognexColorMatchChecker : ColorMatchChecker
    {
        private CogCompositeColorMatchPattern colorMatchPattern = new CogCompositeColorMatchPattern();

        public CognexColorMatchChecker()
        {

        }

        public override Algorithm Clone()
        {
            var CognexColorMatchChecker = new CognexColorMatchChecker();
            CognexColorMatchChecker.Copy(this);

            return CognexColorMatchChecker;
        }

        public override bool Trained => colorMatchPattern.Trained;

        public override void PrepareInspection()
        {
            Train();
        }

        public override void Train()
        {
            var colorMatchCheckerParam = (ColorMatchCheckerParam)param;

            colorMatchPattern.CompositeColorCollection.Clear();

            foreach (ColorPattern colorPattern in colorMatchCheckerParam.ColorPatternList)
            {
                var cogRectangle = new CogRectangle();

                cogRectangle.SetXYWidthHeight(0, 0, colorPattern.Image.Width, colorPattern.Image.Height);

                var item = new CogCompositeColorItem(CognexImageBuilder.ConvertColorImage(colorPattern.Image), cogRectangle);
                item.Name = colorPattern.Name;
                colorMatchPattern.GaussianSmoothing = colorPattern.Smoothing;
                colorMatchPattern.CompositeColorCollection.Add(item);
                try
                {
                    colorMatchPattern.Train();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public override SubResultList Match(AlgoImage algoImage, RectangleF probeRegion, DebugContext debugContext)
        {
            var result = new SubResultList();

            PointF centerPt = DrawingHelper.CenterPoint(probeRegion);

            var cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, probeRegion.Width, probeRegion.Height, 0, 0);

            var colorImage = (CognexColorImage)algoImage;
            colorImage.Save("colorMatch.bmp", debugContext);

            var runParams = new CogCompositeColorMatchRunParams();
            runParams.SortResultSetByScores = true;

            CogCompositeColorMatchResultSet matchResults = colorMatchPattern.Execute(colorImage.Image, cogRectangle, runParams);

            foreach (CogCompositeColorMatchResult matchResult in matchResults)
            {
                result.AddSubResult(new SubResult(matchResult.Color.Name, (float)matchResult.MatchScore));
            }

            return result;
        }

        public override void AppendResultMessage(DynMvp.UI.Message resultMessage, AlgorithmResult algorithmResult)
        {
            throw new NotImplementedException();
        }
    }
}
