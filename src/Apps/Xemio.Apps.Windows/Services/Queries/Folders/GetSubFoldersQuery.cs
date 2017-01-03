using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwCore.Common;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Services.Queries.Folders
{
    public class GetSubFoldersQuery : IQuery<IList<FolderDTO>>, IEquatable<GetSubFoldersQuery>
    {
        public long FolderId { get; }

        public GetSubFoldersQuery(long folderId)
        {
            this.FolderId = folderId;
        }

        #region Equality
        public bool Equals(GetSubFoldersQuery other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.FolderId.Equals(other.FolderId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GetSubFoldersQuery) obj);
        }

        public override int GetHashCode()
        {
            return this.FolderId.GetHashCode();
        }

        public static bool operator ==(GetSubFoldersQuery left, GetSubFoldersQuery right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetSubFoldersQuery left, GetSubFoldersQuery right)
        {
            return !Equals(left, right);
        }
        #endregion
    }

    public class GetSubFoldersQueryHandler : IQueryHandler<GetSubFoldersQuery, IList<FolderDTO>>
    {
        private readonly IFoldersClient _foldersClient;

        public GetSubFoldersQueryHandler(IFoldersClient foldersClient)
        {
            Guard.NotNull(foldersClient, nameof(foldersClient));

            this._foldersClient = foldersClient;
        }

        public Task<IList<FolderDTO>> ExecuteAsync(GetSubFoldersQuery query)
        {
            return this._foldersClient.GetSubFoldersAsync(query.FolderId);
        }
    }
}
