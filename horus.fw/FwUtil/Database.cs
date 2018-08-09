using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.FwUtil
{
    public class Database
    {
        public static int ExecuteCommand(string command, SqlParameter[] parameters = null, string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Config.SqlConnection;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        if (parameters.Any())
                        {
                            sqlCommand.Parameters.AddRange(parameters);
                        }
                    }

                    sqlCommand.Connection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public static int ExecuteProcedure(string procedureName, SqlParameter[] parameters = null, string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Config.SqlConnection;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(procedureName, connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        if (parameters.Any())
                        {
                            sqlCommand.Parameters.AddRange(parameters);
                        }
                    }

                    sqlCommand.Connection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public static DataTable ExecuteQuery(string queryString, string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Config.SqlConnection;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }
    }
}
