using System.Data;
using Npgsql;

using JabilDevPortal.Api.Data.Models.Config;

namespace BackEnd.Infrastructure.Plugins
{
    public class PGSQLConnectionPlugin 
    {
        private readonly string _connectionString = String.Empty;
        private int _timeOut = 120;

        public PGSQLConnectionPlugin(SQLDataBaseSettings settings)
        {
            _connectionString = BuildConnectionString(settings);
        }

        private string BuildConnectionString(SQLDataBaseSettings settings)
        {
            // Ejemplo: Host=myserver;Port=5432;Database=mydb;Username=myuser;Password=mypass
            return $"Host={settings.Server};Port=5432;Database={settings.Database};Username={settings.User};Password={settings.Password};Timeout=120;CommandTimeout=120;Trust Server Certificate=true";
        }

        private string fnStoredProcedureCommandBuilder(out string ExceptionMessage, string procName, params object[] args)
        {
            try
            {
                ExceptionMessage = string.Empty;
                string cmdText = $"CALL {procName}(";
                if (args != null && args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == null)
                        {
                            cmdText += "NULL";
                        }
                        else
                        {
                            var type = args[i].GetType();
                            if (type == typeof(string) || type == typeof(char))
                                cmdText += $"'{args[i].ToString().Replace("'", "''")}'";
                            else if (type == typeof(DateTime))
                                cmdText += $"'{((DateTime)args[i]).ToString("yyyy-MM-dd HH:mm:ss")}'";
                            else
                                cmdText += args[i].ToString();
                        }
                        if (i < args.Length - 1)
                            cmdText += ",";
                    }
                }
                cmdText += ")";
                return cmdText;
            }
            catch (Exception ex)
            {
                ExceptionMessage = "PGSQL.fnStoredProcedureCommandBuilder.Exception: " + ex.Message;
                return string.Empty;
            }
        }

        public DataTable GetDataTable(string query)
        {
            var dt = new DataTable();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.CommandTimeout = _timeOut;
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetDataTable: " + ex.Message, ex);
            }
            return dt;
        }

        public DataTable ExecDataTable(string query)
        {
            // Alias de GetDataTable
            return GetDataTable(query);
        }

        public DataTable ExecDataTable(string query, params object[] args)
        {
            var dt = new DataTable();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    query = fnStoredProcedureCommandBuilder(out string pError, query, args);
                    if (!string.IsNullOrEmpty(pError))
                        throw new Exception(pError);

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.CommandTimeout = _timeOut;
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ExecDataTable with parameters: " + ex.Message, ex);
            }
            return dt;
        }

        public void ExecNonQuery(string query, params object[] args)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    query = fnStoredProcedureCommandBuilder(out string pError, query, args);
                    if (!string.IsNullOrEmpty(pError))
                        throw new Exception(pError);

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.CommandTimeout = _timeOut;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ExecNonQuery: " + ex.Message, ex);
            }
        }

        public void BulkInsert(DataTable dt, string tableName)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var writer = connection.BeginTextImport($"COPY \"{tableName}\" FROM STDIN (FORMAT csv)"))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            // Convierte la fila en CSV
                            var values = row.ItemArray.Select(v => v?.ToString()?.Replace("\"", "\"\"") ?? "").ToArray();
                            string csvLine = string.Join(",", values);
                            writer.WriteLine(csvLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in BulkInsert: " + ex.Message, ex);
            }
        }
    }
}
