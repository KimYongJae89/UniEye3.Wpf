using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Library
{
    public enum PackageType
    {
        [Description("Capacitor")]
        Capacitor,
        [Description("Resistor")]
        Resistor,
        [Description("SOT")]
        SOT,
        [Description("BGA")]
        BGA,
        [Description("Multi Leaded")]
        MultiLeaded,
        [Description("QFP")]
        QFP,
        [Description("Tantalium")]
        Tantalium,
        [Description("Arrays")]
        Arrays,
        [Description("Al Cap")]
        AlCap,
        [Description("QFN")]
        QFN,
        [Description("DPAK")]
        DPAK,
        [Description("Inductor")]
        Inductor,
        [Description("User Package")]
        UserPackage,
    }

    public class TemplateGroup
    {
        public string Name { get; set; }
        public string PackageType { get; set; }
        [JsonIgnore]
        public List<Template> TemplateList { get; set; } = new List<Template>();

        [JsonIgnore]
        public DirectoryInfo DirInfo { get; set; }

        [JsonIgnore]
        public bool HasTemplateLibrary => TemplateList.Count > 0 ? true : false;

        public TemplateGroup()
        {

        }

        public TemplateGroup(string name)
        {
            Name = name;
        }

        public TemplateGroup(DirectoryInfo dirInfo)
        {
            Name = dirInfo.Name;
            DirInfo = dirInfo;
        }

        public void AddTemplate(Template template)
        {
            TemplateList.Add(template);
        }

        public void RemoveTemplate(Template template)
        {
            TemplateList.Remove(template);
        }

        public Template GetTemplate(string templateName)
        {
            return TemplateList.Find(x => x.Name == templateName);
        }

        public Template GetTemplate(Guid guid)
        {
            return TemplateList.Find(x => x.Id == guid);
        }

        internal void Load(DirectoryInfo info)
        {
            foreach (FileInfo fileInfo in info.GetFiles())
            {
                if (fileInfo.Exists)
                {
                    var template = new Template();
                    template.Load(fileInfo);

                    AddTemplate(template);
                }
            }
        }

        internal void Load()
        {
            if (DirInfo != null)
            {
                foreach (FileInfo fileInfo in DirInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly))
                {
                    if (fileInfo.Exists)
                    {
                        var template = new Template();
                        template.Load(fileInfo);

                        AddTemplate(template);
                    }
                }
            }
        }

        //internal void Load(string dirPath)
        //{
        //    string[] dirNames = Directory.GetDirectories(dirPath);
        //    foreach(string dirName in dirNames)
        //    {
        //        string templatePath = Path.Combine(dirPath, dirName);
        //        StepModelReader modelReader = StepModelReaderBuilder.Create(templatePath);

        //        Template template = new Template();
        //        if (modelReader.Load(template, templatePath))
        //        {
        //            template.Path = templatePath;
        //            templateList.Add(template);
        //        }
        //    }
        //}
    }
}
