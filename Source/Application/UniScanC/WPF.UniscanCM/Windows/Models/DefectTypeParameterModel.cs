using System;
using System.Collections.Generic;
using UniScanC.Data;
using UniScanC.Enums;

namespace WPF.UniScanCM.Windows.Models
{
    public class DefectTypeParameterModel
    {
        public List<Tuple<CategoryType, string>> CategoryTypeList { get; set; } = new List<Tuple<CategoryType, string>>();
        public string CategoryName { get; set; }

        public static IEnumerable<DefectTypeParameterModel> CreateModel(List<DefectCategory> defectCategories)
        {
            var defectTypeParameterModels = new List<DefectTypeParameterModel>();

            foreach (DefectCategory category in defectCategories)
            {
                var model = new DefectTypeParameterModel();
                model.CategoryName = category.Name;
                foreach (CategoryType categoryType in category.CategoryTypeList.FindAll(x => x.Use))
                {
                    object data = 0;
                    string unit = "";
                    switch (categoryType.Type)
                    {
                        case ECategoryTypeName.EdgeLower:
                        case ECategoryTypeName.EdgeUpper:
                        case ECategoryTypeName.WidthLower:
                        case ECategoryTypeName.WidthUpper:
                        case ECategoryTypeName.HeightLower:
                        case ECategoryTypeName.HeightUpper: data = string.Format("{0:0.00}", Convert.ToDouble(categoryType.Data) / 1000.0); unit = "mm"; break;

                        case ECategoryTypeName.AreaLower:
                        case ECategoryTypeName.AreaUpper: data = string.Format("{0:0.00}", Convert.ToDouble(categoryType.Data) / 1000.0); unit = "mm²"; break;

                        case ECategoryTypeName.MinGvLower:
                        case ECategoryTypeName.MinGvUpper:
                        case ECategoryTypeName.MaxGvLower:
                        case ECategoryTypeName.MaxGvUpper:
                        case ECategoryTypeName.AvgGvLower:
                        case ECategoryTypeName.AvgGvUpper: data = categoryType.Data; unit = ""; break;
                    }

                    var newCategoryType = new CategoryType(categoryType);
                    newCategoryType.Data = data;

                    var tuple = new Tuple<CategoryType, string>(newCategoryType, unit);
                    model.CategoryTypeList.Add(tuple);
                }

                defectTypeParameterModels.Add(model);
            }

            return defectTypeParameterModels;
        }
    }
}
