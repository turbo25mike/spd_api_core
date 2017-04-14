using System.Collections.Generic;
using System.Linq;
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
                return db.Query<T>(sql, parameters).Single();
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_settings.DB_Connection))
            {
                db.Open();
                return db.Query<T>(sql, parameters);
            }
        }

        public int Execute(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_settings.DB_Connection))
            {
                db.Open();
                return db.Execute(sql, parameters);
            }
        }
    }
}
