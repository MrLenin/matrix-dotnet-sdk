using System;
using System.ComponentModel;

namespace Matrix.Api.ClientServer
{
    namespace Enumerations
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
            Presence,
            Receipt,
            RoomAvatar,
            RoomCanonicalAlias,
            RoomCreate,
            RoomGuestAccess,
            RoomHistoryVisibility,
            RoomJoinRules,
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
            Typing
        }

        public enum GuestAccessKind
        {
            CanJoin,
            Forbidden
        }

        public enum HistoryVisibilityKind
        {
            Invited,
            Joined,
            Shared,
            WorldReadable
        }

        public enum JoinRuleKind
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

        public enum PresenceStatus
        {
            Online,
            Offline,
            Unavailable,
            FreeForChat,
            Hidden
        }

        public enum RequestKind
        {
            Post,
            Put,
            Delete
        }

        public static class EnumerationExtensions
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
                    EventKind.Presence => @"m.presence",
                    EventKind.Receipt => @"m.receipt",
                    EventKind.RoomAvatar => @"m.room.avatar",
                    EventKind.RoomCanonicalAlias => @"m.room.canonical_alias",
                    EventKind.RoomCreate => @"m.room.create",
                    EventKind.RoomGuestAccess => @"m.room.guest_access",
                    EventKind.RoomHistoryVisibility => @"m.room.history_visibility",
                    EventKind.RoomJoinRules => @"m.room.join_rules",
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
                    EventKind.Typing => @"m.typing",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this GuestAccessKind guestAccessKind)
            {
                return guestAccessKind switch
                {
                    GuestAccessKind.CanJoin => @"can_join",
                    GuestAccessKind.Forbidden => @"forbidden",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this HistoryVisibilityKind historyVisibilityKind)
            {
                return historyVisibilityKind switch
                {
                    HistoryVisibilityKind.Invited => @"invited",
                    HistoryVisibilityKind.Joined => @"joined",
                    HistoryVisibilityKind.Shared => @"shared",
                    HistoryVisibilityKind.WorldReadable => @"world_readable",
                    _ => throw new InvalidCastException()
                };
            }

            public static string ToJsonString(this JoinRuleKind joinRule)
            {
                return joinRule switch
                {
                    JoinRuleKind.Invite => @"invite",
                    JoinRuleKind.Knock => @"knock",
                    JoinRuleKind.Private => @"private",
                    JoinRuleKind.Public => @"public",
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

            public static EventKind ToEventType(this string eventType)
            {
                return eventType switch
                {
                    @"m.presence" => EventKind.Presence,
                    @"m.receipt" => EventKind.Receipt,
                    @"m.room.avatar" => EventKind.RoomAvatar,
                    @"m.room.canonical_alias" => EventKind.RoomCanonicalAlias,
                    @"m.room.create" => EventKind.RoomCreate,
                    @"m.room.guest_access" => EventKind.RoomGuestAccess,
                    @"m.room.history_visibility" => EventKind.RoomHistoryVisibility,
                    @"m.room.join_rules" => EventKind.RoomJoinRules,
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
                    @"m.typing" => EventKind.Typing,
                    _ => throw new InvalidCastException()
                };
            }
        }
    }
}