using Broadcast.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
   public  interface IBroadcastTypeService
    {
        List<BroadcastType> GetAll();
        BroadcastType GetById(int id);
    }
}
