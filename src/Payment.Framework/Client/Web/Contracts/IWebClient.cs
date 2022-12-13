using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Flurl;

namespace Payment.Framework.Client.Web.Contracts
{
    public interface IWebClient
    {
        Task<T> Get<T>(string path, QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default);

        Task<string> Get(string path, QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default);

        Task<T> Post<T>(string path, object body, RequestHeaders headers = default,
            CancellationToken cancellationToken = default);

        Task<T> PostUrlEncodedAsync<T>(string path, object body, CancellationToken cancellationToken = default);

        Task<(Stream, string)> GetResponseStream(string path, QueryParamCollection queryParams,
            RequestHeaders headers = default, CancellationToken cancellationToken = default);

        Task<T> Put<T>(string path, object body, RequestHeaders headers = default,
            CancellationToken cancellationToken = default);

        Task<T> Delete<T>(string path, QueryParamCollection queryParams, RequestHeaders headers = default,
            CancellationToken cancellationToken = default);
    }
}