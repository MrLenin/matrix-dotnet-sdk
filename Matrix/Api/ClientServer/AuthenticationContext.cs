using System;

namespace Matrix.Api.ClientServer
{
    /// <summary>
    /// Following http://matrix.org/docs/spec/r0.0.1/client_server.html#id76
    /// </summary>
    public class AuthenticationContext
    {
        public string AccessToken { get; set; }
        public Uri Homeserver { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
    }
}