using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models
{
    public class ApiResponseModel<TData> : BaseResponseModel
    {
        public TData Data { get; set; }

    }
}
