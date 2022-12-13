using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Payment.Framework.Shared.Exception
{
    [ExcludeFromCodeCoverage]
    public class HttpException : System.Exception
    {
        public HttpException(HttpStatusCode httpStatusCode, string message, System.Exception ex = null)
            : base(message, ex)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; }
    }
}