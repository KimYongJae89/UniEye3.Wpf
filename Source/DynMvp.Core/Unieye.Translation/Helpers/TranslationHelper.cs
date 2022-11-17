using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace UniEye.Translation.Helpers
{
    /// <summary>
    /// 다국어 지원 목록
    /// </summary>
    public enum LanguageSettings
    {
        English,
        Korean,
        Chinese_Simpled,
    }

    /// <summary>
    /// 다국어 기능 지원
    /// </summary>
    public class TranslationHelper : INotifyPropertyChanged
    {
        #region 필드
        private static TranslationHelper _instance;

        private CultureInfo _currentCultureInfo;
        #endregion


        #region 생성자
        public TranslationHelper()
        {
            CultureInfos = new CultureInfo[] { CultureInfo.CreateSpecificCulture("en-us"), CultureInfo.CreateSpecificCulture("ko-kr"), CultureInfo.CreateSpecificCulture("zh-Hans") };

            ControlTextPairs = new Dictionary<Control, string>();
            DataGridViewTextPairs = new Dictionary<DataGridViewColumn, string>();
            ToolStripItemPairs = new Dictionary<ToolStripItem, string>();
            PropertyGridPairs = new Dictionary<PropertyGrid, object[]>();
        }
        #endregion


        #region 속성
        public static TranslationHelper Instance => _instance ?? (_instance = new TranslationHelper());

        public static CultureInfo[] CultureInfos { get; set; }

        public CultureInfo CurrentCultureInfo
        {
            get => _currentCultureInfo;
            set
            {
                if (Set(ref _currentCultureInfo, value))
                {
                    ApplyResources();
                }
            }
        }

        private static Lazy<ResourceManager> ResourceManager;

        private static Dictionary<Control, string> ControlTextPairs { get; set; }

        private static Dictionary<DataGridViewColumn, string> DataGridViewTextPairs { get; set; }

        private static Dictionary<ToolStripItem, string> ToolStripItemPairs { get; set; }

        private static Dictionary<PropertyGrid, object[]> PropertyGridPairs { get; set; }
        #endregion


        #region 이벤트
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region 메서드
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 다국어 파일이 존재하는 경로인 resourcePath를 매개변수로 전달받아 ResourceManager 개체를 생성합니다.
        /// </summary>
        /// <param name="resourcePath">다국어 파일의 경로</param>
        public static void Initialize(string resourcePath)
        {
            ResourceManager = new Lazy<ResourceManager>(() => System.Resources.ResourceManager.CreateFileBasedResourceManager("Resources", resourcePath, null));
        }

        /// <summary>
        /// ResourceManager를 이용하여 전달받은 Key로 리소스파일에서 탐색해 실시간으로 언어변환을 할 수 있도록 합니다.
        /// </summary>
        /// <param name="key">번역하고자 하는 String</param>
        /// <returns>지정한 문화권의 리소스파일에서 찾은 Key의 Value값</returns>
        public string Translate(string key)
        {
            string findKey = key?.ToUpper();
            if (string.IsNullOrEmpty(findKey))
            {
                return findKey;
            }

            try
            {
                return ResourceManager.Value.GetString(findKey, CurrentCultureInfo);
            }
            catch (Exception)
            {
                Console.WriteLine("언어 키가 없습니다 : {0}", findKey);
            }

            return string.Format("!{0}!", findKey);
        }

        /// <summary>
        /// ResourceManager를 이용하여 전달받은 Key로 리소스파일에서 탐색해 실시간으로 언어변환을 할 수 있도록 합니다.
        /// </summary>
        /// <param name="key">번역하고자 하는 String</param>
        /// <param name="language">번역하고자 하는 문화권</param>
        /// <returns>지정한 문화권의 리소스파일에서 찾은 Key의 Value값</returns>
        public string Translate(string key, LanguageSettings language)
        {
            Array languageEnum = Enum.GetValues(typeof(LanguageSettings));
            int cultureIndex = Array.FindIndex(languageEnum.Cast<LanguageSettings>().ToArray(), x => x == language);

            if (cultureIndex > CultureInfos.Length)
            {
                return key;
            }

            string findKey = key?.ToUpper();
            if (string.IsNullOrEmpty(findKey))
            {
                return findKey;
            }

            try
            {
                return ResourceManager.Value.GetString(findKey, CultureInfos[cultureIndex]);
            }
            catch (Exception)
            {
                Console.WriteLine("언어 키가 없습니다 : {0}", findKey);
            }

            return string.Format("!{0}!", findKey);
        }

        /// <summary>
        /// CurrentCultureInfo를 지역화 할 문화권으로 변경합니다.
        /// ComboBox의 이벤트에 추가하기 위함입니다.
        /// </summary>
        /// <param name="newCultureinfo">번역하고자 하는 문화권입니다.</param>
        public void ChangeLanguage(string newCultureinfo)
        {
            if (newCultureinfo.Contains("kr") || newCultureinfo.Contains("ko"))
            {
                CurrentCultureInfo = CultureInfo.GetCultureInfo("ko-KR");
            }
            else if (newCultureinfo.Contains("ch"))
            {
                CurrentCultureInfo = CultureInfo.GetCultureInfo("zh-CN");
            }
            else if (newCultureinfo.Contains("en"))
            {
                CurrentCultureInfo = CultureInfo.GetCultureInfo("en-US");
            }
            else
            {
                CurrentCultureInfo = CultureInfo.GetCultureInfo("en-US");
            }
        }

        /// <summary>
        /// 매개변수로 전달받은 컨트롤을 돌면서
        /// ( Key : 해당 컨트롤
        /// Value : 해당 컨트롤의 Text 값 )
        /// 을 Dictionary에 등록합니다. 
        /// </summary>
        /// <param name="mainControl">Form이나 Usercontrol 생성자에서 자기자신을 매개변수로 전달합니다.</param>
        public static void InitializeControl(Control mainControl)
        {
            foreach (Control control in mainControl.Controls)
            {
                if (control is DataGridView dataGridView)
                {
                    for (int index = 0; index < dataGridView.Columns.Count; index++)
                    {
                        DataGridViewTextPairs.Add(dataGridView.Columns[index], dataGridView.Columns[index].HeaderText);
                    }
                }
                else if (control is ToolStrip toolStrip)
                {
                    ToolStripItemCollection toolStripItemCollection = toolStrip.Items;
                    foreach (ToolStripItem item in toolStripItemCollection)
                    {
                        ToolStripItem[] collection = GetAllToolStripItemChildren(item);

                        foreach (ToolStripItem allItem in collection)
                        {
                            Type type = item.GetType();
                            if (allItem is ToolStripMenuItem)
                            {
                                foreach (ToolStripItem i in ((ToolStripMenuItem)allItem).DropDownItems)
                                {
                                    ToolStripItemPairs.Add(i, i.Text);
                                }
                            }
                            else if (allItem is ToolStripSplitButton)
                            {
                                foreach (ToolStripItem i in ((ToolStripSplitButton)allItem).DropDownItems)
                                {
                                    ToolStripItemPairs.Add(i, i.Text);
                                }
                            }
                            else if (allItem is ToolStripDropDownButton)
                            {
                                foreach (ToolStripItem i in ((ToolStripDropDownButton)allItem).DropDownItems)
                                {
                                    ToolStripItemPairs.Add(i, i.Text);
                                }
                            }
                            else if (allItem is ToolStripLabel)
                            {
                                ToolStripItemPairs.Add(allItem, allItem.Text);
                            }
                        }
                    }
                }
                else if (control is Control)
                {
                    if (control is DataGridView subDataGridView)
                    {
                        for (int index = 0; index < subDataGridView.Columns.Count; index++)
                        {
                            DataGridViewTextPairs.Add(subDataGridView.Columns[index], subDataGridView.Columns[index].HeaderText);
                        }
                    }
                    else if (control is PropertyGrid propertyGrid)
                    {
                        object[] items = propertyGrid.SelectedObjects;
                        PropertyGridPairs.Add(propertyGrid, items);
                    }
                    else
                    {
                        if (control is ScrollableControl)
                        {
                            InitializeControl(control);
                        }
                        else
                        {
                            ControlTextPairs.Add(control, control.Text);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ToolStripItem 내에 종속되어 있는 모든 컨트롤을 가져옵니다.
        /// </summary>
        /// <param name="item">ToolStripItem</param>
        /// <returns>종속되어있는 컨트롤을 List에 담은 후 배열로 convert 하여 ToolStripItemp[] 반환합니다.</returns>
        private static ToolStripItem[] GetAllToolStripItemChildren(ToolStripItem item)
        {
            var Items = new List<ToolStripItem> { item };
            if (item is ToolStripMenuItem)
            {
                foreach (ToolStripItem i in ((ToolStripMenuItem)item).DropDownItems)
                {
                    Items.AddRange(GetAllToolStripItemChildren(i));
                }
            }
            else if (item is ToolStripSplitButton)
            {
                foreach (ToolStripItem i in ((ToolStripSplitButton)item).DropDownItems)
                {
                    Items.AddRange(GetAllToolStripItemChildren(i));
                }
            }
            else if (item is ToolStripDropDownButton)
            {
                foreach (ToolStripItem i in ((ToolStripDropDownButton)item).DropDownItems)
                {
                    Items.AddRange(GetAllToolStripItemChildren(i));
                }
            }
            else if (item is ToolStripButton)
            {
                var toolStripButton = new List<ToolStripItem>();
                toolStripButton.Add(item);
                Items.AddRange(toolStripButton);
            }
            return Items.ToArray();
        }

        /// <summary>
        /// 각 Dictionary를 탐색하면서 컨트롤(Dictionary의 key)의 Text값을 번역하여 바꿔줍니다.
        /// </summary>
        private void ApplyResources()
        {
            foreach (KeyValuePair<Control, string> pair in ControlTextPairs)
            {
                pair.Key.Text = TranslationHelper.Instance.Translate(pair.Value);
            }

            foreach (KeyValuePair<DataGridViewColumn, string> pair in DataGridViewTextPairs)
            {
                pair.Key.HeaderText = TranslationHelper.Instance.Translate(pair.Value);
            }

            foreach (KeyValuePair<ToolStripItem, string> pair in ToolStripItemPairs)
            {
                pair.Key.Text = TranslationHelper.Instance.Translate(pair.Value);
            }

            foreach (KeyValuePair<PropertyGrid, object[]> pair in PropertyGridPairs)
            {
                pair.Key.SelectedObjects = null;
                pair.Key.SelectedObjects = pair.Value;
            }
        }
        #endregion
    }
}
