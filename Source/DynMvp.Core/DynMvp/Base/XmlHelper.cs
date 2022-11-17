using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Base
{
    public interface IXmlStoring
    {
        void Save(XmlElement xmlElement);
        void Load(XmlElement xmlElement);
    }

    public class XmlHelper
    {
        public static XmlDocument Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                string tempFileName = fileName + ".bak";
                if (File.Exists(tempFileName) == true)
                {
                    File.Move(tempFileName, fileName);
                }
                else
                {
                    LogHelper.Error(string.Format("There is no XML file : {0}", fileName));
                    return null;
                }
            }

            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(fileName);
            }
            catch (Exception)
            {
                LogHelper.Error(string.Format("There is some error in XML file : {0}", fileName));
                xmlDocument = null;
            }

            return xmlDocument;
        }

        public static void Import(string fileName, XmlElement parentElement)
        {
            var xmlSettings = new XmlReaderSettings();

            var xmlReader = XmlReader.Create(fileName, xmlSettings);

            XmlNode xmlNode = parentElement.OwnerDocument.ReadNode(xmlReader);

            parentElement.AppendChild(xmlNode);
        }

        // Test 필요
        public static void SyncValue(XmlElement xmlElement, string fileName)
        {
            var xmlSettings = new XmlReaderSettings();

            XmlDocument xmlDocument = Load(fileName);

            SyncValue("", xmlElement, xmlDocument);
        }

        public static void SyncValue(string parentKeyName, XmlElement xmlElement, XmlDocument xmlDocument)
        {
            string currentKeyName = parentKeyName + "/" + xmlElement.Name;

            XmlNode xmlNode = xmlDocument.SelectSingleNode(currentKeyName);
            xmlElement.Value = xmlNode.Value;

            foreach (XmlElement subElement in xmlElement)
            {
                SyncValue(currentKeyName, subElement, xmlDocument);
            }
        }

        public static void Save(XmlDocument xmlDocument, string fileName)
        {
            string tempFileName = fileName + "~";
            string bakName = fileName + ".bak";

            var xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Entitize,
                NewLineChars = "\r\n"
            };

            var xmlWriter = XmlWriter.Create(tempFileName, xmlSettings);

            xmlDocument.WriteTo(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();

            FileHelper.SafeSave(tempFileName, bakName, fileName);
        }

        public static void Save(XmlElement xmlElement, string fileName)
        {
            string tempFileName = fileName + "~";
            string bakName = fileName + ".bak";

            var xmlSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Entitize,
                NewLineChars = "\r\n"
            };

            var xmlWriter = XmlWriter.Create(tempFileName, xmlSettings);

            xmlElement.WriteTo(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();

            FileHelper.SafeSave(tempFileName, bakName, fileName);
        }

        public static string GetAttributeValue(XmlElement xmlElement, string attributeName, string defaultValue)
        {
            string attributeValue = xmlElement.GetAttribute(attributeName);
            if (attributeValue == "")
            {
                return defaultValue;
            }

            return attributeValue;
        }

        public static void SetAttributeValue(XmlElement xmlElement, string attributeName, string value)
        {
            xmlElement.SetAttribute(attributeName, value);
        }

        public static int GetValue(XmlElement xmlElement, string keyName, int defaultValue)
        {
            //string value = GetValue(xmlElement, keyName, defaultValue.ToString());
            return int.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static string GetValue(XmlElement xmlElement, string keyName, string defaultValue)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return defaultValue;
            }

            return subElement.InnerText;
        }

        public static bool Exist(XmlElement xmlElement, string keyName)
        {
            XmlElement subElement = xmlElement[keyName];
            return (subElement != null);
        }

        public static XmlElement GetElement(XmlDocument document, string keyName, bool bCreate)
        {
            string[] tokens = keyName.TrimStart('/').Split('/');

            XmlElement subElement;
            XmlElement xmlElement = document.DocumentElement;
            foreach (string elementName in tokens)
            {
                if (xmlElement[elementName] == null)
                {
                    if (bCreate == false)
                    {
                        return null;
                    }

                    subElement = document.CreateElement("", elementName, "");
                    xmlElement.AppendChild(subElement);
                }

                xmlElement = xmlElement[elementName];
            }

            return xmlElement;
        }

        public static void SetValue(XmlDocument document, string keyName, string value)
        {
            XmlElement xmlElement = GetElement(document, keyName, true);
            xmlElement.InnerText = value;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, string value)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = value;
        }

        // int
        public static void SetValue(XmlElement xmlElement, string keyName, int value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static byte GetValue(XmlElement xmlElement, string keyName, byte defaultValue)
        {
            string value = GetValue(xmlElement, keyName, defaultValue.ToString());
            return byte.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, int defaultValue, ref int getValue)
        {
            getValue = int.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, uint defaultValue, ref uint getValue)
        {
            getValue = uint.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // float
        public static void SetValue(XmlElement xmlElement, string keyName, float value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static float GetValue(XmlElement xmlElement, string keyName, float defaultValue)
        {
            return float.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, float defaultValue, ref float getValue)
        {
            getValue = float.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // double
        public static void SetValue(XmlElement xmlElement, string keyName, double value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static double GetValue(XmlElement xmlElement, string keyName, double defaultValue)
        {
            return double.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, int defaultValue, ref double getValue)
        {
            getValue = double.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // bool
        public static void SetValue(XmlElement xmlElement, string keyName, bool value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, bool defaultValue)
        {
            if (!bool.TryParse(GetValue(xmlElement, keyName, defaultValue.ToString()), out bool parse))
            {
                return defaultValue;
            }

            return parse;
        }

        public static void GetValue(XmlElement xmlElement, string keyName, bool defaultValue, ref bool getValue)
        {
            bool ok = bool.TryParse(GetValue(xmlElement, keyName, defaultValue.ToString()), out getValue);
            if (ok == false)
            {
                getValue = defaultValue;
            }
        }

        // Rectangle
        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetAttributeValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetAttributeValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetAttributeValue(subElement, "Height", rectangle.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle rectangle)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            rectangle.X = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Height", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, RectangleF rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetAttributeValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetAttributeValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetAttributeValue(subElement, "Height", rectangle.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref RectangleF rectangle)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            rectangle.X = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Height", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, RotatedRect rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetAttributeValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetAttributeValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetAttributeValue(subElement, "Height", rectangle.Height.ToString());
            XmlHelper.SetAttributeValue(subElement, "Angle", rectangle.Angle.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref RotatedRect rectangle)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            rectangle.X = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Height", "0"));
            rectangle.Angle = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Angle", "0"));

            return true;
        }

        // Pen
        public static void SetValue(XmlElement xmlElement, string keyName, Pen pen)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Color", pen.Color);
            XmlHelper.SetAttributeValue(subElement, "Width", pen.Width.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Pen pen)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            var color = new Color();
            XmlHelper.GetValue(subElement, "Color", ref color);
            int width = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Width", "1"));

            pen = new Pen(color, width);

            return true;
        }

        // Brush
        public static void SetValue(XmlElement xmlElement, string keyName, Brush brush)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            var solidBrush = brush as SolidBrush;

            XmlHelper.SetValue(subElement, "Color", solidBrush.Color);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Brush brush)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            var color = new Color();
            XmlHelper.GetValue(subElement, "Color", ref color);

            brush = new SolidBrush(color);

            return true;
        }

        // Point
        public static void SetValue(XmlElement xmlElement, string keyName, Point point)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "X", point.X.ToString());
            XmlHelper.SetAttributeValue(subElement, "Y", point.Y.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point point)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            point.X = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "X", "0"));
            point.Y = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Y", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, PointF point)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "X", point.X.ToString());
            XmlHelper.SetAttributeValue(subElement, "Y", point.Y.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref PointF point)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            point.X = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "X", "0"));
            point.Y = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Y", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Point3d point3d)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "X", point3d.X.ToString());
            XmlHelper.SetAttributeValue(subElement, "Y", point3d.Y.ToString());
            XmlHelper.SetAttributeValue(subElement, "Z", point3d.Z.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point3d point3d)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            point3d.X = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "X", "0"));
            point3d.Y = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Y", "0"));
            point3d.Z = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Z", "0"));

            return true;
        }

        // Size
        public static void SetValue(XmlElement xmlElement, string keyName, Size size)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "Width", size.Width.ToString());
            XmlHelper.SetAttributeValue(subElement, "Height", size.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Size size)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            size.Width = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Width", "0"));
            size.Height = Convert.ToInt32(XmlHelper.GetAttributeValue(subElement, "Height", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, SizeF size)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "Width", size.Width.ToString());
            XmlHelper.SetAttributeValue(subElement, "Height", size.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref SizeF size)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            size.Width = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Width", "0"));
            size.Height = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Height", "0"));

            return true;
        }

        // Font
        public static void SetValue(XmlElement xmlElement, string keyName, Font font)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetAttributeValue(subElement, "Family", font.FontFamily.GetName(0));
            XmlHelper.SetAttributeValue(subElement, "Size", font.SizeInPoints.ToString());
            XmlHelper.SetAttributeValue(subElement, "Style", font.Style.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Font font)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            string family = XmlHelper.GetAttributeValue(subElement, "Family", "Arial");
            float size = Convert.ToSingle(XmlHelper.GetAttributeValue(subElement, "Size", "10"));
            var style = (FontStyle)Enum.Parse(typeof(FontStyle), XmlHelper.GetAttributeValue(subElement, "Style", "Regular"));

            font = new Font(family, size, style);

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Color color)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = System.Drawing.ColorTranslator.ToHtml(color);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Color color)
        {
            XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
            {
                return false;
            }

            color = System.Drawing.ColorTranslator.FromHtml(subElement.InnerText);

            return true;
        }

        //Enum
        public static void SetValue(XmlElement xmlElement, string keyName, Enum @enum)
        {
            SetValue(xmlElement, keyName, @enum.ToString());
        }

        public static TValue GetValue<TValue>(XmlElement xmlElement, string keyName, TValue defaultValue)
             where TValue : struct
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
            {
                return defaultValue;
            }

            Type type = typeof(TValue);
            string stringValue = XmlHelper.GetValue(xmlElement, keyName, defaultValue.ToString());

            if (type.IsEnum)
            {
                if (Enum.TryParse<TValue>(stringValue, out TValue result))
                {
                    return result;
                }

                return defaultValue;
            }
            else
            {
                try
                {
                    return (TValue)Convert.ChangeType(stringValue, typeof(TValue));
                }
                catch (InvalidCastException ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("XmlHelper::GetValue<{0}> Exception - {1}", type, ex.Message));
                    return defaultValue;
                }
            }
        }
    }
}
