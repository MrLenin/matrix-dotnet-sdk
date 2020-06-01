using System;
using System.Linq;
using System.Net;
using JsonSubTypes;

using Matrix.Api;
using Matrix.Api.ClientServer;
using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomContent;
using Matrix.Api.ClientServer.StateContent;
using Matrix.Api.ClientServer.Structures;
using Matrix.Api.Versions;
using Matrix.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace Matrix.Tests
{
    [TestFixture]
    public class JsonSerializationTests
    {
        private readonly JsonSerializer _jsonSerializer;

        public JsonSerializationTests()
        {
            _jsonSerializer = new JsonSerializer();

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
                .RegisterSubtype(typeof(MessageRoomEvent), EventKind.RoomMessage.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomNameContent>), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomPinnedEventsContent>), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomPowerLevelsContent>), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(RedactionRoomEvent), EventKind.RoomRedaction.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomServerAclContent>), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomThirdPartyInviteContent>), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomTombstoneContent>), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<RoomTopicContent>), EventKind.RoomTopic.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

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

            //_jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
            //    .Of(typeof(IEventContent))
            //    .RegisterSubtypeWithProperty(typeof(PresenceContent), @"presence")
            //    .Build());

            _jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
                .Of(typeof(IRoomContent))
                .RegisterSubtypeWithProperty(typeof(RedactionContent), @"reason")
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

        [Test]
        public void AvatarStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""info"": {
            ""h"": 398,
            ""mimetype"": ""image/jpeg"",
            ""size"": 31037,
            ""w"": 394
        },
        ""url"": ""mxc://example.org/JWEIFJgwEIhweiWJE""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.avatar"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomAvatar));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomAvatarContent>());
            var stateEvent = @event as StateEvent<RoomAvatarContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Url, Is.EqualTo(new Uri(@"mxc://example.org/JWEIFJgwEIhweiWJE")));
            Assert.That(stateEvent.Content.ImageInfo.HasValue, Is.True);
            Assert.That(stateEvent.Content.ImageInfo.Value.Height, Is.EqualTo(398));
            Assert.That(stateEvent.Content.ImageInfo.Value.Width, Is.EqualTo(394));
            Assert.That(stateEvent.Content.ImageInfo.Value.MimeType, Is.EqualTo(@"image/jpeg"));
            Assert.That(stateEvent.Content.ImageInfo.Value.DataSize, Is.EqualTo(31037));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void CanonicalAliasesStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""alias"": ""#somewhere:localhost"",
        ""alt_aliases"": [
            ""#somewhere:example.org"",
            ""#myroom:example.com""
        ]
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.canonical_alias"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomCanonicalAlias));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomCanonicalAliasContent>());
            var stateEvent = @event as StateEvent<RoomCanonicalAliasContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Alias, Is.EqualTo(@"#somewhere:localhost"));
            Assert.That(stateEvent.Content.AlternateAliases, Is.EqualTo(new string[] {@"#somewhere:example.org", @"#myroom:example.com"}));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void CreateStateEventTest()
        {
            const string json = 
@"{
    ""content"": {
        ""creator"": ""@example:example.org"",
        ""m.federate"": true,
        ""predecessor"": {
            ""event_id"": ""$something:example.org"",
            ""room_id"": ""!oldroom:example.org""
        },
        ""room_version"": ""1""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.create"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomCreate));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomCreateContent>());
            var stateEvent = @event as StateEvent<RoomCreateContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Creator, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.Content.Federate, Is.True);
            Assert.That(stateEvent.Content.RoomPredecessor.HasValue, Is.True);
            Assert.That(stateEvent.Content.RoomPredecessor.Value.EventId, Is.EqualTo(@"$something:example.org"));
            Assert.That(stateEvent.Content.RoomPredecessor.Value.RoomId, Is.EqualTo(@"!oldroom:example.org"));
            Assert.That(stateEvent.Content.RoomsVersion, Is.EqualTo(RoomsVersion.V1));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void GuestAccessStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""guest_access"": ""can_join""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.guest_access"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomGuestAccess));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomGuestAccessContent>());
            var stateEvent = @event as StateEvent<RoomGuestAccessContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.GuestAccessKind, Is.EqualTo(GuestAccessKind.CanJoin));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void HistoryVisibilityStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""history_visibility"": ""shared""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.history_visibility"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomHistoryVisibility));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomHistoryVisibilityContent>());
            var stateEvent = @event as StateEvent<RoomHistoryVisibilityContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.HistoryVisibilityKind, Is.EqualTo(HistoryVisibilityKind.Shared));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void JoinRuleStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""join_rule"": ""public""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.join_rules"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomJoinRule));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomJoinRulesContent>());
            var stateEvent = @event as StateEvent<RoomJoinRulesContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.JoinRule, Is.EqualTo(JoinRule.Public));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void MembershipStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""avatar_url"": ""mxc://example.org/SEsfnsuifSDFSSEF"",
        ""displayname"": ""Alice Margatroid"",
        ""membership"": ""join""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": ""@alice:example.org"",
    ""type"": ""m.room.member"",
    ""unsigned"": {
        ""age"": 1234,
        ""invite_room_state"": [
            {
                ""content"": {
                    ""name"": ""Example Room""
                },
                ""sender"": ""@bob:example.org"",
                ""state_key"": """",
                ""type"": ""m.room.name""
            },
            {
                ""content"": {
                    ""join_rule"": ""invite""
                },
                ""sender"": ""@bob:example.org"",
                ""state_key"": """",
                ""type"": ""m.room.join_rules""
            }
        ]
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomMembership));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomMembershipContent>());
            var stateEvent = @event as StateEvent<RoomMembershipContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.AvatarUrl, Is.EqualTo(new Uri(@"mxc://example.org/SEsfnsuifSDFSSEF")));
            Assert.That(stateEvent.Content.DisplayName, Is.EqualTo(@"Alice Margatroid"));
            Assert.That(stateEvent.Content.MembershipState, Is.EqualTo(MembershipState.Join));
            Assert.That(stateEvent.UnsignedData.InviteRoomStates.Count(), Is.EqualTo(2));
            var count = 0;
            foreach (var state in stateEvent.UnsignedData.InviteRoomStates)
            {
                switch (state.Content)
                {
                    case RoomNameContent nameEventContent:
                        Assert.That(nameEventContent.Name, Is.EqualTo(@"Example Room"));
                        ++count;
                        break;
                    case RoomJoinRulesContent joinRuleEventContent:
                        Assert.That(joinRuleEventContent.JoinRule, Is.EqualTo(JoinRule.Invite));
                        ++count;
                        break;
                    default:
                        Assert.Fail(@"Reached unexpected event content type.");
                        break;
                };
                Assert.That(state.Sender, Is.EqualTo(@"@bob:example.org"));
            }
            Assert.That(count, Is.EqualTo(2));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@"@alice:example.org"));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void ThirdPartInviteMembershipStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""avatar_url"": ""mxc://example.org/SEsfnsuifSDFSSEF"",
        ""displayname"": ""Alice Margatroid"",
        ""membership"": ""join"",
        ""third_party_invite"": {
            ""display_name"": ""alice"",
            ""signed"": {
                ""mxid"": ""@alice:example.org"",
                ""signatures"": {
                    ""magic.forest"": {
                        ""ed25519:3"": ""fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg""
                    }
                },
                ""token"": ""abc123""
            }
        }
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": ""@alice:example.org"",
    ""type"": ""m.room.member"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomMembership));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomMembershipContent>());
            var stateEvent = @event as StateEvent<RoomMembershipContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.AvatarUrl, Is.EqualTo(new Uri(@"mxc://example.org/SEsfnsuifSDFSSEF")));
            Assert.That(stateEvent.Content.DisplayName, Is.EqualTo(@"Alice Margatroid"));
            Assert.That(stateEvent.Content.MembershipState, Is.EqualTo(MembershipState.Join));
            Assert.That(stateEvent.Content.ThirdPartyInvite.HasValue, Is.True);
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.DisplayName, Is.EqualTo(@"alice"));
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.SignedContent.InvitedUserId, Is.EqualTo(@"@alice:example.org"));
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.SignedContent.Token, Is.EqualTo(@"abc123"));
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.SignedContent.Signatures.Count, Is.EqualTo(1));
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.SignedContent.Signatures.ContainsKey(@"magic.forest"), Is.True);
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.SignedContent.Signatures[@"magic.forest"].Count,
                Is.EqualTo(1));
            Assert.That(stateEvent.Content.ThirdPartyInvite.Value.SignedContent.Signatures[@"magic.forest"].ContainsKey(@"ed25519:3"), Is.True);
            Assert.That((stateEvent.Content.ThirdPartyInvite.Value.SignedContent.Signatures[@"magic.forest"])[@"ed25519:3"], 
                Is.EqualTo(@"fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg"));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@"@alice:example.org"));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void NameStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""name"": ""The room name""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.name"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomName));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomNameContent>());
            var stateEvent = @event as StateEvent<RoomNameContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Name, Is.EqualTo(@"The room name"));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void PinnedEventsStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""pinned"": [
            ""$someevent:example.org""
        ]
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.pinned_events"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomPinnedEvents));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomPinnedEventsContent>());
            var stateEvent = @event as StateEvent<RoomPinnedEventsContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.PinnedEvents, Is.EqualTo(new string[] {@"$someevent:example.org"}));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void PowerLevelsStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""ban"": 50,
        ""events"": {
            ""m.room.name"": 100,
            ""m.room.power_levels"": 100
        },
        ""events_default"": 0,
        ""invite"": 50,
        ""kick"": 50,
        ""notifications"": {
            ""room"": 20
        },
        ""redact"": 50,
        ""state_default"": 50,
        ""users"": {
            ""@example:localhost"": 100
        },
        ""users_default"": 0
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.power_levels"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomPowerLevels));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomPowerLevelsContent>());
            var stateEvent = @event as StateEvent<RoomPowerLevelsContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.BanLevel, Is.EqualTo(50));
            Assert.That(stateEvent.Content.EventLevels.ContainsKey(@"m.room.name"), Is.True);
            Assert.That(stateEvent.Content.EventLevels.ContainsKey(@"m.room.power_levels"), Is.True);
            Assert.That(stateEvent.Content.EventLevels[@"m.room.name"], Is.EqualTo(100));
            Assert.That(stateEvent.Content.EventLevels[@"m.room.power_levels"], Is.EqualTo(100));
            Assert.That(stateEvent.Content.EventsDefaultLevel, Is.EqualTo(0));
            Assert.That(stateEvent.Content.InviteLevel, Is.EqualTo(50));
            Assert.That(stateEvent.Content.KickLevel, Is.EqualTo(50));
            Assert.That(stateEvent.Content.NotificationLevels.ContainsKey(@"room"), Is.True);
            Assert.That(stateEvent.Content.NotificationLevels[@"room"], Is.EqualTo(20));
            Assert.That(stateEvent.Content.RedactLevel, Is.EqualTo(50));
            Assert.That(stateEvent.Content.StateDefaultLevel, Is.EqualTo(50));
            Assert.That(stateEvent.Content.UserLevels.ContainsKey(@"@example:localhost"), Is.True);
            Assert.That(stateEvent.Content.UserLevels[@"@example:localhost"], Is.EqualTo(100));
            Assert.That(stateEvent.Content.UsersDefaultLevel, Is.EqualTo(0));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void ServerAclStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""allow"": [
            ""*""
        ],
        ""allow_ip_literals"": false,
        ""deny"": [
            ""*.evil.com"",
            ""evil.com""
        ]
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.server_acl"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomServerAcl));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomServerAclContent>());
            var stateEvent = @event as StateEvent<RoomServerAclContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.AllowIpLiterals, Is.EqualTo(false));
            Assert.That(stateEvent.Content.AllowServers.Contains(@"*"), Is.True);
            Assert.That(stateEvent.Content.DenyServers.Contains(@"*.evil.com"), Is.True);
            Assert.That(stateEvent.Content.DenyServers.Contains(@"evil.com"), Is.True);
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void ThirdPartyInviteStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""display_name"": ""Alice Margatroid"",
        ""key_validity_url"": ""https://magic.forest/verifykey"",
        ""public_key"": ""abc123"",
        ""public_keys"": [
            {
                ""key_validity_url"": ""https://magic.forest/verifykey"",
                ""public_key"": ""def456""
            }
        ]
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": ""pc98"",
    ""type"": ""m.room.third_party_invite"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomThirdPartyInvite));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomThirdPartyInviteContent>());
            var stateEvent = @event as StateEvent<RoomThirdPartyInviteContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.DisplayName, Is.EqualTo(@"Alice Margatroid"));
            Assert.That(stateEvent.Content.KeyValidityUrl, Is.EqualTo(new Uri("https://magic.forest/verifykey")));
            Assert.That(stateEvent.Content.PublicKey, Is.EqualTo(@"abc123"));
            Assert.That(stateEvent.Content.PublicKeys.Count(), Is.EqualTo(1));
            var count = 0;
            foreach (var publicKey in stateEvent.Content.PublicKeys)
            {
                Assert.That(publicKey.KeyValidityUrl, Is.EqualTo(new Uri("https://magic.forest/verifykey")));
                Assert.That(publicKey.PublicKey, Is.EqualTo(@"def456"));
                ++count;
            }
            Assert.That(count, Is.EqualTo(1));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@"pc98"));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void TombstoneStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""body"": ""This room has been replaced"",
        ""replacement_room"": ""!newroom:example.org""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.tombstone"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomTombstone));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomTombstoneContent>());
            var stateEvent = @event as StateEvent<RoomTombstoneContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Body, Is.EqualTo(@"This room has been replaced"));
            Assert.That(stateEvent.Content.ReplacementRoomId, Is.EqualTo(@"!newroom:example.org"));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void TopicStateEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""topic"": ""A room topic""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""state_key"": """",
    ""type"": ""m.room.topic"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomTopic));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IStateContent>());
            Assert.That(@event.Content, Is.TypeOf<RoomTopicContent>());
            var stateEvent = @event as StateEvent<RoomTopicContent>;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Topic, Is.EqualTo(@"A room topic"));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.StateKey, Is.EqualTo(@""));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void TextMessageEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""body"": ""This is an example text message"",
        ""format"": ""org.matrix.custom.html"",
        ""formatted_body"": ""<b>This is an example text message</b>"",
        ""msgtype"": ""m.text""
    },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""type"": ""m.room.message"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomMessage));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IMessageContent>());
            Assert.That(@event.Content, Is.TypeOf<TextMessageContent>());
            RoomEvent<TextMessageContent> stateEvent = @event as MessageRoomEvent;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.MessageKind, Is.EqualTo(MessageKind.Text));
            Assert.That(stateEvent.Content.MessageBody, Is.EqualTo(@"This is an example text message"));
            Assert.That(stateEvent.Content.FormattedBody, Is.EqualTo(@"<b>This is an example text message</b>"));
            Assert.That(stateEvent.Content.Format, Is.EqualTo(@"org.matrix.custom.html"));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }

        [Test]
        public void RedactionRoomEventTest()
        {
            const string json =
@"{
    ""content"": {
        ""reason"": ""Spamming""
        },
    ""event_id"": ""$143273582443PhrSn:example.org"",
    ""origin_server_ts"": 1432735824653,
    ""redacts"": ""$fukweghifu23:localhost"",
    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
    ""sender"": ""@example:example.org"",
    ""type"": ""m.room.redaction"",
    ""unsigned"": {
        ""age"": 1234
    }
}";
            using var jsonReader = new JTokenReader(JToken.Parse(json));
            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomRedaction));
            Assert.That(@event.Content, Is.Not.Null);
            Assert.That(@event.Content, Is.InstanceOf<IRoomContent>());
            Assert.That(@event.Content, Is.TypeOf<RedactionContent>());
            var stateEvent = @event as RedactionRoomEvent;
            Assert.That(stateEvent, Is.Not.Null);
            Assert.That(stateEvent.Content.Reason, Is.EqualTo(@"Spamming"));
            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
            Assert.That(stateEvent.RedactedEventId, Is.EqualTo(@"$fukweghifu23:localhost"));
            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        }
    }
}