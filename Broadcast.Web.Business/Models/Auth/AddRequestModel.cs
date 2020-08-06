using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Models.Auth
{
    public class AddRequestModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameTR { get; set; }
        public string NameEN { get; set; }
    }
}
