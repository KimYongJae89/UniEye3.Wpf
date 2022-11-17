using DynMvp.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

public interface IMultiLanguageSupport
{
    void UpdateLanguage();
}

namespace DynMvp.Base
{
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string value) : base(value)
        {

        }

        protected override string GetLocalizedString(string value)
        {
            return StringManager.GetString(GetType().FullName, value);
        }
    }

    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string value)
        {
            base.DisplayNameValue = StringManager.GetString(GetType().FullName, value);
        }
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public LocalizedDescriptionAttribute(string value)
        {
            base.DescriptionValue = StringManager.GetString(GetType().FullName, value);
        }
    }

    public static class StringManager
    {
        private static string xmlFilePath = "";
        public static List<StringTable> stringTableList = new List<StringTable>();
        public static List<IMultiLanguageSupport> multiLanguageSupportList = new List<IMultiLanguageSupport>();
        public static StringTable messageTable = new StringTable("Message");

        public static void Load(string path)
        {
            xmlFilePath = path;

            if (System.IO.File.Exists(path) == false)
            {
                return;
            }

            var xmldocument = new XmlDocument();
            try
            {
                xmldocument.Load(path);
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("StringManager::Load - {0}", ex.Message));
                MessageForm.Show(null, "StringTable load Fail!");
                return;
        }
#endif
            finally { }

            XmlElement rootElement = xmldocument.DocumentElement;
            if (rootElement.Name != "StringTableV2" && rootElement.Name != "StringTableV3")
            {
                return;
            }

            lock (stringTableList)
            {
                stringTableList.Clear();
                foreach (XmlElement stringElment in rootElement)
                {
                    var st = new StringTable(stringElment.Name);
                    st.LoadXml(stringElment);
                    stringTableList.Add(st);

                    if (stringElment.Name == messageTable.Name)
                    {
                        messageTable = st;
                    }
                }
            }

            multiLanguageSupportList.ForEach(f => f.UpdateLanguage());
            Save();
        }

        public static void Save(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = xmlFilePath;
            }

            var xmldocument = new XmlDocument();
            XmlElement rootElement = xmldocument.CreateElement("", "StringTableV3", "");
            xmldocument.AppendChild(rootElement);

            stringTableList.Sort((f, g) => f.Name.CompareTo(g.Name));
            foreach (StringTable stringTable in stringTableList)
            {
                XmlElement stringTableElement = rootElement.OwnerDocument.CreateElement("", stringTable.Name, "");
                rootElement.AppendChild(stringTableElement);

                stringTable.SaveXml(stringTableElement);
            }

            try
            {
                if (string.IsNullOrEmpty(path) == false)
                {
                    xmldocument.Save(path);
                }
            }
            catch (Exception)
            { }
        }

        public static void AddListener(IMultiLanguageSupport item)
        {
            lock (multiLanguageSupportList)
            {
                multiLanguageSupportList.Add(item);
            }

            item.UpdateLanguage();
            //UpdateString(item);
        }

        public static void RemoveListener(IMultiLanguageSupport item)
        {
            lock (multiLanguageSupportList)
            {
                multiLanguageSupportList.Remove(item);
            }
        }

        public static void Clear()
        {
            stringTableList.Clear();
        }

        public static string GetString(string searchKey)
        {
            if (messageTable.IsExist(searchKey))
            {
                return messageTable.GetString(searchKey);
            }
            else
            {
                messageTable.AddString(searchKey, searchKey);
                Save();
                return searchKey;
            }
        }

        public static string GetString(string tableName, string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey))
            {
                return "";
            }

            StringTable stringTable = GetStringTable(tableName);
            if (stringTable == null)
            {
                stringTable = new StringTable(tableName);
                stringTableList.Add(stringTable);
            }

            if (stringTable.IsExist(searchKey))
            {
                return stringTable.GetString(searchKey);
            }
            else
            {
                stringTable.AddString(searchKey, searchKey);
                Save();
                return searchKey;
            }
        }

        /// <summary>
        /// StringTableV2의 SearchKey는 Control.Text
        /// StringTableV3의 SearchKey는 Control.Name, DefaultValue는 Control.Text
        /// V2와 V3의 호환을 위해 Control.Name을 키로 갖는 값이 없으면 Control.Text로 한 번 더 찾아봄.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="searchKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetString(string tableName, string searchKey, string defaultValue)
        {
            if (string.IsNullOrEmpty(searchKey))
            {
                return "";
            }

            StringTable stringTable = GetStringTable(tableName);
            if (stringTable == null)
            {
                stringTable = new StringTable(tableName);
                stringTableList.Add(stringTable);
            }

            if (stringTable.IsExist(searchKey))
            {
                return stringTable.GetString(searchKey);
            }
            else
            {
                string resultValue = defaultValue;
                // StringTableV2와 호환성을 위함(DefaultValue를 Key로 하여 찾아봄)
                if (stringTable.IsExist(defaultValue))
                {
                    string oldValue = stringTable.GetString(defaultValue);
                    stringTable.DeleteString(defaultValue);
                    stringTable.AddString(searchKey, oldValue);
                    resultValue = oldValue;
                }
                else
                {
                    stringTable.AddString(searchKey, defaultValue);
                }
                Save();
                return resultValue;
            }
        }

        public static string GetString(string tableName, Control control)
        {
            if (string.IsNullOrEmpty(control.Text))
            {
                return "";
            }

            return GetString(tableName, control.Name, control.Text);
        }

        public static string GetString(string tableName, DataGridViewColumn dataGridViewColumn)
        {
            if (string.IsNullOrEmpty(dataGridViewColumn.HeaderText))
            {
                return "";
            }

            return GetString(tableName, dataGridViewColumn.Name, dataGridViewColumn.HeaderText);
        }

        public static string GetString(string tableName, ToolStripItem toolStripItem)
        {
            if (string.IsNullOrEmpty(toolStripItem.Text))
            {
                return "";
            }

            return GetString(tableName, toolStripItem.Name, toolStripItem.Text);
        }

        public static void UpdateString(IMultiLanguageSupport obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                object item = fieldInfo.GetValue(obj);

                if (item is DataGridViewColumn dataGridViewColumn)
                {
                    //dataGridViewColumn.HeaderText = StringManager.GetString(type.FullName, dataGridViewColumn.Name, dataGridViewColumn.HeaderText);
                    dataGridViewColumn.HeaderText = StringManager.GetString(type.FullName, dataGridViewColumn);
                }
                else if (item is Control control)
                {
                    //control.Text = StringManager.GetString(type.FullName, control.Name, control.Text);
                    control.Text = StringManager.GetString(type.FullName, control);
                    //UiHelper.AutoFontSize(control);
                }
                else if (item is ToolStripItem toolStripItem)
                {
                    //toolStrip.Text = StringManager.GetString(type.FullName, toolStrip.Name, toolStrip.Text);
                    toolStripItem.Text = StringManager.GetString(type.FullName, toolStripItem);
                }
            }
        }

        private static StringTable GetStringTable(string tableName)
        {
            StringTable stringTable = stringTableList.Find(f => f.Name == tableName);
            return stringTable;
        }

        public static void AddStringTable(StringTable stringTable)
        {
            stringTableList.Add(stringTable);
        }
    }
}
