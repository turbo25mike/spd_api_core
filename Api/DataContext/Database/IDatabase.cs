using System.Collections.Generic;

namespace Api.DataContext.Database
{
    public interface IDatabase
    {
        List<T> Select<T, TCol>(string tableName, TCol[] columns = null, DBWhere<TCol> whereColumns = null, int? limit = null);
        void Update<T, TCol>(string tableName, Dictionary<TCol, object> setColumns, DBWhere<TCol> whereColumns);
        long Insert<T, TCol>(string tableName, Dictionary<TCol, object> setColumns);
        //void Delete<T, TCol>(string connectionString, string tableName, DBWhere<TCol> whereColumns);
    }
}
