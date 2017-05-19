using System;
using Business.DataStore;
using Models;
using Moq;
using Xunit;

namespace Business.Tests
{
    public class MemberDatasourceTests
    {
        private MemberDatasource _datasource;
        private int _someID = 1;

        public void Setup()
        {
            var db = new Mock<IDatabase>();
            _datasource = new MemberDatasource(db.Object);
        }

        [Fact]
        public void ShouldGetMember()
        {
            Setup();
            WhenGetMember();
        }

        private void WhenGetMember()
        {
            ThenMember = _datasource.Get(_someID);
        }

        private Member ThenMember { get; set; }
    }
}
