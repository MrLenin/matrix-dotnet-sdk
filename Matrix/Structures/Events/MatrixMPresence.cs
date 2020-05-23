using System;

namespace Matrix.Structures
{
    public enum EMatrixPresence
    {
        Online,
        Offline,
        Unavailable,
        FreeForChat,
        Hidden
    }

    public class MatrixMPresence : MatrixEventContent
    {
        public string UserId { get; set; }
        public long LastActiveAgo { get; set; }
        public Uri AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public EMatrixPresence Presence { get; set; }
    }
}