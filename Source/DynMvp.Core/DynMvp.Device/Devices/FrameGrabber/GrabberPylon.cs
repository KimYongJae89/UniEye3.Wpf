using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoPylon : CameraInfo
    {
        public string DeviceUserId { get; set; }
        public string IpAddress { get; set; }
        public string SerialNo { get; set; }
        public uint DeviceIndex { get; set; }
        public string ModelName { get; set; }

        public CameraInfoPylon()
        {
            GrabberType = GrabberType.Pylon;

            DeviceUserId = "";
            IpAddress = "";
            SerialNo = "";
            DeviceIndex = 0;
            ModelName = "";
        }

        public CameraInfoPylon(string deviceUserId, string ipAddress, string serialNo)
        {
            GrabberType = GrabberType.Pylon;

            IpAddress = ipAddress;
            SerialNo = serialNo;
            DeviceUserId = deviceUserId;
            DeviceIndex = 0;
            ModelName = "";
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            DeviceUserId = XmlHelper.GetValue(cameraElement, "DeviceUserId", "");
            IpAddress = XmlHelper.GetValue(cameraElement, "IpAddress", "");
            SerialNo = XmlHelper.GetValue(cameraElement, "SerialNo", "");
            ModelName = XmlHelper.GetValue(cameraElement, "ModelName", "");
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "DeviceUserId", DeviceUserId);
            XmlHelper.SetValue(cameraElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(cameraElement, "SerialNo", SerialNo);
            XmlHelper.SetValue(cameraElement, "ModelName", ModelName);
        }
    }

    internal class InvalidDeviceListException : ApplicationException
    {
    }

    internal class InvalidIpAddressException : ApplicationException
    {
    }

    public class GrabberPylon : Grabber
    {
        private List<PylonC.NETSupportLibrary.DeviceEnumerator.Device> deviceList;

        public GrabberPylon(GrabberInfo grabberInfo) : base(GrabberType.Pylon, grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Pylon Device Handler Created");
        }

        public override Camera CreateCamera()
        {
            return new CameraPylon();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new PylonCameraListForm();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public static void GetFeature(string featureFullString, out string deviceUserId, out string ipAddress, out string serialNo, out string modelName)
        {
            deviceUserId = "";
            ipAddress = "";
            serialNo = "";
            modelName = "";

            string[] features = featureFullString.Split('\n');
            foreach (string feature in features)
            {
                string[] tokens = feature.Split(':');

                string keyName = tokens[0].Replace(" ", "");
                if (keyName == "SerialNumber")
                {
                    serialNo = tokens[1].Trim();
                }
                else if (keyName == "IpAddress")
                {
                    ipAddress = tokens[1].Trim();
                }
                else if (keyName == "UserDefinedName")
                {
                    deviceUserId = tokens[1].Trim();
                }
                else if (keyName == "ModelName")
                {
                    modelName = tokens[1].Trim();
                }
            }
        }

        private PylonC.NETSupportLibrary.DeviceEnumerator.Device GetDevice(CameraInfoPylon cameraInfo)
        {
            if (deviceList == null)
            {
                throw new InvalidDeviceListException();
            }

            foreach (PylonC.NETSupportLibrary.DeviceEnumerator.Device device in deviceList)
            {
                GetFeature(device.Tooltip, out string deviceUserId, out string ipAddress, out string serialNo, out string modelName);

                if (string.IsNullOrEmpty(deviceUserId) == false && cameraInfo.DeviceUserId == deviceUserId)
                {
                    return device;
                }
                else if (string.IsNullOrEmpty(ipAddress) == false && cameraInfo.IpAddress == ipAddress)
                {
                    return device;
                }
                else if (string.IsNullOrEmpty(serialNo) == false && cameraInfo.SerialNo == serialNo)
                {
                    return device;
                }
            }

            return null;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialzie camera(s)");

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000");

            PylonC.NET.Pylon.Initialize();

            deviceList = PylonC.NETSupportLibrary.DeviceEnumerator.EnumerateDevices();

            return true;
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoPylon) == false)
            {
                return;
            }

            var cameraInfoPylon = (CameraInfoPylon)cameraInfo;
            try
            {
                PylonC.NETSupportLibrary.DeviceEnumerator.Device pylonDevice = GetDevice(cameraInfoPylon);
                if (pylonDevice == null)
                {
                    string messge = string.Format("Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}", cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);

                    MessageBox.Show(messge);
                    LogHelper.Error(messge);

                    cameraInfoPylon.DeviceIndex = 0;
                    cameraInfoPylon.Enabled = false;

                    return;
                }

                cameraInfoPylon.DeviceIndex = pylonDevice.Index;
            }
            catch (InvalidIpAddressException)
            {
                string messge = string.Format("Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}", cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);

                MessageBox.Show(messge);
                LogHelper.Error(messge);

                cameraInfoPylon.DeviceIndex = 0;
                cameraInfoPylon.Enabled = false;
            }
        }

        public override void Release()
        {
            base.Release();
            PylonC.NET.Pylon.Terminate();
        }
    }
}
