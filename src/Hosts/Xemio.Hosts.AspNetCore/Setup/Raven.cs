using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client;
using Raven.Client.Document;

namespace Xemio.Hosts.AspNetCore.Setup
{
    public static class Raven
    {
        public static void AddRaven(this IServiceCollection self, IConfiguration configuration)
        {
            var ravenDbClient = new DocumentStore
            {
                Url = configuration.GetValue<string>("Url"),
                DefaultDatabase = configuration.GetValue<string>("Database")
            };
            ravenDbClient.Initialize();

            self.AddSingleton<IDocumentStore>(ravenDbClient);
        }
    }
}
