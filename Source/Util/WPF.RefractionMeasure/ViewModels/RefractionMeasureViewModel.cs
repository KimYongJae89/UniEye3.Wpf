using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using WPF.RefractionMeasure.Device;
using WPF.RefractionMeasure.Helper;

namespace WPF.RefractionMeasure.ViewModels
{
    internal class RefractionMeasureViewModel : Observable
    {
        #region Variable

        private Device.Spectrometer spectrometer;

        private Dictionary<string, Device.SpectrometerInfo> deviceList;
        public Dictionary<string, Device.SpectrometerInfo> DeviceList
        {
            get => deviceList;
            set => Set(ref deviceList, value);
        }

        private KeyValuePair<string, Device.SpectrometerInfo> selectedSpectrometer;
        public KeyValuePair<string, Device.SpectrometerInfo> SelectedSpectrometer
        {
            get => selectedSpectrometer;
            set
            {
                Set(ref selectedSpectrometer, value);
                SpectrometerInfo = new ObservableValue<Device.SpectrometerInfo>(selectedSpectrometer.Value);

                spectrometer.ReadSpectrum(Environment.CurrentDirectory, out Dictionary<string, double[]> tempRefData, out Dictionary<string, double[]> tempBgData);
                RefData = tempRefData;
                BgData = tempBgData;
            }
        }

        private ObservableValue<Device.SpectrometerInfo> spectrometerInfo;
        public ObservableValue<Device.SpectrometerInfo> SpectrometerInfo
        {
            get
            {
                if (spectrometer != null && spectrometerInfo != null)
                {
                    spectrometer.UpdateSpectrometer(spectrometerInfo.Value.Name);
                }

                return spectrometerInfo;
            }
            set => Set(ref spectrometerInfo, value);
        }

        private Dictionary<string, double[]> RefData { get; set; }
        private Dictionary<string, double[]> BgData { get; set; }

        private ObservableRangeCollection<PointF> rawData;
        public ObservableRangeCollection<PointF> RawData
        {
            get => rawData;
            set => Set(ref rawData, value);
        }

        private ObservableRangeCollection<PointF> divdiedData;
        public ObservableRangeCollection<PointF> DivdiedData
        {
            get => divdiedData;
            set => Set(ref divdiedData, value);
        }

        private ObservableRangeCollection<PointF> phaseData;
        public ObservableRangeCollection<PointF> PhaseData
        {
            get => phaseData;
            set => Set(ref phaseData, value);
        }

        private ObservableRangeCollection<PointF> refractionData;
        public ObservableRangeCollection<PointF> RefractionData
        {
            get => refractionData;
            set => Set(ref refractionData, value);
        }

        public bool Opened => deviceList.Count > 0 ? true : false;
        public bool Closed => !Opened;

        #endregion

        #region Command

        private ICommand openCommand;
        public ICommand OpenCommand => openCommand ?? (openCommand = new RelayCommand(Open));
        private async void Open()
        {
            int timeOut = 5;
            var source = new ProgressSource();
            source.Step = 1;
            source.Range = timeOut * 20;
            await MessageWindowHelper.ShowProgress("Open Spectrometer", "Now Searching Spectrometer...", new Action(async () =>
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var task = Task.Run(() => spectrometer.Open(), cancellationTokenSource.Token);
                if (SpectrometerTimeOutTask(timeOut * 1000, source) == true)
                {
                    string message = string.Format("Find {0} Spectrometers", spectrometer.DeviceList.Count);
                    await MessageWindowHelper.ShowMessage(this, "Success", message, MessageDialogStyle.Affirmative);
                    spectrometer.ScanDone = ScanDone;
                }
                else
                {
                    string message = string.Format("Can't find Spectrometer");
                    await MessageWindowHelper.ShowMessage(this, "Error", message, MessageDialogStyle.Affirmative);
                    cancellationTokenSource.Cancel();
                }
            }), true, source);
            DeviceList = new Dictionary<string, Device.SpectrometerInfo>();
            DeviceList = spectrometer.DeviceList;
            OnPropertyChanged("Opened");
            OnPropertyChanged("Closed");
        }

        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(Close));
        private async void Close()
        {
            await MessageWindowHelper.ShowProgress("Close Spectrometer", "Now Closing Spectrometer...", new Action(() => spectrometer.Close()), true);
            DeviceList = new Dictionary<string, Device.SpectrometerInfo>();
            DeviceList = spectrometer.DeviceList;
            OnPropertyChanged("Opened");
            OnPropertyChanged("Closed");
        }

        private ICommand referenceCommand;
        public ICommand ReferenceCommand => referenceCommand ?? (referenceCommand = new RelayCommand(Reference));
        private void Reference()
        {
            RefData = spectrometer.SaveRefSpectrum(Environment.CurrentDirectory, 10);
        }

        private ICommand backgroundCommand;
        public ICommand BackgroundCommand => backgroundCommand ?? (backgroundCommand = new RelayCommand(Background));
        private void Background()
        {
            BgData = spectrometer.SaveBgSpectrum(Environment.CurrentDirectory, 10);
        }

        private ICommand baseAngleCommand;
        public ICommand BaseAngleCommand => baseAngleCommand ?? (baseAngleCommand = new RelayCommand(BaseAngle));
        private void BaseAngle()
        {

        }

        private ICommand rotateAngleCommand;
        public ICommand RotateAngleCommand => rotateAngleCommand ?? (rotateAngleCommand = new RelayCommand(RotateAngle));

        private void RotateAngle()
        {

        }

        #endregion

        public RefractionMeasureViewModel()
        {
            spectrometer = new Device.Spectrometer();
            deviceList = new Dictionary<string, Device.SpectrometerInfo>();
        }

        private bool SpectrometerTimeOutTask(int timeOut, ProgressSource source)
        {
            var timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeOut);
            while (spectrometer.NumSpectrometer <= 0)
            {
                if (timeOutTimer.TimeOut)
                {
                    timeOutTimer.Stop();
                    return false;
                }
                Thread.Sleep(50);
                source.StepIt();
            }
            timeOutTimer.Stop();
            return true;
        }

        private void ScanDone(Dictionary<string, double[]> newRaw)
        {
            if (SelectedSpectrometer.Key == null)
            {
                return;
            }

            string key = SelectedSpectrometer.Key;
            Device.SpectrometerInfo info = SelectedSpectrometer.Value;

            var tempRawData = new ObservableRangeCollection<PointF>();
            for (int i = 0; i < newRaw[key].Length; i++)
            {
                tempRawData.Add(new PointF(Convert.ToSingle(info.WaveLengths[i]), Convert.ToSingle(newRaw[key][i])));
            }

            RawData = tempRawData;

            if (RefData != null && BgData != null)
            {
                var tempDividedData = new ObservableRangeCollection<PointF>();
                for (int i = 0; i < newRaw[key].Length; i++)
                {
                    double dividedData = (newRaw[key][i] - BgData[key][i]) / (RefData[key][i] - BgData[key][i]);
                    if (dividedData < 1 && dividedData > 0)
                    {
                        tempDividedData.Add(new PointF(Convert.ToSingle(info.WaveLengths[i]), Convert.ToSingle(dividedData)));
                    }
                }
                DivdiedData = tempDividedData;
            }
        }
    }
}
