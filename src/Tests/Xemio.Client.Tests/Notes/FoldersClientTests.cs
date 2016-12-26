using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NSubstitute;
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
        public void Constructor_Creates_HttpClient_With_BaseAddress_And_Authorization_Header()
        {
            var client = new FoldersClient(BearerToken);

            Assert.NotNull(client.HttpClient);
            Assert.Equal(new Uri(ClientBase.ApiBaseAddress), client.HttpClient.BaseAddress);
            Assert.NotNull(client.HttpClient.DefaultRequestHeaders.Authorization);
            Assert.Equal(ClientBase.AuthorizationScheme, client.HttpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(BearerToken, client.HttpClient.DefaultRequestHeaders.Authorization.Parameter);
        }

        [Fact]
        public async Task GetRootFoldersAsync_Sends_HTTP_Request()
        {
            //Arrange
            var handler = Substitute.For<MockHttpMessageHandler>();
            handler.SendAsyncMocked(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()).Returns(new HttpResponseMessage());
            var client = new FoldersClient(BearerToken, handler);

            //Act
            await client.GetRootFoldersAsync(CancellationToken.None);

            //Assert
            handler.SendAsyncMocked(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()).ReceivedWithAnyArgs(1);
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return this.SendAsyncMocked(request, cancellationToken);
        }

        public virtual Task<HttpResponseMessage> SendAsyncMocked(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }
}