using DynMvp.Base;
using DynMvp.Data;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Module;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.Override;
using ModelManager = UniScanC.Models.ModelManager;

namespace WPF.UniScanCM.Controls.ViewModels
{
    public class ModelDefectCategoryControlViewModel : Observable
    {
        private bool useInspectModule;
        public bool UseInspectModule
        {
            get => useInspectModule;
            set => Set(ref useInspectModule, value);
        }

        private bool useThicknessModule;
        public bool UseThicknessModule
        {
            get => useThicknessModule;
            set => Set(ref useThicknessModule, value);
        }

        private bool useGlossModule;
        public bool UseGlossModule
        {
            get => useGlossModule;
            set => Set(ref useGlossModule, value);
        }

        private string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set
            {
                if (Set(ref categoryName, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.Name = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private EDefectType defectType;
        public EDefectType DefectType
        {
            get => defectType;
            set
            {
                if (Set(ref defectType, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.DefectType = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private EDefectMarkerType defectFigure;
        public EDefectMarkerType DefectFigure
        {
            get => defectFigure;
            set
            {
                if (Set(ref defectFigure, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.DefectFigure = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private Color defectColor = Colors.Red;
        public Color DefectColor
        {
            get => defectColor;
            set
            {
                if (Set(ref defectColor, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.DefectColor = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private int warningLevel = 1;
        public int WarningLevel
        {
            get => warningLevel;
            set
            {
                if (Set(ref warningLevel, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.WarningLevel = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private int defectCount = 1;
        public int DefectCount
        {
            get => defectCount;
            set
            {
                if (Set(ref defectCount, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.DefectCount = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        public UniScanC.Models.Model Model { get; set; }

        public List<InspectModuleInfo> ModuleList => SystemConfig.Instance.ImModuleList;

        private InspectModuleInfo selectedModule;
        public InspectModuleInfo SelectedModule
        {
            get => selectedModule;
            set
            {
                if (Set(ref selectedModule, value))
                {
                    if (Model != null)
                    {
                        VisionModel = Model.VisionModels[value.ModuleNo];
                        DefectCategoryList = new ObservableCollection<DefectCategory>(VisionModel.DefectCategories);
                    }
                }
            }
        }

        private VisionModel visionModel = new VisionModel();
        public VisionModel VisionModel
        {
            get => visionModel;
            set => Set(ref visionModel, value);
        }

        private ObservableCollection<DefectCategory> defectCategoryList;
        public ObservableCollection<DefectCategory> DefectCategoryList
        {
            get => defectCategoryList;
            set => Set(ref defectCategoryList, value);
        }

        private SensorModel sensorModel;
        public SensorModel SensorModel
        {
            get => sensorModel;
            set => Set(ref sensorModel, value);
        }

        private string scanWidthName;
        public string ScanWidthName
        {
            get => scanWidthName;
            set => Set(ref scanWidthName, value);
        }

        private string petParamName;
        public string PetParamName
        {
            get => petParamName;
            set
            {
                if (Set(ref petParamName, value))
                {
                    PetParam = LoadLayerParam(ELayerParamType.PET);
                }
            }
        }

        private string sheetParamName;
        public string SheetParamName
        {
            get => sheetParamName;
            set
            {
                if (Set(ref sheetParamName, value))
                {
                    SheetParam = LoadLayerParam(ELayerParamType.SHEET);
                }
            }
        }

        private ThicknessLayerParam petParam;
        public ThicknessLayerParam PetParam
        {
            get => petParam;
            set => Set(ref petParam, value);
        }

        private ThicknessLayerParam sheetParam;
        public ThicknessLayerParam SheetParam
        {
            get => sheetParam;
            set => Set(ref sheetParam, value);
        }

        private string[] scanWidthList;
        public string[] ScanWidthList
        {
            get => scanWidthList;
            set => Set(ref scanWidthList, value);
        }

        private string[] petParamList;
        public string[] PetParamList
        {
            get => petParamList;
            set => Set(ref petParamList, value);
        }

        private string[] sheetParamList;
        public string[] SheetParamList
        {
            get => sheetParamList;
            set => Set(ref sheetParamList, value);
        }

        private DefectCategory selectedDefectCategory;
        public DefectCategory SelectedDefectCategory
        {
            get => selectedDefectCategory;
            set
            {
                if (Set(ref selectedDefectCategory, value))
                {
                    if (selectedDefectCategory == null)
                    {
                        return;
                    }

                    RefreshFlag = true;
                    CategoryName = selectedDefectCategory.Name;
                    foreach (CategoryType categoryType in selectedDefectCategory.CategoryTypeList)
                    {
                        switch (categoryType.Type)
                        {
                            case ECategoryTypeName.EdgeLower:
                                UseEdgeLower = categoryType.Use;
                                EdgeLower = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.EdgeUpper:
                                UseEdgeUpper = categoryType.Use;
                                EdgeUpper = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.AreaLower:
                                UseAreaLower = categoryType.Use;
                                AreaLower = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.AreaUpper:
                                UseAreaUpper = categoryType.Use;
                                AreaUpper = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.MinGvLower:
                                UseMinGvLower = categoryType.Use;
                                MinGvLower = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.MinGvUpper:
                                UseMinGvUpper = categoryType.Use;
                                MinGvUpper = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.MaxGvLower:
                                UseMaxGvLower = categoryType.Use;
                                MaxGvLower = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.MaxGvUpper:
                                UseMaxGvUpper = categoryType.Use;
                                MaxGvUpper = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.AvgGvLower:
                                UseAvgGvLower = categoryType.Use;
                                AvgGvLower = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.AvgGvUpper:
                                UseAvgGvUpper = categoryType.Use;
                                AvgGvUpper = (double)categoryType.Data;
                                break;
                            case ECategoryTypeName.WidthLower:
                                UseWidthLower = categoryType.Use;
                                WidthLower = Convert.ToDouble(string.Format("{0:0.00}", (double)categoryType.Data));
                                break;
                            case ECategoryTypeName.WidthUpper:
                                UseWidthUpper = categoryType.Use;
                                WidthUpper = Convert.ToDouble(string.Format("{0:0.00}", (double)categoryType.Data));
                                break;
                            case ECategoryTypeName.HeightLower:
                                UseHeightLower = categoryType.Use;
                                HeightLower = Convert.ToDouble(string.Format("{0:0.00}", (double)categoryType.Data));
                                break;
                            case ECategoryTypeName.HeightUpper:
                                UseHeightUpper = categoryType.Use;
                                HeightUpper = Convert.ToDouble(string.Format("{0:0.00}", (double)categoryType.Data));
                                break;
                        }
                    }

                    IsSkip = selectedDefectCategory.IsSkip;
                    DefectType = selectedDefectCategory.DefectType;
                    DefectFigure = selectedDefectCategory.DefectFigure;
                    DefectColor = selectedDefectCategory.DefectColor;
                    WarningLevel = selectedDefectCategory.WarningLevel;
                    DefectCount = selectedDefectCategory.DefectCount;

                    RefreshFlag = false;
                }
            }
        }

        private bool useEdgeLower;
        public bool UseEdgeLower
        {
            get => useEdgeLower;
            set
            {
                if (Set(ref useEdgeLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.EdgeLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useEdgeUpper;
        public bool UseEdgeUpper
        {
            get => useEdgeUpper;
            set
            {
                if (Set(ref useEdgeUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.EdgeUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useAreaLower;
        public bool UseAreaLower
        {
            get => useAreaLower;
            set
            {
                if (Set(ref useAreaLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AreaLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useAreaUpper;
        public bool UseAreaUpper
        {
            get => useAreaUpper;
            set
            {
                if (Set(ref useAreaUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AreaUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useMinGvLower;
        public bool UseMinGvLower
        {
            get => useMinGvLower;
            set
            {
                if (Set(ref useMinGvLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MinGvLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useMinGvUpper;
        public bool UseMinGvUpper
        {
            get => useMinGvUpper;
            set
            {
                if (Set(ref useMinGvUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MinGvUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useMaxGvLower;
        public bool UseMaxGvLower
        {
            get => useMaxGvLower;
            set
            {
                if (Set(ref useMaxGvLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MaxGvLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useMaxGvUpper;
        public bool UseMaxGvUpper
        {
            get => useMaxGvUpper;
            set
            {
                if (Set(ref useMaxGvUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MaxGvUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useAvgGvLower;
        public bool UseAvgGvLower
        {
            get => useAvgGvLower;
            set
            {
                if (Set(ref useAvgGvLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AvgGvLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useAvgGvUpper;
        public bool UseAvgGvUpper
        {
            get => useAvgGvUpper;
            set
            {
                if (Set(ref useAvgGvUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AvgGvUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useWidthLower;
        public bool UseWidthLower
        {
            get => useWidthLower;
            set
            {
                if (Set(ref useWidthLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.WidthLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useWidthUpper;
        public bool UseWidthUpper
        {
            get => useWidthUpper;
            set
            {
                if (Set(ref useWidthUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.WidthUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useHeightLower;
        public bool UseHeightLower
        {
            get => useHeightLower;
            set
            {
                if (Set(ref useHeightLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.HeightLower).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool useHeightUpper;
        public bool UseHeightUpper
        {
            get => useHeightUpper;
            set
            {
                if (Set(ref useHeightUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.HeightUpper).Use = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double edgeLower;
        public double EdgeLower
        {
            get => edgeLower;
            set
            {
                if (Set(ref edgeLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.EdgeLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double edgeUpper;
        public double EdgeUpper
        {
            get => edgeUpper;
            set
            {
                if (Set(ref edgeUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.EdgeUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double areaLower;
        public double AreaLower
        {
            get => areaLower;
            set
            {
                if (Set(ref areaLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AreaLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double areaUpper;
        public double AreaUpper
        {
            get => areaUpper;
            set
            {
                if (Set(ref areaUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AreaUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double minGvLower;
        public double MinGvLower
        {
            get => minGvLower;
            set
            {
                if (Set(ref minGvLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MinGvLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double minGvUpper;
        public double MinGvUpper
        {
            get => minGvUpper;
            set
            {
                if (Set(ref minGvUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MinGvUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double maxGvLower;
        public double MaxGvLower
        {
            get => maxGvLower;
            set
            {
                if (Set(ref maxGvLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MaxGvLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double maxGvUpper;
        public double MaxGvUpper
        {
            get => maxGvUpper;
            set
            {
                if (Set(ref maxGvUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.MaxGvUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double avgGvLower;
        public double AvgGvLower
        {
            get => avgGvLower;
            set
            {
                if (Set(ref avgGvLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AvgGvLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double avgGvUpper;
        public double AvgGvUpper
        {
            get => avgGvUpper;
            set
            {
                if (Set(ref avgGvUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.AvgGvUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double widthLower;
        public double WidthLower
        {
            get => widthLower;
            set
            {
                if (Set(ref widthLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.WidthLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double widthUpper;
        public double WidthUpper
        {
            get => widthUpper;
            set
            {
                if (Set(ref widthUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.WidthUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double heightLower;
        public double HeightLower
        {
            get => heightLower;
            set
            {
                if (Set(ref heightLower, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.HeightLower).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private double heightUpper;
        public double HeightUpper
        {
            get => heightUpper;
            set
            {
                if (Set(ref heightUpper, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.GetCategory(ECategoryTypeName.HeightUpper).Data = value;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        private bool isSkip;
        public bool IsSkip
        {
            get => isSkip;
            set
            {
                if (Set(ref isSkip, value))
                {
                    if (SelectedDefectCategory != null)
                    {
                        SelectedDefectCategory.IsSkip = isSkip;
                        RefreshDefectCategoryList();
                    }
                }
            }
        }

        public bool RefreshFlag { get; set; } = false;

        public System.Windows.Input.ICommand SaveCommand { get; }
        public System.Windows.Input.ICommand BatchSettingCommand { get; }
        public System.Windows.Input.ICommand BaseSettingCommand { get; }
        public System.Windows.Input.ICommand CategoryListOrderUpCommand { get; }
        public System.Windows.Input.ICommand CategoryListOrderDownCommand { get; }
        public System.Windows.Input.ICommand AddCommand { get; }
        public System.Windows.Input.ICommand DeleteCommand { get; }

        public ModelDefectCategoryControlViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandAction);
            BatchSettingCommand = new RelayCommand(BatchSettingCommandAction);
            BaseSettingCommand = new RelayCommand(BaseSettingCommandAction);
            CategoryListOrderUpCommand = new RelayCommand<int>(CategoryListOrderUpCommandAction);
            CategoryListOrderDownCommand = new RelayCommand<int>(CategoryListOrderDownCommandAction);
            AddCommand = new RelayCommand(AddCommandAction);
            DeleteCommand = new RelayCommand(DeleteCommandAction);

            if (ModuleList.Count > 0)
            {
                SelectedModule = ModuleList.FirstOrDefault();
            }

            SystemConfig config = Override.SystemConfig.Instance;
            UseInspectModule = config.UseInspectModule;
            UseThicknessModule = config.UseThicknessModule;
            UseGlossModule = config.UseGlossModule;
        }

        private void SetDefectCategoryData(DefectCategory category)
        {
            category.Name = CategoryName;
            category.DefectType = DefectType;
            category.DefectFigure = DefectFigure;
            category.DefectColor = DefectColor;
            category.WarningLevel = WarningLevel;
            category.DefectCount = DefectCount;

            foreach (CategoryType categoryType in category.CategoryTypeList)
            {
                switch (categoryType.Type)
                {
                    case ECategoryTypeName.AreaLower:
                        categoryType.Use = UseAreaLower;
                        categoryType.Data = AreaLower;
                        break;
                    case ECategoryTypeName.AreaUpper:
                        categoryType.Use = UseAreaUpper;
                        categoryType.Data = AreaUpper;
                        break;
                    case ECategoryTypeName.MinGvLower:
                        categoryType.Use = UseMinGvLower;
                        categoryType.Data = MinGvLower;
                        break;
                    case ECategoryTypeName.MinGvUpper:
                        categoryType.Use = UseMinGvUpper;
                        categoryType.Data = MinGvUpper;
                        break;
                    case ECategoryTypeName.MaxGvLower:
                        categoryType.Use = UseMaxGvLower;
                        categoryType.Data = MaxGvLower;
                        break;
                    case ECategoryTypeName.MaxGvUpper:
                        categoryType.Use = UseMaxGvUpper;
                        categoryType.Data = MaxGvUpper;
                        break;
                    case ECategoryTypeName.AvgGvLower:
                        categoryType.Use = UseAvgGvLower;
                        categoryType.Data = AvgGvLower;
                        break;
                    case ECategoryTypeName.AvgGvUpper:
                        categoryType.Use = UseAvgGvUpper;
                        categoryType.Data = AvgGvUpper;
                        break;
                    case ECategoryTypeName.WidthLower:
                        categoryType.Use = UseWidthLower;
                        categoryType.Data = WidthLower;
                        break;
                    case ECategoryTypeName.WidthUpper:
                        categoryType.Use = UseWidthUpper;
                        categoryType.Data = WidthUpper;
                        break;
                    case ECategoryTypeName.HeightLower:
                        categoryType.Use = UseHeightLower;
                        categoryType.Data = HeightLower;
                        break;
                    case ECategoryTypeName.HeightUpper:
                        categoryType.Use = UseHeightUpper;
                        categoryType.Data = HeightUpper;
                        break;
                }
            }
        }

        private void RefreshDefectCategoryList()
        {
            if (RefreshFlag == true)
            {
                return;
            }

            DefectCategory item = SelectedDefectCategory;

            ObservableCollection<DefectCategory> list = DefectCategoryList;
            DefectCategoryList = null;
            DefectCategoryList = list;

            SelectedDefectCategory = item;
        }

        public void SetModel(ModelBase modelBase)
        {
            Model = modelBase as UniScanC.Models.Model;
            if (Model != null)
            {
                if (SelectedModule != null)
                {
                    VisionModel = Model.VisionModels[SelectedModule.ModuleNo];
                    DefectCategoryList = new ObservableCollection<DefectCategory>(VisionModel.DefectCategories);
                }
            }
        }

        public async Task SaveSettings()
        {
            var actionList = new List<Action>();
            Model.ModelDescription.ModifiedDate = DateTime.Now;
            actionList.Add(() =>
            {
                Model.SaveModel();

                if (SensorModel != null)
                {
                    SensorModel.PetParamName = PetParamName;
                    SensorModel.SheetParamName = SheetParamName;
                    SensorModel.ScanWidthName = ScanWidthName;
                }
            });

            var source = new ProgressSource();
            source.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            await MessageWindowHelper.ShowProgress(TranslationHelper.Instance.Translate("Setting"),
                TranslationHelper.Instance.Translate("Save_the_configuration_file") + ("..."),
                actionList, true, source);
        }

        private ThicknessLayerParam LoadLayerParam(ELayerParamType type)
        {
            if (type == ELayerParamType.PET)
            {
                var petParam = new ThicknessLayerParam(PetParamName, type);
                petParam.LoadDatabase();
                return petParam;

            }
            else
            {
                var sheetParam = new ThicknessLayerParam(SheetParamName, type);
                sheetParam.LoadDatabase();
                return sheetParam;
            }
        }

        private async void SaveCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Defect Category Save");
            string header = TranslationHelper.Instance.Translate("Save");
            string message = TranslationHelper.Instance.Translate("SAVE_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                VisionModel.DefectCategories = DefectCategoryList.ToList();

                await SaveSettings();
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Defect Category Save Complete");

                var commMgr = CommManager.Instance() as CommManager;
                var modelPath = new DirectoryInfo(UniScanC.Models.ModelManager.Instance().ModelPath);
                if (Model != null && await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, Model.Name, modelPath.Name))
                {
                }
                else
                {
                    await MessageWindowHelper.ShowMessageBox(
                        TranslationHelper.Instance.Translate("NOTIFICATION"),
                        TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                        System.Windows.MessageBoxButton.OK);
                }
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Cancel Defect Category Save");
            }
        }

        private async void BatchSettingCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Defect Category Batch Setting");
            string header = TranslationHelper.Instance.Translate("BATCH_SETTING");
            string message = TranslationHelper.Instance.Translate("BATCH_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                VisionModel.DefectCategories = DefectCategoryList.ToList();
                foreach (VisionModel visionModel in Model.VisionModels)
                {
                    if (visionModel != VisionModel)
                    {
                        visionModel.CopyDefectCategoriesFrom(VisionModel);
                    }
                }

                await SaveSettings();
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Defect Category Batch Setting Complete");

                var commMgr = CommManager.Instance() as CommManager;
                var modelPath = new DirectoryInfo(UniScanC.Models.ModelManager.Instance().ModelPath);
                if (Model != null && await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, Model.Name, modelPath.Name))
                {
                }
                else
                {
                    await MessageWindowHelper.ShowMessageBox(
                        TranslationHelper.Instance.Translate("NOTIFICATION"),
                        TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                        System.Windows.MessageBoxButton.OK);
                }
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Cancel Defect Category Batch Setting");
            }
        }

        private async void BaseSettingCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Click Defect Category Base Setting");
            string header = TranslationHelper.Instance.Translate("BASE_SETTING");
            string message = TranslationHelper.Instance.Translate("BASE_SETTING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                var baseVisionModel = new VisionModel();
                if (baseVisionModel.Load(BaseConfig.Instance().ConfigPath))
                {
                    VisionModel.CopyDefectCategoriesFrom(baseVisionModel);
                    await SaveSettings();

                    VisionModel = Model.VisionModels[SelectedModule.ModuleNo];
                    DefectCategoryList = new ObservableCollection<DefectCategory>(VisionModel.DefectCategories);

                    LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Defect Category Base Setting Complete");

                    var commMgr = CommManager.Instance() as CommManager;
                    var modelPath = new DirectoryInfo(UniScanC.Models.ModelManager.Instance().ModelPath);
                    if (Model != null && await commMgr.ExecuteCommand(EUniScanCCommand.OpenModel, Model.Name, modelPath.Name))
                    {
                    }
                    else
                    {
                        await MessageWindowHelper.ShowMessageBox(
                            TranslationHelper.Instance.Translate("NOTIFICATION"),
                            TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                            System.Windows.MessageBoxButton.OK);
                    }
                }
                else
                {
                    await MessageWindowHelper.ShowMessageBox(
                            TranslationHelper.Instance.Translate("NOTIFICATION"),
                            TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                            System.Windows.MessageBoxButton.OK);
                }
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Teaching Page] Cancel Defect Category Base Setting");
            }
        }

        private void CategoryListOrderUpCommandAction(int index)
        {
            if (index <= 0)
            {
                return;
            }

            DefectCategory selectedItem = SelectedDefectCategory;

            DefectCategoryList.Remove(selectedItem);
            DefectCategoryList.Insert(index - 1, selectedItem);

            SelectedDefectCategory = selectedItem;
        }

        private void CategoryListOrderDownCommandAction(int index)
        {
            if (index >= DefectCategoryList.Count() - 1)
            {
                return;
            }

            DefectCategory selectedItem = SelectedDefectCategory;

            DefectCategoryList.Remove(selectedItem);
            DefectCategoryList.Insert(index + 1, selectedItem);

            SelectedDefectCategory = selectedItem;
        }

        private void AddCommandAction()
        {
            if (string.IsNullOrEmpty(CategoryName))
            {
                return;
            }

            var newCategory = new DefectCategory(CategoryName);
            SetDefectCategoryData(newCategory);
            DefectCategoryList.Add(newCategory);
        }

        private void DeleteCommandAction()
        {
            if (SelectedDefectCategory == null)
            {
                return;
            }

            DefectCategoryList.Remove(SelectedDefectCategory);
            SelectedDefectCategory = null;
        }
    }
}
