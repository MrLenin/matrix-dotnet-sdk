
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.StateEventContent;

namespace Matrix.Api.ClientServer
{
    namespace Structures
    {
        public struct AudioInfo
        {
            [DataMember(Name = @"duration")] public int Duration { get; set; }
            [DataMember(Name = @"mimetype")] public string MimeType { get; set; }
            [DataMember(Name = @"size")] public int DataSize { get; set; }
        }

        public struct DiscoveryInfo
        {
            [DataMember(Name = @"m.homeserver")] public IDictionary<string, string> Homeserver { get; }

            [DataMember(Name = @"m.identity_server")]
            public IDictionary<string, string> IdentityServer { get; }

            public IDictionary<string, IDictionary<string, string>> CustomProperties { get; }
        }

        public struct FileInfo
        {
            [DataMember(Name = @"mimetype")] public string MimeType { get; set; }
            [DataMember(Name = @"size")] public int DataSize { get; set; }
            [DataMember(Name = @"thumbnail_url")] public Uri? ThumbnailUrl { get; set; }
            [DataMember(Name = @"thumbnail_file")] public object? EncryptedThumbnailFile { get; set; }
            [DataMember(Name = @"thumbnail_info")] public ThumbnailInfo ThumbnailInfo { get; set; }
        }

        public struct ImageInfo
        {
            [DataMember(Name = @"h")] public uint Height { get; set; }
            [DataMember(Name = @"w")] public uint Width { get; set; }
            [DataMember(Name = @"mimetype")] public string MimeType { get; set; }
            [DataMember(Name = @"size")] public uint DataSize { get; set; }
            [DataMember(Name = @"thumbnail_url")] public Uri ThumbnailUrl { get; set; }
            [DataMember(Name = @"thumbnail_file")] public object? ThumbnailFile { get; set; }
            [DataMember(Name = @"thumbnail_info")] public ThumbnailInfo ThumbnailInfo { get; set; }
        }

        public struct LocationInfo
        {
            [DataMember(Name = @"thumbnail_url")] public Uri ThumbnailUrl { get; set; }
            [DataMember(Name = @"thumbnail_file")] public object? EncryptedThumbnailFile { get; set; }
            [DataMember(Name = @"thumbnail_info")] public ThumbnailInfo ThumbnailInfo { get; set; }
        }

        public struct ThumbnailInfo
        {
            [DataMember(Name = @"h")] public uint Height { get; set; }
            [DataMember(Name = @"w")] public uint Width { get; set; }
            [DataMember(Name = @"mimetype")] public string MimeType { get; set; }
            [DataMember(Name = @"size")] public uint DataSize { get; set; }
        }

        public struct NotificationKeywordLevels
        {
            [DataMember(Name = @"room")] public int RoomKeywordLevel { get; set; }

            public IEnumerable<KeyValuePair<string, int>> AdditionalKeywordLevels { get; set; }
        }

        public struct PublicKeys
        {
            [DataMember(Name = @"key_validity_url")]
            public Uri KeyValidityUrl { get; set; }

            [DataMember(Name = @"public_key")] public string PublicKey { get; set; }
        }

        public struct RoomPredecessor
        {
            [DataMember(Name = @"room_id")] public string RoomId { get; set; }
            [DataMember(Name = @"event_id")] public string EventId { get; set; }
        }

        public class StrippedState<TEventContent>
            where TEventContent : class, IEventContent
        {
            [DataMember(Name = @"content")] public TEventContent Content { get; }
            [DataMember(Name = @"state_key")] public string StateKey { get; set; }
            [DataMember(Name = @"sender")] public string Sender { get; set; }
        }

        public struct ThirdPartyInvite
        {
            [DataMember(Name = @"display_name")] public string DisplayName { get; set; }
            [DataMember(Name = @"signed")] public object SignedContent { get; set; }
        }

        public struct UnsignedData
        {
            [DataMember(Name = @"invite_room_state")]
            public IEnumerable<StrippedState<MembershipEventContent>> InviteRoomStates { get; set; }
        }

        public class UserAuthenticationIdentifier : IAuthenticationIdentifier
        {
            [DataMember(Name = @"user")] public string UserId { get; set; }

            public UserAuthenticationIdentifier() => UserId = "";
            public UserAuthenticationIdentifier(string userId) => UserId = userId;

            public static string ToJsonString()
            {
                return @"m.id.user";
            }
        }

        public class ThirdPartyAuthenticationIdentifier : IAuthenticationIdentifier
        {
            [DataMember(Name = @"medium`")] public string Medium { get; }
            [DataMember(Name = @"address")] public string Address { get; }

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
            [DataMember(Name = @"country")] public string Country { get; }
            [DataMember(Name = @"phone")] public string PhoneNumber { get; }

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
    }
}