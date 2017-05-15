namespace Api.DataStore.SqlScripts
{
    public static class OrgMemberScripts
    {
        public static string GetMemberOrgIDs = @"SELECT OrgID FROM OrgMember WHERE MemberID = @MemberID AND RemovedDate IS NULL";

        public static string IsMember = @"SELECT MemberID FROM OrgMember WHERE MemberID = @MemberID AND OrgID = @OrgID AND RemovedDate IS NULL";

        public static string Insert =
            @"INSERT INTO org_member
            ( `OrgID`,`MemberID`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate` )
            VALUES
            ( @OrgID,@MemberID,@CreatedBy,NOW(),@UpdatedBy,NOW() );
            SELECT LAST_INSERT_ID();";
    }
}
