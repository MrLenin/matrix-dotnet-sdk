using System;
using System.Collections.Generic;

using Matrix.Api.ClientServer;
using Matrix.Api.Versions;
using Matrix.Structures;

namespace Matrix.Api.ClientServer
{
    public class VersionsEndpoint
    {
        private readonly Uri _apiPath;
        private readonly MatrixApi _matrixApi;

        public VersionsEndpoint(MatrixApi matrixApi)
        {
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));
            _apiPath = new Uri(@"/_matrix/client/versions", UriKind.Relative);
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-versions")]
        public VersionsContext RequestVersions()
        {
            _matrixApi.ThrowIfNotSupported();

            var response = _matrixApi.Backend.Request<VersionsResponse>(_apiPath, false);
            // Error checking goes here
            return new VersionsContext
            {
                UnstableFeatures = response.UnstableFeatures,
                Versions = response.ApiVersions
            };
        }
    }

}