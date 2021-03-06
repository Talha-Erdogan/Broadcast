using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Broadcast.API.Business;
using Broadcast.API.Business.Interfaces;
using Broadcast.API.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Broadcast.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // business service ve interface DI container tanimlari
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBroadcastService, BroadcastService>();
            services.AddTransient<IBroadcastTypeService, BroadcastTypeService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IProfileDetailService, ProfileDetailService>();
            services.AddTransient<IProfileEmployeeService, ProfileEmployeeService>();
            services.AddTransient<ISexService, SexService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddSingleton<IConfiguration>(Configuration); //add Configuration to our services collection
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Adding static file middleware
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //config helper'� configure etmek i�in
            Common.ConfigHelper.Configure(Configuration);

            // token helper
            TokenHelper.Configure(app.ApplicationServices);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
