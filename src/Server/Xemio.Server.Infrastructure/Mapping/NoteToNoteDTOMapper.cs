using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Raven.Client;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Entities.Notes;
using Xemio.Server.Infrastructure.Extensions;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Mapping
{
    public class NoteToNoteDTOMapper : MapperBase<Note, NoteDTO>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public NoteToNoteDTOMapper(IAsyncDocumentSession documentSession)
        {
            EnsureArg.IsNotNull(documentSession, nameof(documentSession));

            this._documentSession = documentSession;
        }

        public override async Task<NoteDTO> MapAsync(Note input, CancellationToken cancellationToken = new CancellationToken())
        {
            if (input == null)
                return null;

            var folder = await this._documentSession.LoadAsync<Folder>(input.FolderId, cancellationToken);

            return this.ToNoteDTO(input, folder);
        }

        public override async Task<IList<NoteDTO>> MapListAsync(IList<Note> input, CancellationToken cancellationToken = new CancellationToken())
        {
            var folderIds = input.Where(f => f != null).Select(f => f.FolderId).Distinct().ToList();

            var folders = await this._documentSession.LoadAsync<Folder>(folderIds, cancellationToken);

            return input
                .Select(f => f == null
                    ? null
                    : this.ToNoteDTO(f, folders.First(d => d.Id == f.FolderId)))
                .ToList();
        }

        private NoteDTO ToNoteDTO(Note note, Folder folder)
        {
            if (note == null)
                return null;

            return new NoteDTO
            {
                Id = this._documentSession.ToLongId(note.Id),
                Title = note.Title,
                Content = note.Content,
                FolderId = this._documentSession.ToLongId(folder.Id),
                FolderName = folder.Name,
            };
        }
    }
}