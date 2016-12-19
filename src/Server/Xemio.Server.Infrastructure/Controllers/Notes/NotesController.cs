using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Annotations;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Database;
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
        }

        private readonly XemioContext _xemioContext;
        private readonly IMapper<Note, NoteDTO> _noteToNoteDTOMapper;

        public NotesController(XemioContext xemioContext, IMapper<Note, NoteDTO> noteToNoteDTOMapper)
        {
            EnsureArg.IsNotNull(xemioContext, nameof(xemioContext));
            EnsureArg.IsNotNull(noteToNoteDTOMapper, nameof(noteToNoteDTOMapper));

            this._xemioContext = xemioContext;
            this._noteToNoteDTOMapper = noteToNoteDTOMapper;
        }

        [HttpGet("{noteId:guid}", Name = RouteNames.GetNoteById)]
        [Description("Get the specified note.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(NoteDTO), Description = "The note.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(void), Description = "The note does not exist.")]
        public async Task<IActionResult> GetNoteAsync([Required]Guid? noteId)
        {
            var note = await this._xemioContext.FindAsync<Note>(noteId);

            if (note == null || note.UserId != this.User.Identity.Name)
                return this.NotFound();

            var noteDTO = await this._noteToNoteDTOMapper.MapAsync(note);
            return this.Ok(noteDTO);
        }
        
        [HttpPost(Name = RouteNames.CreateNote)]
        [Description("Create a new note.")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(NoteDTO), Description = "The note was created.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(void), Description = "Some parameters are incorrect.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(void), Description = "The folder does not exist.")]
        public async Task<IActionResult> PostNoteAsync([FromBody][Required]CreateNote createNote)
        {
            var folder = await this._xemioContext.FindAsync<Folder>(createNote.FolderId);

            if (folder == null || folder.UserId != this.User.Identity.Name)
                return this.NotFound();

            var note = new Note
            {
                Title = createNote.Title,
                Content = createNote.Content,
                UserId = this.User.Identity.Name,
                Folder = folder,
            };

            await this._xemioContext.Notes.AddAsync(note);

            await this._xemioContext.SaveChangesAsync();

            var noteDTO = await this._noteToNoteDTOMapper.MapAsync(note);
            return this.CreatedAtRoute(RouteNames.GetNoteById, new { noteId = note.Id }, noteDTO);
        }
    }
}