using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Matrix.Structures;

namespace Matrix.Client
{

    public delegate void MatrixRoomEventDelegate(MatrixRoom room, MatrixEvent evt);

    public delegate void MatrixRoomChangeDelegate();

    public delegate void MatrixRoomReceiptDelegate(string eventId, MatrixReceipts receipts);

    public delegate void MatrixRoomTypingDelegate(string[] userIds);

    public delegate void MatrixRoomMemberEvent(string userId, MatrixMRoomMember member);

    /// <summary>
    /// A room that the user has joined on Matrix.
    /// </summary>
    public class MatrixRoom
    {
        private const int MessageCapacity = 255;

        /// <summary>
        /// The server assigned ID for the room. This can never change.
        /// </summary>
        public string Id { get; }

        public string Name { get; private set; }
        public string Topic { get; private set; }
        public string Creator { get; private set; }

        public SortedDictionary<string,MatrixMRoomMember> Members { get; private set; }

        /// <summary>
        /// Should this Matrix Room federate with other home servers?
        /// </summary>
        /// <value><c>true</c> if should federate; otherwise, <c>false</c>.</value>
        public bool ShouldFederate { get; private set; }

        public string CanonicalAlias { get; private set; }
        public IEnumerable<string> Aliases { get; private set; }

        public EMatrixRoomJoinRules JoinRule { get; private set; }
        public MatrixMRoomPowerLevels PowerLevels { get; private set; }

        /// <summary>
        /// Occurs when a m.room.message is received.
        /// <remarks>This will include your own messages</remarks>
        /// </summary>
        public event MatrixRoomEventDelegate OnMessage;

        public event MatrixRoomChangeDelegate OnEphemeralChanged;
        public event MatrixRoomTypingDelegate OnTypingChanged;
        public event MatrixRoomReceiptDelegate OnReceiptsReceived;

        public event MatrixRoomMemberEvent OnUserJoined;
        public event MatrixRoomMemberEvent OnUserChange;
        public event MatrixRoomMemberEvent OnUserLeft;
        public event MatrixRoomMemberEvent OnUserInvited;
        public event MatrixRoomMemberEvent OnUserBanned;


        /// <summary>
        /// Fires when any room message is received.
        /// </summary>
        public event MatrixRoomEventDelegate OnEvent;

        /// <summary>
        /// Don't fire OnMessage if the message exceeds this age limit (in milliseconds). Set to -1 to ignore.
        /// </summary>
        public int MessageMaximumAge { get; set; } = -1;

        private readonly List<MatrixMRoomMessage> _messages = new List<MatrixMRoomMessage>(MessageCapacity);

        private MatrixEvent[] _ephemeral;

        /// <summary>
        /// Get a list of all the messages received so far.
        /// <remarks>This is not a complete list for the rooms entire history</remarks>
        /// </summary>
        public IEnumerable<MatrixMRoomMessage> Messages => _messages;

        public string HumanReadableName
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                    return Name;

                if (!string.IsNullOrEmpty(CanonicalAlias))
                    return CanonicalAlias;

                var skippingLeave = true;

                while (true)
                {
                    foreach (var (userId, roomMember) in Members)
                    {
                        if (userId == _api.UserId || roomMember.membership == EMatrixRoomMembership.Leave && skippingLeave)
                            continue;

                        if (Members.Count == 2)
                            return roomMember.displayname;

                        if (Members.Count <= 2) continue;

                        var res = $"{roomMember.displayname} and {Members.Count} others";
                        return skippingLeave ? res : $"Empty room (was {res})";
                    }

                    if (!skippingLeave)
                        return "Empty Room";
                    
                    skippingLeave = false;
                }
            }
        }

        private readonly MatrixApi _api;

        /// <summary>
        /// This constructor is intended for the API only.
        /// Initializes a new instance of the <see cref="Matrix.Client.MatrixRoom"/> class.
        /// </summary>
        /// <param name="api">The API to send/recieve requests from</param>
        /// <param name="roomId">Roomid</param>
        public MatrixRoom(MatrixApi api, string roomId)
        {
            Id = roomId;
            _api = api;
            Members = new SortedDictionary<string, MatrixMRoomMember>();
        }

        /// <summary>
        /// This method is intended for the API only.
        /// If a Room receives a new event, process it in here.
        /// </summary>
        /// <param name="matrixEvent">New event</param>
        public void FeedEvent(MatrixEvent matrixEvent)
        {
            if (matrixEvent == null)
                throw new ArgumentNullException(nameof(matrixEvent));

            if (matrixEvent.content == null)
                return; // We can't operate on this

            var t = matrixEvent.content.GetType();

            if (t == typeof(MatrixMRoomCreate))
            {
                var create = (MatrixMRoomCreate)matrixEvent.content;
                Creator = create.creator;
                ShouldFederate = create.mfederate;
            }
            else if (t == typeof(MatrixMRoomName))
            {
                Name = ((MatrixMRoomName)matrixEvent.content).name;
            }
            else if (t == typeof(MatrixMRoomTopic))
            {
                Topic = ((MatrixMRoomTopic)matrixEvent.content).topic;
            }
            else if (t == typeof(MatrixMRoomAliases))
            {
                Aliases = ((MatrixMRoomAliases)matrixEvent.content).aliases;
            }
            else if (t == typeof(MatrixMRoomCanonicalAlias))
            {
                CanonicalAlias = ((MatrixMRoomCanonicalAlias)matrixEvent.content).alias;
            }
            else if (t == typeof(MatrixMRoomJoinRules))
            {
                JoinRule = ((MatrixMRoomJoinRules)matrixEvent.content).join_rule;
            }
            else if (t == typeof(MatrixMRoomPowerLevels))
            {
                PowerLevels = ((MatrixMRoomPowerLevels)matrixEvent.content);
            }
            else if (t == typeof(MatrixMRoomMember))
            {
                var member = (MatrixMRoomMember)matrixEvent.content;
                
                if (!_api.RunningInitialSync)
                {
                    //Handle new join,leave etc
                    MatrixRoomMemberEvent Event = null;
                    switch (member.membership)
                    {
                        case EMatrixRoomMembership.Invite:
                            Event = OnUserInvited;
                            break;
                        case EMatrixRoomMembership.Join:
                            Event = Members.ContainsKey(matrixEvent.state_key) ? OnUserChange : OnUserJoined;
                            break;
                        case EMatrixRoomMembership.Leave:
                            Event = OnUserLeft;
                            break;
                        case EMatrixRoomMembership.Ban:
                            Event = OnUserBanned;
                            break;
                        case EMatrixRoomMembership.Knock:
                            break;
                        default:
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                            throw new ArgumentOutOfRangeException(nameof(member.membership));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
                    }

                    Event?.Invoke(matrixEvent.state_key, member);
                }

                Members[matrixEvent.state_key] = member;
            }
            else if (typeof(MatrixMRoomMessage).IsAssignableFrom(t))
            {
                _messages.Add((MatrixMRoomMessage) matrixEvent.content);
                if (OnMessage != null)
                {
                    if (MessageMaximumAge <= 0 || matrixEvent.age <= MessageMaximumAge)
                    {
                        try
                        {
                            OnMessage.Invoke(this, matrixEvent);
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Console.WriteLine("A OnMessage handler failed");
                            Console.WriteLine(e);
#endif
                        }
                    }
                }
            }

            OnEvent?.Invoke(this, matrixEvent);
        }



        /// <summary>
        /// Attempt to set the name of the room.
        /// This may fail if you do not have the required permissions.
        /// </summary>
        /// <param name="newName">New name.</param>
        public void SetName(string newName)
        {
            MatrixMRoomName nameEvent = new MatrixMRoomName();
            nameEvent.name = newName;
            _api.RoomStateSend(Id, "m.room.name", nameEvent);
        }

        /// <summary>
        /// Attempt to set the topic of the room.
        /// This may fail if you do not have the required permissions.
        /// </summary>
        /// <param name="newTopic">New topic.</param>
        public void SetTopic(string newTopic)
        {
            MatrixMRoomTopic topicEvent = new MatrixMRoomTopic();
            topicEvent.topic = newTopic;
            _api.RoomStateSend(Id, "m.room.topic", topicEvent);
        }

        /// <summary>
        /// Send a new message to the room.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Event ID of the sent message</returns>
        public string SendMessage(MatrixMRoomMessage message)
        {
            var t = _api.RoomMessageSend(Id, "m.room.message", message);
            t.Wait();
            return t.Result;
        }

        public Task <string> SendMessageAsync(MatrixMRoomMessage message)
        {
            return _api.RoomMessageSend(Id, "m.room.message", message);
        }

        /// <summary>
        /// Send a MMessageText message to the room.
        /// </summary>
        /// <param name="body">The string body of the message</param>
        /// <returns>Event ID of the sent message</returns>
        public string SendText(string body)
        {
            MMessageText message = new MMessageText();
            message.body = body;
            return SendMessage(message);
        }

        public string SendNotice(string notice)
        {
            MMessageNotice message = new MMessageNotice();
            message.body = notice;
            return SendMessage(message);
        }

        /// <summary>
        /// Sends a state message.
        /// </summary>
        /// <param name="stateMessage">State message.</param>
        /// <param name="type">Type.</param>
        /// <param name="key">Key.</param>
        /// <returns>Event ID of the sent message</returns>
        public string SendState(MatrixRoomStateEvent stateMessage, string type, string key = "")
        {
            return _api.RoomStateSend(Id, type, stateMessage, key);
        }

        /// <summary>
        /// Sets whether the current user is typing.
        /// </summary>
        /// <param name="typing">Whether the user is typing or not. If false, the timeout key can be omitted.</param>
        /// <param name="timeout">The length of time in milliseconds to mark this user as typing.</param>
        public void SetTyping(bool typing, int timeout = 30000)
        {
            _api.RoomTypingSend(Id, typing, timeout);
        }

        /// <summary>
        /// Applies the new power levels.
        /// <remarks> You must set all the values in powerlevels.</remarks>
        /// </summary>
        /// <param name="powerlevels">Powerlevels.</param>
        public void ApplyNewPowerLevels(MatrixMRoomPowerLevels powerlevels)
        {
            _api.RoomStateSend(Id, "m.room.power_levels", powerlevels);
        }

        /// <summary>
        /// Invite a user to the room by userid.
        /// </summary>
        /// <param name="userid">Userid.</param>
        public void InviteToRoom(string userid)
        {
            _api.InviteToRoom(Id, userid);
        }

        /// <summary>
        /// Invite a user to the room by their object.
        /// </summary>
        /// <param name="user">User.</param>
        public void InviteToRoom(MatrixUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            InviteToRoom(user.UserId);
        }

        /// <summary>
        /// Leave the room on the server.
        /// </summary>
        public void LeaveRoom()
        {
            _api.RoomLeave(Id);
        }

        public void SetMemberDisplayName(string displayname)
        {
            MatrixMRoomMember member;
            if (!Members.TryGetValue(_api.UserId, out member))
            {
                throw new MatrixException("Couldn't find the user's membership event");
            }

            member.displayname = displayname;
            SendState(member, "m.room.member", _api.UserId);
        }

        public void SetMemberAvatar(string avatar)
        {
            MatrixMRoomMember member;
            if (!Members.TryGetValue(_api.UserId, out member))
            {
                throw new MatrixException("Couldn't find the user's membership event");
            }

            member.avatar_url = avatar;
            SendState(member, "m.room.member", _api.UserId);
        }

        public void SetEphemeral(MatrixSyncEvents matrixSyncEvents)
        {
            if (matrixSyncEvents == null)
                throw new ArgumentNullException(nameof(matrixSyncEvents));

            _ephemeral = matrixSyncEvents.events;

            foreach (var matrixEvent in _ephemeral)
            {
                switch (matrixEvent.type)
                {
                    case "m.receipt":
                        var rec = (MatrixMReceipt)matrixEvent.content;
                        foreach (var (eventId, matrixReceipts) in rec.receipts)
                            OnReceiptsReceived?.Invoke(eventId, matrixReceipts);
                        break;

                    case "m.typing":
                        OnTypingChanged?.Invoke(((MatrixMTyping) matrixEvent.content).user_ids);
                        break;

                    default:
                        continue;
                }
            }

            OnEphemeralChanged?.Invoke();
        }

        //TODO: Give this parameters
        public ChunkedMessages FetchMessages()
        {
            return _api.GetRoomMessages(Id);
        }

        public RoomTags GetTags()
        {
            return _api.RoomGetTags(Id);
        }

        public void SetTag(string tagName, double order = 0)
        {
            _api.RoomPutTag(Id, tagName, order);
        }

        public MatrixEventContent GetStateEvent (string type)
        {
            var evContent = _api.GetRoomStateType(Id, type);
            var fakeEvent = new MatrixEvent {content = evContent};
            FeedEvent(fakeEvent);
            return evContent;
        }

    }
}
