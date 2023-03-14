using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sivam.web.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Device { get; set; }
        public string LoggedIn { get; set; }
        public string Location { get; set; }
    }
}