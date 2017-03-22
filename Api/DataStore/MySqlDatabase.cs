using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Reflection;
using Api.Extensions;

namespace Api.DataStore
{
    public class MySqlDatabase : IDatabase
    {
        private readonly IAppSettings _settings;

        public MySqlDatabase(IAppSettings settings)
        {
            _settings = settings;
        }

        public List<T> Select<T>(TableColumns columnList = null, Joins joins = null, Where where = null, int? limitAmount = null)
        {
            var table = typeof(T).Name.SplitNameOnUppercase();
            var columns = "";
            if (columnList == null)
            {
                var columnNames = from prop in typeof(T).GetProperties()
                                  where prop.CanWrite && prop.Name != "RemovedBy" && prop.Name != "RemovedDate"
                                  select prop.Name;

                columnList = new TableColumns();
                columnList.AddRange(columnNames.Select(c => new TableColumn<T>(c)));
            }

            columns = columnList.Flatten();
            var join = joins == null ? "" : joins.Flatten();

            var whereColumns = where == null ? "" : $"WHERE {where.Flatten()}";
            var limit = limitAmount.HasValue ? $"LIMIT {limitAmount}" : "";
            return Query<T>($"SELECT {columns} FROM {table} {join} {whereColumns} {limit}");
        }

        public IModel Insert(IModel request, int memberID)
        {
            var currentDate = DateTime.Now;
            var table = request.SplitNameOnUppercase();

            var setColumns = request.CreateSet();
            setColumns.Add("CreatedBy", memberID);
            setColumns.Add("CreatedDate", currentDate);
            setColumns.Add("UpdatedBy", memberID);
            setColumns.Add("UpdatedDate", currentDate);

            var id = Execute($"INSERT INTO {table} ({setColumns.FlattenKeys()}) VALUES ({setColumns.FlattenValues()})");
            request.SetValue(request.PrimaryKey, id);
            request.CreatedBy = memberID;
            request.CreatedDate = currentDate;
            request.UpdatedBy = memberID;
            request.UpdatedDate = currentDate;
            return request;
        }

        public IModel Update(IModel request, int memberID, string[] set = null, Where where = null)
        {
            var currentDate = DateTime.Now;
            var setColumns = request.CreateSet(set);
            setColumns.Add("UpdatedBy", memberID);
            setColumns.Add("UpdatedDate", currentDate);

            if (where == null)
            {
                where = new Where
                    {
                        new WhereColumn<Member>(request.PrimaryKey, memberID)
                    };
            }

            Execute($"UPDATE {request.SplitNameOnUppercase()} SET {setColumns.Flatten()} WHERE {where.Flatten()}");

            request.UpdatedBy = memberID;
            request.UpdatedDate = currentDate;
            return request;
        }

        public void Delete<T>(int id, int memberID)
        {
            var currentDate = DateTime.Now;
            var setColumns = new Dictionary<string, object>
            {
                ["RemovedBy"] = memberID,
                ["RemovedDate"] = currentDate
            };
            var request = (IModel)Activator.CreateInstance<T>();

            var where = new Where
            {
                new WhereColumn<T>(request.PrimaryKey, request.GetValue(request.PrimaryKey))
            };

            Execute($"UPDATE {request.SplitNameOnUppercase()} SET {setColumns.Flatten()} WHERE {where.Flatten()}");
        }

        private int Execute(string command)
        {
            var result = 0;
            using (var conn = new MySqlConnection(_settings.DB_Connection))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                    result = (int)cmd.LastInsertedId;
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
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    using (var reader = cmd.ExecuteReader())
                    {
                        var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                        while (reader.Read())
                        {
                            var obj = Activator.CreateInstance<T>();

                            foreach (var prop in columns)
                            {
                                var model = obj as Model;
                                model?.SetProp(reader[prop]);
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
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    return dr[i];
            }
            return DBNull.Value;
        }
    }
}
