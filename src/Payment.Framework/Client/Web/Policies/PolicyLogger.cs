using Microsoft.Extensions.Logging;

namespace Payment.Framework.Client.Web.Policies
{
    internal class PolicyLogger
    {
        internal static ILogger<PolicyLogger> Logger { get; set; }
    }
}