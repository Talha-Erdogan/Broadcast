using Broadcast.Web.Business.Common;
using Broadcast.Web.Business.Helpers;
using Broadcast.Web.Business.Interfaces;
using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.BroadcastType;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Broadcast.Web.Business
{
    public class BroadcastTypeService : IBroadcastTypeService
    {
        public ApiResponseModel<List<BroadcastType>> GetAll(string userToken, string displayLanguage)
        {
            ApiResponseModel<List<BroadcastType>> result = new ApiResponseModel<List<BroadcastType>>();
            // portal api'den çekme işlemi 
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                //HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/BroadcastTypes/GetAll")).Result;
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/BroadcastTypes")).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<List<BroadcastType>>>().Result;
            }
            return result;
        }

    }
}
