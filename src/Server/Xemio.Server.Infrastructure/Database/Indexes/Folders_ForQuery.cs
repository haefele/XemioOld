using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Xemio.Server.Infrastructure.Controllers.Notes;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Database.Indexes
{
    public class Folders_ForQuery : AbstractIndexCreationTask<Folder>
    {
        public Folders_ForQuery()
        {
            this.Map = folders =>
                from folder in folders
                select new
                {
                    folder.UserId,
                    folder.ParentFolderId
                };
            
            this.Index(f => f.UserId, FieldIndexing.NotAnalyzed);
            this.Index(f => f.ParentFolderId, FieldIndexing.NotAnalyzed);
        }
    }
}