using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Broadcast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
    public interface IBroadcastService
    {
        PaginatedList<BroadcastWithDetail> GetAllPaginatedWithDetailBySearchFilter(BroadcastSearchFilter searchFilter);
        PaginatedList<BroadcastWithDetail> GetAllPaginatedWhichIsActiveWithDetailBySearchFilter(BroadcastSearchFilter searchFilter);
        List<Data.Entity.Broadcast> GetAllWhichIsActive();
        Data.Entity.Broadcast GetById(int id);
        int Add(Data.Entity.Broadcast record);
        int Update(Data.Entity.Broadcast record);


    }
}
