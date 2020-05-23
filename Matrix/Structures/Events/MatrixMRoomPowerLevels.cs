using System;
using System.Collections.Generic;

namespace Matrix.Structures
{
    /// <summary>
    /// http://matrix.org/docs/spec/r0.0.1/client_server.html#m-room-power-levels
    /// </summary>
    public class MatrixMRoomPowerLevels : MatrixRoomStateEvent
    {
        public Dictionary<string, int> Users { get; }
            = new Dictionary<string, int>();
        public int StateDefault { get; set; }
        public int UsersDefault { get; set; }
        public int EventsDefault { get; set; }
        public int Redact { get; set; }
        public int Ban { get; set; }
        public int Kick { get; set; }
        public Dictionary<string, int> Events { get; }
            = new Dictionary<string, int>();

        public int UserPowerLevel(string userId)
        {
            return Users.ContainsKey(userId) ? Users[userId] : UsersDefault;
        }
    }
}