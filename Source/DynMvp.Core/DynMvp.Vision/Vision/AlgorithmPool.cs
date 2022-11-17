using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class AlgorithmPool
    {
        public List<Algorithm> AlgorithmList { get; } = new List<Algorithm>();

        private static AlgorithmPool _instance = null;

        public void Initialize()
        {
        }

        public void AddAlgorithm(Algorithm algorithm, string algorithmPoolName)
        {
            if (algorithmPoolName == string.Empty)
            {
                Debug.Assert(false, "Algorithm adding to algorithm pool must have the name.");
                return;
            }

            if (GetAlgorithm(algorithmPoolName) != null)
            {
                Debug.Assert(false, "The algorithm adding to algorithm pool is exist in algorithm pool.");
                return;
            }

            AlgorithmList.Add(algorithm);
            algorithm.AlgorithmName = algorithmPoolName;
            algorithm.IsAlgorithmPoolItem = true;
        }

        public Algorithm GetAlgorithm(string algorithmPoolName)
        {
            return AlgorithmList.Find(algorithm => algorithm.AlgorithmName == algorithmPoolName);
        }

        public void RemoveAlgorithm(Algorithm algorithm)
        {
            AlgorithmList.Remove(algorithm);
        }

        public static AlgorithmPool Instance()
        {
            if (_instance == null)
            {
                _instance = new AlgorithmPool();
            }

            return _instance;
        }

        public void Load(string fileName)
        {
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            try
            {
                if (xmlDocument == null)
                {
                    return;
                }

                XmlElement algorithmPoolElement = xmlDocument.DocumentElement;

                foreach (XmlElement algorithmElement in algorithmPoolElement)
                {
                    if (algorithmElement.Name == "Algorithm")
                    {
                        string algorithmName = XmlHelper.GetValue(algorithmElement, "AlgorithmName", "");
                        if (GetAlgorithm(algorithmName) == null)
                        {
                            string algorithmType = XmlHelper.GetValue(algorithmElement, "AlgorithmType", "");
                            Algorithm algorithm = AlgorithmFactory.Instance().CreateAlgorithm(algorithmType);
                            if (algorithm == null)
                            {
                                continue;
                            }

                            algorithm.LoadParam(algorithmElement);

                            AlgorithmList.Add(algorithm);
                            algorithm.IsAlgorithmPoolItem = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        public void Save(string fileName)
        {
            var xmlDocument = new XmlDocument();

            XmlElement algorithmPoolElement = xmlDocument.CreateElement("", "AlgorithmPool", "");
            xmlDocument.AppendChild(algorithmPoolElement);

            AlgorithmList.ForEach(algorithm =>
            {
                XmlElement algorithmElement = algorithmPoolElement.OwnerDocument.CreateElement("", "Algorithm", "");
                algorithmPoolElement.AppendChild(algorithmElement);

                algorithm.SaveParam(algorithmElement);
            });

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}
