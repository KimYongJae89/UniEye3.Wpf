using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Data;
using WPF.UniScanIM.Override;
using WPF.UniScanIM.Views;
using WPF.UniScanIM.Windows.ViewModels;
using WPF.UniScanIM.Windows.Views;

namespace WPF.UniScanIM.ViewModels
{
    public delegate void BoolValueDelegate(bool value);

    public class TopViewModel : Observable
    {
        public BoolValueDelegate FlyoutOpenChanged { get; set; }

        public ObservableCollection<ModuleInfo> ModuleList => SystemConfig.Instance.ModuleList;

        private ModuleInfo selectedModuleInfo;
        public ModuleInfo SelectedModuleInfo
        {
            get => selectedModuleInfo;
            set
            {
                Set(ref selectedModuleInfo, value);
                SystemConfig.Instance.SelectedModuleInfo = value;
            }
        }

        private bool flyoutOpen;
        public bool FlyoutOpen
        {
            get => flyoutOpen;
            set
            {
                if (Set(ref flyoutOpen, value))
                {
                    FlyoutOpenChanged?.BeginInvoke(value, null, null);
                }
            }
        }

        private ICommand grabCommand;
        public ICommand GrabCommand => grabCommand ?? (grabCommand = new RelayCommand<bool>(GrabCommandAction));

        private ICommand inspectCommand;
        public ICommand InspectCommand => inspectCommand ?? (inspectCommand = new RelayCommand<bool>(InspectCommandAction));

        private ICommand updateCommand;
        public ICommand UpdateCommand => updateCommand ?? (updateCommand = new RelayCommand<bool>(UpdateCommandAction));

        private ICommand taskCommand;
        public ICommand TaskCommand => taskCommand ?? (taskCommand = new RelayCommand(TaskCommandAction));

        private ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new RelayCommand(SaveCommandAction));

        private ICommand exitCommand;
        public ICommand ExitCommand => exitCommand ?? (exitCommand = new RelayCommand(ExitCommandAction));

        public TopViewModel()
        {
            SystemConfig.Instance.FlyoutSettingViewModelChanged = false;
            SelectedModuleInfo = ModuleList.FirstOrDefault();
        }

        private void GrabCommandAction(bool isChecked)
        {
            var inspectRunner = SystemManager.Instance().InspectRunner as IMInspectRunner;

            if (isChecked)
            {
                inspectRunner.StartGrab(-1);
            }
            else
            {
                inspectRunner.StopGrab(-1);
            }
        }

        private void InspectCommandAction(bool isChecked)
        {
            var inspectRunner = SystemManager.Instance().InspectRunner as IMInspectRunner;

            bool done = false;

            string lotNo = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string resultImagePath = Path.Combine(UniEye.Base.Config.PathConfig.Instance().Result, lotNo);
            Directory.CreateDirectory(resultImagePath);

            try
            {
                //if (inspectRunner.VisionModels == null)
                {
                    inspectRunner.VisionModels = new List<UniScanC.Models.VisionModel>();
                    var visionModel = new UniScanC.Models.VisionModel()
                    {
                        CalibrateFrameCnt = 1,
                        SkipFirstImage = false,
                        GpuProcessing = true,

                        ThresholdDark = 80,
                        ThresholdLight = 50,
                        ThresholdSize = 20,

                        FrameMarginT = 0,
                        FrameMarginB = 0,

                        OutTargetIntensity = 255,
                        TargetIntensity = 127,
                    };
                    inspectRunner.VisionModels.Add(visionModel);
                }

                // 1sec: 9m/m -> 0.15m/s -> 150000 um/s -> 5000 lines/s @ 30um/px
                inspectRunner.Initialize("UniScan", Path.GetFullPath(resultImagePath), lotNo, 9.0);

                // 1sec: 7.2m/m -> 0.12m/s -> 120 mm/s -> 3000 lines/s @ 40um/px
                inspectRunner.Initialize("UniScan", Path.GetFullPath(resultImagePath), lotNo, 7.2);

                inspectRunner.EnterWaitInspection();
                done = true;

            }
            catch (Exception ex)
            {
                do
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                } while ((ex = ex.InnerException) != null);
                done = false;
            }

            if (!done)
            {
                inspectRunner.ExitWaitInspection();
            }
        }

        private void UpdateCommandAction(bool isChecked)
        {
            var inspectRunner = SystemManager.Instance().InspectRunner as IMInspectRunner;
            inspectRunner.UpdateImageView = isChecked;
        }

        private async void TaskCommandAction()
        {
            var viewModel = new TaskMonitoringWindowViewModel();
            var view = new TaskMonitoringWindowView();
            view.DataContext = viewModel;
            await MessageWindowHelper.ShowChildWindow<bool>(view);
        }

        private void SaveCommandAction()
        {
            var dlg = new System.Windows.Forms.SaveFileDialog
            {
                AddExtension = true,
                Filter = "PNG|*.png|BMP|*.bmp|JPG|*.jpg"
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    SystemConfig.Instance.SaveSelectedSourceImage?.Invoke(SelectedModuleInfo, dlg.FileName);
                    MessageBox.Show("Save Complete");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name);
                }
                //using (var fileStream = new FileStream(dlg.FileName, FileMode.Create))
                //{
                //    var bitmapImage = ImageSource as BitmapSource;

                //    BitmapEncoder encoder;
                //    switch (System.IO.Path.GetExtension(dlg.FileName).ToUpper())
                //    {
                //        case ".JPG":
                //        case ".JPEG":
                //            encoder = new JpegBitmapEncoder();
                //            ((JpegBitmapEncoder)encoder).QualityLevel = 100;
                //            break;
                //        case ".PNG":
                //            encoder = new PngBitmapEncoder();
                //            break;
                //        case ".BMP":
                //        default:
                //            encoder = new BmpBitmapEncoder();
                //            break;
                //    }

                //    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                //    encoder.Save(fileStream);
                //}
            }
        }

        private async void ExitCommandAction()
        {
            if (await CloseProgram())
            {
                var inspectRunner = SystemManager.Instance().InspectRunner as IMInspectRunner;
                SystemManager.Instance().InspectRunner.ExitWaitInspection();
                Application.Current.Shutdown(0);
            }
        }

        private async Task<bool> CloseProgram()
        {
            bool result = await MessageWindowHelper.ShowMessageBox("Exit Program", "Do you want to exit the program?", MessageBoxButton.OKCancel);
            return result;
        }
    }
}