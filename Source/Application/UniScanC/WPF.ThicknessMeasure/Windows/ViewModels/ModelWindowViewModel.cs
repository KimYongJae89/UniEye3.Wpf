using DynMvp.Devices.Spectrometer.Data;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Models;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Windows.ViewModels
{
    public class ModelWindowViewModel : Observable
    {
        #region 필드
        private ModelWindowResult _result = new ModelWindowResult();

        private ICommand _acceptCommand;
        private ICommand _cancelCommand;
        #endregion

        #region 생성자
        public ModelWindowViewModel(ModelWindowResult Result = null)
        {
            if (Result != null)
            {
                this.Result = Result;
            }

            BaseLayerParamList = SystemConfig.Instance.LayerParamList;

            TotalLayerParamList = new ObservableRangeCollection<LayerParamStruct>();
            foreach (KeyValuePair<string, List<LayerParam>> pair in BaseLayerParamList)
            {
                TotalLayerParamList.Add(new LayerParamStruct(pair.Key, pair.Value));
                if (Result != null && Result.LayerParamList.ContainsKey(pair.Key))
                {
                    TotalLayerParamList.Last().SelectedLayerParam = TotalLayerParamList.Last().LayerParamList.Where(x => x.ParamName == Result.LayerParamList[pair.Key]).First();
                }
                else if (pair.Value.Count > 0)
                {
                    TotalLayerParamList.Last().SelectedLayerParam = pair.Value.First();
                }
            }

            ScanWidthList = SystemConfig.Instance.ScanWidthList;
            if (Result != null && ScanWidthList.Exists(x => x.Name == Result.ScanWidth))
            {
                SelectedScanWidth = ScanWidthList.Find(x => x.Name == Result.ScanWidth);
            }
            else if (ScanWidthList.Count > 0)
            {
                SelectedScanWidth = ScanWidthList.First();
            }

            InfoList = ((Override.DeviceManager)DeviceManager.Instance()).Spectrometer.DeviceList.Values.ToList();
            if (Result != null)
            {
                SelectedInfo = InfoList.Find(x => x.Name == Result.SensorType);
            }

            //if (Result != null && infoList.Contains(Result.SensorType))
            //    SelectedSpectrometerName = SpectrometerNameList.Where(x => x.Value == Result.SensorType).First();
            //else if (SpectrometerNameList.Count > 0)
            //    SelectedSpectrometerName = SpectrometerNameList.First();
        }
        #endregion

        #region 속성
        public ModelWindowResult Result { get => _result; set => Set(ref _result, value); }

        public Dictionary<string, List<LayerParam>> BaseLayerParamList { get; set; }

        public ObservableRangeCollection<LayerParamStruct> TotalLayerParamList { get; set; }

        public List<ScanWidth> ScanWidthList { get; set; }

        public ScanWidth SelectedScanWidth { get; set; }

        public List<DynMvp.Devices.Spectrometer.SpectrometerInfo> InfoList { get; set; }

        public DynMvp.Devices.Spectrometer.SpectrometerInfo SelectedInfo { get; set; }

        public ICommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand<ChildWindow>(Accept));

        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand<ChildWindow>(Cancel));
        #endregion

        #region 메서드
        private async void Accept(ChildWindow wnd)
        {
            if (string.IsNullOrEmpty(_result.Name) || string.IsNullOrWhiteSpace(_result.Name))
            {
                await ((MetroWindow)Window.GetWindow(wnd)).ShowMessageAsync(
                    TranslationHelper.Instance.Translate("Error"),
                    TranslationHelper.Instance.Translate("ModelWindow_Error"));

                return;
            }

            foreach (LayerParamStruct layerParam in TotalLayerParamList)
            {
                if (Result.LayerParamList.ContainsKey(layerParam.LayerName) == true)
                {
                    Result.LayerParamList[layerParam.LayerName] = layerParam.SelectedLayerParam.ParamName;
                }
                else
                {
                    Result.LayerParamList.Add(layerParam.LayerName, layerParam.SelectedLayerParam.ParamName);
                }
            }

            Result.ScanWidth = SelectedScanWidth.Name;
            Result.SensorType = SelectedInfo.Name;

            wnd.Close(_result);
        }

        private void Cancel(ChildWindow wnd)
        {
            wnd.Close();
        }
        #endregion
    }

    public class LayerParamStruct : Observable
    {
        #region 필드
        private string layerName;
        private LayerParam selectedLayerParam;
        private ObservableRangeCollection<LayerParam> layerParamList;
        public string LayerName { get => layerName; set => Set(ref layerName, value); }
        #endregion

        #region 생성자
        public LayerParamStruct(string layerName, List<LayerParam> layerParamList)
        {
            LayerName = layerName;
            LayerParamList = new ObservableRangeCollection<LayerParam>();
            foreach (LayerParam param in layerParamList)
            {
                LayerParamList.Add(param.Clone());
            }
        }
        #endregion

        #region 메서드
        public LayerParam SelectedLayerParam { get => selectedLayerParam; set => Set(ref selectedLayerParam, value); }

        public ObservableRangeCollection<LayerParam> LayerParamList { get => layerParamList; set => Set(ref layerParamList, value); }
        #endregion
    }

    public class ModelWindowResult : Unieye.WPF.Base.Controls.ModelWindowResult
    {
        #region 속성
        public Dictionary<string, string> LayerParamList { get; set; } = new Dictionary<string, string>();
        public string ScanWidth { get; set; }
        public string SensorType { get; set; }
        #endregion
    }
}
