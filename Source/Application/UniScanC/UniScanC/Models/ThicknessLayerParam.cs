using DynMvp.Devices.Spectrometer.Data;
using UniScanC.Enums;

namespace UniScanC.Models
{
    public class ThicknessLayerParam : LayerParam
    {
        public ELayerParamType LayerType { get; set; }

        public ThicknessLayerParam(string paramName, ELayerParamType type) : base(paramName)
        {
            LayerType = type;
        }

        public static void CreateLayerDatabase(int name, /*LayerParamType*/int type)
        {
            //FirebirdDbManagerOld dbm = ModelManager.Instance().dbManager;

            //LayerParam param = new LayerParam();

            //string[] cols =
            //{
            //    "NAME",
            //    "PARAM_TYPE",
            //    "DATA_LENGTH_POW",
            //    "START_WAVE_LENGTH",
            //    "END_WAVE_LENGTH",
            //    "MIN_THICKNESS",
            //    "MAX_THICKNESS",
            //    "CALIB_PARAM_0",
            //    "CALIB_PARAM_1",
            //    "VALID_DATA_NUM",
            //    "REFLECTION",
            //    "THRESHOLD",
            //};

            //object[] values =
            //{
            //    name,
            //    type,
            //    param.DataLengthPow,
            //    param.StartWavelength,
            //    param.EndWavelength,
            //    param.MinThickness,
            //    param.MaxThickness,
            //    param.CalibParam0,
            //    param.CalibParam1,
            //    param.ValidDataNum,
            //    param.Refraction,
            //    param.Threshold
            //};

            //dbm.Insert("LAYER_PARAM", cols, values);
        }

        public bool LoadDatabase()
        {
            //FirebirdDbManagerOld dbm = ModelManager.Instance().dbManager;

            //// Parameter
            //var dbDataList = dbm.Select("LAYER_PARAM", null,
            //    string.Format("NAME='{0}' AND PARAM_TYPE={1}", ParamName, (int)LayerType));

            //bool result = dbDataList != null && dbDataList.Count == 1;
            //if (result)
            //{
            //    foreach (var dbData in dbDataList)
            //    {
            //        object data;

            //        if (dbData.TryGetValue("DATA_LENGTH_POW", out data))
            //            DataLengthPow = Convert.ToInt32(data);

            //        if (dbData.TryGetValue("START_WAVE_LENGTH", out data))
            //            StartWavelength = Convert.ToInt32(data);

            //        if (dbData.TryGetValue("END_WAVE_LENGTH", out data))
            //            EndWavelength = Convert.ToInt32(data);

            //        if (dbData.TryGetValue("MIN_THICKNESS", out data))
            //            MinThickness = Convert.ToInt32(data);

            //        if (dbData.TryGetValue("MAX_THICKNESS", out data))
            //            MaxThickness = Convert.ToInt32(data);

            //        if (dbData.TryGetValue("CALIB_PARAM_0", out data))
            //            CalibParam0 = Convert.ToSingle(data);

            //        if (dbData.TryGetValue("CALIB_PARAM_1", out data))
            //            CalibParam1 = Convert.ToSingle(data);

            //        if (dbData.TryGetValue("VALID_DATA_NUM", out data))
            //            ValidDataNum = Convert.ToInt32(data);

            //        if (dbData.TryGetValue("REFLECTION", out data))
            //            Refraction = Convert.ToSingle(data);

            //        if (dbData.TryGetValue("THRESHOLD", out data))
            //            Threshold = Convert.ToSingle(data);
            //    }
            //}

            //return result;

            return true;
        }

        public bool SaveDatabase()
        {
            //FirebirdDbManagerOld dbm = ModelManager.Instance().dbManager;

            //return dbm.Commit(string.Format(
            //    "UPDATE LAYER_PARAM SET " +
            //    "DATA_LENGTH_POW={0}, " +
            //    "START_WAVE_LENGTH={1}, " +
            //    "END_WAVE_LENGTH={2}, " +
            //    "MIN_THICKNESS={3}, " +
            //    "MAX_THICKNESS={4}, " +
            //    "CALIB_PARAM_0={5}, " +
            //    "CALIB_PARAM_1={6}, " +
            //    "REFLECTION={7}, " +
            //    "THRESHOLD={8} " +
            //    "WHERE NAME='{9}' AND PARAM_TYPE={10}",
            //    DataLengthPow,
            //    StartWavelength,
            //    EndWavelength,
            //    MinThickness,
            //    MaxThickness,
            //    CalibParam0,
            //    CalibParam1,
            //    Refraction,
            //    Threshold,
            //    ParamName,
            //    (int)LayerType));

            return true;
        }
    }
}
