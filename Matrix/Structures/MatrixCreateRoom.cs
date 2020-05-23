using System;
using System.Collections.Generic;

namespace Matrix.Structures
{
    public class MatrixCreateRoom
    {
        /// <summary>
        /// A list of user IDs to invite to the room. This will tell the server to invite everyone in the list to the newly created room.
        /// </summary>
        public IEnumerable<string> Invite { get; set; }

        /// <summary>
        /// If this is included, an m.room.name event will be sent into the room to indicate the name of the room. See Room Events for more information on m.room.name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A public visibility indicates that the room will be shown in the published room list. A private visibility will hide the room from the published room list. Rooms default to private visibility if this key is not included.
        /// </summary>
        public EMatrixCreateRoomVisibility Visibility => EMatrixCreateRoomVisibility.Private;

        /// <summary>
        /// If this is included, an m.room.topic event will be sent into the room to indicate the topic for the room. See Room Events for more information on m.room.topic.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Convenience parameter for setting various default state events based on a preset
        /// </summary>
        public EMatrixCreateRoomPreset Preset => EMatrixCreateRoomPreset.PrivateChat;

        private string _roomAliasName;

        /// <summary>
        /// The desired room alias **local part**. If this is included, a room alias will be created and mapped to the newly created room.
        /// </summary>
        /// <value>Room alias local part.</value>
        public string RoomAliasName
        {
            get => _roomAliasName;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (value.Contains("#") || value.Contains(":"))
                    throw new MatrixBadFormatException(value, "local alias", "a local alias must not contain : or #");
                _roomAliasName = value;
            }
        }

        //TODO: Add invite_3pid
        //TODO: Add creation_content
        //TODO: Add initial_state
    }

    public enum EMatrixCreateRoomVisibility
    {
        Public,
        Private
    }

    public enum EMatrixCreateRoomPreset
    {
        PrivateChat,
        PublicChat,
        TrustedPrivateChat
    }
}