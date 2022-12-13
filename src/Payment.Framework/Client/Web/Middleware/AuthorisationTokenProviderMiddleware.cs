using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Payment.Framework.Client.Web.Models;

namespace Payment.Framework.Client.Web.Middleware
{
    public class AuthorisationTokenProviderMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public AuthorisationTokenProviderMiddleware(RequestDelegate requestDelegate) => 
            _requestDelegate = requestDelegate;


        public async Task InvokeAsync(HttpContext context, AuthorisationTokenProvider options)
        {
            if (context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var token))
                options.Token = token;
            await _requestDelegate(context);
        }
    }
}