using System;

namespace Xemio.Server.Infrastructure.Entities.Notes
{
    public class Note : IEntity, IConcurrencyControlledEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public Folder Folder { get; set; }

        public byte[] ETag { get; set; }
    }
}