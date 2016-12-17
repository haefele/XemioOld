using System;

namespace Xemio.Shared.Models.Notes
{
    public class CreateFolder
    {
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}