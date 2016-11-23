using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Raven.Client;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Controllers.Notes
{
    [Route("notes/folders")]
    public class FoldersController : ControllerBase
    {
        private readonly IDocumentStore _documentStore;

        public FoldersController(IDocumentStore documentStore)
        {
            this._documentStore = documentStore;
        }

        [HttpGet]
        [Authorize]
        public async Task<IList<Folder>> GetFolders(Guid? folderId = null)
        {
            using (var session = this._documentStore.OpenAsyncSession())
            {
                return await session.Query<Folder>()
                    .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolderId == folderId)
                    .ToListAsync();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{folderId}")]
        public async Task<Folder> GetFolder(Guid folderId)
        {
            using (var session = this._documentStore.OpenAsyncSession())
            {
                return await session.LoadAsync<Folder>(folderId);
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<Folder> PostFolder([FromBody]CreateFolder data)
        {
            using (var session = this._documentStore.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var folder = new Folder
                {
                    Name = data.Name,
                    ParentFolderId = data.ParentFolderId,
                    UserId = this.User.Identity.Name
                };

                await session.StoreAsync(folder);
                await session.SaveChangesAsync();

                return folder;
            }
        }
    }
}
