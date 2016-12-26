using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UwCore.Common;
using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Services.ApplicationState;
using Xemio.Client.Errors;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Services.Queries.Folders
{
    public class GetRootFoldersQuery : IQuery<IList<FolderDTO>>, IEquatable<GetRootFoldersQuery>
    {
        #region Equality
        public bool Equals(GetRootFoldersQuery other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((GetRootFoldersQuery) obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(GetRootFoldersQuery left, GetRootFoldersQuery right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetRootFoldersQuery left, GetRootFoldersQuery right)
        {
            return !Equals(left, right);
        }
        #endregion
    }

    public class GetRootFoldersQueryHandler : IQueryHandler<GetRootFoldersQuery, IList<FolderDTO>>
    {
        private readonly IFoldersClient _foldersClient;

        public GetRootFoldersQueryHandler(IFoldersClient foldersClient)
        {
            Guard.NotNull(foldersClient, nameof(foldersClient));

            this._foldersClient = foldersClient;
        }

        public Task<IList<FolderDTO>> ExecuteAsync(GetRootFoldersQuery query)
        {
            return this._foldersClient.GetRootFoldersAsync();
        }
    }
}