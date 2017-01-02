using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Caliburn.Micro;
using UwCore.Application;
using UwCore.Common;
using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Extensions;
using Xemio.Apps.Windows.Services.ApplicationState;
using Xemio.Apps.Windows.Services.Auth;
using Xemio.Apps.Windows.ShellModes;
using Xemio.Client.Errors;
using Xemio.Apps.Windows.Errors;

namespace Xemio.Apps.Windows.Services.Queries
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IContainer _container;
        private readonly IQueryCache _queryCache;
        private readonly IAuthService _authService;
        private readonly IApplicationStateService _applicationStateService;
        private readonly IShell _shell;

        public QueryExecutor(IContainer container, IQueryCache queryCache, IAuthService authService, IApplicationStateService applicationStateService, IShell shell)
        {
            Guard.NotNull(container, nameof(container));
            Guard.NotNull(queryCache, nameof(queryCache));
            Guard.NotNull(authService, nameof(authService));
            Guard.NotNull(applicationStateService, nameof(applicationStateService));
            Guard.NotNull(shell, nameof(shell));

            this._container = container;
            this._queryCache = queryCache;
            this._authService = authService;
            this._applicationStateService = applicationStateService;
            this._shell = shell;
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
            catch (UnauthorizedException)
            {
                var currentUser = this._applicationStateService.GetCurrentUser();

                if (currentUser != null)
                {
                    await this._authService.RefreshUserAsync(currentUser);
                    this._applicationStateService.SetCurrentUser(currentUser);

                    this._container.UpdateClients(currentUser);

                    return await this.ExecuteAsync(query);
                }

                //Automatically log the user out
                this._shell.CurrentMode = IoC.Get<LoggedOutShellMode>();

                throw new NoLongerLoggedInException("You are no longer logged in.");
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