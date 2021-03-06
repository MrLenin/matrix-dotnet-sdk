﻿
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
            [JsonProperty(@"m.read")]
            public Dictionary<string, Receipt> ReceiptedUsers { get; private set; }
        }

        public struct Receipt
        {
            [JsonProperty(@"ts")]
            public long TimeStamp { get; set; }
        }

        public struct RoomPredecessor
        {
            [JsonProperty(@"room_id")] public string RoomId { get; set; }
            [JsonProperty(@"event_id")] public string EventId { get; set; }
        }

        public interface IStrippedState
        {
            [JsonProperty(@"content")] IStateContent Content { get; set; }
            [JsonProperty(@"state_key")] string StateKey { get; set; }
            [JsonProperty(@"sender")] string Sender { get; set; }
            [JsonProperty(@"type")] EventKind EventKind { get; set; }
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

        public struct RoomSummary
        {
            [JsonProperty(@"m.heroes")]
            public IEnumerable<string> Heroes { get; set; }
            [JsonProperty(@"m.joined_member_count")]
            public int JoinedMemberCount { get; set; }
            [JsonProperty(@"m.invited_member_count")]
            public int InvitedMemberCount { get; set; }
        }

        public struct RoomEphemeral
        {
            [JsonProperty(@"events")]
            public IEnumerable<IEvent> Events { get; set; }
        }

        public struct UnreadNotificationCount
        {
            [JsonProperty(@"highlight_count")]
            public int HighlightCount { get; set; }
            [JsonProperty(@"notification_count")]
            public int NotificationCount { get; set; }
        }

        public struct JoinedRoom
        {
            [JsonProperty(@"summary")]
            public RoomSummary Summary { get; set; }
            [JsonProperty(@"state")]
            public State State { get; set; }
            [JsonProperty(@"timeline")]
            public Timeline Timeline { get; set; }
            [JsonProperty(@"ephemeral")]
            public RoomEphemeral Ephemeral { get; set; }
            [JsonProperty(@"account_data")]
            public AccountData AccountData { get; set; }
            [JsonProperty(@"unread_notifications")]
            public UnreadNotificationCount UnreadNotificationCount { get; set; }
        }

        public struct InviteState
        {
            [JsonProperty(@"events")]
            public IEnumerable<IStrippedState> Events { get; set; }
        }

        public struct InvitedRoom
        {
            [JsonProperty(@"invite_state")]
            public InviteState State { get; set; }
        }

        public struct State
        {
            [JsonProperty(@"events")]
            public IEnumerable<IStateEvent> Events { get; set; }
        }

        public struct Timeline
        {
            [JsonProperty(@"events")]
            public IEnumerable<IRoomEvent> Events { get; set; }
            [JsonProperty(@"limited")]
            public bool Limited { get; set; }
            [JsonProperty(@"prev_batch")]
            public string PrevBatch { get; set; }
        }

        public struct PresenceData
        {
            [JsonProperty(@"events")]
            public IEnumerable<IEvent> Events { get; set; }
        }

        public struct AccountData
        {
            [JsonProperty(@"events")]
            public IEnumerable<IEvent> Events { get; set; }
        }

        public struct LeftRoom
        {
            [JsonProperty(@"state")]
            public State State { get; set; }
            [JsonProperty(@"timeline")]
            public Timeline Timeline { get; set; }
            [JsonProperty(@"account_data")]
            public AccountData AccountData { get; set; }
        }

        public struct ToDeviceData
        {
            [JsonProperty(@"events")]
            public IEnumerable<IEvent> Events { get; set; }
        }

        public struct DeviceLists
        {
            [JsonProperty(@"changed")]
            public IEnumerable<string> Changed { get; set; }
            [JsonProperty(@"left")]
            public IEnumerable<string> Left { get; set; }
        }

        public struct SyncRooms
        {
            [JsonProperty(@"join")]
            public IDictionary<string, JoinedRoom> JoinedRooms { get; set; }
            [JsonProperty(@"invite")]
            public IDictionary<string, InvitedRoom> InvitedRooms { get; set; }
            [JsonProperty(@"leave")]
            public IDictionary<string, LeftRoom> LeftRooms { get; set; }
        }

        public struct RoomTag
        {
            private double _order;

            [JsonProperty(@"order")]
            public double Order
            {
                get => _order;
                set
                {
                    if (value > 1.0) _order = 1.0;
                    else if (value < 0.0) _order = 0.0;
                    else _order = value;
                }
            }
        }
    }
}