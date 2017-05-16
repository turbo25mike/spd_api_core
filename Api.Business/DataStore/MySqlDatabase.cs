using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dapper;
using Models;
using MySql.Data.MySqlClient;

namespace Business.DataStore
{
    public class MySqlDatabase : IDatabase
    {
        private readonly string _dbConnection;

        public MySqlDatabase(IAppSettings settings)
        {
            _dbConnection = settings.DB_Connection;
        }

        public T QuerySingle<T>(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_dbConnection))
            {
                db.Open();
                return db.Query<T>(Clean(sql), parameters).FirstOrDefault();
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_dbConnection))
            {
                db.Open();
                return db.Query<T>(Clean(sql), parameters);
            }
        }

        public int Execute(string sql, object parameters)
        {
            using (var db = new MySqlConnection(_dbConnection))
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
