using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class GridParam
    {
        public bool UseGrid { get; set; }
        public int RowCount { get; set; } = 2;
        public int ColumnCount { get; set; } = 2;
        public int CellAcceptRatio { get; set; } = 50;

        public GridParam Clone()
        {
            var param = new GridParam();

            param.Copy(this);

            return param;
        }

        public void Copy(GridParam gridParam)
        {
            UseGrid = gridParam.UseGrid;
            RowCount = gridParam.RowCount;
            ColumnCount = gridParam.ColumnCount;
            CellAcceptRatio = gridParam.CellAcceptRatio;
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            UseGrid = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseGrid", "False"));
            RowCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RowCount", "1"));
            ColumnCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ColumnCount", "1"));
            CellAcceptRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CellAcceptRatio", "1"));
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "UseGrid", UseGrid.ToString());
            XmlHelper.SetValue(algorithmElement, "RowCount", RowCount.ToString());
            XmlHelper.SetValue(algorithmElement, "ColumnCount", ColumnCount.ToString());
            XmlHelper.SetValue(algorithmElement, "CellAcceptRatio", CellAcceptRatio.ToString());
        }

        public int GetNumCol()
        {
            if (UseGrid)
            {
                return ColumnCount;
            }

            return 1;
        }

        public int GetNumRow()
        {
            if (UseGrid)
            {
                return RowCount;
            }

            return 1;
        }
    }
}
