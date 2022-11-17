using DynMvp.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Layout.Models
{
    public class LayoutModel : Observable
    {
        private int column = 1;
        public int Column
        {
            get => column;
            set => Set(ref column, value);
        }

        private int columnSpan = 1;
        public int ColumnSpan
        {
            get => columnSpan;
            set => Set(ref columnSpan, value);
        }

        private int row = 1;
        public int Row
        {
            get => row;
            set => Set(ref row, value);
        }

        private int rowSpan = 1;
        public int RowSpan
        {
            get => rowSpan;
            set => Set(ref rowSpan, value);
        }

        private Dictionary<string, object> propertyList = new Dictionary<string, object>();
        public Dictionary<string, object> PropertyList
        {
            get => propertyList;
            set => Set(ref propertyList, value);
        }

        private CustomizeControlViewModel controlViewModel;
        [JsonIgnore]
        public CustomizeControlViewModel ControlViewModel
        {
            get => controlViewModel;
            set => Set(ref controlViewModel, value);
        }

        private Type controlViewModelType;
        public Type ControlViewModelType
        {
            get => controlViewModelType;
            set
            {
                Set(ref controlViewModelType, value);
                if (ControlViewModel == null)
                {
                    ControlViewModel = Activator.CreateInstance(controlViewModelType) as CustomizeControlViewModel;
                }
            }
        }

        public LayoutModel()
        {
        }

        public LayoutModel(Type type)
        {
            ControlViewModelType = type;
        }

        // 저장용 구조체에 있는 값을 컨트롤에 적용한다.
        public void UploadProperty()
        {
            if (ControlViewModel != null)
            {
                foreach (KeyValuePair<string, object> pair in PropertyList)
                {
                    IEnumerable<KeyValuePair<System.Reflection.PropertyInfo, object>> findList = ControlViewModel.PropertyList.Where(x => x.Key.Name == pair.Key);
                    if (findList.Count() > 0)
                    {
                        KeyValuePair<System.Reflection.PropertyInfo, object> find = findList.First();
                        ControlViewModel.Set(find.Key, pair.Value);
                    }
                }
            }
        }

        // Control에 있는 값을 저장하기 위해 구조체로 전달한다.
        public void DownloadProperty()
        {
            PropertyList.Clear();
            foreach (KeyValuePair<System.Reflection.PropertyInfo, object> pair in ControlViewModel.PropertyList)
            {
                propertyList.Add(pair.Key.Name, pair.Value);
            }
        }

        public LayoutModel Clone()
        {
            var newModel = new LayoutModel(ControlViewModel.GetType());
            newModel.Column = Column;
            newModel.ColumnSpan = ColumnSpan;
            newModel.Row = Row;
            newModel.RowSpan = RowSpan;

            foreach (KeyValuePair<string, object> property in PropertyList)
            {
                newModel.PropertyList.Add(property.Key, property.Value);
            }

            return newModel;
        }
    }
}
