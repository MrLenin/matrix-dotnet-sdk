using System;
using System.Linq;
using System.Net;
using JsonSubTypes;

using Matrix.Api;
using Matrix.Api.ClientServer;
using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomEventContent;
using Matrix.Api.ClientServer.StateEventContent;
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
                .RegisterSubtype(typeof(StateEvent<AvatarEventContent>), EventKind.RoomAvatar.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<CanonicalAliasEventContent>), EventKind.RoomCanonicalAlias.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<CreateEventContent>), EventKind.RoomCreate.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<GuestAccessEventContent>), EventKind.RoomGuestAccess.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<HistoryVisibilityEventContent>), EventKind.RoomHistoryVisibility.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<JoinRuleEventContent>), EventKind.RoomJoinRule.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<MembershipEventContent>), EventKind.RoomMembership.ToJsonString())
                .RegisterSubtype(typeof(MessageRoomEvent), EventKind.RoomMessage.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<NameEventContent>), EventKind.RoomName.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<PinnedEventsEventContent>), EventKind.RoomPinnedEvents.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<PowerLevelsEventContent>), EventKind.RoomPowerLevels.ToJsonString())
                .RegisterSubtype(typeof(RedactionRoomEvent), EventKind.RoomRedaction.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<ServerAclEventContent>), EventKind.RoomServerAcl.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<ThirdPartyInviteEventContent>), EventKind.RoomThirdPartyInvite.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<TombstoneEventContent>), EventKind.RoomTombstone.ToJsonString())
                .RegisterSubtype(typeof(StateEvent<TopicEventContent>), EventKind.RoomTopic.ToJsonString())
                .SerializeDiscriminatorProperty()
                .Build());

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

            //_jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
            //    .Of(typeof(IEventContent))
            //    .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
            //    .Build());

            //_jsonSerializer.Converters.Add(JsonSubtypesWithPropertyConverterBuilder
            //    .Of(typeof(IRoomEventContent))
            //    .RegisterSubtypeWithProperty(typeof(PresenceEventContent), @"presence")
            //    .Build());

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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<AvatarEventContent>());
            var stateEvent = @event as StateEvent<AvatarEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<CanonicalAliasEventContent>());
            var stateEvent = @event as StateEvent<CanonicalAliasEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<CreateEventContent>());
            var stateEvent = @event as StateEvent<CreateEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<GuestAccessEventContent>());
            var stateEvent = @event as StateEvent<GuestAccessEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<HistoryVisibilityEventContent>());
            var stateEvent = @event as StateEvent<HistoryVisibilityEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<JoinRuleEventContent>());
            var stateEvent = @event as StateEvent<JoinRuleEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<MembershipEventContent>());
            var stateEvent = @event as StateEvent<MembershipEventContent>;
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
                    case NameEventContent nameEventContent:
                        Assert.That(nameEventContent.Name, Is.EqualTo(@"Example Room"));
                        ++count;
                        break;
                    case JoinRuleEventContent joinRuleEventContent:
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

        // TODO: Implement encryption stuff
        //        [Test]
        //        public void MembershipStateEventTest2()
        //        {
        //            const string json =
        //@"{
        //    ""content"": {
        //        ""avatar_url"": ""mxc://example.org/SEsfnsuifSDFSSEF"",
        //        ""displayname"": ""Alice Margatroid"",
        //        ""membership"": ""join"",
        //        ""third_party_invite"": {
        //            ""display_name"": ""alice"",
        //            ""signed"": {
        //                ""mxid"": ""@alice:example.org"",
        //                ""signatures"": {
        //                    ""magic.forest"": {
        //                        ""ed25519:3"": ""fQpGIW1Snz+pwLZu6sTy2aHy/DYWWTspTJRPyNp0PKkymfIsNffysMl6ObMMFdIJhk6g6pwlIqZ54rxo8SLmAg""
        //                    }
        //                },
        //                ""token"": ""abc123""
        //            }
        //        }
        //    },
        //    ""event_id"": ""$143273582443PhrSn:example.org"",
        //    ""origin_server_ts"": 1432735824653,
        //    ""room_id"": ""!jEsUZKDJdhlrceRyVU:example.org"",
        //    ""sender"": ""@example:example.org"",
        //    ""state_key"": ""@alice:example.org"",
        //    ""type"": ""m.room.member"",
        //    ""unsigned"": {
        //        ""age"": 1234
        //    }
        //}";
        //            using var jsonReader = new JTokenReader(JToken.Parse(json));
        //            var @event = _jsonSerializer.Deserialize<IEvent>(jsonReader);
        //            Assert.That(@event, Is.Not.Null);
        //            Assert.That(@event.EventKind, Is.EqualTo(EventKind.RoomMembership));
        //            Assert.That(@event.Content, Is.Not.Null);
        //            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
        //            Assert.That(@event.Content, Is.TypeOf<MembershipEventContent>());
        //            var stateEvent = @event as StateEvent<MembershipEventContent>;
        //            Assert.That(stateEvent, Is.Not.Null);
        //            Assert.That(stateEvent.Content.AvatarUrl, Is.EqualTo(new Uri(@"mxc://example.org/SEsfnsuifSDFSSEF")));
        //            Assert.That(stateEvent.Content.DisplayName, Is.EqualTo(@"Alice Margatroid"));
        //            Assert.That(stateEvent.Content.MembershipState, Is.EqualTo(MembershipState.Join));
        //            Assert.That(stateEvent.Content.UnsignedData.InviteRoomStates.Count(), Is.EqualTo(2));
        //            var count = 0;
        //            foreach (var state in stateEvent.Content.UnsignedData.InviteRoomStates)
        //            {
        //                switch (state.Content)
        //                {
        //                    case NameEventContent nameEventContent:
        //                        Assert.That(nameEventContent.Name, Is.EqualTo(@"Example Room"));
        //                        ++count;
        //                        break;
        //                    case JoinRuleEventContent joinRuleEventContent:
        //                        Assert.That(joinRuleEventContent.JoinRule, Is.EqualTo(JoinRule.Invite));
        //                        ++count;
        //                        break;
        //                    default:
        //                        Assert.Fail(@"Reached unexpected event content type.");
        //                        break;
        //                };
        //                Assert.That(state.Sender, Is.EqualTo(@"@bob:example.org"));
        //            }
        //            Assert.That(count, Is.EqualTo(2));
        //            Assert.That(stateEvent.EventId, Is.EqualTo(@"$143273582443PhrSn:example.org"));
        //            Assert.That(stateEvent.OriginServerTimestamp, Is.EqualTo(1432735824653));
        //            Assert.That(stateEvent.RoomId, Is.EqualTo(@"!jEsUZKDJdhlrceRyVU:example.org"));
        //            Assert.That(stateEvent.Sender, Is.EqualTo(@"@example:example.org"));
        //            Assert.That(stateEvent.StateKey, Is.EqualTo(@"@alice:example.org"));
        //            Assert.That(stateEvent.UnsignedData, Is.Not.Null);
        //            Assert.That(stateEvent.UnsignedData.Age, Is.EqualTo(1234));
        //        }

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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<NameEventContent>());
            var stateEvent = @event as StateEvent<NameEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<PinnedEventsEventContent>());
            var stateEvent = @event as StateEvent<PinnedEventsEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<PowerLevelsEventContent>());
            var stateEvent = @event as StateEvent<PowerLevelsEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<ServerAclEventContent>());
            var stateEvent = @event as StateEvent<ServerAclEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<ThirdPartyInviteEventContent>());
            var stateEvent = @event as StateEvent<ThirdPartyInviteEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<TombstoneEventContent>());
            var stateEvent = @event as StateEvent<TombstoneEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IStateEventContent>());
            Assert.That(@event.Content, Is.TypeOf<TopicEventContent>());
            var stateEvent = @event as StateEvent<TopicEventContent>;
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
            Assert.That(@event.Content, Is.InstanceOf<IMessageEventContent>());
            Assert.That(@event.Content, Is.TypeOf<TextMessageEventContent>());
            RoomEvent<TextMessageEventContent> stateEvent = @event as MessageRoomEvent;
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
            Assert.That(@event.Content, Is.InstanceOf<IRoomEventContent>());
            Assert.That(@event.Content, Is.TypeOf<RedactionEventContent>());
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