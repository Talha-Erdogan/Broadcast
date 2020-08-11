using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.Web.Models.Announcement
{
    public class ListViewModel : ListBaseViewModel<Business.Models.Broadcast.BroadcastWithDetail, ListFilterViewModel>
    {
        public List<SelectListItem> FilterIsActiveSelectList { get; set; }
    }
}
