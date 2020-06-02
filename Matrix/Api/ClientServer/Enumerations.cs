using System;
using System.ComponentModel;

namespace Matrix.Api.ClientServer
{
    namespace Enumerations
    {
        public enum AuthKind
        {
            Password,
            ReCaptcha,
            OAuth2,
            EmailIdentity,
            Msisdn,
            Token,
            Dummy
        }

        public enum EncryptionAlgorithm
        {
            OlmV1Curve25519AesSha2,
            MegOlmV1AesSha2
        }

        public enum ErrorCode
        {
            Forbidden,
            UnknownToken,
            MissingToken,
            BadJson,
            NotJson,
            NotFound,
            LimitExceeded,
            Unknown,
            Unrecognized,
            Unauthorized,
            UserDeactivated,
            UserInUse,
            InvalidUsername,
            RoomInUse,
            InvalidRoomState,
            ThreepidInUse,
            ThreepidNotFound,
            ThreepidAuthFailed,
            ThreepidDenied,
            ServerNotTrusted,
            UnsupportedRoomVersion,
            IncompatibleRoomVersion,
            BadState,
            GuestAccessForbidden,
            CaptchaNeeded,
            MissingParam,
            InvalidParam,
            TooLarge,
            Exclusive,
            ResourceLimitExceeded,
            CannotLeaveServerNoticeRoom,
            UnknownErrorCode,
            None
        }

        public enum EventKind
        {
            Custom,
            Presence,
            Receipt,
            RoomAvatar,
            RoomCanonicalAlias,
            RoomCreate,
            RoomGuestAccess,
            RoomHistoryVisibility,
            RoomJoinRule,
            RoomMembership,
            RoomMessage,
            RoomName,
            RoomPinnedEvents,
            RoomPowerLevels,
            RoomRedaction,
            RoomServerAcl,
            RoomThirdPartyInvite,
            RoomTombstone,
            RoomTopic,
            Tag,
            Typing
        }

        public enum GuestAccess
        {
            CanJoin,
            Forbidden
        }

        public enum HistoryVisibility
        {
            Invited,
            Joined,
            Shared,
            WorldReadable
        }

        public enum JoinRule
        {
            Public,
            Knock,
            Invite,
            Private
        }

        public enum MembershipState
        {
            Invite,
            Join,
            Knock,
            Leave,
            Ban
        }

        public enum MessageKind
        {
            Audio,
            Emote,
            File,
            Image,
            Location,
            Notice,
            ServerNotice,
            Text,
            Video
        }

        public enum PresenceState
        {
            Online,
            Offline,
            Idle
        }

        public enum RequestKind
        {
            Post,
            Put,
            Delete
        }

        public static class EnumerationExtensions
        {
            public static string ToJsonString(this AuthKind authKind)
            {
                return authKind switch
                {
                    AuthKind.Password => @"m.login.password",
                    AuthKind.ReCaptcha => @"m.login.recaptcha",
                    AuthKind.OAuth2 => @"m.login.oauth2",
                    AuthKind.EmailIdentity => @"m.login.email.identity",
                    AuthKind.Msisdn => @"m.login.msisdn",
                    AuthKind.Token => @"m.login.token",
                    AuthKind.Dummy => @"m.login.dummy",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this EncryptionAlgorithm encryptionAlgorithm)
            {
                return encryptionAlgorithm switch
                {
                    EncryptionAlgorithm.OlmV1Curve25519AesSha2 => @"m.olm.v1.curve25519-aes-sha2",
                    EncryptionAlgorithm.MegOlmV1AesSha2 => @"m.megolm.v1.aes-sha2",
                _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this ErrorCode matrixErrorCode)
            {
                return matrixErrorCode switch
                {
                    ErrorCode.Forbidden => @"M_FORBIDDEN",
                    ErrorCode.UnknownToken => @"M_UNKNOWN_TOKEN",
                    ErrorCode.MissingToken => @"M_MISSING_TOKEN",
                    ErrorCode.BadJson => @"M_BAD_JSON",
                    ErrorCode.NotJson => @"M_NOT_JSON",
                    ErrorCode.NotFound => @"M_NOT_FOUND",
                    ErrorCode.LimitExceeded => @"M_LIMIT_EXCEEDED",
                    ErrorCode.Unknown => @"M_UNKNOWN",
                    ErrorCode.Unrecognized => @"M_UNRECOGNIZED",
                    ErrorCode.Unauthorized => @"M_UNAUTHORIZED",
                    ErrorCode.UserDeactivated => @"M_USER_DEACTIVATED",
                    ErrorCode.UserInUse => @"M_USER_IN_USE",
                    ErrorCode.InvalidUsername => @"M_INVALID_USERNAME",
                    ErrorCode.RoomInUse => @"M_ROOM_IN_USE",
                    ErrorCode.InvalidRoomState => @"M_INVALID_ROOM_STATE",
                    ErrorCode.ThreepidInUse => @"M_THREEPID_IN_USE",
                    ErrorCode.ThreepidNotFound => @"M_THREEPID_NOT_FOUND",
                    ErrorCode.ThreepidAuthFailed => @"M_THREEPID_AUTH_FAILED",
                    ErrorCode.ThreepidDenied => @"M_THREEPID_DENIED",
                    ErrorCode.ServerNotTrusted => @"M_SERVER_NOT_TRUSTED",
                    ErrorCode.UnsupportedRoomVersion => @"M_UNSUPPORTED_ROOM_VERSION",
                    ErrorCode.IncompatibleRoomVersion => @"M_INCOMPATIBLE_ROOM_VERSION",
                    ErrorCode.BadState => @"M_BAD_STATE",
                    ErrorCode.GuestAccessForbidden => @"M_GUEST_ACCESS_FORBIDDEN",
                    ErrorCode.CaptchaNeeded => @"M_CAPTCHA_NEEDED",
                    ErrorCode.MissingParam => @"M_MISSING_PARAM",
                    ErrorCode.InvalidParam => @"M_INVALID_PARAM",
                    ErrorCode.TooLarge => @"M_TOO_LARGE",
                    ErrorCode.Exclusive => @"M_EXCLUSIVE",
                    ErrorCode.ResourceLimitExceeded => @"M_RESOURCE_LIMIT_EXCEEDED",
                    ErrorCode.CannotLeaveServerNoticeRoom => @"M_CANNOT_LEAVE_SERVER_NOTICE_ROOM",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this EventKind eventKind)
            {
                return eventKind switch
                {
                    EventKind.Custom => @"custom.event",
                    EventKind.Presence => @"m.presence",
                    EventKind.Receipt => @"m.receipt",
                    EventKind.RoomAvatar => @"m.room.avatar",
                    EventKind.RoomCanonicalAlias => @"m.room.canonical_alias",
                    EventKind.RoomCreate => @"m.room.create",
                    EventKind.RoomGuestAccess => @"m.room.guest_access",
                    EventKind.RoomHistoryVisibility => @"m.room.history_visibility",
                    EventKind.RoomJoinRule => @"m.room.join_rules",
                    EventKind.RoomMembership => @"m.room.member",
                    EventKind.RoomMessage => @"m.room.message",
                    EventKind.RoomName => @"m.room.name",
                    EventKind.RoomPinnedEvents => @"m.room.pinned_events",
                    EventKind.RoomPowerLevels => @"m.room.power_levels",
                    EventKind.RoomRedaction => @"m.room.redaction",
                    EventKind.RoomServerAcl => @"m.room.server_acl",
                    EventKind.RoomThirdPartyInvite => @"m.room.third_party_invite",
                    EventKind.RoomTombstone => @"m.room.tombstone",
                    EventKind.RoomTopic => @"m.room.topic",
                    EventKind.Tag => @"m.tag",
                    EventKind.Typing => @"m.typing",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this GuestAccess guestAccess)
            {
                return guestAccess switch
                {
                    GuestAccess.CanJoin => @"can_join",
                    GuestAccess.Forbidden => @"forbidden",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this HistoryVisibility historyVisibility)
            {
                return historyVisibility switch
                {
                    HistoryVisibility.Invited => @"invited",
                    HistoryVisibility.Joined => @"joined",
                    HistoryVisibility.Shared => @"shared",
                    HistoryVisibility.WorldReadable => @"world_readable",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this JoinRule joinRule)
            {
                return joinRule switch
                {
                    JoinRule.Invite => @"invite",
                    JoinRule.Knock => @"knock",
                    JoinRule.Private => @"private",
                    JoinRule.Public => @"public",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this MembershipState membershipState)
            {
                return membershipState switch
                {
                    MembershipState.Ban => @"ban",
                    MembershipState.Invite => @"invite",
                    MembershipState.Join => @"join",
                    MembershipState.Knock => @"knock",
                    MembershipState.Leave => @"leave",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this MessageKind messageKind)
            {
                return messageKind switch
                {
                    MessageKind.Audio => @"m.audio",
                    MessageKind.Emote => @"m.emote",
                    MessageKind.File => @"m.file",
                    MessageKind.Image => @"m.image",
                    MessageKind.Location => @"m.location",
                    MessageKind.Notice => @"m.notice",
                    MessageKind.ServerNotice => @"m.server_notice",
                    MessageKind.Text => @"m.text",
                    MessageKind.Video => @"m.video",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this PresenceState presenceState)
            {
                return presenceState switch
                {
                    PresenceState.Online => @"online",
                    PresenceState.Offline => @"offline",
                    PresenceState.Idle => @"unavailable",
                _ => throw new InvalidCastException()
                };
            }

            public static AuthKind ToAuthKind(this string authenticationMethod)
            {
                return authenticationMethod switch
                {
                    @"m.login.password" => AuthKind.Password,
                    @"m.login.recaptcha" => AuthKind.ReCaptcha,
                    @"m.login.oauth2" => AuthKind.OAuth2,
                    @"m.login.email.identity" => AuthKind.EmailIdentity,
                    @"m.login.msisdn" => AuthKind.Msisdn,
                    @"m.login.token" => AuthKind.Token,
                    @"m.login.dummy" => AuthKind.Dummy,
                    _ => throw new InvalidCastException()
                };
            }

            public static EncryptionAlgorithm ToEncryptionAlgorithm(this string encryptionAlgorithm)
            {
                return encryptionAlgorithm switch
                {
                    @"m.olm.v1.curve25519-aes-sha2" => EncryptionAlgorithm.OlmV1Curve25519AesSha2,
                    @"m.megolm.v1.aes-sha2" => EncryptionAlgorithm.MegOlmV1AesSha2,
                    _ => throw new InvalidCastException()
                };
            }

            public static ErrorCode ToErrorCode(this string errorCode)
            {
                if (string.IsNullOrEmpty(errorCode)) return ErrorCode.None;

                return errorCode switch
                {
                    @"M_FORBIDDEN" => ErrorCode.Forbidden,
                    @"M_UNKNOWN_TOKEN" => ErrorCode.UnknownToken,
                    @"M_MISSING_TOKEN" => ErrorCode.MissingToken,
                    @"M_BAD_JSON" => ErrorCode.BadJson,
                    @"M_NOT_JSON" => ErrorCode.NotJson,
                    @"M_NOT_FOUND" => ErrorCode.NotFound,
                    @"M_LIMIT_EXCEEDED" => ErrorCode.LimitExceeded,
                    @"M_UNKNOWN" => ErrorCode.Unknown,
                    @"M_UNRECOGNIZED" => ErrorCode.Unrecognized,
                    @"M_UNAUTHORIZED" => ErrorCode.Unauthorized,
                    @"M_USER_DEACTIVATED" => ErrorCode.UserDeactivated,
                    @"M_USER_IN_USE" => ErrorCode.UserInUse,
                    @"M_INVALID_USERNAME" => ErrorCode.InvalidUsername,
                    @"M_ROOM_IN_USE" => ErrorCode.RoomInUse,
                    @"M_INVALID_ROOM_STATE" => ErrorCode.InvalidRoomState,
                    @"M_THREEPID_IN_USE" => ErrorCode.ThreepidInUse,
                    @"M_THREEPID_NOT_FOUND" => ErrorCode.ThreepidNotFound,
                    @"M_THREEPID_AUTH_FAILED" => ErrorCode.ThreepidAuthFailed,
                    @"M_THREEPID_DENIED" => ErrorCode.ThreepidDenied,
                    @"M_SERVER_NOT_TRUSTED" => ErrorCode.ServerNotTrusted,
                    @"M_UNSUPPORTED_ROOM_VERSION" => ErrorCode.UnsupportedRoomVersion,
                    @"M_INCOMPATIBLE_ROOM_VERSION" => ErrorCode.IncompatibleRoomVersion,
                    @"M_BAD_STATE" => ErrorCode.BadState,
                    @"M_GUEST_ACCESS_FORBIDDEN" => ErrorCode.GuestAccessForbidden,
                    @"M_CAPTCHA_NEEDED" => ErrorCode.CaptchaNeeded,
                    @"M_MISSING_PARAM" => ErrorCode.MissingParam,
                    @"M_INVALID_PARAM" => ErrorCode.InvalidParam,
                    @"M_TOO_LARGE" => ErrorCode.TooLarge,
                    @"M_EXCLUSIVE" => ErrorCode.Exclusive,
                    @"M_RESOURCE_LIMIT_EXCEEDED" => ErrorCode.ResourceLimitExceeded,
                    @"M_CANNOT_LEAVE_SERVER_NOTICE_ROOM" => ErrorCode.CannotLeaveServerNoticeRoom,
                    _ => throw new InvalidCastException()
                };
            }

            public static EventKind ToEventKind(this string eventKind)
            {
                return eventKind switch
                {
                    @"m.presence" => EventKind.Presence,
                    @"m.receipt" => EventKind.Receipt,
                    @"m.room.avatar" => EventKind.RoomAvatar,
                    @"m.room.canonical_alias" => EventKind.RoomCanonicalAlias,
                    @"m.room.create" => EventKind.RoomCreate,
                    @"m.room.guest_access" => EventKind.RoomGuestAccess,
                    @"m.room.history_visibility" => EventKind.RoomHistoryVisibility,
                    @"m.room.join_rules" => EventKind.RoomJoinRule,
                    @"m.room.member" => EventKind.RoomMembership,
                    @"m.room.message" => EventKind.RoomMessage,
                    @"m.room.name" => EventKind.RoomName,
                    @"m.room.pinned_events" => EventKind.RoomPinnedEvents,
                    @"m.room.power_levels" => EventKind.RoomPowerLevels,
                    @"m.room.redaction" => EventKind.RoomRedaction,
                    @"m.room.server_acl" => EventKind.RoomServerAcl,
                    @"m.room.third_party_invite" => EventKind.RoomThirdPartyInvite,
                    @"m.room.tombstone" => EventKind.RoomTombstone,
                    @"m.room.topic" => EventKind.RoomTopic,
                    @"m.tag" => EventKind.Tag,
                    @"m.typing" => EventKind.Typing,
                    _ => EventKind.Custom
                };
            }

            public static GuestAccess ToGuestAccess(this string guestAccessKind)
            {
                return guestAccessKind switch
                {
                    @"can_join" => GuestAccess.CanJoin,
                    @"forbidden" => GuestAccess.Forbidden,
                    _ => throw new InvalidCastException()
                };
            }

            public static HistoryVisibility ToHistoryVisibility(this string historyVisibilityKind)
            {
                return historyVisibilityKind switch
                {
                    @"invited" => HistoryVisibility.Invited,
                    @"joined" => HistoryVisibility.Joined,
                    @"shared" => HistoryVisibility.Shared,
                    @"world_readable" => HistoryVisibility.WorldReadable,
                    _ => throw new InvalidCastException()
                };
            }

            public static JoinRule ToJoinRule(this string joinRule)
            {
                return joinRule switch
                {
                    @"invite" => JoinRule.Invite,
                    @"knock" => JoinRule.Knock,
                    @"private" => JoinRule.Private,
                    @"public" => JoinRule.Public,
                    _ => throw new InvalidCastException()
                };
            }

            public static MembershipState ToMembershipState(this string membershipState)
            {
                return membershipState switch
                {
                    @"ban" => MembershipState.Ban,
                    @"invite" => MembershipState.Invite,
                    @"join" => MembershipState.Join,
                    @"knock" => MembershipState.Knock,
                    @"leave" => MembershipState.Leave,
                    _ => throw new InvalidCastException()
                };
            }

            public static MessageKind ToMessageKind(this string messageKind)
            {
                return messageKind switch
                {
                    @"m.audio" => MessageKind.Audio,
                    @"m.emote" => MessageKind.Emote,
                    @"m.file" => MessageKind.File,
                    @"m.image" => MessageKind.Image,
                    @"m.location" => MessageKind.Location,
                    @"m.notice" => MessageKind.Notice,
                    @"m.server_notice" => MessageKind.ServerNotice,
                    @"m.text" => MessageKind.Text,
                    @"m.video" => MessageKind.Video,
                    _ => throw new InvalidCastException()
                };
            }

            public static PresenceState ToPresenceState(this string presenceStatus)
            {
                return presenceStatus switch
                {
                    @"online" => PresenceState.Online,
                    @"offline" => PresenceState.Offline,
                    @"unavailable" => PresenceState.Idle,
                    _ => throw new InvalidCastException()
                };
            }
        }
    }
}