using Microsoft.EntityFrameworkCore.ChangeTracking;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Extensions
{

    public static class EntityEntryExtensions
    {
        public static void ETagForConcurrencyControlIs<TEntity>(this EntityEntry<TEntity> self, byte[] etag)
            where TEntity : class, IConcurrencyControlledEntity
        {
            self.OriginalValues[nameof(IConcurrencyControlledEntity.ETag)] = etag;
        }
    }
}
