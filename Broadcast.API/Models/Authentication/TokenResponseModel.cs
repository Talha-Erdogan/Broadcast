using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Models.Authentication
{
    public class TokenResponseModel :Business.Models.Employee.EmployeeWithDetail
    {
        public string UserToken { get; set; }

        public int TokenExpirePeriod { get; set; }
    }
}
