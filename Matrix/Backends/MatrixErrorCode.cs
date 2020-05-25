using System.ComponentModel;

namespace Matrix.Backends
{
    public enum MatrixErrorCode
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

    public static class MatrixErrorCodeExtensions
    {
        public static string ToJsonString(this MatrixErrorCode matrixErrorCode)
        {
            return matrixErrorCode switch
            {
                MatrixErrorCode.Forbidden => @"M_FORBIDDEN",
                MatrixErrorCode.UnknownToken => @"M_UNKNOWN_TOKEN",
                MatrixErrorCode.MissingToken => @"M_MISSING_TOKEN",
                MatrixErrorCode.BadJson => @"M_BAD_JSON",
                MatrixErrorCode.NotJson => @"M_NOT_JSON",
                MatrixErrorCode.NotFound => @"M_NOT_FOUND",
                MatrixErrorCode.LimitExceeded => @"M_LIMIT_EXCEEDED",
                MatrixErrorCode.Unknown => @"M_UNKNOWN",
                MatrixErrorCode.Unrecognized => @"M_UNRECOGNIZED",
                MatrixErrorCode.Unauthorized => @"M_UNAUTHORIZED",
                MatrixErrorCode.UserDeactivated => @"M_USER_DEACTIVATED",
                MatrixErrorCode.UserInUse => @"M_USER_IN_USE",
                MatrixErrorCode.InvalidUsername => @"M_INVALID_USERNAME",
                MatrixErrorCode.RoomInUse => @"M_ROOM_IN_USE",
                MatrixErrorCode.InvalidRoomState => @"M_INVALID_ROOM_STATE",
                MatrixErrorCode.ThreepidInUse => @"M_THREEPID_IN_USE",
                MatrixErrorCode.ThreepidNotFound => @"M_THREEPID_NOT_FOUND",
                MatrixErrorCode.ThreepidAuthFailed => @"M_THREEPID_AUTH_FAILED",
                MatrixErrorCode.ThreepidDenied => @"M_THREEPID_DENIED",
                MatrixErrorCode.ServerNotTrusted => @"M_SERVER_NOT_TRUSTED",
                MatrixErrorCode.UnsupportedRoomVersion => @"M_UNSUPPORTED_ROOM_VERSION",
                MatrixErrorCode.IncompatibleRoomVersion => @"M_INCOMPATIBLE_ROOM_VERSION",
                MatrixErrorCode.BadState => @"M_BAD_STATE",
                MatrixErrorCode.GuestAccessForbidden => @"M_GUEST_ACCESS_FORBIDDEN",
                MatrixErrorCode.CaptchaNeeded => @"M_CAPTCHA_NEEDED",
                MatrixErrorCode.MissingParam => @"M_MISSING_PARAM",
                MatrixErrorCode.InvalidParam => @"M_INVALID_PARAM",
                MatrixErrorCode.TooLarge => @"M_TOO_LARGE",
                MatrixErrorCode.Exclusive => @"M_EXCLUSIVE",
                MatrixErrorCode.ResourceLimitExceeded => @"M_RESOURCE_LIMIT_EXCEEDED",
                MatrixErrorCode.CannotLeaveServerNoticeRoom => @"M_CANNOT_LEAVE_SERVER_NOTICE_ROOM",
                _ => throw new System.NotImplementedException()
            };
        }

        public static MatrixErrorCode FromJsonString(this string matrixErrorCode)
        {
            return matrixErrorCode switch
            {
                @"M_FORBIDDEN" => MatrixErrorCode.Forbidden,
                @"M_UNKNOWN_TOKEN" => MatrixErrorCode.UnknownToken,
                @"M_MISSING_TOKEN" => MatrixErrorCode.MissingToken,
                @"M_BAD_JSON" => MatrixErrorCode.BadJson,
                @"M_NOT_JSON" => MatrixErrorCode.NotJson,
                @"M_NOT_FOUND" => MatrixErrorCode.NotFound,
                @"M_LIMIT_EXCEEDED" => MatrixErrorCode.LimitExceeded,
                @"M_UNKNOWN" => MatrixErrorCode.Unknown,
                @"M_UNRECOGNIZED" => MatrixErrorCode.Unrecognized,
                @"M_UNAUTHORIZED" => MatrixErrorCode.Unauthorized,
                @"M_USER_DEACTIVATED" => MatrixErrorCode.UserDeactivated,
                @"M_USER_IN_USE" => MatrixErrorCode.UserInUse,
                @"M_INVALID_USERNAME" => MatrixErrorCode.InvalidUsername,
                @"M_ROOM_IN_USE" => MatrixErrorCode.RoomInUse,
                @"M_INVALID_ROOM_STATE" => MatrixErrorCode.InvalidRoomState,
                @"M_THREEPID_IN_USE" => MatrixErrorCode.ThreepidInUse,
                @"M_THREEPID_NOT_FOUND" => MatrixErrorCode.ThreepidNotFound,
                @"M_THREEPID_AUTH_FAILED" => MatrixErrorCode.ThreepidAuthFailed,
                @"M_THREEPID_DENIED" => MatrixErrorCode.ThreepidDenied,
                @"M_SERVER_NOT_TRUSTED" => MatrixErrorCode.ServerNotTrusted,
                @"M_UNSUPPORTED_ROOM_VERSION" => MatrixErrorCode.UnsupportedRoomVersion,
                @"M_INCOMPATIBLE_ROOM_VERSION" => MatrixErrorCode.IncompatibleRoomVersion,
                @"M_BAD_STATE" => MatrixErrorCode.BadState,
                @"M_GUEST_ACCESS_FORBIDDEN" => MatrixErrorCode.GuestAccessForbidden,
                @"M_CAPTCHA_NEEDED" => MatrixErrorCode.CaptchaNeeded,
                @"M_MISSING_PARAM" => MatrixErrorCode.MissingParam,
                @"M_INVALID_PARAM" => MatrixErrorCode.InvalidParam,
                @"M_TOO_LARGE" => MatrixErrorCode.TooLarge,
                @"M_EXCLUSIVE" => MatrixErrorCode.Exclusive,
                @"M_RESOURCE_LIMIT_EXCEEDED" => MatrixErrorCode.ResourceLimitExceeded,
                @"M_CANNOT_LEAVE_SERVER_NOTICE_ROOM" => MatrixErrorCode.CannotLeaveServerNoticeRoom,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}