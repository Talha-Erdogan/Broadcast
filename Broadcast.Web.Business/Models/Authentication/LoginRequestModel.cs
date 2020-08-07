using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Authentication
{
    public class LoginRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
