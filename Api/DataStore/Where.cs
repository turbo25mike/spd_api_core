using System.Collections.Generic;
using System.Linq;
using Api.Extensions;

namespace Api.DataStore
{
    public enum WhereComparer { Is, Equals, IsNot, Like, In };
    public enum WhereOperator { None, And, Or };
    public class Where: List<WhereItem>
    {
        public string Flatten()
        {
            return string.Join(" ", this.Select(c => c.Flatten()));
        }
    }

    public interface IWhereItem
    {
        string Flatten();
        void AddOperator(WhereOperator op);
    }

    public class WhereColumns : WhereItem
    {
        private WhereOperator _operator;

        private readonly WhereItem[] _items;

        public WhereColumns(WhereItem[] items, WhereOperator op = WhereOperator.None)
        {
            _operator = op;
            _items = items;
        }

        public override string Flatten()
        {
            return $"({string.Join(" ", _items.Select(c => c.Flatten()))}) {OperatorToString()}";
        }

        public override void AddOperator(WhereOperator op)
        {
            _operator = op;
        }

        private string OperatorToString()
        {
            switch (_operator)
            {
                case WhereOperator.None:
                    return "";
                case WhereOperator.Or:
                    return " or ";
                case WhereOperator.And:
                    return " and ";
                default:
                    return " and ";
            }
        }
    }

    public abstract class WhereItem
    {
        public abstract string Flatten();
        public abstract void AddOperator(WhereOperator op);
    }

    public class WhereColumn<T> : WhereItem
    {
        private readonly string _column;
        private readonly WhereComparer _comparer;
        private WhereOperator _operator;
        private readonly object _value;

        public WhereColumn(string columnName, object columnValue = null, WhereComparer cmp = WhereComparer.Equals, WhereOperator op = WhereOperator.None)
        {
            _column = columnName;
            _comparer = cmp;
            _value = columnValue;
            _operator = op;
        }

        public WhereColumn(string c, IEnumerable<double> val, WhereOperator op = WhereOperator.None)
        {
            _column = c;
            _comparer = WhereComparer.In;
            _value = string.Join(",", val);
            _operator = op;
        }

        public WhereColumn(string c, IEnumerable<decimal> val, WhereOperator op = WhereOperator.None)
        {
            _column = c;
            _comparer = WhereComparer.In;
            _value = string.Join(",", val);
            _operator = op;
        }

        public WhereColumn(string c, IEnumerable<int> val, WhereOperator op = WhereOperator.None)
        {
            _column = c;
            _comparer = WhereComparer.In;
            _value = string.Join(",", val);
            _operator = op;
        }

        public WhereColumn(string c, IEnumerable<string> val, WhereOperator op = WhereOperator.None)
        {
            _column = c;
            _comparer = WhereComparer.In;
            _value = string.Join(",", val.Select(v => $"'{v}'"));
            _operator = op;
        }

        public override string Flatten()
        {
            return $"{typeof(T).Name.SplitNameOnUppercase()}.{_column} {ComparerToString()} {GetValue()} {OperatorToString()}";
        }

        public override void AddOperator(WhereOperator op)
        {
            _operator = op;
        }

        private string GetValue()
        {
            if (_value == null || _value.ToString() == "")
                return "NULL";

            var t = _value.GetType();

            if (t == typeof(int) || t == typeof(decimal) || t == typeof(long) || t == typeof(double))
                return _value.ToString();

            if (_comparer == WhereComparer.In)
                return $"({_value})";

            return $"'{_value}'";
        }

        private string ComparerToString()
        {
            return _comparer == WhereComparer.Equals ? "=" : _comparer.ToString().SplitNameOnUppercase(" ");
        }

        private string OperatorToString()
        {
            switch (_operator)
            {
                case WhereOperator.None:
                    return "";
                case WhereOperator.Or:
                    return "or";
                case WhereOperator.And:
                    return "and";
                default:
                    return "and";
            }
        }

    }
}
