using EnsureThat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xemio.Shared.Models.Notes
{
    public class FolderDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
        public byte[] ETag { get; set; }
        public int SubFoldersCount { get; set; }
    }
}
