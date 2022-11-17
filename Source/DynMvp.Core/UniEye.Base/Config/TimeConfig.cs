using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.Config
{
    // 기본적으로 mili second 단위 사용
    public class TimeConfig
    {
        public int ExposureTime { get; set; } = 10;
        public int TriggerDelay { get; set; } = 0;
        /// <summary>
        /// Step 검사 완료 후 대기 시간
        /// </summary>
        public int StepDelay { get; set; } = 0;
        /// <summary>
        /// 검사 시작시 대기 시간
        /// </summary>
        public int InspectionDelay { get; set; } = 0;
        public int NgSignalHoldTime { get; set; } = 0;
        public int RejectPusherPushTime { get; set; } = 200;
        public int RejectPusherPullTime { get; set; } = 500;
        public int RejectWaitTime { get; set; } = 500;
        public int AirActionStableTime { get; set; } = 100;
        public int InspectTimeOut { get; set; } = 60000;

        public int ResultStoringDays { get; set; } = 0;

        private static TimeConfig instance;
        public static TimeConfig Instance()
        {
            if (instance == null)
            {
                instance = new TimeConfig();
            }

            return instance;
        }

        private TimeConfig()
        {
        }

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Time.cfg");

            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Time.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            instance = JsonConvert.DeserializeObject<TimeConfig>(readString);
        }
    }
}
