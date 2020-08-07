using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Interfaces
{
    public interface IAuthenticationService
    {
        ApiResponseModel<LoginResponseModel> Login(string username, string password, string displayLanguage);
    }
}
