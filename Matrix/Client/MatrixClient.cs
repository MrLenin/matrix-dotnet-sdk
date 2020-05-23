using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Matrix.Structures;
using Microsoft.Extensions.Logging;

namespace Matrix.Client
{
    /// <summary>
    /// The Matrix Client is a wrapper over the MatrixAPI object which provides a safe managed way
    /// to interact with a Matrix Home Server.
    /// </summary>
    public class MatrixClient : IDisposable
    {
        private readonly ILogger _log = Logger.Factory.CreateLogger<MatrixClient>();

        public delegate void MatrixInviteDelegate(string roomId, MatrixEventRoomInvited joined);

        /// <summary>
        /// How long to poll for a Sync request before we retry.
        /// </summary>
        /// <value>The sync timeout in milliseconds.</value>
        public int SyncTimeout
        {
            get => Api.SyncTimeout;
            set => Api.SyncTimeout = value;
        }

        private readonly ConcurrentDictionary<string, MatrixRoom> _rooms =
            new ConcurrentDictionary<string, MatrixRoom>();

        public event MatrixInviteDelegate OnInvite;

        public string UserId => Api.UserId;

        /// <summary>
        /// Get the underlying API that MatrixClient wraps. Here be dragons 🐲.
        /// </summary>
        public MatrixApi Api { get; }

        public Keys Keys { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix.Client.MatrixClient"/> class.
        /// The client will preform a connection and try to retrieve version information.
        /// If this fails, a MatrixUnsuccessfulConnection Exception will be thrown.
        /// </summary>
        /// <param name="url">URL before /_matrix/</param>
        public MatrixClient(Uri url)
        {
            _log.LogDebug($"Created new MatrixClient instance for {url}");

            Api = new MatrixApi(url);
            Keys = new Keys(Api);

            try
            {
                Api.ClientVersions();
                Api.OnSyncJoinEvent += MatrixClient_OnEvent;
                Api.OnSyncInviteEvent += MatrixClient_OnInvite;
            }
            catch (MatrixException e)
            {
                throw new MatrixException("An exception occurred while trying to connect", e);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix.Client.MatrixClient"/> class.
        /// This intended for Application Services only who want to preform actions as another user.
        /// Sync is not preformed.
        /// </summary>
        /// <param name="url">URL before /_matrix/</param>
        /// <param name="applicationToken">Application token for the AS.</param>
        /// <param name="userId">Userid as the user you intend to go as.</param>
        public MatrixClient(Uri url, string applicationToken, string userId)
        {
            Api = new MatrixApi(url, applicationToken, userId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix.Client.MatrixClient"/> class for testing.
        /// </summary>
        public MatrixClient(MatrixApi api)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            api.OnSyncJoinEvent += MatrixClient_OnEvent;
            api.OnSyncInviteEvent += MatrixClient_OnInvite;
        }

        /// <summary>
        /// Gets the sync token from the API.
        /// </summary>
        /// <returns>The sync token.</returns>
        public string GetSyncToken()
        {
            return Api.GetSyncToken();
        }

        /// <summary>
        /// Gets the access token from the API.
        /// </summary>
        /// <returns>The access token.</returns>
        public string GetAccessToken()
        {
            return Api.GetAccessToken();
        }

        public MatrixLoginResponse GetCurrentLogin()
        {
            return Api.GetCurrentLogin();
        }

        private void MatrixClient_OnInvite(string roomId, MatrixEventRoomInvited joined)
        {
            OnInvite?.Invoke(roomId, joined);
        }

        private void MatrixClient_OnEvent(string roomId, MatrixEventRoomJoined joined)
        {
            MatrixRoom matrixRoom;
            if (!_rooms.ContainsKey(roomId))
            {
                matrixRoom = new MatrixRoom(Api, roomId);
                _rooms.TryAdd(roomId, matrixRoom);
                //Update existing room
            }
            else
            {
                matrixRoom = _rooms[roomId];
            }

            joined.State.Events.ToList().ForEach(x => { matrixRoom.FeedEvent(x); });
            joined.Timeline.Events.ToList().ForEach(x => { matrixRoom.FeedEvent(x); });
            matrixRoom.SetEphemeral(joined.Ephemeral);
        }

        /// <summary>
        /// Login with a given username and password.
        /// Currently, this is the only login method the SDK supports.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="deviceId">Device ID</param>
        public MatrixLoginResponse LoginWithPassword(string username, string password, string deviceId = null)
        {
            var result = Api.ClientLogin(new MatrixLoginPassword(username, password, deviceId));
            Api.SetLogin(result);
            return result;
        }

        /// <param name="syncToken"> If you stored the sync token before, you can set it for the API here</param>
        public void StartSync(string syncToken = "")
        {
            Api.SetSyncToken(syncToken);
            Api.ClientSync();
            Api.StartSyncThreads();
        }

        /// <summary>
        /// Login with a given username and token.
        /// This method will also start a sync with the server
        /// Currently, this is the only login method the SDK supports.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="token">Access Token</param>
        public void LoginWithToken(string username, string token)
        {
            Api.ClientLogin(new MatrixLoginToken(username, token));
            Api.ClientSync();
            Api.StartSyncThreads();
        }

        /// <summary>
        /// Use existing login information when connecting to Matrix.
        /// </summary>
        /// <param name="userId">Full Matrix user id.</param>
        /// <param name="accessToken">Access token.</param>
        public void UseExistingToken(string userId, string accessToken)
        {
            Api.SetLogin(new MatrixLoginResponse
            {
                UserId = userId,
                AccessToken = accessToken,
                Homeserver = Api.BaseUrl
            });
        }

        /// <summary>
        /// Get information about a user from the server.
        /// </summary>
        /// <returns>A MatrixUser object</returns>
        /// <param name="userId">User ID</param>
        public MatrixUser GetUser(string userId = null)
        {
            userId ??= Api.UserId;
            var profile = Api.ClientProfile(userId);
            return profile != null ? new MatrixUser(profile, userId) : null;
        }

        public void SetDisplayName(string displayName)
        {
            Api.ClientSetDisplayName(Api.UserId, displayName);
        }

        public void SetAvatar(Uri avatarUrl)
        {
            Api.ClientSetAvatar(Api.UserId, avatarUrl);
        }

        /// <summary>
        /// Get all the Rooms that the user has joined.
        /// </summary>
        /// <returns>Array of MatrixRooms</returns>
        public IEnumerable<MatrixRoom> GetAllRooms()
        {
            return _rooms.Values;
        }

        /// <summary>
        /// Creates a new room with the specified details, or a blank one otherwise.
        /// </summary>
        /// <returns>A MatrixRoom object</returns>
        /// <param name="roomDetails">Optional set of options to send to the server.</param>
        public MatrixRoom CreateRoom(MatrixCreateRoom roomDetails = null)
        {
            var roomId = Api.ClientCreateRoom(roomDetails);
            if (roomId == null) return null;

            var room = JoinRoom(roomId);
            return room;
        }

        /// <summary>
        /// Alias for <see cref="MatrixClient.CreateRoom(MatrixCreateRoom)"/> which lets you set common items before creation.
        /// </summary>
        /// <returns>A MatrixRoom object</returns>
        /// <param name="name">The room name.</param>
        /// <param name="alias">The primary alias</param>
        /// <param name="topic">The room topic</param>
        public MatrixRoom CreateRoom(string name, string alias = null, string topic = null)
        {
            var room = new MatrixCreateRoom
            {
                Name = name,
                RoomAliasName = alias,
                Topic = topic
            };
            return CreateRoom(room);
        }

        /// <summary>
        /// Join a matrix room. If the user has already joined this room, do nothing.
        /// </summary>
        /// <returns>The room.</returns>
        /// <param name="roomId">roomId or alias</param>
        public MatrixRoom JoinRoom(string roomId)
        {
            if (_rooms.ContainsKey(roomId)) return _rooms[roomId];

            roomId = Api.ClientJoin(roomId);

            if (roomId == null)
                return null;

            var room = new MatrixRoom(Api, roomId);
            _rooms.TryAdd(room.Id, room);

            return _rooms[roomId];
        }

        public MatrixMediaFile UploadFile(string contentType, byte[] data)
        {
            var url = Api.MediaUpload(contentType, data);
            return new MatrixMediaFile(Api, url, contentType);
        }

        /// <summary>
        /// Return a joined room object by it's roomId.
        /// </summary>
        /// <returns>The room.</returns>
        /// <param name="roomId">Room ID.</param>
        public MatrixRoom GetRoom(string roomId)
        {
            _rooms.TryGetValue(roomId, out var room);
            if (room != null) return room;

            _log.LogInformation($"Don't have {roomId} synced, getting the room from /state");
            // If we don't have the room, attempt to grab it's state.
            var state = Api.GetRoomState(roomId);
            room = new MatrixRoom(Api, roomId);
            foreach (var matrixEvent in state)
                room.FeedEvent(matrixEvent);

            _rooms.TryAdd(roomId, room);
            return room;
        }

        /// <summary>
        /// Get a room object by any of it's registered aliases.
        /// </summary>
        /// <returns>The room by alias.</returns>
        /// <param name="alias">CanonicalAlias or any Alias</param>
        public MatrixRoom GetRoomByAlias(string alias)
        {
            var room = _rooms.Values.FirstOrDefault(
                x =>
                {
                    if (x.CanonicalAlias == alias) return true;
                    return x.Aliases != null && x.Aliases.Contains(alias);
                });

            return room;
        }

        /// <summary>
        /// Add a new type of message to be decoded during sync operations.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="type">Type that inherits MatrixMRoomMessage</param>
        public void AddRoomMessageType(string messageType, Type type)
        {
            Api.AddMessageType(messageType, type);
        }

        /// <summary>
        /// Add a new type of state event to be decoded during sync operations.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="type">Type that inherits MatrixMRoomMessage</param>
        public void AddStateEventType(string messageType, Type type)
        {
            Api.AddEventType(messageType, type);
        }

        public PublicRooms GetPublicRooms(int limit = 0, string since = "", string server = "")
        {
            return Api.PublicRooms(limit, since, server);
        }

        public void DeleteFromRoomDirectory(string alias)
        {
            Api.DeleteFromRoomDirectory(alias);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="Matrix.Client.MatrixClient"/> object.
        /// In addition, this will stop the sync thread.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Matrix.Client.MatrixClient"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="Matrix.Client.MatrixClient"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the <see cref="Matrix.Client.MatrixClient"/>
        /// so the garbage collector can reclaim the memory that the <see cref="Matrix.Client.MatrixClient"/> was occupying.</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            Api.StopSyncThreads();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}