namespace Api.DataContext.Models
{
    public interface IAppSettings {
        string DB_Connection { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string DB_Connection { get; set; }
    }
}
