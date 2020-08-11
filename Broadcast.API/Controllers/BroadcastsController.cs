using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Broadcast.API.Business.Interfaces;
using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Broadcast;
using Broadcast.API.Common;
using Broadcast.API.Common.Enums;
using Broadcast.API.Common.Model;
using Broadcast.API.Filters;
using Broadcast.API.Models;
using Broadcast.API.Models.Broadcasts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Broadcast.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BroadcastsController : ControllerBase
    {
        private readonly IBroadcastService _broadcastService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BroadcastsController(IBroadcastService broadcastService, IWebHostEnvironment hostingEnvironment)
        {
            _broadcastService = broadcastService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("")]
        [HttpGet]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_ANNOUNCEMENT_LIST, AuthCodeStatic.PAGE_NEWS_LIST)]
        [ProducesResponseType(typeof(ApiResponseModel<PaginatedList<BroadcastWithDetail>>), 200)]
        public IActionResult GetAllPaginatedWithDetail([FromQuery] GetAllPaginatedRequestModel requestModel, [FromHeader] string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<BroadcastWithDetail>> { DisplayLanguage = displayLanguage };
            try
            {
                var searchFilter = new BroadcastSearchFilter
                {
                    CurrentPage = requestModel.CurrentPage ?? 1,
                    PageSize = requestModel.PageSize ?? 10,
                    SortOn = requestModel.SortOn,
                    SortDirection = requestModel.SortDirection,
                    Filter_BroadcastTypeId = requestModel.BroadcastTypeId,
                    Filter_TitleTR = requestModel.TitleTR,
                    Filter_TitleEN = requestModel.TitleEN,
                    Filter_IsActive = requestModel.IsActive,
                };
                //tarihe göre filitreleme istenildiğinde formatlı şekilde tarih bilgisi alınması işlevi
                if (!string.IsNullOrEmpty(requestModel.ValidationEndDateTime))
                {
                    if (DateTime.TryParseExact(requestModel.ValidationEndDateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endDate))
                    {
                        searchFilter.Filter_ValidationEndDateTime = endDate;
                    }
                }

                responseModel.Data = _broadcastService.GetAllPaginatedWithDetailBySearchFilter(searchFilter);
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{broadcastType}")]
        [HttpGet]
        [TokenAuthorizeFilter]
        [ProducesResponseType(typeof(ApiResponseModel<PaginatedList<BroadcastWithDetail>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel<object>), (int)HttpStatusCode.NotFound)]
        public IActionResult GetByBroadcastType(string broadcastType, [FromQuery]GetAllPaginatedWhichIsActiveRequestModel requestModel, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<BroadcastWithDetail>> { DisplayLanguage = displayLanguage };
            // Set broadcast type name by route
            if (!Enum.TryParse(typeof(BroadcastTypeEnum), broadcastType, true, out var broadcastTypeId))
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = $"'{broadcastType}' resource not found";

                return NotFound(responseModel);
            }

            var fileHostBaseUrl = GetFileHostBaseUrlFromCurrentRequest(); // şirket resimlerinin tam url adres bilgisi icin kullanilacak

            try
            {
                var searchFilter = new BroadcastSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_BroadcastTypeId = (int)broadcastTypeId;
                responseModel.Data = _broadcastService.GetAllPaginatedWhichIsActiveWithDetailBySearchFilter(searchFilter);

                //resimlerin link olarak geriye döndürülmesi
                if (responseModel.Data != null && responseModel.Data.Items.Count > 0)
                {
                    foreach (var item in responseModel.Data.Items)
                    {
                        if (item.ImageFilePath != null)
                        {
                            item.ImageFilePath = fileHostBaseUrl + "" + item.ImageFilePath;
                        }
                    }
                }

                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{id}/getbyid")]
        [HttpGet]
        [TokenAuthorizeFilter]
        [ProducesResponseType(typeof(ApiResponseModel<Data.Entity.Broadcast>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel<object>), (int)HttpStatusCode.NotFound)]
        public IActionResult GetById(int id, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Broadcast>();
            responseModel.DisplayLanguage = displayLanguage;

            if (id <= 0)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Id Is Required";
                return BadRequest(responseModel);
            }
            var fileHostBaseUrl = GetFileHostBaseUrlFromCurrentRequest(); // şirket resimlerinin tam url adres bilgisi icin kullanilacak
            try
            {
                responseModel.Data = _broadcastService.GetById(id);
                if (responseModel.Data == null)
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "NoRecordsFound";
                    return NotFound(responseModel);
                }
                if (responseModel.Data.ImageFilePath != null)
                {
                    responseModel.Data.ImageFilePath = fileHostBaseUrl + "" + responseModel.Data.ImageFilePath;
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                responseModel.ResultStatusMessage = "Success";
                return Ok(responseModel);

            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("upload-file")]
        [HttpPost]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_ADD, AuthCodeStatic.PAGE_ANNOUNCEMENT_ADD)]
        public ApiResponseModel<string> AddBroadcastFiles([FromForm]List<IFormFile> imageFile, [FromHeader]string displayLanguage)
        {
            string imageFilePath = null;
            var responseModel = new ApiResponseModel<string>() { DisplayLanguage = displayLanguage };

            //sonraki işlemlerde bu kontrol kaldırılabilir.
            if (imageFile != null && imageFile.Count > 1)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "sadece 1 resim ekleyebilirsiniz.";
                return responseModel;
            }

            //resim eklenmiş ise; 
            if (imageFile != null && imageFile.Count > 0)
            {

                long size = imageFile.Sum(f => f.Length);
                try
                {
                    foreach (var item in imageFile)
                    {
                        List<string> allowedExtensions = ConfigHelper.ImageExtension;
                        string fileExtension = Path.GetExtension(item.FileName).ToLower();
                        bool isAllowed = allowedExtensions.Contains(fileExtension);
                        if (isAllowed)
                        {
                            if (item.Length > 0)
                            {
                                // full path to file in temp location
                                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "BroadcastFiles");

                                //mac cihazlarda -> image isminde boşluk oldugunda url olarak algılamamaktadır. bu yuzden replace ediyoruz.
                                var newImageName = Guid.NewGuid().ToString() + item.FileName.Replace(" ", "");
                                var fileNameWithPath = string.Concat(filePath, "\\", newImageName);
                                imageFilePath = newImageName;
                                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                            }
                            else
                            {
                                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                                responseModel.ResultStatusMessage = "Picture Format Incompatible";
                                return responseModel;
                            }
                        }
                        else
                        {
                            responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                            responseModel.ResultStatusMessage = "Picture Format Incompatible";
                            return responseModel;
                        }

                    }
                }
                catch
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Images Could Not Be Saved";
                    return responseModel;
                }

            }
            else
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Images Could Not Be Saved";
                return responseModel;
            }
            responseModel.Data = imageFilePath;
            responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
            responseModel.ResultStatusMessage = "Success";
            return responseModel;
        }

        [Route("")]
        [HttpPost]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_ADD, AuthCodeStatic.PAGE_ANNOUNCEMENT_ADD)]
        public ApiResponseModel<Data.Entity.Broadcast> AddBroadcast([FromBody]AddRequestModel requestModel, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Broadcast>() {DisplayLanguage=displayLanguage };
            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.ID;

            try
            {
                var record = new Data.Entity.Broadcast();
                record.TitleTR = requestModel.TitleTR;
                record.TitleEN = requestModel.TitleEN;
                record.DescriptionTR = requestModel.DescriptionTR;
                record.DescriptionEN = requestModel.DescriptionEN;
                if (!String.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    record.ImageFilePath = "BroadcastFiles/" + requestModel.ImageFilePath;
                }
                record.VideoFileUrl = requestModel.VideoFileUrl;
                record.ValidationEndDateTime = requestModel.ValidationEndDateTime;
                record.IsActive = false; // default olarak false yapılır.
                record.CreatedDateTime = DateTime.Now;
                record.CreatedBy = employeeId; 
                record.BroadcastTypeId = requestModel.BroadcastTypeId;

                var dbResult = _broadcastService.Add(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor

                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                }
                else
                {
                    //hata oluşursa varsa  resmi silmemiz gerekecek
                    if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "BroadcastFiles") + "\\" + requestModel.ImageFilePath;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                //hata oluşursa varsa  resmi silmemiz gerekecek
                if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "BroadcastFiles") + "\\" + requestModel.ImageFilePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }
            return responseModel;
        }

        [Route("{id:int}")]
        [HttpPut]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_EDIT, AuthCodeStatic.PAGE_ANNOUNCEMENT_EDIT)]
        public ApiResponseModel<Data.Entity.Broadcast> EditBroadcastWithDetail(int id, [FromBody]AddRequestModel requestModel, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Broadcast>() {  DisplayLanguage=displayLanguage};
            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.ID;
            try
            {
                var broadcast = _broadcastService.GetById(id);
                if (broadcast == null)
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "No Records Found";
                    return responseModel;
                }

                var record = new Data.Entity.Broadcast();
                broadcast.Id = requestModel.Id;
                broadcast.TitleTR = requestModel.TitleTR;
                broadcast.TitleEN = requestModel.TitleEN;
                broadcast.DescriptionTR = requestModel.DescriptionTR;
                broadcast.DescriptionEN = requestModel.DescriptionEN;
                if (!String.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    broadcast.ImageFilePath = "BroadcastFiles/" + requestModel.ImageFilePath;
                }
                else
                {
                    broadcast.ImageFilePath = broadcast.ImageFilePath;
                }
                broadcast.VideoFileUrl = requestModel.VideoFileUrl;
                broadcast.ValidationEndDateTime = requestModel.ValidationEndDateTime;
                broadcast.ModifiedDateTime = DateTime.Now;
                broadcast.ModifiedBy = employeeId;

                var dbResult = _broadcastService.Update(broadcast);
                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                }
                else
                {
                    //hata oluşursa varsa  resmi silmemiz gerekecek
                    if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                    {
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "BroadcastFiles") + "\\" + requestModel.ImageFilePath;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                }
            }
            catch (Exception ex)
            {
                //hata oluşursa varsa  resmi silmemiz gerekecek
                if (!string.IsNullOrEmpty(requestModel.ImageFilePath))
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "BroadcastFiles") + "\\" + requestModel.ImageFilePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }
            return responseModel;
        }

        [Route("{id:int}/status")]
        [HttpPut]
        //[TokenAuthorizeFilter(AuthCodeStatic.PAGE_NEWS_ACTIVEPASSIVE, AuthCodeStatic.PAGE_ANNOUNCEMENT_ACTIVEPASSIVE)]
        public IActionResult UpdateStatus(int id, UpdateStatusRequestModel status, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Data.Entity.Broadcast> { DisplayLanguage = displayLanguage };
            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.ID;
            try
            {
                var record = _broadcastService.GetById(id);
                // If record is not exist...
                if (record == null)
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "No Records Found";
                    return NotFound(responseModel);
                }

                // Set record status info
                record.IsActive = status.IsActive;
                record.ModifiedBy = employeeId; //todo: token bilgisinden alınacak
                record.ModifiedDateTime = DateTime.Now;

                // Update record
                var dbResult = _broadcastService.Update(record);

                // Record updating is success...
                if (dbResult > 0)
                {
                    // Return success model
                    responseModel.Data = record;
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }

                // Otherwise Return internal server error model
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = "Could Not Be Saved";
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
            }

            return StatusCode(500, responseModel);
        }


        [NonAction]
        private string GetFileHostBaseUrlFromCurrentRequest()
        {
            var result = "";

            result = string.Format("{0}://{1}/", Request.Scheme.ToString(), Request.Host.ToString());

            return result;
        }


    }
}