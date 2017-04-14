using System.Collections.Generic;

namespace Api.DataStore
{
    public interface IDatabase
    {
        IEnumerable<T> Query<T>(string sql, object parameters);
        T QuerySingle<T>(string sql, object parameters);
        int Execute(string sql, object parameters);
    }
}
