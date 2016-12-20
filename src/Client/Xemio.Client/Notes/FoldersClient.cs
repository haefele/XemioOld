using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Newtonsoft.Json;
using Xemio.Client.Errors;
using Xemio.Shared.Models.Notes;

namespace Xemio.Client.Notes
{
    public class FoldersClient : ClientBase
    {
        public const string ApiEndpoint = "/notes/folders";

        public FoldersClient(string bearerToken, HttpMessageHandler httpMessageHandler = null)
            : base(ApiEndpoint, bearerToken, httpMessageHandler)
        {
        }
        
        public Task<IList<FolderDTO>> GetRootFoldersAsync(CancellationToken cancellationToken)
        {
            return base.GetAsync<IList<FolderDTO>>(string.Empty, cancellationToken);
        }
    }

    public abstract class ClientBase : IDisposable
    {
        public const string ApiBaseAddress = "http://localhost:9000";
        public const string AuthorizationScheme = "Bearer";

        public HttpClient HttpClient { get; }
        public string BearerToken { get; }
        
        protected ClientBase(string baseEndpoint, string bearerToken, HttpMessageHandler httpMessageHandler = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(baseEndpoint, nameof(baseEndpoint));
            EnsureArg.IsNotNullOrWhiteSpace(bearerToken, nameof(bearerToken));
            
            this.HttpClient = new HttpClient(httpMessageHandler ?? new HttpClientHandler());
            this.HttpClient.BaseAddress = new Uri(ApiBaseAddress + baseEndpoint);
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, bearerToken);

            this.BearerToken = bearerToken;
        }

        public void Dispose()
        {
        }
        
        internal async Task<T> GetAsync<T>(string path, CancellationToken cancellationToken)
        {
            var response = await this.HttpClient.GetAsync(path, cancellationToken);

            this.ValidateResponse(response, HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        private void ValidateResponse(HttpResponseMessage responseMessage, HttpStatusCode expectedStatusCode)
        {
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException("The request is not authorized to access this data.");

            if (responseMessage.StatusCode == HttpStatusCode.Conflict)
                throw new ConflictException("The data changed in the meantime.");

            if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                throw new BadRequestException("oopsie");

            if (responseMessage.StatusCode != expectedStatusCode)
                throw new Exception("Shit is going dooooown!");
        }
    }
}