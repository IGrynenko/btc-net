using BTC.Services;
using BTC.Services.Helpers;
using BTC.Services.Interfaces;
using BTC.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IDataWorker<User>, DataWorker<User>>(_ 
                => new DataWorker<User>(Configuration.GetSection("MainFolder").Value + "\\" + Configuration.GetSection("DataSource:Users").Value));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDataHostService, DataHostService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataHostService dataHostService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dataHostService.StartUp();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
