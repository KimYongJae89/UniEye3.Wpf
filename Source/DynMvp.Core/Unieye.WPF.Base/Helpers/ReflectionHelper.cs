using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unieye.WPF.Base.Helpers
{
    public static class ReflectionHelper
    {
        public static List<Type> FindAllInheritedTypes<T>()
        {
            var typeList = new List<Type>();

            Type ParentType = typeof(T);

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                typeList.AddRange(GetLoadableTypes(assembly).Where(x => x != null && x.IsClass && !x.IsAbstract && /*x != ParentType && */x.IsSubclassOf(ParentType)).ToList());
            }

            return typeList;
        }

        // InspectionFlow 에서 사용
        public static List<Type> FindAllInheritedTypes(Type type)
        {
            var typeList = new List<Type>();

            Type ParentType = type;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                typeList.AddRange(GetLoadableTypes(assembly)?.Where(x => x != null && x.IsClass && !x.IsAbstract && /*x != ParentType && */x.IsSubclassOf(ParentType)).ToList());
            }

            return typeList;
        }

        public static List<Type> FindAllInheritedTypesDefinedAttribute(Type type)
        {
            var typeList = new List<Type>();

            Type ParentType = type;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var list = GetLoadableTypes(assembly)?.Where(x => x != null && x.IsClass).ToList();
                IEnumerable<Type> sortList = list.Where(x => x.CustomAttributes.Count(y => y.AttributeType == type) > 0);

                typeList.AddRange(sortList);
            }

            return typeList;
        }

        public static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
