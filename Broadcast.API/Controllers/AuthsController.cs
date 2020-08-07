using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broadcast.API.Business.Interfaces;
using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Auth;
using Broadcast.API.Common;
using Broadcast.API.Common.Enums;
using Broadcast.API.Common.Model;
using Broadcast.API.Data.Entity;
using Broadcast.API.Filters;
using Broadcast.API.Models;
using Broadcast.API.Models.Auths;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Broadcast.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("")]
        [HttpGet]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_AUTH_LIST)]
        public IActionResult GetAllPaginatedWithDetail([FromQuery]GetAllPaginatedRequestModel requestModel, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<PaginatedList<Auth>>();
            responseModel.DisplayLanguage = displayLanguage;

            try
            {
                var searchFilter = new AuthSearchFilter();
                searchFilter.CurrentPage = requestModel.CurrentPage.HasValue ? requestModel.CurrentPage.Value : 1;
                searchFilter.PageSize = requestModel.PageSize.HasValue ? requestModel.PageSize.Value : 10;
                searchFilter.SortOn = requestModel.SortOn;
                searchFilter.SortDirection = requestModel.SortDirection;
                searchFilter.Filter_Code = requestModel.Code;
                searchFilter.Filter_NameTR = requestModel.NameTR;
                searchFilter.Filter_NameEN = requestModel.NameEN;
                responseModel.Data = _authService.GetAllPaginatedWithDetailBySearchFilter(searchFilter);

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

        [Route("{Id}")]
        [HttpGet]
        [TokenAuthorizeFilter]
        public IActionResult GetById(int id, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Auth>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                responseModel.Data = _authService.GetById(id);
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


        [HttpPost]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_AUTH_ADD)]
        public IActionResult Add([FromBody]AddRequestModel requestModel, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Auth>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = new Auth();
                record.Code = requestModel.Code;
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                record.IsDeleted = false;

                var dbResult = _authService.Add(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // oluşturulan entity bilgisinde id kolonu atanmış olur ve entity geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{Id}")]
        [HttpPut]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_AUTH_EDIT)]
        public IActionResult Edit(int id, [FromBody]AddRequestModel requestModel, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Auth>();
            responseModel.DisplayLanguage = displayLanguage;
            try
            {
                var record = _authService.GetById(id);
                record.Code = requestModel.Code;
                record.NameEN = requestModel.NameEN;
                record.NameTR = requestModel.NameTR;
                var dbResult = _authService.Update(record);
                if (dbResult > 0)
                {
                    responseModel.Data = record; 
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }

        [Route("{Id}")]
        [HttpDelete]
        [TokenAuthorizeFilter(AuthCodeStatic.PAGE_AUTH_DELETE)]
        public IActionResult Delete(int id, [FromHeader]string displayLanguage)
        {
            var responseModel = new ApiResponseModel<Auth>();
            responseModel.DisplayLanguage = displayLanguage;

            //user bilgilerinden filitre parametreleri eklenir.
            TokenModel tokenModel = TokenHelper.DecodeTokenFromRequestHeader(Request);
            var employeeId = tokenModel.ID; 
            
            try
            {
                var record = _authService.GetById(id);
                record.IsDeleted = true;
                record.DeletedDateTime = DateTime.Now;
                record.DeletedBy = employeeId;
                var dbResult = _authService.Update(record);

                if (dbResult > 0)
                {
                    responseModel.Data = record; // 'isDeleted= true' yapılan -> entity bilgisi geri gönderiliyor
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Success;
                    responseModel.ResultStatusMessage = "Success";
                    return Ok(responseModel);
                }
                else
                {
                    responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                    responseModel.ResultStatusMessage = "Could Not Be Saved";
                    responseModel.Data = null;
                    return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
                }
            }
            catch (Exception ex)
            {
                responseModel.ResultStatusCode = ResultStatusCodeStatic.Error;
                responseModel.ResultStatusMessage = ex.Message;
                responseModel.Data = null;
                return StatusCode(StatusCodes.Status500InternalServerError, responseModel);
            }
        }


    }
}