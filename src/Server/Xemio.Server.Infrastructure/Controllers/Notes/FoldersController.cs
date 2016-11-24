using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xemio.Server.Infrastructure.Database;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Controllers.Notes
{
    [Authorize]
    [Route("notes/folders")]
    public class FoldersController : ControllerBase
    {
        private readonly XemioContext _xemioContext;

        public FoldersController(XemioContext xemioContext)
        {
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));

            this._xemioContext = xemioContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFolders(Guid? folderId = null)
        {
            var folders = await this._xemioContext.Folders
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolderId == folderId)
                .ToListAsync();

            return Ok(folders);
        }

        [HttpGet("{folderId}", Name = nameof(GetFolder))]
        public async Task<IActionResult> GetFolder(Guid folderId)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));

            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null)
                return NotFound();

            return Ok(folder);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostFolder([FromBody]CreateFolder data)
        {
            EnsureArg.IsNotNull(data, nameof(data));

            var folder = new Folder
            {
                Name = data.Name,
                ParentFolderId = data.ParentFolderId,
                UserId = this.User.Identity.Name
            };

            await this._xemioContext.AddAsync(folder);
            await this._xemioContext.SaveChangesAsync();

            return Created(this.Url.Link(nameof(GetFolder), new { folderId = folder.Id }), folder);
        }

        [HttpDelete("{folderId}")]
        public async Task<IActionResult> DeleteFolder(Guid folderId, [FromQuery]byte[] etag = null)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));

            try
            {
                Folder folder = await this._xemioContext.FindAsync<Folder>(folderId);

                if (folder == null)
                    return NotFound();

                if (etag != null)
                    this._xemioContext.ETagForConcurrencyControlIs(folder, etag);

                List<Folder> subFolders = await this._xemioContext.Folders
                    .Where(f => f.ParentFolderId == folderId)
                    .ToListAsync();
            
                foreach (var subFolder in subFolders)
                {
                    subFolder.ParentFolderId = null;
                }

                this._xemioContext.Remove(folder);

                await this._xemioContext.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return base.BadRequest("The folder was modified in the meantime.");
            }
        }
    }
}
