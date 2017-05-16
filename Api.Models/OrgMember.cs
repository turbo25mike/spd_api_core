namespace Models
{
    public class OrgMember : Model
    {
        public int OrgMemberID { get; set; }
        public int OrgID { get; set; }
        public int MemberID { get; set; }
    }
}
