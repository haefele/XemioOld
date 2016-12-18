using System.Threading.Tasks;
using EnsureThat;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Database;
using Xemio.Server.Infrastructure.Entities.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Mapping
{
    public class NoteToNoteDTOMapper : MapperBase<Note, NoteDTO>
    {
        private readonly XemioContext _xemioContext;

        public NoteToNoteDTOMapper(XemioContext xemioContext)
        {
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));

            this._xemioContext = xemioContext;
        }

        public override async Task<NoteDTO> MapAsync(Note input)
        {
            if (input == null)
                return null;

            await this._xemioContext.Entry(input)
                .Reference(f => f.Folder)
                .LoadAsync();

            return new NoteDTO
            {
                Id = input.Id,
                Title = input.Title,
                Content = input.Content,
                ETag = input.ETag,
                FolderId = input.Folder.Id,
                FolderName = input.Folder.Name
            };
        }
    }
}