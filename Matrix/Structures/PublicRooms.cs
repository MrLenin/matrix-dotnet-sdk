using System;
using System.Collections.Generic;

namespace Matrix.Structures
{
    public class PublicRooms
    {
        public IEnumerable<PublicRoomsChunk> Chunk { get; set; }
        public string NextBatch { get; set; }
        public string PrevBatch { get; set; }
        public int TotalRoomCountEstimate { get; set; }
    }

    public class PublicRoomsChunk
    {
        public IEnumerable<string> Aliases { get; set; }
        public string CanonicalAlias { get; set; }
        public string Name { get; set; }
        public int NumJoinedMembers { get; set; }
        public string RoomId { get; set; }
        public string Topic { get; set; }
        public bool WorldReadable { get; set; }
        public bool GuestCanJoin { get; set; }
        public Uri AvatarUrl { get; set; }
    }
}