using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.BarcodeReader;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Devices
{
    /// <summary>
    /// 시스템에 연결된 장치 목록을 관리
    /// </summary>
    public class DeviceManager
    {
        private static DeviceManager instance = null;
        public static DeviceManager Instance()
        {
            if (instance == null)
            {
                instance = new DeviceManager();
            }

            return instance;
        }

        public static void SetInstance(DeviceManager deviceManager)
        {
            instance = deviceManager;
        }

        protected List<Device> deviceList = new List<Device>();
        public List<Device> DeviceList => deviceList;

        protected PortMap portMap = null;
        public PortMap PortMap
        {
            get => portMap;
            set => portMap = value;
        }

        protected DigitalIoHandler digitalIoHandler = null;
        public DigitalIoHandler DigitalIoHandler => digitalIoHandler;

        protected MotionHandler motionHandler = null;
        public MotionHandler MotionHandler => motionHandler;

        protected List<AxisHandler> axisHandlerList = new List<AxisHandler>();
        public List<AxisHandler> AxisHandlerList => axisHandlerList;

        protected GrabberList grabberList = new GrabberList();
        public GrabberList GrabberList => grabberList;

        protected CameraHandler cameraHandler = new CameraHandler();
        public CameraHandler CameraHandler => cameraHandler;

        protected LightCtrlHandler lightCtrlHandler = new LightCtrlHandler();
        public LightCtrlHandler LightCtrlHandler => lightCtrlHandler;

        protected List<DaqChannel> daqChannelList;
        public List<DaqChannel> DaqChannelList => daqChannelList;

        protected IBarcodeReader barcodeReader;
        public IBarcodeReader BarcodeReader => barcodeReader;

        protected ImageAcquisition imageAcquisition;
        public ImageAcquisition ImageAcquisition => imageAcquisition;

        protected AxisHandler robotStage;
        public AxisHandler RobotStage => robotStage;

        protected TowerLamp towerLamp;
        public TowerLamp TowerLamp => towerLamp;

        protected ConveyorSystem conveyorSystem;
        public ConveyorSystem ConveyorSystem => conveyorSystem;

        protected ObjectPool<ImageBuffer> imageBufferPool;
        public ObjectPool<ImageBuffer> ImageBufferPool => imageBufferPool;

        public DeviceManager()
        {
            CreatePortMap();
        }

        public void AddDevice(Device device)
        {
            deviceList.Add(device);
        }

        public void RemoveDevice(Device device)
        {
            deviceList.Remove(device);
        }

        public void UpdateDeviceState(string name, DeviceState state, string stateMessage = "")
        {
            Device device = deviceList.Find(x => x.Name == name);
            if (device != null)
            {
                device.State = state;
                device.StateMessage = stateMessage;
            }
        }

        public bool IsAllDeviceReady()
        {
            foreach (Device device in deviceList)
            {
                if (device.IsReady() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public Device GetDevice(DeviceType deviceType, string name)
        {
            foreach (Device device in deviceList)
            {
                if (device.DeviceType == deviceType && device.Name == name)
                {
                    return device;
                }
            }

            return null;
        }

        public AxisHandler GetAxisHandler(string axisHandlerName)
        {
            return axisHandlerList.Find(x => x.Name == axisHandlerName);
        }

        public List<Device> GetDeviceList(DeviceType deviceType)
        {
            var subDeviceList = new List<Device>();
            foreach (Device device in deviceList)
            {
                if (device.DeviceType == deviceType)
                {
                    subDeviceList.Add(device);
                }
            }

            return subDeviceList;
        }

        protected void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
            {
                reportProgress.ReportProgress(percentage, StringManager.GetString(message));
            }
        }

        public virtual void CreatePortMap()
        {
            portMap = new PortMap();
        }

        public virtual void CreateImageAcquisition()
        {
            imageAcquisition = new ImageAcquisition();
            imageAcquisition.Initialize(cameraHandler, lightCtrlHandler);

            imageBufferPool = new ObjectPool<ImageBuffer>();

            for (int i = 0; i < DeviceConfig.Instance().NumImageBuffer; i++)
            {
                imageBufferPool.PutObject(imageAcquisition.CreateImageBuffer());
            }
        }

        public virtual void Initialize(bool nonVision, IReportProgress reportProgress)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - Initialize Machine");

            DoReportProgress(reportProgress, 15, "Initialize Grabber");

            if (nonVision == false)
            {
                grabberList.Initialize(DeviceConfig.Instance().GrabberInfoList);
                InitializeCamera();
            }

            DoReportProgress(reportProgress, 20, "Initialize Motion");
            InitializeMotion();

            DoReportProgress(reportProgress, 25, "Initialize Digital IO");
            digitalIoHandler = new DigitalIoHandler();
            digitalIoHandler.Initialize(DeviceConfig.Instance().DigitalIoInfoList, motionHandler);

            if (nonVision == false)
            {
                DoReportProgress(reportProgress, 35, "Initialize Light Ctrl");
                InitializeLightCtrl();

                LightConfig.Instance().Initialize();
                LightConfig.Instance().Load();
            }

            DoReportProgress(reportProgress, 50, "Initialize DAQ Device");
            InitializeDaqDevice();

            if (DeviceConfig.Instance().UseBarcodeReader)
            {
                barcodeReader = BarcodeReaderFactory.Create(DeviceConfig.Instance().BarcodeReaderType);
            }

            if (nonVision == false)
            {
                CreateImageAcquisition();
            }

            if (DeviceConfig.Instance().UseConveyorSystem == true)
            {
                CreateConveyorSystem();
            }
        }

        public virtual void InitializeGrabberNLight(bool nonVision, IReportProgress reportProgress)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - InitializeGrabberNLight Machine");

            grabberList.Initialize(DeviceConfig.Instance().GrabberInfoList);
            InitializeCamera();

            InitializeLightCtrl();
            LightConfig.Instance().Initialize();
            LightConfig.Instance().Load();
        }

        private void InitializeCamera()
        {
            foreach (Grabber grabber in grabberList)
            {
                CameraConfiguration cameraConfiguration = grabber.GetCameraConfiguration();
                cameraHandler.AddCamera(grabber, cameraConfiguration);
            }
        }

        public virtual void CreateAxisHandler() { }

        public virtual void CreateTowerLamp(string configDir)
        {
            if (DeviceConfig.Instance().UseTowerLamp)
            {
                towerLamp = new TowerLamp();
                towerLamp.Setup(DigitalIoHandler, DeviceConfig.Instance().TowerLampUpdateInterval);
                towerLamp.SetupPort(PortMap["OutTowerLampRed"], PortMap["OutTowerLampYellow"], PortMap["OutTowerLampGreen"], PortMap["OutTowerBuzzer"]);
                towerLamp.Load(configDir);
                towerLamp.Start();
            }
        }

        public virtual void CreateConveyorSystem()
        {
            if (DeviceConfig.Instance().UseConveyorSystem == false)
            {
                return;
            }

            conveyorSystem = new ConveyorSystem();
            conveyorSystem.UpdatePortNo();
        }

        private void InitializeMotion()
        {
            motionHandler = new MotionHandler();
            motionHandler.Initialize(DeviceConfig.Instance().MotionInfoList, DeviceConfig.Instance().VirtualMode);

            CreateAxisHandler();

            List<AxisHandler> axisHandlerList = DeviceManager.Instance().axisHandlerList;

            if (DeviceConfig.Instance().UseRobotStage == true)
            {
                var robotStage = new AxisHandler("RobotStage");

                axisHandlerList.Add(robotStage);
                this.robotStage = robotStage;
            }

            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                axisHandler.Load(motionHandler);
            }

            foreach (AxisHandler axisHandler in this.axisHandlerList)
            {
                axisHandler.TurnOnServo(true);
            }
        }

        private void InitializeLightCtrl()
        {
            foreach (LightCtrlInfo lightCtrlInfo in DeviceConfig.Instance().LightCtrlInfoList)
            {
                LightCtrl lightCtrl = LightCtrlFactory.Create(lightCtrlInfo, digitalIoHandler, DeviceConfig.Instance().VirtualMode);

                if (lightCtrl != null)
                {
                    if (lightCtrlInfo.Type == LightCtrlType.IO)
                    {
                        var ioLightCtrlInfo = (IoLightCtrlInfo)lightCtrlInfo;
                        List<IoPort> ioPortList = portMap.GetPorts(IoGroup.Light);

                        ioLightCtrlInfo.LightCtrlIoPortList.AddRange(ioPortList);
                    }

                    lightCtrlHandler.AddLightCtrl(lightCtrl);

                    AddDevice(lightCtrl);
                }
            }
        }

        private void InitializeDaqDevice()
        {
            daqChannelList = new List<DaqChannel>();

            foreach (DaqChannelProperty daqChannelProperty in DeviceConfig.Instance().DaqChannelPropertyList)
            {
                DaqChannel daqChannel = DaqChannelManager.Instance().CreateDaqChannel(daqChannelProperty.DaqChannelType, "Daq Channel", DeviceConfig.Instance().VirtualMode);

                if (daqChannel != null)
                {
                    daqChannel.Initialize(daqChannelProperty);
                    daqChannelList.Add(daqChannel);

                    DeviceManager.Instance().AddDevice(daqChannel);
                }
            }
        }

        public void RobotOrigin()
        {
            if (axisHandlerList.Count == 0)
            {
                return;
            }

            var form = new HomeProgressForm();
            form.Show(RobotOriginProc);
        }

        public virtual void Release()
        {
            List<IoPort> portList = portMap.GetPorts(IoDirection.Output);
            foreach (IoPort port in portList)
            {
                try
                {
                    DeviceManager.Instance().DigitalIoHandler.WriteOutput(port, port.ReleaseOn);
                }
                catch (InvalidCastException)
                {
                }
            }

            if (lightCtrlHandler != null)
            {
                lightCtrlHandler.TurnOff();
            }

            if (cameraHandler != null)
            {
                cameraHandler.Release();
            }

            if (grabberList != null)
            {
                grabberList.Release();
            }

            if (digitalIoHandler != null)
            {
                digitalIoHandler.Release();
            }

            imageBufferPool?.ClearObject();
        }

        public virtual void RobotOriginProc()
        {
            var allAxisList = new List<Axis>();
            AxisHandlerList.ForEach(x => allAxisList.AddRange(x.AxisList));
            var allAxisHandler = new AxisHandler("All Axis");
            allAxisHandler.AddAxis(allAxisList);

            allAxisHandler.StopMove();
            Thread.Sleep(100);
            allAxisHandler.HomeMove();
        }
    }
}
