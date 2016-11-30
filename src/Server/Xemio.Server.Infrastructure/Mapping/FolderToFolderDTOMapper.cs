using System;
using Xemio.Server.Infrastructure.Entites.Notes;
using Xemio.Shared.Models.Notes;
using Xemio.Server.Contracts.Mapping;
using System.Threading.Tasks;
using Google.Protobuf;
using EnsureThat;
using Xemio.Server.Infrastructure.Database;

namespace Xemio.Server.Infrastructure.Mapping
{

    public class FolderToFolderDTOMapper : MapperBase<Folder, FolderDTO>
    {
        private readonly IMapper<Guid?, string> _guidToStringMapper;
        private readonly XemioContext _xemioContext;

        public FolderToFolderDTOMapper(IMapper<Guid?, string> guidToStringMapper, XemioContext xemioContext)
        {
            EnsureArg.IsNotNull(guidToStringMapper, nameof(guidToStringMapper));
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));

            this._guidToStringMapper = guidToStringMapper;
            this._xemioContext = xemioContext;
        }

        public override async Task<FolderDTO> MapAsync(Folder input)
        {
            if (input == null)
                return null;

            await this._xemioContext.Entry(input)
                .Reference(F => F.ParentFolder)
                .LoadAsync();

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
