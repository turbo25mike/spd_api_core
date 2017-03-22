using System.Collections.Generic;
using System.Linq;
using Api.Extensions;

namespace Api.DataStore
{
    public class Joins : List<JoinItem>
    {
        public string Flatten()
        {
            return string.Join(" ", this.Select(c => c.Flatten()));
        }
    }

    public abstract class JoinItem
    {
        public abstract string Flatten();
    }

    public class Join<TModelA, TModelB>: JoinItem where TModelA: IModel where TModelB : IModel
{
        private readonly string _tableA;
        private readonly string _tableB;
        private readonly string _colA;
        private readonly string _colB;
        private readonly JoinStyle _joinery;

        public enum JoinStyle{ Left, Right, Inner, Outer};

        public Join(string colA, string colB, JoinStyle joinery = JoinStyle.Inner)
        {
            _tableA = typeof(TModelA).Name.SplitNameOnUppercase();
            _tableB = typeof(TModelB).Name.SplitNameOnUppercase();
            _colA = colA;
            _colB = colB;
            _joinery = joinery;
        }

        public override string Flatten()
        {
            return $"{_joinery} join {_tableB} on {_tableB}.{_colB} = {_tableA}.{_colA}";
        }
    }
}
