namespace Matrix.Backends
{
    // Taken from https://matrix.org/docs/spec/client_server/unstable.html#api-standards
    public enum MatrixErrorCode
    {
        Forbidden,
        UnknownToken,
        BadJson,
        NotJson,
        NotFound,
        LimitExceeded,
        UserInUse,
        RoomInUse,
        BadPagination,
        Exclusive,
        Unknown,
        TooLarge,
        UnknownErrorCode,
        None
    }
}