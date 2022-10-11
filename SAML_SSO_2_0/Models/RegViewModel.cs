using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAML_SSO_2_0.Models
{
    public class RegViewModel
    {
        [Required(ErrorMessage ="This is required")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string email { get; set; }

        public string mobilePhone { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string password { get; set; }

        public string question { get; set; }

        public string answer { get; set; }


    }
}
