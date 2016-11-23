using System;
using System.Collections.Generic;
using System.Text;

namespace Xemio.Shared.Models.Notes
{
    public class Folder
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public Guid? ParentFolderId { get; set; }
    }
}
