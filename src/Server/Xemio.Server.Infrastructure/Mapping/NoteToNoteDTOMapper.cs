using System.Threading.Tasks;
using EnsureThat;
using Xemio.Server.Contracts.Mapping;
using Xemio.Server.Infrastructure.Entities.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Mapping
{
    public class NoteToNoteDTOMapper : MapperBase<Note, NoteDTO>
    {
        public override Task<NoteDTO> MapAsync(Note input)
        {
            throw new System.NotImplementedException();
        }
    }
}