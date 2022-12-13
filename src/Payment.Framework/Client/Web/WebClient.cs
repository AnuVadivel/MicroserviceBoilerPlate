using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Payment.Framework.Client.Web.Contracts;
using Payment.Framework.Client.Web.Exceptions;
using Payment.Framework.Client.Web.Models;
using Payment.Framework.Client.Web.Models.Config;

namespace Payment.Framework.Client.Web
{
    public class WebClient : IWebClient
    {
        private readonly AuthorisationTokenProvider _authorisationTokenProvider;
        private readonly IFlurlClient _flurlClient;
        private readonly ILogger _logger;
        private readonly WebClientConfig _webClientConfig;

        public WebClient(
            IFlurlClientFactory flurlClientFactory,
            AuthorisationTokenProvider authorisationTokenProvider,
            ILogger<WebClient> logger,
            WebClientConfig webClientConfig)
        {
            _logger = logger;
            _webClientConfig = webClientConfig;
            _flurlClient = flurlClientFactory.Get(Url.Combine(webClientConfig.BaseUrl, "/"));
            _authorisationTokenProvider = authorisationTokenProvider;
        }

        public async Task<T> Get<T>(string path, QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"GET request: {_flurlClient.BaseUrl + path}");
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .WithHeader(HeaderNames.Authorization, GetAuthToken())
                    .WithHeaders(headers?.Headers)
                    .SetQueryParams(queryParams)
                    .GetJsonAsync<T>(cancellationToken);

                return response;
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing GET request {_flurlClient.BaseUrl + path}", httpException);
            }
        }

        public async Task<string> Get(string path, QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"GET request: {_flurlClient.BaseUrl + path}");
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .WithHeader(HeaderNames.Authorization, GetAuthToken())
                    .WithHeaders(headers?.Headers)
                    .SetQueryParams(queryParams)
                    .GetStringAsync(cancellationToken);

                return response;
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing GET request {_flurlClient.BaseUrl + path}",
                    httpException);
            }
        }

        public async Task<T> PostUrlEncodedAsync<T>(string path, object body, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Post request: {_flurlClient.BaseUrl + path}");
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .PostUrlEncodedAsync(body, cancellationToken)
                    .ReceiveJson<T>();

                return response;
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing POST request {_flurlClient.BaseUrl + path}",
                    httpException);
            }
        }

        public async Task<T> Post<T>(string path, object body, RequestHeaders headers = default,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Post request: {_flurlClient.BaseUrl + path}");
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .WithHeader(HeaderNames.Authorization, GetAuthToken())
                    .WithHeaders(headers?.Headers)
                    .PostJsonAsync(body, cancellationToken)
                    .ReceiveJson<T>();

                return response;
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing POST request {_flurlClient.BaseUrl + path}",
                    httpException);
            }
        }

        public async Task<T> Put<T>(string path, object body, RequestHeaders headers = default,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Put request: {_flurlClient.BaseUrl + path}");
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .WithHeader(HeaderNames.Authorization, GetAuthToken())
                    .WithHeaders(headers?.Headers)
                    .PutJsonAsync(body, cancellationToken)
                    .ReceiveJson<T>();

                return response;
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing PUT request {_flurlClient.BaseUrl + path}", httpException);
            }
        }

        public async Task<T> Delete<T>(string path, QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Delete request: {_flurlClient.BaseUrl + path}");
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .WithHeader(HeaderNames.Authorization, GetAuthToken())
                    .WithHeaders(headers?.Headers)
                    .SetQueryParams(queryParams)
                    .DeleteAsync(cancellationToken)
                    .ReceiveJson<T>();
                return response;
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing DELETE request {_flurlClient.BaseUrl + path}", httpException);
            }
        }

        public async Task<(Stream, string)> GetResponseStream(string path,
            QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _flurlClient
                    .Request(path)
                    .WithHeader(HeaderNames.Authorization, GetAuthToken())
                    .WithHeaders(headers?.Headers)
                    .SetQueryParams(queryParams)
                    .SendAsync(HttpMethod.Get, completionOption: HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken: cancellationToken);

                return (await response.GetStreamAsync(), response.Headers.FirstOrDefault(HeaderNames.ContentType));
            }
            catch (FlurlHttpException httpException)
            {
                throw LogAndCreateException(
                    $"An error occured while executing GET response stream request {_flurlClient.BaseUrl + path}",
                    httpException);
            }
        }

        private WebClientException LogAndCreateException(string message, FlurlHttpException httpException)
        {
            _logger.LogError(message);
            return new WebClientException(httpException.Message, httpException);
        }

        private string GetAuthToken() =>
            _webClientConfig.AuthToken ?? _authorisationTokenProvider.Token;
    }
}