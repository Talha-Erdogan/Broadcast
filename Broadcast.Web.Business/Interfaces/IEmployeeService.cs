﻿using Broadcast.Web.Business.Models;
using Broadcast.Web.Business.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Interfaces
{
    public interface IEmployeeService
    {
        ApiResponseModel<PaginatedList<EmployeeWithDetail>> GetAllPaginatedWithDetailBySearchFilter(string userToken, string displayLanguage, EmployeeSearchFilter searchFilter);

        ApiResponseModel<EmployeeWithDetail> GetById(string userToken, string displayLanguage, int id);
        ApiResponseModel<Employee> Add(string userToken, string displayLanguage, Employee employee);
        ApiResponseModel<Employee> Edit(string userToken, string displayLanguage, Employee employee);

    }
}
