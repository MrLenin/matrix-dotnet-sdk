using System;

using Matrix.Api.Versions;
using Matrix.Backends;
using Matrix.Structures;
using Newtonsoft.Json.Linq;

namespace Matrix.Api
{
    public class LoginApi
    {
        private readonly MatrixApi _matrixApi;

        public LoginApi(MatrixApi matrixApi) =>
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));

        [MatrixSpec(ClientServerApiVersion.R001, "post-matrix-client-r0-login")]
        public MatrixLoginResponse ClientLogin(MatrixLogin login)
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/client/r0/login", UriKind.Relative);
            var error = _matrixApi.Backend.HandlePost(apiPath, false, JObject.FromObject(login), out var result);

            if (error.IsOk) return result.ToObject<MatrixLoginResponse>();

            throw new MatrixException(error.ToString());
        }
    }
}