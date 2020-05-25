﻿using System;
using System.Threading.Tasks;

using Matrix.Api.Versions;
using Matrix.Backends;
using Matrix.Structures;
using Newtonsoft.Json.Linq;

namespace Matrix.Api
{
    public class DeviceApi
    {
        private readonly MatrixApi _matrixApi;

        public DeviceApi(MatrixApi matrixApi) =>
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));

        [MatrixSpec(ClientServerApiVersion.R030, "get-matrix-client-r0-devices")]
        public async Task<Device[]> GetDevices()
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new Uri("/_matrix/client/r0/devices", UriKind.Relative);
            var res = await _matrixApi.Backend.HandleGetAsync(apiPath, true).ConfigureAwait(false);

            if (res.Error.IsOk) return res.Result.ToObject<Device[]>();

            throw new MatrixException(res.Error.ToString());
        }
    }
}