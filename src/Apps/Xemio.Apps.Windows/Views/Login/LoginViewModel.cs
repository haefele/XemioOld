using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Caliburn.Micro;
using ReactiveUI;
using UwCore;
using UwCore.Application;
using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Extensions;
using Xemio.Apps.Windows.Services.ApplicationState;
using Xemio.Apps.Windows.Services.Auth;
using Xemio.Apps.Windows.Services.Queries;
using Xemio.Apps.Windows.Services.Queries.Folders;
using Xemio.Apps.Windows.ShellModes;
using Xemio.Client.Notes;
using Xemio.Shared.Models.Notes;

namespace Xemio.Apps.Windows.Views.Login
{
    public class LoginViewModel : UwCoreScreen
    {
        private readonly IAuthService _authService;
        private readonly IApplicationStateService _applicationStateService;
        private readonly IContainer _container;
        private readonly IShell _shell;

        public UwCoreCommand<Unit> Login { get; }

        public LoginViewModel(IAuthService authService, IApplicationStateService applicationStateService, IContainer container, IShell shell)
        {
            this._authService = authService;
            this._applicationStateService = applicationStateService;
            this._container = container;
            this._shell = shell;

            this.Login = UwCoreCommand
                .Create(this.LoginImpl)
                .ShowLoadingOverlay("Login")
                .HandleExceptions();
        }

        private async Task LoginImpl(CancellationToken arg)
        {
            var user = await this._authService.LoginAsync();

            this._applicationStateService.SetCurrentUser(user);
            await this._applicationStateService.SaveStateAsync();

            this._shell.CurrentMode = IoC.Get<LoggedInShellMode>();
        }

        protected override async void OnActivate()
        {
            base.OnActivate();
        }
    }
}