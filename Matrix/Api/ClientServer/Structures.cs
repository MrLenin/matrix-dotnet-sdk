
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.StateContent;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Matrix.Api.ClientServer
{
    namespace Structures
    {
        public struct AudioInfo
        {
            [JsonProperty(@"duration")] public int Duration { get; set; }
            [JsonProperty(@"mimetype")] public string MimeType { get; set; }
            [JsonProperty(@"size")] public int DataSize { get; set; }
        }

        public struct DiscoveryInfo
        {
            [JsonProperty(@"m.homeserver")] public IDictionary<string, string> Homeserver { get; }

            [JsonProperty(@"m.identity_server")]
            public IDictionary<string, string> IdentityServer { get; }

            public IDictionary<string, IDictionary<string, string>> CustomProperties { get; }
        }

        public struct FileInfo
        {
            [JsonProperty(@"mimetype")] public string MimeType { get; set; }
            [JsonProperty(@"size")] public int DataSize { get; set; }
            [JsonProperty(@"thumbnail_url")] public Uri? ThumbnailUrl { get; set; }
            [JsonProperty(@"thumbnail_file")] public object? EncryptedThumbnailFile { get; set; }
            [JsonProperty(@"thumbnail_info")] public ThumbnailInfo ThumbnailInfo { get; set; }
        }

        public struct ImageInfo
        {
            [JsonProperty(@"h")] public uint Height { get; set; }
            [JsonProperty(@"w")] public uint Width { get; set; }
            [JsonProperty(@"mimetype")] public string MimeType { get; set; }
            [JsonProperty(@"size")] public uint DataSize { get; set; }
            [JsonProperty(@"thumbnail_url")] public Uri ThumbnailUrl { get; set; }
            [JsonProperty(@"thumbnail_file")] public object? ThumbnailFile { get; set; }
            [JsonProperty(@"thumbnail_info")] public ThumbnailInfo ThumbnailInfo { get; set; }
        }

        public struct LocationInfo
        {
            [JsonProperty(@"thumbnail_url")] public Uri ThumbnailUrl { get; set; }
            [JsonProperty(@"thumbnail_file")] public object? EncryptedThumbnailFile { get; set; }
            [JsonProperty(@"thumbnail_info")] public ThumbnailInfo ThumbnailInfo { get; set; }
        }

        public struct ThumbnailInfo
        {
            [JsonProperty(@"h")] public uint Height { get; set; }
            [JsonProperty(@"w")] public uint Width { get; set; }
            [JsonProperty(@"mimetype")] public string MimeType { get; set; }
            [JsonProperty(@"size")] public uint DataSize { get; set; }
        }

        public struct PublicKeys
        {
            [JsonProperty(@"key_validity_url")]
            public Uri KeyValidityUrl { get; set; }

            [JsonProperty(@"public_key")] public string PublicKey { get; set; }
        }

        public struct ReceiptedEvent
        {
            public Dictionary<string, Receipt> ReceiptedUsers { get; private set; }
        }

        public struct Receipt
        {
            public long TimeStamp { get; set; }
        }

        public struct RoomPredecessor
        {
            [JsonProperty(@"room_id")] public string RoomId { get; set; }
            [JsonProperty(@"event_id")] public string EventId { get; set; }
        }

        public interface IStrippedState
        {
            [JsonProperty(@"content")] public IStateContent Content { get; set; }
            [JsonProperty(@"state_key")] public string StateKey { get; set; }
            [JsonProperty(@"sender")] public string Sender { get; set; }
            [JsonProperty(@"type")] public EventKind EventKind { get; set; }
        }

        public class StrippedState<TStateEventContent> : IStrippedState
            where TStateEventContent : class, IStateContent
        {
            public TStateEventContent Content { get; set; }

            IStateContent IStrippedState.Content
            {
                get => Content;
                set => Content = (TStateEventContent) value;
            }

            public string StateKey { get; set; }
            public string Sender { get; set; }
            public EventKind EventKind { get; set; }
        }

        public struct ThirdPartyInviteSigned
        {
            [JsonProperty(@"mxid")]
            public string InvitedUserId { get; set; }
            [JsonProperty(@"token")]
            public string Token { get; set; }
            [JsonProperty(@"signatures")]
            public IDictionary<string, IDictionary<string, string>> Signatures { get; set; }
        }

        public struct ThirdPartyInvite
        {
            [JsonProperty(@"display_name")] public string DisplayName { get; set; }
            [JsonProperty(@"signed")] public ThirdPartyInviteSigned SignedContent { get; set; }
        }

        public class UnsignedData
        {
            [JsonProperty(@"age")] public int? Age { get; set; }
            [JsonProperty(@"invite_room_state")] public IEnumerable<IStrippedState>? InviteRoomStates { get; set; }
            [JsonProperty(@"redacted_because")] public string? RedactedInResponseTo { get; set; }
            [JsonProperty(@"transaction_id")] public string? TransactionId { get; set; }
        }

        public class UserAuthIdentifier : IAuthIdentifier
        {
            [JsonProperty(@"user")] public string UserId { get; set; }

            public UserAuthIdentifier() => UserId = "";
            public UserAuthIdentifier(string userId) => UserId = userId;

            public static string ToJsonString()
            {
                return @"m.id.user";
            }
        }

        public class ThirdPartyAuthIdentifier : IAuthIdentifier
        {
            [JsonProperty(@"medium`")] public string Medium { get; }
            [JsonProperty(@"address")] public string Address { get; }

            public ThirdPartyAuthIdentifier(string medium, string address)
            {
                Medium = medium;
                Address = address;
            }

            public static string ToJsonString()
            {
                return @"m.id.thirdparty";
            }
        }

        public class PhoneAuthIdentifier : IAuthIdentifier
        {
            [JsonProperty(@"country")] public string Country { get; }
            [JsonProperty(@"phone")] public string PhoneNumber { get; }

            public PhoneAuthIdentifier(string country, string phoneNumber)
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