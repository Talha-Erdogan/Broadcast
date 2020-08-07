using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broadcast.API.Business.Interfaces;
using Broadcast.API.Common.Enums;
using Broadcast.API.Filters;
using Broadcast.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Broadcast.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BroadcastTypesController : ControllerBase
    {
        private readonly IBroadcastTypeService _broadcastTypeService;
        public BroadcastTypesController(IBroadcastTypeService broadcastTypeService)
        {
            _broadcastTypeService = broadcastTypeService;
        }

        [HttpGet]
        [TokenAuthorizeFilter] // sadece oturum acilma kontrolu yapilir
        public IActionResult GetBroadcastTypes([FromHeader]string displayLanguage)
        {
            ApiResponseModel<List<Data.Entity.BroadcastType>> responseModel = new ApiResponseModel<List<Data.Entity.BroadcastType>>() { DisplayLanguage =displayLanguage};
            try
            {
                var broadcastTypes = _broadcastTypeService.GetAll();
                responseModel.Data = broadcastTypes;
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


    }
}