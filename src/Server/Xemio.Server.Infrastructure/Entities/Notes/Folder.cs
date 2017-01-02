using System;
using System.Collections.Generic;

namespace Xemio.Server.Infrastructure.Entities.Notes
{
    public class Folder : IEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public Guid? ParentFolderId { get; set; }
    }
}
