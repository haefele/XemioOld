using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Hosts.AspNetCore.Setup
{

    public static class Database
    {
        public static void AddDatabase(this IServiceCollection self, IConfiguration configuration)
        {
            var documentStore = new DocumentStore();
            documentStore.ParseConnectionString(configuration.GetValue<string>("ConnectionString"));

            documentStore.Initialize();

            self.AddSingleton<IDocumentStore>(documentStore);
            self.AddScoped<IAsyncDocumentSession>((sp) =>
            {
                var session = sp.GetService<IDocumentStore>().OpenAsyncSession();

                session.Advanced.WaitForIndexesAfterSaveChanges();

                return session;
            });
        }

        public static void MigrateDatabase(this IApplicationBuilder self)
        {
            var documentStore = self.ApplicationServices.GetService<IDocumentStore>();
            IndexCreation.CreateIndexes(typeof(Folder).GetTypeInfo().Assembly, documentStore);
        }
    }
}
