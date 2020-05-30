namespace Matrix.Structures
{
    public class Device
    {
        /// <summary>
        /// Required. Identifier of this device.
        /// </summary>
        public string DeviceId { get; protected set; }

        /// <summary>
        /// Display name set by the user for this device. Absent if no name has been set.
        /// </summary>
        public string DisplayName { get; protected set; }

        /// <summary>
        /// The IP address where this device was last seen. (May be a few minutes out of date, for efficiency reasons).
        /// </summary>
        public string LastSeenIp { get; protected set; }

        /// <summary>
        /// The timestamp (in milliseconds since the unix epoch) when this devices was last seen. (May be a few minutes out of date, for efficiency reasons).
        /// </summary>
        public int LastSeenTimeStamp { get; protected set; }
    }
}