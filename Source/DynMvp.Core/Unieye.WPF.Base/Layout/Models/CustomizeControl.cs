using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Layout.Models
{
    public class CustomizeControl : UserControl
    {
        private ImageSource imageSource = null;
        public ImageSource ImageSource
        {
            get
            {
                if (imageSource == null)
                {
                    string typeName = GetType().Name;
                    System.Reflection.Assembly assem = GetType().Assembly;

                    string baseUri = string.Format("pack://application:,,,/{0};component", assem.GetName().Name);
                    string imageUri = string.Format("Resources/{0}.png", typeName);

                    if (Uri.TryCreate(imageUri, UriKind.Relative, out Uri uri))
                    {
                        imageSource = new System.Windows.Media.Imaging.BitmapImage(uri);
                    }
                }

                return imageSource;
            }
        }

        public Dictionary<DependencyProperty, object> PropertyList { get; set; } = new Dictionary<DependencyProperty, object>();

        public void Set(DependencyProperty property, object value = null)
        {
            if (value == null)
            {
                value = GetValue(property);
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
        }

        public static List<Type> GetControlProperties()
        {
            return ReflectionHelper.FindAllInheritedTypes<CustomizeControl>();
        }
    }
}
