namespace Models
{
    public class OrgRole : Model
    {
        public int OrgRoleID { get; set; }
        public int OrgID { get; set; }
        public string Role { get; set; }
    }
}
