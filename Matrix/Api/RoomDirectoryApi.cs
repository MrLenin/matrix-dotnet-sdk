using System;
using System.Globalization;
using System.Web;
using Matrix.Backends;
using Matrix.Structures;

namespace Matrix.Api
{
    public class RoomDirectoryApi
    {
        private readonly MatrixApi _matrixApi;

        public RoomDirectoryApi(MatrixApi matrixApi) =>
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));

        public PublicRooms PublicRooms(int limit, string since, string server)
        {
            _matrixApi.ThrowIfNotSupported();

            var qs = HttpUtility.ParseQueryString(string.Empty);

            if (limit != 0)
                qs.Set("limit", limit.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(since))
                qs.Set("since", since);

            if (!string.IsNullOrEmpty(server))
                qs.Set("server", server);

            var apiPath = new Uri($"/_matrix/client/r0/publicRooms?{qs}", UriKind.Relative);
            var error = _matrixApi.Backend.HandleGet(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return result.ToObject<PublicRooms>();
        }

        public void DeleteFrom(string alias)
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/directory/room/{alias}", UriKind.Relative);
            var error = _matrixApi.Backend.HandleDelete(apiPath, true, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }
    }
}