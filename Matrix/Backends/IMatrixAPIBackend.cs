using System;
using System.Net.Http;
using System.Threading.Tasks;

using Matrix.Api;

namespace Matrix.Backends
{
    public interface IMatrixApiBackend
    {
        TResponse Request<TResponse>(Uri url, bool authenticate, HttpContent? httpContent = null)
            where TResponse : IResponse, new();

        TResponse Request<TResponse>(IRequest request, bool authenticate, HttpContent? httpContent = null)
            where TResponse : IResponse, new();

        Task<TResponse> RequestAsync<TResponse>(Uri url, bool authenticate,
            HttpContent? httpContent = null)
            where TResponse : IResponse, new();

        Task<TResponse> RequestAsync<TResponse>(IRequest request, bool authenticate,
            HttpContent? httpContent = null)
            where TResponse : IResponse, new();

        void SetAccessToken(string accessToken);
    }
}