using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;
using UniScanC.Algorithm.Base;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Models
{
    public class VisionModel : Observable
    {
        #region 생성자
        public VisionModel() { }

        public VisionModel(VisionModel visionModel)
        {
            if (visionModel != null)
            {
                CopyFrom(visionModel);
            }
        }
        #endregion


        #region 속성
        private bool useAutoLight = false;
        public bool UseAutoLight
        {
            get => useAutoLight;
            set => Set(ref useAutoLight, value);
        }

        private ELightFittingType lightFittingType = ELightFittingType.Linear;
        public ELightFittingType LightFittingType
        {
            get => lightFittingType;
            set => Set(ref lightFittingType, value);
        }


        private int topLightValue = 0;
        public int TopLightValue
        {
            get => topLightValue;
            set => Set(ref topLightValue, value);
        }

        private int bottomLightValue = 128;
        public int BottomLightValue
        {
            get => bottomLightValue;
            set => Set(ref bottomLightValue, value);
        }


        private bool useTDILight = false;
        public bool UseTDILight
        {
            get => useTDILight;
            set => Set(ref useTDILight, value);
        }

        private float minSpeed = 0.0f;
        public float MinSpeed
        {
            get => minSpeed;
            set => Set(ref minSpeed, value);
        }

        private float maxSpeed = 0.0f;
        public float MaxSpeed
        {
            get => maxSpeed;
            set => Set(ref maxSpeed, value);
        }

        private int minSpeedTopLightValue = 128;
        public int MinSpeedTopLightValue
        {
            get => minSpeedTopLightValue;
            set => Set(ref minSpeedTopLightValue, value);
        }

        private int maxSpeedTopLightValue = 128;
        public int MaxSpeedTopLightValue
        {
            get => maxSpeedTopLightValue;
            set => Set(ref maxSpeedTopLightValue, value);
        }

        private int minSpeedBottomLightValue = 128;
        public int MinSpeedBottomLightValue
        {
            get => minSpeedBottomLightValue;
            set => Set(ref minSpeedBottomLightValue, value);
        }

        private int maxSpeedBottomLightValue = 128;
        public int MaxSpeedBottomLightValue
        {
            get => maxSpeedBottomLightValue;
            set => Set(ref maxSpeedBottomLightValue, value);
        }

        private int thresholdDark = 20;
        public int ThresholdDark
        {
            get => thresholdDark;
            set => Set(ref thresholdDark, value);
        }

        private int thresholdLight = 20;
        public int ThresholdLight
        {
            get => thresholdLight;
            set => Set(ref thresholdLight, value);
        }

        private int thresholdSize = 100;
        public int ThresholdSize
        {
            get => thresholdSize;
            set => Set(ref thresholdSize, value);
        }

        private float thresholdColor = 20;
        public float ThresholdColor
        {
            get => thresholdColor;
            set => Set(ref thresholdColor, value);
        }

        private int colorAverageCount = 2;
        public int ColorAverageCount
        {
            get => colorAverageCount;
            set => Set(ref colorAverageCount, value);
        }


        private float frameMarginL = 2;
        public float FrameMarginL
        {
            get => frameMarginL;
            set => Set(ref frameMarginL, value);
        }

        private float frameMarginT = 0;
        public float FrameMarginT
        {
            get => frameMarginT;
            set => Set(ref frameMarginT, value);
        }

        private float frameMarginR = 2;
        public float FrameMarginR
        {
            get => frameMarginR;
            set => Set(ref frameMarginR, value);
        }

        private float frameMarginB = 0;
        public float FrameMarginB
        {
            get => frameMarginB;
            set => Set(ref frameMarginB, value);
        }


        private double patternWidth = 250;
        public double PatternWidth
        {
            get => patternWidth;
            set => Set(ref patternWidth, value);
        }

        private double patternHeight = 250;
        public double PatternHeight
        {
            get => patternHeight;
            set => Set(ref patternHeight, value);
        }

        private double patternMarginX = 1;
        public double PatternMarginX
        {
            get => patternMarginX;
            set => Set(ref patternMarginX, value);
        }

        private double patternMarginY = 1;
        public double PatternMarginY
        {
            get => patternMarginY;
            set => Set(ref patternMarginY, value);
        }


        private bool skipFirstImage = false;
        public bool SkipFirstImage
        {
            get => skipFirstImage;
            set => Set(ref skipFirstImage, value);
        }

        private bool gpuProcessing = false;
        public bool GpuProcessing
        {
            get => gpuProcessing;
            set => Set(ref gpuProcessing, value);
        }

        private int targetIntensity = 128;
        public int TargetIntensity
        {
            get => targetIntensity;
            set => Set(ref targetIntensity, value);
        }

        private int outTargetIntensity = 255;
        public int OutTargetIntensity
        {
            get => outTargetIntensity;
            set => Set(ref outTargetIntensity, value);
        }

        private int calibrateFrameCnt = 2;
        public int CalibrateFrameCnt
        {
            get => calibrateFrameCnt;
            set => Set(ref calibrateFrameCnt, value);
        }

        private int maxDegreeOfParallelism = 5;
        public int MaxDegreeOfParallelism
        {
            get => maxDegreeOfParallelism;
            set => Set(ref maxDegreeOfParallelism, value);
        }

        private EDefectPriority defectPriority = EDefectPriority.Big;
        public EDefectPriority DefectPriority
        {
            get => defectPriority;
            set => Set(ref defectPriority, value);
        }

        private int maxDefectCount = 20;
        public int MaxDefectCount
        {
            get => maxDefectCount;
            set => Set(ref maxDefectCount, value);
        }

        private int inflateSize = 50;
        public int InflateSize
        {
            get => inflateSize;
            set => Set(ref inflateSize, value);
        }

        private bool useOtherCategory = true;
        public bool UseOtherCategory
        {
            get => useOtherCategory;
            set => Set(ref useOtherCategory, value);
        }


        private List<DefectCategory> defectCategories = new List<DefectCategory>();
        public List<DefectCategory> DefectCategories
        {
            get => defectCategories;
            set => Set(ref defectCategories, value);
        }

        private List<INodeParam> nodeParams = new List<INodeParam>();
        public List<INodeParam> NodeParams
        {
            get => nodeParams;
            set => Set(ref nodeParams, value);
        }
        #endregion


        #region 메서드
        public VisionModel Clone()
        {
            var model = new VisionModel();
            model.CopyFrom(this);

            return model;
        }

        public void CopyFrom(VisionModel visionModel)
        {
            CopyParametersFrom(visionModel);
            CopyDefectCategoriesFrom(visionModel);
            CopyNodeParamsFrom(visionModel);
        }

        public void CopyParametersFrom(VisionModel visionModel)
        {
            UseAutoLight = visionModel.UseAutoLight;
            LightFittingType = visionModel.lightFittingType;

            TopLightValue = visionModel.TopLightValue;
            BottomLightValue = visionModel.BottomLightValue;

            UseTDILight = visionModel.UseTDILight;
            MinSpeed = visionModel.MinSpeed;
            MaxSpeed = visionModel.MaxSpeed;
            MinSpeedTopLightValue = visionModel.MinSpeedTopLightValue;
            MaxSpeedTopLightValue = visionModel.MaxSpeedTopLightValue;
            MinSpeedBottomLightValue = visionModel.MinSpeedBottomLightValue;
            MaxSpeedBottomLightValue = visionModel.MaxSpeedBottomLightValue;

            ThresholdDark = visionModel.ThresholdDark;
            ThresholdLight = visionModel.ThresholdLight;
            ThresholdSize = visionModel.ThresholdSize;
            ThresholdColor = visionModel.ThresholdColor;
            ColorAverageCount = visionModel.ColorAverageCount;

            FrameMarginL = visionModel.FrameMarginL;
            FrameMarginT = visionModel.FrameMarginT;
            FrameMarginR = visionModel.FrameMarginR;
            FrameMarginB = visionModel.FrameMarginB;

            PatternWidth = visionModel.PatternWidth;
            PatternHeight = visionModel.PatternHeight;
            PatternMarginX = visionModel.PatternMarginX;
            PatternMarginY = visionModel.PatternMarginY;

            SkipFirstImage = visionModel.SkipFirstImage;
            GpuProcessing = visionModel.GpuProcessing;
            TargetIntensity = visionModel.TargetIntensity;
            OutTargetIntensity = visionModel.OutTargetIntensity;
            CalibrateFrameCnt = visionModel.CalibrateFrameCnt;
            MaxDegreeOfParallelism = visionModel.MaxDegreeOfParallelism;
            MaxDefectCount = visionModel.MaxDefectCount;
            DefectPriority = visionModel.DefectPriority;

            InflateSize = visionModel.InflateSize;
            UseOtherCategory = visionModel.UseOtherCategory;
        }

        public void CopyDefectCategoriesFrom(VisionModel visionModel)
        {
            DefectCategories = new List<DefectCategory>(visionModel.DefectCategories);
        }

        public void CopyNodeParamsFrom(VisionModel visionModel)
        {
            NodeParams = new List<INodeParam>(visionModel.NodeParams);
        }

        public bool Load(string modelPath)
        {
            string filePath = $"{modelPath}\\VisionModel.xml";
            if (File.Exists(filePath) == false)
            {
                return false;
            }

            string readString = File.ReadAllText(filePath);
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            VisionModel model = JsonConvert.DeserializeObject<VisionModel>(readString, setting);
            CopyFrom(model);

            return true;
        }

        public void Save(string modelPath)
        {
            string filePath = $"{modelPath}\\VisionModel.xml";
            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            string writeString = JsonConvert.SerializeObject(this, Formatting.Indented, setting);
            File.WriteAllText(filePath, writeString);
        }
        #endregion
    }
}
