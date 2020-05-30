
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Structures;

namespace Matrix.Api.ClientServer
{
    public interface IAuthenticationIdentifier
    {
        
    }

    public abstract class AuthenticationRequest : IRequest
    {
        [DataMember(Name = @"device_id")]
        public string DeviceId { get; protected set; }
        [DataMember(Name = @"initial_device_display_name")]
        public string DeviceDisplayName { get; protected set; }

        // User-interactive authentication - should move into own type?
        [DataMember(Name = @"session")]
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
        [DataMember(Name = @"identifier")]
        public TAuthenticationIdentifier AuthenticationIdentifier { get; }
        [DataMember(Name = @"password")]
        public string Password { get; }

        [Obsolete(@"Deprecated in favor of User Identifiers r0.4.0")]
        [DataMember(Name = @"user")]
        public string? UserId { get; }
        [Obsolete(@"Deprecated in favor of User Identifiers r0.4.0")]
        [DataMember(Name = @"medium")]
        public string? Medium { get; }
        [Obsolete(@"Deprecated in favor of User Identifiers r0.4.0")]
        [DataMember(Name = @"address")]
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
        [DataMember(Name = @"token")]
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
        [DataMember(Name = @"user_id")]
        public string UserId { get; }
        [DataMember(Name = @"access_token")]
        public string AccessToken { get; }
        [DataMember(Name = @"device_id")]
        public string DeviceId { get; }
        [DataMember(Name = @"well_known")]
        public DiscoveryInfo DiscoveryInfo { get; }
        
        [Obsolete(@"Api use deprecated: r0.4.0")]
        [DataMember(Name = @"home_server")]
        public string Homeserver { get; }

        // User-interactive and non-interactive
        [DataMember(Name = @"flows")]
        public IEnumerable<AuthenticationKind> AuthenticationMethods { get; }

        // User-interactive authentication - should move into own type?
        [DataMember(Name = @"completed")]
        public IEnumerable<string> CompletedStages { get; }
        [DataMember(Name = @"params")]
        public Dictionary<string, KeyValuePair<string, string>> Parameters { get; }
        [DataMember(Name = @"session")]
        public string SessionId { get; }

        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public uint RetryAfterMilliseconds { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}