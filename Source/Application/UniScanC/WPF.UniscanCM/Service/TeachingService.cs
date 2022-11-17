using ControlzEx.Standard;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Extensions;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.Config;
using UniEye.Translation.Helpers;
using UniScanC.Enums;
using UniScanC.Models;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Windows.ViewModels;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Service
{
    public class TeachingService
    {
        public delegate void UpdateImageDelegate(string imName, BitmapSource source);

        public UpdateImageDelegate OnUpdateImage { get; set; }

        // 모델을 오픈할 때 마다 복사해서 가지고 있는 모델
        // Model를 복사해서 열어 놓는다.
        // 파라미터를 바꾸고 싶지 않은 경우 저장을 하지 않고 빠져 나올 수 있도록
        public UniScanC.Models.Model Model { get; set; }

        public VisionModel VisionModel { get; set; }

        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;

        private CommManager CommManager => CommManager.Instance() as CommManager;

        private SystemConfig SystemConfig => SystemConfig.Instance;

        private SystemManager SystemManager => SystemManager.Instance() as SystemManager;

        private bool OnGrab { get; set; } = false;

        private Timer GetImageTimer { get; set; }

        public TeachingService()
        {
            GetImageTimer = new Timer();
            GetImageTimer.Interval = 1000;
            GetImageTimer.Elapsed += GetImageTimer_Elapsed;
        }

        private void GetImageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer)sender;
            timer.Stop();
            GetIMTeachingImage("ModuleGrab", ImageFormat.Png, true);
            timer.Start();
        }

        private void GetIMTeachingImage(string fileName, ImageFormat imageFormat, bool checkLockFile = false)
        {
            string rootPath = Path.GetFullPath(SystemConfig.ResultPath);
            foreach (UniScanC.Module.InspectModuleInfo module in SystemConfig.ImModuleList)
            {
                string imageFIle = Path.Combine(rootPath, $"{fileName}_{module.ModuleNo}.{imageFormat}");
                string lockFilePath = imageFIle.Remove(imageFIle.Length - Path.GetExtension(imageFIle).Length, Path.GetExtension(imageFIle).Length);

                if (checkLockFile)
                {
                    // Lock 파일이 있는지 확인. 없으면 리턴
                    if (!File.Exists(lockFilePath))
                    {
                        continue;
                    }
                }
                if (!File.Exists(imageFIle))
                {
                    continue;
                }

                // 이 방법으로 하면 이미지를 잡고 있는 것으로 인식됨
                //BitmapImage source = new BitmapImage();
                //source.BeginInit();
                //source.UriSource = new Uri(imageFIle, UriKind.RelativeOrAbsolute);
                //source.EndInit();
                //source.Freeze();

                //// 이미지를 Canvas에 넘겨줌
                //OnUpdateImage(module.ModuleTopic, source);

                // 아래 방법으로 했을 때 원본 컬러 이미지에서 색이 이상하게 불러와졌음
                using (var bitmap = ImageHelper.LoadImage(imageFIle) as System.Drawing.Bitmap)
                {
                    if (bitmap != null)
                    {
                        BitmapSource source = ImageHelper.BitmapToBitmapSource(bitmap);
                        // 이미지를 Canvas에 넘겨줌
                        OnUpdateImage(module.ModuleTopic, source);
                    }
                }

                File.Delete(lockFilePath);
            }
        }

        public void SetLight(bool isOn, int topLightValue = -1, int bottomLightValue = -1)
        {
            if (isOn)
            {
                if (topLightValue == -1)
                {
                    DeviceManager.TopLightOn(VisionModel.TopLightValue);
                }
                else
                {
                    DeviceManager.TopLightOn(topLightValue);
                }

                if (topLightValue == -1)
                {
                    DeviceManager.BottomLightOn(VisionModel.BottomLightValue);
                }
                else
                {
                    DeviceManager.BottomLightOn(bottomLightValue);
                }
            }
            else
            {
                DeviceManager.TopLightOff();
                DeviceManager.BottomLightOff();
            }
        }

        public async Task Inspect()
        {
            if (await CommManager.ExecuteCommand(EUniScanCCommand.TeachInspect, ""))
            {
                GetIMTeachingImage("ModuleResult", ImageFormat.Png);
            }
        }

        public async Task<bool> Grab()
        {
            if (await CommManager.ExecuteCommand(EUniScanCCommand.TeachGrab, OnGrab.ToString()))
            {
                if (!OnGrab)
                {
                    if (!VisionModel.UseTDILight)
                    {
                        DeviceManager.TopLightOn(VisionModel.TopLightValue);
                        DeviceManager.BottomLightOn(VisionModel.BottomLightValue);
                    }
                    else
                    {
                        float targetSpeed = SystemConfig.Instance.TargetSpeed;
                        if (DeviceManager.PLCMachineIf != null)
                        {
                            targetSpeed = DeviceManager.PLCMachineIf.PreviousMachineIfData.GET_TARGET_SPEED / 10.0f;
                        }

                        if (targetSpeed > 0)
                        {
                            // TDI 카메라의 경우 설정한 조명 값을 사용하여 비례하는 조명 값으로 설정 해준다.
                            if (CMLightCalibrationService.CalTDILightValue(VisionModel, targetSpeed, out int TopLightValue, out int BottomLightValue))
                            {
                                DeviceManager.TopLightOn(TopLightValue);
                                DeviceManager.BottomLightOn(BottomLightValue);
                            }
                            else
                            {
                                ErrorManager.Instance().Report((int)ErrorSection.Process, ErrorLevel.Warning, ErrorSection.Process.ToString(),
                                    TranslationHelper.Instance.Translate("LIGHT_SETTING_WARNING"),
                                    TranslationHelper.Instance.Translate("MIN_SPEED") + $" : {VisionModel.MinSpeed}" +
                                    TranslationHelper.Instance.Translate("MAX_SPEED") + $" : {VisionModel.MaxSpeed}");
                                return OnGrab;
                            }
                        }
                        else
                        {
                            ErrorManager.Instance().Report((int)ErrorSection.Process, ErrorLevel.Warning, ErrorSection.Process.ToString(),
                                    TranslationHelper.Instance.Translate("LINE_SPEED_WARNING"), TranslationHelper.Instance.Translate("LINE_SPEED") + $" : {targetSpeed}");
                            return OnGrab;
                        }
                    }

                    GetImageTimer.Start();
                    SystemManager.EnableEncoder(true);
                }
                else
                {
                    SystemManager.EnableEncoder(false);
                    GetImageTimer.Stop();
                    DeviceManager.TopLightOff();
                    DeviceManager.BottomLightOff();
                    GetIMTeachingImage("ModuleGrab", ImageFormat.Png, true);
                }
                OnGrab = !OnGrab;
            }
            return OnGrab;
        }
    }
}
