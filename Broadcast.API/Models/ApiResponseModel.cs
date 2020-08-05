using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Models
{
    public class ApiResponseModel<TData> : BaseResponseModel
    {
        public TData Data { get; set; }
    }
}
