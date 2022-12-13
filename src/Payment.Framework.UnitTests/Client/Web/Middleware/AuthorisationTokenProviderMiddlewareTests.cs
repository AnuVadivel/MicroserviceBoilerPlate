using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Payment.Framework.Client.Web.Middleware;
using Payment.Framework.Client.Web.Models;
using Xunit;

namespace Payment.Framework.UnitTests.Client.Web.Middleware
{
    public class AuthorisationTokenProviderMiddlewareTests
    {
        private readonly DefaultHttpContext _httpContext;
        private readonly Mock<RequestDelegate> _delegateMock;
        private readonly AuthorisationTokenProvider _authTokenProvider;
        private readonly AuthorisationTokenProviderMiddleware _authMiddleware;

        public AuthorisationTokenProviderMiddlewareTests()
        {
            _httpContext = new DefaultHttpContext();
            _delegateMock = new Mock<RequestDelegate>();
            _authTokenProvider = new AuthorisationTokenProvider();
            _authMiddleware = new AuthorisationTokenProviderMiddleware(_delegateMock.Object);
        }

        [Fact]
        public void ShouldSetsUserAndCallsNextDelegate()
        {
            _httpContext.Request.Headers["Authorization"] = "Auth-header";

            _authMiddleware.InvokeAsync(_httpContext, _authTokenProvider).Wait();
            
            _authTokenProvider.Token.Should().Be("Auth-header");
            _delegateMock.Verify(f => f(_httpContext), Times.Once);
        }
    }
}