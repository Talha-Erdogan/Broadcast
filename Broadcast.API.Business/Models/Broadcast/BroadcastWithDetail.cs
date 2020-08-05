using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Models.Broadcast
{
   public class BroadcastWithDetail : Data.Entity.Broadcast
    {
        //broadcast type columns
        public string BroadcastType_NameTR { get; set; }
        public string BroadcastType_NameEN { get; set; }
    }
}
