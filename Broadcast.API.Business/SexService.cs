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
   public  class SexService : ISexService
    {
        private IConfiguration _config;

        public SexService(IConfiguration config)
        {
            _config = config;
        }


        public List<Sex> GetAll()
        {
            List<Sex> resultList = new List<Sex>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                resultList.AddRange(dbContext.Sex.AsNoTracking().ToList());
            }
            return resultList;
        }

        public Sex GetById(int id)
        {
            Sex result = new Sex();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.Sex.Where(r => r.Id == id).AsNoTracking().SingleOrDefault();
            }
            return result;
        }

    }
}
