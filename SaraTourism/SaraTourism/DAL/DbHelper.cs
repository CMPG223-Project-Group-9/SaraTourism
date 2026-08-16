using System;
using System.Data;
using MySql.Data.MySqlClient;
using SaraTourism.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    /// <summary>
    /// Thin ADO.NET wrapper around MySqlConnection. Every data access call in this
    /// project goes through a stored procedure - no inline SQL - per the project's
    /// SQL requirements. Connections are opened and disposed per call (short-lived),
    /// which is the standard pattern for a web app where requests are stateless.
    /// </summary>
    public static class DbHelper
    {
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SaraTourismDb"].ConnectionString; }
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>Runs a stored procedure that returns a result set, as a DataTable.</summary>
        public static DataTable ExecuteProcTable(string procName, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure })
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        /// <summary>Runs a stored procedure with no result set (insert/update/delete-style).</summary>
        public static int ExecuteProcNonQuery(string procName, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure })
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Runs a stored procedure that has one OUT parameter (e.g. the new ID from an insert)
        /// and returns that output parameter's value.
        /// </summary>
        public static object ExecuteProcWithOutput(string procName, string outputParamName, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure })
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                cmd.ExecuteNonQuery();
                return cmd.Parameters[outputParamName].Value;
            }
        }

        public static MySqlParameter Param(string name, object value)
        {
            return new MySqlParameter(name, value ?? DBNull.Value);
        }

        public static MySqlParameter OutParam(string name, MySqlDbType type)
        {
            return new MySqlParameter(name, type) { Direction = ParameterDirection.Output };
        }
    }
}
