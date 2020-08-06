using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.ProfileDetail
{
    public class DeleteByProfileIdAndAuthIdRequestModel
    {
        public int ProfileId { get; set; }
        public int AuthId { get; set; }
    }
}
