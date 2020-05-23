using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public class TestingApiBackend : IMatrixApiBackend
    {
        public MatrixRequestError HandleGet(Uri apiPath, bool authenticate, out JToken result)
        {
            result = null;
            return null;
        }

        public Task<MatrixApiResult> HandleGetAsync(Uri apiPath, bool authenticate)
        {
            return Task.FromResult<MatrixApiResult>(default);
        }

        public MatrixRequestError HandlePost(Uri apiPath, bool authenticate, JToken request, out JToken result)
        {
            result = null;
            return null;
        }

        public MatrixRequestError HandlePost(Uri apiPath, bool authenticate, JToken request,
            Dictionary<string, string> headers, out JToken result)
        {
            result = null;
            return null;
        }

        public MatrixRequestError HandlePost(Uri apiPath, bool authenticate, byte[] request,
            Dictionary<string, string> headers, out JToken result)
        {
            result = null;
            return null;
        }

        public MatrixRequestError HandlePut(Uri apiPath, bool authenticate, JToken request, out JToken result)
        {
            result = null;
            return null;
        }

        public Task<MatrixApiResult> HandlePutAsync(Uri apiPath, bool authenticate, JToken request)
        {
            return null;
        }

        public MatrixRequestError HandleDelete(Uri apiPath, bool authenticate, out JToken result)
        {
            result = null;
            return null;
        }

        public void SetAccessToken(string accessToken)
        {
        }
    }
}