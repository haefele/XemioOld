using System.Threading.Tasks;
using UwCore.Services.ApplicationState;
using AS = UwCore.Services.ApplicationState.ApplicationState;

namespace Xemio.Apps.Windows.Services.Queries
{
    public class QueryCache : IQueryCache
    {
        private readonly IApplicationStateService _applicationStateService;

        public QueryCache(IApplicationStateService applicationStateService)
        {
            this._applicationStateService = applicationStateService.GetStateServiceFor(typeof(QueryCache));
        }

        public Task<bool> TryGetCachedQueryAsync<TResult>(IQuery<TResult> query, out TResult result)
        {
            string key = this.GetKey(query);

            if (this._applicationStateService.HasValueFor(key, AS.Local) == false)
            {
                result = default(TResult);   
                return Task.FromResult(false);
            }

            result = this._applicationStateService.Get<TResult>(key, AS.Local);
            return Task.FromResult(true);
        }

        public Task CacheQueryAsync<TResult>(IQuery<TResult> query, TResult data)
        {
            var key = this.GetKey(query);

            this._applicationStateService.Set(key, data, AS.Local);
            return Task.CompletedTask;
        }

        private string GetKey<TResult>(IQuery<TResult> query)
        {
            return $"{query.GetType().Name}:{query.GetHashCode()}";
        }
    }
}