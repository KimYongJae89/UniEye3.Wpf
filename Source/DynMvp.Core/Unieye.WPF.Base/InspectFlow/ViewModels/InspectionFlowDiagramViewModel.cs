using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.InspectFlow.Models;
using Unieye.WPF.Base.Services;

namespace Unieye.WPF.Base.InspectFlow.ViewModels
{
    public class SelectedBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(bool))
            {
                bool selected = (bool)value;
                return selected ? Application.Current.Resources["AccentBaseColorBrush"] : Application.Current.Resources["WhiteBrush"];
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InspectionFlowModelButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = value as InspectionFlowDiagramModel;
            return model?.GetType().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InspectionFlowDiagramModelLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = value as InspectionFlowDiagramModel;
            if (model?.PreviousModel == null)
            {
                return Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class InspectionFlowDiagramViewModel : Observable
    {
        public ObservableRangeCollection<InspectionFlowDiagramTreeItem> InspectionFlowDiagramTreeItemList { get; set; } = new ObservableRangeCollection<InspectionFlowDiagramTreeItem>();
        public ObservableRangeCollection<InspectionFlowDiagramModel> InspectionFlowDiagramModelList { get; set; } = new ObservableRangeCollection<InspectionFlowDiagramModel>();

        private object selectedTreeItem;
        public object SelectedTreeItem
        {
            get => selectedTreeItem;
            set => Set(ref selectedTreeItem, value);
        }

        private InspectionFlowDiagramModel selectedInspectionFlowDiagramModel;
        public InspectionFlowDiagramModel SelectedInspectionFlowDiagramModel
        {
            get => selectedInspectionFlowDiagramModel;
            set => Set(ref selectedInspectionFlowDiagramModel, value);
        }

        private InspectionFlowDiagramModel dropTargetInspectionFlowDiagramModel;
        public InspectionFlowDiagramModel DropTargetInspectionFlowDiagramModel
        {
            get => dropTargetInspectionFlowDiagramModel;
            set => Set(ref dropTargetInspectionFlowDiagramModel, value);
        }

        private object dragItem;
        public object DragItem
        {
            get => dragItem;
            set => Set(ref dragItem, value);
        }

        private bool isOpenContextMenu = false;
        public bool IsOpenContextMenu
        {
            get => isOpenContextMenu;
            set
            {
                if (Set(ref isOpenContextMenu, value))
                {
                    if (!isOpenContextMenu)
                    {
                        SelectedInspectionFlowDiagramModel = null;
                    }
                }
            }
        }

        private ICommand treeItemSelectedCommand;
        public ICommand TreeItemSelectedCommand => treeItemSelectedCommand ?? (treeItemSelectedCommand = new RelayCommand<object>(TreeItemSelected));

        private ICommand treeItemDropCommand;
        public ICommand TreeItemDropCommand => treeItemDropCommand ?? (treeItemDropCommand = new RelayCommand<FrameworkElement>(TreeItemDrop));

        private ICommand templateItemDropCommand;
        public ICommand TemplateItemDropCommand => templateItemDropCommand ?? (templateItemDropCommand = new RelayCommand<object>(TemplateItemDrop));

        private ICommand treeItemMouseMoveCommand;
        public ICommand TreeItemMouseMoveCommand => treeItemMouseMoveCommand ?? (treeItemMouseMoveCommand = new RelayCommand<FrameworkElement>(TreeItemMouseMove));

        private ICommand treeItemMouseDownCommand;
        public ICommand TreeItemMouseDownCommand => treeItemMouseDownCommand ?? (treeItemMouseDownCommand = new RelayCommand<FrameworkElement>(TreeItemMouseDown));

        private ICommand inspectorDoubleClickCommand;
        public ICommand InspectorDoubleClickCommand => inspectorDoubleClickCommand ?? (inspectorDoubleClickCommand = new RelayCommand<InspectionFlowDiagramModel>(InspectorDoubleClick));

        private ICommand inspectionFlowModelItemClickCommand;
        public ICommand InspectionFlowModelItemClickCommand => inspectionFlowModelItemClickCommand ?? (inspectionFlowModelItemClickCommand = new RelayCommand<List<object>>(InspectionFlowModelItemClick));

        private ICommand inspectionFlowModelItemMoveCommand;
        public ICommand InspectionFlowModelItemMoveCommand => inspectionFlowModelItemMoveCommand ?? (inspectionFlowModelItemMoveCommand = new RelayCommand<FrameworkElement>(InspectionFlowModelItemMove));

        private ICommand canvasMouseDownCommand;
        public ICommand CanvasMouseDownCommand => canvasMouseDownCommand ?? (canvasMouseDownCommand = new RelayCommand(CanvasMouseDown));

        private ICommand canvasMouseUpCommand;
        public ICommand CanvasMouseUpCommand => canvasMouseUpCommand ?? (canvasMouseUpCommand = new RelayCommand(CanvasMouseUp));

        private ICommand insertBeforeCommand;
        public ICommand InsertBeforeCommand => insertBeforeCommand ?? (insertBeforeCommand = new RelayCommand(InsertBefore));

        private ICommand insertAfterCommand;
        public ICommand InsertAfterCommand => insertAfterCommand ?? (insertAfterCommand = new RelayCommand(InsertAfter));


        private ICommand plusCalculateCommand;
        public ICommand PlusCalculateCommand => plusCalculateCommand ?? (plusCalculateCommand = new RelayCommand(PlusCalculate));


        private ICommand minusCalculateCommand;
        public ICommand MinusCalculateCommand => minusCalculateCommand ?? (minusCalculateCommand = new RelayCommand(MinusCalculate));

        private void TreeItemSelected(object obj)
        {
            SelectedTreeItem = obj;
        }

        private void TreeItemDrop(FrameworkElement uiElement)
        {
            var item = DragItem as InspectionFlowDiagramTreeItem;

            if (item?.MainType != typeof(InspectionFlowModel))
            {
                return;
            }

            Type type = item.MainType;

            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            var pt = new System.Windows.Point(point.X, point.Y);
            pt = uiElement.PointFromScreen(pt);

            var diagramModel = new InspectionFlowDiagramModel();
            diagramModel.X = pt.X;
            diagramModel.Y = pt.Y;

            var InspectionFlowModel = Activator.CreateInstance(type) as InspectionFlowModel;
            diagramModel.InspectionFlowModel = InspectionFlowModel;

            InspectionFlowDiagramModelList.Add(diagramModel);

            DragItem = null;
        }

        private void TemplateItemDrop(object item)
        {
            if (item is InspectionFlowDiagramModel)
            {
                if (ReferenceEquals(item, DragItem))
                {
                    return;
                }

                if (!(item is InspectionFlowDiagramModel diagramModel))
                {
                    return;
                }

                if (DragItem is Type)
                {
                    var type = DragItem as Type;
                    var FlowAlgorithmModel = Activator.CreateInstance(type) as FlowAlgorithmModel;
                    diagramModel.InspectionFlowModel.FlowAlgorithms.Add(FlowAlgorithmModel);
                }
                else if (DragItem is InspectionFlowDiagramModel)
                {
                    IsOpenContextMenu = true;
                    DropTargetInspectionFlowDiagramModel = item as InspectionFlowDiagramModel;
                }

                DragItem = null;
            }
        }

        private void TreeItemMouseMove(FrameworkElement uiElement)
        {
            //Type type = SelectedTreeItem as Type;

            //if (type != typeof(FlowAlgorithmModel))
            //    return;
        }

        private Point ModelPoint { get; set; }
        private Point MouseDownPoint { get; set; }
        private void InspectionFlowModelItemClick(List<object> objList)
        {
            var model = objList.Find(x => x is InspectionFlowDiagramModel) as InspectionFlowDiagramModel;
            SelectedInspectionFlowDiagramModel = model;

            if (SelectedInspectionFlowDiagramModel != null)
            {
                SelectedInspectionFlowDiagramModel.IsSelected = true;

                if (Keyboard.IsKeyDown(Key.LeftAlt))
                {
                    var uiElement = objList.Find(x => x is UIElement) as UIElement;
                    DragItem = SelectedInspectionFlowDiagramModel;
                    DragDrop.DoDragDrop(uiElement, DragItem, DragDropEffects.Move);
                }
                else
                {
                    ModelPoint = new Point(SelectedInspectionFlowDiagramModel.X, SelectedInspectionFlowDiagramModel.Y);
                    MouseDownPoint = Mouse.GetPosition(null);
                    Mouse.SetCursor(Cursors.SizeAll);
                }
            }
        }

        private void InspectionFlowModelItemMove(FrameworkElement uiElement)
        {
            if (SelectedInspectionFlowDiagramModel != null && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Point pt = Mouse.GetPosition(null);

                Point dist = ModelPoint + (pt - MouseDownPoint);

                SelectedInspectionFlowDiagramModel.X = dist.X;
                SelectedInspectionFlowDiagramModel.Y = dist.Y;

                DragItem = SelectedInspectionFlowDiagramModel;
            }
        }

        private void TreeItemMouseDown(FrameworkElement uiElement)
        {
            DragItem = SelectedTreeItem;
            DragDrop.DoDragDrop(uiElement, DragItem, DragDropEffects.Copy);
        }

        private void InspectorDoubleClick(InspectionFlowDiagramModel model)
        {
            var view = new Views.InspectionFlowView();
            view.InspectionFlowModel = model.InspectionFlowModel;
            view.ShowDialog();
        }

        private void CanvasMouseDown()
        {
            if (SelectedInspectionFlowDiagramModel != null)
            {
                SelectedInspectionFlowDiagramModel.IsSelected = false;
            }

            SelectedInspectionFlowDiagramModel = null;
        }

        private void CanvasMouseUp()
        {
            Mouse.SetCursor(Cursors.None);
            DragItem = null;
        }

        private void InsertBefore()
        {
            InspectionFlowDiagramModelList.Remove(SelectedInspectionFlowDiagramModel);

            var list = InspectionFlowDiagramModelList.ToList();
            int index = list.FindIndex(x => ReferenceEquals(x, DropTargetInspectionFlowDiagramModel));
            InspectionFlowDiagramModelList.Insert(index, SelectedInspectionFlowDiagramModel);
        }

        private void InsertAfter()
        {
            InspectionFlowDiagramModelList.Remove(SelectedInspectionFlowDiagramModel);

            var list = InspectionFlowDiagramModelList.ToList();
            int index = list.FindIndex(x => ReferenceEquals(x, DropTargetInspectionFlowDiagramModel));
            InspectionFlowDiagramModelList.Insert(index + 1, SelectedInspectionFlowDiagramModel);
        }

        private void PlusCalculate()
        {
        }

        private void MinusCalculate()
        {
        }

        public InspectionFlowDiagramViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(InspectionFlowDiagramTreeItemList, InspectionFlowDiagramTreeItemList);
            BindingOperations.EnableCollectionSynchronization(InspectionFlowDiagramModelList, InspectionFlowDiagramModelList);

            InspectionFlowDiagramModelList.CollectionChanged += OnInspectionFlowDiagramModelListChanged;

            InspectionFlowDiagramTreeItemList.AddRange(InspectionFlowDiagramTreeItem.CreateModel(typeof(InspectionFlowModel), typeof(FlowAlgorithmModel)));
        }

        private void OnInspectionFlowDiagramModelListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int order = 1;
            InspectionFlowDiagramModel lastModel = null;
            foreach (InspectionFlowDiagramModel model in InspectionFlowDiagramModelList)
            {
                model.Order = order++;
                model.PreviousModel = lastModel;
                lastModel = model;
            }
        }
    }
}
