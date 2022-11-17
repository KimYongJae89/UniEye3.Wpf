using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Module;
using UniScanC.Struct;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class AlgoTaskSettingWindowViewModel : Observable
    {
        // 각 모듈들의 이름
        private List<string> imList = new List<string>();
        public List<string> IMList
        {
            get => imList;
            set => Set(ref imList, value);
        }

        // 선택된 모듈의 이름
        private string selectedIM = string.Empty;
        public string SelectedIM
        {
            get => selectedIM;
            set
            {
                Set(ref selectedIM, value);
                if (value != null && ResultSettingDictionary.ContainsKey(value))
                {
                    // IM 을 선택했을 때 하위 항목을 업데이트 하는 함수
                    ResultLinksList = ResultSettingDictionary[value];
                }

                if (value != null && AlgoSettingDictionary.ContainsKey(value))
                {
                    // IM 을 선택했을 때 하위 항목을 업데이트
                    AlgoList = AlgoSettingDictionary[value];
                    var tempAlgoList = new ObservableCollection<string>(AlgoList.Select(x => x.AlgoParam.Name));
                    tempAlgoList.Insert(0, "ModuleImageDataByte");
                    tempAlgoList.Insert(0, "");
                    OutputAlgoList = tempAlgoList;
                }
            }
        }

        // 테스크에 추가 할 알고리즘 선택을 위한 변수
        private List<IAlgorithmBaseParam> selectableAlgoList = new List<IAlgorithmBaseParam>();
        public List<IAlgorithmBaseParam> SelectableAlgoList
        {
            get => selectableAlgoList;
            set => Set(ref selectableAlgoList, value);
        }

        // 사용할 테스크에 추가한 알고리즘 목록
        private ObservableCollection<AlgoModel> algoList = new ObservableCollection<AlgoModel>();
        public ObservableCollection<AlgoModel> AlgoList
        {
            get => algoList;
            set => Set(ref algoList, value);
        }

        // 입출력을 설정하기 위해서 선택한 알고리즘
        private AlgoModel selectedAlgo;
        public AlgoModel SelectedAlgo
        {
            get => selectedAlgo;
            set
            {
                Set(ref selectedAlgo, value);
                if (SelectedIM != null && ResultSettingDictionary.ContainsKey(SelectedIM))
                {
                    // IM 을 선택했을 때 하위 항목을 업데이트 하는 함수
                    ResultLinksList = ResultSettingDictionary[SelectedIM];
                }

                if (value != null)
                {
                    var tempAlgoList = new ObservableCollection<string>(AlgoList.Select(x => x.AlgoParam.Name));
                    tempAlgoList.Insert(0, "ModuleImageDataByte");
                    tempAlgoList.Insert(0, "");
                    OutputAlgoList = tempAlgoList;

                    if (IsResultToggled)
                    {
                        CanEditLink = false;
                    }
                    else
                    {
                        // SetNode 일 경우에만 링크 편집이 가능함
                        CanEditLink = selectedAlgo.AlgoParam.GetType().Name.Equals(typeof(SetNodeParam<object>).Name);
                    }
                }
            }
        }

        // 링크 시킬 수 있는 알고리즘 목록
        private ObservableCollection<string> outputAlgoList = new ObservableCollection<string>();
        public ObservableCollection<string> OutputAlgoList
        {
            get => outputAlgoList;
            set => Set(ref outputAlgoList, value);
        }

        // 입출력을 설정하기 위해서 선택한 링크
        private ILink selectedLink;
        public ILink SelectedLink
        {
            get => selectedLink;
            set => Set(ref selectedLink, value);
        }

        // 결과에 연결할 수 있는 입력 목록
        private ObservableCollection<LinkS> resultLinksList = new ObservableCollection<LinkS>();
        public ObservableCollection<LinkS> ResultLinksList
        {
            get => resultLinksList;
            set => Set(ref resultLinksList, value);
        }

        // 알고리즘에 관한 세팅 Dictionary
        private Dictionary<string, ObservableCollection<AlgoModel>> AlgoSettingDictionary { get; } = new Dictionary<string, ObservableCollection<AlgoModel>>();

        // 결과 값에 관한 세팅 Dictionary
        private Dictionary<string, ObservableCollection<LinkS>> ResultSettingDictionary { get; } = new Dictionary<string, ObservableCollection<LinkS>>();

        // 결과 링크를 띄우고 있을 경우 bool
        private bool isResultToggled = false;
        public bool IsResultToggled
        {
            get => isResultToggled;
            set
            {
                if (Set(ref isResultToggled, value))
                {
                    if (value)
                    {
                        CanEditLink = false;
                    }
                    else
                    {
                        if (selectedAlgo != null)
                        {
                            CanEditLink = selectedAlgo.AlgoParam.GetType().Name.Equals(typeof(SetNodeParam<object>).Name);
                        }
                    }
                }
            }
        }

        // SetNode 일 경우 링크 편집을 하기 위한 bool
        private bool canEditLink = false;
        public bool CanEditLink
        {
            get => canEditLink;
            set => Set(ref canEditLink, value);
        }

        public ICommand ResetSettingCommand { get; }
        public ICommand BatchSettingCommand { get; }
        public ICommand AddAlgoCommand { get; }
        public ICommand DeleteAlgoCommand { get; }
        public ICommand AddLinkCommand { get; }
        public ICommand DeleteLinkCommand { get; }
        public ICommand OrderUpCommand { get; }
        public ICommand OrderDownCommand { get; }
        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }

        public AlgoTaskSettingWindowViewModel(AlgoTaskManagerSettingDictionary tempSettingDictionary, List<InspectModuleInfo> imModuleList)
        {
            ResetSettingCommand = new RelayCommand(ResetSettingCommandAction);
            BatchSettingCommand = new RelayCommand(BatchSettingCommandAction);
            AddAlgoCommand = new RelayCommand<IAlgorithmBaseParam>(AddAlgoCommandAction);
            DeleteAlgoCommand = new RelayCommand(DeleteAlgoCommandAction);
            AddLinkCommand = new RelayCommand(AddLinkCommandAction);
            DeleteLinkCommand = new RelayCommand(DeleteLinkCommandAction);
            OrderUpCommand = new RelayCommand(OrderUpCommandAction);
            OrderDownCommand = new RelayCommand(OrderDownCommandAction);
            OKCommand = new RelayCommand<ChildWindow>(OKCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            InitializeAlgoList();
            LoadSettings(tempSettingDictionary);
            InitializeImList(imModuleList);
        }

        private void InitializeAlgoList()
        {
            // 선택할 수 있는 알고리즘 목록 생성
            List<Type> algoParams = ReflectionHelper.FindAllInheritedTypesDefinedAttribute(typeof(AlgorithmBaseParamAttribute));
            var tempAlgos = new List<IAlgorithmBaseParam>();
            foreach (Type algoParam in algoParams)
            {
                IAlgorithmBaseParam algoParamClass;
                if (algoParam.ContainsGenericParameters == false)
                {
                    System.Reflection.ConstructorInfo constructor = algoParam.GetConstructor(Type.EmptyTypes);
                    algoParamClass = constructor.Invoke(null) as IAlgorithmBaseParam;
                }
                else
                {
                    Type genericConstructor = algoParam.MakeGenericType(new Type[] { typeof(string) });
                    System.Reflection.ConstructorInfo constructor = genericConstructor.GetConstructor(Type.EmptyTypes);
                    algoParamClass = constructor.Invoke(null) as IAlgorithmBaseParam;
                }

                if (algoParamClass != null)
                {
                    tempAlgos.Add(algoParamClass);
                }
            }
            SelectableAlgoList = tempAlgos;
        }

        private void InitializeImList(List<InspectModuleInfo> imModuleList)
        {
            // 리스트에 IM 항목을 추가
            IMList = imModuleList.Select(x => x.ModuleTopic).ToList();
            // 기존에 없던 IM 세팅일 경우 새로 추가
            foreach (string imName in IMList)
            {
                if (!AlgoSettingDictionary.ContainsKey(imName))
                {
                    AlgoSettingDictionary.Add(imName, new ObservableCollection<AlgoModel>());
                }
                if (!ResultSettingDictionary.ContainsKey(imName))
                {
                    ResultSettingDictionary.Add(imName, MakeResultLinks());
                }
            }
        }

        private void LoadSettings(AlgoTaskManagerSettingDictionary tempSettingDictionary)
        {
            // 기존 항목 복사하여 데이터 추가
            foreach (KeyValuePair<string, AlgoTaskManagerSetting> settingPair in tempSettingDictionary.Clone())
            {
                var algoModelList = new ObservableCollection<AlgoModel>();
                foreach (INodeParam param in settingPair.Value.ParamList)
                {
                    algoModelList.Add(new AlgoModel(param, settingPair.Value.LinkList));
                }
                AlgoSettingDictionary.Add(settingPair.Key, algoModelList);

                ResultSettingDictionary.Add(settingPair.Key, MakeResultLinks(settingPair.Value.LinkList));
            }
        }

        private async void ResetSettingCommandAction()
        {
            string header = TranslationHelper.Instance.Translate("RESET_SETTING");
            string message = TranslationHelper.Instance.Translate("RESET_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                AlgoSettingDictionary.Clear();
                AlgoList.Clear();
                ResultSettingDictionary.Clear();
                ResultLinksList.Clear();
                foreach (string imName in IMList)
                {
                    AlgoTaskManagerSetting @default = FromJSON();

                    AlgoModel[] models = @default.ParamList.Select(f => new AlgoModel(f, @default.LinkList.FindAll(g => g.IsDestination(f)))).ToArray();
                    AlgoSettingDictionary.Add(imName, new ObservableCollection<AlgoModel>(models));
                    ResultSettingDictionary.Add(imName, MakeResultLinks(@default.LinkList));
                }
            }
        }

        private AlgoTaskManagerSetting FromJSON()
        {
            // json에서 로드하도록..?
            return AlgoTaskSettingDefault.SamsungScreen_All;
        }

        private async void BatchSettingCommandAction()
        {
            string header = TranslationHelper.Instance.Translate("BATCH_SETTING");
            string message = TranslationHelper.Instance.Translate("BATCH_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                AlgoSettingDictionary.Clear();
                ResultSettingDictionary.Clear();
                foreach (string imName in IMList)
                {
                    var tempAlgoList = new ObservableCollection<AlgoModel>();
                    foreach (AlgoModel algo in AlgoList)
                    {
                        tempAlgoList.Add(algo.Clone());
                    }
                    AlgoSettingDictionary.Add(imName, tempAlgoList);

                    var tempResultList = new ObservableCollection<LinkS>();
                    foreach (LinkS resultLink in ResultLinksList)
                    {
                        tempResultList.Add(resultLink.Clone() as LinkS);
                    }
                    ResultSettingDictionary.Add(imName, tempResultList);
                }
                AlgoList = AlgoSettingDictionary[SelectedIM];
                ResultLinksList = ResultSettingDictionary[SelectedIM];
            }
        }

        private async void AddAlgoCommandAction(IAlgorithmBaseParam selectedAlgo)
        {
            if (selectedAlgo.GetType().FullName.Equals(typeof(SetNodeParam<string>).FullName))
            {
                var view = new SetNodeSettingWindowView();
                view.DataContext = new SetNodeSettingWindowViewModel();
                IAlgorithmBaseParam result = await MessageWindowHelper.ShowChildWindow<IAlgorithmBaseParam>(view);
                if (result != null)
                {
                    AlgoList.Add(new AlgoModel(result, null));
                    OutputAlgoList.Add(result.Name);
                }
            }
            else
            {
                AlgoList.Add(new AlgoModel(selectedAlgo.Clone(), null));
                OutputAlgoList.Add(selectedAlgo.Name);
            }
        }

        private void DeleteAlgoCommandAction()
        {
            int originIndex = AlgoList.ToList().FindIndex(x => x.Equals(SelectedAlgo));

            AlgoList.Remove(SelectedAlgo);
            OutputAlgoList.RemoveAt(originIndex + 1);

            if (originIndex < AlgoList.Count)
            {
                SelectedAlgo = AlgoList[originIndex];
            }
            else if (AlgoList.Count > 0)
            {
                SelectedAlgo = AlgoList.Last();
            }
        }

        private void AddLinkCommandAction()
        {
            SelectedAlgo.AlgoLinkSList.Add(new LinkS("", "", SelectedAlgo.AlgoParam.Name, "ListArray"));
            RefreshLinks();
            SelectedLink = SelectedAlgo.AlgoLinkSList.Last();
        }

        private void DeleteLinkCommandAction()
        {
            SelectedAlgo.AlgoLinkSList.Remove(SelectedLink);
            RefreshLinks();
        }

        private void OrderUpCommandAction()
        {
            int originIndex = AlgoList.ToList().FindIndex(x => x.Equals(SelectedAlgo));
            if (originIndex > 0)
            {
                AlgoList.Move(originIndex, originIndex - 1);
                OutputAlgoList.Move(originIndex + 1, originIndex);
            }
        }

        private void OrderDownCommandAction()
        {
            int originIndex = AlgoList.ToList().FindIndex(x => x.Equals(SelectedAlgo));
            if (originIndex < AlgoList.Count - 1)
            {
                AlgoList.Move(originIndex, originIndex + 1);
                OutputAlgoList.Move(originIndex + 1, originIndex + 2);
            }
        }

        private void OKCommandAction(ChildWindow wnd)
        {
            var algoTaskManagerSettingDictionary = new AlgoTaskManagerSettingDictionary();
            foreach (KeyValuePair<string, ObservableCollection<AlgoModel>> algoSetting in AlgoSettingDictionary)
            {
                var settings = new AlgoTaskManagerSetting();
                // 알고리즘 추가, 알고리즘 링크 추가
                foreach (AlgoModel algo in algoSetting.Value)
                {
                    settings.AddTask(algo.AlgoParam);
                    IEnumerable<LinkS> algoLinks = algo.AlgoLinkSList.Cast<LinkS>();
                    IEnumerable<LinkS> filteredAlgoLinks = algoLinks.Where(x =>
                    !string.IsNullOrWhiteSpace(x.DstUnitName) && !string.IsNullOrWhiteSpace(x.DstPortName) &&
                    !string.IsNullOrWhiteSpace(x.SrcUnitName) && !string.IsNullOrWhiteSpace(x.SrcPortName));

                    foreach (LinkS links in filteredAlgoLinks)
                    {
                        bool isMatch = false;
                        Type srcType;
                        (string, Type) InPropName = algo.AlgoParam.InPropNameTypes.FirstOrDefault(x => x.Item1 == links.DstPortName);
                        if (InPropName.Item2 == null)
                        {
                            continue;
                        }

                        Type dstType = InPropName.Item2;

                        if (links.SrcUnitName == "ModuleImageDataByte")
                        {
                            srcType = ModuleImageDataByte.GetProps().First(x => x.Item1 == links.SrcPortName).Item2;
                            isMatch = ModuleImageDataByte.GetProps().FirstOrDefault(x => x.Item2.FullName.Equals(dstType.FullName)).Item2 != null;
                        }
                        else
                        {
                            AlgoModel srcAlgo = algoSetting.Value.First(x => x.AlgoParam.Name == links.SrcUnitName);
                            srcType = srcAlgo.AlgoParam.OutPropNameTypes.First(x => x.Item1 == links.SrcPortName).Item2;
                            isMatch = srcAlgo.AlgoParam.OutPropNameTypes.FirstOrDefault(x => x.Item2.FullName.Equals(dstType.FullName)).Item2 != null;
                        }

                        if (isMatch)
                        {
                            settings.AddLink(new LinkS(links.SrcUnitName, links.SrcPortName, links.DstUnitName, links.DstPortName));
                        }
                        else
                        {
                            if (algo.AlgoParam.GetType().Name == typeof(SetNodeParam<>).Name)
                            {
                                settings.AddLink(new LinkS(links.SrcUnitName, links.SrcPortName, links.DstUnitName, links.DstPortName));
                            }
                            else if (AlgoLinkConverter.IsExistConverter(srcType, dstType))
                            {
                                var tempLinkex = new LinkEx(links.SrcUnitName, links.SrcPortName, links.DstUnitName, links.DstPortName, srcType, dstType);
                                settings.AddLink(tempLinkex);
                            }
                        }
                    }
                }
                // 결과 링크 추가
                IEnumerable<LinkS> filteredResultLinks = ResultSettingDictionary[algoSetting.Key].Where(x =>
                    !string.IsNullOrWhiteSpace(x.DstUnitName) && !string.IsNullOrWhiteSpace(x.DstPortName) &&
                    !string.IsNullOrWhiteSpace(x.SrcUnitName) && !string.IsNullOrWhiteSpace(x.SrcPortName));

                foreach (LinkS links in filteredResultLinks)
                {
                    var inspectResult = new InspectResult();
                    bool isMatch = false;
                    Type srcType;
                    Type dstType = inspectResult.GetPropNameTypes().First(x => x.Item1 == links.DstPortName).Item2;

                    if (links.SrcUnitName == "ModuleImageDataByte")
                    {
                        srcType = ModuleImageDataByte.GetProps().First(x => x.Item1 == links.SrcPortName).Item2;
                        isMatch = ModuleImageDataByte.GetProps().FirstOrDefault(x => x.Item2.FullName.Equals(dstType.FullName)).Item2 != null;
                    }
                    else
                    {
                        AlgoModel srcAlgo = algoSetting.Value.FirstOrDefault(x => x.AlgoParam.Name == links.SrcUnitName);
                        if (srcAlgo == null)
                        {
                            continue;
                        }

                        srcType = srcAlgo.AlgoParam.OutPropNameTypes.First(x => x.Item1 == links.SrcPortName).Item2;
                        isMatch = srcAlgo.AlgoParam.OutPropNameTypes.FirstOrDefault(x => x.Item2.FullName.Equals(dstType.FullName)).Item2 != null;
                    }

                    if (isMatch)
                    {
                        settings.AddLink(new LinkS(links.SrcUnitName, links.SrcPortName, links.DstUnitName, links.DstPortName));
                    }
                    else
                    {
                        if (AlgoLinkConverter.IsExistConverter(srcType, dstType))
                        {
                            var tempLinkex = new LinkEx(links.SrcUnitName, links.SrcPortName, links.DstUnitName, links.DstPortName, srcType, dstType);
                            settings.AddLink(tempLinkex);
                        }
                    }
                }

                // 최종 결과에 추가
                algoTaskManagerSettingDictionary.Add(algoSetting.Key, settings);
            }
            wnd.Close(algoTaskManagerSettingDictionary);
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(null);
        }

        private ObservableCollection<LinkS> MakeResultLinks(List<ILink> linkList = null)
        {
            var resultLinksList = new ObservableCollection<LinkS>();
            var inspectResult = new InspectResult();

            if (linkList != null)
            {
                var linksList = linkList.Cast<LinkS>().ToList();
                var filteredLinksList = linksList.FindAll(x => x.DstUnitName == "InspectResult").ToList();
                foreach (string inProp in inspectResult.GetPropNameTypes().Select(f => f.Item1))
                {
                    LinkS findLink = filteredLinksList.Find(x => x.DstPortName == inProp);
                    if (findLink != null)
                    {
                        resultLinksList.Add(findLink.Clone() as LinkS);
                    }
                    else
                    {
                        resultLinksList.Add(new LinkS("", "", "InspectResult", inProp));
                    }
                }
            }
            else
            {
                foreach (string inProp in inspectResult.GetPropNameTypes().Select(f => f.Item1))
                {
                    resultLinksList.Add(new LinkS("", "", "InspectResult", inProp));
                }
            }

            return resultLinksList;
        }

        private void RefreshLinks()
        {
            string originAlgoName = SelectedAlgo.AlgoParam.Name;
            SelectedAlgo = null;
            SelectedAlgo = AlgoList.First(x => x.AlgoParam.Name == originAlgoName);
        }
    }
}
