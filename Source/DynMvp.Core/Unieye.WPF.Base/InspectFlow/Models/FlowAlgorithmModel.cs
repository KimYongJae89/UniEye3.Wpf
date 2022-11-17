using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.InspectFlow.Models
{
    public class AlgorithmParameter : WTuple<string, object>
    {
        public AlgorithmParameter(string item1, object item2) : base(item1, item2) { }
    }

    public abstract class FlowAlgorithmModel
    {
        public AlgoImage BufferImage { get; set; } = null;

        [FlowAlgorithmParameter]
        public ImagingLibrary ImagingLibrary { get; set; } = ImagingLibrary.MatroxMIL;

        public abstract AlgoImage Inspect(AlgoImage srcImage);

        public List<AlgorithmParameter> ParameterList { get; set; } = new List<AlgorithmParameter>();

        public static List<Type> GetAlgorithmTypeList()
        {
            return ReflectionHelper.FindAllInheritedTypes<FlowAlgorithmModel>();
        }

        public FlowAlgorithmModel()
        {
            GenerateParameterDicationary();
        }

        private void GenerateParameterDicationary()
        {
            ParameterList.Clear();

            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.IsDefined(typeof(FlowAlgorithmParameterAttribute)))
                {
                    object propertyValue = property.GetValue(this, null);
                    Insert(propertyValue, property.Name);
                }
            }
        }

        public void UpdateParameter()
        {
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (AlgorithmParameter parameter in ParameterList)
            {
                PropertyInfo item = properties.FirstOrDefault(x => x.IsDefined(typeof(FlowAlgorithmParameterAttribute)) && x.Name == parameter.Item1);
                if (item != null)
                {
                    item.SetValue(this, Convert.ChangeType(parameter.Item2, item.PropertyType));
                }
            }
        }

        public void Insert(object parameter, string parameterName)
        {
            AlgorithmParameter AlgorithmParameter = ParameterList.Find(x => x.Item1 == parameterName);
            if (AlgorithmParameter == null)
            {
                ParameterList.Add(new AlgorithmParameter(parameterName, parameter));
            }
            else
            {
                AlgorithmParameter.Item2 = parameter;
            }
        }

        public AlgoImage CloneImage(AlgoImage srcImage)
        {
            AlgoImage dstImage;

            if (srcImage.LibraryType != ImagingLibrary)
            {
                var imageBuilder = ImageBuilder.GetInstance(ImagingLibrary);
                dstImage = imageBuilder.Build(ImageType.Grey, srcImage.Width, srcImage.Height);
                dstImage.SetByte(srcImage.GetByte());
            }
            else
            {
                dstImage = srcImage.Clone();
            }

            return dstImage;
        }
    }
}
