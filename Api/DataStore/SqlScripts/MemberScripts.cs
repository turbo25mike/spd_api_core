namespace Api.DataStore.SqlScripts
{
    public static class MemberScripts
    {
        public static string GetMember = 
            @"SELECT * FROM member WHERE LoginID = @LoginID;";

        public static string Update =
            @"UPDATE member 
            SET  LoginID = @LoginID, Nickname = @Nickname, Picture = @Picture, Email = @Email, EmailVerified = @EmailVerified, GivenName = @GivenName, FamilyName = @FamilyName, Name = @Name, UpdatedBy = @UpdatedBy, UpdatedDate = NOW()
            WHERE MemberID = @MemberID;";

        public static string Insert =
            @"INSERT INTO member 
            ( `LoginID`,`Nickname`,`Picture`,`Email`,`EmailVerified`,`GivenName`,`FamilyName`,`Name`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate` )
            VALUES 
            ( @Nickname,@Picture,@Email,@EmailVerified,@GivenName,@FamilyName,@Name,2,NOW(),2,NOW() );
            SELECT LAST_INSERT_ID();";
    }
}
