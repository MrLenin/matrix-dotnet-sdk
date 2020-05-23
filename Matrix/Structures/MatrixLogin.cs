using System;

namespace Matrix.Structures
{
    public abstract class MatrixLogin
    {
    }

    /// <summary>
    /// Following https://matrix.org/docs/spec/client_server/r0.4.0.html#id213
    /// </summary>
    public class MatrixLoginPassword : MatrixLogin
    {
        public MatrixLoginPassword(string user, string password, string deviceId = null, string deviceDisplayName = null)
        {
            User = user;
            Password = password;
            DeviceId = deviceId;
            DeviceDisplayName = deviceDisplayName;
        }

        public string Type { get; } = "m.login.password";
        public string User { get; }
        public string Password { get; }
        public string DeviceId { get; }
        public string DeviceDisplayName { get; }
    }

    public class MatrixLoginToken : MatrixLogin
    {
        public MatrixLoginToken(string user, string token)
        {
            User = user;
            Token = token;
        }

        public string User { get; }
        public string Token { get; }
        public string TransactionId { get; } = Guid.NewGuid().ToString();
        public string Type { get; } = "m.login.token";
    }

    /// <summary>
    /// Following http://matrix.org/docs/spec/r0.0.1/client_server.html#id76
    /// </summary>
    public class MatrixLoginResponse
    {
        public string AccessToken { get; set; }
        public Uri Homeserver { get; set; }
        public string UserId { get; set; }
    }
}