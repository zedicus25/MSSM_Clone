using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Controls;

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
            GetAllTablesNames();
        }


        public List<string> GetAllTablesNames()
        {
            using (SqlConnection con = new SqlConnection(_connectionPath))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", con))
                {
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        List<string> names = new List<string>();
                        while (reader.Read())
                        {
                            names.Add((string)reader["TABLE_NAME"]);
                        }
                        return names;
                    }
                }
            }
        }


        public void InsertDataToDataBase(string tableName, Dictionary<string, string> data)
        {

            using (SqlConnection connection = new SqlConnection(_connectionPath))
            {
                connection.Open();
                string command = $"SELECT * FROM [{tableName}]";
                using (SqlCommand sqlCommand = GetSqlCommand(connection, command))
                {
                    SqlDataReader sqlData = sqlCommand.ExecuteReader();
                    
                    DataTable dataTable = sqlData.GetSchemaTable();
                    List<string> keysForRemove = new List<string>();
                    int i = 0;
                    while (sqlData.Read())
                    {
                        keysForRemove.Add(dataTable.Columns[i].ColumnName);
                            i++;
                    }
                    
                    /*foreach (DataColumn column in dataTable.Columns)
                    {
                        if(column.ColumnName.Equals("shipperid"))
                            keysForRemove.Add("1");
                    }*/
                    /*foreach (var item in data)
                    {
                        if (dataTable.Columns[0].AutoIncrement)

                    }*/
                }
            }


            /*StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO [dbo].[{tableName}](");
            foreach (var item in data)
            {
                if (!item.Key.Contains("id") && !item.Key.Contains("Id"))
                    sb.Append($"[{item.Key}],");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES(");
            foreach (var item in data)
            {
                if (!item.Key.Contains("id") && !item.Key.Contains("Id"))
                    sb.Append($"'{item.Value}',");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(");");
            using (SqlConnection con = new SqlConnection(_connectionPath))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sb.ToString(), con))
                {
                    if (command.ExecuteNonQuery() > 0)
                        SendMessage?.Invoke("Added!");
                    else
                        SendMessage?.Invoke("Not Added!");
                }
            }*/

        }

        public List<List<object>> GetFieldsData(string tableName)
        {
            try
            {
                using (SqlConnection connection = GetSqlConnection())
                {
                    connection.Open();
                    string command = $"SELECT * FROM [{tableName}]";
                    using (SqlCommand sqlCommand = GetSqlCommand(connection, command))
                    {
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
                }
            }
            catch (Exception ex)
            {
                SendMessage?.Invoke(ex.Message);
            }
            return new List<List<object>>();
        }

        public List<string> GetFieldsName(string tableName)
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
                using (SqlConnection connection = GetSqlConnection())
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
