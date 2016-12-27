using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Text;
using UwCore.Application;
using UwCore.Hamburger;
using Xemio.Apps.Windows.Views.Login;

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
}