using System;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;

namespace Payment.Framework.Client.Web.Exceptions
{
    public class WebClientException : Exception
    {
        private readonly FlurlHttpException _exception;

        public WebClientException(string message) : base(message)
        {
        }

        public WebClientException(string message, FlurlHttpException exception) : base(message, exception)
        {
            _exception = exception;
            if (_exception.Call?.Response.StatusCode != null)
                StatusCode = (HttpStatusCode)_exception.Call?.Response.StatusCode;
        }

        public HttpStatusCode? StatusCode { get; internal set; }

        public async Task<string> GetResponseStringAsync()
        {
            return await _GetResponseStringAsync();
        }

        public async Task<T> GetResponseJsonAsync<T>()
        {
            return await _GetResponseJsonAsync<T>();
        }

        protected virtual async Task<string> _GetResponseStringAsync()
        {
            return _exception != null ? await _exception.GetResponseStringAsync() : string.Empty;
        }

        protected virtual async Task<T> _GetResponseJsonAsync<T>()
        {
            return _exception != null ? await _exception.GetResponseJsonAsync<T>() : default;
        }
    }
}