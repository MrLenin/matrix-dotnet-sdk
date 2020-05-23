using System;
using Matrix.Structures;

namespace Matrix.Client
{
    public class MatrixMediaFile
    {
        private readonly Uri _baseUrl;
        private readonly Uri _mxcUrl;
        private string _contentType;
        private readonly MatrixFileInfo _fileInfo;

        public MatrixMediaFile(MatrixApi api, Uri mxcUrl, string contentType)
        {
            if (api == null) throw new ArgumentNullException(nameof(api));

            _baseUrl = api.BaseUrl;
            _mxcUrl = mxcUrl;
            _contentType = contentType;
        }

        public Uri GetMxcUrl()
        {
            return _mxcUrl;
        }

        public Uri GetThumbnailUrl(int width, int height, string method = "crop")
        {
            var mxcString = _mxcUrl.ToString().Substring(6);
            return new Uri(_baseUrl,
                $"/_matrix/media/r0/thumbnail/{mxcString}?width={width}&height={height}&method={method}");
        }

        public Uri GetUrl()
        {
            var mxcString = _mxcUrl.ToString().Substring(6);
            return new Uri(_baseUrl, $"/_matrix/media/r0/download/{mxcString}");
        }
    }
}