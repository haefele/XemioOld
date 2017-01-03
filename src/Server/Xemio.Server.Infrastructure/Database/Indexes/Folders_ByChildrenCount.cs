using System;
using System.Linq;
using Raven.Client.Indexes;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Database.Indexes
{
    public class Folders_ByChildrenCount : AbstractMultiMapIndexCreationTask<Folders_ByChildrenCount.Result>
    {
        public class Result
        {
            public string FolderId { get; set; }
            public int SubFolderCount { get; set; }
            public int NotesCount { get; set; }
        }

        public Folders_ByChildrenCount()
        {
            this.AddMap<Folder>(folders => from folder in folders
                where folder.ParentFolderId != null
                select new
                {
                    FolderId = folder.ParentFolderId,
                    SubFolderCount = 1,
                    NotesCount = 0,
                });

            this.AddMap<Note>(notes => from note in notes
                select new
                {
                    FolderId = note.FolderId,
                    SubFolderCount = 0,
                    NotesCount = 1
                });

            this.Reduce = results => from result in results
                group result by result.FolderId into g
                select new
                {
                    FolderId = g.Key,
                    SubFolderCount = g.Sum(f => f.SubFolderCount),
                    NotesCount = g.Sum(f => f.NotesCount)
                };
        }
    }
}