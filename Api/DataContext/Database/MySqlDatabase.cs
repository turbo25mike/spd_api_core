using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Reflection;
using Api.DataContext.Models;

namespace Api.DataContext.Database
{
    public class MySqlDatabase : IDatabase
    {
        private readonly IAppSettings _settings;

        public MySqlDatabase(IAppSettings settings)
        {
            _settings = settings;
        }

        public List<T> Select<T, TCol>(string tableName, TCol[] columnList = null, DBWhere<TCol> whereColumns = null, int? limitAmount = null)
        {
            var columns = columnList == null ? "*" : string.Join(",", columnList);
            var where = whereColumns == null ? "" : $"WHERE {whereColumns.Flatten()}";
            var limit = limitAmount.HasValue ? $"LIMIT {limitAmount.ToString()}" : "";
            return Query<T>($"SELECT {columns} FROM {tableName} {where} {limit}");
        }

        public long Insert<T, TCol>(string tableName, Dictionary<TCol, object> setColumns)
        {
            return Execute($"INSERT INTO {tableName} ({setColumns.FlattenKeys()}) VALUES ({setColumns.FlattenValues()})");
        }

        public void Update<T, TCol>(string tableName, Dictionary<TCol, object> setColumns, DBWhere<TCol> whereColumns)
        {
            Execute($"UPDATE {tableName} SET {setColumns.Flatten()} WHERE {whereColumns.Flatten()}");
        }

        //public void Delete<T, TCol>(string tableName, DBWhere<TCol> whereColumns)
        //{
        //    Execute(database, $"DELETE FROM {tableName} WHERE {whereColumns.Flatten()}");
        //}

        private long Execute(string command)
        {
            var result = 0L;
            using (var conn = new MySqlConnection(_settings.DB_Connection))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                    result = cmd.LastInsertedId;
                }
            }
            return result;
        }

        private List<T> Query<T>(string command)
        {
            var list = new List<T>();
            using (var conn = new MySqlConnection(_settings.DB_Connection))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = Activator.CreateInstance<T>();
                            foreach (var prop in obj.GetType().GetProperties())
                            {
                                if (prop.CanWrite && !Equals(GetColumn(reader, prop.Name), DBNull.Value))
                                {
                                    prop.SetValue(obj, reader[prop.Name], null);
                                }
                            }
                            list.Add(obj);
                        }
                    }
                }
            }
            return list;
        }

        private object GetColumn(MySqlDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    return dr[i];
            }
            return DBNull.Value;
        }
    }
}
