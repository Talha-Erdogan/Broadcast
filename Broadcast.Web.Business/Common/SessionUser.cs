using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.Web.Business.Common
{
    // Asp.net Session StateServer kullanimi icin Serializable attribute eklenir
    [Serializable]
    public class SessionUser
    {
        public string ID { get; set; }
        public string TRNationalId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string SexId { get; set; }

        public string UserToken { get; set; }


        public List<Auth> AuthList { get; set; }
        public List<Profile> ProfileList { get; set; }
    }
}
