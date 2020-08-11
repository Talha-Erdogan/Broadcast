using Broadcast.API.Business.Interfaces;
using Broadcast.API.Business.Models;
using Broadcast.API.Business.Models.Broadcast;
using Broadcast.API.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Broadcast.API.Business
{
    public class BroadcastService : IBroadcastService
    {
        private IConfiguration _config;

        public BroadcastService(IConfiguration config)
        {
            _config = config;
        }

        public PaginatedList<BroadcastWithDetail> GetAllPaginatedWithDetailBySearchFilter(BroadcastSearchFilter searchFilter)
        {
            PaginatedList<BroadcastWithDetail> resultList = new PaginatedList<BroadcastWithDetail>(new List<BroadcastWithDetail>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {

                var query = from b in dbContext.Broadcast
                            from bt in dbContext.BroadcastType.Where(x => x.Id == b.BroadcastTypeId).DefaultIfEmpty()
                            select new BroadcastWithDetail()
                            {
                                Id = b.Id,
                                TitleTR = b.TitleTR,
                                TitleEN = b.TitleEN,
                                DescriptionTR = b.DescriptionTR,
                                DescriptionEN = b.DescriptionEN,
                                ImageFilePath = b.ImageFilePath,
                                VideoFileUrl = b.VideoFileUrl,
                                ValidationEndDateTime = b.ValidationEndDateTime,
                                IsActive = b.IsActive,
                                CreatedBy = b.CreatedBy,
                                CreatedDateTime = b.CreatedDateTime,
                                ModifiedBy = b.ModifiedBy,
                                ModifiedDateTime = b.ModifiedDateTime,
                                BroadcastTypeId = b.BroadcastTypeId,
                                BroadcastType_NameTR = bt == null ? String.Empty : bt.NameTR,
                                BroadcastType_NameEN = bt == null ? String.Empty : bt.NameEN,
                            };
                // filtering
                if (searchFilter.Filter_BroadcastTypeId.HasValue)
                {
                    query = query.Where(r => r.BroadcastTypeId == searchFilter.Filter_BroadcastTypeId.Value);
                }
                if (!string.IsNullOrEmpty(searchFilter.Filter_TitleTR))
                {
                    query = query.Where(r => r.TitleTR.Contains(searchFilter.Filter_TitleTR));
                }
                if (!string.IsNullOrEmpty(searchFilter.Filter_TitleEN))
                {
                    query = query.Where(r => r.TitleEN.Contains(searchFilter.Filter_TitleEN));
                }
                if (searchFilter.Filter_IsActive.HasValue)
                {
                    bool _isActive = searchFilter.Filter_IsActive.Value == 1 ? true : false;
                    query = query.Where(r => r.IsActive == _isActive);
                }
                if (searchFilter.Filter_ValidationEndDateTime.HasValue)
                {
                    query = query.Where(r => r.ValidationEndDateTime.Date == searchFilter.Filter_ValidationEndDateTime.Value.Date);
                }

                // asnotracking
                query = query.AsNoTracking();
                //total count
                var totalCount = query.Count();
                //sorting
                if (!string.IsNullOrEmpty(searchFilter.SortOn))
                {
                    // using System.Linq.Dynamic.Core; nuget paketi ve namespace eklenmelidir, dynamic order by yapmak icindir
                    query = query.OrderBy(searchFilter.SortOn + " " + searchFilter.SortDirection.ToUpper());
                }
                else
                {
                    // deefault sıralama vermek gerekiyor yoksa skip metodu hata veriyor ef 6'da -- 28.10.2019 15:40
                    // https://stackoverflow.com/questions/3437178/the-method-skip-is-only-supported-for-sorted-input-in-linq-to-entities
                    //query = query.OrderBy(r => r.BroadcastStatusId);
                    query = query.OrderByDescending(r => r.Id);
                }

                //paging
                query = query.Skip((searchFilter.CurrentPage - 1) * searchFilter.PageSize).Take(searchFilter.PageSize);

                resultList = new PaginatedList<BroadcastWithDetail>(
                    query.ToList(),
                    totalCount,
                    searchFilter.CurrentPage,
                    searchFilter.PageSize,
                    searchFilter.SortOn,
                    searchFilter.SortDirection
                    );
            }

            return resultList;
        }

        public PaginatedList<BroadcastWithDetail> GetAllPaginatedWhichIsActiveWithDetailBySearchFilter(BroadcastSearchFilter searchFilter)
        {
            PaginatedList<BroadcastWithDetail> resultList = new PaginatedList<BroadcastWithDetail>(new List<BroadcastWithDetail>(), 0, searchFilter.CurrentPage, searchFilter.PageSize, searchFilter.SortOn, searchFilter.SortDirection);

            using (AppDBContext dbContext = new AppDBContext(_config))
            {

                var query = from b in dbContext.Broadcast
                            from bt in dbContext.BroadcastType.Where(x => x.Id == b.BroadcastTypeId).DefaultIfEmpty()
                            where b.IsActive == true
                            select new BroadcastWithDetail()
                            {
                                Id = b.Id,
                                TitleTR = b.TitleTR,
                                TitleEN = b.TitleEN,
                                DescriptionTR = b.DescriptionTR,
                                DescriptionEN = b.DescriptionEN,
                                ImageFilePath = b.ImageFilePath,
                                VideoFileUrl = b.VideoFileUrl,
                                ValidationEndDateTime = b.ValidationEndDateTime,
                                IsActive = b.IsActive,
                                CreatedBy = b.CreatedBy,
                                CreatedDateTime = b.CreatedDateTime,
                                ModifiedBy = b.ModifiedBy,
                                ModifiedDateTime = b.ModifiedDateTime,
                                BroadcastTypeId = b.BroadcastTypeId,
                                BroadcastType_NameTR = bt == null ? String.Empty : bt.NameTR,
                                BroadcastType_NameEN = bt == null ? String.Empty : bt.NameEN,
                            };
                // filtering
                if (searchFilter.Filter_BroadcastTypeId.HasValue)
                {
                    query = query.Where(r => r.BroadcastTypeId == searchFilter.Filter_BroadcastTypeId.Value);
                }
                if (!string.IsNullOrEmpty(searchFilter.Filter_TitleTR))
                {
                    query = query.Where(r => r.TitleTR.Contains(searchFilter.Filter_TitleTR));
                }
                if (!string.IsNullOrEmpty(searchFilter.Filter_TitleEN))
                {
                    query = query.Where(r => r.TitleEN.Contains(searchFilter.Filter_TitleEN));
                }
                if (searchFilter.Filter_IsActive.HasValue)
                {
                    bool _isActive = searchFilter.Filter_IsActive.Value == 1 ? true : false;
                    query = query.Where(r => r.IsActive == _isActive);
                }
                if (searchFilter.Filter_ValidationEndDateTime.HasValue)
                {
                    query = query.Where(r => r.ValidationEndDateTime.Date == searchFilter.Filter_ValidationEndDateTime.Value.Date);
                }

                // asnotracking
                query = query.AsNoTracking();
                //total count
                var totalCount = query.Count();
                //sorting
                if (!string.IsNullOrEmpty(searchFilter.SortOn))
                {
                    // using System.Linq.Dynamic.Core; nuget paketi ve namespace eklenmelidir, dynamic order by yapmak icindir
                    query = query.OrderBy(searchFilter.SortOn + " " + searchFilter.SortDirection.ToUpper());
                }
                else
                {
                    // deefault sıralama vermek gerekiyor yoksa skip metodu hata veriyor ef 6'da -- 28.10.2019 15:40
                    // https://stackoverflow.com/questions/3437178/the-method-skip-is-only-supported-for-sorted-input-in-linq-to-entities
                    //query = query.OrderBy(r => r.BroadcastStatusId);
                    query = query.OrderByDescending(r => r.Id);
                }

                //paging
                query = query.Skip((searchFilter.CurrentPage - 1) * searchFilter.PageSize).Take(searchFilter.PageSize);

                resultList = new PaginatedList<BroadcastWithDetail>(
                    query.ToList(),
                    totalCount,
                    searchFilter.CurrentPage,
                    searchFilter.PageSize,
                    searchFilter.SortOn,
                    searchFilter.SortDirection
                    );
            }

            return resultList;
        }

        public List<Data.Entity.Broadcast> GetAllWhichIsActive()
        {
            List<Data.Entity.Broadcast> resultList = new List<Data.Entity.Broadcast>();
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                resultList.AddRange(dbContext.Broadcast.Where(x => x.IsActive == true).AsNoTracking().ToList());
            }
            return resultList;
        }

        public Data.Entity.Broadcast GetById(int id)
        {
            Data.Entity.Broadcast result = new Data.Entity.Broadcast();

            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                result = dbContext.Broadcast.Where(r => r.Id == id).AsNoTracking().SingleOrDefault();
            }

            return result;
        }

        public int Add(Data.Entity.Broadcast record)
        {
            int result = 0;
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                dbContext.Entry(record).State = EntityState.Added;
                result = dbContext.SaveChanges();
            }
            return result;
        }

        public int Update(Data.Entity.Broadcast record)
        {
            int result = 0;
            using (AppDBContext dbContext = new AppDBContext(_config))
            {
                dbContext.Entry(record).State = EntityState.Modified;
                result = dbContext.SaveChanges();
            }
            return result;
        }
    }
}
