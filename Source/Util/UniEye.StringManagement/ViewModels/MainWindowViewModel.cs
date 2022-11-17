using MahApps.Metro.Controls.Dialogs;
using StringManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using UniEye.StringManagement.Base.Helpers;
using UniEye.StringManagement.Helpers;

namespace UniEye.StringManagement.ViewModels
{
    public class MainWindowViewModel : Observable
    {
        #region 필드
        private Dictionary<string, ObservableCollection<ObservableValue<string>>> templanguageDictionary;

        private Dictionary<string, ObservableCollection<ObservableValue<string>>> searchDictionary = new Dictionary<string, ObservableCollection<ObservableValue<string>>>(new Dictionary<string, ObservableCollection<ObservableValue<string>>>());

        private KeyValuePair<string, ObservableCollection<ObservableValue<string>>> selectedPair;

        private IEnumerable<object> selectedPairs;

        private string exportPath;

        private string messageString;

        private string keyString = string.Empty;

        private bool findBlank = false;

        private int languageCount = 0;
        #endregion


        #region 생성자
        public MainWindowViewModel()
        {
            LoadSettings();

            LoadButtonClick = new RelayCommand(LoadButtonAction);
            HelpCommand = new RelayCommand(HelpAction);
            SortCommand = new RelayCommand(SortAction);
            ClearButtonClick = new RelayCommand(ClearButtonActionAsync);
            SaveButtonClick = new RelayCommand(SaveButtonAction);
            SelectExportPathCommand = new RelayCommand(SelectExportPathAction);
            ExportButtonClick = new RelayCommand(ExportButtonAction);
            AddButtonClick = new RelayCommand(AddButtonAction);
            EditButtonClick = new RelayCommand(EditButtonAction);
            DeleteButtonClick = new RelayCommand(DeleteButtonAction);
            ForceFindBlankCommand = new RelayCommand(ForceFindBlankAction);
            FindBlankCommand = new RelayCommand(FindBlankAction);
        }
        #endregion


        #region 속성
        public ICommand LoadButtonClick { get; }

        public ICommand HelpCommand { get; }

        public ICommand SortCommand { get; }

        public ICommand ClearButtonClick { get; }

        public ICommand SaveButtonClick { get; }

        public ICommand SelectExportPathCommand { get; }

        public ICommand ExportButtonClick { get; }

        public ICommand AddButtonClick { get; }

        public ICommand EditButtonClick { get; }

        public ICommand DeleteButtonClick { get; }

        public ICommand ForceFindBlankCommand { get; }

        public ICommand FindBlankCommand { get; }

        public Dictionary<string, ObservableCollection<ObservableValue<string>>> TemplanguageDictionary
        {
            get => templanguageDictionary;
            set => Set(ref templanguageDictionary, value);
        }

        public Dictionary<string, ObservableCollection<ObservableValue<string>>> SearchDictionary
        {
            get => searchDictionary;
            set => Set(ref searchDictionary, value);
        }

        public KeyValuePair<string, ObservableCollection<ObservableValue<string>>> SelectedPair
        {
            get => selectedPair;
            set => Set(ref selectedPair, value);
        }

        public IEnumerable<object> SelectedPairs
        {
            get => selectedPairs;
            set => Set(ref selectedPairs, value);
        }

        public string ExportPath
        {
            get => exportPath;
            set => Set(ref exportPath, value);
        }

        public string MessageString
        {
            get => messageString;
            set => Set(ref messageString, value);
        }

        public string KeyString
        {
            get => keyString;
            set
            {
                if (Set(ref keyString, value))
                {
                    Update();
                }
            }
        }

        public bool FindBlank
        {
            get => findBlank;
            set
            {
                if (Set(ref findBlank, value))
                {
                    if (findBlank)
                    {
                        KeyString = string.Empty;

                        Dictionary<string, ObservableCollection<ObservableValue<string>>> dictionary;

                        dictionary = SearchDictionary.Where(x => x.Value.Any(pair => pair.Value.Trim() == string.Empty)).ToDictionary(x => x.Key, x => x.Value);

                        if (ReferenceEquals(TemplanguageDictionary, dictionary))
                        {
                            TemplanguageDictionary = null;
                        }

                        TemplanguageDictionary = dictionary;
                    }
                    else
                    {
                        TemplanguageDictionary = SearchDictionary;
                    }

                }
            }
        }

        public int LanguageCount
        {
            get => languageCount;
            set => Set(ref languageCount, value);
        }

        private ObservableCollection<string> FileNameList { get; set; } = new ObservableCollection<string>();

        private ObservableCollection<string> LanguageTypeList { get; set; } = new ObservableCollection<string>();

        private bool SortedFlag { get; set; } = false;
        #endregion


        #region 메서드
        public void LoadSettings()
        {
            System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
            ExportPath = appSettings["ExportPath"] ?? string.Empty;
        }

        public void SaveSettings()
        {
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;

            if (settings["ExportPath"] == null)
            {
                settings.Add("ExportPath", ExportPath);
            }
            else
            {
                settings["ExportPath"].Value = ExportPath;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        private void LoadButtonAction()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "resx files (*.resx)|*.resx|all files (*.*)|*.*";

            if (dlg.ShowDialog() != true)
            {
                return;
            }

            foreach (string fileName in dlg.FileNames)
            {
                LoadXml(fileName);
            }

            LanguageCount = SearchDictionary.Count;
            Update();
        }

        private bool CheckFileLoaded()
        {
            if (FileNameList.Count == 0)
            {
                ShowOkMessage("Waring", "편집할 파일을 열어주세요.");
                return false;
            }
            return true;
        }

        private bool CheckSeletedRow()
        {
            if (SelectedPair.Key == null)
            {
                ShowOkMessage("Waring", "편집할 키를 선택해주세요.");
                return false;
            }
            return true;
        }

        private bool CheckDuplicateFile(string filename)
        {
            foreach (string strPath in FileNameList)
            {
                string strFileName = Path.GetFileNameWithoutExtension(strPath);

                if (strFileName == filename)
                {
                    ShowOkMessage("Warning", "같은 이름의 파일이 이미 열려있습니다.");
                    return false;
                }
            }
            return true;
        }

        private void HelpAction()
        {
            ShowOkMessage("Shortcut Key", "데이터 제거 : Ctrl+L".PadRight(20) + "파일열기 : Ctrl+O".PadRight(20) + "파일저장 : Ctrl+S".PadRight(20) + "\n"
                + "키 추가 : Ctrl+N".PadRight(20) + "키 편집 : Ctrl+M".PadRight(20) + "키 삭제 : Ctrl+D".PadRight(20) + "파일 변환 : Alt+E".PadRight(20) + "키 정렬 : Alt+S".PadRight(20) + "빈 항목 찾기 : Ctrl+B".PadRight(20));
        }

        private void SortAction()
        {
            if (!CheckFileLoaded())
            {
                return;
            }

            Dictionary<string, ObservableCollection<ObservableValue<string>>> dictionary;

            if (KeyString == string.Empty)
            {
                dictionary = SearchDictionary;
            }
            else
            {
                string findString = KeyString.ToUpper();
                dictionary = SearchDictionary.Where(x => x.Key.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0).ToDictionary(x => x.Key, x => x.Value);
            }

            if (ReferenceEquals(TemplanguageDictionary, dictionary))
            {
                TemplanguageDictionary = null;
            }

            var sortedDictionary = dictionary.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            if (SortedFlag == false)
            {
                TemplanguageDictionary = sortedDictionary;
                SortedFlag = true;
            }
            else
            {
                TemplanguageDictionary = dictionary;
                SortedFlag = false;
            }
        }

        private async void ClearButtonActionAsync()
        {
            if (!CheckFileLoaded())
            {
                return;
            }

            MessageDialogResult result = await ShowMessage(this, "Clear", "불러온 파일의 항목을 제거하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                SearchDictionary.Clear();
                FileNameList.Clear();
                LanguageTypeList.Clear();

                KeyString = "";
                MessageString = "";
                LanguageCount = 0;

                TemplanguageDictionary = null;
            }
        }

        private async void SaveButtonAction()
        {
            if (!CheckFileLoaded())
            {
                return;
            }

            MessageDialogResult result = await ShowMessage(this, "Save", "파일에 저장하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                if (SearchDictionary.Count != 0)
                {
                    for (int i = 0; i < FileNameList.Count; i++)
                    {
                        SaveXml(FileNameList[i], i);
                    }
                }

                SaveSettings();
            }
        }

        private void SelectExportPathAction()
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ExportPath;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportPath = dialog.SelectedPath;
            }
        }

        private async void ExportButtonAction()
        {
            if (!CheckFileLoaded())
            {
                return;
            }

            MessageDialogResult result = await ShowMessage(this, "Export", "resx 파일을 resources 파일로 변환하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                if (SearchDictionary.Count != 0)
                {
                    for (int i = 0; i < FileNameList.Count; i++)
                    {
                        ExportResourcesFile(FileNameList[i], ExportPath);
                    }
                }
            }
        }

        private void AddButtonAction()
        {
            if (!CheckFileLoaded())
            {
                return;
            }

            var keyWindow = new KeyWindow();
            var keyWindowViewModel = new KeyWindowViewModel(SearchDictionary, FileNameList.Count);
            keyWindowViewModel.KeyWindowMode = KeyWindowViewModel.WindowMode.Add;
            keyWindow.DataContext = keyWindowViewModel;
            if (keyWindow.ShowDialog() == true)
            {
                Update();
            }
        }

        private void EditButtonAction()
        {
            if (!CheckSeletedRow())
            {
                return;
            }

            var keyWindow = new KeyWindow();
            var keyWindowViewModel = new KeyWindowViewModel(SearchDictionary, FileNameList.Count, selectedPair);
            keyWindowViewModel.KeyWindowMode = KeyWindowViewModel.WindowMode.Edit;
            keyWindow.DataContext = keyWindowViewModel;

            if (keyWindow.ShowDialog() == true)
            {
                Update();
            }
        }

        private async void DeleteButtonAction()
        {
            if (!CheckSeletedRow())
            {
                return;
            }

            MessageDialogResult result = await ShowMessage(this, "Delete", "선택한 데이터를 삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                foreach (object pair in SelectedPairs)
                {
                    var keyValuePair = (KeyValuePair<string, ObservableCollection<ObservableValue<string>>>)pair;
                    SearchDictionary.Remove(keyValuePair.Key);
                }
                Update();
            }
        }

        private void ForceFindBlankAction()
        {
            FindBlank = !FindBlank;
        }

        private void FindBlankAction()
        {
            var keyValuePairs = new List<KeyValuePair<string, ObservableCollection<ObservableValue<string>>>>(); ;
            foreach (KeyValuePair<string, ObservableCollection<ObservableValue<string>>> data in searchDictionary)
            {
                ObservableCollection<ObservableValue<string>> value = data.Value;

                for (int index = 0; index < value.Count; index++)
                {
                    if (value[index].Value == null || value[index].Value == "")
                    {
                        var keyValuePair = new KeyValuePair<string, ObservableCollection<ObservableValue<string>>>(data.Key, data.Value);
                        keyValuePairs.Add(keyValuePair);
                        break;
                    }

                }
            }
            if (keyValuePairs.Count != 0)
            {
                SelectedPair = keyValuePairs[0];
            }
            else
            {
                ShowOkMessage("Warning", "비어있는 항목이 없습니다.");
            }
        }

        private void ExportResourcesFile(string path, string savePath)
        {
            string resPath;

            var pri = new System.Diagnostics.ProcessStartInfo();
            var pro = new System.Diagnostics.Process();

            pri.FileName = "cmd.exe";

            pri.CreateNoWindow = true;
            pri.UseShellExecute = false;

            pri.RedirectStandardInput = true;
            pri.RedirectStandardOutput = true;
            pri.RedirectStandardError = true;

            pro.EnableRaisingEvents = false;
            pro.StartInfo = pri;
            pro.Start();
#if DEBUG
            resPath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
#else
            resPath = Environment.CurrentDirectory;
#endif

            string fileName = Path.GetFileNameWithoutExtension(path);

            pro.StandardInput.WriteLine(@"""" + resPath + @"\resgen"" " + @"""" + path + @"""" + " " + @"""" + savePath + @"\" + fileName + ".resources" + @"""");
            pro.StandardInput.Close();

            string resultValue = pro.StandardOutput.ReadToEnd();
            pro.WaitForExit();
            pro.Close();

            //MessageBox.Show(resultValue);
        }

        private async void Update()
        {
            await Task.Run(() =>
            {
                FindBlank = false;

                Dictionary<string, ObservableCollection<ObservableValue<string>>> dictionary;

                if (KeyString == string.Empty)
                {
                    dictionary = SearchDictionary;
                }
                else
                {
                    string findString = KeyString.ToUpper();
                    dictionary = SearchDictionary.Where(x =>
                    x.Key.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.Value.Any(value => value?.Value?.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0)).ToDictionary(x => x.Key, x => x.Value);
                }

                if (ReferenceEquals(TemplanguageDictionary, dictionary))
                {
                    TemplanguageDictionary = null;
                }

                TemplanguageDictionary = dictionary;
            });
        }

        private void SaveXml(string path, int index)
        {
            if (File.Exists(path) == false)
            {
                return;
            }

            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(path);
            }
            finally { }

            XmlNode firstNode = xmlDocument.FirstChild;
            XmlNodeList oldElement = xmlDocument.SelectNodes("//data");

            foreach (XmlNode xmlNode in oldElement)
            {
                XmlNode parentNode = xmlNode.ParentNode;
                parentNode.RemoveChild(xmlNode);
            }

            XmlNode rootNode;
            XmlElement rootElement;
            XmlElement childElement;
            XmlAttribute xmlAtb1;
            XmlAttribute xmlAtb2;

            foreach (KeyValuePair<string, ObservableCollection<ObservableValue<string>>> pair in SearchDictionary)
            {
                rootNode = xmlDocument.SelectSingleNode("//root");
                rootElement = xmlDocument.CreateElement("data");

                childElement = xmlDocument.CreateElement("value");
                xmlAtb1 = xmlDocument.CreateAttribute("name");
                xmlAtb1.Value = pair.Key.ToUpper();

                xmlAtb2 = xmlDocument.CreateAttribute("xml:space");
                xmlAtb2.Value = "preserve";

                rootElement.SetAttributeNode(xmlAtb1);
                rootElement.SetAttributeNode(xmlAtb2);

                rootElement.AppendChild(childElement);
                rootNode.AppendChild(rootElement);

                if (index < pair.Value.Count)
                {
                    childElement.InnerText = pair.Value[index].Value;
                }
                else
                {
                    childElement.InnerText = "";
                }
            }
            xmlDocument.Save(path);
        }

        private void LoadXml(string path)
        {
            if (File.Exists(path) == false)
            {
                return;
            }

            string fileType = Path.GetExtension(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string languageType = fileName.Split('-').GetValue(fileName.Split('-').Length - 1).ToString();

            var xmlDocument = new XmlDocument();

            if (!CheckDuplicateFile(fileName))
            {
                return;
            }

            try
            {
                xmlDocument.Load(path);
            }
            finally { }

            if (fileType == ".resx")
            {
                LoadStringTable(xmlDocument);
                FileNameList.Add(path);
                LanguageTypeList.Add(languageType);
            }
        }

        private void LoadStringTable(XmlDocument xmlDocument)
        {
            XmlElement rootElement = xmlDocument.DocumentElement;
            XmlNodeList dataNodeList = xmlDocument.SelectNodes("//data");

            foreach (XmlNode data in dataNodeList)
            {
                string item1 = data.Attributes.Item(0).Value;
                string item2 = data.InnerText.ToString().Trim();

                var langData = new ObservableValue<string>(item2);

                if (!SearchDictionary.ContainsKey(item1))
                {
                    var list = new ObservableCollection<ObservableValue<string>>();
                    list.Add(langData);

                    SearchDictionary.Add(item1, list);
                }
                else
                {
                    SearchDictionary[item1].Add(langData);
                }
            }
        }

        private async void ShowOkMessage(string title, string message)
        {
            MessageDialogResult result = await ShowMessage(this, title, message, MessageDialogStyle.Affirmative);

            if (result == MessageDialogResult.Affirmative)
            {
                return;
            }
        }

        private static async Task<MessageDialogResult> ShowMessage(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            var settings = new MetroDialogSettings();
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            settings.AnimateShow = false;
            settings.AnimateHide = false;

            return await DialogCoordinator.Instance.ShowMessageAsync(context, title, message, style, settings);
        }
        #endregion
    }
}
