using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DynMvp.Base
{
    public class BaseConfig
    {
        public string TempPath { get; private set; } = "";
        public string ConfigPath { get; private set; } = "";
        public int TrackerSize { get; set; } = 10;
        public bool SaveDebugImage { get; set; } = false;

        private static BaseConfig instance = null;
        public static BaseConfig Instance()
        {
            if (instance == null)
            {
                instance = new BaseConfig();
            }

            return instance;
        }

        public void Init()
        {
            string currentDirectory = Environment.CurrentDirectory;

            ConfigPath = $"{currentDirectory}\\..\\Config";
            TempPath = $"{currentDirectory}\\..\\Temp";

            if (Directory.Exists(TempPath) == false)
            {
                Directory.CreateDirectory(TempPath);
            }

            if (Directory.Exists(ConfigPath) == false)
            {
                Directory.CreateDirectory(ConfigPath);
            }
        }
    }
}
