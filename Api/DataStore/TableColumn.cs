using System.Collections.Generic;
using System.Linq;
using Api.Extensions;

namespace Api.DataStore
{
    public class TableColumns : List<TableItem>
    {
        public string Flatten()
        {
            return string.Join(",", this.Select(c => c.Flatten()));
        }
    }

    public abstract class TableItem
    {
        public abstract string Flatten();
    }

    public class TableColumn<T>: TableItem
    {
        private readonly string _column;

        public TableColumn(string col)
        {
            _column = col;
        }

        public override string Flatten()
        {
            return $"{typeof(T).Name.SplitNameOnUppercase()}.{_column}";
        }
    }
}
