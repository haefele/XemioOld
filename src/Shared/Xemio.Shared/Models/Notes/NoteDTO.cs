using System;

namespace Xemio.Shared.Models.Notes
{
    public class NoteDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid FolderId { get; set; }
        public byte[] ETag { get; set; }
        public string FolderName { get; set; }
    }
}