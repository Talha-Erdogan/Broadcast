﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Employee
{
    public class AddRequestModel
    {
        public int ID { get; set; }

        public string TRNationalId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public int SexId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
