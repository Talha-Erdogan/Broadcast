using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
    public interface IEmployeeService
    {
        PaginatedList<EmployeeWithDetail> GetAllPaginatedWithDetailBySearchFilter(EmployeeSearchFilter searchFilter);
        EmployeeWithDetail GetByIdWithDetail(int id);


    }
}
