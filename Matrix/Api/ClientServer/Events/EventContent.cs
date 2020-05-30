
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.Structures;
using Matrix.Api.Versions;


namespace Matrix.Api.ClientServer.RoomEventContent
{
    public class AudioMessageEventContent : IRoomMessageEventContent
    {
        [DataMember(Name = @"info")]
        public AudioInfo AudioInfo { get; set; }
        [DataMember(Name = @"url")]
        public Uri? Url { get; set; }
        [DataMember(Name = @"file")]
        public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind { get; } = MessageKind.Audio;
    }

    public class FileMessageEventContent : IRoomMessageEventContent
    {
        [DataMember(Name = @"filename")]
        public string FileName { get; set; }
        [DataMember(Name = @"info")]
        public FileInfo FileInfo { get; set; }
        [DataMember(Name = @"url")]
        public Uri? Url { get; set; }
        [DataMember(Name = @"file")]
        public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.File;
    }

    public class ImageMessageEventContent : IRoomMessageEventContent
    {
        [DataMember(Name = @"info")]
        public ImageInfo ImageInfo { get; set; }
        [DataMember(Name = @"url")]
        public Uri Url { get; set; }
        [DataMember(Name = @"file")]
        public object? EncryptedFile { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Image;
    }

    public class LocationMessageEventContent : IRoomMessageEventContent
    {
        [DataMember(Name = @"geo_uri")]
        public Uri GeoUri { get; set; }
        [DataMember(Name = @"info")]
        public LocationInfo? LocationInfo { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.Location;
    }

    public class ServerNoticeMessageEventContent : IRoomMessageEventContent
    {
        [DataMember(Name = @"server_notice_type")]
        public string ServerNoticeKind { get; set; }
        [DataMember(Name = @"admin_contact")]
        public string? AdminContact { get; set; }
        [DataMember(Name = @"limited_type")]
        public string? LimitKind { get; set; }

        public string MessageBody { get; set; }
        public MessageKind MessageKind => MessageKind.ServerNotice;
    }

    public class TextMessageEventContent : IRoomMessageEventContent
    {
        [DataMember(Name = @"format")]
        public string Format { get; set; }
        [DataMember(Name = @"formatted_body")]
        public string FormattedBody { get; set; }

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
        [DataMember(Name = @"reason")]
        public string Reason { get; set; }
    }

    [Obsolete(@"**NB: Usage of this event is discouraged in favour of the receipts module. **Most clients will not recognise this event**")]
    public class MessageFeedbackEventContent : IRoomEventContent
    {
        public enum FeedbackKind
        {
            Delivered,
            Read
        }
        [DataMember(Name = @"target_event_id")]
        public string TargetEventId { get; set; }
        [DataMember(Name = @"type")]
        public FeedbackKind Feedback { get; set; }
    }
}

namespace Matrix.Api.ClientServer.StateEventContent
{
    public class AvatarEventContent : IStateEventContent
    {
        [DataMember(Name = @"info")] public ImageInfo? ImageInfo { get; set; }
        [DataMember(Name = @"url")] public Uri Url { get; set; }
    }

    public class CanonicalAliasEventContent : IStateEventContent
    {
        [DataMember(Name = @"alias")] public string Alias { get; set; }
        [DataMember(Name = @"alt_aliases")] public IEnumerable<string> AlternateAliases { get; set; }
    }

    public class CreateEventContent : IStateEventContent
    {
        [DataMember(Name = @"creator")] public string Creator { get; set; }
        [DataMember(Name = @"m.federate")] public bool Federate { get; set; }
        [DataMember(Name = @"room_version")] public RoomsVersion RoomsVersion { get; set; } = RoomsVersion.V1;
        [DataMember(Name = @"predecessor")] public RoomPredecessor? RoomPredecessor { get; set; }
    }

    public class GuestAccessEventContent : IStateEventContent
    {
        [DataMember(Name = @"guest_access")]
        public GuestAccessKind GuestAccessKind { get; set; } = GuestAccessKind.Forbidden;
    }

    public class HistoryVisibilityEventContent : IStateEventContent
    {
        [DataMember(Name = @"history_visibility")]
        public HistoryVisibilityKind HistoryVisibilityKind { get; set; }
    }

    public class JoinRulesEventContent : IStateEventContent
    {
        [DataMember(Name = @"join_rule")] public JoinRuleKind JoinRuleKind { get; set; }
    }

    public class MembershipEventContent : IStateEventContent
    {
        [DataMember(Name = @"avatar_url")]
        public Uri AvatarUrl { get; set; }
        [DataMember(Name = @"displayname")]
        public string? DisplayName { get; set; }
        [DataMember(Name = @"membership")]
        public MembershipState MembershipState { get; set; }
        [DataMember(Name = @"is_direct")]
        public bool IsDirect { get; set; }
        [DataMember(Name = @"third_party_invite")]
        public ThirdPartyInvite ThirdPartyInvite { get; set; }
        [DataMember(Name = @"unsigned")]
        public UnsignedData UnsignedData { get; set; }
    }

    public class NameEventContent : IStateEventContent
    {
        [DataMember(Name = @"name")]
        public string Name { get; set; }
    }

    public class PinnedEventsEventContent : IStateEventContent
    {
        [DataMember(Name = @"pinned")]
        public IEnumerable<string> PinnedEvents { get; set; }
    }

    public class PowerLevelsEventContent : IStateEventContent
    {
        [DataMember(Name = @"ban")]
        public int BanLevel { get; set; }
        [DataMember(Name = @"events")]
        public IEnumerable<KeyValuePair<string, int>> EventLevels { get; set; }
        [DataMember(Name = @"events_default")]
        public int EventsDefaultLevel { get; set; }
        [DataMember(Name = @"invite")]
        public int InviteLevel { get; set; }
        [DataMember(Name = @"kick")]
        public int KickLevel { get; set; }
        [DataMember(Name = @"redact")]
        public int RedactLevel { get; set; }
        [DataMember(Name = @"state_default")]
        public int StateDefaultLevel { get; set; }
        [DataMember(Name = @"users")]
        public IEnumerable<KeyValuePair<string, int>> UserLevels { get; set; }
        [DataMember(Name = @"users_default")]
        public int UsersDefaultLevel { get; set; }
        [DataMember(Name = @"notifications")]
        public NotificationKeywordLevels NotificationKeywordLevels { get; set; }
    }

    public class ServerAclEventContent : IStateEventContent
    {
        [DataMember(Name = @"allow_ip_literals")]
        public bool AllowIpLiterals { get; set; }
        [DataMember(Name = @"allow")]
        public IEnumerable<string> AllowServers { get; set; }
        [DataMember(Name = @"deny")]
        public IEnumerable<string> DenyServers { get; set; }
    }

    public class ThirdPartyInviteEventContent : IStateEventContent
    {
        [DataMember(Name = @"display_name")]
        public string DisplayName { get; set; }
        [DataMember(Name = @"key_validity_url")]
        public Uri KeyValidityUrl { get; set; }
        [DataMember(Name = @"public_key")]
        public string PublicKey { get; set; }
        [DataMember(Name = @"public_keys")]
        public IEnumerable<PublicKeys> PublicKeys { get; set; }
    }

    public class TombstoneEventContent : IStateEventContent
    {
        [DataMember(Name = @"body")]
        public string Body { get; set; }
        [DataMember(Name = @"replacement_room")]
        public string ReplacementRoomId { get; set; }
    }

    public class TopicEventContent : IStateEventContent
    {
        [DataMember(Name = @"topic")]
        public string Topic { get; set; }
    }
}