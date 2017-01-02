using System.Collections.Generic;
using System.Linq;
using Xemio.Shared.Models.Notes;
using Xemio.Server.Contracts.Mapping;
using System.Threading.Tasks;
using EnsureThat;
using Raven.Client;
using Raven.Client.Linq;
using Xemio.Server.Infrastructure.Database.Indexes;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Mapping
{
    public class FolderToFolderDTOMapper : MapperBase<Folder, FolderDTO>
    {
        private readonly IAsyncDocumentSession _documentSession;

        public FolderToFolderDTOMapper(IAsyncDocumentSession documentSession)
        {
            this._documentSession = documentSession;
        }

        public override async Task<FolderDTO> MapAsync(Folder input)
        {
            if (input == null)
                return null;

            var counts = await this._documentSession.Query<Folders_ByChildrenCount.Result, Folders_ByChildrenCount>()
                .Where(f => f.FolderId == input.Id)
                .FirstOrDefaultAsync();

            return new FolderDTO
            {
                Id = input.Id,
                Name = input.Name,
                ParentFolderId = input.ParentFolderId,
                NotesCount = counts?.NotesCount ?? 0,
                SubFoldersCount = counts?.SubFolderCount ?? 0,
            };
        }
    }
}
