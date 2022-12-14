using System;
using System.Collections.Generic;
using System.Linq;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Data
{
    public class ThicknessScanData : Observable
    {
        public DateTime StartTime { get; private set; }

        private int reelPosition;
        public int ReelPosition { get => reelPosition; set => Set(ref reelPosition, value); }

        private float startPosition;
        public float StartPosition { get => startPosition; set => Set(ref startPosition, value); }

        private float validStartPosition;
        public float ValidStartPosition { get => validStartPosition; set => Set(ref validStartPosition, value); }

        private float endPosition;
        public float EndPosition { get => endPosition; set => Set(ref endPosition, value); }

        private float validEndPosition;
        public float ValidEndPosition { get => validEndPosition; set => Set(ref validEndPosition, value); }

        private float offset;
        public float Offset { get => offset; set => Set(ref offset, value); }

        private List<ThicknessData> dataList = new List<ThicknessData>();
        public List<ThicknessData> DataList { get => dataList; set => Set(ref dataList, value); }

        private List<float> zoneData = new List<float>();
        public List<float> ZoneData { get => zoneData; set => Set(ref zoneData, value); }

        private float min;
        public float Min { get => min; set => Set(ref min, value); }

        private float max;
        public float Max { get => max; set => Set(ref max, value); }

        private float average;
        public float Average { get => average; set => Set(ref average, value); }

        private float leftAverage;
        public float LeftAverage { get => leftAverage; set => Set(ref leftAverage, value); }

        private float rightAverage;
        public float RightAverage { get => rightAverage; set => Set(ref rightAverage, value); }

        public float Dev => max - min;

        public int ValidPointCount => dataList.Count;

        public ThicknessScanData()
        {
            StartTime = DateTime.Now;
            ReelPosition = 0;
            Offset = 0;

            Min = float.MaxValue;
            Max = float.MinValue;
            Average = float.MaxValue;
            LeftAverage = float.MaxValue;
            RightAverage = float.MaxValue;
        }

        public void Clear()
        {
            StartTime = DateTime.Now;
            ReelPosition = 0;
            Offset = 0;

            dataList.Clear();
            zoneData.Clear();

            Min = float.MaxValue;
            Max = float.MinValue;
            Average = float.MaxValue;
        }

        public ThicknessScanData Clone()
        {
            var scanData = new ThicknessScanData();

            scanData.StartTime = StartTime;
            scanData.ReelPosition = ReelPosition;
            scanData.Offset = Offset;

            scanData.DataList = Clone(DataList);
            scanData.ZoneData = Clone(ZoneData);

            scanData.Min = Min;
            scanData.Max = Max;
            scanData.Average = Average;
            scanData.LeftAverage = LeftAverage;
            scanData.RightAverage = RightAverage;

            return scanData;
        }

        public List<ThicknessData> Clone(List<ThicknessData> sourceList)
        {
            var targetList = new List<ThicknessData>();

            for (int i = 0; i < sourceList.Count(); i++)
            {
                targetList.Add(new ThicknessData(sourceList[i].Time, sourceList[i].Position, sourceList[i].Thickness, sourceList[i].Refraction, sourceList[i].Angle));
            }

            return targetList;
        }

        public List<float> Clone(List<float> sourceList)
        {
            var targetList = new List<float>();

            for (int i = 0; i < sourceList.Count(); i++)
            {
                targetList.Add(sourceList[i]);
            }

            return targetList;
        }

        public void AddValue(DateTime time, float position, float thickness, float refraction = 1, float angle = 1)
        {
            dataList.Add(new ThicknessData(time, position, thickness, refraction, angle));
        }

        public void AddZoneValue(float zoneThickness)
        {
            if (zoneThickness != 0)
            {
                zoneData.Add(zoneThickness);
            }
        }
    }
}
