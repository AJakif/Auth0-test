using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAML_SSO_2_0.Models
{
    
    public class ResponseModel
    {
        public string id { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public string activated { get; set; }
        public string statusChanged { get; set; }
        public string lastLogin { get; set; }
        public DateTime lastUpdated { get; set; }
        public string passwordChanged { get; set; }
        public Profile profile { get; set; }
        public Credentials credentials { get; set; }
        public _links _links { get; set; }
        public Self self { get; set; }
    }

    public class _links
    {
        public Activate activate { get; set; }
    }

    public class Activate
    {
        public string href { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }
    
}
