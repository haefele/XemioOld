using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.Shared.Models.Notes;

namespace Xemio.Client.Notes
{
    public interface IFoldersClient
    {
        Task<FolderDTO> CreateFolderAsync(CreateFolder data, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteFolderAsync(Guid folderId, byte[] etag = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<FolderDTO> GetFolderAsync(Guid folderId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<FolderDTO>> GetRootFoldersAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<FolderDTO>> GetSubFoldersAsync(Guid folderId, CancellationToken cancellationToken = default(CancellationToken));
        Task<FolderDTO> UpdateFolderAsync(Guid folderId, JObject changes, byte[] etag = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}