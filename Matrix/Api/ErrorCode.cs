using System.ComponentModel;

namespace Matrix.Api
{
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

    public static class ErrorCodeExtensions
    {
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
                _ => throw new System.NotImplementedException()
            };
        }

        public static ErrorCode ToErrorCode(this string matrixErrorCode)
        {
            return matrixErrorCode switch
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
                _ => throw new System.NotImplementedException()
            };
        }
    }
}