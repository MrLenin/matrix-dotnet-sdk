using System;
using System.Collections.Generic;
using Matrix.Backends;
using Newtonsoft.Json.Linq;

namespace Matrix.Api
{
    public class MediaApi
    {
        private readonly MatrixApi _matrixApi;

        public MediaApi(MatrixApi matrixApi) =>
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));

        public Uri Upload(string contentType, byte[] data)
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/media/r0/upload", UriKind.Relative);
            var error = _matrixApi.Backend.HandlePost(apiPath, true, data,
                new Dictionary<string, string> {{"Content-Type", contentType}}, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            var mxcString = (result as JObject)?.GetValue("content_uri", StringComparison.InvariantCulture)
                .ToObject<string>();
            return new Uri(mxcString, UriKind.Relative);
        }
    }
}