﻿using Microsoft.Extensions.DependencyInjection;
using System;
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
            self.AddTransient<IMapper<Folder, FolderDTO>, FolderToFolderDTOMapper>();
        }
    }
}
