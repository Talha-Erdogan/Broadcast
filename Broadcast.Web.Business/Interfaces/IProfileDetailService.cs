﻿using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.ProfileDetail;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Interfaces
{
    public interface IProfileDetailService
    {
        ApiResponseModel<List<Business.Models.Auth.Auth>> GetAllAuthByCurrentUser(string userToken, string displayLanguage, string employeeId);
        ApiResponseModel<List<Business.Models.Auth.Auth>> GetAllAuthByProfileId(string userToken, string displayLanguage, int profileId);
        ApiResponseModel<List<Business.Models.Auth.Auth>> GetAllAuthByProfileIdWhichIsNotIncluded(string userToken, string displayLanguage, int profileId);
        ApiResponseModel<ProfileDetail> Add(string userToken, string displayLanguage, ProfileDetail profileDetail);
        ApiResponseModel<int> DeleteByProfileIdAndAuthId(string userToken, string displayLanguage, int profileId, int authId);
    }
}