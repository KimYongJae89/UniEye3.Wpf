using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.Planbss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public enum Judgment
    {
        OK, NG, Overkill, Skip
    }

    public interface Searchable
    {
        bool UseWholeImage { get; }
        Size GetSearchRangeSize();
        void SetSearchRangeSize(Size size);
    }

    public abstract class ResultValueItem
    {
        public abstract string GetValueString();
    }

    public enum InspectLevel
    {
        Low, Medium, High
    }

    public class AlgorithmInspectParam
    {
        public List<ImageD> InspectImageList { get; set; }
        public Image3D InspectImage3d { get; set; }
        public RotatedRect ProbeRegionInFov { get; set; }
        public RotatedRect InspectRegionInFov { get; set; }
        public DebugContext DebugContext { get; set; }
        public Calibration CameraCalibration { get; set; }
        public Size CameraImageSize { get; set; }
        public InspectLevel InspectLevel { get; set; }

        public AlgorithmInspectParam(List<ImageD> inspectImageList, Size cameraImageSize, RotatedRect probeRegionInFov, RotatedRect inspectRegionInFov, Calibration calibration, DebugContext debugContext)
        {
            InspectImageList = inspectImageList;
            CameraImageSize = cameraImageSize;
            ProbeRegionInFov = probeRegionInFov;
            InspectRegionInFov = inspectRegionInFov;
            CameraCalibration = calibration;
            DebugContext = debugContext;
        }
    }

    public class AlgorithmConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(Algorithm))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(string) &&
                 value is Algorithm)
            {
                var algorithm = (Algorithm)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public abstract class AlgorithmParam
    {
        public ImageType SourceImageType { get; set; }
        public ImageBandType ImageBand { get; set; } = ImageBandType.Luminance;

        public abstract AlgorithmParam Clone();

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

        public virtual void Copy(AlgorithmParam srcAlgorithmParam)
        {
            SourceImageType = srcAlgorithmParam.SourceImageType;
            ImageBand = srcAlgorithmParam.ImageBand;
        }

        public virtual void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            Copy(srcAlgorithmParam);
        }

        public virtual void LoadParam(XmlElement algorithmElement)
        {
            SourceImageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(algorithmElement, "SourceImageType", "Grey"));
            ImageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), XmlHelper.GetValue(algorithmElement, "ImageBand", "Luminance"));
        }

        public virtual void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "SourceImageType", SourceImageType.ToString());
            XmlHelper.SetValue(algorithmElement, "ImageBand", ImageBand.ToString());
        }
    }

    [TypeConverterAttribute(typeof(AlgorithmConverter))]
    public abstract class Algorithm
    {
        [BrowsableAttribute(false)]
        public string AlgorithmName { get; set; } = "";
        [BrowsableAttribute(false)]
        public bool IsAlgorithmPoolItem { get; set; }
        public List<Algorithm> SubAlgorithmList { get; } = new List<Algorithm>();
        [BrowsableAttribute(false)]
        public int[] LightTypeIndexArr { get; } = null;

        protected bool isColorAlgorithm = false;
        [BrowsableAttribute(false)]
        public bool IsColorAlgorithm => isColorAlgorithm;

        protected AlgorithmParam param = null;
        public AlgorithmParam Param
        {
            get => param;
            set => param = value;
        }

        [BrowsableAttribute(false)]
        private List<IFilter> filterList = new List<IFilter>();
        public List<IFilter> FilterList
        {
            get => filterList;
            set => filterList = value;
        }
        public bool Enabled { get; set; } = true;
        public bool UseCustomFilter { get; set; } = true;

        public abstract Algorithm Clone();

        /// <summary>
        /// 알고리즘 초기화시 lightTypeIndex의 크기를 지정하고, 조명 index의 초기값을 지정한다.
        /// </summary>
        /// <param name="numLightTypeIndex"></param>
        public Algorithm(int numLightTypeIndex = 1)
        {
            LightTypeIndexArr = new int[numLightTypeIndex];
            LightTypeIndexArr[0] = 0;
        }

        public virtual void Clear()
        {

        }

        public virtual void SyncParam(Algorithm srcAlgorithm)
        {
            if (srcAlgorithm.Equals(this))
            {
                return;
            }

            isColorAlgorithm = srcAlgorithm.isColorAlgorithm;
            param.SyncParam(srcAlgorithm.Param);

            if (srcAlgorithm.FilterList?.Count > 0)
            {
                filterList = new List<IFilter>();
                foreach (IFilter filter in srcAlgorithm.FilterList)
                {
                    filterList.Add(filter.Clone());
                }
                //this.filterList = srcAlgorithm.FilterList;
            }

            Enabled = srcAlgorithm.Enabled;
        }

        public virtual object GetParamValue(string paramName)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(paramName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }

            if (param != null)
            {
                return param.GetParamValue(paramName);
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

            if (param != null)
            {
                return param.SetParamValue(paramName, value);
            }

            return false;
        }

        public virtual void Copy(Algorithm algorithm)
        {
            isColorAlgorithm = algorithm.isColorAlgorithm;
            param.Copy(algorithm.Param);

            filterList.Clear();

            foreach (IFilter filter in algorithm.FilterList) // 도는데용 ㅠㅠ
            {
                filterList.Add(filter.Clone());
            }

            Enabled = algorithm.Enabled;
        }

        protected void CopyGraphics(AlgorithmResult algorithmResult, HelpDraw helpDraw, int offsetX, int offsetY)
        {
            long countRect = helpDraw.SizeRect();
            for (int i = 0; i < countRect; i++)
            {
                DrawRect drRect = helpDraw.GetRect(i);

                Rectangle Area = drRect.mrArea;
                var rect = new Rectangle(offsetX + Area.Left, offsetY + Area.Top, Area.Width, Area.Height);
                if (drRect.mbCircle)
                {
                    algorithmResult.ResultFigures.AddFigure(new EllipseFigure(rect, drRect.gsColor));
                }
                else
                {
                    algorithmResult.ResultFigures.AddFigure(new RectangleFigure(rect, drRect.gsColor));
                }
            }

            long countCross = helpDraw.SizeCross();
            for (int i = 0; i < countCross; i++)
            {
                DrawCross drCross = helpDraw.GetCross(i);

                var pt = new Point(drCross.mpt.X + offsetX, drCross.mpt.Y + offsetY);
                var crossFigure = new CrossFigure(pt, drCross.mnLength, drCross.gsColor);
                algorithmResult.ResultFigures.AddFigure(crossFigure);
            }

            long countLine = helpDraw.SizeLine();
            for (int i = 0; i < countLine; i++)
            {
                DrawLine drLine = helpDraw.GetLine(i);

                var pt1 = new Point(drLine.mpt1.X + offsetX, drLine.mpt1.Y + offsetY);
                var pt2 = new Point(drLine.mpt2.X + offsetX, drLine.mpt2.Y + offsetY);
                var lineFigure = new LineFigure(pt1, pt2, drLine.gsColor);
                algorithmResult.ResultFigures.AddFigure(lineFigure);
            }
        }

        public virtual string[] GetPreviewNames()
        {
            return new string[] { "Default" };
        }

        public virtual ImageD Filter(ImageD image, int previewFilterType, int targetLightTypeIndex = -1)
        {
            if (image is Image3D)
            {
                return image;
            }

            Debug.Assert(image != null);

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey, param.ImageBand);
            Filter(algoImage, previewFilterType, targetLightTypeIndex);

            var fillterredImage = (Image2D)algoImage.ToImageD();
            return fillterredImage;
        }

        public virtual ImageD PreFilterClipImage(ImageD filteredImage, Rectangle clipRegion, int previewFilterType)
        {
            ImageD clipImage = filteredImage.ClipImage(clipRegion);

            return clipImage;
        }

        public virtual ImageD PostFilterCopyForm(ImageD filteredImage, ImageD clipFilteredImage, Rectangle clipRegion, int previewFilterType)
        {
            filteredImage.CopyFrom(clipFilteredImage, new Rectangle(0, 0, clipRegion.Width, clipRegion.Height), clipFilteredImage.Pitch, new Point(clipRegion.X, clipRegion.Y));

            return filteredImage;
        }

        /// <summary>
        /// 1. 단일 필터링 Sequence
        /// 단일 조명을 사용하고, 하나의 필터링 이미지를 사용하는 경우는 알고리즘에 추가된 필터의 순서대로,
        /// 이미지를 필터링하여 결과를 반환한다. FilterList 사용에 문제가 없다. 다만, 알고리즘 사용시 특정 필터가 반드시 필요한 경우,
        /// 하위 알고리즘 클래스에서 인스턴스 생성시 해당 필터의 인스턴스도 같이 생성해 주고, 해당 필터의 EssentialFilter 값을 true로 설정해 준다.
        /// 예를 들어 BinaryCounter의 경우, BinarizeFilter가 반드시 필요하기 때문에, 이 알고리즘의 경우, 인스턴스 생성시  BinarizeFilter를 FilterList에 추가해 주고
        /// EssentialFilter를 true로 설정해 주어야 한다.
        /// 2. 다중 필터링 Sequence - 단일 조명
        /// 단일 조명을 사용하나, 필터링 단계별로 획득된 이미지를 검사에 사용할 경우,
        /// 다양한 필터링 결과를 화면에 표시해 주어야 한다.
        /// 이 경우, previewFilterType에 설정된 필터 인덱스에 맞는 필터링 결과 이미지를 생성한 결과를 반환해 주어야 한다.
        /// 하위 알고리즘 클래스는 동일한 필터링 결과를 생성할 수 있도록, 인스턴스 생성시 FilterList에 필터 인스턴스를 동시에 
        /// 생성해 주거나, 알고리즘내에 필터링 코드를 추가해 주어야 한다.
        /// 코드 상의 혼란을 줄이기 위해 후자를 추천하고, 동일한 이유로 useCustomFilter를 true로 설정하여 FilterParamTab을 Hide하도록 한다.
        /// 다음 함수를 반드시 override해 주어야 한다.
        /// - Filter(AlgoImage algoImage, int previewFilterType, int targetLightTypeIndex=-1)
        /// - string[] GetPreviewNames()
        /// 3. 다중 필터링 Sequence - 다중 조명
        /// </summary>
        /// <param name="algoImage"></param>
        /// <param name="previewFilterType"></param>
        /// <param name="targetLightTypeIndex"></param>
        public virtual void Filter(AlgoImage algoImage, int previewFilterType = 0, int targetLightTypeIndex = -1)
        {
            //algoImage.Save(@"d:\preFilter.bmp", null);

            AlgoImage srcAlgoImage = algoImage.Clone();
            foreach (IFilter filter in filterList)
            {
                filter.Filter(algoImage);
            }
        }

        public virtual void PrepareInspection()
        {

        }

        public virtual bool CanProcess3dImage()
        {
            return false;
        }

        public virtual void LoadParam(XmlElement algorithmElement)
        {
            string algorithmType = XmlHelper.GetValue(algorithmElement, "AlgorithmType", ""); ;

            Enabled = AlgorithmFactory.Instance().IsAlgorithmEnabled(algorithmType);
            AlgorithmName = XmlHelper.GetValue(algorithmElement, "AlgorithmName", "");

            LoadFilterList(algorithmElement);
            LoadLightType(algorithmElement);
            param.LoadParam(algorithmElement);
        }

        private void LoadFilterList(XmlElement algorithmElement)
        {
            XmlElement filterListElement = algorithmElement["FilterList"];
            if (filterListElement != null)
            {
                filterList.Clear();

                foreach (XmlElement filterElement in filterListElement)
                {
                    if (filterElement.Name == "Filter")
                    {
                        var filterType = (FilterType)Enum.Parse(typeof(FilterType), XmlHelper.GetValue(filterElement, "FilterType", "EdgeExtraction"));

                        IFilter filter = FilterFactory.CreateFilter(filterType);
                        filter.LoadParam(filterElement);
                        filterList.Add(filter);
                    }
                }
            }
        }

        private void LoadLightType(XmlElement lightTypeElement)
        {
            int lightIdx = 0;
            foreach (XmlElement element in lightTypeElement)
            {
                if (element.Name == "LightTypeIndex")
                {
                    XmlElement childElement = element;
                    foreach (XmlElement element2 in childElement)
                    {
                        if (element2.Name == "Index")
                        {
                            int idx = Convert.ToInt32(element2.InnerText);
                            if (idx >= 0)
                            {
                                LightTypeIndexArr[lightIdx] = idx;
                            }
                        }
                    }

                    lightIdx++;
                }
            }
        }

        public virtual void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "AlgorithmName", AlgorithmName);
            XmlHelper.SetValue(algorithmElement, "AlgorithmType", GetAlgorithmType().ToString());

            XmlElement filterListElement = algorithmElement.OwnerDocument.CreateElement("FilterList");
            algorithmElement.AppendChild(filterListElement);

            foreach (IFilter filter in filterList)
            {
                XmlElement filterElement = filterListElement.OwnerDocument.CreateElement("Filter");
                filterListElement.AppendChild(filterElement);

                XmlHelper.SetValue(filterElement, "FilterType", filter.GetFilterType().ToString());
                filter.SaveParam(filterElement);
            }

            SaveLightType(algorithmElement);

            param.SaveParam(algorithmElement);
        }

        private void SaveLightType(XmlElement lightTypeElement)
        {
            XmlElement element = lightTypeElement.OwnerDocument.CreateElement("LightTypeIndex");
            lightTypeElement.AppendChild(element);

            foreach (int index in LightTypeIndexArr)
            {
                XmlHelper.SetValue(element, "Index", index.ToString());
            }
        }

        /// <summary>
        /// 검사 영역을 알고리즘에 설정된 파라미터를 기준으로 변경한다.
        /// 전체 이미지 설정 ( UseWholeImage )이 true로 설정되어 있을 경우는 이 함수를 호출하지 않는다.
        /// </summary>
        /// <param name="inspRegion"></param>
        /// <returns></returns>
        public virtual RotatedRect AdjustInspectRegion(RotatedRect inspRegion)
        {
            return inspRegion;
        }

        public virtual void BuildSelectedFigures(RotatedRect probeRect, FigureGroup tempFigures)
        {

        }

        public abstract string GetAlgorithmType();
        public abstract string GetAlgorithmTypeShort();

        public abstract void AppendResultMessage(Message resultMessage, AlgorithmResult algorithmResult);

        public abstract List<ResultValue> GetResultValues();
        public abstract AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam);
    }
}
