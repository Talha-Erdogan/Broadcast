using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.BroadcastType;
using Broadcast.Web.Business.Models.Sex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Interfaces
{
    public interface ISexService
    {
        ApiResponseModel<List<Sex>> GetAll(string userToken, string displayLanguage);

    }
}
