﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Models.ProfileDetails
{
    public class DeleteByProfileIdAndAuthIdRequestModel
    {
        public int ProfileId { get; set; }
        public int AuthId { get; set; }
    }
}
