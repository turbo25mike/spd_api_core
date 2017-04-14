namespace Api.DataStore.SqlScripts
{
    public static class OrgScripts
    {
        public static string GetMemberOrgs = 
            @"SELECT o.* FROM org_member om 
            JOIN org o ON o.OrgID = om.OrgID
            WHERE MemberID = @MemberID";

        public static string Insert =
            @"INSERT INTO org
            ( Name,BillingID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate )
            VALUES
            ( @Name, @BillingID, @CreatedBy, NOW(), @UpdatedBy, NOW() );
            SELECT LAST_INSERT_ID();";

        public static string Update =
            @"UPDATE org
            SET Name=@Name,BillingID=@BillingID, UpdatedBy=@UpdatedBy,UpdatedDate=NOW()
            WHERE @OrgID = @OrgID;";
    }
}
