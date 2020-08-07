using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Authentication
{
   public class LoginResponseModel :Employee.EmployeeWithDetail
    {
        public string UserToken { get; set; }
        public int TokenExpirePeriod { get; set; }
    }
}
