using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Barcode
{
    public class BarcodeTextItem
    {
        public string Name { get; set; } = "";
        public Point Position { get; set; } = new Point(80, 10);
        public Font Font { get; set; } = new Font("Arial", 7);
        public StringAlignment Alignment { get; set; } = StringAlignment.Near;
        public bool Use { get; set; } = true;
    }

    public class BarcodeSettings
    {
        public BarcodeRendererType BarcodeRendererType { get; set; }

        private List<BarcodeTextItem> barcodeTextItemList = new List<BarcodeTextItem>();

        private Point barcodePos;
        public Point BarcodePos
        {
            get => barcodePos;
            set => barcodePos = value;
        }

        private Size barcodeSize;
        public Size BarcodeSize
        {
            get => barcodeSize;
            set => barcodeSize = value;
        }

        private Size pageSize;
        public Size PageSize
        {
            get => pageSize;
            set => pageSize = value;
        }

        public IEnumerator<BarcodeTextItem> GetEnumerator()
        {
            return barcodeTextItemList.GetEnumerator();
        }

        public BarcodeSettings()
        {
            BarcodeRendererType = BarcodeRendererType.Code128;

            barcodePos = new Point(80, 20);
            barcodeSize = new Size(120, 30);
        }

        public void RemoveTextItem()
        {
            barcodeTextItemList.Clear();
        }

        public void AddTextItem(BarcodeTextItem barcodeTextItem)
        {
            barcodeTextItemList.Add(barcodeTextItem);
        }

        public void LoadTextItem(XmlElement BarcodeTextItemElement, BarcodeTextItem barcodeTextItem)
        {
            barcodeTextItem.Name = XmlHelper.GetValue(BarcodeTextItemElement, "Name", "");

            var position = new Point(); ;
            XmlHelper.GetValue(BarcodeTextItemElement, "Pos", ref position);
            barcodeTextItem.Position = position;

            string fontSizeStr = XmlHelper.GetValue(BarcodeTextItemElement, "FontSize", "");
            if (string.IsNullOrEmpty(fontSizeStr))
            {
                var font = new Font("Arial", 7);
                XmlHelper.GetValue(BarcodeTextItemElement, "Font", ref font);
                barcodeTextItem.Font = font;
            }
            else
            {
                barcodeTextItem.Font = new Font("Arial", Convert.ToSingle(fontSizeStr));
            }

            barcodeTextItem.Alignment = (StringAlignment)Enum.Parse(typeof(StringAlignment), XmlHelper.GetValue(BarcodeTextItemElement, "Alignment", barcodeTextItem.Alignment.ToString()));
            barcodeTextItem.Use = Convert.ToBoolean(XmlHelper.GetValue(BarcodeTextItemElement, "Use", barcodeTextItem.Use.ToString()));
        }

        public void SaveTextItem(XmlElement BarcodeTextItemElement, BarcodeTextItem barcodeTextItem)
        {
            XmlHelper.SetValue(BarcodeTextItemElement, "Name", barcodeTextItem.Name);
            XmlHelper.SetValue(BarcodeTextItemElement, "Use", barcodeTextItem.Use.ToString());
            XmlHelper.SetValue(BarcodeTextItemElement, "Pos", barcodeTextItem.Position);
            XmlHelper.SetValue(BarcodeTextItemElement, "Font", barcodeTextItem.Font);
            XmlHelper.SetValue(BarcodeTextItemElement, "Alignment", barcodeTextItem.Alignment.ToString());
        }

        public void Load(XmlElement configElement)
        {
            XmlElement barcodeElement = configElement["Barcode"];
            if (barcodeElement == null)
            {
                return;
            }

            XmlHelper.GetValue(barcodeElement, "BarcodePageSize", ref pageSize);

            BarcodeRendererType = (BarcodeRendererType)Enum.Parse(typeof(BarcodeRendererType), XmlHelper.GetValue(barcodeElement, "BarcodeRendererType", BarcodeRendererType.ToString()));
            XmlHelper.GetValue(barcodeElement, "BarcodePos", ref barcodePos);
            XmlHelper.GetValue(barcodeElement, "BarcodeSize", ref barcodeSize);

            XmlElement barcodeTextItemListElement = barcodeElement["BarcodeTextItemList"];
            if (barcodeTextItemListElement == null)
            {
                return;
            }

            foreach (XmlElement barcodeTextItemElement in barcodeTextItemListElement)
            {
                var barcodeTextItem = new BarcodeTextItem();

                LoadTextItem(barcodeTextItemElement, barcodeTextItem);

                barcodeTextItemList.Add(barcodeTextItem);
            }
        }

        public void Save(XmlElement configElement)
        {
            XmlElement barcodeElement = configElement.OwnerDocument.CreateElement("", "Barcode", "");
            configElement.AppendChild(barcodeElement);

            XmlHelper.SetValue(barcodeElement, "BarcodePageSize", pageSize);

            XmlHelper.SetValue(barcodeElement, "BarcodeRendererType", BarcodeRendererType.ToString());
            XmlHelper.SetValue(barcodeElement, "BarcodePos", barcodePos);
            XmlHelper.SetValue(barcodeElement, "BarcodeSize", barcodeSize);

            XmlElement barcodeTextItemListElement = configElement.OwnerDocument.CreateElement("", "BarcodeTextItemList", "");
            barcodeElement.AppendChild(barcodeTextItemListElement);

            foreach (BarcodeTextItem barcodeTextItem in barcodeTextItemList)
            {
                XmlElement barcodeTextItemElement = configElement.OwnerDocument.CreateElement("", "BarcodeTextItem", "");
                barcodeTextItemListElement.AppendChild(barcodeTextItemElement);

                SaveTextItem(barcodeTextItemElement, barcodeTextItem);
            }
        }
    }
}
