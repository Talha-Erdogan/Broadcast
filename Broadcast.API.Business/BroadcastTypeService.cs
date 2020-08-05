using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Broadcast.API.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Broadcast.API.Data.Entity;
using Broadcast.API.Data;

namespace Broadcast.API.Business
{
    public class BroadcastTypeService : IBroadcastTypeService
    {
        private IConfiguration _config;

        public BroadcastTypeService(IConfiguration config)
        {
            _config = config;
        }

        public List<BroadcastType> GetAll()
        {
            List<BroadcastType> resultList = new List<BroadcastType>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                resultList.AddRange(dbContext.BroadcastType.AsNoTracking().ToList());
            }
            return resultList;
        }

        public BroadcastType GetById(int id)
        {
            BroadcastType result = new BroadcastType();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.BroadcastType.Where(r => r.Id == id).AsNoTracking().SingleOrDefault();
            }
            return result;
        }



    }
}
