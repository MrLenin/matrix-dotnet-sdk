using System.Collections.Generic;

using Matrix.Api.ClientServer.Events;

namespace Matrix.Structures
{
    /// <summary>
    /// From http://matrix.org/docs/spec/r0.0.1/client_server.html#get-matrix-client-r0-sync
    /// </summary>
    public class MatrixSync
    {
        public string NextBatch { get; set; }
        public MatrixSyncEvents AccountData { get; set; }
        public MatrixSyncEvents Presence { get; set; }
        public MatrixSyncRooms Rooms { get; set; }
    }

    public class MatrixSyncEvents
    {
        public IEnumerable<IEvent> Events { get; set; }
    }

    public class MatrixSyncRooms
    {
        public Dictionary<string, MatrixEventRoomInvited> Invite { get; set; }
        public Dictionary<string, MatrixEventRoomJoined> Join { get; set; }
        public Dictionary<string, MatrixEventRoomLeft> Leave { get; set; }
    }
}