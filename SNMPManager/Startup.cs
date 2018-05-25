using System;
using AutoMapper;
using BuildingBlocks.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SNMPManager.Controllers;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Sonars.Repositories;
using SNMPManager.Models;

namespace SNMPManager
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
            services.AddMvc();
            const string connection = @"Server=localhost;Database=SnmpManager;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<SnmpManagerContext>(options => options.UseSqlServer(connection));

            services.AddTransient<SonarsRepository>();
            services.AddAutoMapper(cfg =>
                {
                    cfg.CreateMap<Sonar, SonarDto>();
                    cfg.CreateMap<Guid, string>().ConvertUsing(g => g.ToString("N"));
                    cfg.CreateMap<CreateSonarDto, Sonar>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
