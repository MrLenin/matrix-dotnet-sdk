using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.Structures;
using Matrix.Api.Versions;

using Newtonsoft.Json;


namespace Matrix.Api.ClientServer.EventContent
{
    public class PresenceContent : IEventContent
    {
        [JsonProperty(@"last_active_ago")]
        public long LastActiveAgo { get; }
        [JsonProperty(@"avatar_url")]
        public Uri AvatarUrl { get; }
        [JsonProperty(@"displayname")]
        public string DisplayName { get; }
        [JsonProperty(@"presence")]
        public PresenceStatus PresenceStatus { get; }
        [JsonProperty(@"currently_active")]
        public bool CurrentlyActive { get; }
        [JsonProperty(@"status_msg")]
        public string StatusMessage { get; }
    }

    public class ReceiptContent : IEventContent
    {
        public Dictionary<string, ReceiptedEvent> ReceiptedEvents { get; set; }
    }
}

namespace Matrix.Api.ClientServer.RoomContent
{
    public class AudioMessageContent : IMessageContent
    {
        [JsonProperty(@"info")] public AudioInfo AudioInfo { get; set; }
        [JsonProperty(@"url")] public Uri? Url { get; set; }
        [JsonProperty(@"file")] public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Audio;
    }

    public class FileMessageContent : IMessageContent
    {
        [JsonProperty(@"filename")] public string FileName { get; set; }
        [JsonProperty(@"info")] public FileInfo FileInfo { get; set; }
        [JsonProperty(@"url")] public Uri? Url { get; set; }
        [JsonProperty(@"file")] public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.File;
    }

    public class ImageMessageContent : IMessageContent
    {
        [JsonProperty(@"info")] public ImageInfo ImageInfo { get; set; }
        [JsonProperty(@"url")] public Uri Url { get; set; }
        [JsonProperty(@"file")] public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Image;
    }

    public class LocationMessageContent : IMessageContent
    {
        [JsonProperty(@"geo_uri")] public Uri GeoUri { get; set; }
        [JsonProperty(@"info")] public LocationInfo? LocationInfo { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Location;
    }

    public class ServerNoticeMessageContent : IMessageContent
    {
        [JsonProperty(@"server_notice_type")] public string ServerNoticeKind { get; set; }
        [JsonProperty(@"admin_contact")] public string? AdminContact { get; set; }
        [JsonProperty(@"limited_type")] public string? LimitKind { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.ServerNotice;
    }

    public class TextMessageContent : IMessageContent
    {
        [JsonProperty(@"format")] public string Format { get; set; }
        [JsonProperty(@"formatted_body")] public string FormattedBody { get; set; }

        public string MessageBody { get; set; }
        public virtual MessageKind MessageKind => MessageKind.Text;
    }

    public class EmoteMessageContent : TextMessageContent
    {
        public override MessageKind MessageKind => MessageKind.Emote;
    }

    public class NoticeMessageContent : TextMessageContent
    {
        public override MessageKind MessageKind => MessageKind.Notice;
    }

    public class RedactionContent : IRoomContent
    {
        [JsonProperty(@"reason")] public string Reason { get; set; }
    }

    [Obsolete(
        @"**NB: Usage of this event is discouraged in favour of the receipts module. **Most clients will not recognise this event**")]
    public class MessageFeedbackContent : IRoomContent
    {
        public enum FeedbackKind
        {
            Delivered,
            Read
        }

        [JsonProperty(@"target_event_id")] public string TargetEventId { get; set; }
        [JsonProperty(@"type")] public FeedbackKind Feedback { get; set; }
    }
}

namespace Matrix.Api.ClientServer.StateContent
{
    public class RoomAvatarContent : IStateContent
    {
        [JsonProperty(@"info")] public ImageInfo? ImageInfo { get; set; }
        [JsonProperty(@"url")] public Uri Url { get; set; }
    }

    public class RoomCanonicalAliasContent : IStateContent
    {
        [JsonProperty(@"alias")] public string? Alias { get; set; }
        [JsonProperty(@"alt_aliases")] public IEnumerable<string> AlternateAliases { get; set; }
    }

    public class RoomCreateContent : IStateContent
    {
        [JsonProperty(@"creator")] public string Creator { get; set; }
        [JsonProperty(@"m.federate")] public bool Federate { get; set; } = true;
        [JsonProperty(@"room_version")] public RoomsVersion RoomsVersion { get; set; } = RoomsVersion.V1;
        [JsonProperty(@"predecessor")] public RoomPredecessor? RoomPredecessor { get; set; }
    }

    public class RoomGuestAccessContent : IStateContent
    {
        [JsonProperty(@"guest_access")]
        public GuestAccessKind GuestAccessKind { get; set; } = GuestAccessKind.Forbidden;
    }

    public class RoomHistoryVisibilityContent : IStateContent
    {
        [JsonProperty(@"history_visibility")] public HistoryVisibilityKind HistoryVisibilityKind { get; set; }
    }

    public class RoomJoinRulesContent : IStateContent
    {
        [JsonProperty(@"join_rule")] public JoinRule JoinRule { get; set; }
    }

    public class RoomMembershipContent : IStateContent
    {
        [JsonProperty(@"avatar_url")] public Uri? AvatarUrl { get; set; }
        [JsonProperty(@"displayname")] public string? DisplayName { get; set; }
        [JsonProperty(@"membership")] public MembershipState MembershipState { get; set; }
        [JsonProperty(@"is_direct")] public bool IsDirect { get; set; }
        [JsonProperty(@"third_party_invite")] public ThirdPartyInvite? ThirdPartyInvite { get; set; }

    }

    public class RoomNameContent : IStateContent
    {
        [JsonProperty(@"name")] public string Name { get; set; }
    }

    public class RoomPinnedEventsContent : IStateContent
    {
        [JsonProperty(@"pinned")] public IEnumerable<string> PinnedEvents { get; set; }
    }

    public class RoomPowerLevelsContent : IStateContent
    {
        [JsonProperty(@"ban")] public int BanLevel { get; set; }
        [JsonProperty(@"events")] public IDictionary<string, int> EventLevels { get; set; }
        [JsonProperty(@"events_default")] public int EventsDefaultLevel { get; set; }
        [JsonProperty(@"invite")] public int InviteLevel { get; set; }
        [JsonProperty(@"kick")] public int KickLevel { get; set; }
        [JsonProperty(@"redact")] public int RedactLevel { get; set; }
        [JsonProperty(@"state_default")] public int StateDefaultLevel { get; set; }
        [JsonProperty(@"users")] public IDictionary<string, int> UserLevels { get; set; }
        [JsonProperty(@"users_default")] public int UsersDefaultLevel { get; set; }
        [JsonProperty(@"notifications")] public IDictionary<string, int> NotificationLevels { get; set; }
    }

    public class RoomServerAclContent : IStateContent
    {
        [JsonProperty(@"allow_ip_literals")] public bool AllowIpLiterals { get; set; }
        [JsonProperty(@"allow")] public IEnumerable<string> AllowServers { get; set; }
        [JsonProperty(@"deny")] public IEnumerable<string> DenyServers { get; set; }
    }

    public class RoomThirdPartyInviteContent : IStateContent
    {
        [JsonProperty(@"display_name")] public string DisplayName { get; set; }
        [JsonProperty(@"key_validity_url")] public Uri KeyValidityUrl { get; set; }
        [JsonProperty(@"public_key")] public string PublicKey { get; set; }
        [JsonProperty(@"public_keys")] public IEnumerable<PublicKeys> PublicKeys { get; set; }
    }

    public class RoomTombstoneContent : IStateContent
    {
        [JsonProperty(@"body")] public string Body { get; set; }
        [JsonProperty(@"replacement_room")] public string ReplacementRoomId { get; set; }
    }

    public class RoomTopicContent : IStateContent
    {
        [JsonProperty(@"topic")] public string Topic { get; set; }
    }
}
