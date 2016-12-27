using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Autofac;
using Caliburn.Micro;
using UwCore.Application;
using UwCore.Hamburger;
using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Extensions;
using Xemio.Apps.Windows.Services.ApplicationState;
using Xemio.Apps.Windows.Views.Notes;

namespace Xemio.Apps.Windows.ShellModes
{
    public class LoggedInShellMode : ShellMode
    {
        private readonly IContainer _container;
        private readonly IApplicationStateService _applicationStateService;

        public HamburgerItem NotesHamburgerItem { get; }
        public HamburgerItem LogoutHamburgerItem { get; }

        public LoggedInShellMode(IContainer container, IApplicationStateService applicationStateService)
        {
            this._container = container;
            this._applicationStateService = applicationStateService;

            this.NotesHamburgerItem = new NavigatingHamburgerItem("Notes", Symbol.Caption, typeof(NotesViewModel));
            this.LogoutHamburgerItem = new ClickableHamburgerItem("Logout", Symbol.Admin, this.Logout);
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
            this.Shell.SecondaryActions.Add(this.LogoutHamburgerItem);
        }

        protected override async Task RemoveActions()
        {
            await base.RemoveActions();

            this.Shell.Actions.Remove(this.NotesHamburgerItem);
            this.Shell.SecondaryActions.Remove(this.LogoutHamburgerItem);
        }
        
        private async void Logout()
        {
            this._applicationStateService.SetCurrentUser(null);
            await this._applicationStateService.SaveStateAsync();

            this._container.UpdateClients(null);

            this.Shell.CurrentMode = IoC.Get<LoggedOutShellMode>();
        }
    }
}