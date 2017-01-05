using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Client;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Entities.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Controllers.Notes
{
    [Authorize]
    [Route("notes")]
    public class NotesController : ControllerBase
    {
        public static class RouteNames
        {
            public const string CreateNote = nameof(CreateNote);
            public const string GetNoteById = nameof(GetNoteById);
            public const string GetNotesFromFolder = nameof(GetNotesFromFolder);
            public const string UpdateNote = nameof(UpdateNote);
            public const string DeleteNote = nameof(DeleteNote);
        }

        private readonly IAsyncDocumentSession _documentSession;
        private readonly IMapper<Note, NoteDTO> _noteToNoteDTOMapper;

        public NotesController(IAsyncDocumentSession documentSession, IMapper<Note, NoteDTO> noteToNoteDTOMapper)
        {
            EnsureArg.IsNotNull(documentSession, nameof(documentSession));
            EnsureArg.IsNotNull(noteToNoteDTOMapper, nameof(noteToNoteDTOMapper));

            this._documentSession = documentSession;
            this._noteToNoteDTOMapper = noteToNoteDTOMapper;
        }

        [HttpGet("{noteId:long}", Name = RouteNames.GetNoteById)]
        public async Task<IActionResult> GetNoteByIdAsync([Required]long? noteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var note = await this._documentSession.LoadAsync<Note>(noteId, cancellationToken);

            if (note == null || note.UserId != this.User.Identity.Name)
                return this.NotFound();

            var noteDTO = await this._noteToNoteDTOMapper.MapAsync(note, cancellationToken);

            return this.Ok(noteDTO);
        }

        [HttpGet(Name = RouteNames.GetNotesFromFolder)]
        public async Task<IActionResult> GetNotesFromFolderAsync([Required][FromQuery]long? folderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var folder = await this._documentSession.LoadAsync<Folder>(folderId, cancellationToken);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();

            var notes = await this._documentSession.Query<Note>()
                .Where(f => f.UserId == this.User.Identity.Name && f.FolderId == folder.Id)
                .ToListAsync(cancellationToken);

            var noteDTOs = await this._noteToNoteDTOMapper.MapListAsync(notes, cancellationToken);

            return this.Ok(noteDTOs);
        }

        [HttpPost(Name = RouteNames.CreateNote)]
        public async Task<IActionResult> CreateNoteAsync([FromBody][Required]CreateNote createNote, CancellationToken cancellationToken = default(CancellationToken))
        {
            var folder = await this._documentSession.LoadAsync<Folder>(createNote.FolderId, cancellationToken);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();

            var note = new Note
            {
                Title = createNote.Title,
                Content = createNote.Content,
                UserId = this.User.Identity.Name,
                FolderId = folder.Id,
            };

            await this._documentSession.StoreAsync(note, cancellationToken);
            await this._documentSession.SaveChangesAsync(cancellationToken);

            var noteDTO = await this._noteToNoteDTOMapper.MapAsync(note, cancellationToken);

            return this.CreatedAtRoute(RouteNames.GetNoteById, new { noteId = note.Id }, noteDTO);
        }

        [HttpPatch("{noteId:long}", Name = RouteNames.UpdateNote)]
        public async Task<IActionResult> UpdateNoteAsync([Required] long? noteId, [FromBody][Required]UpdateNote data, CancellationToken cancellationToken = default(CancellationToken))
        {
            var note = await this._documentSession.LoadAsync<Note>(noteId, cancellationToken);

            if (note == null || note.UserId != this.User.Identity.Name)
                return this.NotFound();

            if (data.HasTitle())
            {
                note.Title = data.Title;
            }

            if (data.HasContent())
            {
                note.Content = data.Content;
            }

            if (data.HasFolderId())
            {
                var folder = await this._documentSession.LoadAsync<Folder>(data.FolderId, cancellationToken);

                if (folder != null && folder.UserId == this.User.Identity.Name)
                    note.FolderId = folder.Id;
            }

            await this._documentSession.SaveChangesAsync(cancellationToken);

            var noteDTO = await this._noteToNoteDTOMapper.MapAsync(note, cancellationToken);

            return this.Ok(noteDTO);
        }

        [HttpDelete("{noteId:long}", Name = RouteNames.DeleteNote)]
        public async Task<IActionResult> DeleteNoteAsync([Required] long? noteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var note = await this._documentSession.LoadAsync<Note>(noteId, cancellationToken);

            if (note == null || note.UserId != this.User.Identity.Name)
                return this.NotFound();

            this._documentSession.Delete(note);
            await this._documentSession.SaveChangesAsync(cancellationToken);

            return this.Ok();
        }
    }
}