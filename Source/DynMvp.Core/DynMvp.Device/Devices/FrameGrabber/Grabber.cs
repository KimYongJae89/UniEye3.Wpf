using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public enum GrabberType
    {
        Virtual,
        Pylon,
        Pylon2,
        MultiCam,
        uEye,
        MIL,
        GenTL,
        Sapera,
        PylonLine,
        MILCXP
    }

    public class GrabberInfo
    {
        public string Name { get; set; }
        public GrabberType Type { get; set; }
        public int NumCamera { get; set; }
        public CameraConfiguration CameraConfiguration { get; set; }

        public GrabberInfo()
        {

        }

        public GrabberInfo(string name, GrabberType type)
        {
            Name = name;
            Type = type;
        }

        public GrabberInfo(string name, GrabberType type, int numCamera)
        {
            Name = name;
            Type = type;
            NumCamera = numCamera;
        }

        public void LoadXml(XmlElement grabberElement)
        {
            Name = XmlHelper.GetValue(grabberElement, "Name", "");
            Type = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(grabberElement, "Type", "Pylon"));
            NumCamera = Convert.ToInt32(XmlHelper.GetValue(grabberElement, "NumCamera", ""));
        }

        public void SaveXml(XmlElement grabberElement)
        {
            XmlHelper.SetValue(grabberElement, "Name", Name.ToString());
            XmlHelper.SetValue(grabberElement, "Type", Type.ToString());
            XmlHelper.SetValue(grabberElement, "NumCamera", NumCamera.ToString());
        }

        public GrabberInfo Clone()
        {
            var grabberInfo = new GrabberInfo();
            grabberInfo.Copy(this);

            return grabberInfo;
        }

        public virtual void Copy(GrabberInfo srcGrabberInfo)
        {
            Name = srcGrabberInfo.Name;
            Type = srcGrabberInfo.Type;
            NumCamera = srcGrabberInfo.NumCamera;
        }
    }

    public class GrabberInfoList : List<GrabberInfo>
    {
        public int GetNumCamera()
        {
            int numCamera = 0;
            foreach (GrabberInfo grabberInfo in this)
            {
                numCamera += grabberInfo.NumCamera;
            }

            return numCamera;
        }

        public GrabberInfoList Clone()
        {
            var newGrabberInfoList = new GrabberInfoList();

            foreach (GrabberInfo grabberInfo in this)
            {
                newGrabberInfoList.Add(grabberInfo.Clone());
            }

            return newGrabberInfoList;
        }
    }


    public abstract class Grabber : Device
    {
        public GrabberType Type { get; set; }

        public GrabberInfo GrabberInfo { get; private set; }

        public Grabber(GrabberType grabberType, GrabberInfo grabberInfo)
        {
            Name = grabberInfo.Name;
            if (string.IsNullOrEmpty(Name))
            {
                Name = grabberType.ToString();
            }

            DeviceType = DeviceType.FrameGrabber;
            Type = grabberType;

            GrabberInfo = grabberInfo;
            UpdateState(DeviceState.Idle, "Created");
        }

        public abstract Camera CreateCamera();

        public abstract bool Initialize();
        public abstract void UpdateCameraInfo(CameraInfo cameraInfo);

        public abstract bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration);

        public CameraConfiguration GetCameraConfiguration()
        {
            Debug.Assert(GrabberInfo != null);

            if (GrabberInfo.CameraConfiguration != null)
            {
                return GrabberInfo.CameraConfiguration;
            }

            var cameraConfiguration = new CameraConfiguration();

            string filePath = string.Format("{0}\\CameraConfiguration_{1}.xml", BaseConfig.Instance().ConfigPath, Name);
            if (cameraConfiguration.LoadCameraConfiguration(filePath) == false)
            {
                MessageForm.Show(null, "Can't load camera configuration file. Please, set up the camera first");
            }

            GrabberInfo.CameraConfiguration = cameraConfiguration;

            return cameraConfiguration;
        }
    }

    public class GrabberList : List<Grabber>
    {
        public Grabber GetGrabber(string name)
        {
            return Find(x => x.Name == name);
        }

        public Grabber GetGrabber(GrabberType grabberType)
        {
            return Find(x => x.Type == grabberType);
        }

        public void Release()
        {
            foreach (Grabber grabber in this)
            {
                grabber.Release();
            }
        }

        public void Initialize(GrabberInfoList grabberInfoList)
        {
            foreach (GrabberInfo grabberInfo in grabberInfoList)
            {
                Grabber grabber = GrabberFactory.Create(grabberInfo);
                if (grabber != null)
                {
                    bool done = grabber.Initialize();
                    if (!done)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                            ErrorSection.Grabber.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Can't initialize grabber. {0}", grabberInfo.Type.ToString()));

                        grabber.UpdateState(DeviceState.Error, "Grabber is invalid.");
                    }
                    else
                    {
                        grabber.UpdateState(DeviceState.Ready, "Grabber initialization succeeded.");
                    }

                    Add(grabber);
                }
            }
        }
    }
}
