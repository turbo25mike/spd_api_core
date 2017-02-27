using System.Collections.Generic;
using Api.DataStore;
using Api.Extensions;
using FluentAssertions;
using Xunit;

namespace Api.Tests
{
    public class GenericExtensionsTests
    {
        [Fact]
        public void ShouldGetValue()
        {
            WhenGetValue(_member.PrimaryKey).Should().Be(_member.MemberID);
            WhenGetValue(nameof(Member.MemberID)).Should().Be(_member.MemberID);
            WhenGetValue(nameof(Member.LoginID)).Should().Be(_member.LoginID);
            WhenGetValue(nameof(Member.UserName)).Should().Be(_member.UserName);
        }

        [Fact]
        public void ShouldSetValue()
        {
            WhenSetValue(nameof(Member.MemberID), 2).Should().Be(2);
            WhenSetValue(nameof(Member.LoginID), "test").Should().Be("test");
            WhenSetValue(nameof(Member.UserName), "newName").Should().Be("newName");
        }

        [Fact]
        public void ShouldCreateSets()
        {
            WhenCreateSet();
            ThenSet.Should().ContainKey(nameof(Member.MemberID)).And.ContainValue(_member.MemberID);
            ThenSet.Should().ContainKey(nameof(Member.LoginID)).And.ContainValue(_member.LoginID);
            ThenSet.Should().ContainKey(nameof(Member.UserName)).And.ContainValue(_member.UserName);


            WhenCreateSet(new[] {nameof(Member.MemberID)});
            ThenSet.Should().ContainKey(nameof(Member.MemberID)).And.ContainValue(_member.MemberID);
            WhenCreateSet(new[] { nameof(Member.LoginID) });
            ThenSet.Should().ContainKey(nameof(Member.LoginID)).And.ContainValue(_member.LoginID);
            WhenCreateSet(new[] { nameof(Member.UserName) });
            ThenSet.Should().ContainKey(nameof(Member.UserName)).And.ContainValue(_member.UserName);
        }

        private readonly Member _member = new Member
            {
                MemberID = 1,
                LoginID = "testLoginID",
                UserName = "testUser"
            };

        public Dictionary<string, object> ThenSet;

        private object WhenGetValue(string prop)
        {
            return _member.GetValue(prop);
        }

        private object WhenSetValue(string prop, object val)
        {
            _member.SetValue(prop, val);
            return WhenGetValue(prop);
        }

        private void WhenCreateSet(string[] props = null)
        {
            ThenSet = _member.CreateSet(props);
        }
    }
}
