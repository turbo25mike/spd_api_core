using System.Collections.Generic;

namespace Api.DataStore
{
    public interface IDatabase
    {
        List<T> Select<T>(string[] columns = null, DBWhere where = null, int? limit = null);
        IModel Update(IModel request, string[] set= null, DBWhere where = null);
        IModel Insert(IModel request);
        void Delete<T>(int id);
    }
}
