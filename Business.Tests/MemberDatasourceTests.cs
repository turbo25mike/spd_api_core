using System;
using Business.DataStore;
using Moq;
using Xunit;

namespace Business.Tests
{
    public class MemberDatasourceTests
    {
        private MemberDatasource _datasource;

        public void Setup()
        {
            var db = new Mock<IDatabase>();
            _datasource = new MemberDatasource(db.Object);
        }

        [Fact]
        public void Test1()
        {
            Setup();
        }
    }
}
