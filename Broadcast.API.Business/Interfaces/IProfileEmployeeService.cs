using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.ProfileEmployee;
using Broadcast.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
    public interface IProfileEmployeeService
    {
        PaginatedList<Employee> GetAllEmployeePaginatedWithDetailBySearchFilter(ProfileEmployeeSearchFilter searchFilter);
        PaginatedList<Employee> GetAllEmployeeWhichIsNotIncludedPaginatedWithDetailBySearchFilter(ProfileEmployeeSearchFilter searchFilter);
        List<Profile> GetAllProfileByCurrentUser(int employeeId);
        List<Profile> GetAllProfileByEmployeeIdAndAuthCode(int employeeId, string authCode);
        int Add(ProfileEmployee record);
        int DeleteByProfileIdAndEmployeeId(int profileId, int employeeId);

    }
}
