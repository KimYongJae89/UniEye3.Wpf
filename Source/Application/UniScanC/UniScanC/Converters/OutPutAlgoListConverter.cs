using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Unieye.WPF.Base.Helpers;
using UniScanC.Algorithm;
using UniScanC.Algorithm.Base;
using UniScanC.AlgoTask;
using UniScanC.Data;
using UniScanC.Struct;

namespace UniScanC.Converters
{
    public class OutPutAlgoListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is LinkS links))
            {
                return null;
            }

            if (values[1] == null)
            {
                return null;
            }
            else if (string.IsNullOrWhiteSpace(values[1].ToString()))
            {
                return null;
            }

            var selectedLinksDstPortTypeList = new List<(string, Type)>();
            if (links.DstUnitName == "InspectResult")
            {
                var inspectResult = new InspectResult();
                selectedLinksDstPortTypeList = inspectResult.GetPropNameTypes().ToList();
            }
            else
            {
                if (!(values[2] is AlgoModel selectedAlgo))
                {
                    return null;
                }

                INodeParam algoParam = selectedAlgo.AlgoParam;
                if (algoParam.GetType().Name.Equals(typeof(SetNodeParam<object>).Name))
                {
                    Type baseListType = typeof(List<>);
                    Type genericType = algoParam.GetType().GetGenericArguments().FirstOrDefault();
                    Type defineConstructor = baseListType.GetGenericTypeDefinition();
                    Type genericConstructor = defineConstructor.MakeGenericType(new Type[] { genericType });
                    System.Reflection.ConstructorInfo constructor = genericConstructor.GetConstructor(Type.EmptyTypes);

                    selectedLinksDstPortTypeList.Add(("ListArray", constructor.DeclaringType));
                }
                else
                {
                    selectedLinksDstPortTypeList = algoParam.InPropNameTypes.ToList();
                }
            }

            Type selectedLinksDstPortType = selectedLinksDstPortTypeList.Find(x => x.Item1 == links.DstPortName).Item2;
            if (selectedLinksDstPortType != null)
            {
                if (links.SrcUnitName == "ModuleImageDataByte")
                {
                    return ModuleImageDataByte.GetProps().Where(x => x.Item2.FullName.Equals(selectedLinksDstPortType.FullName)).Select(f => f.Item1).ToList();
                }
                else
                {
                    if (!(values[3] is IEnumerable<AlgoModel> algoList))
                    {
                        return null;
                    }

                    AlgoModel algo = algoList.FirstOrDefault(x => x.AlgoParam.Name == links.SrcUnitName);
                    if (algo != null)
                    {
                        var list = new List<string>();
                        list.AddRange(algo.AlgoParam.OutPropNameTypes.Where(x => x.Item2.FullName.Equals(selectedLinksDstPortType.FullName)).Select(f => f.Item1).ToList());
                        list.AddRange(algo.AlgoParam.OutPropNameTypes.Where(x => AlgoLinkConverter.IsExistConverter(x.Item2, selectedLinksDstPortType)).Select(f => f.Item1).ToList());
                        return list;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
