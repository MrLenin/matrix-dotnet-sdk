using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.Events.RoomStateContent;
using Matrix.Properties;
using Matrix.Structures;

using Newtonsoft.Json;

namespace Matrix.Client
{
    public delegate void RoomEventDelegate(MatrixRoom room, RoomEvent roomEvent);
    public delegate void RoomMessageEventDelegate(MatrixRoom room, IRoomMessageEventContent roomMessageEventContent);

    public delegate void MatrixRoomChangeDelegate();

    public delegate void MatrixRoomReceiptDelegate(string eventId, ReceiptedEvent receipts);

    public delegate void MatrixRoomTypingDelegate(IEnumerable<string> userIds);

    public delegate void RoomMembershipEventDelegate(string userId, MembershipEventContent roomMembershipEventContent);

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

        public SortedDictionary<string, MembershipEventContent> Members { get; private set; }

        /// <summary>
        /// Should this Matrix Room federate with other home servers?
        /// </summary>
        /// <value><c>true</c> if should federate; otherwise, <c>false</c>.</value>
        public bool ShouldFederate { get; private set; }

        public string CanonicalAlias { get; private set; }
        public IEnumerable<string> Aliases { get; private set; }

        public JoinRulesKind JoinRule { get; private set; }
        public MatrixMRoomPowerLevels PowerLevels { get; private set; }

        /// <summary>
        /// Occurs when a m.room.message is received.
        /// <remarks>This will include your own messages</remarks>
        /// </summary>
        public event RoomMessageEventDelegate OnMessage;

        public event MatrixRoomChangeDelegate OnEphemeralChanged;
        public event MatrixRoomTypingDelegate OnTypingChanged;
        public event MatrixRoomReceiptDelegate OnReceiptsReceived;

        public event RoomMembershipEventDelegate OnUserJoined;
        public event RoomMembershipEventDelegate OnUserChange;
        public event RoomMembershipEventDelegate OnUserLeft;
        public event RoomMembershipEventDelegate OnUserInvited;
        public event RoomMembershipEventDelegate OnUserBanned;


        private readonly JsonSerializer _jsonSerializer;

        /// <summary>
        /// Fires when any room message is received.
        /// </summary>
        //public event RoomEventDelegate OnEvent;

        /// <summary>
        /// Don't fire OnMessage if the message exceeds this age limit (in milliseconds). Set to -1 to ignore.
        /// </summary>
        public int MessageMaximumAge { get; set; } = -1;

        private readonly List<MatrixMRoomMessage> _messages = new List<MatrixMRoomMessage>(MessageCapacity);

        private IEnumerable<IEvent> _ephemeral;

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
                        if (userId == _api.UserId ||
                            roomMember.MembershipState == MembershipState.Leave && skippingLeave)
                            continue;

                        if (Members.Count == 2)
                            return roomMember.DisplayName;

                        if (Members.Count <= 2) continue;

                        var res = $"{roomMember.DisplayName} and {Members.Count} others";
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
            _jsonSerializer = new JsonSerializer();
            Members = new SortedDictionary<string, MembershipEventContent>();
        }

        /// <summary>
        /// This method is intended for the API only.
        /// If a Room receives a new event, process it in here.
        /// </summary>
        /// <param name="roomEvent">New event</param>
        public void FeedEvent(RoomEvent roomEvent)
        {
            if (roomEvent == null)
                throw new ArgumentNullException(nameof(roomEvent));

            if (roomEvent.Content == null)
                return; // We can't operate on this
            if (roomEvent.GetType() == typeof(RoomStateEvent))
            {
                var roomStateEvent = roomEvent as RoomStateEvent;
                switch (roomStateEvent.EventKind)
                {
                    case EventKind.RoomAvatar:
                        break;
                    case EventKind.RoomCanonicalAlias:
                        var canonicalAliasEventContent = (CanonicalAliasEventContent)roomStateEvent.Content;
                        CanonicalAlias = canonicalAliasEventContent.Alias;
                        Aliases = canonicalAliasEventContent.AlternateAliases;
                        break;

                    case EventKind.RoomCreate:
                        var createEventContent = (CreateEventContent)roomStateEvent.Content;
                        Creator = createEventContent.Creator;
                        ShouldFederate = createEventContent.Federate;
                        break;

                    case EventKind.RoomGuestAccess:
                        break;
                    case EventKind.RoomHistoryVisibility:
                        break;

                    case EventKind.RoomJoinRules:
                        var joinRulesEventContent = (JoinRulesEventContent)roomStateEvent.Content;
                        JoinRule = joinRulesEventContent.JoinRulesKind;
                        break;

                    case EventKind.RoomMembership:
                        var membershipContent = (MembershipEventContent) roomStateEvent.Content;

                        if (!_api.Sync.IsInitialSync)
                        {
                            //Handle new join,leave etc
                            RoomMembershipEventDelegate eventDelegate = null;
                            switch (membershipContent.MembershipState)
                            {
                                case MembershipState.Invite:
                                    eventDelegate = OnUserInvited;
                                    break;
                                case MembershipState.Join:
                                    eventDelegate = Members.ContainsKey(roomStateEvent.StateKey)
                                        ? OnUserChange
                                        : OnUserJoined;
                                    break;
                                case MembershipState.Leave:
                                    eventDelegate = OnUserLeft;
                                    break;
                                case MembershipState.Ban:
                                    eventDelegate = OnUserBanned;
                                    break;
                                case MembershipState.Knock:
                                    break;
                                default:
                                    throw new IndexOutOfRangeException(nameof(membershipContent.MembershipState));
                            }

                            eventDelegate?.Invoke(roomStateEvent.StateKey, membershipContent);
                        }

                        Members[roomStateEvent.StateKey] = membershipContent;
                        break;

                    case EventKind.RoomName:
                        break;
                    case EventKind.RoomPinnedEvents:
                        break;
                    case EventKind.RoomPowerLevels:
                        break;
                    case EventKind.RoomServerAcl:
                        break;
                    case EventKind.RoomThirdPartyInvite:
                        break;
                    case EventKind.RoomTombstone:
                        break;
                    case EventKind.RoomTopic:
                        break;
                }
            }
            else
            {
                switch (roomEvent.EventKind)
                {
                    case EventKind.RoomMessage:
                        break;
                    case EventKind.RoomRedaction:
                        break;
                }
            }
//            if (t == typeof(MatrixMRoomCreate))
//            {
//                var create = (MatrixMRoomCreate) roomEvent.Content;
//                Creator = create.Creator;
//                ShouldFederate = create.Federated;
//            }
//            else if (t == typeof(MatrixMRoomName))
//            {
//                Name = ((MatrixMRoomName) roomEvent.Content).Name;
//            }
//            else if (t == typeof(MatrixMRoomTopic))
//            {
//                Topic = ((MatrixMRoomTopic) roomEvent.Content).Topic;
//            }
//            else if (t == typeof(MatrixMRoomPowerLevels))
//            {
//                PowerLevels = (MatrixMRoomPowerLevels) roomEvent.Content;
//            }
//            else if (typeof(MatrixMRoomMessage).IsAssignableFrom(t))
//            {
//                _messages.Add((MatrixMRoomMessage) roomEvent.Content);
//                if (OnMessage != null)
//                    if (MessageMaximumAge <= 0 || roomEvent.Age <= MessageMaximumAge)
//                        try
//                        {
//                            OnMessage.Invoke(this, roomEvent);
//                        }
//                        catch (Exception e)
//                        {
//#if DEBUG
//                            Console.WriteLine("A OnMessage handler failed");
//                            Console.WriteLine(e);
//                            throw;
//#endif
//                        }
//            }

            //OnEvent?.Invoke(this, roomEvent);
        }


        /// <summary>
        /// Attempt to set the name of the room.
        /// This may fail if you do not have the required permissions.
        /// </summary>
        /// <param name="newName">New name.</param>
        public void SetName(string newName)
        {
            var nameEvent = new NameEventContent()
            {
                Name = newName
            };

            _api.Room.SendState(Id, "m.room.name", nameEvent);
        }

        /// <summary>
        /// Attempt to set the topic of the room.
        /// This may fail if you do not have the required permissions.
        /// </summary>
        /// <param name="newTopic">New topic.</param>
        public void SetTopic(string newTopic)
        {
            var topicEvent = new TopicEventContent()
            {
                Topic = newTopic
            };
            _api.Room.SendState(Id, "m.room.topic", topicEvent);
        }

        /// <summary>
        /// Send a new message to the room.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Event ID of the sent message</returns>
        public string SendMessage(MatrixMRoomMessage message)
        {
            var t = _api.Room.SendMessage(Id, "m.room.message", message);
            t.Wait();
            return t.Result;
        }

        public Task<string> SendMessageAsync(MatrixMRoomMessage message)
        {
            return _api.Room.SendMessage(Id, "m.room.message", message);
        }

        /// <summary>
        /// Send a MMessageText message to the room.
        /// </summary>
        /// <param name="body">The string body of the message</param>
        /// <returns>Event ID of the sent message</returns>
        public string SendText(string body)
        {
            var message = new MMessageText
            {
                Body = body
            };
            return SendMessage(message);
        }

        public string SendNotice(string notice)
        {
            var message = new MMessageNotice
            {
                Body = notice
            };
            return SendMessage(message);
        }

        /// <summary>
        /// Sends a state message.
        /// </summary>
        /// <param name="stateMessage">State message.</param>
        /// <param name="type">Type.</param>
        /// <param name="key">Key.</param>
        /// <returns>Event ID of the sent message</returns>
        public string SendState<T>(T stateMessage, string type, string key = "")
            where T : class, IRoomStateEventContent
        {
            return _api.Room.SendState<T>(Id, type, stateMessage, key);
        }

        /// <summary>
        /// Sets whether the current user is typing.
        /// </summary>
        /// <param name="typing">Whether the user is typing or not. If false, the timeout key can be omitted.</param>
        /// <param name="timeout">The length of time in milliseconds to mark this user as typing.</param>
        public void SetTyping(bool typing, int timeout = 30000)
        {
            _api.Room.SendTyping(Id, typing, timeout);
        }

        /// <summary>
        /// Applies the new power levels.
        /// <remarks> You must set all the values in powerlevels.</remarks>
        /// </summary>
        /// <param name="powerlevels">Powerlevels.</param>
        public void ApplyNewPowerLevels(PowerLevelsEventContent powerlevels)
        {
            _api.Room.SendState(Id, "m.room.power_levels", powerlevels);
        }

        /// <summary>
        /// Invite a user to the room by userid.
        /// </summary>
        /// <param name="userid">Userid.</param>
        public void InviteToRoom(string userid)
        {
            _api.Room.InviteTo(Id, userid);
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
            _api.Room.Leave(Id);
        }

        public void SetMemberDisplayName(string displayName)
        {
            if (!Members.TryGetValue(_api.UserId, out var member))
                throw new MatrixException(Resources.UserMembershipEventNotFound);

            member.DisplayName = displayName;
            SendState(member, "m.room.member", _api.UserId);
        }

        public void SetMemberAvatar(Uri avatarUrl)
        {
            if (!Members.TryGetValue(_api.UserId, out var member))
                throw new MatrixException(Resources.UserMembershipEventNotFound);

            member.AvatarUrl = avatarUrl;
            SendState(member, "m.room.member", _api.UserId);
        }

        public void SetEphemeral(MatrixSyncEvents matrixSyncEvents)
        {
            if (matrixSyncEvents == null)
                throw new ArgumentNullException(nameof(matrixSyncEvents));

            _ephemeral = matrixSyncEvents.Events;

            foreach (var matrixEvent in _ephemeral)
                switch (matrixEvent)
                {
                    case ReceiptEvent receiptEvent:
                        var rec = receiptEvent.Content;
                        foreach (var (eventId, receiptedUsers) in rec.ReceiptedEvents)
                            OnReceiptsReceived?.Invoke(eventId, receiptedUsers);
                        break;

                    //case TypingEvent typingEvent:
                    //    OnTypingChanged?.Invoke(typingEvent.Content.UserIds);
                    //    break;

                    default:
                        continue;
                };

            OnEphemeralChanged?.Invoke();
        }

        //TODO: Give this parameters
        public ChunkedMessages FetchMessages()
        {
            return _api.Room.GetMessages(Id);
        }

        public RoomTags GetTags()
        {
            return _api.Room.GetTags(Id);
        }

        public void SetTag(string tagName, double order = 0)
        {
            _api.Room.PutTag(Id, tagName, order);
        }

        public IRoomStateEventContent GetStateEvent(string type)
        {
            var evContent = _api.Room.GetStateType(Id, type);
            var fakeEvent = new RoomStateEvent() {Content = evContent};
            FeedEvent(fakeEvent);
            return evContent;
        }
    }
}