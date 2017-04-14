using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Dapper;

namespace Api.DataStore
{
    public class MySqlDatabase : IDatabase
    {
        private readonly IAppSettings _settings;

        public MySqlDatabase(IAppSettings settings)
        {
            _settings = settings;
        }

        public T QuerySingle<T>(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_settings.DB_Connection))
            {
                db.Open();
                return db.Query<T>(Clean(sql), parameters).Single();
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_settings.DB_Connection))
            {
                db.Open();
                return db.Query<T>(Clean(sql), parameters);
            }
        }

        public int Execute(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_settings.DB_Connection))
            {
                db.Open();
                return db.Execute(Clean(sql), parameters);
            }
        }

        private string Clean(string script)
        {
            var clean = Regex.Replace(script, @"\r\n?|\n", " ");
            return clean;
        }
    }
}
