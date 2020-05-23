using System;
using System.Collections.Generic;
using Matrix.Backends;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public partial class MatrixApi
    {
        public Uri MediaUpload(string contentType, byte[] data)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/media/r0/upload", UriKind.Relative);
            var error = _matrixApiBackend.Post(apiPath, true, data,
                new Dictionary<string, string> {{"Content-Type", contentType}}, out var result);
            
            if (!error.IsOk) throw new MatrixException(error.ToString());

            var mxcString = (result as JObject)?.GetValue("content_uri", StringComparison.InvariantCulture).ToObject<string>();
            return new Uri(mxcString, UriKind.Relative);
        }
    }
}