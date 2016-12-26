using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Newtonsoft.Json;
using Xemio.Client.Errors;

namespace Xemio.Client
{
    public abstract class ClientBase : IDisposable
    {
        public const string ApiBaseAddress = "http://localhost:9000";
        public const string AuthorizationScheme = "Bearer";

        public HttpClient HttpClient { get; }
        public string BearerToken { get; }
        
        protected ClientBase(string bearerToken, HttpMessageHandler httpMessageHandler = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(bearerToken, nameof(bearerToken));
            
            this.HttpClient = new HttpClient(httpMessageHandler ?? new HttpClientHandler());
            this.HttpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            this.HttpClient.BaseAddress = new Uri(ApiBaseAddress);
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, bearerToken);

            this.BearerToken = bearerToken;
        }

        public void Dispose()
        {
            this.HttpClient.Dispose();
        }

        protected Task<T> GetAsync<T>(string path, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.GetAsync<T>(path, null, cancellationToken, expectedStatusCode);
        }
        protected Task<T> GetAsync<T>(string path, IDictionary<string, string> query, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.SendAsync<T>(HttpMethod.Get, path, query, null, cancellationToken, expectedStatusCode);
        }

        protected Task<T> PostAsync<T>(string path, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.PostAsync<T>(path, null, data, cancellationToken, expectedStatusCode);
        }
        protected Task<T> PostAsync<T>(string path, IDictionary<string, string> query, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.SendAsync<T>(HttpMethod.Post, path, query, data, cancellationToken, expectedStatusCode);
        }

        protected Task<T> PatchAsync<T>(string path, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.PatchAsync<T>(path, null, data, cancellationToken, expectedStatusCode);
        }
        protected Task<T> PatchAsync<T>(string path, IDictionary<string, string> query, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.SendAsync<T>(new HttpMethod("PATCH"), path, query, data, cancellationToken, expectedStatusCode);
        }

        protected Task<T> DeleteAsync<T>(string path, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.DeleteAsync<T>(path, null, data, cancellationToken, expectedStatusCode);
        }
        protected Task<T> DeleteAsync<T>(string path, IDictionary<string, string> query, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            return this.SendAsync<T>(HttpMethod.Delete, path, query, data, cancellationToken, expectedStatusCode);
        }

        private async Task<T> SendAsync<T>(HttpMethod method, string path, IDictionary<string, string> query, object data, CancellationToken cancellationToken, HttpStatusCode expectedStatusCode)
        {
            if (query != null)
                path += QueryString.Build(query);

            var requestMessage = new HttpRequestMessage(method, path);

            if (data != null)
            {
                string serializedData = JsonConvert.SerializeObject(data);
                var content = new StringContent(serializedData, Encoding.UTF8, "application/json");
                requestMessage.Content = content;
            }

            var response = await this.HttpClient.SendAsync(requestMessage, cancellationToken);

            this.ValidateResponse(response, expectedStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
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
                throw new Exception($"Shit is going dooooown! Response status code is: {responseMessage.StatusCode}, Expected: {expectedStatusCode}");
        }
    }
}