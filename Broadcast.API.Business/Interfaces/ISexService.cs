using Broadcast.API.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Broadcast.API.Business.Interfaces
{
    public interface ISexService
    {
        List<Sex> GetAll();
        Sex GetById(int id);
    }
}
