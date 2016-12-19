using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xemio.Server.Infrastructure.Database;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Controllers
{
    [Route("debug")]
    public class DebugController : ControllerBase
    {
        private readonly XemioContext _xemioContext;

        public DebugController(XemioContext xemioContext)
        {
            this._xemioContext = xemioContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var note = new Note
            {
                UserId = "auth0|58318a1b9ca666c66c96d2c6",
                Title = "Erste Notiz!",
                Folder = await this._xemioContext.Folders.FindAsync(Guid.Parse("D002CC11-D7A3-42D7-9047-08D417D78446")),
                Content = "Inhalt hier!",
            };
            await this._xemioContext.Notes.AddAsync(note);

            await this._xemioContext.SaveChangesAsync();

            return this.Ok(note);
        }
    }
}