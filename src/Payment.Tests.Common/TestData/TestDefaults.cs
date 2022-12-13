using System.Net;
using Payment.Framework.Api.Error;

namespace Payment.Tests.Common.TestData
{
    public static class TestDefaults
    {
        public static ErrorResponse DefaultError(string message) => new ErrorResponse
        {
            Error = new ErrorDescription
            {
                Status = HttpStatusCode.NotFound,
                Description = message
            }
        };
    }
}