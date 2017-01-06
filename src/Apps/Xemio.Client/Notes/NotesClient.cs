using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xemio.Shared.Models.Notes;

namespace Xemio.Client.Notes
{
    public class NotesClient : ClientBase, INotesClient
    {
        public NotesClient(string bearerToken, HttpMessageHandler httpMessageHandler = null) 
            : base(bearerToken, httpMessageHandler)
        {
        }

        public Task<NoteDTO> CreateNoteAsync(CreateNote data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.PostAsync<NoteDTO>("notes", data, cancellationToken, HttpStatusCode.Created);
        }

        public Task DeleteNoteAsync(long noteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.DeleteAsync<object>($"notes/{noteId}", null, cancellationToken, HttpStatusCode.OK);
        }

        public Task<NoteDTO> GetNoteAsync(long noteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAsync<NoteDTO>($"notes/{noteId}", cancellationToken, HttpStatusCode.OK);
        }

        public Task<IList<NoteDTO>> GetNotesAsync(long folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new Dictionary<string, string>
            {
                ["folderId"] = folderId.ToString()
            };

            return base.GetAsync<IList<NoteDTO>>("notes", query, cancellationToken, HttpStatusCode.OK);
        }

        public Task<NoteDTO> UpdateNoteAsync(long noteId, UpdateNote changes, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.PatchAsync<NoteDTO>($"notes/{noteId}", changes, cancellationToken, HttpStatusCode.OK);
        }
    }
}