using System.Threading.Tasks;

namespace Xemio.Apps.Windows.Services.Auth
{
    public interface IAuthService
    {
        Task<User> LoginAsync();
        Task RefreshUserAsync(User user);
    }

    public class User
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
