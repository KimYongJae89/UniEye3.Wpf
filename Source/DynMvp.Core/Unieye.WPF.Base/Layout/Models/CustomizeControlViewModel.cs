using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Layout.Models
{
    public interface INotifyModelChanged
    {
        void OnUpdateModel(ModelBase modelBase);
    }

    public interface INotifyProductResultChanged
    {
        void OnUpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null);
    }

    public delegate void SelectedDefectUpdateDelegate(object SelectedDefect);
    public interface INotifySelectedDefectChanged
    {
        SelectedDefectUpdateDelegate SelectedDefectUpdate { get; set; }
        void OnUpdateSelectedDefect(object selectedDefect);
    }

    public delegate void PatternWidthUpdateDelegate(double patternWidth);
    public interface INotifyPatternWidthChanged
    {
        void OnUpdatePatternWidth(double patternWidth);
    }

    public interface INotifyLogChanged
    {
        void NotifyLog(LogLevel logLevel, LoggerType loggerType, string message);
    }

    public class CustomizeControlViewModel : Observable
    {
        #region 생성자
        public CustomizeControlViewModel(Type controlViewModelType)
        {
            PropertyInfo[] properties = controlViewModelType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CustomAttributes.Count(x => x.AttributeType == typeof(LayoutControlViewModelPropertyAttribute)) > 0)
                {
                    Set(property);
                }
            }
        }
        #endregion


        #region 속성
        public Dictionary<PropertyInfo, object> PropertyList { get; set; } = new Dictionary<PropertyInfo, object>();
        #endregion


        #region 메서드
        public void Set(PropertyInfo property, object value = null)
        {
            if (value == null)
            {
                value = property.GetValue(this);
            }

            if (property.PropertyType.IsEnum && !(value is Enum))
            {
                Array arrayValues = Enum.GetValues(property.PropertyType);
                value = arrayValues.GetValue((long)value);
            }

            if (!PropertyList.ContainsKey(property))
            {
                PropertyList.Add(property, value);
            }
            else
            {
                PropertyList[property] = value;
            }

            property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
        }

        public virtual UserControl CreateControlView()
        {
            return new UserControl();
        }

        public static List<Type> GetControlProperties()
        {
            return ReflectionHelper.FindAllInheritedTypes<CustomizeControlViewModel>();
        }
        #endregion
    }
}
