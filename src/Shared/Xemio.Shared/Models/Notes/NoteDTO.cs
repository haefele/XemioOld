using System;

namespace Xemio.Shared.Models.Notes
{
    public class NoteDTO
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public long FolderId { get; set; }
        public string FolderName { get; set; }
    }
}