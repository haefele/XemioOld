using System;
using Newtonsoft.Json.Linq;

namespace Xemio.Shared.Models.Notes
{
    public class UpdateNote : JObject
    {
        public bool HasTitle()
        {
            return this.TryGetValue(nameof(NoteDTO.Title), StringComparison.OrdinalIgnoreCase, out var _);
        }
        public string Title
        {
            get
            {
                this.TryGetValue(nameof(NoteDTO.Title), StringComparison.OrdinalIgnoreCase, out var titleToken);
                return titleToken.ToObject<string>();
            }
            set
            {
                this[nameof(NoteDTO.Title)] = value;
            }
        }

        public bool HasContent()
        {
            return this.TryGetValue(nameof(NoteDTO.Content), StringComparison.OrdinalIgnoreCase, out var _);
        }
        public string Content
        {
            get
            {
                this.TryGetValue(nameof(NoteDTO.Content), StringComparison.OrdinalIgnoreCase, out var contentToken);
                return contentToken.ToObject<string>();
            }
            set
            {
                this[nameof(NoteDTO.Content)] = value;
            }
        }

        public bool HasFolderId()
        {
            return this.TryGetValue(nameof(NoteDTO.FolderId), StringComparison.OrdinalIgnoreCase, out var _);
        }
        public long? FolderId
        {
            get
            {
                this.TryGetValue(nameof(NoteDTO.FolderId), StringComparison.OrdinalIgnoreCase, out var titleToken);
                return titleToken.ToObject<long?>();
            }
            set
            {
                this[nameof(NoteDTO.Title)] = value;
            }
        }
    }
}