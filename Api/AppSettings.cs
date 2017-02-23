using System;
using Microsoft.Extensions.Configuration;

namespace Api
{
    public interface IAppSettings {
        string DB_Connection { get; set; }
        string Auth0_Domain { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public AppSettings(IConfigurationRoot config)
        {
            DB_Connection = Environment.GetEnvironmentVariable("APP_DB_CONNECTION") ?? config["App:DB_Connection"];
            Auth0_Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? config["Auth0:Domain"];
        }

        public string DB_Connection { get; set; }
        public string Auth0_Domain{ get; set; }
    }
}
