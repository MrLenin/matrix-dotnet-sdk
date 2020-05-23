using System;

namespace Matrix.Structures
{
    public enum EMatrixRoomMembership
    {
        Invite,
        Join,
        Knock,
        Leave,
        Ban
    }

    public class MatrixMRoomMember : MatrixRoomStateEvent
    {
        public MatrixInvite ThirdPartyInvite { get; set; }
        public EMatrixRoomMembership Membership { get; set; }
        public Uri AvatarUrl { get; set; }
        public string DisplayName { get; set; }
    }
}