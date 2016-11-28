using System;
using Xemio.Server.Infrastructure.Entites.Notes;
using Xemio.Shared.Models.Notes;
using Xemio.Server.Contracts.Mapping;
using System.Threading.Tasks;
using Google.Protobuf;
using EnsureThat;

namespace Xemio.Server.Infrastructure.Mapping
{

    public class FolderToFolderDTOMapper : MapperBase<Folder, FolderDTO>
    {
        private readonly IMapper<Guid?, string> _guidToStringMapper;

        public FolderToFolderDTOMapper(IMapper<Guid?, string> guidToStringMapper)
        {
            EnsureArg.IsNotNull(guidToStringMapper, nameof(guidToStringMapper));

            this._guidToStringMapper = guidToStringMapper;
        }

        public override async Task<FolderDTO> MapAsync(Folder input)
        {
            if (input == null)
                return null;

            return new FolderDTO
            {
                Id = await this._guidToStringMapper.MapAsync(input.Id),
                Etag = ByteString.CopyFrom(input.ETag),
                Name = input.Name,
                ParentFolderId = await this._guidToStringMapper.MapAsync(input.ParentFolder?.Id) ?? string.Empty,
                UserId = input.UserId,
            };
        }
    }
}
