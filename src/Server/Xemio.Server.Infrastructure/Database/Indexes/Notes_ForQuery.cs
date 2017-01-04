using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Database.Indexes
{
    public class Notes_ForQuery : AbstractIndexCreationTask<Note>
    {
        public Notes_ForQuery()
        {
            this.Map = notes =>
                from note in notes
                select new
                {
                    note.UserId,
                    note.FolderId,
                };

            this.Index(f => f.UserId, FieldIndexing.NotAnalyzed);
            this.Index(f => f.FolderId, FieldIndexing.NotAnalyzed);
        }
    }
}