using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Gerber
{
    public class DrillStatItem
    {
        public int DrillNum { get; set; } = -1;
        public double DrillSize { get; set; } = 0.0;
        public string DrillUnit { get; set; } = "";
        public int DrillCount { get; set; } = 0;
    }

    /*! Struct holding statistics of drill commands used.  Used in reporting statistics */
    public class DrillStat
    {
        public int LayerCount { get; set; }
        public int Comment { get; set; }
        public int F { get; set; }
        public int G00 { get; set; }
        public int G01 { get; set; }
        public int G02 { get; set; }
        public int G03 { get; set; }
        public int G04 { get; set; }
        public int G05 { get; set; }
        public int G90 { get; set; }
        public int G91 { get; set; }
        public int G93 { get; set; }
        public int GUnknown { get; set; }

        public int M00 { get; set; }
        public int M01 { get; set; }
        public int M18 { get; set; }
        public int M25 { get; set; }
        public int M30 { get; set; }
        public int M31 { get; set; }
        public int M45 { get; set; }
        public int M47 { get; set; }
        public int M48 { get; set; }
        public int M71 { get; set; }
        public int M72 { get; set; }
        public int M95 { get; set; }

        public int M97 { get; set; }
        public int M98 { get; set; }
        public int MUnknown { get; set; }
        public int R { get; set; }
        public int Unknown { get; set; }
        /* used to total up the drill count across all layers/sizes */
        public int TotalCount { get; set; }
        public string Detect { get; set; }

        public List<ErrorItem> ErrorList { get; } = new List<ErrorItem>();
        public List<DrillStatItem> DrillList { get; } = new List<DrillStatItem>();

        public void Clear()
        {
            ErrorList.Clear();
            DrillList.Clear();
        }

        public void AddDrillStat(DrillStat drillStat)
        {
            LayerCount++;

            Comment += drillStat.Comment;
            /* F codes go here */

            G00 += drillStat.G00;
            G01 += drillStat.G01;
            G02 += drillStat.G02;
            G03 += drillStat.G03;
            G04 += drillStat.G04;
            G05 += drillStat.G05;
            G90 += drillStat.G90;
            G91 += drillStat.G91;
            G93 += drillStat.G93;
            GUnknown += drillStat.GUnknown;

            M00 += drillStat.M00;
            M01 += drillStat.M01;
            M18 += drillStat.M18;
            M25 += drillStat.M25;
            M30 += drillStat.M30;
            M31 += drillStat.M31;
            M45 += drillStat.M45;
            M47 += drillStat.M47;
            M48 += drillStat.M48;
            M71 += drillStat.M71;
            M72 += drillStat.M72;
            M95 += drillStat.M95;
            M97 += drillStat.M97;
            M98 += drillStat.M98;
            MUnknown += drillStat.MUnknown;

            R += drillStat.R;

            ErrorList.AddRange(drillStat.ErrorList);

            foreach (DrillStatItem stateItem in drillStat.DrillList)
            {
                AddDrillStatItem(stateItem);
            }

            if (string.IsNullOrEmpty(drillStat.Detect) == false)
            {
                Detect += drillStat.Detect;
            }
        }

        public void AddDrillStatItem(DrillStatItem stateItem)
        {
            AddDrillStatItem(stateItem.DrillNum, stateItem.DrillSize, stateItem.DrillUnit, stateItem.DrillCount);
        }

        public void AddDrillStatItem(int drillNum, double drillSize, string drillUnit, int drillCount)
        {
            DrillStatItem drillStatItem = DrillList.Find(x => x.DrillNum == drillNum);
            if (drillStatItem == null)
            {
                drillStatItem = new DrillStatItem() { DrillNum = drillNum, DrillSize = drillSize, DrillUnit = drillUnit, DrillCount = drillCount };
                DrillList.Add(drillStatItem);
            }
            else
            {
                drillStatItem.DrillCount += drillCount;
            }
        }

        public void IncrementDrillStatItem(int drillNum, double drillSize, string drillUnit)
        {
            DrillStatItem drillStatItem = DrillList.Find(x => x.DrillNum == drillNum);
            if (drillStatItem == null)
            {
                drillStatItem = new DrillStatItem() { DrillNum = drillNum, DrillSize = drillSize, DrillUnit = drillUnit, DrillCount = 1 };
                DrillList.Add(drillStatItem);
            }
            else
            {
                drillStatItem.DrillCount++;
            }
        }

        public void ModifyDrillStatItem(int drillNum, double drillSize, string drillUnit)
        {
            DrillStatItem drillStatItem = DrillList.Find(x => x.DrillNum == drillNum);
            if (drillStatItem != null)
            {
                drillStatItem.DrillSize = drillSize;
                drillStatItem.DrillUnit = drillUnit;
            }
        }

        public void AddError(int layerIndex, string errorMsg, MessageType messageType = MessageType.Error)
        {
            ErrorList.Add(new ErrorItem() { LayerIndex = layerIndex, Message = errorMsg, Type = messageType });
        }
    }
}
