using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Payment.Framework.Client.Web.Middleware;

namespace Payment.Framework.Client.Web.Configurator
{
    public static class WebClientExtensions
    {
        private static WebClientConfigurator _webClientConfigurator;

        public static WebClientConfigurator WebClient(this IServiceCollection serviceCollection)
        {
            _webClientConfigurator = new WebClientConfigurator(serviceCollection);
            return _webClientConfigurator;
        }

        public static IApplicationBuilder ConfigureDefaultAuthTokenProvider(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<AuthorisationTokenProviderMiddleware>();
            return applicationBuilder;
        }
    }
}