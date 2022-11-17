using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using UniScanC.Enums;

namespace UniScanC.Data
{
    public class Defect
    {
        public string InspectTime { get; set; }
        public int ModuleNo { get; set; }
        public int FrameIndex { get; set; }
        public int DefectNo { get; set; }

        public string DefectTypeName { get; set; }
        public EDefectType DefectType { get; set; }
        public List<DefectCategory> DefectCategories { get; set; } = new List<DefectCategory>();

        public RectangleF BoundingRect { get; set; }
        public double Area { get; set; }
        public int MinGv { get; set; }
        public int MaxGv { get; set; }
        public int AvgGv { get; set; }

        public PointF DefectPos { get; set; }
        public float Edge { get; set; }

        public BitmapSource DefectImage { get; set; }
        public BitmapSource BinaryImage { get; set; }

        public string DefectImagePath { get; set; }
        public object Tag { get; set; }

        public RectangleF ImageClipRect { get; set; }
        public RectangleF TouchHeadRect { get; set; }
        public RectangleF TouchTailRect { get; set; }

        public bool IsSkip => DefectCategories.Find(x => x.IsSkip = true) != null;
        public bool IsOnBorder => !TouchHeadRect.IsEmpty || !TouchTailRect.IsEmpty;
        public bool IsOnUpperBorder => !TouchHeadRect.IsEmpty;
        public bool IsOnLowerBorder => !TouchTailRect.IsEmpty;

        public Defect() { }

        public Defect(Defect src)
        {
            InspectTime = src.InspectTime;
            ModuleNo = src.ModuleNo;
            FrameIndex = src.FrameIndex;
            DefectNo = src.DefectNo;

            DefectTypeName = src.DefectTypeName;
            DefectType = src.DefectType;
            DefectCategories.Clear();
            foreach (DefectCategory defectCategory in src.DefectCategories)
            {
                var tempDefectCategory = new DefectCategory("");
                tempDefectCategory.CopyFrom(defectCategory);
                DefectCategories.Add(tempDefectCategory);
            }

            BoundingRect = src.BoundingRect;
            Area = src.Area;
            MinGv = src.MinGv;
            MaxGv = src.MaxGv;
            AvgGv = src.AvgGv;

            DefectPos = src.DefectPos;
            Edge = src.Edge;

            DefectImage = src.DefectImage?.Clone();
            DefectImage?.Freeze();
            BinaryImage = src.BinaryImage?.Clone();
            BinaryImage?.Freeze();

            DefectImagePath = src.DefectImagePath;
            Tag = src.Tag;

            ImageClipRect = src.ImageClipRect;
            TouchHeadRect = src.TouchHeadRect;
            TouchTailRect = src.TouchHeadRect;
        }

        public DefectCategory TopDefectCategory()
        {
            if (DefectCategories == null || DefectCategories.Count == 0)
            {
                return null;
            }

            return DefectCategories.OrderBy(x => x.WarningLevel).First();
        }

        public void Dispose()
        {
            DefectImage = null;
            BinaryImage = null;
        }

        public Defect Clone()
        {
            return new Defect(this);
        }

        public void CategorySearch(List<DefectCategory> modelDefectCategories, SizeF resolution, bool isOthersDefect = true)
        {
            if (modelDefectCategories != null)
            {
                foreach (DefectCategory category in modelDefectCategories.OrderBy(x => x.WarningLevel))
                {
                    foreach (IGrouping<bool, CategoryType> categoryTypePair in category.CategoryTypeList.ToLookup(x => x.Use))
                    {
                        if (categoryTypePair.Key == false)
                        {
                            continue;
                        }

                        bool isValid = true;
                        foreach (CategoryType categoryType in categoryTypePair)
                        {
                            switch (categoryType.Type)
                            {
                                case ECategoryTypeName.EdgeLower:
                                    {
                                        float centerX = (BoundingRect.X + BoundingRect.Width) / 2;
                                        isValid = Math.Abs(Edge - centerX) * resolution.Width < Convert.ToSingle(categoryType.Data);
                                    }
                                    break;
                                case ECategoryTypeName.EdgeUpper:
                                    {
                                        float centerX = (BoundingRect.X + BoundingRect.Width) / 2;
                                        isValid = Math.Abs(Edge - centerX) * resolution.Width > Convert.ToSingle(categoryType.Data);
                                    }
                                    break;
                                case ECategoryTypeName.AreaLower:
                                    isValid = Area * resolution.Width * resolution.Height < Convert.ToDouble(categoryType.Data);
                                    break;
                                case ECategoryTypeName.AreaUpper:
                                    isValid = Area * resolution.Width * resolution.Height > Convert.ToDouble(categoryType.Data);
                                    break;
                                case ECategoryTypeName.AvgGvLower:
                                    isValid = AvgGv < Convert.ToInt32(categoryType.Data);
                                    break;
                                case ECategoryTypeName.AvgGvUpper:
                                    isValid = AvgGv > Convert.ToInt32(categoryType.Data);
                                    break;
                                case ECategoryTypeName.MinGvLower:
                                    isValid = MinGv < Convert.ToInt32(categoryType.Data);
                                    break;
                                case ECategoryTypeName.MinGvUpper:
                                    isValid = MinGv > Convert.ToInt32(categoryType.Data);
                                    break;
                                case ECategoryTypeName.MaxGvLower:
                                    isValid = MaxGv < Convert.ToInt32(categoryType.Data);
                                    break;
                                case ECategoryTypeName.MaxGvUpper:
                                    isValid = MaxGv > Convert.ToInt32(categoryType.Data);
                                    break;
                                case ECategoryTypeName.WidthLower:
                                    isValid = BoundingRect.Width * resolution.Width < Convert.ToDouble(categoryType.Data);
                                    break;
                                case ECategoryTypeName.WidthUpper:
                                    isValid = BoundingRect.Width * resolution.Width > Convert.ToDouble(categoryType.Data);
                                    break;
                                case ECategoryTypeName.HeightLower:
                                    isValid = BoundingRect.Height * resolution.Height < Convert.ToDouble(categoryType.Data);
                                    break;
                                case ECategoryTypeName.HeightUpper:
                                    isValid = BoundingRect.Height * resolution.Height > Convert.ToDouble(categoryType.Data);
                                    break;
                            }

                            if (!isValid)
                            {
                                break;
                            }
                        }

                        if (isValid)
                        {
                            DefectCategories.Add(new DefectCategory(category));
                            return;
                        }
                    }
                }
            }

            if (isOthersDefect == true && modelDefectCategories.Find(x => x.Name == "OTHERS") == null)
            {
                DefectCategories.Add(DefectCategory.GetDefaultCategory());
            }
        }
    }
}
