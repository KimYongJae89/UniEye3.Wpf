using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;
using UniScanC.Algorithm.Base;
using UniScanC.Data;

namespace UniScanC.AlgoTask
{
    public delegate void OnInspectCompletedDelegate(InspectResult inspectResult);

    public class AlgoTaskManagerSetting
    {
        public List<INodeParam> ParamList { get; set; }
        public List<ILink> LinkList { get; set; }

        [JsonIgnore]
        public int TaskCount => ParamList.Count;

        public AlgoTaskManagerSetting()
        {
            ParamList = new List<INodeParam>();
            LinkList = new List<ILink>();
        }

        public AlgoTaskManagerSetting(INodeParam[] nodeParams, ILink[] links) : this()
        {
            ParamList.AddRange(nodeParams);
            LinkList.AddRange(links);
        }

        public AlgoTaskManagerSetting(AlgoTaskManagerSetting setting) : this()
        {
            CopyFrom(setting);
        }

        public void CopyFrom(AlgoTaskManagerSetting setting)
        {
            if (setting.ParamList.Count == 0 && setting.LinkList.Count == 0)
            {
                setting = AlgoTaskSettingDefault.SaintGobain_All.Clone();
            }

            ParamList.AddRange(setting.ParamList.Select(f => f.Clone()));
            LinkList.AddRange(setting.LinkList.Select(f => f.Clone()));
        }

        public AlgoTaskManagerSetting Clone()
        {
            return new AlgoTaskManagerSetting(this);
        }

        public void Clear()
        {
            ParamList.Clear();
            LinkList.Clear();
        }

        public void AddTask(params INodeParam[] algorithmParams)
        {
            ParamList.AddRange(algorithmParams);
        }

        public void AddLink(params ILink[] links)
        {
            LinkList.AddRange(links);
        }

        public void Save(string fullName)
        {
            Task.Run(async () => await SaveAsync(fullName)).Wait();
        }

        public async Task SaveAsync(string fullName)
        {
            string writeString = await Json.StringifyAsync(this, null);
            File.WriteAllText(fullName, writeString);
        }

        public static AlgoTaskManagerSetting Load(string fullName)
        {
            return Task<AlgoTaskManagerSetting>.Run(async () => await LoadAsync(fullName)).Result;
        }

        public static async Task<AlgoTaskManagerSetting> LoadAsync(string fullName)
        {
            return await Task.Run<AlgoTaskManagerSetting>(async () =>
            {
                if (!File.Exists(fullName))
                {
                    throw new FileNotFoundException();
                }

                string readString = File.ReadAllText(fullName);
                return await Json.ToObjectAsync<AlgoTaskManagerSetting>(readString);
            });
        }
    }

    public class AlgoTaskManagerSettingDictionary : Dictionary<string, AlgoTaskManagerSetting>
    {
        public AlgoTaskManagerSettingDictionary()
        {

        }

        public AlgoTaskManagerSettingDictionary((string, AlgoTaskManagerSetting)[] settings)
        {
            Array.ForEach(settings, f => Add(f.Item1, f.Item2));
        }

        public AlgoTaskManagerSettingDictionary(AlgoTaskManagerSettingDictionary settingDictionary)
        {
            foreach (KeyValuePair<string, AlgoTaskManagerSetting> setting in settingDictionary)
            {
                Add(setting.Key, setting.Value.Clone());
            }
        }

        public void UpdateModuleName(IEnumerable<string> moduleNames)
        {
            IEnumerable<AlgoTaskManagerSetting> settings = this.Select(f => f.Value).ToArray();
            Clear();

            for (int i = 0; i < moduleNames.Count(); i++)
            {
                if (settings.Count() > i)
                {
                    Add(moduleNames.ElementAt(i), settings.ElementAt(i));
                }
                else
                {
                    Add(moduleNames.ElementAt(i), new AlgoTaskManagerSetting());
                }
            }
        }

        public void CopyFrom(AlgoTaskManagerSettingDictionary src)
        {
            Clear();
            foreach (KeyValuePair<string, AlgoTaskManagerSetting> pair in src)
            {
                Add(pair.Key, pair.Value.Clone());
            }
        }

        public AlgoTaskManagerSettingDictionary Clone()
        {
            return new AlgoTaskManagerSettingDictionary(this);
        }

        public void Save(string directoryPath)
        {
            foreach (KeyValuePair<string, AlgoTaskManagerSetting> pair in this)
            {
                string fullPath = Path.Combine(directoryPath, $@"{pair.Key}.json");
                pair.Value.Save(fullPath);
            }
        }
    }
}
