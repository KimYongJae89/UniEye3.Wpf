using System;
using System.Collections.Generic;
using System.Windows.Media;
using UniScanC.Enums;

namespace UniScanC.Data
{
    public class DefectCategory
    {
        public string Name { get; set; } = "";
        public EDefectType DefectType { get; set; } = EDefectType.Dust;
        public EDefectMarkerType DefectFigure { get; set; } = 0;
        public Color DefectColor { get; set; } = Colors.Red;
        public int WarningLevel { get; set; } = 1;
        public int DefectCount { get; set; } = 1;
        public bool IsSkip { get; set; } = false;

        public List<CategoryType> CategoryTypeList { get; set; } = new List<CategoryType>();

        public DefectCategory() { }

        public DefectCategory(string name)
        {
            Name = name;
            DefectType = EDefectType.Dust;
            DefectFigure = 0;
            WarningLevel = 5;
            DefectCount = 1;
            DefectColor = Colors.Red;

            foreach (ECategoryTypeName categoryType in Enum.GetValues(typeof(ECategoryTypeName)))
            {
                var type = new CategoryType(categoryType);
                type.Use = false;
                type.Data = (double)0;

                CategoryTypeList.Add(type);
            }
        }

        public DefectCategory(DefectCategory srcCategory)
        {
            CopyFrom(srcCategory);
        }

        public CategoryType GetCategory(ECategoryTypeName typeName)
        {
            return CategoryTypeList.Find(x => x.Type == typeName);
        }

        public void CopyFrom(DefectCategory category)
        {
            Name = category.Name;
            DefectType = category.DefectType;
            DefectFigure = category.DefectFigure;
            DefectColor = category.DefectColor;
            WarningLevel = category.WarningLevel;
            DefectCount = category.DefectCount;
            IsSkip = category.IsSkip;
            CategoryTypeList.Clear();
            foreach (CategoryType categoryType in category.CategoryTypeList)
            {
                var type = new CategoryType(categoryType);
                CategoryTypeList.Add(type);
            }
        }

        private static DefectCategory defaultCategory;
        public static DefectCategory GetDefaultCategory()
        {
            if (defaultCategory == null)
            {
                defaultCategory = new DefectCategory("OTHERS");
                defaultCategory.DefectType = EDefectType.Dust;
                defaultCategory.DefectFigure = EDefectMarkerType.Circle;
                defaultCategory.DefectColor = Colors.DarkRed;
                defaultCategory.WarningLevel = 100;
                defaultCategory.DefectCount = 1;
            }

            return defaultCategory;
        }

        private static DefectCategory colorCategory;
        public static DefectCategory GetColorCategory()
        {
            if (colorCategory == null)
            {
                colorCategory = new DefectCategory("COLOR");
                colorCategory.DefectType = EDefectType.Mixed;
                colorCategory.DefectFigure = EDefectMarkerType.Circle;
                colorCategory.DefectColor = Colors.DarkGreen;
                colorCategory.WarningLevel = 100;
                colorCategory.DefectCount = 1;
            }

            return colorCategory;
        }
    }
}
