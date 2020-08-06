using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Broadcast
{
    public class GetAllPaginatedRequestModel : ListBaseRequestModel
    {
        public int? BroadcastTypeId { get; set; }
        public string TitleTR { get; set; }
        public string TitleEN { get; set; }
        public int? IsActive { get; set; }
        public string ValidationEndDateTime { get; set; }

    }
}
