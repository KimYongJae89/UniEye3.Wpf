using DynMvp.Base;
using DynMvp.Data.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace UniScanC.Data
{
    public class InspectDataImporter
    {
        //DB Info
        public string DbIpAddress { get; set; } = "127.0.0.1";
        public string DbName { get; set; } = "UniScan";
        public string DbUserName { get; set; } = "postgres";
        public string DbPassword { get; set; } = "masterkey";

        public void SetDataBaseInfo(string dbIpAddress, string dbName, string dbUserName, string dbPassword)
        {
            DbIpAddress = dbIpAddress;
            DbName = dbName;
            DbUserName = dbUserName;
            DbPassword = dbPassword;
        }

        public List<Dictionary<string, object>> ImportLotData(DateTime startDate, DateTime endDate)
        {
            var dataList = new List<Dictionary<string, object>>();

            string startDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string endDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string whereQuery = $"where start_date >= '{startDate.ToString(startDateTimeFormat)}' and " +
                                $"start_date <= '{endDate.ToString(endDateTimeFormat)}'";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.SelectData(out dataList, "Lot", whereQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportSameLotNames(string lotName)
        {
            var dataList = new List<Dictionary<string, object>>();

            string whereQuery = $"where lot_name like '{lotName}@%'";
            string fullQuery = $"select lot_name from \"Lot\" {whereQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportFrameData(string lotName)
        {
            var dataList = new List<Dictionary<string, object>>();

            string whereQuery = $"where lot_name = '{lotName}'";
            string fullQuery = $"select * from \"Frame\" {whereQuery} order by frame_index";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportUpdatedFrameData(string lotName, int lastRowIndex, int updateRowIndex)
        {
            var dataList = new List<Dictionary<string, object>>();

            string fromQuery = $"from (select row_number() over() as row, * from \"Frame\" where lot_name = '{lotName}') t";
            string whereQuery = $"where row > '{lastRowIndex}' and row <= '{updateRowIndex}'";
            string fullQuery = $"select * {fromQuery} {whereQuery} order by frame_index";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportUpdatedModuleFrameData(string lotName, string moduleNo, int lastRowIndex, int updateRowIndex)
        {
            var dataList = new List<Dictionary<string, object>>();

            string fromQuery = $"from (select row_number() over() as row, * from \"Frame\" where lot_name = '{lotName}' and module_index = '{moduleNo}') t";
            string whereQuery = $"where row > '{lastRowIndex}' and row <= '{updateRowIndex}'";
            string fullQuery = $"select * {fromQuery} {whereQuery} order by frame_index";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportDefectData(string lotName, int frameIndex, int moduleIndex)
        {
            var dataList = new List<Dictionary<string, object>>();

            string whereQuery = $"where lot_name = '{lotName}' and frame_index = '{frameIndex}' and module_Index = '{moduleIndex}'";
            string fullQuery = $"select * from \"Defect\" {whereQuery} order by frame_index";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportLastFrame(string lotName, int count = 1)
        {
            var dataList = new List<Dictionary<string, object>>();
            var lastData = new Dictionary<string, object>();

            string whereQuery = $"where lot_name = '{lotName}'";
            string fullQuery = $"select * from \"Frame\" {whereQuery} order by frame_index desc limit {count}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public BitmapSource ImportDefectImage(string filePath)
        {
            try
            {
                if (filePath == string.Empty)
                {
                    return null;
                }

                var uri = new Uri(filePath);
                var bitmapSource = new BitmapImage(uri);
                bitmapSource.Freeze();
                return bitmapSource;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"InspectDataImporter::ImportDefectImage Fail to import image : {ex.Message}");
                return null;
            }
        }

        public BitmapSource ImportFrameImage(string filePath)
        {
            try
            {
                if (filePath == string.Empty)
                {
                    return null;
                }

                var uri = new Uri(filePath);
                var bitmapSource = new BitmapImage(uri);
                bitmapSource.Freeze();
                return bitmapSource;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"InspectDataImporter::ImportFrameImage Fail to import image : {ex.Message}");
                return null;
            }
        }

        public int ImportMaxRowIndex(string lotName)
        {
            var dataList = new List<Dictionary<string, object>>();
            int maxRowIndex = -1;

            string fromQuery = $"from (select row_number() over() as row from \"Frame\" where lot_name = '{lotName}') t";
            string fullQuery = $"select max(row) {fromQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            if (dataList.Count > 0)
            {
                dataList.First().TryGetValue("max", out object value);
                if (value != DBNull.Value)
                {
                    maxRowIndex = Convert.ToInt32(dataList.First()["max"]);
                }
            }

            return maxRowIndex;
        }

        public int ImportModuleMaxRowIndex(string lotName, string moduleNo)
        {
            var dataList = new List<Dictionary<string, object>>();
            int maxRowIndex = -1;

            string fromQuery = $"from (select row_number() over() as row from \"Frame\" where lot_name = '{lotName}' and module_index = '{moduleNo}') t";
            string fullQuery = $"select max(row) {fromQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            if (dataList.Count > 0)
            {
                dataList.First().TryGetValue("max", out object value);
                if (value != DBNull.Value)
                {
                    maxRowIndex = Convert.ToInt32(dataList.First()["max"]);
                }
            }

            return maxRowIndex;
        }

        public int ImportLotCount(DateTime startDate, DateTime endDate)
        {
            var dataList = new List<Dictionary<string, object>>();
            int lotCount = -1;

            string startDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string endDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string whereQuery = $"where start_date >= '{startDate.ToString(startDateTimeFormat)}' and " +
                                $"start_date <= '{endDate.ToString(endDateTimeFormat)}'";
            string fullQuery = $"select count(*) from \"Lot\" {whereQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            if (dataList.Count > 0)
            {
                lotCount = Convert.ToInt32(dataList.First()["count"]);
            }

            return lotCount;
        }

        public int ImportFrameCount(string lotName)
        {
            var dataList = new List<Dictionary<string, object>>();
            int defectCount = -1;

            string whereQuery = $"where lot_name = '{lotName}'";
            string fullQuery = $"select count(*) from \"Frame\" {whereQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            if (dataList.Count > 0)
            {
                defectCount = Convert.ToInt32(dataList.First()["count"]);
            }

            return defectCount;
        }

        public int ImportDefectCount(string lotName, string defectType = "")
        {
            var dataList = new List<Dictionary<string, object>>();
            int defectCount = 0;

            string whereQuery = $"where lot_name = '{lotName}'";
            if (defectType != "")
            {
                whereQuery += $" and defect_type = '{defectType}'";
            }

            string fullQuery = $"select count(*) from \"Defect\" {whereQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            if (dataList.Count > 0)
            {
                defectCount = Convert.ToInt32(dataList.First()["count"]);
            }

            return defectCount;
        }

        public bool ImportIsExistLotName(string lotName)
        {
            var dataList = new List<Dictionary<string, object>>();

            string whereQuery = $"where lot_name = '{lotName}'";
            string fullQuery = $"select lot_name from \"Lot\" {whereQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            if (dataList.Count > 0)
            {
                dataList.First().TryGetValue("lot_name", out object value);
                if (value != DBNull.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
