using DynMvp.Base;
using DynMvp.Component.DepthSystem;
using DynMvp.Component.DepthSystem.UI;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Daq.UI;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Dio.UI;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.FrameGrabber.UI;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.MotionController.UI;
using DynMvp.Devices.UI;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.UI;

namespace UniEye.Base.Settings.UI
{
    internal enum DeviceListType
    {
        Grabber, Motion, DigitalIo, LightController, Daq
    }

    public partial class ConfigDevicePanel : UserControl
    {
        private string title;
        public string Title
        {
            set => title = value;
        }

        private DeviceListType deviceListType;
        private GrabberInfoList grabberInfoList;
        private MotionInfoList motionInfoList;
        private DigitalIoInfoList digitalIoInfoList;
        private LightCtrlInfoList lightCtrlInfoList;
        private DaqChannelPropertyList daqChannelPropertyList;

        // 현재는 안씀
        //DepthScannerInfoList depthScannerInfoList;
        //DepthScannerConfiguration depthScannerConfiguration;

        public ConfigDevicePanel()
        {
            InitializeComponent();

            buttonSelectGrabber.Text = StringManager.GetString(buttonSelectGrabber.Text);
            buttonSelectMotion.Text = StringManager.GetString(buttonSelectMotion.Text);
            buttonSelectDigitalIo.Text = StringManager.GetString(buttonSelectDigitalIo.Text);
            buttonSelectLightCtrl.Text = StringManager.GetString(buttonSelectLightCtrl.Text);
            buttonSelectDaq.Text = StringManager.GetString(buttonSelectDaq.Text);
            addButton.Text = StringManager.GetString(addButton.Text);
            editButton.Text = StringManager.GetString(editButton.Text);
            deleteButton.Text = StringManager.GetString(deleteButton.Text);
            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            // Load Config.
            // Form_Load Event에서 할 경우 해당 폼이 활성화 되어야만 호출되기 때문에 저장할때 데이터가 없어 null이 되는 문제가 발생한다.
            var deviceConfig = DeviceConfig.Instance();

            grabberInfoList = deviceConfig.GrabberInfoList.Clone();
            motionInfoList = deviceConfig.MotionInfoList.Clone();
            digitalIoInfoList = deviceConfig.DigitalIoInfoList.Clone();
            lightCtrlInfoList = deviceConfig.LightCtrlInfoList.Clone();
            daqChannelPropertyList = deviceConfig.DaqChannelPropertyList.Clone();

            useDoorSensor.Checked = deviceConfig.UseDoorSensor;
            useModelBarcode.Checked = deviceConfig.UseBarcodeReader;

            useRobotStage.Checked = deviceConfig.UseRobotStage;
            useConveyorMotor.Checked = deviceConfig.UseConveyorMotor;
            useConveyorSystem.Checked = deviceConfig.UseConveyorSystem;

            useTowerLamp.Checked = deviceConfig.UseTowerLamp;
            useSoundBuzzer.Checked = deviceConfig.UseSoundBuzzer;
        }

        private void ConfigDevicePanel_Load(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Grabber);
        }


        public void SaveSetting()
        {
            // Panel이 로드되지 않은 상태로 저장하면 전부 null값이 입력되어 다운된다.
            var deviceConfig = DeviceConfig.Instance();
            deviceConfig.GrabberInfoList = grabberInfoList;
            deviceConfig.MotionInfoList = motionInfoList;
            deviceConfig.DigitalIoInfoList = digitalIoInfoList;
            deviceConfig.LightCtrlInfoList = lightCtrlInfoList;
            deviceConfig.DaqChannelPropertyList = daqChannelPropertyList;

            deviceConfig.UseDoorSensor = useDoorSensor.Checked;
            deviceConfig.UseBarcodeReader = useModelBarcode.Checked;

            deviceConfig.UseRobotStage = useRobotStage.Checked;
            deviceConfig.UseConveyorMotor = useConveyorMotor.Checked;
            deviceConfig.UseConveyorSystem = useConveyorSystem.Checked;

            deviceConfig.UseTowerLamp = useTowerLamp.Checked;
            deviceConfig.UseSoundBuzzer = useSoundBuzzer.Checked;
        }



        // 왼쪽 메인 메뉴 버튼 이벤트
        private void buttonSelectGrabber_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Grabber);
        }

        private void buttonSelectMotion_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Motion);
        }

        private void buttonSelectDigitalIo_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.DigitalIo);
        }

        private void buttonSelectLightCtrl_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.LightController);
        }

        private void buttonSelectDaq_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Daq);
        }

        private void buttonMotionContfig_Click(object sender, EventArgs e)
        {
            if (motionInfoList.Count == 0)
            {
                MessageBox.Show("There is no Motion device. Please, add Motion device first.");
                return;
            }

            var motionHandler = new MotionHandler();
            motionHandler.Initialize(motionInfoList, false);
            motionHandler.TurnOnServo(true);

            var axisHandlerNames = new List<string>();
            axisHandlerNames.Add("RobotStage");

            if (DeviceConfig.Instance().UseConveyorMotor)
            {
                axisHandlerNames.Add("Conveyor");
            }

            var axisHandlerList = new List<AxisHandler>();

            foreach (string axisHandlerName in axisHandlerNames)
            {
                axisHandlerList.Add(new AxisHandler(axisHandlerName));
            }

            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                axisHandler.Load(motionHandler);
            }

            var form = new AxisConfigurationForm();

            form.Initialize(axisHandlerList, motionHandler);
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (AxisHandler axisHandler in axisHandlerList)
                {
                    axisHandler.Save(PathConfig.Instance().Result + "\\..\\Config");
                }
            }

            motionHandler.TurnOnServo(false);
            motionHandler.Release();
        }



        private void RefreshList(DeviceListType deviceListType)
        {
            this.deviceListType = deviceListType;
            dataGridViewDeviceList.Rows.Clear();

            panelOption.Controls.Clear();

            switch (deviceListType)
            {
                case DeviceListType.Grabber:
                    RefreshGrabberList();
                    break;
                case DeviceListType.Motion:
                    RefreshMotionList();
                    break;
                case DeviceListType.DigitalIo:
                    RefreshDigitalIoList();
                    break;
                case DeviceListType.LightController:
                    RefreshLightCtrlList();
                    break;
                case DeviceListType.Daq:
                    RefreshDaqChannelList();
                    break;
            }
        }

        // Grabber
        private void RefreshGrabberList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Camera";

            foreach (GrabberInfo grabberInfo in grabberInfoList)
            {
                AddGrabberInfo(grabberInfo);
            }
        }

        private void AddGrabberInfo(GrabberInfo grabberInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), grabberInfo.Name, grabberInfo.Type.ToString(), grabberInfo.NumCamera);
            dataGridViewDeviceList.Rows[index].Tag = grabberInfo;
        }

        // Motion
        private void RefreshMotionList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Axis";

            foreach (MotionInfo motionInfo in motionInfoList)
            {
                AddMotionInfo(motionInfo);
            }
        }

        private void AddMotionInfo(MotionInfo motionInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), motionInfo.Name, motionInfo.Type.ToString(), motionInfo.NumAxis);
            dataGridViewDeviceList.Rows[index].Tag = motionInfo;
        }

        // DIO
        private void RefreshDigitalIoList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Channel";

            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                AddDigitalIoInfo(digitalIoInfo);
            }
        }

        private void AddDigitalIoInfo(DigitalIoInfo digitalIoInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), digitalIoInfo.Name, digitalIoInfo.Type.ToString(), string.Format("I{0} / O{1}", digitalIoInfo.NumInPort, digitalIoInfo.NumOutPort));
            dataGridViewDeviceList.Rows[index].Tag = digitalIoInfo;

        }

        // Light
        private void RefreshLightCtrlList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Light";

            foreach (LightCtrlInfo lightCtrlInfo in lightCtrlInfoList)
            {
                AddLightCtrlInfo(lightCtrlInfo);
            }
        }

        private void AddLightCtrlInfo(LightCtrlInfo lightCtrlInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), lightCtrlInfo.Name, lightCtrlInfo.Type.ToString(), lightCtrlInfo.NumChannel);
            dataGridViewDeviceList.Rows[index].Tag = lightCtrlInfo;
        }

        // Daq
        private void RefreshDaqChannelList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Channel Name";

            foreach (DaqChannelProperty daqChannelProperty in daqChannelPropertyList)
            {
                AddDaqChannel(daqChannelProperty);
            }
        }

        private void AddDaqChannel(DaqChannelProperty daqChannelProperty)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), daqChannelProperty.ChannelName, daqChannelProperty.DaqChannelType.ToString(), daqChannelProperty.ChannelName);
            dataGridViewDeviceList.Rows[index].Tag = daqChannelProperty;
        }

        // 상단 버튼 이벤트
        private void addButton_Click(object sender, EventArgs e)
        {
            switch (deviceListType)
            {
                case DeviceListType.Grabber:
                    {
                        var form = new NewGrabberForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            var grabberInfo = new GrabberInfo(form.GrabberName, form.GrabberType, form.NumCamera);
                            grabberInfoList.Add(grabberInfo);
                            AddGrabberInfo(grabberInfo);

                            var cameraConfiguration = new CameraConfiguration();
                            for (int i = 0; i < form.NumCamera; i++)
                            {
                                cameraConfiguration.AddCameraInfo(CameraInfo.Create(grabberInfo.Type));
                            }

                            string filePath = string.Format("{0}\\CameraConfiguration_{1}.xml", BaseConfig.Instance().ConfigPath, grabberInfo.Name);
                            cameraConfiguration.SaveCameraConfiguration(filePath);
                        }
                    }
                    break;
                case DeviceListType.Motion:
                    {
                        var form = new NewMotionForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(form.MotionType);
                            motionInfo.Name = form.MotionName;
                            motionInfoList.Add(motionInfo);
                            AddMotionInfo(motionInfo);
                        }
                    }
                    break;
                case DeviceListType.DigitalIo:
                    {
                        var form = new NewDigitalIoForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            digitalIoInfoList.Add(form.DigitalIoInfo);
                            AddDigitalIoInfo(form.DigitalIoInfo);
                        }
                    }
                    break;
                case DeviceListType.LightController:
                    {
                        var digitalIoHandler = new DigitalIoHandler();
                        digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

                        var form = new LightConfigForm();
                        form.LightCtrlName = string.Format("Light {0}", dataGridViewDeviceList.RowCount + 1);
                        form.DigitalIoHandler = digitalIoHandler;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            lightCtrlInfoList.Add(form.LightCtrlInfo);
                            AddLightCtrlInfo(form.LightCtrlInfo);
                        }
                    }
                    break;
                case DeviceListType.Daq:
                    {
                        var form = new NewDaqChannelForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DaqChannelProperty daqChannelProperty = DaqChannelPropertyFactory.Create(form.DaqChannelType);
                            daqChannelProperty.Name = form.DaqChannelName;
                            daqChannelPropertyList.Add(daqChannelProperty);
                            AddDaqChannel(daqChannelProperty);
                        }
                    }
                    break;
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewDeviceList.SelectedRows.Count == 0)
            {
                return;
            }

            object editTarget = dataGridViewDeviceList.SelectedRows[0].Tag;

            switch (deviceListType)
            {
                case DeviceListType.Grabber:
                    EditGrabber((GrabberInfo)editTarget);
                    break;
                case DeviceListType.Motion:
                    EditMotionInfo((MotionInfo)editTarget);
                    break;
                case DeviceListType.DigitalIo:
                    EditDigitalIoInfo((DigitalIoInfo)editTarget);
                    break;
                case DeviceListType.LightController:
                    EditLightCtrlInfo((LightCtrlInfo)editTarget);
                    break;
                case DeviceListType.Daq:
                    {
                        var form = new DaqPropertyForm();
                        form.DaqChannelProperty = (DaqChannelProperty)editTarget;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = form.DaqChannelProperty.Name;
                        }
                    }
                    break;
            }
        }

        private void EditGrabber(GrabberInfo grabberInfo)
        {
            Grabber grabber = GrabberFactory.Create(grabberInfo);

            var cameraConfiguration = new CameraConfiguration();
            string filePath = string.Format("{0}\\CameraConfiguration_{1}.xml", BaseConfig.Instance().ConfigPath, grabberInfo.Name);
            if (File.Exists(filePath) == true)
            {
                cameraConfiguration.LoadCameraConfiguration(filePath);
            }

            if (grabber.SetupCameraConfiguration(grabberInfo.NumCamera, cameraConfiguration) == true)
            {
                grabberInfo.NumCamera = cameraConfiguration.CameraInfoList.Count;
                dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = grabberInfo.NumCamera.ToString();
            }

            if (grabberInfo.NumCamera > 0 && cameraConfiguration.CameraInfoList.Count < grabberInfo.NumCamera)
            {
                MessageBox.Show("The number of camera is less then required number of camera");
                return;
            }

            cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private void EditMotionInfo(MotionInfo motionInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;

            if (motionInfo is PciMotionInfo)
            {
                var form = new PciMotionInfoForm();
                form.PciMotionInfo = (PciMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is NetworkMotionInfo)
            {
                var form = new NetworkMotionInfoForm();
                form.NetworkMotionInfo = (NetworkMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is SerialMotionInfo)
            {
                var form = new SerialMotionInfoForm();
                form.SerialMotionInfo = (SerialMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is VirtualMotionInfo)
            {
                var form = new VirtualMotionInfoForm();
                form.VirtualMotionInfo = (VirtualMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }

            if (dialogResult == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = motionInfo.Name;
                dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = motionInfo.NumAxis;
            }
        }

        private void EditDigitalIoInfo(DigitalIoInfo digitalIoInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;
            if (digitalIoInfo is PciDigitalIoInfo)
            {
                var form = new PciDigitalIoInfoForm();
                form.PciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;
                dialogResult = form.ShowDialog();
            }
            else if (digitalIoInfo is SlaveDigitalIoInfo)
            {
                var form = new SlaveDigitalIoInfoForm();
                form.SlaveDigitalIoInfo = (SlaveDigitalIoInfo)digitalIoInfo;
                form.MotionInfoList = motionInfoList;
                dialogResult = form.ShowDialog();
            }

            if (dialogResult == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = digitalIoInfo.Name;
            }
        }

        private void EditDaqChannelProperty(DaqChannelProperty daqChannelProperty)
        {
            var form = new DaqPropertyForm();
            form.DaqChannelProperty = daqChannelProperty;
            if (form.ShowDialog() == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = daqChannelProperty.Name;
            }
        }

        private void EditLightCtrlInfo(LightCtrlInfo lightCtrlInfo)
        {
            var digitalIoHandler = new DigitalIoHandler();
            digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

            var form = new LightConfigForm();
            form.LightCtrlInfo = lightCtrlInfo;
            form.DigitalIoHandler = digitalIoHandler;
            if (form.ShowDialog() == DialogResult.OK)
            {
                lightCtrlInfoList.Remove(lightCtrlInfo);

                lightCtrlInfo = form.LightCtrlInfo;

                lightCtrlInfoList.Add(lightCtrlInfo);

                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = lightCtrlInfo.Name;
                dataGridViewDeviceList.SelectedRows[0].Cells[2].Value = lightCtrlInfo.Type.ToString();
                dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = lightCtrlInfo.NumChannel.ToString();
                dataGridViewDeviceList.SelectedRows[0].Tag = lightCtrlInfo;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewDeviceList.SelectedRows.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Do you want to delete the selected device", title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                object editTarget = dataGridViewDeviceList.SelectedRows[0].Tag;

                dataGridViewDeviceList.Rows.Remove(dataGridViewDeviceList.SelectedRows[0]);

                // 중간 Row 삭제하면 No. 변경되게 처리
                foreach (DataGridViewRow row in dataGridViewDeviceList.Rows)
                {
                    row.Cells[0].Value = row.Index + 1;
                }

                switch (deviceListType)
                {
                    case DeviceListType.Grabber:
                        grabberInfoList.Remove((GrabberInfo)editTarget);
                        break;
                    case DeviceListType.Motion:
                        motionInfoList.Remove((MotionInfo)editTarget);
                        break;
                    case DeviceListType.DigitalIo:
                        digitalIoInfoList.Remove((DigitalIoInfo)editTarget);
                        break;
                    case DeviceListType.LightController:
                        lightCtrlInfoList.Remove((LightCtrlInfo)editTarget);
                        break;
                    case DeviceListType.Daq:
                        daqChannelPropertyList.Remove((DaqChannelProperty)editTarget);
                        break;
                }
            }
        }

        // Grid
        private void dataGridViewDeviceList_SelectionChanged(object sender, EventArgs e)
        {
            bool enable = (dataGridViewDeviceList.SelectedRows.Count > 0);

            editButton.Enabled = enable;
            deleteButton.Enabled = enable;
            buttonMoveUp.Enabled = enable;
            buttonMoveDown.Enabled = enable;
        }

        // 우측 버튼 이벤트
        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(dataGridViewDeviceList);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(dataGridViewDeviceList);
        }
    }
}
