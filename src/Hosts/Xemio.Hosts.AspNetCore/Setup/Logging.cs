using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Xemio.Hosts.AspNetCore.Setup
{

    public static class Logging
    {
        public static void UseLogging(this IApplicationBuilder self, IConfiguration configuration)
        {
            var loggerFactory = self.ApplicationServices.GetService<ILoggerFactory>();

            loggerFactory.AddConsole(configuration);
            loggerFactory.AddDebug();
        }
    }
}
