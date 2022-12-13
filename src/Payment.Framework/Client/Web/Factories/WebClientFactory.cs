using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using Payment.Framework.Client.Web.Contracts;
using Payment.Framework.Client.Web.Models;
using Payment.Framework.Client.Web.Models.Config;

namespace Payment.Framework.Client.Web.Factories
{
    public class WebClientFactory : IWebClientFactory
    {
        private readonly AuthorisationTokenProvider _authorisationTokenProvider;
        private readonly IFlurlClientFactory _flurlClientFactory;
        private readonly ILogger<WebClient> _webClientLogger;

        public WebClientFactory(
            IFlurlClientFactory flurlClientFactory,
            AuthorisationTokenProvider authorisationTokenProvider,
            ILogger<WebClient> webClientLogger)
        {
            _flurlClientFactory = flurlClientFactory;
            _authorisationTokenProvider = authorisationTokenProvider;
            _webClientLogger = webClientLogger;
        }

        public IWebClient Get(WebClientConfig config) =>
            new WebClient(_flurlClientFactory, _authorisationTokenProvider, _webClientLogger, config);
    }
}