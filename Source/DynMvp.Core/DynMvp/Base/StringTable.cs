using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Base
{
    public class StringTable
    {
        public string Name { get; set; }

        public int Count => stringTable.Count;

        private Dictionary<string, string> stringTable = new Dictionary<string, string>();

        public StringTable()
        {
        }

        public StringTable(string name)
        {
            Name = name;
        }

        public Dictionary<string, string>.Enumerator GetEnumerator()
        {
            return stringTable.GetEnumerator();
        }

        public void Load(string fileName)
        {
            if (System.IO.File.Exists(fileName) == false)
            {
                return;
            }

            stringTable = new Dictionary<string, string>();

            if (Path.GetExtension(fileName) == ".xml")
            {
                LoadXml(fileName);
            }
            else if (Path.GetExtension(fileName) == ".st")
            {
                LoadSt(fileName);
            }
        }

        public void LoadXml(string fileName)
        {
            var xmldocument = new XmlDocument();
            xmldocument.Load(fileName);

            XmlElement rootElement = xmldocument.DocumentElement;

            foreach (XmlElement stringElment in rootElement)
            {
                if (stringElment.Name == "String")
                {
                    string key = XmlHelper.GetValue(stringElment, "Key", "");
                    string value = XmlHelper.GetValue(stringElment, "Value", "");


                    if (string.IsNullOrEmpty(key) == false && string.IsNullOrEmpty(value) == false)
                    {
                        stringTable.Add(key, value);
                    }
                }
            }
        }

        public void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("String");
            foreach (XmlElement stringElment in xmlNodeList)
            {
                string key = XmlHelper.GetValue(stringElment, "Key", "");
                string value = XmlHelper.GetValue(stringElment, "Value", "");

                if (string.IsNullOrEmpty(key) == false && string.IsNullOrEmpty(value) == false && stringTable.ContainsKey(key) == false)
                {
                    stringTable.Add(key, value);
                }
            }
        }

        private void LoadSt(string fileName)
        {
            string[] stringLines = File.ReadAllLines(fileName);

            foreach (string line in stringLines)
            {
                string[] tokens = line.Split('=');

                if (tokens.Count() == 2)
                {
                    if (string.IsNullOrEmpty(tokens[0]) == false && string.IsNullOrEmpty(tokens[1]) == false)
                    {
                        stringTable.Add(tokens[0], tokens[1]);
                    }
                }
            }
        }

        public void Save(string fileName)
        {
            if (Path.GetExtension(fileName) == ".xml")
            {
                SaveXml(fileName);
            }
            else if (Path.GetExtension(fileName) == ".st")
            {
                SaveSt(fileName);
            }
        }

        public void SaveSt(string fileName)
        {
            var stringBuilder = new StringBuilder();

            foreach (KeyValuePair<string, string> stringPair in stringTable.OrderBy(key => key.Key))
            {
                stringBuilder.AppendLine(string.Format("{0}={1}", stringPair.Key, stringPair.Value));
            }

            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        public void SaveXml(string fileName)
        {
            var xmldocument = new XmlDocument();
            XmlElement rootElement = xmldocument.CreateElement("", "StringTable", "");
            xmldocument.AppendChild(rootElement);

            foreach (KeyValuePair<string, string> stringPair in stringTable.OrderBy(key => key.Key))
            {
                XmlElement stringElement = rootElement.OwnerDocument.CreateElement("", "String", "");
                rootElement.AppendChild(stringElement);

                XmlHelper.SetValue(stringElement, "Key", stringPair.Key);
                XmlHelper.SetValue(stringElement, "Value", stringPair.Value);
            }

            xmldocument.Save(fileName);
        }

        public void SaveXml(XmlElement xmlElement)
        {
            foreach (KeyValuePair<string, string> stringPair in stringTable.OrderBy(key => key.Key))
            {
                XmlElement stringElement = xmlElement.OwnerDocument.CreateElement("", "String", "");
                xmlElement.AppendChild(stringElement);

                XmlHelper.SetValue(stringElement, "Key", stringPair.Key);
                XmlHelper.SetValue(stringElement, "Value", stringPair.Value);
            }
        }

        public bool IsExist(string key)
        {
            return stringTable.ContainsKey(key);
        }

        public void AddString(string key, string value)
        {
            stringTable.Add(key, value);
        }

        public string GetString(string searchKey)
        {
            if (stringTable.ContainsKey(searchKey))
            {
                return stringTable[searchKey];
            }
            return null;
        }

        public bool SetString(string searchKey, string value)
        {
            if (stringTable.TryGetValue(searchKey, out string tempValue) == true)
            {
                stringTable[searchKey] = value;
                return true;
            }

            return false;
        }

        public bool DeleteString(string key)
        {
            return stringTable.Remove(key);
        }
    }
}
