using Xemio.Shared.Models.Notes;
using Xemio.Server.Contracts.Mapping;
using System.Threading.Tasks;
using EnsureThat;
using Xemio.Server.Infrastructure.Database;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Mapping
{

    public class FolderToFolderDTOMapper : MapperBase<Folder, FolderDTO>
    {
        private readonly XemioContext _xemioContext;

        public FolderToFolderDTOMapper(XemioContext xemioContext)
        {
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));
            
            this._xemioContext = xemioContext;
        }

        public override async Task<FolderDTO> MapAsync(Folder input)
        {
            if (input == null)
                return null;

            await this._xemioContext.Entry(input)
                .Reference(f => f.ParentFolder)
                .LoadAsync();

            await this._xemioContext.Entry(input)
                .Collection(f => f.SubFolders)
                .LoadAsync();

            return new FolderDTO
            {
                Id = input.Id,
                ETag = input.ETag,
                Name = input.Name,
                ParentFolderId = input.ParentFolder?.Id,
                SubFoldersCount = input.SubFolders.Count,
            };
        }
    }
}
