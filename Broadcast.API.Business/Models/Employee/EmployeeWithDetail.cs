﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Models.Employee
{
    public class EmployeeWithDetail : Data.Entity.Employee
    {
        //sex columns
        public string Sex_NameTR { get; set; }
        public string Sex_NameEN { get; set; }
    }
}
