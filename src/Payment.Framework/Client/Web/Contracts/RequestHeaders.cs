using System.Collections.Generic;

namespace Payment.Framework.Client.Web.Contracts
{
    public class RequestHeaders
    {
        public RequestHeaders() => 
            Headers = new Dictionary<string, object>();

        internal IDictionary<string, object> Headers { get; }

        public RequestHeaders AddHeader(string key, object value)
        {
            Headers[key] = value;
            return this;
        }
    }
}