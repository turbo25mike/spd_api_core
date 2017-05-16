namespace Models
{
    public class Auth0Identities
    {
        public string user_id { get; set; }
        public string provider { get; set; }
        public string connection { get; set; }
        public bool isSocial { get; set; }
    }
}
