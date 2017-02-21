using System;
using MySql.Data.MySqlClient;

namespace Business
{
    public class MemberContext
    {
        public static string GetAdmin()
        {
            var adminName = "";
            using (var conn = new MySqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "server=localhost;uid=root;pwd=admin"))
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
