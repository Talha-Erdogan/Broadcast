using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Models.Broadcasts
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
