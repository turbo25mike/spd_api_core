using System;
using Api.Models;
using MySql.Data.MySqlClient;

namespace Api.DataContext
{
    public class MemberContext
    {
        private IAppSettings _settings;

        public MemberContext(IAppSettings settings)
        {
            _settings = settings;
        }

        public string GetAdmin()
        {
            var adminName = "";
            using (var conn = new MySqlConnection(_settings.DB_Connection))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT UserName FROM member Limit 1", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        adminName = reader.GetString("UserName");
                    }
                }
            }
            return adminName;
        }
    }
}
