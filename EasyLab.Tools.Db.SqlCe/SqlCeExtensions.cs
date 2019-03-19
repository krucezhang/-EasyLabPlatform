/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            2/08/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;

namespace EasyLab.Tools.Db.SqlCe
{
    static class SqlCeExtensions
    {
        /// <summary>
        /// Check whether a table exists
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool TableExists(this SqlCeConnection connection, string tableName)
        {
            if (tableName == null)
            {
                throw new ArgumentNullException("tableName");
            }
            if (String.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Invalid table name");
            }
            if (connection == null)
            {
                throw new ArgumentException("connection");
            }
            if (connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException(String.Format("TableExists requires an open and available Connection. The connection's current state is {0}", connection.State));
            }
            using (SqlCeCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT 1 FROM Information_Schema.Tables WHERE TABLE_NAME = @tableName";
                command.Parameters.AddWithValue("tableName", tableName);
                object result = command.ExecuteScalar();

                return result != null;
            }
        }
        /// <summary>
        /// Check whether an index exists.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static bool IndexExists(this SqlCeConnection connection, string indexName)
        {
            using (SqlCeCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT 1 FROM INFORMATION_SCHEMA.INDEXES WHERE INDEX_NAME = @indexName";
                command.Parameters.AddWithValue("indexName", indexName);
                object result = command.ExecuteScalar();

                return result != null;
            }
        }
        /// <summary>
        /// Check whether an column exists
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ColumnExists(this SqlCeConnection connection, string tableName, string columnName)
        {
            using (SqlCeCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName";
                command.Parameters.AddWithValue("tableName", tableName);
                command.Parameters.AddWithValue("columnName", columnName);
                object result = command.ExecuteScalar();

                return result != null;
            }
        }
        /// <summary>
        /// Get DB table's column type
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string GetColumnDataType(this SqlCeConnection connection, string tableName, string columnName)
        {
            using (SqlCeCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName";
                command.Parameters.AddWithValue("tableName", tableName);
                command.Parameters.AddWithValue("columnName", columnName);
                object result = command.ExecuteScalar();

                return result as string;
            }
        }

        public static bool CanAlterColumn(this SqlCeConnection connection, string tableName, string columnName)
        {
            string dataType = GetColumnDataType(connection, tableName, columnName);

            if (string.Equals(dataType, "ntext", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Check whether a data row exists
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool DataExists(this SqlCeConnection connection, string tableName, string columns, string values)
        {
            using (SqlCeCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = ParseCheckDataSql(connection, tableName, columns, values);
                object result = command.ExecuteScalar();

                return result != null;
            }
        }

        private static List<string> GetPKColumns(SqlCeConnection connection, string tableName)
        {
            var results = new List<string>();
            var sb = new StringBuilder();

            sb.Append(" SELECT T2.COLUMN_NAME");
            sb.Append(" FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T1");
            sb.Append(" JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE T2 ");
            sb.AppendFormat(" ON T1.CONSTRAINI_NAME = T2.CONSTRAINI_NAME AND T1.TABLE_NAME = T2.TABLE_NAME AND T1.CONSTRAINI_TYPE = 'PRIMARY KEY' AND T1.TABLE_NAME = '{0}'", tableName);

            using (SqlCeCommand commmand = connection.CreateCommand())
            {
                commmand.CommandType = CommandType.Text;
                commmand.CommandText = sb.ToString();
                var reader = commmand.ExecuteReader();
                while (reader.Read())
                {
                    results.Add((string)reader["COLUMN_NAME"]);
                }
            }

            return results;
        }

        private static string ParseCheckDataSql(SqlCeConnection connection, string tableName, string columns, string values)
        {
            return String.Format("SELECT 1 FROM {0} WHERE {1}", tableName, GetWhereClause(connection, tableName, columns, values));
        }

        private static string GetWhereClause(SqlCeConnection connection, string tableName, string columns, string values)
        {
            var dataSet = ParseColumnAndValues(columns, values);
            if (dataSet.Count > 1)
            {
                var pks = GetPKColumns(connection, tableName);

                var allColumns = dataSet.Keys.ToArray();

                foreach (var column in allColumns)
                {
                    if (!pks.Contains(column))
                    {
                        dataSet.Remove(column);
                    }
                }
            }
            if (dataSet.Count == 0)
            {
                return "0 = 0";
            }

            return string.Join(" AND ", dataSet.Select(o => string.Format("{0} = {1} ", o.Key, o.Value)).ToArray());
        }

        private static Dictionary<string, string> ParseColumnAndValues(string columns, string values)
        {
            var results = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(columns))
            {
                columns = columns.Replace("[", string.Empty).Replace("]", string.Empty);
                var columnNames = columns.Split(',');
                var dataValues = (values ?? string.Empty).Split(',');

                for (int i = 0; i < columnNames.Length; i++)
                {
                    string value = dataValues.Length > i ? dataValues[i] : "''";
                    results.Add(columnNames[i], value);
                }
            }
            return results;
        }
    }
}
