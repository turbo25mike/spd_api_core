using System;
using System.Collections.Generic;
using Api.DataContext.Models;
using Api.DataContext.Database;

namespace Api.DataContext.Stores
{
    public class MemberStore: IStore<Member, MemberStore.Column>
    {
        private IDatabase _DB;
        private const string _TableName = "member";
        public enum Column { MemberID, LoginID, UserName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, RemovedBy, RemovedDate }

        public MemberStore(IDatabase db)
        {
            _DB = db;
        }

        public IEnumerable<Member> Get(DBWhere<Column> where = null, Column[] columns = null, int? limit = null)
        {
            columns = columns ?? new[] {
                Column.MemberID,
                Column.UserName,
                Column.UpdatedBy,
                Column.UpdatedDate,
                Column.CreatedDate,
                Column.CreatedBy
            };
            var results = _DB.Select<Member, Column>(_TableName, columns, where, limit);
            return results;
        }

        public Member Create(Member request, int memberId)
        {
            var creationDate = DateTime.Now;
            var set = new Dictionary<Column, object>
            {
                [Column.LoginID] = request.LoginID,
                [Column.UserName] = request.UserName,
                [Column.CreatedBy] = memberId,
                [Column.CreatedDate] = creationDate
            };
            var id = _DB.Insert<Member, Column>(_TableName, set);
            return new Member
            {
                MemberID = id,
                UserName = request.UserName,
                CreatedBy = memberId,
                CreatedDate = creationDate
            };
        }

        public void Update(Member request, int memberId)
        {
            var set = new Dictionary<Column, object>
            {
                [Column.UserName] = request.UserName,
                [Column.UpdatedBy] = memberId,
                [Column.UpdatedDate] = DateTime.Now
            };
            var where = new DBWhere<Column>
            {
                new DBWhereColumn<Column>(Column.MemberID, request.MemberID)
            };
            _DB.Update<Member, Column>(_TableName, set, where);
        }

        public void Remove(long id, int memberId)
        {
            var set = new Dictionary<Column, object>
            {
                [Column.RemovedBy] = memberId,
                [Column.RemovedDate] = DateTime.Now
            };
            var where = new DBWhere<Column>
            {
                new DBWhereColumn<Column>(Column.MemberID, id)
            };
            _DB.Update<Member, Column>(_TableName, set, where);
        }
    }
}
