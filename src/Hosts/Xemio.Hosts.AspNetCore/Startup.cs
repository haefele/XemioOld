﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation.AspNetCore;
using Xemio.Hosts.AspNetCore.Setup;
using Xemio.Server.Infrastructure.Controllers.Notes;
using Xemio.Server.Infrastructure.Filters;
using Xemio.Server.Infrastructure.Validators;

namespace Xemio.Hosts.AspNetCore
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            this.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase(this.Configuration.GetSection("Database"));

            services.AddMappers();
            services.AddCors();
            services.AddLogging();

            services
                .AddMvc(f =>
                    {
                        f.Filters.Add(typeof(RequiredParameterFilterAttribute));
                        f.Filters.Add(typeof(ValidModelStateFilterAttribute));
                    })
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<CreateFolderValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.MigrateDatabase();

            app.UseLogging(this.Configuration.GetSection("Logging"));
            
            app.UseCors(f => f.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuth0Authentication(this.Configuration.GetSection("Auth0"));
            app.UseMvc();
        }
    }
}
