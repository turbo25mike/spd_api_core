namespace Api.DataStore.SqlScripts
{
    public static class MemberScripts
    {
        public static string GetMember = 
            @"SELECT * FROM member WHERE LoginID = @LoginID;";

        public static string Update = 
            @"UPDATE member 
            SET  LoginID = @LoginID,  UserName = @UserName
            WHERE MemberID = @MemberID;";

        public static string Insert =
            @"INSERT INTO member 
            ( `LoginID`,`UserName`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate` )
            VALUES 
            ( @LoginID, @UserName, 0, NOW(), 0, NOW() );
            SELECT CAST(SCOPE_IDENTITY() as int);";
    }
}
