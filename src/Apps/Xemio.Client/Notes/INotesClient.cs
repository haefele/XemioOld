using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xemio.Shared.Models.Notes;

namespace Xemio.Client.Notes
{
    public interface INotesClient
    {
        Task<NoteDTO> CreateNoteAsync(CreateNote data, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteNoteAsync(long noteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<NoteDTO> GetNoteAsync(long noteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<NoteDTO>> GetNotesAsync(long folderId, CancellationToken cancellationToken = default(CancellationToken));
        Task<NoteDTO> UpdateNoteAsync(long noteId, UpdateNote changes, CancellationToken cancellationToken = default(CancellationToken));
    }
}