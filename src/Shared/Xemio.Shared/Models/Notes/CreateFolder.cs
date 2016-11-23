using System;
using System.Collections.Generic;
using System.Text;

namespace Xemio.Shared.Models.Notes
{
    public class CreateFolder
    {
        public string Name { get; set; }
        public Guid? ParentFolderId { get; set; }
    }
}
