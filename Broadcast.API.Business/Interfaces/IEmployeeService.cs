using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Employee;
using Broadcast.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
    public interface IEmployeeService
    {
        PaginatedList<EmployeeWithDetail> GetAllPaginatedWithDetailBySearchFilter(EmployeeSearchFilter searchFilter);
        Employee GetById(int id);
        EmployeeWithDetail GetByIdWithDetail(int id);
        int Add(Data.Entity.Employee record);
        int Update(Data.Entity.Employee record);
    }
}
