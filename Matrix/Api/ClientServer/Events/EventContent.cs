using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.Structures;
using Matrix.Api.Versions;

using Newtonsoft.Json;


namespace Matrix.Api.ClientServer.RoomEventContent
{
    public class AudioMessageEventContent : IMessageEventContent
    {
        [JsonProperty(@"info")] public AudioInfo AudioInfo { get; set; }
        [JsonProperty(@"url")] public Uri? Url { get; set; }
        [JsonProperty(@"file")] public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Audio;
    }

    public class FileMessageEventContent : IMessageEventContent
    {
        [JsonProperty(@"filename")] public string FileName { get; set; }
        [JsonProperty(@"info")] public FileInfo FileInfo { get; set; }
        [JsonProperty(@"url")] public Uri? Url { get; set; }
        [JsonProperty(@"file")] public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.File;
    }

    public class ImageMessageEventContent : IMessageEventContent
    {
        [JsonProperty(@"info")] public ImageInfo ImageInfo { get; set; }
        [JsonProperty(@"url")] public Uri Url { get; set; }
        [JsonProperty(@"file")] public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Image;
    }

    public class LocationMessageEventContent : IMessageEventContent
    {
        [JsonProperty(@"geo_uri")] public Uri GeoUri { get; set; }
        [JsonProperty(@"info")] public LocationInfo? LocationInfo { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Location;
    }

    public class ServerNoticeMessageEventContent : IMessageEventContent
    {
        [JsonProperty(@"server_notice_type")] public string ServerNoticeKind { get; set; }
        [JsonProperty(@"admin_contact")] public string? AdminContact { get; set; }
        [JsonProperty(@"limited_type")] public string? LimitKind { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.ServerNotice;
    }

    public class TextMessageEventContent : IMessageEventContent
    {
        [JsonProperty(@"format")] public string Format { get; set; }
        [JsonProperty(@"formatted_body")] public string FormattedBody { get; set; }

        public string MessageBody { get; set; }
        public virtual MessageKind MessageKind => MessageKind.Text;
    }

    public class EmoteMessageEventContent : TextMessageEventContent
    {
        public override MessageKind MessageKind => MessageKind.Emote;
    }

    public class NoticeMessageEventContent : TextMessageEventContent
    {
        public override MessageKind MessageKind => MessageKind.Notice;
    }

    public class RedactionEventContent : IRoomEventContent
    {
        [JsonProperty(@"reason")] public string Reason { get; set; }
    }

    [Obsolete(
        @"**NB: Usage of this event is discouraged in favour of the receipts module. **Most clients will not recognise this event**")]
    public class MessageFeedbackEventContent : IRoomEventContent
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

namespace Matrix.Api.ClientServer.StateEventContent
{
    public class AvatarEventContent : IStateEventContent
    {
        [JsonProperty(@"info")] public ImageInfo? ImageInfo { get; set; }
        [JsonProperty(@"url")] public Uri Url { get; set; }
    }

    public class CanonicalAliasEventContent : IStateEventContent
    {
        [JsonProperty(@"alias")] public string Alias { get; set; }
        [JsonProperty(@"alt_aliases")] public IEnumerable<string> AlternateAliases { get; set; }
    }

    public class CreateEventContent : IStateEventContent
    {
        [JsonProperty(@"creator")] public string Creator { get; set; }
        [JsonProperty(@"m.federate")] public bool Federate { get; set; }
        [JsonProperty(@"room_version")] public RoomsVersion RoomsVersion { get; set; } = RoomsVersion.V1;
        [JsonProperty(@"predecessor")] public RoomPredecessor? RoomPredecessor { get; set; }
    }

    public class GuestAccessEventContent : IStateEventContent
    {
        [JsonProperty(@"guest_access")]
        public GuestAccessKind GuestAccessKind { get; set; } = GuestAccessKind.Forbidden;
    }

    public class HistoryVisibilityEventContent : IStateEventContent
    {
        [JsonProperty(@"history_visibility")] public HistoryVisibilityKind HistoryVisibilityKind { get; set; }
    }

    public class JoinRuleEventContent : IStateEventContent
    {
        [JsonProperty(@"join_rule")] public JoinRule JoinRule { get; set; }
    }

    public class MembershipEventContent : IStateEventContent
    {
        [JsonProperty(@"avatar_url")] public Uri? AvatarUrl { get; set; }
        [JsonProperty(@"displayname")] public string? DisplayName { get; set; }
        [JsonProperty(@"membership")] public MembershipState MembershipState { get; set; }
        [JsonProperty(@"is_direct")] public bool IsDirect { get; set; }
        [JsonProperty(@"third_party_invite")] public ThirdPartyInvite ThirdPartyInvite { get; set; }

    }

    public class NameEventContent : IStateEventContent
    {
        [JsonProperty(@"name")] public string Name { get; set; }
    }

    public class PinnedEventsEventContent : IStateEventContent
    {
        [JsonProperty(@"pinned")] public IEnumerable<string> PinnedEvents { get; set; }
    }

    public class PowerLevelsEventContent : IStateEventContent
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

    public class ServerAclEventContent : IStateEventContent
    {
        [JsonProperty(@"allow_ip_literals")] public bool AllowIpLiterals { get; set; }
        [JsonProperty(@"allow")] public IEnumerable<string> AllowServers { get; set; }
        [JsonProperty(@"deny")] public IEnumerable<string> DenyServers { get; set; }
    }

    public class ThirdPartyInviteEventContent : IStateEventContent
    {
        [JsonProperty(@"display_name")] public string DisplayName { get; set; }
        [JsonProperty(@"key_validity_url")] public Uri KeyValidityUrl { get; set; }
        [JsonProperty(@"public_key")] public string PublicKey { get; set; }
        [JsonProperty(@"public_keys")] public IEnumerable<PublicKeys> PublicKeys { get; set; }
    }

    public class TombstoneEventContent : IStateEventContent
    {
        [JsonProperty(@"body")] public string Body { get; set; }
        [JsonProperty(@"replacement_room")] public string ReplacementRoomId { get; set; }
    }

    public class TopicEventContent : IStateEventContent
    {
        [JsonProperty(@"topic")] public string Topic { get; set; }
    }
}
