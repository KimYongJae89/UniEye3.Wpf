using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    public class ContextAction : Observable
    {
        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private ICommand action;
        public ICommand Action
        {
            get => action;
            set => Set(ref action, value);
        }

        private Brush icon;
        public Brush Icon
        {
            get => icon;
            set => Set(ref icon, value);
        }
    }

    public class InspectionFlowDiagramModel : Observable
    {
        private int order = 1;
        public int Order
        {
            get => order;
            set => Set(ref order, value);
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set => Set(ref isSelected, value);
        }

        private double x = 0;
        public double X
        {
            get => x;
            set => Set(ref x, value);
        }

        private double y = 0;
        public double Y
        {
            get => y;
            set => Set(ref y, value);
        }

        private double width = 100;
        public double Width
        {
            get => width;
            set => Set(ref width, value);
        }

        private double height = 100;
        public double Height
        {
            get => height;
            set => Set(ref height, value);
        }

        public InspectionFlowModel inspectionFlowModel;
        public InspectionFlowModel InspectionFlowModel
        {
            get => inspectionFlowModel;
            set => Set(ref inspectionFlowModel, value);
        }

        private InspectionFlowDiagramModel previousModel;
        public InspectionFlowDiagramModel PreviousModel
        {
            get => previousModel;
            set => Set(ref previousModel, value);
        }
    }

    public class InspectionFlowDiagramTreeItem
    {
        public string Name { get; set; } = "New Inspector";
        public Type MainType { get; set; }
        public ObservableRangeCollection<Type> SubTypeList { get; set; } = new ObservableRangeCollection<Type>();

        public static List<InspectionFlowDiagramTreeItem> CreateModel(Type mainType, Type subType)
        {
            var modelList = new List<InspectionFlowDiagramTreeItem>();

            var model = new InspectionFlowDiagramTreeItem();
            model.MainType = mainType;
            model.SubTypeList.AddRange(ReflectionHelper.FindAllInheritedTypes(subType));
            modelList.Add(model);

            return modelList;

            //foreach(var type in ReflectionHelper.FindAllInheritedTypes(mainType))
            //{
            //    var model = new InspectionFlowDiagramTreeItem();
            //    model.MainType = type;
            //    model.SubTypeList.AddRange(ReflectionHelper.FindAllInheritedTypes(subType));

            //    modelList.Add(model);
            //}

            //return modelList;
        }
    }
}
