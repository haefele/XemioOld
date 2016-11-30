using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xemio.Server.Contracts.Mapping;
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
            public const string UpdateFolder = nameof(UpdateFolder);
            public const string DeleteFolder = nameof(DeleteFolder);
        }

        private readonly XemioContext _xemioContext;
        private readonly IMapper<Folder, FolderDTO> _folderToFolderDTOMapper;

        public FoldersController(XemioContext xemioContext, IMapper<Folder, FolderDTO> folderToFolderDTOMapper)
        {
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));
            EnsureArg.IsNotNull(folderToFolderDTOMapper, nameof(folderToFolderDTOMapper));

            this._xemioContext = xemioContext;
            this._folderToFolderDTOMapper = folderToFolderDTOMapper;
        }
        
        [HttpGet(Name = RouteNames.GetFolders)]
        public async Task<IActionResult> GetFoldersAsync(Guid? folderId = null)
        {
            var folders = await this._xemioContext.Folders
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolder.Id == folderId)
                .ToListAsync();

            var folderDTOs = await this._folderToFolderDTOMapper.MapListAsync(folders);

            return this.Ok(folderDTOs);
        }

        [HttpGet("{folderId}", Name = RouteNames.GetFolderByFolderId)]
        public async Task<IActionResult> GetFolderAsync(Guid folderId)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));

            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null)
                return this.NotFound();

            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.Ok(folderDTO);
        }
        
        [HttpPost(Name = RouteNames.CreateFolder)]
        public async Task<IActionResult> PostFolderAsync([FromBody]CreateFolder data)
        {
            EnsureArg.IsNotNull(data, nameof(data));

            var folder = new Folder
            {
                Name = data.Name,
                ParentFolder = string.IsNullOrWhiteSpace(data.ParentFolderId) == false 
                    ? await this._xemioContext.FindAsync<Folder>(Guid.Parse(data.ParentFolderId)) 
                    : null,
                UserId = this.User.Identity.Name
            };

            await this._xemioContext.AddAsync(folder);
            await this._xemioContext.SaveChangesAsync();

            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.Created(this.Url.Link(RouteNames.GetFolderByFolderId, new { folderId = folder.Id }), folderDTO);
        }

        [HttpPatch("{folderId}", Name = RouteNames.UpdateFolder)]
        public async Task<IActionResult> PatchFolderAsync(Guid folderId, [FromBody]JObject data, [FromQuery]byte[] etag = null)
        {
            EnsureArg.IsNotNull(data, nameof(data));
            
            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null)
                return this.NotFound();

            if (etag != null)
                this._xemioContext.Entry(folder).ETagForConcurrencyControlIs(etag);

            JToken nameToken;
            if (data.TryGetValue(nameof(FolderDTO.Name), StringComparison.OrdinalIgnoreCase, out nameToken))
            {
                var name = nameToken.ToObject<string>();
                folder.Name = name;
            }

            JToken parentFolderIdToken;
            if (data.TryGetValue(nameof(FolderDTO.ParentFolderId), StringComparison.OrdinalIgnoreCase, out parentFolderIdToken))
            {
                await this._xemioContext.Entry(folder)
                    .Reference(f => f.ParentFolder)
                    .LoadAsync();

                var parentFolderId = parentFolderIdToken.ToObject<Guid?>();

                if (parentFolderId == null)
                {
                    folder.ParentFolder = null;
                }
                else
                {
                    var parentFolder = await this._xemioContext.FindAsync<Folder>(parentFolderId.Value);

                    if (parentFolder != null)
                        folder.ParentFolder = parentFolder;
                }
            }

            await this._xemioContext.SaveChangesAsync();

            return this.Ok();
        }

        [HttpDelete("{folderId}", Name = RouteNames.DeleteFolder)]
        public async Task<IActionResult> DeleteFolderAsync(Guid folderId, [FromQuery]byte[] etag = null)
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
