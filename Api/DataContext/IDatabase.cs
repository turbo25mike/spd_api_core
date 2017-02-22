using System.Collections.Generic;

namespace Api.DataContext
{
    public interface IDatabase
    {
        List<T> Select<T>(string[] columns = null, DBWhere where = null, int? limit = null);
        IModel Update(IModel request, string[] set, DBWhere where, int memberID);
        IModel Insert(IModel request, int memberID);
        void Delete<T>(int id, int memberID);
    }
}
