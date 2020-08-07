using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Models.Authentication
{
    public class EmployeeLoginResponse
    {
        public bool IsValid { get; set; }
        public Business.Models.Employee.EmployeeWithDetail EmployeeWithDetail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
