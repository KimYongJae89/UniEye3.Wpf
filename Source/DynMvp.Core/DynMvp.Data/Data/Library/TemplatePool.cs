using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Library
{
    public class TemplatePool
    {
        #region Properties

        public List<TemplateGroup> GroupList { get; set; } = new List<TemplateGroup>();

        public DirectoryInfo DirInfo { get; set; }

        #endregion

        public void Clear()
        {
            GroupList.Clear();
        }

        public void AddTemplateGroup(TemplateGroup templateGroup)
        {
            GroupList.Add(templateGroup);
        }

        public Template GetTemplate(string packageType, string templateName)
        {
            foreach (TemplateGroup group in GroupList)
            {
                if (group.Name == packageType)
                {
                    return group.GetTemplate(templateName);
                }
            }

            return null;
        }

        internal void Load(string path)
        {
            string[] dirNames = Directory.GetDirectories(path);

            foreach (string dirName in dirNames)
            {
                string dirPath = Path.Combine(path, dirName);
                string datPath = Path.Combine(dirPath, "TemplateGroup.dat");
                if (File.Exists(datPath) == false)
                {
                    {
                        continue;
                    }
                }

                string readString = File.ReadAllText(datPath);
                TemplateGroup templateGroup = JsonConvert.DeserializeObject<TemplateGroup>(readString);
            }
        }
    }
}
