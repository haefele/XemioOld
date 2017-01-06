using System;
using System.Collections.Generic;
using Autofac;
using Caliburn.Micro;
using UwCore.Application;
using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Services.ApplicationState;
using Xemio.Apps.Windows.Services.Auth;
using Xemio.Apps.Windows.Services.Commands;
using Xemio.Apps.Windows.Services.Commands.Folders;
using Xemio.Apps.Windows.Services.Queries;
using Xemio.Apps.Windows.Services.Queries.Folders;
using Xemio.Apps.Windows.ShellModes;
using Xemio.Apps.Windows.ViewParts.NoteFolderHierarchy;
using Xemio.Apps.Windows.Views.Login;
using Xemio.Apps.Windows.Views.Notes;
using Xemio.Client.Errors;

namespace Xemio.Apps.Windows
{
    sealed partial class App
    {
        public override ShellMode GetCurrentMode()
        {
            var currentUser = IoC.Get<IApplicationStateService>().GetCurrentUser();

            return currentUser != null
                ? (ShellMode)IoC.Get<LoggedInShellMode>()
                : IoC.Get<LoggedOutShellMode>();
        }

        public override string GetErrorTitle() => "Error";

        public override string GetErrorMessage() => "An error occured.";

        public override Type GetCommonExceptionType() => typeof(XemioException);

        public override IEnumerable<Type> GetServiceTypes()
        {
            yield return typeof(IAuthService);
            yield return typeof(AuthService);

            yield return typeof(IQueryCache);
            yield return typeof(QueryCache);

            yield return typeof(IQueryExecutor);
            yield return typeof(QueryExecutor);

            yield return typeof(ICommandQueue);
            yield return typeof(CommandQueue);
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            //Queries
            builder.RegisterType<GetRootFoldersQueryHandler>().AsImplementedInterfaces();
            builder.RegisterType<GetSubFoldersQueryHandler>().AsImplementedInterfaces();

            //Commands
            builder.RegisterType<CreateFolderCommandHandler>().AsImplementedInterfaces();
        }

        public override IEnumerable<Type> GetViewModelTypes()
        {
            yield return typeof(LoginViewModel);
            yield return typeof(NotesViewModel);

            yield return typeof(NoteFolderHierarchyViewModel);
        }

        public override IEnumerable<Type> GetShellModeTypes()
        {
            yield return typeof(LoggedOutShellMode);
            yield return typeof(LoggedInShellMode);
        }
    }
}
