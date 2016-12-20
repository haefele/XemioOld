using System;
using System.Threading.Tasks;
using Xemio.Client.Notes;
using Xunit;

namespace Xemio.Client.Tests.Notes
{
    public class FoldersClientTests
    {
        public const string BearerToken = "A.B.C";

        [Fact]
        public void Constructor_Throws_NullReferenceException_If_Bearer_Token_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FoldersClient(null);
            });
        }

        [Fact]
        public void Constructor_Throws_ArgumentException_If_Bearer_Token_Is_Empty()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new FoldersClient(string.Empty);
            });
        }

        [Fact]
        public void Constructor_Throws_ArgumentException_If_Bearer_Token_Is_Whitespace()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new FoldersClient("  ");
            });
        }

        [Fact]
        public void Constructor_Creates_HttpClient()
        {
            var client = new FoldersClient(BearerToken);

            Assert.NotNull(client.HttpClient);
            Assert.Equal(new Uri(ClientBase.ApiBaseAddress + FoldersClient.ApiEndpoint), client.HttpClient.BaseAddress);
            Assert.NotNull(client.HttpClient.DefaultRequestHeaders.Authorization);
            Assert.Equal(ClientBase.AuthorizationScheme, client.HttpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(BearerToken, client.HttpClient.DefaultRequestHeaders.Authorization.Parameter);
        }
    }
}