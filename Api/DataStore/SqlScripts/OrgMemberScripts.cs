namespace Api.DataStore.SqlScripts
{
    public static class OrgMemberScripts
    {
        public static string GetMemberOrgIDs = @"SELECT OrgID FROM OrgMember WHERE MemberID = @MemberID";

        public static string Insert =
            @"INSERT INTO org_member
            ( `OrgID`,`MemberID`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate` )
            VALUES
            ( @OrgID,@MemberID,@CreatedBy,NOW(),@UpdatedBy,NOW() );
            SELECT CAST(SCOPE_IDENTITY() as int);";
    }
}
