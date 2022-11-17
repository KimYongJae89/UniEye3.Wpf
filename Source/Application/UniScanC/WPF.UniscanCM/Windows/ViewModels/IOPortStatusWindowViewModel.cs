using DynMvp.Devices.Dio;
using MahApps.Metro.SimpleChildWindow;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Controls.Models;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class IOPortStatusWindowViewModel : Observable
    {
        public ObservableCollection<IOPortStatusModel> IOInputPortCollection { get; set; } = new ObservableCollection<IOPortStatusModel>();
        public ObservableCollection<IOPortStatusModel> IOOutputPortCollection { get; set; } = new ObservableCollection<IOPortStatusModel>();
        public int SelectedRowIndex { get; set; }

        public ICommand SwitchCheckChangedCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        private Timer timer = new Timer();

        public IOPortStatusWindowViewModel()
        {
            PortMap portMap = DeviceManager.Instance().PortMap;

            foreach (IoPort port in portMap.GetPorts(IoDirection.Input))
            {
                IOInputPortCollection.Add(new IOPortStatusModel(port.PortNo, port.Name));
            }

            foreach (IoPort port in portMap.GetPorts(IoDirection.Output))
            {
                IOOutputPortCollection.Add(new IOPortStatusModel(port.PortNo, port.Name));
            }

            timer.Interval = 200;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            SwitchCheckChangedCommand = new RelayCommand(() =>
            {
                IoPort ioPort = DeviceManager.Instance().PortMap.GetPort(IOOutputPortCollection[SelectedRowIndex].PortName);
                if (ioPort != null)
                {
                    bool ioPortStatus = DeviceManager.Instance().DigitalIoHandler.ReadOutput(ioPort);
                    DeviceManager.Instance().DigitalIoHandler.WriteOutput(ioPort, !ioPortStatus);
                }
            });

            OkCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close());

            CancelCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close());
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PortMap portMap = DeviceManager.Instance().PortMap;

            foreach (IOPortStatusModel port in IOInputPortCollection)
            {
                IoPort ioPort = portMap.GetPort(port.PortName);
                if (ioPort != null)
                {
                    port.PortStatus = DeviceManager.Instance().DigitalIoHandler.ReadInput(ioPort);
                }
            }

            foreach (IOPortStatusModel port in IOOutputPortCollection)
            {
                IoPort ioPort = portMap.GetPort(port.PortName);
                if (ioPort != null)
                {
                    port.PortStatus = DeviceManager.Instance().DigitalIoHandler.ReadOutput(ioPort);
                }
            }
        }
    }
}
