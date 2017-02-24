using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Api.DataStore
{
    public class MySqlDatabase : IDatabase
    {
        private readonly IAppSettings _settings;

        public MySqlDatabase(IAppSettings settings, IHttpContextAccessor httpContextAccessor)
        {
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<T> Select<T>(string[] columnList = null, DBWhere where = null, int? limitAmount = null)
        {
            var request = Activator.CreateInstance<T>();
            var table = GetTableName(typeof(T).Name);
            if (columnList == null)
                columnList = (from prop in request.GetType().GetProperties()
                              where prop.CanWrite && prop.Name != "RemovedBy" && prop.Name != "RemovedDate"
                              select prop.Name).ToArray();
            var columns = string.Join(",", columnList);
            var whereColumns = where == null ? "" : $"WHERE {where.Flatten()}";
            var limit = limitAmount.HasValue ? $"LIMIT {limitAmount}" : "";
            return Query<T>($"SELECT {columns} FROM {table} {whereColumns} {limit}");
        }

        public IModel Insert(IModel request)
        {
            var currentDate = DateTime.Now;
            var table = GetTableName(request.GetType().Name);

            var setColumns = request.CreateSet();
            setColumns.Add("CreatedBy", GetCurrentMemberID());
            setColumns.Add("CreatedDate", currentDate);
            setColumns.Add("UpdatedBy", GetCurrentMemberID());
            setColumns.Add("UpdatedDate", currentDate);


            var id = Execute($"INSERT INTO {table} ({setColumns.FlattenKeys()}) VALUES ({setColumns.FlattenValues()})");
            request.SetValue(request.PrimaryKey, id);
            request.CreatedBy = GetCurrentMemberID();
            request.CreatedDate = currentDate;
            request.UpdatedBy = GetCurrentMemberID();
            request.UpdatedDate = currentDate;
            return request;
        }

        public IModel Update(IModel request, string[] set = null, DBWhere where = null)
        {
            var currentDate = DateTime.Now;
            var setColumns = request.CreateSet(set);
            setColumns.Add("UpdatedBy", GetCurrentMemberID());
            setColumns.Add("UpdatedDate", currentDate);

            if (where == null)
                where = new DBWhere { new DBWhereColumn(request.PrimaryKey, request.GetValue(request.PrimaryKey)) };

            Execute($"UPDATE {GetTableName(request.GetType().Name)} SET {setColumns.Flatten()} WHERE {where.Flatten()}");

            request.UpdatedBy = GetCurrentMemberID();
            request.UpdatedDate = currentDate;
            return request;
        }

        public void Delete<T>(int id)
        {
            var currentDate = DateTime.Now;
            var setColumns = new Dictionary<string, object>
            {
                ["RemovedBy"] = GetCurrentMemberID(),
                ["RemovedDate"] = currentDate
            };
            var request = (IModel)Activator.CreateInstance<T>();

            var where = new DBWhere
            {
                new DBWhereColumn(request.PrimaryKey, request.GetValue(request.PrimaryKey))
            };

            Execute($"UPDATE {GetTableName(request.GetType().Name)} SET {setColumns.Flatten()} WHERE {where.Flatten()}");
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

        private string GetTableName(string className)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(className, "_").ToLower();
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
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.CurrentCultureIgnoreCase))
                    return dr[i];
            }
            return DBNull.Value;
        }

        private int? _currentMemberID;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int GetCurrentMemberID()
        {
            if (_currentMemberID.HasValue) return _currentMemberID.Value;
            var identity = _httpContextAccessor.HttpContext.User.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identity)) throw new ArgumentNullException(nameof(ClaimsPrincipal));
            var result = Select<Member>(new[] { nameof(Member.MemberID) }, new DBWhere { new DBWhereColumn(nameof(Member.LoginID), identity) }, 1).FirstOrDefault();
            if (result == null) throw new ArgumentNullException(nameof(ClaimsPrincipal));
            _currentMemberID = result.MemberID;
            return _currentMemberID.Value;
        }
    }
}
