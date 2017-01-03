using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xemio.Shared.Models.Notes;

namespace Xemio.Client.Notes
{
    public class FoldersClient : ClientBase, IFoldersClient
    {
        public FoldersClient(string bearerToken, HttpMessageHandler httpMessageHandler = null)
            : base(bearerToken, httpMessageHandler)
        {
        }
        
        public Task<IList<FolderDTO>> GetRootFoldersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAsync<IList<FolderDTO>>("notes/folders", cancellationToken, HttpStatusCode.OK);
        }

        public Task<IList<FolderDTO>> GetSubFoldersAsync(long folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAsync<IList<FolderDTO>>($"notes/folders/{folderId}/folders", cancellationToken, HttpStatusCode.OK);
        }

        public Task<FolderDTO> GetFolderAsync(long folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAsync<FolderDTO>($"notes/folders/{folderId}", cancellationToken, HttpStatusCode.OK);
        }

        public Task<FolderDTO> CreateFolderAsync(CreateFolder data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.PostAsync<FolderDTO>("notes/folders", data, cancellationToken, HttpStatusCode.Created);
        }

        public Task<FolderDTO> UpdateFolderAsync(long folderId, JObject changes, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.PatchAsync<FolderDTO>($"notes/folders/{folderId}", changes, cancellationToken, HttpStatusCode.OK);
        }

        public Task DeleteFolderAsync(long folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.DeleteAsync<object>($"notes/folders/{folderId}", null, cancellationToken, HttpStatusCode.OK);
        }
    }
}