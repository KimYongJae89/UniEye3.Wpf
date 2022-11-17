using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Device
{
    public class VirtualImage
    {
        string directory;
        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }   

        int numInspectionStep;
        public int NumInspectionStep
        {
            get { return numInspectionStep; }
            set { numInspectionStep = value; }
        }

        int numLight;
        public int NumLight
        {
            get { return numLight; }
            set { numLight = value; }
        }

        int numCamera;
        public int NumCamera
        {
            get { return numCamera; }
            set { numCamera = value; }
        }

        string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string GetFileName(int index)
        {
            return String.Format("{0}\\{1}_Step{2}.img", directory, name, index);
        }

        public void Load(string fileName)
        {
            if (File.Exists(fileName) == false)
                return;

            directory = Path.GetDirectoryName(fileName);
            name = Path.GetFileNameWithoutExtension(fileName);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlElement virtualImageElement = xmlDocument.DocumentElement;

            numInspectionStep = Convert.ToInt32(XmlHelper.GetValue(virtualImageElement, "NumInspectionStep", numInspectionStep.ToString()));
            numLight = Convert.ToInt32(XmlHelper.GetValue(virtualImageElement, "NumLight", numLight.ToString()));
            numCamera = Convert.ToInt32(XmlHelper.GetValue(virtualImageElement, "NumCamera", numCamera.ToString()));

            description = XmlHelper.GetValue(virtualImageElement, "Description", description);
        }

        public void Save(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement virtualImageElement = xmlDocument.CreateElement("", "VirtualImage", "");
            xmlDocument.AppendChild(virtualImageElement);

            XmlHelper.SetValue(virtualImageElement, "NumInspectionStep", Configuration.NumInspectionStep.ToString());
            XmlHelper.SetValue(virtualImageElement, "NumLight", Configuration.NumLight.ToString());
            XmlHelper.SetValue(virtualImageElement, "NumCamera", Configuration.NumCamera.ToString());

            XmlHelper.SetValue(virtualImageElement, "Description", description);

            xmlDocument.Save(fileName);
        }
    }
}
