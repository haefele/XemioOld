using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Database.Indexes;
using Xemio.Server.Infrastructure.Entities.Notes;
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

        private readonly IAsyncDocumentSession _documentSession;
        private readonly IMapper<Folder, FolderDTO> _folderToFolderDTOMapper;

        public FoldersController(IAsyncDocumentSession documentSession, IMapper<Folder, FolderDTO> folderToFolderDTOMapper)
        {
            EnsureArg.IsNotNull(documentSession, nameof(documentSession));
            EnsureArg.IsNotNull(folderToFolderDTOMapper, nameof(folderToFolderDTOMapper));

            this._documentSession = documentSession;
            this._folderToFolderDTOMapper = folderToFolderDTOMapper;
        }

        [HttpGet(Name = RouteNames.GetRootFolders)]
        public async Task<IActionResult> GetRootFoldersAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var folders = await this._documentSession.Query<Folder, Folders_ForQuery>()
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolderId == null)
                .ToListAsync(cancellationToken);

            var folderDTOs = await this._folderToFolderDTOMapper.MapListAsync(folders);

            return this.Ok(folderDTOs);
        }

        [HttpGet("{folderId:guid}/folders", Name = RouteNames.GetSubFolders)]
        public async Task<IActionResult> GetSubFoldersAsync([Required]Guid? folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var folder = await this._documentSession.LoadAsync<Folder>(folderId, cancellationToken);

            if (folder == null)
                return this.NotFound();

            var folders = await this._documentSession.Query<Folder, Folders_ForQuery>()
                .Where(f => f.UserId == this.User.Identity.Name && f.ParentFolderId == folderId)
                .ToListAsync(cancellationToken);

            var folderDTOs = await this._folderToFolderDTOMapper.MapListAsync(folders);

            return this.Ok(folderDTOs);
        }

        [HttpGet("{folderId:guid}", Name = RouteNames.GetFolderById)]
        public async Task<IActionResult> GetFolderAsync([Required]Guid? folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var folder = await this._documentSession.LoadAsync<Folder>(folderId, cancellationToken);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();

            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.Ok(folderDTO);
        }

        [HttpPost(Name = RouteNames.CreateFolder)]
        public async Task<IActionResult> CreateFolderAsync([FromBody][Required]CreateFolder data, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parentFolder = data.ParentFolderId != null
                ? await this._documentSession.LoadAsync<Folder>(data.ParentFolderId.Value, cancellationToken)
                : null;

            var folder = new Folder
            {
                Name = data.Name,
                ParentFolderId = parentFolder?.Id,
                UserId = this.User.Identity.Name
            };

            await this._documentSession.StoreAsync(folder, cancellationToken);
            await this._documentSession.SaveChangesAsync(cancellationToken);

            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.CreatedAtRoute(RouteNames.GetFolderById, new { folderId = folder.Id }, folderDTO);
        }

        [HttpPatch("{folderId:guid}", Name = RouteNames.UpdateFolder)]
        public async Task<IActionResult> UpdateFolderAsync([Required]Guid? folderId, [FromBody][Required]JObject data, CancellationToken cancellationToken = default(CancellationToken))
        {
            var folder = await this._documentSession.LoadAsync<Folder>(folderId, cancellationToken);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();
            
            if (data.TryGetValue(nameof(FolderDTO.Name), StringComparison.OrdinalIgnoreCase, out var nameToken))
            {
                var name = nameToken.ToObject<string>();
                folder.Name = name;
            }

            if (data.TryGetValue(nameof(FolderDTO.ParentFolderId), StringComparison.OrdinalIgnoreCase, out var parentFolderIdToken))
            {
                var parentFolderId = parentFolderIdToken.ToObject<Guid?>();

                if (parentFolderId == null)
                {
                    folder.ParentFolderId = null;
                }
                else
                {
                    var parentFolder = await this._documentSession.LoadAsync<Folder>(parentFolderId.Value, cancellationToken);

                    if (parentFolder != null)
                        folder.ParentFolderId = parentFolder.Id;
                }
            }
            
            await this._documentSession.SaveChangesAsync(cancellationToken);

            var folderDTO = await this._folderToFolderDTOMapper.MapAsync(folder);

            return this.Ok(folderDTO);
        }

        [HttpDelete("{folderId:guid}", Name = RouteNames.DeleteFolder)]
        public async Task<IActionResult> DeleteFolderAsync([Required]Guid? folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Folder folder = await this._documentSession.LoadAsync<Folder>(folderId, cancellationToken);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();
            
            this._documentSession.Delete(folder);
            await this._documentSession.SaveChangesAsync(cancellationToken);

            return this.Ok();
        }
    }
}
