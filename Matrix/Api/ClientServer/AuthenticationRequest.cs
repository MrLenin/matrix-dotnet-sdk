
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;

namespace Matrix.Api.ClientServer
{
    public enum AuthenticationKind
    {
        Password,
        ReCaptcha,
        OAuth2,
        EmailIdentity,
        Msisdn,
        Token,
        Dummy
    }

    public static class AuthenticationKindExtensions
    {
        public static string ToJsonString(this AuthenticationKind authenticationKind)
        {
            return authenticationKind switch
            {
                AuthenticationKind.Password => @"m.login.password",
                AuthenticationKind.ReCaptcha => @"m.login.recaptcha",
                AuthenticationKind.OAuth2 => @"m.login.oauth2",
                AuthenticationKind.EmailIdentity => @"m.login.email.identity",
                AuthenticationKind.Msisdn => @"m.login.msisdn",
                AuthenticationKind.Token => @"m.login.token",
                AuthenticationKind.Dummy => @"m.login.dummy",
                _ => throw new InvalidEnumArgumentException(nameof(authenticationKind),
                        (int)authenticationKind, authenticationKind.GetType())
            };
        }

        public static AuthenticationKind ToAuthenticationKind(this string authenticationMethod)
        {
            return authenticationMethod switch
            {
                @"m.login.password" => AuthenticationKind.Password,
                @"m.login.recaptcha" => AuthenticationKind.ReCaptcha,
                @"m.login.oauth2" => AuthenticationKind.OAuth2,
                @"m.login.email.identity" => AuthenticationKind.EmailIdentity,
                @"m.login.msisdn" => AuthenticationKind.Msisdn,
                @"m.login.token" => AuthenticationKind.Token,
                @"m.login.dummy" => AuthenticationKind.Dummy,
                _ => throw new InvalidDataException(authenticationMethod)
            };
        }
    }

    public interface IAuthenticationIdentifier
    {
        
    }

    public class UserAuthenticationIdentifier : IAuthenticationIdentifier
    {
        [DataMember(Name = @"user")]
        public string UserId { get; set;  }

        public UserAuthenticationIdentifier() => UserId = "";
        public UserAuthenticationIdentifier(string userId) => UserId = userId;

        public static string ToJsonString()
        {
            return @"m.id.user";
        }
    }

    public class ThirdPartyAuthenticationIdentifier : IAuthenticationIdentifier
    {
        [DataMember(Name = @"medium`")]
        public string Medium { get; }
        [DataMember(Name = @"address")]
        public string Address { get; }

        public ThirdPartyAuthenticationIdentifier(string medium, string address)
        {
            Medium = medium;
            Address = address;
        }

        public static string ToJsonString()
        {
            return @"m.id.thirdparty";
        }
    }

    public class PhoneAuthenticationIdentifier : IAuthenticationIdentifier
    {
        [DataMember(Name = @"country")]
        public string Country { get; }
        [DataMember(Name = @"phone")]
        public string PhoneNumber { get; }

        public PhoneAuthenticationIdentifier(string country, string phoneNumber)
        {
            Country = country;
            PhoneNumber = phoneNumber;
        }

        public static string ToJsonString()
        {
            return @"m.id.phone";
        }
    }

    public class DiscoveryInformation
    {
        [DataMember(Name = @"m.homeserver")]
        public IDictionary<string, string> Homeserver { get; }
        [DataMember(Name = @"m.identity_server")]
        public IDictionary<string, string> IdentityServer { get; }

        public IDictionary<string, IDictionary<string, string>> CustomProperties { get; }
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
        public DiscoveryInformation DiscoveryInformation { get; }
        
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