using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.Broadcast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Interfaces
{
    public interface IBroadcastService
    {
        ApiResponseModel<PaginatedList<BroadcastWithDetail>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, BroadcastSearchFilter searchFilter);
        ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> GetById(string userToken, string displayLanguage, int id);
        ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> AddWithDetail(string userToken, string displayLanguage, AddBroadcastWithDetail addbroadcast);
        ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> EditWithDetail(string userToken, string displayLanguage, EditBroadcastWithDetail editbroadcast);
        ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> UpdateStatus(string userToken, string displayLanguage, int broadcastId, bool broadcastStatus);
    }
}
