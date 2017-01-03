using System;

namespace Xemio.Shared.Models.Notes
{
    public class CreateFolder
    {
        public string Name { get; set; }
        public long? ParentFolderId { get; set; }
    }
}