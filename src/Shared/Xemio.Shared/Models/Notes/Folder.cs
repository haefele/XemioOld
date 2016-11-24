using System;

namespace Xemio.Shared.Models.Notes
{
    public class Folder : IEntity, IConcurrencyControlledEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public Guid? ParentFolderId { get; set; }

        public byte[] ETag { get; set; }
    }
}
