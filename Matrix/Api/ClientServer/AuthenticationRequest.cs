
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
    public interface IAuthenticationIdentifier
    {
        
    }

    public abstract class AuthenticationRequest : IRequest
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
        public AuthenticationRequest? Authentication { get; } = null;

        public AuthenticationRequest()
        {
            Path = new Uri(@"/_matrix/client/r0/login", UriKind.Relative);
            RequestKind = RequestKind.Post;
        }
    }

    public class PasswordAuthenticationRequest<TAuthenticationIdentifier> : AuthenticationRequest
        where TAuthenticationIdentifier : IAuthenticationIdentifier
    {
        [JsonProperty(@"identifier")]
        public TAuthenticationIdentifier AuthenticationIdentifier { get; }
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

        public PasswordAuthenticationRequest(TAuthenticationIdentifier authenticationIdentifier, string password)
        {
            if (authenticationIdentifier == null) throw new ArgumentNullException(nameof(authenticationIdentifier));
            if (authenticationIdentifier.GetType() == typeof(PhoneAuthenticationIdentifier))
                throw new ArgumentException(@"Password authentication may use only User or ThirdParty identifiers.");

            AuthenticationIdentifier = authenticationIdentifier;
            Password = password;
        }
    }

    public class TokenAuthenticationRequest : AuthenticationRequest
    {
        [JsonProperty(@"token")]
        public string AuthenticationToken { get; }

        public TokenAuthenticationRequest(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
        }

        public static string ToJsonString()
        {
            return @"m.login.token";
        }
    }

    public class AuthenticationResponse : IResponse
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
        public IEnumerable<AuthenticationKind> AuthenticationMethods { get; }

        // User-interactive authentication - should move into own type?
        [JsonProperty(@"completed")]
        public IEnumerable<string> CompletedStages { get; }
        [JsonProperty(@"params")]
        public Dictionary<string, KeyValuePair<string, string>> Parameters { get; }
        [JsonProperty(@"session")]
        public string SessionId { get; }

        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public uint RetryAfterMilliseconds { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}