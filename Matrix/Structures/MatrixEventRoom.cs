namespace Matrix.Structures
{
    public class MatrixEventRoomLeft
    {
        public MatrixTimeline Timeline { get; set; }
        public MatrixSyncEvents State { get; set; }
    }

    public class MatrixEventRoomJoined
    {
        public MatrixTimeline Timeline { get; set; }
        public MatrixSyncEvents State { get; set; }
        public MatrixSyncEvents AccountData { get; set; }
        public MatrixSyncEvents Ephemeral { get; set; }
    }

    public class MatrixEventRoomInvited
    {
        public MatrixSyncEvents Events { get; set; }
    }
}