using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Broadcast
{
    public class BroadcastSearchFilter
    {
        // paging
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        // sorting
        public string SortOn { get; set; }
        public string SortDirection { get; set; }

        // filters   
        public int? Filter_BroadcastTypeId { get; set; }
        public string Filter_TitleTR { get; set; }
        public string Filter_TitleEN { get; set; }
        public int? Filter_IsActive { get; set; }
        public DateTime? Filter_ValidationEndDateTime { get; set; }
    }
}
