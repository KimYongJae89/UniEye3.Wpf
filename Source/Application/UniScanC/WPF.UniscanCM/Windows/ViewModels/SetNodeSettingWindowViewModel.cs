using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Algorithm.Base;
using UniScanC.Data;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class SetNodeSettingWindowViewModel
    {
        #region 생성자
        public SetNodeSettingWindowViewModel()
        {
            OKCommand = new RelayCommand<ChildWindow>(OKCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            SelectableNodeTypeList = Enum.GetNames(typeof(ESetNodeType)).ToList();
            SelectableTypeList = new List<Type>() { typeof(Defect) };

            SelectedNodeType = ESetNodeType.Union.ToString();
            SelectedType = typeof(Defect);
        }

        private void OKCommandAction(ChildWindow wnd)
        {
            wnd.Close(MakeSetNodeParam());
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(null);
        }
        #endregion


        #region 속성
        public ICommand OKCommand { get; }
        public ICommand CancelCommand { get; }
        public ConstructorInfo ConstructorInfo { get; set; }

        public List<string> SelectableNodeTypeList { get; set; }
        public string SelectedNodeType { get; set; }
        public List<Type> SelectableTypeList { get; set; }
        public Type SelectedType { get; set; }
        #endregion


        #region 메서드
        private IAlgorithmBaseParam MakeSetNodeParam()
        {
            var nodeType = (ESetNodeType)Enum.Parse(typeof(ESetNodeType), SelectedNodeType);
            Type algoParam = new SetNodeParam<object>().GetType();
            Type defineConstructor = algoParam.GetGenericTypeDefinition();
            Type genericConstructor = defineConstructor.MakeGenericType(new Type[] { SelectedType });
            ConstructorInfo constructor = genericConstructor.GetConstructor(new Type[] { typeof(ESetNodeType) });
            var setNodeParam = constructor.Invoke(new object[] { nodeType }) as IAlgorithmBaseParam;
            return setNodeParam;
        }
        #endregion
    }
}
