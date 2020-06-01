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
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomEventContent;
using Matrix.Api.ClientServer.StateEventContent;
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
                .RegisterSubtype(typeof(StateEvent<AvatarEventContent>), EventKind.RoomAvatar.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<CanonicalAliasEventContent>), EventKind.RoomCanonicalAlias.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<CreateEventContent>), EventKind.RoomCreate.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<GuestAccessEventContent>), EventKind.RoomGuestAccess.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<HistoryVisibilityEventContent>), EventKind.RoomHistoryVisibility.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<JoinRuleEventContent>), EventKind.RoomJoinRule.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<MembershipEventContent>), EventKind.RoomMembership.ToJsonString())
                .RegisterSubtype(typeof(RoomEvent<IMessageEventContent>), EventKind.RoomMessage.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<NameEventContent>), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<PinnedEventsEventContent>), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<PowerLevelsEventContent>), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(RoomEvent<RedactionEventContent>), EventKind.RoomRedaction.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<ServerAclEventContent>), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<ThirdPartyInviteEventContent>), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<TombstoneEventContent>), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<TopicEventContent>), EventKind.RoomTopic.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IStrippedState), @"type")
                .RegisterSubtype(typeof(StrippedState<AvatarEventContent>), EventKind.RoomAvatar.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<CanonicalAliasEventContent>), EventKind.RoomCanonicalAlias.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<CreateEventContent>), EventKind.RoomCreate.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<GuestAccessEventContent>), EventKind.RoomGuestAccess.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<HistoryVisibilityEventContent>), EventKind.RoomHistoryVisibility.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<JoinRuleEventContent>), EventKind.RoomJoinRule.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<MembershipEventContent>), EventKind.RoomMembership.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<NameEventContent>), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<PinnedEventsEventContent>), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<PowerLevelsEventContent>), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<ServerAclEventContent>), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<ThirdPartyInviteEventContent>), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<TombstoneEventContent>), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StrippedState<TopicEventContent>), EventKind.RoomTopic.ToJsonString())
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
                .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
                .Build());

            //_jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
            //    .Of(typeof(IRoomEventContent))
            //    .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
            //    .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IMessageEventContent), @"msgtype")
                .RegisterSubtype(typeof(AudioMessageEventContent), MessageKind.Audio.ToJsonString())
                .RegisterSubtype(typeof(EmoteMessageEventContent), MessageKind.Emote.ToJsonString())
                .RegisterSubtype(typeof(FileMessageEventContent), MessageKind.File.ToJsonString())
                .RegisterSubtype(typeof(ImageMessageEventContent), MessageKind.Image.ToJsonString())
                .RegisterSubtype(typeof(LocationMessageEventContent), MessageKind.Location.ToJsonString())
                .RegisterSubtype(typeof(NoticeMessageEventContent), MessageKind.Notice.ToJsonString())
                .RegisterSubtype(typeof(ServerNoticeMessageEventContent), MessageKind.ServerNotice.ToJsonString())
                .RegisterSubtype(typeof(TextMessageEventContent), MessageKind.Text.ToJsonString())
                //.RegisterSubtype(typeof(VideoMessageEventContent), MessageKind.Video.ToJsonString())
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IStateEventContent))
                .RegisterSubtypeWithProperty(typeof(CanonicalAliasEventContent), @"alias")
                .RegisterSubtypeWithProperty(typeof(CanonicalAliasEventContent), @"alt_aliases")
                .RegisterSubtypeWithProperty(typeof(AvatarEventContent), @"url")
                .RegisterSubtypeWithProperty(typeof(CreateEventContent), @"creator")
                .RegisterSubtypeWithProperty(typeof(GuestAccessEventContent), @"guest_access")
                .RegisterSubtypeWithProperty(typeof(HistoryVisibilityEventContent), @"history_visibility")
                .RegisterSubtypeWithProperty(typeof(JoinRuleEventContent), @"join_rule")
                .RegisterSubtypeWithProperty(typeof(MembershipEventContent), @"membership")
                .RegisterSubtypeWithProperty(typeof(NameEventContent), @"name")
                .RegisterSubtypeWithProperty(typeof(PinnedEventsEventContent), @"pinned")
                .RegisterSubtypeWithProperty(typeof(RedactionEventContent), @"redacts")
                .RegisterSubtypeWithProperty(typeof(ThirdPartyInviteEventContent), @"key_validity_url")
                .RegisterSubtypeWithProperty(typeof(TombstoneEventContent), @"replacement_room")
                .RegisterSubtypeWithProperty(typeof(TopicEventContent), @"topic")
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IRequest), @"type")
                .RegisterSubtype(typeof(PasswordAuthenticationRequest<>), AuthenticationKind.Password.ToJsonString())
                .RegisterSubtype(typeof(TokenAuthenticationRequest), AuthenticationKind.Token.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IAuthenticationIdentifier), @"type")
                .RegisterSubtype(typeof(UserAuthenticationIdentifier), UserAuthenticationIdentifier.ToJsonString())
                .RegisterSubtype(typeof(ThirdPartyAuthenticationIdentifier),
                    ThirdPartyAuthenticationIdentifier.ToJsonString())
                .RegisterSubtype(typeof(PhoneAuthenticationIdentifier), PhoneAuthenticationIdentifier.ToJsonString())
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