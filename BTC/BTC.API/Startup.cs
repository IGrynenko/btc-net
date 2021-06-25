using AutoMapper;
using BTC.API.Helpers;
using BTC.API.Interfaces;
using BTC.API.Services;
using BTC.Services;
using BTC.Services.Helpers;
using BTC.Services.Interfaces;
using BTC.Services.Mappers;
using BTC.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.API
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
            services.Configure<JwtTokenSettings>(Configuration.GetSection("Authentication"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                    opt =>
                    {
                        opt.RequireHttpsMetadata = false;
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Authentication:Key").Value)),
                            ValidateLifetime = true,
                            ValidateAudience = true,
                            ValidAudience = Configuration.GetSection("Authentication:Audiance").Value,
                            ValidateIssuer = true,
                            ValidIssuer = Configuration.GetSection("Authentication:Issuer").Value
                        };
                    });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<MappingProfile>();
            });

            services.AddControllers();

            services.AddSingleton(mappingConfig.CreateMapper());
            services.AddSingleton<IDataWorker<User>, DataWorker<User>>(_ 
                => new DataWorker<User>(Configuration.GetSection("MainFolder").Value + "\\" + Configuration.GetSection("DataSource:Users").Value));

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDataHostService, DataHostService>();

            services.AddScoped<ITokenService, TokenService>();
        }

        // ADD exeptions middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataHostService dataHostService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dataHostService.StartUp();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
