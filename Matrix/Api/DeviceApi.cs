using System;
using System.Threading.Tasks;
using Matrix.Backends;
using Matrix.Structures;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public partial class MatrixApi
    {
        [MatrixSpec(EMatrixSpecApiVersion.R030, EMatrixSpecApi.ClientServer, "get-matrix-client-r0-devices")]
        public async Task<Device[]> GetDevices()
        {
            ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/client/r0/devices", UriKind.Relative);
            var res = await _matrixApiBackend.GetAsync(apiPath, true).ConfigureAwait(false);
            
            if (res.Error.IsOk) return res.Result.ToObject<Device[]>();

            throw new MatrixException(res.Error.ToString());
        }
    }
}