using DynMvp.Data.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace UniScanC.Data
{
    public class GlossDataImporter
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

        public int ImportMaxRowIndex(string lotName)
        {
            var dataList = new List<Dictionary<string, object>>();
            int maxRowIndex = -1;

            string fromQuery = $"from (select row_number() over() as row from \"GM_Traverse\" where lot_name = '{lotName}') t";
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

        public List<Dictionary<string, object>> ImportUpdatedTraverseData(string lotName, int lastRowIndex, int updateRowIndex)
        {
            var dataList = new List<Dictionary<string, object>>();

            string fromQuery = $"from (select row_number() over() as row, * from \"GM_Traverse\" where lot_name = '{lotName}') t";
            string whereQuery = $"where row > '{lastRowIndex}' and row <= '{updateRowIndex}'";
            string fullQuery = $"select * {fromQuery} {whereQuery}";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }

        public List<Dictionary<string, object>> ImportThicknessData(string lotName, int frameIndex)
        {
            var dataList = new List<Dictionary<string, object>>();

            string whereQuery = $"where lot_name = '{lotName}' and roll_position = '{frameIndex}'";
            string fullQuery = $"select * from \"GM_Data\" {whereQuery} order by traverse_position";

            IDbManager dbManager = new PostgreDbManager();
            dbManager.Initialize(DbIpAddress, DbName, DbUserName, DbPassword);
            dbManager.CustomSelectData(out dataList, fullQuery);

            return dataList;
        }
    }
}
