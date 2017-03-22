using Api.DataStore;
using FluentAssertions;
using Xunit;

namespace Api.Tests.DataStore
{
    public class DBWhereTests
    {
        [Fact]
        public void ShouldFlattenSingle()
        {
            GivenWhere(new Where { new WhereColumn<Member>(Column, 1)});
            WhenFlatten();
            ThenFlattened.Should().Be($"{nameof(Member)}.{Column} = 1 ");
        }

        [Fact]
        public void ShouldFlattenMultiple()
        {
            GivenWhere(new Where
                {
                    new WhereColumn<Member>(Column, 1, op:WhereOperator.And),
                    new WhereColumn<Org>(Column, 2)
                });
            WhenFlatten();
            ThenFlattened.Should().Be($"{nameof(Member)}.{Column} = 1 and {nameof(Org)}.{Column} = 2 ");
        }

        private void GivenWhere(Where where)
        {
            _where = where;
        }

        private void WhenFlatten()
        {
            ThenFlattened = _where.Flatten();
        }

        private const string Column = "test";

        public string ThenFlattened;

        private Where _where = new Where { new WhereColumn<Member>(Column, 1) };
    }
}
