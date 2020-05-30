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
        private IEnumerable<AuthenticationKind> RequestAuthenticationKinds()
        {
            _matrixApi.ThrowIfNotSupported();

            var response =  _matrixApi.Backend.
                Request<AuthenticationResponse>(_apiPath, false);
            // Error testing goes here
            return response.AuthenticationMethods;
        }

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-login")]
        public AuthenticationContext Login<T>(AuthenticationRequest authenticationRequest)
            where T : AuthenticationResponse, new()
        {
            if (authenticationRequest == null) throw new ArgumentNullException(nameof(authenticationRequest));
            _matrixApi.ThrowIfNotSupported();
            var response = _matrixApi.Backend.Request<T>(authenticationRequest, false);
            return new AuthenticationContext
            {
                AccessToken = response.AccessToken,
                DeviceId = response.DeviceId,
                Homeserver = new Uri(response.Homeserver), // FIXME: do this right
                UserId = response.UserId
            };
        }
    }
}