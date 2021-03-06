﻿namespace Models
{
    public interface IAppSettings {
        string DB_Connection { get; set; }
        string Environment { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string DB_Connection { get; set; }
        public string Environment { get; set; }
    }
}
