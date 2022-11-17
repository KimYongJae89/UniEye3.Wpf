using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    [TypeConverterAttribute(typeof(BrightnessCheckerConverter))]
    public class MultiChecker : Algorithm
    {
        List<Algorithm> algorithmList = new List<Algorithm>();

        public void AddAlgorithm(Algorithm algorithm)
        {
            algorithm.Index = algorithmList.Count;
            algorithmList.Add(algorithm);
        }

        public void RemoveAlgorithm(Algorithm algorithmDel)
        {
            algorithmList.Remove(algorithmDel);

            int index = 0;
            foreach (Algorithm algorithm in algorithmList)
            {
                algorithm.Index = index++;
            }
        }

        public override Algorithm Clone()
        {
            MultiChecker multiChecker = new MultiChecker();
            multiChecker.Copy(this);

            return multiChecker;
        }

        public override void Copy(Algorithm algorithm)
        {
            base.Copy(algorithm);

            MultiChecker multiChecker = (MultiChecker)algorithm;

            foreach(Algorithm srcAlgorithm in multiChecker.algorithmList)
            {
                algorithmList.Add(srcAlgorithm.Clone());
            }
        }

        public static string TypeName
        {
            get { return "MultiChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Multi";
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            foreach (Algorithm algorithm in algorithmList)
            {
                algorithm.AppendAdditionalFigures(figureGroup, region);
            }
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            foreach (XmlElement subAlgorithmElement in algorithmElement)
            {
                if (subAlgorithmElement.Name == "SubAlgorithm")
                {
                    Algorithm algorithm;
                    AlgorithmArchiver.Instance.LoadAlgorithm(subAlgorithmElement, out algorithm);

                    algorithmList.Add(algorithm);
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            foreach (Algorithm algorithm in algorithmList)
            {
                XmlElement subAlgorithmElement = algorithmElement.OwnerDocument.CreateElement("SubAlgorithm");
                algorithmElement.AppendChild(subAlgorithmElement);

                algorithm.SaveParam(subAlgorithmElement);
            }
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            foreach (Algorithm algorithm in algorithmList)
            {
                algorithm.AdjustInspRegion(ref inspRegion, ref useWholeImage);
            }
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> wholeResultValues = new List<AlgorithmResultValue>();
            foreach (Algorithm algorithm in algorithmList)
            {
                List<AlgorithmResultValue> resultValues = algorithm.GetResultValues();
                foreach(AlgorithmResultValue resultValue in resultValues)
                {
                    resultValue.Name = string.Format("{0}-{1}", algorithm.Index, resultValue.Name);
                    wholeResultValues.Add(resultValue);
                }
            }

            return wholeResultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            MultiAlgorithmResult multiAlgorithmResult = new MultiAlgorithmResult();
            foreach (Algorithm algorithm in algorithmList)
            {
                AlgorithmResult algorithmResult = algorithm.Inspect(inspectParam);
                multiAlgorithmResult.Add(algorithmResult);

                List<AlgorithmResultValue> resultValues = algorithmResult.ResultValueList;
                foreach (AlgorithmResultValue resultValue in resultValues)
                {
                    resultValue.Name = string.Format("{0}-{1}", algorithm.Index, resultValue.Name);
                }
            }

            return multiAlgorithmResult;
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new MultiAlgorithmResult();
        }
    }

    public class MultiAlgorithmResult : AlgorithmResult
    {
        List<AlgorithmResult> algorithmResultList = new List<AlgorithmResult>();

        public void Add(AlgorithmResult algorithmResult)
        {
            algorithmResultList.Add(algorithmResult);
        }
    }
}
