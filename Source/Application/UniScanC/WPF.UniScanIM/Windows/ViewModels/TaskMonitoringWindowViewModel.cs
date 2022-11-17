using DynMvp.Devices.Dio;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.MachineInterface;
using UniScanC.Controls.Models;
using UniScanC.Data;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Windows.ViewModels
{
    public class TaskMonitoringWindowViewModel : Observable
    {
        #region 생성자
        public TaskMonitoringWindowViewModel()
        {
            var inspectRunner = SystemManager.Instance().InspectRunner as IMInspectRunner;
            inspectRunner.AfterInspectCompleted += AfterInspectCompletedAction;

            OkCommand = new RelayCommand<ChildWindow>((wnd) =>
            {
                inspectRunner.AfterInspectCompleted -= AfterInspectCompletedAction;
                wnd.Close(true);
            });

        }
        #endregion


        #region 속성
        public ICommand OkCommand { get; }

        private bool OnProcessing { get; set; } = false;

        private List<IMModule> TempLoadFactorList { get; set; } = new List<IMModule>();

        private ObservableCollection<IMModule> loadFactorList = new ObservableCollection<IMModule>();
        public ObservableCollection<IMModule> LoadFactorList
        {
            get => loadFactorList;
            set => Set(ref loadFactorList, value);
        }
        #endregion


        #region 메서드
        private void AfterInspectCompletedAction(InspectResult inspectResult)
        {
            if (OnProcessing)
            {
                return;
            }

            OnProcessing = true;

            foreach (KeyValuePair<string, double> loadFator in inspectResult.LoadFactorList)
            {
                IMModule imModule = TempLoadFactorList.Find(x => x.ModuleNo == inspectResult.ModuleNo.ToString());
                if (imModule == null)
                {
                    IMModule newIMModule = new IMModule()
                    {
                        ModuleNo = inspectResult.ModuleNo.ToString(),
                        AlgoTaskStates = new List<AlgoTaskState>()
                    };

                    TempLoadFactorList.Add(newIMModule);

                    imModule = newIMModule;
                }

                AlgoTaskState algoTaskState = imModule.AlgoTaskStates.Find(x => x.AlgoName == loadFator.Key);
                if (algoTaskState == null)
                {
                    AlgoTaskState newAlgoTaskState = new AlgoTaskState()
                    {
                        AlgoName = loadFator.Key,
                        AlgoLoadFactor = loadFator.Value,
                        AlgoLoadFactorString = loadFator.Value.ToString("N2")
                    };

                    imModule.AlgoTaskStates.Add(newAlgoTaskState);

                    algoTaskState = newAlgoTaskState;
                }
                else
                {
                    algoTaskState.AlgoLoadFactor = loadFator.Value;
                    algoTaskState.AlgoLoadFactorString = loadFator.Value.ToString("N2");
                }
            }

            TempLoadFactorList.Sort((x, y) => x.ModuleNo.CompareTo(y.ModuleNo));
            LoadFactorList = new ObservableCollection<IMModule>(TempLoadFactorList);

            OnProcessing = false;
        }
        #endregion


        #region 클래스
        public class IMModule : Observable
        {
            private string moduleNo;
            public string ModuleNo
            {
                get => moduleNo;
                set => Set(ref moduleNo, value);
            }

            private List<AlgoTaskState> algoTaskStates;
            public List<AlgoTaskState> AlgoTaskStates
            {
                get => algoTaskStates;
                set => Set(ref algoTaskStates, value);
            }
        }

        public class AlgoTaskState : Observable
        {
            private string algoName;
            public string AlgoName
            {
                get => algoName;
                set => Set(ref algoName, value);
            }

            private double algoLoadFactor;
            public double AlgoLoadFactor
            {
                get => algoLoadFactor;
                set => Set(ref algoLoadFactor, value);
            }

            private string algoLoadFactorString;
            public string AlgoLoadFactorString
            {
                get => algoLoadFactorString;
                set => Set(ref algoLoadFactorString, value);
            }
        }
        #endregion
    }
}
