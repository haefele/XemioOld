using UwCore.Services.ApplicationState;
using Xemio.Apps.Windows.Services.Auth;
using AS = UwCore.Services.ApplicationState.ApplicationState;

namespace Xemio.Apps.Windows.Services.ApplicationState
{
    public static class ApplicationStateServiceExtensions
    {
        public static User GetCurrentUser(this IApplicationStateService self)
        {
            return self.Get<User>("CurrentUser", AS.Local);
        }
        public static void SetCurrentUser(this IApplicationStateService self, User currentUser)
        {
            self.Set("CurrentUser", currentUser, AS.Local);
        }
    }
}