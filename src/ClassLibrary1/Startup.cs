using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ClassLibrary1
{
    public class Startup
    {
        public IConfigurationRoot Config { get; }

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.Config = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional:true, reloadOnChange:true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables(prefix: "XEMIO")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment environment, ILoggerFactory loggerFactory)
        {
            app.UseCors(f => f.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build());

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = ,
                }
            });

            app.UseMvc();
        }
    }
}
