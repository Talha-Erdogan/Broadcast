﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Models.Auths
{
    public class AddRequestModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameTR { get; set; }
        public string NameEN { get; set; }
    }
}
