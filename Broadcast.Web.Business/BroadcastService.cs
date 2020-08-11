using Broadcast.Web.Business.Common;
using Broadcast.Web.Business.Common.Enums;
using Broadcast.Web.Business.Helpers;
using Broadcast.Web.Business.Interfaces;
using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.Broadcast;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Broadcast.Web.Business
{
    public class BroadcastService : IBroadcastService
    {
        public ApiResponseModel<PaginatedList<BroadcastWithDetail>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, BroadcastSearchFilter searchFilter)
        {
            ApiResponseModel<PaginatedList<BroadcastWithDetail>> result = new ApiResponseModel<PaginatedList<BroadcastWithDetail>>()
            {
                Data = new PaginatedList<BroadcastWithDetail>(new List<BroadcastWithDetail>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection)
            };
            //todo: portal api'den çekme işlemi olacak            
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Broadcasts?CurrentPage={0}&PageSize={1}&SortOn={2}&SortDirection={3}&BroadcastTypeId={4}&TitleTR={5}&TitleEN={6}&IsActive={7}&ValidationEndDateTime={8}",
                   searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection, searchFilter.Filter_BroadcastTypeId, searchFilter.Filter_TitleTR, searchFilter.Filter_TitleEN, searchFilter.Filter_IsActive, searchFilter.Filter_ValidationEndDateTimeAsString)).Result;

                result = response.Content.ReadAsJsonAsync<ApiResponseModel<PaginatedList<BroadcastWithDetail>>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> GetById(string userToken, string displayLanguage, int id)
        {
            ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> result = new ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>();
            // portal api'den çekme işlemi             
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                HttpResponseMessage response = httpClient.GetAsync(string.Format("v1/Broadcasts/" + id + "/getbyid")).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>>().Result;
            }
            return result;
        }

        public ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> AddWithDetail(string userToken, string displayLanguage, AddBroadcastWithDetail addbroadcast)
        {
            ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> result = new ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>();

            // ilk olarak image varsa o kayıt edilir.
            ApiResponseModel<string> resultImageAdd = new ApiResponseModel<string>();
            if (addbroadcast.ImageInformation != null && addbroadcast.ImageInformation.Length > 0)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                    httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                    HttpResponseMessage response = httpClient.PostAsImageFiles(string.Format("v1/Broadcasts/upload-file"), addbroadcast.ImageInformation).Result;
                    resultImageAdd = response.Content.ReadAsJsonAsync<ApiResponseModel<string>>().Result;
                    if (resultImageAdd.ResultStatusCode != ResultStatusCodeStatic.Success)
                    {
                        result.ResultStatusCode = resultImageAdd.ResultStatusCode;
                        result.ResultStatusMessage = resultImageAdd.ResultStatusMessage;
                        return result;
                    }
                }
            }

            //sonrasında veriler kayıt edilir.
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.TitleTR = addbroadcast.TitleTR;
                portalApiRequestModel.TitleEN = addbroadcast.TitleEN;
                portalApiRequestModel.DescriptionTR = addbroadcast.DescriptionTR;
                portalApiRequestModel.DescriptionEN = addbroadcast.DescriptionEN;
                portalApiRequestModel.BroadcastTypeId = addbroadcast.BroadcastTypeId;
                portalApiRequestModel.BroadcastTypeId = addbroadcast.BroadcastTypeId;
                portalApiRequestModel.BroadcastTypeId = addbroadcast.BroadcastTypeId;
                portalApiRequestModel.BroadcastTypeId = addbroadcast.BroadcastTypeId;
                if (addbroadcast.ImageInformation != null && addbroadcast.ImageInformation.Length > 0)
                {
                    portalApiRequestModel.ImageFilePath = resultImageAdd.Data;
                }
                portalApiRequestModel.VideoFileUrl = addbroadcast.VideoFileUrl;
                portalApiRequestModel.ValidationEndDateTime = addbroadcast.ValidationEndDateTime;
                portalApiRequestModel.IsActive = false;
                portalApiRequestModel.CreatedBy = SessionHelper.CurrentUser.ID;
                portalApiRequestModel.BroadcastTypeId = addbroadcast.BroadcastTypeId;

                HttpResponseMessage response = httpClient.PostAsJsonAsync(string.Format("v1/Broadcasts"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>>().Result;
            }

            return result;
        }

        public ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> EditWithDetail(string userToken, string displayLanguage, EditBroadcastWithDetail editbroadcast)
        {
            ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> result = new ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>();

            // ilk olarak image varsa o kayıt edilir.
            ApiResponseModel<string> resultImageAdd = new ApiResponseModel<string>();
            if (editbroadcast.ImageInformation != null && editbroadcast.ImageInformation.Length > 0)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                    httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                    HttpResponseMessage response = httpClient.PostAsImageFiles(string.Format("v1/Broadcasts/upload-file"), editbroadcast.ImageInformation).Result;
                    resultImageAdd = response.Content.ReadAsJsonAsync<ApiResponseModel<string>>().Result;
                    if (resultImageAdd.ResultStatusCode != ResultStatusCodeStatic.Success)
                    {
                        result.ResultStatusCode = resultImageAdd.ResultStatusCode;
                        result.ResultStatusMessage = resultImageAdd.ResultStatusMessage;
                        return result;
                    }
                }
            }

            //sonrasında veriler kayıt edilir.
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);

                var portalApiRequestModel = new AddRequestModel();
                portalApiRequestModel.Id = editbroadcast.Id;
                portalApiRequestModel.TitleTR = editbroadcast.TitleTR;
                portalApiRequestModel.TitleEN = editbroadcast.TitleEN;
                portalApiRequestModel.DescriptionTR = editbroadcast.DescriptionTR;
                portalApiRequestModel.DescriptionEN = editbroadcast.DescriptionEN;
                portalApiRequestModel.BroadcastTypeId = editbroadcast.BroadcastTypeId;
                portalApiRequestModel.BroadcastTypeId = editbroadcast.BroadcastTypeId;
                portalApiRequestModel.BroadcastTypeId = editbroadcast.BroadcastTypeId;
                portalApiRequestModel.BroadcastTypeId = editbroadcast.BroadcastTypeId;
                if (editbroadcast.ImageInformation != null && editbroadcast.ImageInformation.Length > 0)
                {
                    portalApiRequestModel.ImageFilePath = resultImageAdd.Data;
                }
                portalApiRequestModel.VideoFileUrl = editbroadcast.VideoFileUrl;
                portalApiRequestModel.ValidationEndDateTime = editbroadcast.ValidationEndDateTime;
                portalApiRequestModel.IsActive = editbroadcast.IsActive;
                portalApiRequestModel.ModifiedBy = SessionHelper.CurrentUser.ID;
                portalApiRequestModel.ModifiedDateTime = DateTime.Now;
                portalApiRequestModel.BroadcastTypeId = editbroadcast.BroadcastTypeId;

                HttpResponseMessage response = httpClient.PutAsJsonAsync(string.Format("v1/Broadcasts/" + portalApiRequestModel.Id), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>>().Result;
            }

            return result;
        }


        public ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> UpdateStatus(string userToken, string displayLanguage, int broadcastId, bool broadcastStatus)
        {
            ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast> result = new ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>();

            // api'yi çağırma yapılır, gelen sonuç elde edilir
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(ConfigHelper.ApiUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                httpClient.DefaultRequestHeaders.Add("DisplayLanguage", displayLanguage);
                var portalApiRequestModel = new UpdateStatusRequestModel();
                portalApiRequestModel.IsActive = broadcastStatus;
                HttpResponseMessage response = httpClient.PutAsJsonAsync(string.Format("v1/Broadcasts/" + broadcastId + "/status"), portalApiRequestModel).Result;
                result = response.Content.ReadAsJsonAsync<ApiResponseModel<Broadcast.Web.Business.Models.Broadcast.Broadcast>>().Result;
            }
            return result;
        }

    }
}
