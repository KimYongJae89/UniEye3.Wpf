using System;
using System.Collections.Generic;
using System.Linq;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Module;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Service
{
    public static class CMLightCalibrationService
    {
        #region 필드
        public static readonly int maxLightValue = 220;

        public static readonly int minImageValue = 50;

        public static readonly int maxImageValue = 255;
        #endregion


        #region 속성
        public static ELightFittingType LightFittingType { get; set; } = ELightFittingType.Linear;

        public static int TargetLigthValue { get; set; } = 150;

        public static bool OnCalibration { get; set; }

        private static int TargetImageValue { get; set; } = 128;

        private static int StartTargetLigthValue { get; set; } = 110;

        private static int LightValueStep { get; set; } = 20;

        private static Dictionary<string, float> CalibrationLightValueDic { get; set; } = new Dictionary<string, float>();

        private static List<int> ValidLightValueList { get; set; } = new List<int>();

        private static List<float> ValidImageAverageList { get; set; } = new List<float>();
        #endregion


        #region 메서드
        public static void LightCalibrationInitialize()
        {
            if (ModelManager.Instance().CurrentModel is Model model)
            {
                TargetImageValue = model.VisionModels.FirstOrDefault().TargetIntensity;
                LightFittingType = model.VisionModels.FirstOrDefault().LightFittingType;
            }
            TargetLigthValue = StartTargetLigthValue;

            OnCalibration = true;

            ValidLightValueList.Clear();
            ValidImageAverageList.Clear();

            CalibrationLightValueDic.Clear();
            List<InspectModuleInfo> moduleList = SystemConfig.Instance.ImModuleList;
            foreach (InspectModuleInfo moduleInfo in moduleList)
            {
                CalibrationLightValueDic.Add(moduleInfo.ModuleTopic, -1);
            }
        }

        public static void AddImageAverageValue(string module, List<string> parameters)
        {
            foreach (string param in parameters)
            {
                string[] splitParam = param.Split(';');
                if (CalibrationLightValueDic.ContainsKey(splitParam[0]))
                {
                    CalibrationLightValueDic[module] = Convert.ToSingle(splitParam[1]);
                    break;
                }
            }

            if (CalibrationLightValueDic.Values.Count(x => x == -1) == 0)
            {
                CalculateNextTargetLight(TargetLigthValue, CalibrationLightValueDic.Values.Average());
                ClearCalibrationLightValueDic();
            }
        }

        private static void CalculateNextTargetLight(int currentLigthValue, float imageAverageValue)
        {
            switch (LightFittingType)
            {
                default:
                case ELightFittingType.Linear:
                    {
                        if (imageAverageValue > minImageValue && imageAverageValue < maxImageValue)
                        {
                            TargetLigthValue = Convert.ToInt32(Math.Round(TargetImageValue / imageAverageValue * currentLigthValue));
                            OnCalibration = false;
                        }
                        else
                        {
                            if (imageAverageValue > TargetImageValue)
                            {
                                TargetLigthValue -= LightValueStep;
                            }
                            else
                            {
                                TargetLigthValue += LightValueStep;
                            }
                        }
                    }
                    break;
                case ELightFittingType.LinearIntercept:
                    {
                        // 최초 측정일 경우
                        if (ValidLightValueList.Count == 0)
                        {
                            if (imageAverageValue > minImageValue && imageAverageValue < maxImageValue)
                            {
                                ValidLightValueList.Add(currentLigthValue);
                                ValidImageAverageList.Add(imageAverageValue);
                            }

                            if (imageAverageValue > TargetImageValue)
                            {
                                TargetLigthValue -= LightValueStep;
                            }
                            else
                            {
                                TargetLigthValue += LightValueStep;
                            }
                        }
                        else
                        {
                            float beforeImageAverage = ValidImageAverageList.FirstOrDefault();
                            float beforeLightValue = ValidLightValueList.FirstOrDefault();

                            if (imageAverageValue > minImageValue && imageAverageValue < maxImageValue && imageAverageValue != beforeImageAverage)
                            {
                                ValidLightValueList.Add(currentLigthValue);
                                ValidImageAverageList.Add(imageAverageValue);

                                float angle = (currentLigthValue - beforeLightValue) / (imageAverageValue - beforeImageAverage);
                                TargetLigthValue = Convert.ToInt32(Math.Round(angle * (TargetImageValue - imageAverageValue) + currentLigthValue));
                                OnCalibration = false;
                            }
                            else
                            {
                                if (imageAverageValue > TargetImageValue)
                                {
                                    TargetLigthValue -= LightValueStep;
                                }
                                else
                                {
                                    TargetLigthValue += LightValueStep;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private static void ClearCalibrationLightValueDic()
        {
            List<InspectModuleInfo> moduleList = SystemConfig.Instance.ImModuleList;
            foreach (InspectModuleInfo moduleInfo in moduleList)
            {
                CalibrationLightValueDic[moduleInfo.ModuleTopic] = -1;
            }
        }

        // TDI카메라 사용시 조명 값 계산 메서드
        public static bool CalTDILightValue(VisionModel visionModel, float targetSpeed, out int topValue, out int bottomValue)
        {
            topValue = bottomValue = 0;

            float minSpeed = Math.Min(visionModel.MaxSpeed, visionModel.MinSpeed);
            float maxSpeed = Math.Max(visionModel.MaxSpeed, visionModel.MinSpeed);
            if (minSpeed == maxSpeed)
            {
                return false;
            }

            int minSpeedTop = Math.Min(visionModel.MinSpeedTopLightValue, visionModel.MaxSpeedTopLightValue);
            int maxSpeedTop = Math.Max(visionModel.MinSpeedTopLightValue, visionModel.MaxSpeedTopLightValue);
            int minSpeedBottom = Math.Min(visionModel.MinSpeedBottomLightValue, visionModel.MaxSpeedBottomLightValue);
            int maxSpeedBottom = Math.Max(visionModel.MinSpeedBottomLightValue, visionModel.MaxSpeedBottomLightValue);

            float topLightAngle = (maxSpeedTop - minSpeedTop) / (maxSpeed - minSpeed);
            float bottomLightAngle = (maxSpeedBottom - minSpeedBottom) / (maxSpeed - minSpeed);

            topValue = Convert.ToInt32(Math.Round(topLightAngle * (targetSpeed - minSpeed) + minSpeedTop));
            bottomValue = Convert.ToInt32(Math.Round(bottomLightAngle * (targetSpeed - minSpeed) + minSpeedBottom));

            return true;
        }
        #endregion
    }
}
