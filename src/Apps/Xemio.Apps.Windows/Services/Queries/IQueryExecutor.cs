using System.Threading.Tasks;

namespace Xemio.Apps.Windows.Services.Queries
{
    public interface IQueryExecutor
    {
        Task<QueryResult<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}