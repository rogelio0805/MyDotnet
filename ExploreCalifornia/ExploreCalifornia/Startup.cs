using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExploreCalifornia
{
    public class Startup
    {

        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config.develop.json", optional: true, reloadOnChange: true)
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<SpecialDataContext>(); Static Content 
            services.AddTransient<FormattingService>();

            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExceptions =
                    configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")
            });

            services.AddDbContext<SpecialDataContext>(option =>
            {
                var connectionString = configuration.GetConnectionString("SpecialDataContext");
                option.UseSqlServer(connectionString);
            });

            services.AddDbContext<BlogDataContext>( option => 
            {
                var connectionString = configuration.GetConnectionString("BlogDataContext");
                option.UseSqlServer(connectionString);
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseExceptionHandler("/error.html");

            var configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddJsonFile(env.ContentRootPath + "/config.json")
                                    .AddJsonFile(env.ContentRootPath + "/config.develop.json", true)
                                    .Build();

            if (configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions"))
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                {
                    throw new Exception("ERROR");
                }
                await next();
            });

            app.UseMvc( routes => 
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();
        }
    }
}
