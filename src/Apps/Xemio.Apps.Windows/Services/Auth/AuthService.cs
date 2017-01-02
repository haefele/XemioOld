using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Newtonsoft.Json.Linq;
using Xemio.Client;

namespace Xemio.Apps.Windows.Services.Auth
{
    public class AuthService : IAuthService
    {
        private const string Domain = "haefele.eu.auth0.com";
        private const string ClientId = "TtPzHCk2Q4683MfM0ECxHdwA264cH28s";
        private const string CallbackUrl = "https://" + Domain + "/mobile";
        private const string AuthorizeUrl = "https://" + Domain + "/authorize";
        private const string DelegationUrl = "https://" + Domain + "/delegation";
        private const string UserInfoUrl = "https://" + Domain + "/userinfo";
        
        public async Task<User> LoginAsync()
        {
            var state = this.NewState();
            var device = this.GetDeviceName();

            var query = new Dictionary<string, string>
            {
                ["client_id"] = ClientId,
                ["redirect_uri"] = CallbackUrl,
                ["response_type"] = "token",
                ["connection"] = "windowslive",
                ["scope"] = "openid offline_access",
                ["state"] = state,
                ["device"] = device
            };

            var startUrl = new Uri(AuthorizeUrl + QueryString.Build(query));
            var endUrl = new Uri(CallbackUrl);

            var authResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUrl, endUrl);

            if (authResult.ResponseStatus != WebAuthenticationStatus.Success)
                return null;

            var parsed = this.ParseAuthResult(authResult.ResponseData);

            if (parsed["state"] != state)
                return null;

            var user = new User
            {
                AccessToken = parsed["access_token"],
                IdToken = parsed["id_token"],
                RefreshToken = parsed["refresh_token"],
            };

            await this.FillUserWithAdditionalData(user);

            return user;
        }

        public async Task RefreshUserAsync(User user)
        {
            var options = new Dictionary<string, string>
            {
                ["refresh_token"] = user.RefreshToken,
                ["client_id"] = ClientId,
                ["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                ["api_type"] = "app"
            };
            
            var response = await new HttpClient().PostAsync(DelegationUrl, new FormUrlEncodedContent(options));
            var responseContent = await response.Content.ReadAsStringAsync();

            var responseJson = JObject.Parse(responseContent);
            user.IdToken = responseJson.Value<string>("id_token");

            await this.FillUserWithAdditionalData(user);
        }

        private async Task FillUserWithAdditionalData(User user)
        {
            var query = new Dictionary<string, string>
            {
                ["access_token"] = user.AccessToken,
            };

            var response = await new HttpClient().GetAsync(UserInfoUrl + QueryString.Build(query));
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JObject.Parse(responseContent);
                user.Email = responseJson.Value<string>("email");
                user.EmailVerified = responseJson.Value<bool>("email_verified");
                user.FirstName = responseJson.Value<string>("given_name");
                user.LastName = responseJson.Value<string>("family_name");
            }
        }

        private string NewState()
        {
            return Guid.NewGuid().ToString("N");
        }

        private string GetDeviceName()
        {
            var eas = new EasClientDeviceInformation();
            return eas.SystemSku;
        }

        private IDictionary<string, string> ParseAuthResult(string endUrl)
        {
            int indexOfHashtag = endUrl.IndexOf("#", StringComparison.OrdinalIgnoreCase);
            var everyThingAfterHashtag =  endUrl.Substring(indexOfHashtag + 1);

            return everyThingAfterHashtag.Split('&')
                .Select(f => f.Split('='))
                .ToDictionary(f => f[0], f => f[1]);
        }
    }
}