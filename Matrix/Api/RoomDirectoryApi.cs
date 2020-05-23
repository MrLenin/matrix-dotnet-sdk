using System;
using System.Web;
using Matrix.Backends;
using Matrix.Structures;

namespace Matrix
{
    public partial class MatrixApi
    {
        public PublicRooms PublicRooms(int limit, string since, string server)
        {
            ThrowIfNotSupported();

            var qs = HttpUtility.ParseQueryString(string.Empty);

            if (limit != 0)
                qs.Set("limit", limit.ToString());

            if (!string.IsNullOrEmpty(since))
                qs.Set("since", since);

            if (!string.IsNullOrEmpty(server))
                qs.Set("server", server);

            var apiPath = new Uri($"/_matrix/client/r0/publicRooms?{qs}", UriKind.Relative);
            var error = _matrixApiBackend.HandleGet(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return result.ToObject<PublicRooms>();
        }

        public void DeleteFromRoomDirectory(string alias)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/directory/room/{alias}", UriKind.Relative);
            var error = _matrixApiBackend.HandleDelete(apiPath, true, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }
    }
}