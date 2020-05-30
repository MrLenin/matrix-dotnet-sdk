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
using Matrix.Api.ClientServer.Structures;

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
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomAvatar.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomCanonicalAlias.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomCreate.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomGuestAccess.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomHistoryVisibility.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomJoinRules.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomMembership.ToJsonString())
                .RegisterSubtype(typeof(RoomEvent), EventKind.RoomMessage.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(RoomEvent), EventKind.RoomRedaction.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StateEvent), EventKind.RoomTopic.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IEventContent))
                .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IRoomEventContent))
                .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesConverterBuilder
                .Of(typeof(IRoomMessageEventContent), @"msgtype")
                .RegisterSubtype(typeof(AudioMessageEventContent), @"m.audio")
                .RegisterSubtype(typeof(EmoteMessageEventContent), @"m.emote")
                .RegisterSubtype(typeof(FileMessageEventContent), @"m.file")
                .RegisterSubtype(typeof(ImageMessageEventContent), @"m.image")
                .RegisterSubtype(typeof(LocationMessageEventContent), @"m.location")
                .RegisterSubtype(typeof(NoticeMessageEventContent), @"m.notice")
                .RegisterSubtype(typeof(ServerNoticeMessageEventContent), @"m.server_notice")
                .RegisterSubtype(typeof(TextMessageEventContent), @"m.text")
                //.RegisterSubtype(typeof(VideoMessageEventContent), @"m.text")
                .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IStateEventContent))
                .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
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
                .RegisterSubtype(typeof(ThirdPartyAuthenticationIdentifier), ThirdPartyAuthenticationIdentifier.ToJsonString())
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