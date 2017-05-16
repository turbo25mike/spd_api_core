using System;
using System.Collections.Generic;
using Models;
using Business.DataStore;

namespace Business
{
    public interface IOrgDatasource
    {
        IEnumerable<Org> Get(int memberID);
        IEnumerable<Ticket> GetTickets(int id, int memberID);
        IEnumerable<Member> GetMembers(int id, int memberID);

        int Insert(Org org, int memberID);
        int InsertMember(int id, int newMemberID, int memberID);
        void Update(Org org, int memberID);
        
        bool IsMember(int id, int memberID);
    }

    public class OrgDatasource : Datasource, IOrgDatasource
    {
        public OrgDatasource(IDatabase db) : base(db) {}

        public IEnumerable<Ticket> GetTickets(int id, int memberID)
        {
            if (!IsMember(id, memberID))
                return null;
            var script = @"SELECT * FROM Org o
              LEFT JOIN work w ON w.OrgID = o.OrgID
              LEFT JOIN Ticket t ON t.WorkID = w.WorkID
              WHERE o.OrgID = @id AND w.RemovedBy IS NULL AND t.RemovedBy IS NULL AND Resolved = false;";

            return DB.Query<Ticket>(script, new { id });
        }

        public bool IsMember(int id, int memberID)
        {
            var script = @"SELECT MemberID FROM org_member
              Where OrgID = @id AND MemberID = @memberID AND RemovedDate IS NULL";

            var isMember = DB.QuerySingle<OrgMember>(script, new { id, memberID });
            return isMember != null;
        }

        public IEnumerable<Org> Get(int memberID)
        {
            var script = @"SELECT OrgID FROM org_member WHERE MemberID = @MemberID AND RemovedDate IS NULL";
            return DB.Query<Org>(script, new { memberID });
        }

        public IEnumerable<Member> GetMembers(int id, int memberID)
        {
            var script = @"SELECT o.* FROM org_member om 
            JOIN org o ON o.OrgID = om.OrgID
            WHERE om.MemberID = @memberID AND om.OrgID = @id AND om.RemovedDate IS NULL";

            return DB.Query<Member>(script, new { id, memberID });
        }

        public int Insert(Org org, int memberID)
        {
            var script = @"INSERT INTO org
            ( Name,BillingID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
            VALUES
            ( @Name, @BillingID, @CreatedBy, NOW(), @UpdatedBy, NOW() );
            SELECT LAST_INSERT_ID();";

            org.CreatedBy = memberID;
            org.UpdatedBy = memberID;
            org.OrgID = DB.QuerySingle<int>(script, org);
            InsertMember(org.OrgID, memberID, memberID);
            return org.OrgID;
        }

        public int InsertMember(int id, int newMemberID, int memberID)
        {
            if (!IsMember(id, memberID))
                throw new ArgumentOutOfRangeException();

            var script = @"INSERT INTO org_member
            ( `OrgID`,`MemberID`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate` )
            VALUES
            ( @OrgID,@newMemberID,@memberID,NOW(),@memberID,NOW() );
            SELECT LAST_INSERT_ID();";

            return DB.QuerySingle<int>(script, new { newMemberID, memberID});
        }

        public void Update(Org org, int memberID)
        {
            var script = @"UPDATE org
            SET Name=@Name,BillingID=@BillingID, UpdatedBy=@UpdatedBy,UpdatedDate=NOW()
            WHERE @OrgID = @OrgID;";
            org.UpdatedBy = memberID;
            DB.Execute(script, org);
        }
        
    }
}
