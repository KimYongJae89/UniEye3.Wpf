using DynMvp.Devices;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Views;

namespace Unieye.WPF.Base.ViewModels
{
    internal class TowerLampSettingViewModel : Observable
    {
        #region 변수

        private List<TowerLampState> towerLampStateList;
        public List<TowerLampState> TowerLampStateList
        {
            get => towerLampStateList;
            set => Set(ref towerLampStateList, value);
        }

        private TowerLampState selectedTowerLamp;
        public TowerLampState SelectedTowerLamp
        {
            get => selectedTowerLamp;
            set => Set(ref selectedTowerLamp, value);
        }

        #endregion

        #region Command 

        private ICommand towerLampStateCommand;
        public ICommand TowerLampStateCommand => towerLampStateCommand ?? (towerLampStateCommand = new RelayCommand<Lamp>(LampState));

        private void LampState(Lamp lamp)
        {
            var settingWindow = new TowerLampOptionWindow(lamp as Lamp);
            if (settingWindow.ShowDialog() == true)
            {
                UpdateTowerLamp();
            }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new RelayCommand(Save));

        private void Save()
        {
            if (towerLamp != null)
            {
                towerLamp.Save(DynMvp.Base.BaseConfig.Instance().ConfigPath);
            }
        }

        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand<ChildWindow>(Close));

        private void Close(ChildWindow window)
        {
            window.Close();
        }

        #endregion

        public TowerLampSettingViewModel()
        {
            UpdateTowerLamp();
        }

        private TowerLamp towerLamp;
        public void UpdateTowerLamp()
        {
            TowerLampStateList = null;

            towerLamp = DeviceManager.Instance().TowerLamp;
            if (towerLamp != null)
            {
                TowerLampStateList = towerLamp.TowerLampStateList;
            }
        }
    }
}
