using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Autofac;
using Microsoft.Graphics.Canvas.Text;
using UwCore.Application;
using UwCore.Hamburger;
using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Extensions;
using Xemio.Apps.Windows.Services.ApplicationState;
using Xemio.Apps.Windows.Views.Login;
using Xemio.Apps.Windows.Views.Notes;

namespace Xemio.Apps.Windows.ShellModes
{
    public class LoggedOutShellMode : ShellMode
    {
        public NavigatingHamburgerItem LoginHamburgerItem { get; }

        public LoggedOutShellMode()
        {
            this.LoginHamburgerItem = new NavigatingHamburgerItem("Login", Symbol.Link, typeof(LoginViewModel));
        }

        protected override async Task OnEnter()
        {
            await base.OnEnter();

            this.LoginHamburgerItem.Execute();
        }

        protected override async Task AddActions()
        {
            await base.AddActions();

            this.Shell.Actions.Add(this.LoginHamburgerItem);
        }

        protected override async Task RemoveActions()
        {
            await base.RemoveActions();

            this.Shell.Actions.Remove(this.LoginHamburgerItem);
        }
    }

    public class LoggedInShellMode : ShellMode
    {
        private readonly IContainer _container;
        private readonly IApplicationStateService _applicationStateService;

        public HamburgerItem NotesHamburgerItem { get; }

        public LoggedInShellMode(IContainer container, IApplicationStateService applicationStateService)
        {
            this._container = container;
            this._applicationStateService = applicationStateService;

            this.NotesHamburgerItem = new NavigatingHamburgerItem("Notes", Symbol.Caption, typeof(NotesViewModel));
        }

        protected override async Task OnEnter()
        {
            await base.OnEnter();

            var currentUser = this._applicationStateService.GetCurrentUser();
            this._container.UpdateClients(currentUser);

            this.NotesHamburgerItem.Execute();
        }

        protected override async Task AddActions()
        {
            await base.AddActions();

            this.Shell.Actions.Add(this.NotesHamburgerItem);
        }

        protected override async Task RemoveActions()
        {
            await base.RemoveActions();

            this.Shell.Actions.Remove(this.NotesHamburgerItem);
        }
    }
}