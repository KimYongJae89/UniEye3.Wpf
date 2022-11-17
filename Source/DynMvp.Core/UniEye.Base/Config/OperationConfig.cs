using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace UniEye.Base.Config
{
    public class OperationConfig
    {
        public string SystemVersion { get; set; } = "1.0";
        public string SystemType { get; set; } = "";
        public int SystemRevision { get; set; } = -1;
        public ImagingLibrary ImagingLibrary { get; set; } = ImagingLibrary.OpenCv;

        public string DefaultUserId { get; set; } = "op";
        public string DefaultUserPassword { get; set; } = "op";

        public bool RepeatInspection { get; set; } = false;
        public bool AutoResetProductionCount { get; set; } = false;
        public bool DebugMode { get; set; } = false;
        public int CpuUsage { get; set; } = 0;

        public int DumpType { get; set; } = 0;

        /// <summary>
        /// 사용자 계정 관리를 사용할 것인지 여부를 설정한다.
        /// LogInForm / Change User / User Manager 등이 활성화 되고, 권한에 따른 기능 설정이 변경되어야 한다.
        /// </summary>
        public bool UseUserManager { get; set; } = false;
        public bool UseRemoteBackup { get; set; } = false;
        public bool SingleModel { get; set; } = false;

        public int ResultStoringDays { get; set; } = 0;

        public bool UseDefectReview { get; set; } = false;
        public DataPathType DataPathType { get; set; } = DataPathType.Model_Day;
        public bool UseAlgorithmPool { get; set; } = false;

        public bool UseMachineAlive { get; set; } = false;

        private static OperationConfig instance;
        public static OperationConfig Instance()
        {
            if (instance == null)
            {
                instance = new OperationConfig();
            }
            return instance;
        }

        private OperationConfig() { }

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Operation.cfg");

            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Operation.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            instance = JsonConvert.DeserializeObject<OperationConfig>(readString);
        }

        public string GetBuildNo()
        {
            Version assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            // assemblyVersion.Build = days after 2000-01-01
            // assemblyVersion.Revision*2 = seconds after 0-hour  (NEVER daylight saving time) 
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(assemblyVersion.Build).AddSeconds(assemblyVersion.Revision * 2);

            return buildDate.ToString("yyMMdd");
        }
    }
}
