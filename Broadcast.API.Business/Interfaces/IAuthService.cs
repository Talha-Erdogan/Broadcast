using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Auth;
using Broadcast.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
    public interface IAuthService
    {
        PaginatedList<Auth> GetAllPaginatedWithDetailBySearchFilter(AuthSearchFilter searchFilter);
        List<Auth> GetAll();
        Auth GetById(int id);
        int Add(Auth record);
        int Update(Auth record);
    }
}
