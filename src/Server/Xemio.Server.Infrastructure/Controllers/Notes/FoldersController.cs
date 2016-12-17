using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Database;
using Xemio.Server.Infrastructure.Entities.Notes;
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
            public const string GetRootFolders = nameof(GetRootFolders);
            public const string GetSubFolders = nameof(GetSubFolders);
            public const string GetFolderById = nameof(GetFolderById);
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
        
        [HttpGet(Name = RouteNames.GetRootFolders)]
        [Description("Get all root folders.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(IList<FolderDTO>), Description = "All root folders.")]
        public async Task<IActionResult> GetRootFoldersAsync()
        {
            var folders = await this._xemioContext.Folders
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolder == null)
                .ToListAsync();

            var folderDTOs = await this._folderToFolderDTOMapper.MapListAsync(folders);

            return this.Ok(folderDTOs);
        }

        [HttpGet("{folderId:guid}/folders", Name = RouteNames.GetSubFolders)]
        [Description("Get all sub folders from the specified folder.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(void), Description = "The folder does not exist.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(IList<FolderDTO>), Description = "All sub folders.")]
        public async Task<IActionResult> GetSubFoldersAsync(Guid folderId)
        {
            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null)
                return this.NotFound();

            var folders = await this._xemioContext.Folders
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolder.Id == folderId)
                .ToListAsync();

            var folderDTOs = await this._folderToFolderDTOMapper.MapListAsync(folders);

            return this.Ok(folderDTOs);
        }

        [HttpGet("{folderId:guid}", Name = RouteNames.GetFolderById)]
        [Description("Get the specified folder.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(FolderDTO), Description = "The folder.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(void), Description = "The folder does not exist.")]
        public async Task<IActionResult> GetFolderAsync(Guid folderId)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));

            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();
            
            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.Ok(folderDTO);
        }
        
        [HttpPost(Name = RouteNames.CreateFolder)]
        [Description("Create a new folder.")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(FolderDTO), Description = "The folder was created.")]
        public async Task<IActionResult> PostFolderAsync([FromBody]CreateFolder data)
        {
            EnsureArg.IsNotNull(data, nameof(data));

            var folder = new Folder
            {
                Name = data.Name,
                ParentFolder = data.ParentFolderId != null 
                    ? await this._xemioContext.FindAsync<Folder>(data.ParentFolderId.Value) 
                    : null,
                UserId = this.User.Identity.Name
            };

            await this._xemioContext.AddAsync(folder);
            await this._xemioContext.SaveChangesAsync();

            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.Created(this.Url.Link(RouteNames.GetFolderById, new { folderId = folder.Id }), folderDTO);
        }

        [HttpPatch("{folderId:guid}", Name = RouteNames.UpdateFolder)]
        [Description("Update the specified folder.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(void), Description = "The folder does not exist.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(void), Description = "The folder was updated.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, typeof(void), Description = "The folder was changed in the meantime.")]
        public async Task<IActionResult> PatchFolderAsync(Guid folderId, [FromBody]JObject data, [FromQuery]byte[] etag = null)
        {
            EnsureArg.IsNotNull(data, nameof(data));
            
            var folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();
            
            if (etag != null)
                this._xemioContext.Entry(folder).ETagForConcurrencyControlIs(etag);

            if (data.TryGetValue(nameof(FolderDTO.Name), StringComparison.OrdinalIgnoreCase, out var nameToken))
            {
                var name = nameToken.ToObject<string>();
                folder.Name = name;
            }

            if (data.TryGetValue(nameof(FolderDTO.ParentFolderId), StringComparison.OrdinalIgnoreCase, out var parentFolderIdToken))
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

        [HttpDelete("{folderId:guid}", Name = RouteNames.DeleteFolder)]
        [Description("Delete the specified folder and all subfolders.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, typeof(void), Description = "The folder was changed in the meantime.")]
        public async Task<IActionResult> DeleteFolderAsync(Guid folderId, [FromQuery]byte[] etag = null)
        {
            EnsureArg.IsNotEmpty(folderId, nameof(folderId));
            
            Folder folder = await this._xemioContext.FindAsync<Folder>(folderId);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();

            if (etag != null)
                this._xemioContext.Entry(folder).ETagForConcurrencyControlIs(etag);
                
            this._xemioContext.Remove(folder);

            await this._xemioContext.SaveChangesAsync();

            return this.Ok();
        }
    }
}
