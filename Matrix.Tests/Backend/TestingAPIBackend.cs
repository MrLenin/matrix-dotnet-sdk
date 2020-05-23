using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public class TestingApiBackend : IMatrixApiBackend
    {
        public MatrixRequestError Get(Uri apiPath, bool authenticate, out JToken result)
        {
            result = null;
            return null;
        }

        public Task<MatrixApiResult> GetAsync(Uri apiPath, bool authenticate)
        {
            return Task.FromResult<MatrixApiResult>(default);
        }

        public MatrixRequestError Post(Uri apiPath, bool authenticate, JToken request, out JToken result)
        {
            result = null;
            return null;
        }

        public MatrixRequestError Post(Uri apiPath, bool authenticate, JToken request,
            Dictionary<string, string> headers, out JToken result)
        {
            result = null;
            return null;
        }

        public MatrixRequestError Post(Uri apiPath, bool authenticate, byte[] request,
            Dictionary<string, string> headers, out JToken result)
        {
            result = null;
            return null;
        }

        public MatrixRequestError Put(Uri apiPath, bool authenticate, JToken request, out JToken result)
        {
            result = null;
            return null;
        }

        public Task<MatrixApiResult> PutAsync(Uri apiPath, bool authenticate, JToken request)
        {
            return null;
        }

        public MatrixRequestError Delete(Uri apiPath, bool authenticate, out JToken result)
        {
            result = null;
            return null;
        }

        public void SetAccessToken(string accessToken)
        {
        }
    }
}