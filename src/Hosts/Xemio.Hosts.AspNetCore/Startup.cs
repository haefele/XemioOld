using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag.AspNetCore;
using NSwag.CodeGeneration.SwaggerGenerators.WebApi.Processors.Security;
using System.Reflection;
using Xemio.Hosts.AspNetCore.Setup;
using Xemio.Server.Infrastructure.Controllers.Notes;
using Xemio.Server.Infrastructure.Filters;

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

            services.AddMvc(f =>
            {
                f.Filters.Add(typeof(ConcurrencyExceptionFilterAttribute));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.MigrateDatabase();

            app.UseLogging(this.Configuration.GetSection("Logging"));
            
            app.UseCors(f => f.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuth0Authentication(this.Configuration.GetSection("Auth0"));
            app.UseSwaggerUi(typeof(FoldersController).GetTypeInfo().Assembly, new SwaggerUiOwinSettings
            {
                Title = "Xemio HTTP API",
                Description = "All available HTTP endpoints for the xemio API.",
                DefaultPropertyNameHandling = PropertyNameHandling.CamelCase,
                SwaggerRoute = "/swagger/swagger.json",
                SwaggerUiRoute = "/swagger",
                DocumentProcessors =
                {
                    new SecurityDefinitionAppender("Authorization", new NSwag.SwaggerSecurityScheme
                    {
                        Type = NSwag.SwaggerSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = NSwag.SwaggerSecurityApiKeyLocation.Header,
                        Description = "Use the id_token from Auth0 as a bearer token."
                    }),
                }
            });
            app.UseMvc();
        }
    }
}
