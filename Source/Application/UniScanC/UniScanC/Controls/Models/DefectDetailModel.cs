using System.Collections.Generic;
using System.Linq;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Module;

namespace UniScanC.Controls.Models
{
    public class DefectDetailModel
    {
        public CategoryType CategoryType { get; set; } = null;
        public DefectCategory DefectCategory { get; set; } = null;
        public string Caption { get; set; } = string.Empty;
        public object Value { get; set; } = null;
        public bool IsValid { get; set; } = false;

        public DefectDetailModel(string caption, object value)
        {
            Caption = caption;
            if (value != null && (value.GetType() == typeof(double) || value.GetType() == typeof(float)))
            {
                Value = string.Format("{0:0.000}", value);
            }
            else
            {
                Value = value;
            }
        }

        public DefectDetailModel(DefectCategory category, string caption, object value, CategoryType categoryType, bool isValid)
        {
            DefectCategory = category;
            Caption = caption;
            if (value != null && (value.GetType() == typeof(double) || value.GetType() == typeof(float)))
            {
                Value = string.Format("{0:0.000}", value);
            }
            else
            {
                Value = value;
            }

            CategoryType = categoryType;

            IsValid = isValid;
        }

        public static IEnumerable<DefectDetailModel> CreateModel(Defect defect, DefectCategory DefectCategory, bool isEditParmeter)
        {
            if (DefectCategory == null)
            {
                return null;
            }

            var inspectResult = defect.Tag as InspectResult;
            var modelList = new List<DefectDetailModel>();

            InspectModuleState inspectModuleState = ModuleManager.Instance.ModuleStateList.Where(x => x is InspectModuleState).Cast<InspectModuleState>().FirstOrDefault(x => x.ModuleIndex == defect.ModuleNo);
            if (inspectModuleState != null)
            {
                List<CategoryType> typeList = DefectCategory.CategoryTypeList.FindAll(x => x.Use);

                modelList.Add(new DefectDetailModel("Inspect_Time", defect.InspectTime));
                modelList.Add(new DefectDetailModel("Defect_No", defect.DefectNo));
                modelList.Add(new DefectDetailModel("Defect_Type", DefectCategory.Name));
                modelList.Add(new DefectDetailModel(DefectCategory, "PosX_MM", defect.DefectPos.X, null, false));
                modelList.Add(new DefectDetailModel(DefectCategory, "PosY_M", defect.DefectPos.Y, null, false));
                modelList.Add(new DefectDetailModel(DefectCategory, "Width_MM", defect.BoundingRect.Width,
                    GetCategoryType(typeList, ECategoryTypeName.WidthLower, ECategoryTypeName.WidthUpper),
                    isEditParmeter && typeList.Exists(x => x.Type == ECategoryTypeName.WidthLower || x.Type == ECategoryTypeName.WidthUpper)));
                modelList.Add(new DefectDetailModel(DefectCategory, "Height_MM", defect.BoundingRect.Height,
                    GetCategoryType(typeList, ECategoryTypeName.HeightLower, ECategoryTypeName.HeightUpper),
                    isEditParmeter && typeList.Exists(x => x.Type == ECategoryTypeName.HeightLower || x.Type == ECategoryTypeName.HeightUpper)));
                modelList.Add(new DefectDetailModel(DefectCategory, "Area_MM2", defect.Area,
                    GetCategoryType(typeList, ECategoryTypeName.AreaLower, ECategoryTypeName.AreaUpper),
                    isEditParmeter && typeList.Exists(x => x.Type == ECategoryTypeName.AreaLower || x.Type == ECategoryTypeName.AreaUpper)));
                modelList.Add(new DefectDetailModel(DefectCategory, "Avg_Gv", defect.AvgGv,
                    GetCategoryType(typeList, ECategoryTypeName.AvgGvLower, ECategoryTypeName.AvgGvUpper),
                    isEditParmeter && typeList.Exists(x => x.Type == ECategoryTypeName.AvgGvLower || x.Type == ECategoryTypeName.AvgGvUpper)));
                modelList.Add(new DefectDetailModel(DefectCategory, "Min_Gv", defect.MinGv,
                    GetCategoryType(typeList, ECategoryTypeName.MinGvLower, ECategoryTypeName.MinGvUpper),
                    isEditParmeter && typeList.Exists(x => x.Type == ECategoryTypeName.MinGvLower || x.Type == ECategoryTypeName.MinGvUpper)));
                modelList.Add(new DefectDetailModel(DefectCategory, "Max_Gv", defect.MaxGv,
                    GetCategoryType(typeList, ECategoryTypeName.MaxGvLower, ECategoryTypeName.MaxGvUpper),
                    isEditParmeter && typeList.Exists(x => x.Type == ECategoryTypeName.MaxGvLower || x.Type == ECategoryTypeName.MaxGvUpper)));
            }

            return modelList;
        }

        private static CategoryType GetCategoryType(List<CategoryType> typeList, ECategoryTypeName lower, ECategoryTypeName upper)
        {
            CategoryType data = typeList.Find(x => x.Type == lower);
            if (data != null)
            {
                return data;
            }

            data = typeList.Find(x => x.Type == upper);
            if (data != null)
            {
                return data;
            }

            return null;
        }
    }
}
