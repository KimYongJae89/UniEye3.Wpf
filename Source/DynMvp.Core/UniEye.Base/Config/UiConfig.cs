using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniEye.Base.Config
{
    public enum LanguageType
    {
        English,
        Korean,
        Chinese_Simple,
    }

    public class UiConfig
    {
        public static string[] LanguageCode =
        {
            "en",
            "ko-KR",
            "zh-CN",
        };

        public string Language { get; set; } = "English";
        public string Title { get; set; } = "UniEye";
        public bool ShowProgramTitle { get; set; } = false;
        public string ProgramTitle { get; set; } = "UniEye";
        public string Copyright { get; set; } = "Copyright 2015. UniEye Inc. All rights reserved";
        public bool UseUpDownControl { get; set; } = false;
        public bool ShowCenterGuide { get; set; } = false;
        public Point CenterGuidePos { get; set; }
        public int CenterGuideThickness { get; set; } = 1;
        public bool ShowNumber { get; set; } = false;
        public bool ShowScore { get; set; } = false;
        public bool ShowNGImage { get; set; } = false;

        public int TrackerSize { get; set; } = 10;
        /// <summary>
        /// 검사 완료 후 최종 이미지를 다시 획득하여 화면에 표시할지 여부를 설정한다.
        /// 일반적으로 Aligner와 같이 사용된다.
        /// </summary>
        public bool ShowFinalImage { get; set; } = false;
        public bool Use3dViewer { get; set; } = false;

        private static UiConfig instance = null;
        public static UiConfig Instance()
        {
            if (instance == null)
            {
                instance = new UiConfig();
            }

            return instance;
        }

        public string GetLocaleCode()
        {
            string localeCode = "";

            if (Language == null || Language == "" || Language == "English")
            {
                return "";
            }

            int startIndex = Language.IndexOf("[");
            int endIndex = Language.IndexOf("]");
            if (startIndex > -1 && endIndex > -1 && endIndex > startIndex)
            {
                localeCode = Language.Substring(startIndex + 1, endIndex - startIndex - 1);
            }

            return localeCode;
        }

        public CultureInfo LoadLanguage()
        {
            switch (Language)
            {
                case "Korean[ko-kr]":
                    return CultureInfo.GetCultureInfo("ko-KR");
                case "Chinese(Simplified)[zh-cn]":
                    return CultureInfo.GetCultureInfo("zh-CN");
                case "English":
                default:
                    return CultureInfo.GetCultureInfo("en-US");
            }
        }

        public void SaveLanguage(CultureInfo CurrentCultureInfo)
        {
            switch (CurrentCultureInfo.Name)
            {
                case "ko-KR":
                    Language = "Korean[ko-kr]";
                    break;
                case "zh-CN":
                    Language = "Chinese(Simplified)[zh-cn]";
                    break;
                case "en-US":
                default:
                    Language = "English";
                    break;
            }

            Save();
        }

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "UserInterface.cfg");

            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "UserInterface.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            instance = JsonConvert.DeserializeObject<UiConfig>(readString);
        }
    }
}
