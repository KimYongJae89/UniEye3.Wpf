using System;
using System.Collections.Generic;
using System.Linq;
using Unieye.WPF.Base.Helpers;

namespace UniScanC.Data
{
    public class GlossScanData : Observable
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

        //private float offset;
        //public float Offset { get => offset; set => Set(ref offset, value); }

        private List<GlossData> dataList = new List<GlossData>();
        public List<GlossData> DataList { get => dataList; set => Set(ref dataList, value); }

        //private List<float> zoneData = new List<float>();
        //public List<float> ZoneData { get => zoneData; set => Set(ref zoneData, value); }

        private float min;
        public float Min { get => min; set => Set(ref min, value); }

        private float max;
        public float Max { get => max; set => Set(ref max, value); }

        private float average;
        public float Average { get => average; set => Set(ref average, value); }

        //private float leftAverage;
        //public float LeftAverage { get => leftAverage; set => Set(ref leftAverage, value); }

        //private float rightAverage;
        //public float RightAverage { get => rightAverage; set => Set(ref rightAverage, value); }

        //public float Dev => max - min;

        private float dev;
        public float Dev { get => dev; set => Set(ref dev, value); }

        public int ValidPointCount => dataList.Count;

        public GlossScanData()
        {
            StartTime = DateTime.Now;
            ReelPosition = 0;
            //Offset = 0;

            Min = float.MaxValue;
            Max = float.MinValue;
            Average = float.MaxValue;
            //LeftAverage = float.MaxValue;
            //RightAverage = float.MaxValue;
        }

        public void Clear()
        {
            StartTime = DateTime.Now;
            ReelPosition = 0;
            //Offset = 0;

            dataList.Clear();
            //zoneData.Clear();

            Min = float.MaxValue;
            Max = float.MinValue;
            Average = float.MaxValue;
        }

        public GlossScanData Clone()
        {
            var scanData = new GlossScanData();

            scanData.StartTime = StartTime;
            scanData.ReelPosition = ReelPosition;
            //scanData.Offset = Offset;

            scanData.DataList = Clone(DataList);
            //scanData.ZoneData = Clone(ZoneData);

            scanData.Min = Min;
            scanData.Max = Max;
            scanData.Average = Average;
            //scanData.LeftAverage = LeftAverage;
            //scanData.RightAverage = RightAverage;

            return scanData;
        }

        public List<GlossData> Clone(List<GlossData> sourceList)
        {
            var targetList = new List<GlossData>();

            for (int i = 0; i < sourceList.Count(); i++)
            {
                targetList.Add(new GlossData(sourceList[i].Time, sourceList[i].Position, sourceList[i].Data));
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

        public void AddValue(DateTime time, float position, float data)
        {
            dataList.Add(new GlossData(time, position, data));
        }
    }
}
