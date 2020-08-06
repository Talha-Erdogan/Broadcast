﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Broadcast.API.Common
{
    public static class ConfigHelper
    {
        private static IConfiguration _configuration { get; set; }

        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static List<string> ImageExtension
        {
            get
            {
                return _configuration.GetValue<string>("ImageExtension").Split(",").ToList();
            }
        }



    }
}