using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.ProfileEmployee
{
    public class GetAllEmployeeWhichIsNotIncludedPaginatedRequestModel : ListBaseRequestModel
    {
        public int ProfileId { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_LastName { get; set; }
    }
}
