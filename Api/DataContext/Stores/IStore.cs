using System.Collections.Generic;
using Api.DataContext.Database;

namespace Api.DataContext.Stores
{
    public interface IStore<T, E>
    {
        IEnumerable<T> Get(DBWhere<E> where = null, E[] columns = null, int? limit = null);
        T Create(T obj, int memberId);
        void Update(T obj, int memberId);
        void Remove(long id, int memberId);
    }
}
