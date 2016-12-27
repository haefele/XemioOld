using System.Threading.Tasks;

namespace Xemio.Apps.Windows.Services.Auth
{
    public interface IAuthService
    {
        Task<User> LoginAsync();
        Task RefreshUserAsync(User user);
    }
}
