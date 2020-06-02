using System;
using System.Collections.Generic;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.Versions;

namespace Matrix.Api.ClientServer
{
    public class LoginEndpoint
    {
        private readonly Uri _apiPath;
        private readonly MatrixApi _matrixApi;

        public LoginEndpoint(MatrixApi matrixApi)
        {
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));
            _apiPath = new Uri(@"/_matrix/client/r0/login", UriKind.Relative);
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-login")]
        private IEnumerable<AuthKind> RequestAuthenticationKinds()
        {
            _matrixApi.ThrowIfNotSupported();

            var response =  _matrixApi.Backend.
                Request<AuthResponse>(_apiPath, false);
            // Error testing goes here
            return response.AuthKinds;
        }

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-login")]
        public AuthContext Login<T>(AuthRequest authRequest)
            where T : AuthResponse, new()
        {
            if (authRequest == null) throw new ArgumentNullException(nameof(authRequest));
            _matrixApi.ThrowIfNotSupported();
            var response = _matrixApi.Backend.Request<T>(authRequest, false);
            return new AuthContext
            {
                AccessToken = response.AccessToken,
                DeviceId = response.DeviceId,
                Homeserver = new Uri(response.Homeserver), // FIXME: do this right
                UserId = response.UserId
            };
        }
    }
}