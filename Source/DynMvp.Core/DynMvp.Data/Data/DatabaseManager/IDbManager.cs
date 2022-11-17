using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data.DatabaseManager
{
    public interface IDbManager
    {
        void Initialize(string serverName, string databaseName, string userName, string password);

        bool ConnectDatabase();
        void DisconnectDatabase();
        bool IsDatabaseExist();

        bool IsTableExist(string tableName);
        bool SelectTable(out List<Dictionary<string, object>> dataList, string whereQuery = "= 'public'");

        bool BeginTransaction();
        bool EndTransaction();

        bool InsertData(string tableName, List<string> columns, List<object> datas);
        bool SelectData(out List<Dictionary<string, object>> dataList, string tableName, string whereQuery = "");
        bool CustomSelectData(out List<Dictionary<string, object>> dataList, string fullQuery);
    }
}
