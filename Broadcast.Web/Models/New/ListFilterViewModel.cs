using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.Web.Models.New
{
    public class ListFilterViewModel
    {
        // filters      
        public int? Filter_BroadcastTypeId { get; set; }
        public string Filter_TitleTR { get; set; }
        public string Filter_TitleEN { get; set; }
        public DateTime? Filter_ValidationEndDateTime { get; set; }
        public int? Filter_IsActive { get; set; }
    }
}
