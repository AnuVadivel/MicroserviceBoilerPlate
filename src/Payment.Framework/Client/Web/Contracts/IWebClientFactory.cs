using Payment.Framework.Client.Web.Models.Config;

namespace Payment.Framework.Client.Web.Contracts
{
    public interface IWebClientFactory
    {
        IWebClient Get(WebClientConfig config);
    }
}