using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using UwCore.Common;

namespace Xemio.Apps.Windows.Services.Queries
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IContainer _container;
        private readonly IQueryCache _queryCache;

        public QueryExecutor(IContainer container, IQueryCache queryCache)
        {
            Guard.NotNull(container, nameof(container));
            Guard.NotNull(queryCache, nameof(queryCache));

            this._container = container;
            this._queryCache = queryCache;
        }

        public async Task<QueryResult<TResult>> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            Guard.NotNull(query, nameof(query));

            try
            {
                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
                var handler = this._container.Resolve(handlerType);
                var method = handler.GetType().GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.ExecuteAsync));

                TResult result = await (Task<TResult>)method.Invoke(handler, new object[] { query });

                await this._queryCache.CacheQueryAsync(query, result);

                return new QueryResult<TResult>(false, result);
            }
            catch
            {
                TResult result;
                if (await this._queryCache.TryGetCachedQueryAsync(query, out result))
                {
                    return new QueryResult<TResult>(true, result);
                }

                throw;
            }
        }
    }
}