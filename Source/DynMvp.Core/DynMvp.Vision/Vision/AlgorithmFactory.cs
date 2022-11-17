using DynMvp.Base;
using DynMvp.Vision.Cognex;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.Matrox;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Vision
{
    public class AlgorithmStrategy
    {
        public string AlgorithmType { get; set; }
        public ImagingLibrary LibraryType { get; set; }
        public ImageType ImageType { get; set; }
        public string SubLibraryType { get; set; }
        public bool Enabled { get; set; } = false;

        public AlgorithmStrategy(string algorithmType, ImagingLibrary libraryType, string subLibraryType = "", ImageType imageType = ImageType.Grey)
        {
            AlgorithmType = algorithmType;
            LibraryType = libraryType;
            ImageType = imageType;
            SubLibraryType = subLibraryType;
        }

        public override string ToString()
        {
            return AlgorithmType;
        }

        public override bool Equals(object obj)
        {
            var algorithmStrategy = obj as AlgorithmStrategy;
            if (algorithmStrategy == null)
            {
                return false;
            }

            return AlgorithmType == algorithmStrategy.AlgorithmType
                && LibraryType == algorithmStrategy.LibraryType
                && ImageType == algorithmStrategy.ImageType
                && SubLibraryType == algorithmStrategy.SubLibraryType;
        }

        public override int GetHashCode()
        {
            return AlgorithmType.GetHashCode();
        }
    }

    public class AlgorithmFactory
    {
        private static AlgorithmFactory instance = null;
        public static AlgorithmFactory Instance()
        {
            return instance;
        }

        public static void SetInstance(AlgorithmFactory algorithmFactory)
        {
            instance = algorithmFactory;
        }
        public int LicenseErrorCount { get; private set; } = 0;

        private List<AlgorithmStrategy> algorithmStrategyList = new List<AlgorithmStrategy>();

        public virtual void Setup(ImagingLibrary imagingLibrary)
        {
            LogHelper.Debug(LoggerType.StartUp, "Setup Algorithm Strategy Start");

            switch (imagingLibrary)
            {
                case ImagingLibrary.CognexVisionPro:
                    LogHelper.Debug(LoggerType.StartUp, "Initialize Vision Pro Algorithms");
                    AddStrategy(new AlgorithmStrategy(PatternMatching.TypeName, ImagingLibrary.CognexVisionPro, "VxPatMax|VxPatQuick"));
                    AddStrategy(new AlgorithmStrategy(WidthChecker.TypeName, ImagingLibrary.CognexVisionPro, "VxDimensioning"));
                    AddStrategy(new AlgorithmStrategy(BarcodeReader.TypeName, ImagingLibrary.CognexVisionPro, "VxSymbol"));
                    AddStrategy(new AlgorithmStrategy(CharReader.TypeName, ImagingLibrary.CognexVisionPro, "VxOCR"));
                    AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(LineChecker.TypeName, ImagingLibrary.CognexVisionPro, "VxDimensioning"));
                    break;
                case ImagingLibrary.MatroxMIL:
                    LogHelper.Debug(LoggerType.StartUp, "Initialize MIL Algorithms");
                    AddStrategy(new AlgorithmStrategy(PatternMatching.TypeName, ImagingLibrary.MatroxMIL, "PAT"));
                    AddStrategy(new AlgorithmStrategy(WidthChecker.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AddStrategy(new AlgorithmStrategy(EdgeDetector.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AddStrategy(new AlgorithmStrategy(CircleDetector.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.MatroxMIL, "CAL"));
                    AddStrategy(new AlgorithmStrategy(BlobChecker.TypeName, ImagingLibrary.MatroxMIL, "BLOB"));
                    AddStrategy(new AlgorithmStrategy(LineDetector.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AddStrategy(new AlgorithmStrategy(BarcodeReader.TypeName, ImagingLibrary.MatroxMIL, "CODE"));
                    AddStrategy(new AlgorithmStrategy(CharReader.TypeName, ImagingLibrary.MatroxMIL, "OCR"));
                    AddStrategy(new AlgorithmStrategy(LineChecker.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AddStrategy(new AlgorithmStrategy(EdgeChecker.TypeName, ImagingLibrary.MatroxMIL, "MEAS"));
                    AddStrategy(new AlgorithmStrategy(BlobSubtractor.TypeName, ImagingLibrary.MatroxMIL, "BLOB"));
                    AddStrategy(new AlgorithmStrategy(GridSubtractor.TypeName, ImagingLibrary.MatroxMIL, "BLOB"));
                    break;
                case ImagingLibrary.OpenCv:
                    LogHelper.Debug(LoggerType.StartUp, "Initialize OpenCV Algorithms");
                    AddStrategy(new AlgorithmStrategy(PatternMatching.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(WidthChecker.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(EdgeDetector.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(CircleDetector.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(BlobChecker.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(LineDetector.TypeName, ImagingLibrary.OpenCv, ""));
                    AddStrategy(new AlgorithmStrategy(EdgeChecker.TypeName, ImagingLibrary.OpenCv, ""));
                    break;
            }

            AddStrategy(new AlgorithmStrategy(BinaryCounter.TypeName, ImagingLibrary.OpenCv, ""));
            AddStrategy(new AlgorithmStrategy(BrightnessChecker.TypeName, ImagingLibrary.OpenCv, ""));
            AddStrategy(new AlgorithmStrategy(RectChecker.TypeName, ImagingLibrary.OpenCv, ""));
            AddStrategy(new AlgorithmStrategy(CircleChecker.TypeName, ImagingLibrary.OpenCv, ""));
            AddStrategy(new AlgorithmStrategy(CalibrationChecker.TypeName, ImagingLibrary.OpenCv, ""));
            AddStrategy(new AlgorithmStrategy(DepthChecker.TypeName, ImagingLibrary.OpenCv, ""));
            AddStrategy(new AlgorithmStrategy(ColorChecker.TypeName, ImagingLibrary.OpenCv, ""));

            if (IsUseMatroxMil())
            {
                MatroxHelper.InitApplication();
            }

            SetAlgorithmEnabled(PatternMatching.TypeName, true);

            LogHelper.Debug(LoggerType.StartUp, "Setup Algorithm Strategy End");
        }


        public virtual Algorithm CreateAlgorithm(string algorithmType)
        {
            switch (algorithmType)
            {
                case PatternMatching.TypeName: return new PatternMatching();
                case BinaryCounter.TypeName: return new BinaryCounter();
                case BrightnessChecker.TypeName: return new BrightnessChecker();
                case WidthChecker.TypeName: return new WidthChecker();
                case EdgeChecker.TypeName: return new EdgeChecker();
                case LineChecker.TypeName: return new LineChecker();
                case CircleChecker.TypeName: return new CircleChecker();
                case BlobChecker.TypeName: return new BlobChecker();
                case BarcodeReader.TypeName: return CreateBarcodeReader();
                case CharReader.TypeName: return CreateCharReader();
                case ColorChecker.TypeName: return new ColorChecker();
                case ColorMatchChecker.TypeName: return CreateColorMatchChecker();
                case RectChecker.TypeName: return new RectChecker();
                case CalibrationChecker.TypeName: return new CalibrationChecker();
                case DepthChecker.TypeName: return new DepthChecker();
                case BlobSubtractor.TypeName: return new BlobSubtractor();
                case GridSubtractor.TypeName: return new GridSubtractor();
            }

            return null;
        }

        public void ClearStrategyList()
        {
            algorithmStrategyList.Clear();
        }

        public void AddStrategy(AlgorithmStrategy strategy)
        {
            AlgorithmStrategy algorithmStrategy = GetStrategy(strategy.AlgorithmType);
            if (algorithmStrategy != null)
            {
                algorithmStrategyList.Remove(algorithmStrategy);
            }

            algorithmStrategyList.Add(strategy);
            LogHelper.Debug(LoggerType.StartUp, string.Format("Algorithm Strategy: {0} added.", strategy.AlgorithmType));
        }

        public bool IsUseMatroxMil()
        {
            if (instance == null)
            {
                return false;
            }

            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                if (algorithmStrategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsUseCognexVisionPro()
        {
            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                if (algorithmStrategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    return true;
                }
            }

            return false;
        }

        public void InitStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Init Algorithm Strategy");

            if (File.Exists(strategyFileName) == true)
            {
                LoadStrategy(strategyFileName);
            }
            else
            {
                SaveStrategy(strategyFileName);
            }
        }

        private void LoadStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Algorithm Strategy");

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(strategyFileName);

            XmlElement strategyListElement = xmlDocument.DocumentElement;
            foreach (XmlElement strategyElement in strategyListElement)
            {
                if (strategyElement.Name == "AlgorithmStrategy")
                {
                    string algorithmType = XmlHelper.GetValue(strategyElement, "AlgorithmType", "PatternMatching");
                    AlgorithmStrategy algorithmStrategy = GetStrategy(algorithmType);
                    if (algorithmStrategy != null)
                    {
                        algorithmStrategy.LibraryType = (ImagingLibrary)Enum.Parse(typeof(ImagingLibrary), XmlHelper.GetValue(strategyElement, "LibraryType", "OpenCv"));
                        algorithmStrategy.SubLibraryType = XmlHelper.GetValue(strategyElement, "SubLibraryType", "");
                    }
                }
            }
        }

        public void SaveStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Algorithm Strategy");

            var xmlDocument = new XmlDocument();

            XmlElement strategyListElement = xmlDocument.CreateElement("", "AlgorithmStrategyList", "");
            xmlDocument.AppendChild(strategyListElement);

            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                XmlElement strategyElement = xmlDocument.CreateElement("", "AlgorithmStrategy", "");
                strategyListElement.AppendChild(strategyElement);

                XmlHelper.SetValue(strategyElement, "AlgorithmType", algorithmStrategy.AlgorithmType);
                XmlHelper.SetValue(strategyElement, "LibraryType", algorithmStrategy.LibraryType.ToString());
                XmlHelper.SetValue(strategyElement, "SubLibraryType", algorithmStrategy.SubLibraryType);
            }

            xmlDocument.Save(strategyFileName);
        }

        public bool IsAlgorithmEnabled(string algorithmType)
        {
            AlgorithmStrategy findedStrategy = GetStrategy(algorithmType);
            if (findedStrategy != null)
            {
                return findedStrategy.Enabled;
            }

            return false;
        }

        public void SetAlgorithmEnabled(string algorithmType, bool enabled)
        {
            AlgorithmStrategy findedStrategy = GetStrategy(algorithmType);
            if (findedStrategy != null)
            {
                findedStrategy.Enabled = enabled;
                LogHelper.Debug(LoggerType.StartUp, string.Format("Algorithm Strategy: {0} is {1}", findedStrategy.AlgorithmType, findedStrategy.Enabled));
            }
            else
            {
                LogHelper.Error(string.Format("Algorithm License Error : {0}", algorithmType));
                LicenseErrorCount++;
            }
        }

        public AlgorithmStrategy GetStrategy(string algorithmType)
        {
            AlgorithmStrategy findedStrategy = algorithmStrategyList.Find(delegate (AlgorithmStrategy strategy) { return strategy.AlgorithmType == algorithmType; });
            if (findedStrategy != null)
            {
                if (LicenseManager.LicenseExist(findedStrategy.LibraryType, findedStrategy.SubLibraryType) == true)
                {
                    return findedStrategy;
                }
            }

            return null;
        }

        public Pattern CreatePattern()
        {
            AlgorithmStrategy strategy = GetStrategy(PatternMatching.TypeName);

            if (strategy != null)
            {
                //if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                //    return new CognexPattern(strategy.SubLibraryType);
                if (strategy.LibraryType == ImagingLibrary.EuresysOpenEVision)
                {
                    return new OpenEVisionPattern();
                }
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilPattern();
                }
            }

            return new OpenCvPattern();
        }

        public LineDetector CreateLineDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(LineDetector.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    return new CognexLineDetector();
                }
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilLineDetector();
                }
            }

            return new OpenCvLineDetector();
        }

        public EdgeDetector CreateEdgeDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(EdgeDetector.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    return new CognexEdgeDetector();
                }
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilEdgeDetector();
                }
            }

            return new OpenCvEdgeDetector();
        }

        public CharReader CreateCharReader()
        {
            AlgorithmStrategy strategy = GetStrategy(CharReader.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    return new CognexCharReader();
                }
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilCharReader();
                }
            }

            return new OpenCvCharReader();
        }

        public BarcodeReader CreateBarcodeReader()
        {
            AlgorithmStrategy strategy = GetStrategy(BarcodeReader.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    return new CognexBarcodeReader();
                }
                else if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilBarcodeReader();
                }
            }

            return new OpenCvBarcodeReader();
        }

        public ColorMatchChecker CreateColorMatchChecker()
        {
            AlgorithmStrategy strategy = GetStrategy(ColorMatchChecker.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    return new CognexColorMatchChecker();
                }
            }

            return new OpenCvColorMatchChecker();
        }

        public CircleDetector CreateCircleDetector()
        {
            AlgorithmStrategy strategy = GetStrategy(CircleDetector.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilCircleDetector();
                }
            }

            return new OpenCvCircleDetector();
        }

        public Calibration CreateCalibration()
        {
            AlgorithmStrategy strategy = GetStrategy(Calibration.TypeName);

            if (strategy != null)
            {
                if (strategy.LibraryType == ImagingLibrary.MatroxMIL)
                {
                    return new MilCalibration();
                }
            }

            return new OpenCvCalibration();
            return null;
        }
    }
}
