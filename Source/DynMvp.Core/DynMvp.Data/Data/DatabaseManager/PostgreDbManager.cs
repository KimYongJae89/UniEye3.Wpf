using DynMvp.Base;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data.DatabaseManager
{
    // Postgre Database Manager
    public class PostgreDbManager : IDbManager
    {
        private string serverName;
        private string dbName;
        private string userName;
        private string password;

        private NpgsqlConnection conDatabase;
        private NpgsqlTransaction transaction;

        public void Dispose()
        {
            DisconnectDatabase();
        }

        public PostgreDbManager()
        {

        }

        public void Initialize(string serverName, string dbName, string userName, string password)
        {
            this.serverName = serverName;
            this.dbName = dbName;
            this.userName = userName;
            this.password = password;
        }

        // Database Handler
        public bool ConnectDatabase()
        {
            string connectionString = $"host={serverName};username={userName};password={password};database={dbName}";

            try
            {
                //throw new NpgsqlException();
                conDatabase = new NpgsqlConnection(connectionString);
                conDatabase.Open();
                return true;
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't connect to Postgre DB : " + ex.Message);
            }
            catch (NpgsqlException ex)
            {
                LogHelper.Error(LoggerType.Inspection, "Can't connect to Postgre DB : " + ex.Message);
            }
            conDatabase?.Dispose();
            conDatabase = null;
            return false;
        }

        public void DisconnectDatabase()
        {
            if (conDatabase == null)
            {
                return;
            }

            EndTransaction();

            conDatabase.Close();
            conDatabase.Dispose();
            conDatabase = null;
        }

        public bool IsDatabaseExist()
        {
            bool isExist = true;
            string query = string.Format("select 'true' where exists (select from pg_database where datname = '{0}')", dbName);

            string connectionString = $"host={serverName};username={userName};password={password}";

            var conServer = new NpgsqlConnection(connectionString);
            conServer.Open();

            var command = new NpgsqlCommand(query, conServer);
            if (command != null)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }

            conServer.Close();

            return isExist;
        }

        // Table Handler
        public bool IsTableExist(string tableName)
        {
            bool isExist = true;
            string queury = string.Format("select 'true' where exists (select from information_schema.tables where table_name = '{0}')", tableName);

            if (ConnectDatabase())
            {
                var command = new NpgsqlCommand(queury, conDatabase);
                if (command != null)
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        isExist = true;
                    }
                    else
                    {
                        isExist = false;
                    }

                    DisconnectDatabase();
                    return isExist;
                }
                else
                {
                    DisconnectDatabase();
                    return isExist;
                }
            }
            return isExist;
        }

        public bool SelectTable(out List<Dictionary<string, object>> dataList, string whereQuery = "= 'public'")
        {
            dataList = new List<Dictionary<string, object>>();

            string baseQuery = "select table_name from information_schema.tables where table_schema ";
            string query = "";
            query = baseQuery + whereQuery + " order by table_name;";

            if (ConnectDatabase())
            {
                var command = new NpgsqlCommand(query, conDatabase);
                if (command != null)
                {
                    try
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var dbData = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                dbData.Add(reader.GetName(i), reader.GetValue(i));
                            }

                            dataList.Add(dbData);
                        }
                    }
                    catch (NpgsqlException)
                    {
                        DisconnectDatabase();
                        dataList = null;
                        return false;
                    }
                    DisconnectDatabase();
                    return true;
                }
                else
                {
                    DisconnectDatabase();
                    dataList = null;
                    return false;
                }
            }
            dataList = null;
            return false;
        }

        // Data Handler
        public bool BeginTransaction()
        {
            if (conDatabase == null)
            {
                if (ConnectDatabase() == false)
                {
                    return false;
                }
            }

            if (transaction != null)
            {
                EndTransaction();
            }

            transaction = conDatabase.BeginTransaction();

            return true;
        }

        public bool EndTransaction()
        {
            if (conDatabase == null)
            {
                return false;
            }

            if (transaction == null)
            {
                return false;
            }

            try
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Comm, ex.Message);
                return false;
            }
            return true;
        }

        public bool InsertData(string tableName, List<string> columns, List<object> datas)
        {
            if (conDatabase == null)
            {
                return false;
            }

            string query = string.Format("insert into public.\"{0}\"", tableName);
            string columnString = "";
            for (int i = 0; i < columns.Count; i++)
            {
                columnString += "\"" + columns[i] + "\"";
                if (i < columns.Count - 1)
                {
                    columnString += ", ";
                }
            }
            string dataString = "";
            for (int i = 0; i < datas.Count; i++)
            {
                dataString += "'" + datas[i].ToString() + "'";
                if (i < datas.Count - 1)
                {
                    dataString += ", ";
                }
            }
            query += "(" + columnString + ") values (" + dataString + ");";

            var command = new NpgsqlCommand(query, conDatabase);
            if (command != null)
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (NpgsqlException ne)
                {
                    LogHelper.Debug(LoggerType.Comm, $"PostgreDbManager::InsertData - Error {ne.StackTrace}");
                    return false;
                }
                catch (InvalidOperationException ioe)
                {
                    LogHelper.Debug(LoggerType.Comm, $"PostgreDbManager::InsertData - Error {ioe.StackTrace}");
                    return false;
                }
                catch (Exception e)
                {
                    LogHelper.Debug(LoggerType.Comm, $"PostgreDbManager::InsertData - Error {e.StackTrace}");
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SelectData(out List<Dictionary<string, object>> dataList, string tableName, string whereQuery = "")
        {
            dataList = new List<Dictionary<string, object>>();

            if (ConnectDatabase() == false)
            {
                return false;
            }

            string query = $"select * from \"{tableName}\" {whereQuery}";

            bool result = false;

            var command = new NpgsqlCommand(query, conDatabase);
            if (command != null)
            {
                try
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var dbData = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dbData.Add(reader.GetName(i), reader.GetValue(i));
                        }

                        dataList.Add(dbData);
                    }

                    result = true;
                }
                catch (NpgsqlException)
                {
                }
            }

            DisconnectDatabase();

            return result;
        }

        public bool CustomSelectData(out List<Dictionary<string, object>> dataList, string fullQuery = "")
        {
            dataList = new List<Dictionary<string, object>>();

            if (ConnectDatabase() == false)
            {
                return false;
            }

            string query = fullQuery;

            bool result = false;

            var command = new NpgsqlCommand(query, conDatabase);
            if (command != null)
            {
                try
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var dbData = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            dbData.Add(reader.GetName(i), reader.GetValue(i));
                        }

                        dataList.Add(dbData);
                    }

                    result = true;
                }
                catch (NpgsqlException)
                {
                }
            }

            DisconnectDatabase();

            return result;
        }
    }
}
