using DynMvp.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.Library
{
    public class TreeViewItem
    {
        #region Internal member variables

        private static int contructedCount = 1;

        #endregion

        #region Properties

        public int Id { get; set; } = -1;
        public int ParentId { get; set; } = -1;
        public int ImageIndex { get; set; } = -1;
        public int SelectedIndex { get; set; } = -1;
        public string Name { get; set; }
        public DirectoryInfo DirInfo { get; set; }
        public FileInfo FileInfo { get; set; }
        public TemplateGroup TemplateLibGroup { get; set; }

        #endregion

        public TreeViewItem(DirectoryInfo directoryInfo, int parentId, int imageIndex, int selectedIndex = -1)
        {
            Id = contructedCount++;
            ParentId = parentId;
            Name = directoryInfo.Name;
            ImageIndex = imageIndex;
            SelectedIndex = selectedIndex;
            DirInfo = directoryInfo;
        }
    }

    public class LibraryData
    {
        public string Name { get; set; }
        public string Path { get; set; }
        [JsonIgnore]
        public TemplatePool TemplatePool { get; set; }

        [JsonIgnore]
        public TemplateGroup TemplateGroup { get; set; }

        public LibraryData()
        {

        }

        public LibraryData(string name, string path = "")
        {
            Name = name;
            Path = path;
        }
    }

    public class LibraryManager
    {
        #region Internal member variables

        private TemplatePool templatePool = new TemplatePool();
        private List<TreeViewItem> treeViewItemList = new List<TreeViewItem>();
        private List<LibraryData> libraryDataList = new List<LibraryData>();

        #endregion

        #region Properties

        public string RootPath { get; set; }

        #endregion

        #region Singleton

        private static LibraryManager libraryManager;
        public static LibraryManager Instance()
        {
            if (libraryManager == null)
            {
                libraryManager = new LibraryManager();
                libraryManager.RootPath = string.Format(@"{0}\..\Library", Environment.CurrentDirectory);
            }

            return libraryManager;
        }

        #endregion


        #region Load

        public void Load()
        {
            templatePool.Clear();
            treeViewItemList.Clear();

            if (Directory.Exists(RootPath) == false)
            {
                Directory.CreateDirectory(RootPath);
            }

            var directoryInfo = new DirectoryInfo(LibraryManager.Instance().RootPath);

            if (directoryInfo.Exists)
            {
                templatePool.DirInfo = directoryInfo;

                try
                {
                    TreeViewItem item = LoadTemplateGroup(directoryInfo, 0);
                    treeViewItemList.Add(item);
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
            }
        }

        private TreeViewItem LoadTemplateGroup(DirectoryInfo dirInfo, int parentId)
        {
            if (dirInfo.Exists == false)
            {
                return null;
            }

            var group = new TemplateGroup(dirInfo);

            group.Load();
            templatePool.AddTemplateGroup(group);

            var treeItem = new TreeViewItem(dirInfo, parentId, 0, 0);
            treeItem.TemplateLibGroup = group;

            foreach (DirectoryInfo subDirectoryInfo in dirInfo.GetDirectories())
            {
                TreeViewItem item = LoadTemplateGroup(subDirectoryInfo, treeItem.Id);
                treeViewItemList.Add(item);
            }

            //foreach (Template template in group.TemplateList)
            //{
            //    TreeViewItem item = new TreeViewItem(dirInfo, treeItem.Id, 2);
            //    treeViewItemList.Add(item);
            //}

            return treeItem;
        }

        #endregion

        #region Get

        public List<TreeViewItem> GetTreeItemList()
        {
            return treeViewItemList;
        }

        public TemplatePool GetTemplatePool()
        {
            return templatePool;
        }

        public TemplateGroup GetTemplateGroup(DirectoryInfo info)
        {
            return templatePool.GroupList.Find(x => x.DirInfo.FullName == info.FullName);
        }

        public Template GetTemplate(string key)
        {
            Template template = null;

            foreach (LibraryData libraryData in libraryDataList)
            {
                template = libraryData.TemplateGroup.TemplateList.Find(x => x.Key == key);

                if (template != null)
                {
                    break;
                }
            }

            return template;
        }

        public Template GetTemplate(DirectoryInfo groupDirectoryinfo, Guid guid)
        {
            Template template = null;
            TemplateGroup templateGroup = GetTemplateGroup(groupDirectoryinfo);

            if (templateGroup != null)
            {
                template = templateGroup.TemplateList.Find(x => x.Id == guid);

                if (template != null)
                {
                    return template;
                }
            }

            return template;
        }

        public Template GetTemplate(Guid guid)
        {
            Template template = null;

            foreach (LibraryData libraryData in libraryDataList)
            {
                template = libraryData.TemplateGroup.TemplateList.Find(x => x.Id == guid);

                if (template != null)
                {
                    break;
                }
            }

            return template;
        }

        public List<Template> GetTemplateList(DirectoryInfo groupDirectoryinfo, Guid guid)
        {
            var templateList = new List<Template>();
            TemplateGroup templateGroup = GetTemplateGroup(groupDirectoryinfo);

            if (templateGroup != null)
            {
                templateList = templateGroup.TemplateList;
            }

            return templateList;
        }

        #endregion

        public void AddTemplateGroup(DirectoryInfo directoryInfo)
        {
            var templateGroup = new TemplateGroup(directoryInfo.Name);
            templateGroup.DirInfo = directoryInfo;

            templatePool.AddTemplateGroup(templateGroup);
        }

        public void AddTemplateGroup(TemplateGroup templateGroup)
        {
            templatePool.AddTemplateGroup(templateGroup);
        }

        public void RemoveTemplateGroup(DirectoryInfo directoryInfo)
        {
            TemplateGroup templateGroup = templatePool.GroupList.Find(x => x.DirInfo == directoryInfo);
            templatePool.GroupList.Remove(templateGroup);
        }

        public void RemoveTemplateGroup(TemplateGroup templateGroup)
        {
            RemoveTemplateGroup(templateGroup.DirInfo);
        }

        public void AddTemplate(DirectoryInfo directoryInfo, Template template)
        {
            TemplateGroup group = GetTemplateGroup(directoryInfo);

            if (group == null)
            {
                AddTemplateGroup(directoryInfo);
            }

            group.AddTemplate(template);
        }

        public void RemoveTemplate(Template template, DirectoryInfo directoryInfo)
        {
            TemplateGroup group = GetTemplateGroup(directoryInfo);

            if (group != null)
            {
                group.RemoveTemplate(template);
            }
        }

        #region Existed Codes

        public void Init()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Library.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            libraryDataList = JsonConvert.DeserializeObject<List<LibraryData>>(readString);
        }

        //public void AddTemplatePool(string name, string path)
        //{
        //    libraryDataList.Add(new LibraryData(name, path));
        //}

        //public void DeleteTemplatePool(string name)
        //{
        //    LibraryData libraryData = libraryDataList.Find(x => x.Name == name);
        //    if (libraryData != null)
        //        libraryDataList.Remove(libraryData);
        //}

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Library.cfg");

            string writeString = JsonConvert.SerializeObject(libraryDataList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        private string[] GetLibraryNames()
        {
            var names = new List<string>();
            libraryDataList.ForEach((x) => names.Add(x.Name));

            return names.ToArray();
        }

        public TemplatePool GetTemplatePool(string name)
        {
            LibraryData libraryData = libraryDataList.Find(x => x.Name == name);
            if (libraryData != null)
            {
                return libraryData.TemplatePool;
            }

            if (Directory.Exists(libraryData.Path) == true)
            {
                var templatePool = new TemplatePool();
                libraryData.TemplatePool = templatePool;

                templatePool.Load(libraryData.Path);

                return templatePool;
            }

            return null;
        }

        #endregion
    }
}
