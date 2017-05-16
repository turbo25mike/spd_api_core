namespace Models
{
    public class Member : Model
    {
        public int MemberID { get; set; }
        public string LoginID { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Name { get; set; }

        public void Map(Auth0User auth)
        {
            Nickname = auth.nickname;
            Picture = auth.picture;
            Email = auth.email;
            EmailVerified = auth.email_verified;
            GivenName = auth.given_name;
            FamilyName = auth.family_name;
            Name = auth.name;
            LoginID = auth.user_id;
        }
    }
}
