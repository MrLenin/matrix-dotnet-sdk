using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public interface IMatrixApiBackend
    {
        MatrixRequestError Get(Uri apiPath, bool authenticate, out JToken result);
        Task<MatrixApiResult> GetAsync(Uri apiPath, bool authenticate);
        MatrixRequestError Delete(Uri apiPath, bool authenticate, out JToken result);
        MatrixRequestError Post(Uri apiPath, bool authenticate, JToken request, out JToken result);
        MatrixRequestError Put(Uri apiPath, bool authenticate, JToken request, out JToken result);
        Task<MatrixApiResult> PutAsync(Uri apiPath, bool authenticate, JToken request);

        MatrixRequestError Post(Uri apiPath, bool authenticate, JToken request, Dictionary<string, string> headers,
            out JToken result);

        MatrixRequestError Post(Uri apiPath, bool authenticate, byte[] request, Dictionary<string, string> headers,
            out JToken result);

        void SetAccessToken(string accessToken);
    }

    public struct MatrixApiResult
    {
        public JToken Result { get; set; }
        public MatrixRequestError Error { get; set; }
    }
}