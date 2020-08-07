using Broadcast.Web.Business.Common;
using Broadcast.Web.Business.Helpers;
using Broadcast.Web.Business.Interfaces;
using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Broadcast.Web.Business
{
    public class AuthenticationService : IAuthenticationService
    {
        public ApiResponseModel<LoginResponseModel> Login(string username, string password, string displayLanguage)
        {
            ApiResponseModel<LoginResponseModel> result = new ApiResponseModel<LoginResponseModel>();
            //todo: portal api'den çekme işlemi olacak
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                var portalApiRequestModel = new LoginRequestModel();
                portalApiRequestModel.UserName = username;
                portalApiRequestModel.Password = password;
                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/Authentication/Token"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<LoginResponseModel>>().Result;
            }
            return result;
        }


    }
}
