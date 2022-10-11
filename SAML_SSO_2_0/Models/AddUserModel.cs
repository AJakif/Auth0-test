using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAML_SSO_2_0.Models
{
    public class AddUserModel
    {
        public Profile profile { get; set; }
        public Credentials credentials { get; set; }
    }

    public class Profile
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string mobilePhone { get; set; }
    }

    public class Credentials
    {
        public Password password { get; set; }
        public Recovery_question recovery_question { get; set; }
        public Provider provider { get; set; }
    }

    public class Password
    {
        public string value { get; set; }
    }

    public class Recovery_question
    {
        public string question { get; set; }
        public string answer { get; set; }
    }

    public class Provider
    {
        public string type { get; set; }
        public string name { get; set; }
    }

}
