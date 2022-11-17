using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Controls.Models;
using UniScanC.Controls.Views;

namespace UniScanC.Controls.ViewModels
{
    public class IOPortStatusControlViewModel : CustomizeControlViewModel
    {
        #region 생성자
        public IOPortStatusControlViewModel() : base(typeof(IOPortStatusControlViewModel))
        {
            SwitchCheckChangedCommand = new RelayCommand(SwitchCheckChangedCommandAction);

            PortMap portMap = DeviceManager.PortMap;

            foreach (IoPort port in portMap.GetPorts(IoDirection.Input))
            {
                if (port.Group == IoGroup.General)
                {
                    IOInputPortCollection.Add(new IOPortStatusModel(port.PortNo, port.Description));
                }
            }

            foreach (IoPort port in portMap.GetPorts(IoDirection.Output))
            {
                if (port.Group == IoGroup.General)
                {
                    IOOutputPortCollection.Add(new IOPortStatusModel(port.PortNo, port.Description));
                }
            }

            timer.Interval = 200;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void SwitchCheckChangedCommandAction()
        {
            if (SelectedRowIndex >= IOOutputPortCollection.Count)
            {
                return;
            }

            IoPort ioPort = DeviceManager.Instance().PortMap.GetPort(IOOutputPortCollection[SelectedRowIndex].PortName);
            if (ioPort != null)
            {
                bool ioPortStatus = DeviceManager.Instance().DigitalIoHandler.ReadOutput(ioPort);
                DeviceManager.Instance().DigitalIoHandler.WriteOutput(ioPort, !ioPortStatus);
            }
        }
        #endregion


        #region 속성(LayoutControlViewModel)
        #endregion


        #region 속성
        public ICommand SwitchCheckChangedCommand { get; }

        private Timer timer = new Timer();

        private DeviceManager DeviceManager => DeviceManager.Instance();

        public ObservableCollection<IOPortStatusModel> IOInputPortCollection { get; set; } = new ObservableCollection<IOPortStatusModel>();

        public ObservableCollection<IOPortStatusModel> IOOutputPortCollection { get; set; } = new ObservableCollection<IOPortStatusModel>();

        public int SelectedRowIndex { get; set; }
        #endregion


        #region 메서드
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DeviceManager.DigitalIoHandler == null)
            {
                return;
            }

            PortMap portMap = DeviceManager.Instance().PortMap;
            foreach (IOPortStatusModel port in IOInputPortCollection)
            {
                IoPort ioPort = portMap.GetPort(port.PortName);
                if (ioPort != null)
                {
                    port.PortStatus = DeviceManager.DigitalIoHandler.ReadInput(ioPort);
                }
            }

            foreach (IOPortStatusModel port in IOOutputPortCollection)
            {
                IoPort ioPort = portMap.GetPort(port.PortName);
                if (ioPort != null)
                {
                    port.PortStatus = DeviceManager.DigitalIoHandler.ReadOutput(ioPort);
                }
            }
        }

        public override UserControl CreateControlView()
        {
            return new IOPortStatusControlView();
        }
        #endregion
    }
}
