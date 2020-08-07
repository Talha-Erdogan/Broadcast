using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.BroadcastType;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Interfaces
{
    public interface ISexService
    {
        ApiResponseModel<List<BroadcastType>> GetAll(string userToken, string displayLanguage);

    }
}
