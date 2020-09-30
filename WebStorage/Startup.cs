using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStorage.Models;
using WebStorage.Services;
using Hangfire;
using Hangfire.SqlServer;
using System;
using WebStorage.Services.Interfaces;

namespace WebStorage
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 500000000; // 500mb 
            });

            services.AddControllersWithViews();
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.ConfigureIdentity(Configuration);
            services.ConfigureCustomServices();
            services.ConfigureHangfire(Configuration);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<DeleteFilesService>(x => x.CheckToDeleteFolder(), Cron.Minutely);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
