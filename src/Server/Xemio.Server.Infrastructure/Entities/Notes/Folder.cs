using System;

namespace Xemio.Server.Infrastructure.Entities.Notes
{
    public class Folder : IEntity
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public string ParentFolderId { get; set; }
    }
}
