using System;
using System.Collections.Generic;

namespace Xemio.Server.Infrastructure.Entites.Notes
{
    public class Folder : IEntity, IConcurrencyControlledEntity
    {
        public Folder()
        {
            this.SubFolders = new List<Folder>();
        }

        public Guid Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public Folder ParentFolder { get; set; }

        public IList<Folder> SubFolders { get; set; }

        public byte[] ETag { get; set; }
    }
}
