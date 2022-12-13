using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Content;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using Payment.Framework.Client.Web.Contracts;
using Payment.Framework.Client.Web.Exceptions;
using Payment.Framework.Client.Web.Models;
using Payment.Framework.Client.Web.Models.Config;
using Payment.Framework.Shared.Util;
using Xunit;
using WebClient = Payment.Framework.Client.Web.WebClient;

namespace Payment.Framework.UnitTests.Client.Web
{
    public class WebClientTests : IDisposable
    {
        private const string AuthToken = "test-token";
        private const string BaseUrl = "https://www.testurl.com";
        private readonly AuthorisationTokenProvider _authorisationProviderMock;
        private readonly Mock<IFlurlClientFactory> _flurlFactoryMock;
        private readonly HttpTest _httpTest;
        private readonly ILogger<WebClient> _loggerMock;
        private readonly QueryParamCollection _queryParams;
        private readonly Dictionary<string, List<int>> _requestBody;
        private readonly string _somePath;
        private readonly string _somePathToMatch;
        private readonly IWebClient _webClient;

        public WebClientTests()
        {
            _flurlFactoryMock = new Mock<IFlurlClientFactory>();
            _flurlFactoryMock.Setup(factoryMock => factoryMock.Get(It.IsAny<Url>())).Returns(new FlurlClient(BaseUrl));
            _authorisationProviderMock = new AuthorisationTokenProvider { Token = AuthToken };
            _loggerMock = new Mock<ILogger<WebClient>>().Object;
            _httpTest = new HttpTest();
            _somePath = "/some/path";
            _somePathToMatch = "*/some/path";
            _requestBody = new Dictionary<string, List<int>> { { "Param", new List<int> { 2 } } };
            _queryParams = new QueryParamCollection { { "a", 1 } };
            _webClient = SetupWebClient();
        }

        private RequestHeaders CustomHeaders => new RequestHeaders().AddHeader("Key", "Value");

        private RequestHeaders TokenHeaders =>
            new RequestHeaders().AddHeader(HeaderNames.Authorization, "Bearer some-auth-token");

        public void Dispose()
        {
            _httpTest.Dispose();
        }

        [Fact]
        public async Task Post_GivenRequestPathAndBody_ShouldSendRequestAlongWithRequestBody()
        {
            await _webClient.Post<HttpResponse>(_somePath, _requestBody, CustomHeaders);
            _httpTest
                .ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Post)
                .WithHeader("Key", "Value")
                .WithHeader(HeaderNames.Authorization, AuthToken)
                .WithRequestBody(NewtonSoftSerializeUtility.Serialize(_requestBody));
        }

        [Fact]
        public async Task
            Post_GivenRequestBodyAndCustomAuthHeader_ShouldSendRequestAlongWithRequestBodyAndOverrideAuthHeader()
        {
            var customAuthHeader = new RequestHeaders().AddHeader(HeaderNames.Authorization, "CustomAuthToken");
            await _webClient.Post<HttpResponse>(_somePath, _requestBody, customAuthHeader);
            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Post)
                .WithHeader(HeaderNames.Authorization, "CustomAuthToken")
                .WithRequestBody(NewtonSoftSerializeUtility.Serialize(_requestBody));
        }

        [Fact]
        public async Task Post_GivenRequestPathAndBody_ShouldReturnCorrectResponse()
        {
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            var response = await _webClient.Post<HttpResponse>(_somePath, _requestBody);

            response.Should().BeOfType<HttpResponse>();
        }

        [Fact]
        public void Post_GivenAnExceptionOccurred_ShouldThrowException()
        {
            _httpTest.RespondWithJson(null, 400);

            Func<Task> action = async () => await _webClient.Post<HttpResponse>(_somePath, _requestBody);
            action.Should().ThrowAsync<WebClientException>().Result.And.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostUrlEncodedAsync_GivenRequestPathAndBody_ShouldSendRequestAlongWithRequestBody()
        {
            var urlEncodedContent =
                new CapturedUrlEncodedContent(new FlurlRequest().Settings.UrlEncodedSerializer.Serialize(_requestBody));
            await _webClient.PostUrlEncodedAsync<HttpResponse>(_somePath, _requestBody);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Post)
                .WithRequestBody(urlEncodedContent.Content);
        }

        [Fact]
        public async Task PostUrlEncodedAsync_GivenRequestPathAndBody_ShouldReturnCorrectResponse()
        {
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            var response = await _webClient.PostUrlEncodedAsync<HttpResponse>(_somePath, _requestBody);

            response.Should().BeOfType<HttpResponse>();
        }

        [Fact]
        public void PostUrlEncodedAsync_GivenAnExceptionOccurred_ShouldThrowException()
        {
            _httpTest.RespondWithJson(null, 400);

            Func<Task> action = async () => await _webClient.PostUrlEncodedAsync<HttpResponse>(_somePath, _requestBody);
            action.Should().ThrowAsync<WebClientException>().Result.And.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_GivenRequestPathAndNoQueryParams_ShouldSendRequestAndReceiveJsonResponse()
        {
            await _webClient.Get<HttpResponse>(_somePath, null, CustomHeaders);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Get)
                .WithHeader("Key", "Value")
                .WithHeader(HeaderNames.Authorization, AuthToken);
        }

        [Fact]
        public async Task Get_GivenRequestPathAndQueryParams_ShouldSendRequestAndReceiveJsonResponse()
        {
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            var response = await _webClient.Get<HttpResponse>(_somePath, _queryParams);

            response.Should().BeOfType<HttpResponse>();
        }

        [Fact]
        public void Get_GivenAnExceptionOccurred_ShouldThrowException()
        {
            _httpTest.RespondWithJson(null, 500);

            Func<Task> action = async () => await _webClient.Post<HttpResponse>(_somePath, _requestBody);
            action.Should().ThrowAsync<WebClientException>().Result.And.StatusCode.Should()
                .Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Get_GivenAuthToken_ShouldAddAuthorizationHeaderWithAuthTokenValue()
        {
            _httpTest.RespondWith("test-string");

            await _webClient.Get(_somePath, _queryParams, TokenHeaders);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Get).WithOAuthBearerToken("some-auth-token");
        }

        [Fact]
        public async Task Get_GivenValidRequest_ShouldReturnStringResponse()
        {
            _httpTest.RespondWith("test-string");

            var response = await _webClient.Get(_somePath, _queryParams, TokenHeaders);

            response.Should().BeOfType<string>();
            response.Should().BeEquivalentTo("test-string");
        }

        [Fact]
        public async Task GetResponseStream_GivenPathAndQueryString_ShouldReturnStream()
        {
            _httpTest.RespondWith("test body");
            var (stream, contentType) =
                await _webClient.GetResponseStream(_somePath, _queryParams, CustomHeaders);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Get)
                .WithHeader("Key", "Value")
                .WithHeader(HeaderNames.Authorization, AuthToken);
            contentType.Should().Be("text/plain; charset=utf-8");

            stream.Should().NotBeNull();
            (await new StreamReader(stream).ReadToEndAsync()).Should().Be("test body");
        }

        [Fact]
        public void GetResponseStream_GivenAnExceptionIsThrown_ShouldThrowException()
        {
            _httpTest.RespondWith("", 400);

            Func<Task> action = async () =>
                await _webClient.GetResponseStream(_somePath, _queryParams);

            action.Should().ThrowAsync<WebClientException>().Result.And.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_GivenRequestPathAndBody_ShouldSendRequestAlongWithRequestBody()
        {
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());
            await _webClient.Put<HttpResponse>(_somePath, _requestBody, CustomHeaders);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Put)
                .WithHeader("Key", "Value")
                .WithHeader(HeaderNames.Authorization, AuthToken)
                .WithRequestBody(NewtonSoftSerializeUtility.Serialize(_requestBody));
        }

        [Fact]
        public async Task Put_GivenRequestPathAndBody_ShouldReturnCorrectResponse()
        {
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            var response = await _webClient.Put<HttpResponse>(_somePath, _requestBody);

            response.Should().BeOfType<HttpResponse>();
        }

        [Fact]
        public void Put_GivenAnExceptionIsThrown_ShouldThrowException()
        {
            _httpTest.RespondWithJson("", 400);

            Func<Task> action = async () => await _webClient.Put<HttpResponse>(_somePath, _requestBody);

            action.Should().ThrowAsync<WebClientException>().Result.And.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_GivenRequestPathAndNoQueryParams_ShouldSendRequestAndReceiveJsonResponse()
        {
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            var response = await _webClient.Delete<HttpResponse>(_somePath, null, CustomHeaders);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Delete)
                .WithHeader("Key", "Value")
                .WithHeader(HeaderNames.Authorization, AuthToken);
            response.Should().BeOfType<HttpResponse>();
        }

        [Fact]
        public void Delete_GivenAnExceptionOccured_ShouldThrowException()
        {
            _httpTest.RespondWithJson(HttpStatusCode.Forbidden);

            Func<Task> action = async () => await _webClient.Delete<HttpResponse>(_somePath, null);

            action.Should().ThrowAsync<WebClientException>();
        }

        [Fact]
        public async Task ShouldUseCustomAuthToken()
        {
            const string customAuthToken = "custom-token";
            var webClient = SetupWebClient(customAuthToken);
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            var response = await webClient.Get<HttpResponse>(_somePath, _queryParams);

            _httpTest.ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Get).WithHeader(HeaderNames.Authorization, customAuthToken);
            response.Should().BeOfType<HttpResponse>();
        }

        [Fact]
        public async Task ShouldNotSetAuthorizationHeaderWhenAuthTokenIsNull()
        {
            var authTokenProvider = new AuthorisationTokenProvider();
            var webClient = new WebClient(_flurlFactoryMock.Object, authTokenProvider, _loggerMock,
                new WebClientConfig
                {
                    BaseUrl = BaseUrl,
                    AuthToken = null
                });
            _httpTest.RespondWithJson(HttpResponseFaker.Generator.Generate());

            await webClient.Get<HttpResponse>(_somePath, _queryParams);
            _httpTest
                .ShouldHaveCalled(_somePathToMatch)
                .WithVerb(HttpMethod.Get)
                .WithoutHeader(HeaderNames.Authorization);
        }

        private IWebClient SetupWebClient(string authToken = null)
        {
            var webClient = new WebClient(_flurlFactoryMock.Object, _authorisationProviderMock, _loggerMock,
                new WebClientConfig
                {
                    BaseUrl = BaseUrl,
                    AuthToken = authToken
                });
            return webClient;
        }
    }
}