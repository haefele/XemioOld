using System;

namespace Xemio.Server.Infrastructure.Entities
{
    interface IEntity
    {
        Guid Id { get; set; }
    }
}
