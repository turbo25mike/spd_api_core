using Models;
using Business.DataStore;

namespace Business
{
    public interface IMemberDatasource
    {
        Member Get(int memberID);
        Member Get(string identity);
        int Insert(Member member);
        void Update(Member member);
    }

    public class MemberDatasource : Datasource, IMemberDatasource
    {
        public MemberDatasource(IDatabase db) : base(db) {}

        public int Insert(Member member)
        {
            var script = @"INSERT INTO member 
            ( `LoginID`,`Nickname`,`Picture`,`Email`,`EmailVerified`,`GivenName`,`FamilyName`,`Name`,`CreatedBy`,`CreatedDate`,`UpdatedBy`,`UpdatedDate` )
            VALUES 
            ( @Nickname,@Picture,@Email,@EmailVerified,@GivenName,@FamilyName,@Name,2,NOW(),2,NOW() );
            SELECT LAST_INSERT_ID();";

            return DB.QuerySingle<int>(script, member);
        }

        public void Update(Member member)
        {
            var script = @"UPDATE member 
            SET  LoginID = @LoginID, Nickname = @Nickname, Picture = @Picture, Email = @Email, EmailVerified = @EmailVerified, GivenName = @GivenName, FamilyName = @FamilyName, Name = @Name, UpdatedBy = @UpdatedBy, UpdatedDate = NOW()
            WHERE MemberID = @MemberID;";

            DB.Execute(script, member);
        }

        public Member Get(string identity)
        {
            var script = @"SELECT * FROM member WHERE LoginID = @identity;";

            return DB.QuerySingle<Member>(script, new { identity });
        }

        public Member Get(int memberID)
        {
            var script = @"SELECT * FROM member WHERE MemberID = @memberID;";

            return DB.QuerySingle<Member>(script, new { memberID });
        }
    }
}
