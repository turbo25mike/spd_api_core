﻿using System;

namespace Models
{
    public class Auth0User
    {
        public bool email_verified { get; set; }
        public string email { get; set; }
        public string clientID { get; set; }
        public DateTime updated_at { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string user_id { get; set; }
        public string nickname { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public Auth0Identities[] identities { get; set; }
        public DateTime created_at { get; set; }
        public string sub { get; set; }
    }
}
