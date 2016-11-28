using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xemio.Server.Infrastructure.Database;
using Xemio.Server.Infrastructure.Entites.Notes;
using Xemio.Server.Infrastructure.Extensions;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Controllers.Notes
{
    [Authorize]
    [Route("notes/folders")]
    public class FoldersController : ControllerBase
    {
        public static class RouteNames
        {
            public const string GetFolders = nameof(GetFolders);
            public const string GetFolderByFolderId = nameof(GetFolderByFolderId);
            public const string CreateFolder = nameof(CreateFolder);
            public const string DeleteFolder = nameof(DeleteFolder);
        }

        private readonly XemioContext _xemioContext;

        public FoldersController(XemioContext xemioContext)
        {
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));

            this._xemioContext = xemioContext;
        }
        
        [HttpGet(Name = RouteNames.GetFolders)]
        public async Task<IActionResult> GetFolders(Guid? folderId = null)
        {
            var folders = await this._xemioContext.Folders
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolder.Id == folderId)
                .ToListAsync();

            return this.Ok(folders);
        }

        [HttpGet("{folderId}", Name = RouteNames.GetFolderByFolderId)]
        public async Task<IActionResult> GetFolder(Guid folderId)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));

            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null)
                return this.NotFound();

            return this.Ok(folder);
        }
        
        [HttpPost(Name = RouteNames.CreateFolder)]
        public async Task<IActionResult> PostFolder([FromBody]CreateFolder data)
        {
            EnsureArg.IsNotNull(data, nameof(data));

            var folder = new Folder
            {
                Name = data.Name,
                ParentFolder = data.ParentFolderId != null 
                    ? await this._xemioContext.FindAsync<Folder>(data.ParentFolderId) 
                    : null,
                UserId = this.User.Identity.Name
            };

            await this._xemioContext.AddAsync(folder);
            await this._xemioContext.SaveChangesAsync();

            return this.Created(this.Url.Link(RouteNames.GetFolderByFolderId, new { folderId = folder.Id }), folder);
        }

        [HttpDelete("{folderId}", Name = RouteNames.DeleteFolder)]
        public async Task<IActionResult> DeleteFolder(Guid folderId, [FromQuery]byte[] etag = null)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));

            try
            {
                Folder folder = await this._xemioContext.FindAsync<Folder>(folderId);

                if (folder == null)
                    return this.NotFound();

                if (etag != null)
                    this._xemioContext.Entry(folder).ETagForConcurrencyControlIs(etag);
                
                this._xemioContext.Remove(folder);

                await this._xemioContext.SaveChangesAsync();

                return this.Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.Conflict();
            }
        }
    }
}
