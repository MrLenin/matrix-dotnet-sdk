using System;
using Matrix.Backends;
using Matrix.Structures;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public partial class MatrixApi
    {
        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "post-matrix-client-r0-login")]
        public MatrixLoginResponse ClientLogin(MatrixLogin login)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/client/r0/login", UriKind.Relative);
            var error = _matrixApiBackend.Post(apiPath, false, JObject.FromObject(login), out var result);

            if (error.IsOk) return result.ToObject<MatrixLoginResponse>();

            throw new MatrixException(error.ToString());
        }
    }
}