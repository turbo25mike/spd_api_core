﻿namespace Api
{
    public interface IAppSettings {
        string DB_Connection { get; set; }
        string Auth0_Domain { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string DB_Connection { get; set; }
        public string Auth0_Domain{ get; set; }
    }
}