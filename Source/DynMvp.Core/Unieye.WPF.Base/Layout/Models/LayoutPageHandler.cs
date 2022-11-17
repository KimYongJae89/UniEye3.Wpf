using DynMvp.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Layout.Models
{
    public class LayoutPageHandler : Observable
    {
        private string pageName = "Layout";
        public string PageName
        {
            get => pageName;
            set => Set(ref pageName, value);
        }

        private int rows = 1;
        public int Rows
        {
            get => rows;
            set => Set(ref rows, value);
        }

        private int columns = 1;
        public int Columns
        {
            get => columns;
            set => Set(ref columns, value);
        }

        public ObservableRangeCollection<LayoutModel> ModelList { get; set; } = new ObservableRangeCollection<LayoutModel>();

        public void Add(LayoutModel layoutModel)
        {
            ModelList.Add(layoutModel);
        }

        public void Remove(LayoutModel layoutModel)
        {
            ModelList.Remove(layoutModel);
        }

        public LayoutPageHandler Clone()
        {
            var layoutPageHandler = new LayoutPageHandler();
            layoutPageHandler.CopyFrom(this);
            return layoutPageHandler;
        }

        public void CopyFrom(LayoutPageHandler layoutList)
        {
            PageName = layoutList.PageName;
            Columns = layoutList.Columns;
            Rows = layoutList.Rows;
            ModelList.Clear();
            foreach (LayoutModel layoutModel in layoutList.ModelList)
            {
                ModelList.Add(layoutModel.Clone());
            }

            foreach (LayoutModel model in ModelList)
            {
                model.UploadProperty();
            }
        }

        public void Save(string fileName = "Layout")
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, $"{fileName}.cfg");

            foreach (LayoutModel model in ModelList)
            {
                model.DownloadProperty();
            }

            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            string writeString = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented, setting);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load(string fileName = "Layout")
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, $"{fileName}.cfg");
            if (File.Exists(cfgPath) == false)
            {
                Save(fileName);
                return;
            }

            var setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;

            string readString = File.ReadAllText(cfgPath);
            try
            {
                LayoutPageHandler layoutList = JsonConvert.DeserializeObject<LayoutPageHandler>(readString, setting);
                CopyFrom(layoutList);
            }
            catch (JsonSerializationException ex)
            {
                LogHelper.Error($"LayoutPageHandler::Load - {ex.Message}");
            }
        }
    }
}
