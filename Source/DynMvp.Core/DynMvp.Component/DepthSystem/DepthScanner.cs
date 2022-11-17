using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Component.DepthSystem
{
    public enum DepthScannerType
    {
        None, Virtual, Exyma, ExymaDual, Kinect2
    }

    public abstract class DepthScannerInfo
    {
        public string Name { get; set; }
        public DepthScannerType Type { get; set; }
        public int Index { get; set; }

        public static DepthScannerInfo Create(DepthScannerType type)
        {
            DepthScannerInfo depthScannerInfo = null;
            switch (type)
            {
                case DepthScannerType.Exyma:
                    depthScannerInfo = new ExymaScannerInfo(type);
                    break;
                case DepthScannerType.ExymaDual:
                    depthScannerInfo = new ExymaScannerInfo(type);
                    break;
            }

            return depthScannerInfo;
        }

        public virtual void Load(XmlElement nodeElement)
        {
            Name = XmlHelper.GetValue(nodeElement, "Name", "");
            Index = Convert.ToInt32(XmlHelper.GetValue(nodeElement, "Index", "0"));
            Type = (DepthScannerType)Enum.Parse(typeof(DepthScannerType), XmlHelper.GetValue(nodeElement, "Type", "Exyma"));
        }

        public virtual void Save(XmlElement nodeElement)
        {
            XmlHelper.SetValue(nodeElement, "Name", Name.ToString());
            XmlHelper.SetValue(nodeElement, "Type", Type.ToString());
            XmlHelper.SetValue(nodeElement, "Index", Index.ToString());
        }

        public abstract DepthScannerInfo Clone();

        public virtual void Copy(DepthScannerInfo srcDepthScannerInfo)
        {
            Index = srcDepthScannerInfo.Index;
            Name = srcDepthScannerInfo.Name;
            Type = srcDepthScannerInfo.Type;
        }
    }

    public class DepthScannerInfoList : List<DepthScannerInfo>
    {
        public DepthScannerInfoList Clone()
        {
            var newDepthScannerInfoList = new DepthScannerInfoList();

            foreach (DepthScannerInfo depthScannerInfo in this)
            {
                newDepthScannerInfoList.Add(depthScannerInfo.Clone());
            }

            return newDepthScannerInfoList;
        }
    }

    public class DepthScannerConfiguration
    {
        public DepthScannerInfoList DepthScannerInfoList { get; } = new DepthScannerInfoList();

        public IEnumerator<DepthScannerInfo> GetEnumerator()
        {
            return DepthScannerInfoList.GetEnumerator();
        }

        public void LoadConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Scanner Configuration");

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                return;
            }

            string configPath = Path.GetDirectoryName(fileName);

            XmlElement scannerListElement = xmlDocument.DocumentElement;
            foreach (XmlElement scannerElement in scannerListElement)
            {
                if (scannerElement.Name == "DepthScanner")
                {
                    var depthScannerType = (DepthScannerType)Enum.Parse(typeof(DepthScannerType), XmlHelper.GetValue(scannerElement, "Type", "Exyma"));

                    var scannerInfo = DepthScannerInfo.Create(depthScannerType);
                    if (scannerInfo != null)
                    {
                        scannerInfo.Load(scannerElement);

                        if (scannerInfo is ExymaScannerInfo exymaScannerInfo)
                        {
                            exymaScannerInfo.ConfigPath = configPath;
                        }

                        DepthScannerInfoList.Add(scannerInfo);
                    }
                }
            }
        }

        public void SaveConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Scanner Configuration");

            var xmlDocument = new XmlDocument();

            XmlElement depthScannerListElement = xmlDocument.CreateElement("", "DepthScannerList", "");
            xmlDocument.AppendChild(depthScannerListElement);

            foreach (DepthScannerInfo depthScannerInfo in DepthScannerInfoList)
            {
                XmlElement depthScannerElement = xmlDocument.CreateElement("", "DepthScanner", "");
                depthScannerListElement.AppendChild(depthScannerElement);

                depthScannerInfo.Save(depthScannerElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }

        public DepthScannerInfo GetDepthScannerInfo(int index)
        {
            foreach (DepthScannerInfo depthScannerInfo in DepthScannerInfoList)
            {
                if (depthScannerInfo.Index == index)
                {
                    return depthScannerInfo;
                }
            }

            return null;
        }

        public void AddDepthScannerInfo(DepthScannerInfo depthScannerInfo)
        {
            DepthScannerInfoList.Add(depthScannerInfo);
        }
    }

    public abstract class DepthScanner : ImageDevice
    {
        protected DepthScannerInfo depthScannerInfo;

        public static DepthScanner Create(DepthScannerType type)
        {
            DepthScanner depthScanner = null;
            switch (type)
            {
                case DepthScannerType.Exyma:
                    depthScanner = new ExymaSingleScanner();
                    break;
                case DepthScannerType.ExymaDual:
                    depthScanner = new ExymaDualScanner();
                    break;
            }

            return depthScanner;
        }

        public DepthScanner()
        {
            Name = "DepthScanner";

            DeviceType = DeviceType.DepthScanner;
            UpdateState(DeviceState.Idle);
        }

        public override bool IsDepthScanner()
        {
            return true;
        }

        public abstract Image3D CreateDepthImage();
        public abstract bool Initialize(DepthScannerInfo depthScannerInfo, CameraHandler cameraHandler);
        public abstract bool Calibrate(float targetRadius2d, float targetRadius3d, float exposure2d, TransformDataList transformDataList);
    }
}
