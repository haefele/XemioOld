using System;
using Newtonsoft.Json.Linq;

namespace Xemio.Shared.Models.Notes
{
    public class UpdateFolder : JObject
    {
        public bool HasName()
        {
            return this.TryGetValue(nameof(FolderDTO.Name), StringComparison.OrdinalIgnoreCase, out var _);
        }

        public string Name
        {
            get
            {
                this.TryGetValue(nameof(FolderDTO.Name), StringComparison.OrdinalIgnoreCase, out var nameToken);
                return nameToken.ToObject<string>();
            }
            set
            {
                this[nameof(FolderDTO.Name)] = value;
            }
        }


        public bool HasParentFolderId()
        {
            return this.TryGetValue(nameof(FolderDTO.ParentFolderId), StringComparison.OrdinalIgnoreCase, out var _);
        }

        public long? ParentFolderId
        {
            get
            {
                this.TryGetValue(nameof(FolderDTO.ParentFolderId), StringComparison.OrdinalIgnoreCase, out var parentFolderIdToken);
                return parentFolderIdToken.ToObject<long?>();
            }
            set
            {
                this[nameof(FolderDTO.ParentFolderId)] = value;
            }
        }
    }
}