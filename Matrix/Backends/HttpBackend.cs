using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public class HttpBackend : IMatrixApiBackend
    {
        private Uri _baseUrl;
        private string _accessToken;
        private string _userId;
        private HttpClient _client;

        public HttpBackend(Uri apiUrl, string userId = null, HttpClient client = null)
        {
            _baseUrl = apiUrl;
            ServicePointManager.ServerCertificateValidationCallback += AcceptCertificate;
            _client = client ?? new HttpClient();
            _userId = userId;
        }

        private static bool AcceptCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true; //Find a better way to handle mono certs.
        }

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        private Uri GetPath(Uri apiPath, bool auth)
        {
            apiPath = new Uri(_baseUrl, apiPath);

            if (!auth) return apiPath;

            apiPath = new Uri(apiPath,
                (string.IsNullOrEmpty(apiPath.Query) ? "&" : "?") + "access_token=" + _accessToken);

            if (_userId != null)
                apiPath = new Uri(apiPath, "&user_id=" + _userId);

            return apiPath;
        }

        private static async Task<MatrixApiResult> RequestWrap(Task<HttpResponseMessage> task)
        {
            var apiResult = new MatrixApiResult();
            try
            {
                var (jToken, httpStatusCode) = await GenericRequest(task).ConfigureAwait(false);
                apiResult.Result = jToken;
                apiResult.Error = new MatrixRequestError("", MatrixErrorCode.None, httpStatusCode);
            }
            catch (MatrixServerError e)
            {
                var retryAfter = -1;

                if (e.ErrorObject.ContainsKey("retry_after_ms"))
                    retryAfter = e.ErrorObject["retry_after_ms"].ToObject<int>();

                apiResult.Error = new MatrixRequestError(e.Message, e.ErrorCode, HttpStatusCode.InternalServerError,
                    retryAfter);
            }

            return apiResult;
        }

        public MatrixRequestError Get(Uri apiPath, bool authenticate, out JToken result)
        {
            apiPath = GetPath(apiPath, authenticate);
            var task = _client.GetAsync(apiPath);
            var res = RequestWrap(task);
            res.Wait();
            result = res.Result.Result;
            return res.Result.Error;
        }

        public Task<MatrixApiResult> GetAsync(Uri apiPath, bool authenticate)
        {
            var task = _client.GetAsync(GetPath(apiPath, authenticate));
            return RequestWrap(task);
        }

        public MatrixRequestError Delete(Uri apiPath, bool authenticate, out JToken result)
        {
            apiPath = GetPath(apiPath, authenticate);
            var task = _client.DeleteAsync(apiPath);
            var res = RequestWrap(task);
            res.Wait();
            result = res.Result.Result;
            return res.Result.Error;
        }

        public MatrixRequestError Put(Uri apiPath, bool authenticate, JToken data, out JToken result)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            using var content = new StringContent(data.ToString(Formatting.None), Encoding.UTF8, "application/json");
            apiPath = GetPath(apiPath, authenticate);
            var task = _client.PutAsync(apiPath, content);
            var res = RequestWrap(task);
            res.Wait();
            result = res.Result.Result;
            return res.Result.Error;
        }

        public Task<MatrixApiResult> PutAsync(Uri apiPath, bool authenticate, JToken request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            using var content = new StringContent(request.ToString(Formatting.None), Encoding.UTF8, "application/json");
            apiPath = GetPath(apiPath, authenticate);
            var task = _client.PutAsync(apiPath, content);
            return RequestWrap(task);
        }

        public MatrixRequestError Post(Uri apiPath, bool authenticate, JToken data, Dictionary<string, string> headers,
            out JToken result)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            using var content = data != null
                ? new StringContent(data.ToString(), Encoding.UTF8, "application/json")
                : new StringContent("{}");

            foreach (var (header, value) in headers)
                content.Headers.Add(header, value);

            apiPath = GetPath(apiPath, authenticate);
            var task = _client.PostAsync(apiPath, content);
            var res = RequestWrap(task);
            res.Wait();
            result = res.Result.Result;
            return res.Result.Error;
        }

        public MatrixRequestError Post(Uri apiPath, bool authenticate, byte[] data, Dictionary<string, string> headers,
            out JToken result)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            using var content = data != null ? new ByteArrayContent(data) : new ByteArrayContent(Array.Empty<byte>());

            foreach (var (header, value) in headers)
                content.Headers.Add(header, value);

            apiPath = GetPath(apiPath, authenticate);
            var task = _client.PostAsync(apiPath, content);
            var res = RequestWrap(task);
            res.Wait();
            result = res.Result.Result;
            return res.Result.Error;
        }

        public MatrixRequestError Post(Uri apiPath, bool authenticate, JToken data, out JToken result)
        {
            return Post(apiPath, authenticate, data, new Dictionary<string, string>(), out result);
        }

        private static async Task<Tuple<JToken, HttpStatusCode>> GenericRequest(Task<HttpResponseMessage> task)
        {
            Task<string> stringTask = null;
            HttpResponseMessage httpResult;

            try
            {
                httpResult = await task.ConfigureAwait(false);
                if (httpResult.StatusCode.HasFlag(HttpStatusCode.OK))
                    stringTask = httpResult.Content.ReadAsStringAsync();
                else
                    return new Tuple<JToken, HttpStatusCode>(null, httpResult.StatusCode);
            }
            catch (AggregateException e)
            {
                throw new MatrixException(e.InnerException.Message, e.InnerException);
            }

            var json = await stringTask.ConfigureAwait(false);
            var result = JToken.Parse(json);

            if (result.Type == JTokenType.Object && result["errcode"] != null)
                throw new MatrixServerError(result["errcode"].ToObject<string>(),
                    result["error"].ToObject<string>(), result as JObject);

            return new Tuple<JToken, HttpStatusCode>(result, httpResult.StatusCode);
        }
    }
}