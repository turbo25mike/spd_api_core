using System.Collections.Generic;
using System.Collections;

namespace Api.DataStore
{
    public interface IDatabase
    {
        List<T> Select<T>(TableColumns columns = null, Joins joins = null, Where where = null, int? limit = null);
        IModel Update(IModel request, int memberID, string[] set= null, Where where = null);
        IModel Insert(IModel request, int memberID);
        void Delete<T>(int id, int memberID);
    }
}
