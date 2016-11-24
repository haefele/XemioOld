using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xemio.Server.Infrastructure.Database;

namespace Xemio.Hosts.AspNetCore.Setup
{

    public static class Database
    {
        public static void AddDatabase(this IServiceCollection self, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionString");
            self.AddDbContext<XemioContext>(options => options.UseSqlServer(connectionString));
        }

        public static void MigrateDatabase(this IApplicationBuilder self)
        {
            var context = self.ApplicationServices.GetService<XemioContext>();
            context.Database.Migrate();
        }
    }
}
