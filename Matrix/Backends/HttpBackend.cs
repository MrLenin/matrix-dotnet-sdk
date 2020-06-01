using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using JsonSubTypes;

using Matrix.Api;
using Matrix.Api.ClientServer;
using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.EventContent;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomContent;
using Matrix.Api.ClientServer.StateContent;
using Matrix.Api.ClientServer.Structures;
using Matrix.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix.Backends
{
    public sealed class HttpBackend : IMatrixApiBackend, IDisposable
    {
        private readonly Uri _baseUrl;
        private string _accessToken;
        private readonly string _userId;
        private readonly HttpClient _client;
        private readonly JsonSerializer _jsonSerializer;

        public HttpBackend(Uri apiUrl, string userId)
        {
            _jsonSerializer = new JsonSerializer();
            _client = new HttpClient();
            _baseUrl = apiUrl;
            _userId = userId;
            
            ServicePointManager.ServerCertificateValidationCallback += AcceptCertificate;

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IEvent), @"type")
                .RegisterSubtype(typeof(PresenceEvent), PresenceEvent.ToJsonString())
                .RegisterSubtype(typeof(ReceiptEvent), ReceiptEvent.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomAvatarContent>), EventKind.RoomAvatar.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomCanonicalAliasContent>), EventKind.RoomCanonicalAlias.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomCreateContent>), EventKind.RoomCreate.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomGuestAccessContent>), EventKind.RoomGuestAccess.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomHistoryVisibilityContent>), EventKind.RoomHistoryVisibility.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomJoinRulesContent>), EventKind.RoomJoinRule.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomMembershipContent>), EventKind.RoomMembership.ToJsonString())
                .RegisterSubtype(typeof(RoomEvent<IMessageContent>), EventKind.RoomMessage.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomNameContent>), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomPinnedEventsContent>), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomPowerLevelsContent>), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(RoomEvent<RedactionContent>), EventKind.RoomRedaction.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomServerAclContent>), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomThirdPartyInviteContent>), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomTombstoneContent>), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomTopicContent>), EventKind.RoomTopic.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IStrippedState), @"type")
                .RegisterSubtype(typeof(StrippedState<RoomAvatarContent>), EventKind.RoomAvatar.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomCanonicalAliasContent>), EventKind.RoomCanonicalAlias.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomCreateContent>), EventKind.RoomCreate.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomGuestAccessContent>), EventKind.RoomGuestAccess.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomHistoryVisibilityContent>), EventKind.RoomHistoryVisibility.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomJoinRulesContent>), EventKind.RoomJoinRule.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomMembershipContent>), EventKind.RoomMembership.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomNameContent>), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomPinnedEventsContent>), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomPowerLevelsContent>), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomServerAclContent>), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomThirdPartyInviteContent>), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomTombstoneContent>), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<RoomTopicContent>), EventKind.RoomTopic.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

            _jsonSerializer.Converters.Add(new AuthenticationKindJsonConverter());
            _jsonSerializer.Converters.Add(new ErrorCodeJsonConverter());
            _jsonSerializer.Converters.Add(new EventKindJsonConverter());
            _jsonSerializer.Converters.Add(new GuestAccessKindJsonConverter());
            _jsonSerializer.Converters.Add(new HistoryVisibilityKindJsonConverter());
            _jsonSerializer.Converters.Add(new JoinRuleKindJsonConverter());
            _jsonSerializer.Converters.Add(new MembershipStateJsonConverter());
            _jsonSerializer.Converters.Add(new MessageKindJsonConverter());
            _jsonSerializer.Converters.Add(new PresenceStatusJsonConverter());
            _jsonSerializer.Converters.Add(new ClientServerVersionJsonConverter());
            _jsonSerializer.Converters.Add(new RoomsVersionsJsonConverter());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IEventContent))
                .RegisterSubtypeWithProperty(typeof(PresenceContent), @"presence")
                .Build());

            //_jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
            //    .Of(typeof(IRoomContent))
            //    .RegisterSubtypeWithProperty(typeof(PresenceContent), @"presence")
            //    .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IMessageContent), @"msgtype")
                .RegisterSubtype(typeof(AudioMessageContent), MessageKind.Audio.ToJsonString())
                .RegisterSubtype(typeof(EmoteMessageContent), MessageKind.Emote.ToJsonString())
                .RegisterSubtype(typeof(FileMessageContent), MessageKind.File.ToJsonString())
                .RegisterSubtype(typeof(ImageMessageContent), MessageKind.Image.ToJsonString())
                .RegisterSubtype(typeof(LocationMessageContent), MessageKind.Location.ToJsonString())
                .RegisterSubtype(typeof(NoticeMessageContent), MessageKind.Notice.ToJsonString())
                .RegisterSubtype(typeof(ServerNoticeMessageContent), MessageKind.ServerNotice.ToJsonString())
                .RegisterSubtype(typeof(TextMessageContent), MessageKind.Text.ToJsonString())
                //.RegisterSubtype(typeof(VideoMessageEventContent), MessageKind.Video.ToJsonString())
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IStateContent))
                .RegisterSubtypeWithProperty(typeof(RoomCanonicalAliasContent), @"alias")
                .RegisterSubtypeWithProperty(typeof(RoomCanonicalAliasContent), @"alt_aliases")
                .RegisterSubtypeWithProperty(typeof(RoomAvatarContent), @"url")
                .RegisterSubtypeWithProperty(typeof(RoomCreateContent), @"creator")
                .RegisterSubtypeWithProperty(typeof(RoomGuestAccessContent), @"guest_access")
                .RegisterSubtypeWithProperty(typeof(RoomHistoryVisibilityContent), @"history_visibility")
                .RegisterSubtypeWithProperty(typeof(RoomJoinRulesContent), @"join_rule")
                .RegisterSubtypeWithProperty(typeof(RoomMembershipContent), @"membership")
                .RegisterSubtypeWithProperty(typeof(RoomNameContent), @"name")
                .RegisterSubtypeWithProperty(typeof(RoomPinnedEventsContent), @"pinned")
                .RegisterSubtypeWithProperty(typeof(RedactionContent), @"redacts")
                .RegisterSubtypeWithProperty(typeof(RoomThirdPartyInviteContent), @"key_validity_url")
                .RegisterSubtypeWithProperty(typeof(RoomTombstoneContent), @"replacement_room")
                .RegisterSubtypeWithProperty(typeof(RoomTopicContent), @"topic")
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IRequest), @"type")
                .RegisterSubtype(typeof(PasswordAuthRequest<>), AuthenticationKind.Password.ToJsonString())
                .RegisterSubtype(typeof(TokenAuthRequest), AuthenticationKind.Token.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IAuthIdentifier), @"type")
                .RegisterSubtype(typeof(UserAuthIdentifier), UserAuthIdentifier.ToJsonString())
                .RegisterSubtype(typeof(ThirdPartyAuthIdentifier),
                    ThirdPartyAuthIdentifier.ToJsonString())
                .RegisterSubtype(typeof(PhoneAuthIdentifier), PhoneAuthIdentifier.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());
        }

        private static bool AcceptCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true; //Find a better way to handle mono certs.
        }

        public void SetAccessToken(string token)
        {
            _accessToken = token;
        }

        private Uri GetPath(Uri apiPath, bool auth)
        {
            apiPath = new Uri(_baseUrl, apiPath);

            if (!auth) return apiPath;

            apiPath = new Uri(apiPath,
                (string.IsNullOrEmpty(apiPath.Query) ? "&" : "?") + "access_token=" + _accessToken);

            if (_userId != null)
                apiPath = new Uri(apiPath, "&user_id=" + _userId);

            return apiPath;
        }

        public TResponse Request<TResponse>(Uri url, bool authenticate, HttpContent? httpContent = null)
            where TResponse : IResponse, new()
        {
            var response = RequestAsync<TResponse>(url, authenticate, httpContent);
            response.Wait();
            return response.Result;
        }

        public TResponse Request<TResponse>(IRequest request, bool authenticate, HttpContent? httpContent = null)
            where TResponse : IResponse, new()
        {
            var response = RequestAsync<TResponse>(request, authenticate, httpContent);
            response.Wait();
            return response.Result;
        }

        public async Task<TResponse> RequestAsync<TResponse>(Uri url, bool authenticate,
            HttpContent? httpContent = null)
            where TResponse : IResponse, new()
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var absolutePath = GetPath(url, authenticate);
            var httpResponse = await _client.GetAsync(absolutePath)
                .ConfigureAwait(false);
            
            if (httpResponse.StatusCode.HasFlag(HttpStatusCode.OK))
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync().
                    ConfigureAwait(false);
                using var jsonReader = new JTokenReader(JToken.Parse(jsonString));
                var response = _jsonSerializer.Deserialize<TResponse>(jsonReader);
                if (response == null) throw new NullReferenceException(@"");
                response.HttpStatusCode = httpResponse.StatusCode;
                return response;
            }
            else
            {
                return new TResponse
                {
                    ErrorCode = ErrorCode.None,
                    HttpStatusCode = httpResponse.StatusCode
                };
            }
        }

        public async Task<TResponse> RequestAsync<TResponse>(IRequest request, bool authenticate,
            HttpContent? httpContent = null)
            where TResponse : IResponse, new()
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var absolutePath = GetPath(request.Path, authenticate);
            using var httpResponse =  request.RequestKind switch
            {
                RequestKind.Put =>
                    await _client.PutAsync(absolutePath, httpContent)
                        .ConfigureAwait(false),
                RequestKind.Post =>
                    await _client.PostAsync(absolutePath, httpContent)
                        .ConfigureAwait(false),
                RequestKind.Delete =>
                    await _client.DeleteAsync(absolutePath)
                        .ConfigureAwait(false),
            };

            if (httpResponse.StatusCode.HasFlag(HttpStatusCode.OK))
            {
                var jsonString = await httpResponse.Content.ReadAsStringAsync().
                    ConfigureAwait(false);
                using var jsonReader = new JTokenReader(JToken.Parse(jsonString));
                var response = _jsonSerializer.Deserialize<TResponse>(jsonReader);
                if (response == null) throw new NullReferenceException(@"");
                response.HttpStatusCode = httpResponse.StatusCode;
                return response;
            }
            else
            {
                return new TResponse
                {
                    ErrorCode = ErrorCode.None,
                    HttpStatusCode = httpResponse.StatusCode
                };
            }

            //catch (MatrixServerError e)
            //{
            //    var retryAfter = -1;

            //    if (e.ErrorObject.ContainsKey("retry_after_ms"))
            //        retryAfter = e.ErrorObject["retry_after_ms"].ToObject<int>();

            //    apiResult.Error = new MatrixRequestError(e.Message, e.ErrorCode, HttpStatusCode.InternalServerError,
            //        retryAfter);
            //}
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}