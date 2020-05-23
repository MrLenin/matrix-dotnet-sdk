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
        public string user_id;
        public long last_active_ago;
        public string avatar_url;
        public string displayname;
        public EMatrixPresence presence;
    }
}