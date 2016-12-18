using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Entities.Notes;
using Xemio.Server.Infrastructure.Mapping;
using Xemio.Shared.Models.Notes;

namespace Xemio.Hosts.AspNetCore.Setup
{

    public static class Mappers
    {
        public static void AddMappers(this IServiceCollection self)
        {
            var mappers = from type in typeof(FolderToFolderDTOMapper).GetTypeInfo().Assembly.GetTypes()
                          let interfaces = type.GetInterfaces()
                          from mapper in interfaces.Where(f => f.GetTypeInfo().IsGenericType && f.GetGenericTypeDefinition() == typeof(IMapper<,>))
                          select new
                          {
                              Interface = mapper,
                              Implementation = type
                          };

            foreach (var mapper in mappers)
            {
                self.Add(ServiceDescriptor.Transient(mapper.Interface, mapper.Implementation));
            }
        }
    }
}
