using System.Collections.Generic;
using System.Linq;

namespace Api.DataStore
{
    public enum DBWhereComparer { IsEqual, Equals, IsNotEqual, Like, In };
    public enum DBWhereOperator { None, And, Or };
    public class DBWhere : List<DBWhereColumn>
    {
        public string Flatten()
        {
            return string.Join(" ", this.Select(c => c.Flatten()));
        }
    }

    public class DBWhereColumn
    {
        private readonly string _column;
        private readonly DBWhereComparer _comparer;
        private readonly DBWhereOperator _operator;
        private readonly object _value;

        public DBWhereColumn(string c, object val, DBWhereComparer cmp = DBWhereComparer.Equals, DBWhereOperator op = DBWhereOperator.None)
        {
            _column = c;
            _comparer = cmp;
            _value = val;
            _operator = op;
        }

        public DBWhereColumn(string c, IEnumerable<object> val, DBWhereOperator op = DBWhereOperator.None)
        {
            _column = c;
            _comparer = DBWhereComparer.In;
            _value = string.Join(",", val);
            _operator = op;
        }

        public string Flatten()
        {
            return _column + ComparerToString() + GetValue() + OperatorToString();
        }

        private string GetValue()
        {
            if (_value == null)
                return "NULL";

            var t = _value.GetType();

            if (t == typeof(int) || t == typeof(decimal) || t == typeof(long))
                return _value.ToString();

            if (_comparer == DBWhereComparer.In)
                return $"({_value})";

            return $"'{_value}'";
        }

        private string ComparerToString()
        {
            switch (_comparer)
            {
                case DBWhereComparer.IsEqual:
                    return " is ";
                case DBWhereComparer.IsNotEqual:
                    return " is not ";
                case DBWhereComparer.Like:
                    return " like ";
                case DBWhereComparer.Equals:
                    return " = ";
                case DBWhereComparer.In:
                    return " in ";
                default:
                    return " = ";
            }
        }

        private string OperatorToString()
        {
            switch (_operator)
            {
                case DBWhereOperator.None:
                    return "";
                case DBWhereOperator.Or:
                    return " or ";
                case DBWhereOperator.And:
                    return " and ";
                default:
                    return " and ";
            }
        }

    }
}
