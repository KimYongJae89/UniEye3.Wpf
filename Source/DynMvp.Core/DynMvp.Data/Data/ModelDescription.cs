using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Data
{
    public class ModelDescription
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public bool UseByMasterModel { get; set; }
        public string MasterModelName { get; set; }
        public string Owner { get; set; }
        public bool IsTrained { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public ExportPacketFormat ExportPacketFormat { get; set; } = new ExportPacketFormat();
        public ExportPacketFormat ReportPacketFormat { get; set; } = new ExportPacketFormat();

        public ModelDescription()
        {
            ProductName = "";
            ProductCode = "";

            ReportPacketFormat.PacketStart = "";
            ReportPacketFormat.PacketEnd = "";
            ReportPacketFormat.Separator = ",";
        }

        public virtual ModelDescription Clone()
        {
            ModelDescription desc = ModelManager.Instance().ModelBuilder.CreateModelDescription();

            CopyTo(desc);

            return desc;
        }

        public virtual void CopyTo(ModelDescription dstDesc)
        {
            dstDesc.Name = Name;
            dstDesc.Category = Category;
            dstDesc.ProductName = ProductName;
            dstDesc.ProductCode = ProductCode;
            dstDesc.Description = Description;
            dstDesc.UseByMasterModel = UseByMasterModel;
            dstDesc.MasterModelName = MasterModelName;
            dstDesc.Owner = Owner;
            dstDesc.IsTrained = IsTrained;
            dstDesc.CreatedDate = CreatedDate;
            dstDesc.ModifiedDate = ModifiedDate;
            dstDesc.ExportPacketFormat = ExportPacketFormat.Clone();
            dstDesc.ReportPacketFormat = ReportPacketFormat.Clone();
        }

        public virtual void Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                MessageBox.Show(string.Format("Can't find model description file. {0}", fileName));
                return;
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            Load(xmlDocument.DocumentElement);
        }

        public virtual void Load(XmlElement modelDescElement)
        {
            Description = XmlHelper.GetValue(modelDescElement, "Description", "");

            string text = XmlHelper.GetValue(modelDescElement, "CreateDate", "");
            if (text.Length != 0)
            {
                CreatedDate = DateTime.Parse(text);
            }

            text = XmlHelper.GetValue(modelDescElement, "ModifiedDate", "");
            if (text.Length != 0)
            {
                ModifiedDate = DateTime.Parse(text);
            }

            Category = XmlHelper.GetValue(modelDescElement, "Category", "");
            Category = StringManager.GetString(Category);

            ProductName = XmlHelper.GetValue(modelDescElement, "ProductName", "");
            ProductCode = XmlHelper.GetValue(modelDescElement, "ProductCode", "");
            MasterModelName = XmlHelper.GetValue(modelDescElement, "MasterModelName", "");

            UseByMasterModel = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "UseByMasterModel", "False"));

            XmlElement exportPacketFormatElement = modelDescElement["ExportPacketFormat"];
            if (exportPacketFormatElement != null)
            {
                ExportPacketFormat.Load(exportPacketFormatElement);
            }

            XmlElement reportPacketFormatElement = modelDescElement["ReportPacketFormat"];
            if (reportPacketFormatElement != null)
            {
                ReportPacketFormat.Load(reportPacketFormatElement);
                ReportPacketFormat.PacketStart = "";
                ReportPacketFormat.PacketEnd = "";
                ReportPacketFormat.Separator = ",";
            }
        }

        public virtual void Save(string fileName)
        {
            var xmlDocument = new XmlDocument();

            XmlElement modelDescElement = xmlDocument.CreateElement("", "ModelDescription", "");
            xmlDocument.AppendChild(modelDescElement);

            Save(modelDescElement);

            xmlDocument.Save(fileName);
        }

        public virtual void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "Description", Description);

            XmlHelper.SetValue(modelDescElement, "CreateDate", CreatedDate.ToString());

            ModifiedDate = DateTime.Now;
            XmlHelper.SetValue(modelDescElement, "ModifiedDate", ModifiedDate.ToString());

            XmlHelper.SetValue(modelDescElement, "Category", Category);
            XmlHelper.SetValue(modelDescElement, "ProductName", ProductName);
            XmlHelper.SetValue(modelDescElement, "ProductCode", ProductCode);
            XmlHelper.SetValue(modelDescElement, "MasterModelName", MasterModelName);

            XmlHelper.SetValue(modelDescElement, "UseByMasterModel", UseByMasterModel.ToString());

            XmlElement exportPacketFormatElement = modelDescElement.OwnerDocument.CreateElement("", "ExportPacketFormat", "");
            modelDescElement.AppendChild(exportPacketFormatElement);

            ExportPacketFormat.Save(exportPacketFormatElement);

            XmlElement reportPacketFormatElement = modelDescElement.OwnerDocument.CreateElement("", "ReportPacketFormat", "");
            modelDescElement.AppendChild(reportPacketFormatElement);

            ReportPacketFormat.Save(reportPacketFormatElement);
        }

        public virtual void Load(ValueTable<string> valueTable)
        {
            Description = valueTable["Description"];
            Category = valueTable["Category"];
            ProductName = valueTable["ProductName"];
            ProductCode = valueTable["ProductCode"];
        }

        public virtual void Save(ValueTable<string> valueTable)
        {
            valueTable.AddValue("Description", Description);
            valueTable.AddValue("Category", Category);
            valueTable.AddValue("ProductName", ProductName);
            valueTable.AddValue("ProductCode", ProductCode);
        }

        public Bitmap GetPreviewImage()
        {
            string imagePath = Path.Combine(ModelManager.Instance().ModelPath, Name, "Image", "Prev.bmp");
            if (File.Exists(imagePath) == false)
            {
                return null;
            }

            return (Bitmap)ImageHelper.LoadImage(imagePath);
        }
    }
}
