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
using Xemio.Server.Infrastructure.Extensions;

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

            return this.ToFolderDTO(input, counts);
        }

        public override async Task<IList<FolderDTO>> MapListAsync(IList<Folder> input)
        {
            var folderIds = input.Where(f => f != null).Select(f => f.Id).ToList();

            var counts = await this._documentSession.Query<Folders_ByChildrenCount.Result, Folders_ByChildrenCount>()
                .Where(f => f.FolderId.In(folderIds))
                .ToListAsync();

            return input
                .Select(f => this.ToFolderDTO(f, counts.FirstOrDefault(d => d.FolderId == f.Id)))
                .ToList();
        }

        private FolderDTO ToFolderDTO(Folder folder, Folders_ByChildrenCount.Result counts)
        {
            if (folder == null)
                return null;
            
            return new FolderDTO
            {
                Id = this._documentSession.ToLongId(folder.Id),
                Name = folder.Name,
                ParentFolderId = folder.ParentFolderId == null ? (long?)null : this._documentSession.ToLongId(folder.ParentFolderId),
                NotesCount = counts?.NotesCount ?? 0,
                SubFoldersCount = counts?.SubFolderCount ?? 0,
            };
        }
    }
}
