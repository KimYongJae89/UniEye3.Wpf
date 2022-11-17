namespace UniScanC.Models
{
    public class ScanWidth
    {
        public string Name { get; set; }
        public float Start { get; set; } = 0;
        public float End { get; set; } = 0;
        public float ValidStart { get; set; } = 0;
        public float ValidEnd { get; set; } = 0;

        public ScanWidth()
        {
            Name = "Width";
            Start = 0;
            End = 360;
            ValidStart = 30;
            ValidEnd = 330;
        }

        public ScanWidth(string name)
        {
            Name = name;
            Start = 0;
            End = 360;
            ValidStart = 30;
            ValidEnd = 330;
        }

        public ScanWidth(string name, float start, float end, float validStart, float validEnd)
        {
            Name = name;
            Start = start;
            End = end;
            ValidStart = validStart;
            ValidEnd = validEnd;
        }

        public ScanWidth Clone()
        {
            var scanWidth = new ScanWidth();

            scanWidth.CopyFrom(this);

            return scanWidth;
        }

        public void CopyFrom(ScanWidth dstScanWidth)
        {
            Name = dstScanWidth.Name;
            Start = dstScanWidth.Start;
            End = dstScanWidth.End;
            ValidStart = dstScanWidth.ValidStart;
            ValidEnd = dstScanWidth.ValidEnd;
        }

        public bool LoadDatabase()
        {
            //FirebirdDbManagerOld dbm = ModelManager.Instance().dbManager;

            //// Parameter
            //var dbDataList = dbm.Select("SCAN_WIDTH", null, string.Format("NAME='{0}'", Name));

            //bool result = dbDataList != null && dbDataList.Count == 1;
            //if (result)
            //{
            //    foreach (var dbData in dbDataList)
            //    {
            //        object data;

            //        if (dbData.TryGetValue("START_POS", out data))
            //            Start = Convert.ToSingle(data);

            //        if (dbData.TryGetValue("END_POS", out data))
            //            End = Convert.ToSingle(data);

            //        if (dbData.TryGetValue("VALID_START_POS", out data))
            //            ValidStart = Convert.ToSingle(data);

            //        if (dbData.TryGetValue("VALID_END_POS", out data))
            //            ValidEnd = Convert.ToSingle(data);
            //    }
            //}

            //return result;

            return true;
        }

        public bool SaveDatabase()
        {
            //FirebirdDbManagerOld dbm = ModelManager.Instance().dbManager;

            //return dbm.Commit(string.Format(
            //    "UPDATE SCAN_WIDTH SET " +
            //    "START_POS={0}, " +
            //    "END_POS={1}, " +
            //    "VALID_START_POS={2}, " +
            //    "VALID_END_POS={3} " +
            //    "WHERE NAME='{4}'",
            //    Start,
            //    End,
            //    ValidStart,
            //    ValidEnd,
            //    Name));

            return true;
        }
    }
}
