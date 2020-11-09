using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using IPAddressProccessor.API.Data;
using IPAddressProccessor.API.Services.Abstract;
using IPAddressProccessor.API.Services.Concrete;
using IPAddressProccessor.API.Services.HostedServices;
using IPAddressProccessor.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace IPAddressProccessor.API
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IPAddressesContext>(cfg =>
            {
                cfg.UseSqlServer(config.GetConnectionString("IPAddressProcessor"));
            });

            services.AddMemoryCache();

            services.AddScoped<IPAddressProcessorSeeder>();
            services.AddScoped<IIPAddressesRepository, IPAddressesRepository>();

            services.AddScoped<ICacheProvider, CacheProvider>();
            services.AddScoped<IIPInfoProvider, IPStackIPInfoProvider>();
            services.AddScoped<ExternalAPIIPDetailsService>();
            services.AddScoped<DatabaseIPDetailsService>();
            services.AddScoped<IIPDetailsService, CachedIPDetailsService>();
            services.AddScoped<IBatchUpdateJobsService, BatchUpdateJobsService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHostedService<BatchUpdateJobWorker>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "IP Address Processor API", Version = "v1" }
                ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "IP Address Processor API v1"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapControllers();
            });
        }
    }
}
