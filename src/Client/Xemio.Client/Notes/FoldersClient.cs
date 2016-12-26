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

        public Task<IList<FolderDTO>> GetSubFoldersAsync(Guid folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAsync<IList<FolderDTO>>($"notes/folders/{folderId:D}/folders", cancellationToken, HttpStatusCode.OK);
        }

        public Task<FolderDTO> GetFolderAsync(Guid folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAsync<FolderDTO>($"notes/folders/{folderId:D}", cancellationToken, HttpStatusCode.OK);
        }

        public Task<FolderDTO> CreateFolderAsync(CreateFolder data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.PostAsync<FolderDTO>("notes/folders", data, cancellationToken, HttpStatusCode.Created);
        }

        public Task<FolderDTO> UpdateFolderAsync(Guid folderId, JObject changes, byte[] etag = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new Dictionary<string, string>();

            if (etag != null)
                query["etag"] = Convert.ToBase64String(etag);

            return base.PatchAsync<FolderDTO>($"notes/folders/{folderId:D}", query, changes, cancellationToken, HttpStatusCode.OK);
        }

        public Task DeleteFolderAsync(Guid folderId, byte[] etag = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new Dictionary<string, string>();

            if (etag != null)
                query["etag"] = Convert.ToBase64String(etag);

            return base.DeleteAsync<object>($"notes/folders/{folderId:D}", query, null, cancellationToken, HttpStatusCode.OK);
        }
    }
}