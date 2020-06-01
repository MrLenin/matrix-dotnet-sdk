using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Matrix.Api;

using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public class TestingApiBackend : IMatrixApiBackend
    {
        public TResponse Request<TResponse>(Uri url, bool authenticate, HttpContent httpContent = null) where TResponse : IResponse, new()
        {
            return default;
        }

        public TResponse Request<TResponse>(IRequest request, bool authenticate, HttpContent httpContent = null) where TResponse : IResponse, new()
        {
            return default;
        }

        public Task<TResponse> RequestAsync<TResponse>(Uri url, bool authenticate, HttpContent httpContent = null) where TResponse : IResponse, new()
        {
            return null;
        }

        public Task<TResponse> RequestAsync<TResponse>(IRequest request, bool authenticate, HttpContent httpContent = null) where TResponse : IResponse, new()
        {
            return null;
        }

        public void SetAccessToken(string accessToken)
        {
        }
    }
}