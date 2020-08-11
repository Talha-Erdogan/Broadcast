using Broadcast.Web.Business.Common;
using Broadcast.Web.Business.Helpers;
using Broadcast.Web.Business.Interfaces;
using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.BroadcastType;
using Broadcast.Web.Business.Models.Sex;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Broadcast.Web.Business
{
   public class SexService : ISexService
    {
        public ApiResponseModel<List<Sex>> GetAll(string userToken, string displayLanguage)
        {
            ApiResponseModel<List<Sex>> result = new ApiResponseModel<List<Sex>>();
            // portal api'den çekme işlemi 
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Sex")).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<List<Sex>>>().Result;
            }
            return result;
        }
    }
}
