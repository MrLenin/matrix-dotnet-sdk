
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Structures;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer
{
    public interface IAuthIdentifier
    {
        
    }

    public abstract class AuthRequest : IRequest
    {
        [JsonProperty(@"device_id")]
        public string DeviceId { get; protected set; }
        [JsonProperty(@"initial_device_display_name")]
        public string DeviceDisplayName { get; protected set; }

        // User-interactive authentication - should move into own type?
        [JsonProperty(@"session")]
        public string SessionId { get; protected set; }

        public RequestKind RequestKind { get; }
        public Uri Path { get; }
        public IEnumerable<byte> Content { get; }
        public AuthRequest? Auth { get; } = null;

        public AuthRequest()
        {
            Path = new Uri(@"/_matrix/client/r0/login", UriKind.Relative);
            RequestKind = RequestKind.Post;
        }
    }

    public class PasswordAuthRequest<TAuthIdentifier> : AuthRequest
        where TAuthIdentifier : IAuthIdentifier
    {
        [JsonProperty(@"identifier")]
        public TAuthIdentifier AuthIdentifier { get; }
        [JsonProperty(@"password")]
        public string Password { get; }

        [Obsolete(@"Deprecated in favor of User Identifiers r0.4.0")]
        [JsonProperty(@"user")]
        public string? UserId { get; }
        [Obsolete(@"Deprecated in favor of User Identifiers r0.4.0")]
        [JsonProperty(@"medium")]
        public string? Medium { get; }
        [Obsolete(@"Deprecated in favor of User Identifiers r0.4.0")]
        [JsonProperty(@"address")]
        public string? Address { get; }

        public PasswordAuthRequest(TAuthIdentifier authIdentifier, string password)
        {
            if (authIdentifier == null) throw new ArgumentNullException(nameof(authIdentifier));
            if (authIdentifier.GetType() == typeof(PhoneAuthIdentifier))
                throw new ArgumentException(@"Password authentication may use only User or ThirdParty identifiers.");

            AuthIdentifier = authIdentifier;
            Password = password;
        }
    }

    public class TokenAuthRequest : AuthRequest
    {
        [JsonProperty(@"token")]
        public string AuthToken { get; }

        public TokenAuthRequest(string authToken)
        {
            AuthToken = authToken;
        }

        public static string ToJsonString()
        {
            return @"m.login.token";
        }
    }

    public class AuthResponse : IResponse
    {
        // Non-interactive
        [JsonProperty(@"user_id")]
        public string UserId { get; }
        [JsonProperty(@"access_token")]
        public string AccessToken { get; }
        [JsonProperty(@"device_id")]
        public string DeviceId { get; }
        [JsonProperty(@"well_known")]
        public DiscoveryInfo DiscoveryInfo { get; }
        
        [Obsolete(@"Api use deprecated: r0.4.0")]
        [JsonProperty(@"home_server")]
        public string Homeserver { get; }

        // User-interactive and non-interactive
        [JsonProperty(@"flows")]
        public IEnumerable<AuthKind> AuthKinds { get; }

        // User-interactive authentication - should move into own type?
        [JsonProperty(@"completed")]
        public IEnumerable<string> CompletedStages { get; }
        [JsonProperty(@"params")]
        public Dictionary<string, KeyValuePair<string, string>> Parameters { get; }
        [JsonProperty(@"session")]
        public string SessionId { get; }

        public ErrorCode? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public long? RetryAfterMilliseconds { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}