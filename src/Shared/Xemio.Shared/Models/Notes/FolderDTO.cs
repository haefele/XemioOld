using EnsureThat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xemio.Shared.Models.Notes
{
    public class FolderDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentFolderId { get; set; }
        public int SubFoldersCount { get; set; }
        public int NotesCount { get; set; }
    }
}
