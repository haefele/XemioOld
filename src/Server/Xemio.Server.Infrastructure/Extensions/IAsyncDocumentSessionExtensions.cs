using Raven.Client;

namespace Xemio.Server.Infrastructure.Extensions
{
    public static class IAsyncDocumentSessionExtensions
    {
        internal static long ToLongId(this IAsyncDocumentSession self, string id)
        {
            return long.Parse(self.Advanced.DocumentStore.Conventions.FindIdValuePartForValueTypeConversion(null, id));
        }
    }
}