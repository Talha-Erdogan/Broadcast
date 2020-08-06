using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.ProfileEmployee
{
    public class DeleteByProfileIdAndEmployeeIdRequestModel
    {
        public int ProfileId { get; set; }
        public int EmployeeId { get; set; }
    }
}
