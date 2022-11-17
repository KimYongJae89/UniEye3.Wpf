using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class ColorPattern
    {
        public string Name { get; set; } = "";
        public Image2D Image { get; set; } = null;
        public int MatchScore { get; set; }
        public int Smoothing { get; set; }

        private float imageWidth;

        public float ImageWidth
        {
            get => imageWidth;
            set => imageWidth = Image.Width;
        }

        private float imageHeight;

        public float ImageHeight
        {
            get => imageHeight;
            set => imageHeight = Image.Height;
        }

        public ColorPattern(string name, Image2D image)
        {
            Name = name;
            Image = image;
        }

        public ColorPattern(string name, string imageString)
        {
            Name = name;

            Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
            if (bitmap != null)
            {
                Image = Image2D.ToImage2D(bitmap);
                bitmap.Dispose();
            }
        }

        public string GetImageString()
        {
            if (Image == null)
            {
                return "";
            }

            var patternBitmap = Image.ToBitmap();
            string patternImageString = ImageHelper.BitmapToBase64String(patternBitmap);
            patternBitmap.Dispose();

            return patternImageString;
        }

        public void SetImageString(string imageString)
        {
            Bitmap patternBitmap = ImageHelper.Base64StringToBitmap(imageString);
            if (patternBitmap != null)
            {
                Image = Image2D.ToImage2D(patternBitmap);
                patternBitmap.Dispose();
            }
        }
    }

    public class ColorMatchCheckerParam : AlgorithmParam
    {
        public int MatchScore { get; set; }
        public string MatchColor { get; set; }
        public int Smoothing { get; set; }
        public string ColorPatternFileName { get; set; }
        public bool UseColorPatternFile { get; set; }
        public List<ColorPattern> ColorPatternList { get; set; } = new List<ColorPattern>();


        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgorithmParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            var param = (ColorMatchCheckerParam)srcAlgorithmParam;

            MatchScore = param.MatchScore;
            MatchColor = param.MatchColor;
            Smoothing = param.Smoothing;
            ColorPatternFileName = param.ColorPatternFileName;
            UseColorPatternFile = param.UseColorPatternFile;
            ColorPatternList = param.ColorPatternList;
        }

        public ColorPattern AddColorPattern(string name, Image2D image)
        {
            var colorPattern = new ColorPattern(name, image);
            colorPattern.Smoothing = Smoothing;
            if (colorPattern != null)
            {
                ColorPatternList.Add(colorPattern);
            }

            return colorPattern;
        }

        public void DeleteColorPattern(string name)
        {
            ColorPatternList.RemoveAll(x => x.Name == name);
        }
        public void RemoveAllColorPattern()
        {
            ColorPatternList.Clear();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            MatchScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MatchScore", ""));
            Smoothing = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Smoothing", ""));
            MatchColor = XmlHelper.GetValue(algorithmElement, "MatchColor", "");
            UseColorPatternFile = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseColorPatternFile", ""));
            ColorPatternFileName = XmlHelper.GetValue(algorithmElement, "ColorPatternFileName", "");

            if (UseColorPatternFile)
            {
                if (File.Exists(ColorPatternFileName))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(ColorPatternFileName);

                    LoadColorImage(xmlDocument.DocumentElement);
                }
            }
            else
            {
                LoadColorImage(algorithmElement);
            }
        }

        public void LoadColorImage(XmlElement colorPatternListElement)
        {
            foreach (XmlElement colorPatternElement in colorPatternListElement)
            {
                if (colorPatternElement.Name == "ColorPattern")
                {
                    string name = XmlHelper.GetValue(colorPatternElement, "Name", "");
                    string imageString = XmlHelper.GetValue(colorPatternElement, "Image", "");

                    if (name != "" && imageString != "")
                    {
                        var colorPattern = new ColorPattern(name, imageString);

                        ColorPatternList.Add(colorPattern);
                    }
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "UseColorPatternFile", UseColorPatternFile.ToString());
            XmlHelper.SetValue(algorithmElement, "ColorPatternFileName", ColorPatternFileName);
            XmlHelper.SetValue(algorithmElement, "MatchScore", MatchScore.ToString());
            XmlHelper.SetValue(algorithmElement, "Smoothing", Smoothing.ToString());
            XmlHelper.SetValue(algorithmElement, "MatchColor", MatchColor.ToString());
            XmlHelper.SetValue(algorithmElement, "ImageSetFileName", MatchColor.ToString());
            if (UseColorPatternFile == true)
            {
                var xmlDocument = new XmlDocument();

                XmlElement colorPatternListElement = xmlDocument.CreateElement("", "ColorPattern", "");
                xmlDocument.AppendChild(colorPatternListElement);

                SaveColorPattern(colorPatternListElement);

                xmlDocument.Save(ColorPatternFileName);
            }
            else
            {
                SaveColorPattern(algorithmElement);
            }
        }

        public void SaveColorPattern(XmlElement colorPatternListElement)
        {
            foreach (ColorPattern colorPattern in ColorPatternList)
            {
                XmlElement colorPatternElement = colorPatternListElement.OwnerDocument.CreateElement("", "ColorPattern", "");
                colorPatternListElement.AppendChild(colorPatternElement);

                XmlHelper.SetValue(colorPatternElement, "Name", colorPattern.Name);
                XmlHelper.SetValue(colorPatternElement, "Image", colorPattern.GetImageString());

                //colorPatternElement.Save(colorPatternFileName);
            }


        }
    }

    public abstract class ColorMatchChecker : Algorithm
    {
        public ColorMatchChecker()
        {
            param = new ColorMatchCheckerParam();
            isColorAlgorithm = true;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            var colorMatchChecker = (ColorMatchChecker)algorithm;

            param.Copy(colorMatchChecker.Param);
        }

        public const string TypeName = "ColorMatchChecker";

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "ColorMatch";
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            Train();
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);
        }

        public override List<ResultValue> GetResultValues()
        {
            var colorMatchCheckerParam = (ColorMatchCheckerParam)param;

            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Match Color", "", colorMatchCheckerParam.MatchColor));
            resultValues.Add(new ResultValue("Score", "", 100, colorMatchCheckerParam.MatchScore, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.InspectImageList[0], ImageType.Color);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect inspectRegionInFov = inspectParam.InspectRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(inspectRegionInFov, probeRegionInFov);

            var colorMatchCheckerParam = (ColorMatchCheckerParam)param;

            var colorMatchResult = new AlgorithmResult();

            if (Trained == true)
            {
                SubResultList subResultList = Match(clipImage, probeRegionInClip, debugContext);

                SubResult maxResult = subResultList.GetMaxResult();

                bool result = (maxResult.Value * 100 > colorMatchCheckerParam.MatchScore);

                bool colorFound = false;
                if (string.IsNullOrEmpty(colorMatchCheckerParam.MatchColor) == false)
                {
                    string[] colorNames = colorMatchCheckerParam.MatchColor.Split(';');
                    foreach (string colorName in colorNames)
                    {
                        if (maxResult.Name == colorName)
                        {
                            colorFound = true;
                            break;
                        }
                    }

                    result &= colorFound;
                }

                colorMatchResult.SetResult(result);
                colorMatchResult.AddResultValue(new ResultValue("Score", "", 100, colorMatchCheckerParam.MatchScore, maxResult.Value * 100));
                colorMatchResult.AddResultValue(new ResultValue("Match Color", "", null, colorMatchCheckerParam.MatchColor));
                colorMatchResult.AddResultValue(new ResultValue("SubResultList", "", null, subResultList));
            }
            else
            {
                colorMatchResult.BriefMessage = "Algorithm is not trained";
                colorMatchResult.SetResult(false);
            }

            return colorMatchResult;
        }

        public abstract bool Trained { get; }
        public abstract void Train();
        public abstract SubResultList Match(AlgoImage algoImage, RectangleF probeRegion, DebugContext debugContext);
    }
}
