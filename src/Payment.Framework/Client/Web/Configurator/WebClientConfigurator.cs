using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payment.Framework.Client.Web.Contracts;
using Payment.Framework.Client.Web.Factories;
using Payment.Framework.Client.Web.Models;
using Payment.Framework.Client.Web.Policies;
using Payment.Framework.Client.Web.Utils;

namespace Payment.Framework.Client.Web.Configurator
{
    public class WebClientConfigurator
    {
        private readonly IServiceCollection _serviceCollection;
        internal WebClientConfigurator(IServiceCollection serviceCollection) => 
            _serviceCollection = serviceCollection;

        public void Add()
        {
            AddLogger();
            AddRequiredServices();
            AddFlurlWithPolly();
        }
        
        public void AddLogger()
        {
            PolicyLogger.Logger = _serviceCollection
                .BuildServiceProvider().GetService<ILogger<PolicyLogger>>();
        }

        public void AddRequiredServices()
        {
            _serviceCollection.AddScoped<IWebClientFactory, WebClientFactory>();
            _serviceCollection.AddScoped<AuthorisationTokenProvider>();
            _serviceCollection.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
        }

        public void AddFlurlWithPolly()
        {
            FlurlHttp.Configure(settings =>
            {
                settings.JsonSerializer = new JsonSerializer();
            });
        }

    }
}