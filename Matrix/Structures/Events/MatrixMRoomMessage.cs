using System;

namespace Matrix.Structures
{
    public class MatrixMRoomMessage : MatrixEventContent
    {
        public virtual string MessageType { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            return Body;
        }
    }

    public class MMessageNotice : MatrixMRoomMessage
    {
        public override string MessageType => "m.notice";
    }

    public class MMessageText : MatrixMRoomMessage
    {
        public override string MessageType => "m.text";
    }

    public class MMessageEmote : MatrixMRoomMessage
    {
        public override string MessageType => "m.emote";
    }

    public class MMessageImage : MatrixMRoomMessage
    {
        public override string MessageType => "m.image";
        public MatrixImageInfo Info { get; set; }
        public MatrixImageInfo ThumbnailInfo { get; set; }
        public Uri Url { get; set; }
        public Uri ThumbnailUrl { get; set; }
    }

    public class MMessageFile : MatrixMRoomMessage
    {
        public override string MessageType => "m.file";
        public MatrixFileInfo Info { get; set; }
        public MatrixImageInfo ThumbnailInfo { get; set; }
        public Uri Url { get; set; }
        public Uri ThumbnailUrl { get; set; }
        public string FileName { get; set; }
    }

    public class MMessageLocation : MatrixMRoomMessage
    {
        public override string MessageType => "m.location";
        public Uri GeoUrl { get; set; }
        public Uri ThumbnailUrl { get; set; }
        public MatrixImageInfo ThumbnailInfo { get; set; }
    }

    public class MMessageCustomHtml : MatrixMRoomMessage
    {
        public override string MessageType => "m.notice";
        public string Format => "org.matrix.custom.html";
        public string FormattedBody { get; set; }
    }
}