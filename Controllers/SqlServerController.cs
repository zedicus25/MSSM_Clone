using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MSSM_Clone.Controllers
{
    public class SqlServerController
    {
        private string _connectionPath;
        public static event Action<string> SendMessage;
        public static event Action<bool> ConnectionResult;

        public SqlServerController(string serverName, string dataBaseName)
        {
           CreateConnectionPath(serverName, dataBaseName);
        }

        public Dictionary<string, List<object>> GetDataFromTable(string tableName)
        {
            Dictionary<string, List<object>> data = new Dictionary<string, List<object>>();
            List<string> names = GetFieldsName(tableName);
            List<List<object>> rowValues = GetFieldsData(tableName);
            for (int i = 0; i < names.Count; i++)
            {
                List<object> collumValues = new List<object>();
                for (int j = 0; j < rowValues.Count; j++)
                {
                    collumValues.Add(rowValues[j][i]);
                }
                data.Add(names[i], collumValues);
            }
            return data;
        }

        private List<List<object>> GetFieldsData(string tableName)
        {
            try
            {
                SqlConnection connection = GetSqlConnection();
                connection.Open();
                string command = $"SELECT * FROM [{tableName}]";
                SqlCommand sqlCommand = GetSqlCommand(connection, command);
                List<List<object>> fieldsData = new List<List<object>>();
                SqlDataReader sqlData = sqlCommand.ExecuteReader();
                while (sqlData.Read())
                {
                    List<object> fields = new List<object>();
                    for (int i = 0; i < sqlData.FieldCount; i++)
                    {
                        fields.Add(sqlData.GetValue(i));
                    }
                    fieldsData.Add(fields);
                }
                return fieldsData;
            }
            catch (Exception ex)
            {
                SendMessage?.Invoke(ex.Message);
            }
            return new List<List<object>>();
        }

        private List<string> GetFieldsName(string tableName)
        {
            try
            {
                using (SqlConnection connection = GetSqlConnection())
                {
                    connection.Open();
                    string command = $"SELECT * FROM [{tableName}]";
                    using (SqlCommand sqlCommand = GetSqlCommand(connection, command))
                    {
                        List<string> names = new List<string>();
                        SqlDataReader sqlData = sqlCommand.ExecuteReader();
                        for (int i = 0; i < sqlData.FieldCount; i++)
                        {
                            names.Add(sqlData.GetName(i));
                        }
                        return names;
                    }
                }
                    
              
            }
            catch (Exception ex) 
            {
                SendMessage?.Invoke(ex.Message);
            }
            return new List<string>();
        }
       

        private void CreateConnectionPath(string serverName, string dataBaseName)
        {
            _connectionPath = TryCreateConnectionPath(serverName, dataBaseName);
            try
            {
                using(SqlConnection connection = GetSqlConnection())
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    ConnectionResult?.Invoke(true);
                    SendMessage?.Invoke("Succesful connection!");

                }
            }
            catch (Exception ex)
            {
                SendMessage?.Invoke(ex.Message);
                ConnectionResult?.Invoke(false);
            }
        }

        private string TryCreateConnectionPath(string serverName, string dataBaseName)
        {
            if (serverName.Equals(String.Empty))
                throw new ArgumentException("Server name cannot be empty");
            if (dataBaseName.Equals(String.Empty))
                throw new ArgumentException("Data base name cannot be empty");
            return $@"Server={serverName.Trim()};Database={dataBaseName.Trim()};Trusted_Connection=True;";
        }

        private SqlConnection GetSqlConnection() => new SqlConnection(_connectionPath);
        private SqlCommand GetSqlCommand(SqlConnection connection, string command) => new SqlCommand(command, connection);
    }
}
