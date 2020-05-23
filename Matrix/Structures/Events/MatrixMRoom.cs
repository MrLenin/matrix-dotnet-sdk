using System.Collections.Generic;
using System.Dynamic;

namespace Matrix.Structures
{
    public enum EMatrixRoomJoinRules
    {
        Public,
        Knock,
        Invite,
        Private
    }

    public enum EMatrixRoomHistoryVisibility
    {
        Invited,
        Joined,
        Shared,
        WorldReadable
    }

    public class MatrixRoomStateEvent : MatrixEventContent
    {
    }

    public class MatrixMRoomAliases : MatrixRoomStateEvent
    {
        public IEnumerable<string> Aliases { get; set; }
    }

    public class MatrixMRoomCanonicalAlias : MatrixRoomStateEvent
    {
        public string Alias { get; set; }
    }

    public class MatrixMRoomCreate : MatrixRoomStateEvent
    {
        public bool Federated { get; set; } = true;
        public string Creator { get; set; }
    }

    public class MatrixMRoomJoinRules : MatrixRoomStateEvent
    {
        public EMatrixRoomJoinRules JoinRule { get; set; }
    }

    public class MatrixMRoomName : MatrixRoomStateEvent
    {
        public string Name { get; set; }
    }

    public class MatrixMRoomTopic : MatrixRoomStateEvent
    {
        public string Topic { get; set; }
    }

    public class MatrixMRoomHistoryVisibility : MatrixRoomStateEvent
    {
        public EMatrixRoomHistoryVisibility HistoryVisibility { get; set; }
    }
}