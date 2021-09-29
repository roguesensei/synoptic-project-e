using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SynopticProject_Project_E.DAL;
using SynopticProject_Project_E.Helpers;
using SynopticProject_Project_E.Models;
using System;
using System.Linq;

namespace SynopticProject_Project_E
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
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Verify at least one admin exists
            if (!UserDAL.GetUsers().Any(x => x.IsAdmin))
            {
                // Create default super user
                UserUploadModel defaultSuperUser = ConfigurationHelper.GetAppSettings()?.DefaultSuperUser;
                if (defaultSuperUser == null)
                {
                    throw new Exception("Could not find a DefaultSuperUser in the appsettins.json");
                }

                UserDAL.CreateUser(defaultSuperUser, true);
            }
        }
    }
}
