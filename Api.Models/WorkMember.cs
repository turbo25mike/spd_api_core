namespace Models
{
    public class WorkMember : Model
    {
        public int WorkMemberID { get; set; }
        public int WorkID { get; set; }
        public int OrgRoleID { get; set; }
        public int MemberID { get; set; }
    }
}
