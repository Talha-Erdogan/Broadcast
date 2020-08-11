using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broadcast.Web.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Broadcast.Web.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IBroadcastService _broadcastService;
        private readonly IBroadcastTypeService _broadcastTypeService;

        public AnnouncementController(IBroadcastService broadcastService, IBroadcastTypeService broadcastTypeService)
        {
            _broadcastService = broadcastService;
            _broadcastTypeService = broadcastTypeService;
        }

       
    }
}