using Infragistics.Controls.Charts;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Unieye.WPF.Base.Infragistics
{
    public enum SeriesType
    {
        LineSeries,
        ColumnSeries,
        ScatterSeries,
        ScatterLineSeries,
    }

    public class SeriesBinderBehavior : DependencyObject
    {
        public static readonly DependencyProperty SeriesBinderProperty =
            DependencyProperty.RegisterAttached("SeriesBinder", typeof(SeriesBinderInfo), typeof(SeriesBinderBehavior),
                new PropertyMetadata(null, (o, e) => OnSeriesBiderChanged(o as XamDataChart, e.OldValue as SeriesBinderInfo, e.NewValue as SeriesBinderInfo)));

        public static SeriesBinderInfo GetSeriesBinder(DependencyObject target)
        {
            return target.GetValue(SeriesBinderProperty) as SeriesBinderInfo;
        }

        public static void SetSeriesBinder(DependencyObject target, object value)
        {
            target.SetValue(SeriesBinderProperty, value);
        }

        private static void OnSeriesBiderChanged(XamDataChart chart, SeriesBinderInfo oldVal, SeriesBinderInfo newVal)
        {
            if (chart == null)
            {
                return;
            }
            if (oldVal != null)
            {
                oldVal.DetachOwner(chart);
            }
            if (newVal != null)
            {
                newVal.AttachOwner(chart);
            }
        }
    }

    public class SeriesBinderInfo : FrameworkElement
    {
        #region Fields
        private XamDataChart _owner;
        private Dictionary<object, Series> _seriesObjectMapper;
        #endregion

        #region Constructor

        public SeriesBinderInfo()
        {
            _seriesObjectMapper = new Dictionary<object, Series>();
        }

        #endregion

        #region Initializing Methods

        public void AttachOwner(XamDataChart chart)
        {
            if (_owner != null)
            {
                DetachOwner(_owner);
            }
            _owner = chart;
            SetBinding(DataContextProperty, new Binding("DataContext") { Source = _owner });

        }

        public void DetachOwner(XamDataChart _owner)
        {
            _owner = null;
            _seriesObjectMapper.Clear();
            ClearValue(DataContextProperty);
        }

        #endregion

        #region Properties

        private const string TypePathPropertyName = "TypePath";
        public string TypePath
        {
            get => (string)GetValue(TypePathProperty);
            set => SetValue(TypePathProperty, value);
        }

        public static readonly DependencyProperty TypePathProperty =
            DependencyProperty.Register(
                TypePathPropertyName,
                typeof(string),
                typeof(SeriesBinderInfo),
                new PropertyMetadata(null));

        private const string SeriesSourcePropertyName = "SeriesSource";
        public IEnumerable SeriesSource
        {
            get => (IEnumerable)GetValue(SeriesSourceProperty);
            set => SetValue(SeriesSourceProperty, value);
        }

        public static readonly DependencyProperty SeriesSourceProperty =
            DependencyProperty.Register(
                SeriesSourcePropertyName,
                typeof(IEnumerable),
                typeof(SeriesBinderInfo),
                new PropertyMetadata(null));

        private const string ItemsSourcePathPropertyName = "ItemsSourcePath";
        public string ItemsSourcePath
        {
            get => (string)GetValue(ItemsSourcePathProperty);
            set => SetValue(ItemsSourcePathProperty, value);
        }

        public static readonly DependencyProperty ItemsSourcePathProperty =
            DependencyProperty.Register(
                ItemsSourcePathPropertyName,
                typeof(string),
                typeof(SeriesBinderInfo),
                new PropertyMetadata(null));

        private const string TitlePropertyName = "Title";
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                    TitlePropertyName,
                    typeof(string),
                    typeof(SeriesBinderInfo),
                    new PropertyMetadata(null));

        private const string XMemberPathPropertyName = "XMemberPath";
        public string XMemberPath
        {
            get => (string)GetValue(XMemberPathProperty);
            set => SetValue(XMemberPathProperty, value);
        }

        public static readonly DependencyProperty XMemberPathProperty =
            DependencyProperty.Register(
                    XMemberPathPropertyName,
                    typeof(string),
                    typeof(SeriesBinderInfo),
                    new PropertyMetadata(null));

        private const string YMemberPathPropertyName = "YMemberPath";
        public string YMemberPath
        {
            get => (string)GetValue(YMemberPathProperty);
            set => SetValue(YMemberPathProperty, value);
        }

        public static readonly DependencyProperty YMemberPathProperty =
            DependencyProperty.Register(
                    YMemberPathPropertyName,
                    typeof(string),
                    typeof(SeriesBinderInfo),
                    new PropertyMetadata(null));

        private const string MarkerTypePropertyName = "MarkerType";
        public string MarkerType
        {
            get => (string)GetValue(MarkerTypeProperty);
            set => SetValue(MarkerTypeProperty, value);
        }

        public static readonly DependencyProperty MarkerTypeProperty =
            DependencyProperty.Register(
                    MarkerTypePropertyName,
                    typeof(string),
                    typeof(SeriesBinderInfo),
                    new PropertyMetadata(null));

        private const string MarkerOutlinePropertyName = "MarkerOutline";
        public string MarkerOutline
        {
            get => (string)GetValue(MarkerOutlineProperty);
            set => SetValue(MarkerOutlineProperty, value);
        }

        public static readonly DependencyProperty MarkerOutlineProperty =
            DependencyProperty.Register(
                    MarkerOutlinePropertyName,
                    typeof(string),
                    typeof(SeriesBinderInfo),
                    new PropertyMetadata(null));

        private const string MarkerBrushPropertyName = "MarkerBrush";
        public string MarkerBrush
        {
            get => (string)GetValue(MarkerBrushProperty);
            set => SetValue(MarkerBrushProperty, value);
        }

        public static readonly DependencyProperty MarkerBrushProperty =
            DependencyProperty.Register(
                    MarkerBrushPropertyName,
                    typeof(string),
                    typeof(SeriesBinderInfo),
                    new PropertyMetadata(null));

        #endregion

        #region Event Handlers

        private void SeriesBinderInfo_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        object seriesSource = e.NewItems[0];
                        AddSeries(seriesSource);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        object seriesSource = e.OldItems[0];
                        RemoveSeries(seriesSource);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    {
                        GenerateSeries();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Helper Methods

        private void RemoveSeries(object seriesSource)
        {
            if (_seriesObjectMapper.ContainsKey(seriesSource))
            {
                var type = (SeriesType)seriesSource.GetType().GetProperty(TypePath).GetValue(seriesSource, null);
                Series s = _seriesObjectMapper[seriesSource];
                UpdateSeriesProperties(s, type, seriesSource, true);
                _owner.Series.Remove(s);
                _seriesObjectMapper.Remove(seriesSource);
            }
        }

        private void AddSeries(object seriesSource)
        {
            if (!_seriesObjectMapper.ContainsKey(seriesSource))
            {
                var type = (SeriesType)seriesSource.GetType().GetProperty(TypePath).GetValue(seriesSource, null);
                Series series = CreateSeries(type, seriesSource);
                _owner.Series.Add(series);
                _seriesObjectMapper.Add(seriesSource, series);
            }
        }

        private void GenerateSeries()
        {
            _owner.Series.Clear();
            foreach (object item in SeriesSource)
            {
                AddSeries(item);
            }
        }

        private Series CreateSeries(SeriesType type, object seriesSource)
        {
            Series series = null;
            switch (type)
            {
                case SeriesType.LineSeries:
                    {
                        series = new LineSeries();
                    }
                    break;
                case SeriesType.ColumnSeries:
                    {
                        series = new ColumnSeries();
                    }
                    break;
                case SeriesType.ScatterSeries:
                    {
                        series = new ScatterSeries();
                        var scatter = series as ScatterSeries;
                        scatter.MaximumMarkers = 5000;
                    }
                    break;
                case SeriesType.ScatterLineSeries:
                    {
                        series = new ScatterLineSeries();
                        var scatter = series as ScatterLineSeries;
                        scatter.MaximumMarkers = 5000;
                    }
                    break;
                default:
                    break;
            }
            //series.Resolution = 1.5;
            UpdateSeriesProperties(series, type, seriesSource, false);

            return series;
        }

        private void UpdateSeriesProperties(Series series, SeriesType type, object seriesSource, bool clearOnly)
        {
            if (series != null)
            {
                switch (type)
                {
                    case SeriesType.LineSeries:
                    case SeriesType.ColumnSeries:
                        {
                            var category = series as HorizontalAnchoredCategorySeries;
                            category.ClearValue(HorizontalAnchoredCategorySeries.TitleProperty);
                            category.ClearValue(HorizontalAnchoredCategorySeries.ValueMemberPathProperty);
                            category.ClearValue(HorizontalAnchoredCategorySeries.ItemsSourceProperty);
                            category.ClearValue(HorizontalAnchoredCategorySeries.BrushProperty);
                            category.ClearValue(HorizontalAnchoredCategorySeries.OutlineProperty);

                            if (!clearOnly)
                            {
                                category.ShowDefaultTooltip = true;

                                category.SetBinding(HorizontalAnchoredCategorySeries.TitleProperty,
                                    new Binding(Title) { Source = seriesSource });

                                category.SetBinding(HorizontalAnchoredCategorySeries.ValueMemberPathProperty,
                                    new Binding(YMemberPath) { Source = seriesSource });

                                category.SetBinding(HorizontalAnchoredCategorySeries.ItemsSourceProperty,
                                    new Binding(ItemsSourcePath) { Source = seriesSource });

                                category.SetBinding(HorizontalAnchoredCategorySeries.BrushProperty,
                                    new Binding(MarkerBrush) { Source = seriesSource });

                                category.SetBinding(HorizontalAnchoredCategorySeries.OutlineProperty,
                                    new Binding(MarkerOutline) { Source = seriesSource });

                                foreach (Axis axis in _owner.Axes)
                                {
                                    if (axis is CategoryXAxis categoryXAxis)
                                    {
                                        category.XAxis = categoryXAxis;
                                    }
                                    else if (axis is NumericYAxis numericYAxis)
                                    {
                                        category.YAxis = numericYAxis;
                                    }
                                }
                            }
                        }
                        break;
                    case SeriesType.ScatterSeries:
                    case SeriesType.ScatterLineSeries:
                        {
                            var scatter = series as ScatterBase;
                            scatter.ClearValue(ScatterBase.TitleProperty);
                            scatter.ClearValue(ScatterBase.XMemberPathProperty);
                            scatter.ClearValue(ScatterBase.YMemberPathProperty);
                            scatter.ClearValue(ScatterBase.ItemsSourceProperty);
                            scatter.ClearValue(ScatterBase.MarkerTypeProperty);
                            scatter.ClearValue(ScatterBase.MarkerBrushProperty);
                            scatter.ClearValue(ScatterBase.MarkerOutlineProperty);

                            if (!clearOnly)
                            {
                                scatter.SetBinding(ScatterBase.TitleProperty,
                                    new Binding(Title) { Source = seriesSource });

                                scatter.SetBinding(ScatterBase.XMemberPathProperty,
                                    new Binding(XMemberPath) { Source = seriesSource });

                                scatter.SetBinding(ScatterBase.YMemberPathProperty,
                                    new Binding(YMemberPath) { Source = seriesSource });

                                scatter.SetBinding(ScatterBase.ItemsSourceProperty,
                                    new Binding(ItemsSourcePath) { Source = seriesSource });

                                scatter.SetBinding(ScatterBase.MarkerTypeProperty,
                                    new Binding(MarkerType) { Source = seriesSource });

                                scatter.SetBinding(ScatterBase.MarkerBrushProperty,
                                    new Binding(MarkerBrush) { Source = seriesSource });

                                scatter.SetBinding(ScatterBase.MarkerOutlineProperty,
                                    new Binding(MarkerOutline) { Source = seriesSource });

                                foreach (Axis axis in _owner.Axes)
                                {
                                    if (axis is NumericXAxis numericXAxis)
                                    {
                                        scatter.XAxis = numericXAxis;
                                    }
                                    else if (axis is NumericYAxis numericYAxis)
                                    {
                                        scatter.YAxis = numericYAxis;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Base Overrides

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case SeriesSourcePropertyName:
                    {
                        if (e.OldValue != null
                            && e.OldValue is INotifyCollectionChanged)
                        {
                            (e.OldValue as INotifyCollectionChanged).CollectionChanged -= SeriesBinderInfo_CollectionChanged;
                        }

                        if (e.NewValue != null
                            && e.NewValue is INotifyCollectionChanged)
                        {
                            (e.NewValue as INotifyCollectionChanged).CollectionChanged += SeriesBinderInfo_CollectionChanged;
                        }

                        GenerateSeries();
                    }
                    break;
                case XMemberPathPropertyName:
                case YMemberPathPropertyName:
                case TypePathPropertyName:
                    {
                        if (SeriesSource != null)
                        {
                            GenerateSeries();
                        }
                    }
                    break;
                default:
                    break;
            }
            base.OnPropertyChanged(e);
        }

        #endregion
    }

    public class SeriesViewModel : INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string name;
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        private SeriesType type;
        public SeriesType Type
        {
            get => type;
            set => Set(ref type, value);
        }

        private IEnumerable source;
        public IEnumerable Source
        {
            get => source;
            set => Set(ref source, value);
        }

        private string xPath;
        public string XPath
        {
            get => xPath;
            set => Set(ref xPath, value);
        }

        private string yPath;
        public string YPath
        {
            get => yPath;
            set => Set(ref yPath, value);
        }

        private MarkerType pointType;
        public MarkerType PointType
        {
            get => pointType;
            set => Set(ref pointType, value);
        }

        private Brush pointOutLine;
        public Brush PointOutLine
        {
            get => pointOutLine;
            set => Set(ref pointOutLine, value);
        }

        private Brush pointBrush;
        public Brush PointBrush
        {
            get => pointBrush;
            set => Set(ref pointBrush, value);
        }
    }
}