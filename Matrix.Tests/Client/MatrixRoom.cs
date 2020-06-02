//using System;
//using NUnit.Framework;
//using Matrix;
//using Matrix.Api.ClientServer.Enumerations;
//using Matrix.Api.ClientServer.Events;
//using Matrix.Api.ClientServer.RoomContent;
//using Matrix.Api.ClientServer.StateContent;
//using Matrix.Client;
//using Matrix.Structures;
//using Moq;

//namespace Matrix.Tests.Client
//{
//    [TestFixture]
//    public class MatrixRoomTests
//    {
//        [Test]
//        public void CreateMatrixRoomTest()
//        {
//            var room = new MatrixRoom(null, "!abc:localhost");
//            Assert.That(room.Id, Is.EqualTo("!abc:localhost"), "The Room ID must be correct.");
//            Assert.That(room.Members, Is.Empty, "The Room must have no members.");
//        }

//        [Test]
//        public void FeedEventCreatorTest()
//        {
//            var room = new MatrixRoom(null, "!abc:localhost");
//            var creationEvent = new StateEvent<RoomCreateContent>()
//            {
//                Content = new RoomCreateContent()
//                {
//                    Creator = "@Half-Shot:localhost",
//                    Federate = false,
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(creationEvent, @"@Half-Shot:localhost"));
//            Assert.That(room.Creator, Is.EqualTo("@Half-Shot:localhost"), "Creator is correct.");
//            Assert.That(room.ShouldFederate, Is.False, "Should not federate.");
//        }

//        [Test]
//        public void FeedEventNameTest()
//        {
//            var room = new MatrixRoom(null, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomNameContent() 
//                {
//                    Name = "Snug Fox Party!"
//                }
//            };

//            room.FeedEvent(Utils.MockStateEvent(ev, @"@Half-Shot:localhost"));
//            Assert.That(room.Name, Is.EqualTo("Snug Fox Party!"), "Name is correct.");
//        }

//        [Test]
//        public void FeedEventTopicTest()
//        {
//            var room = new MatrixRoom(null, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomTopicContent()
//                {
//                    Topic = "Foxes welcome!"
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, @"@Half-Shot:localhost"));
//            Assert.That(room.Topic, Is.EqualTo("Foxes welcome!"), "Topic is correct.");
//        }

//        [Test]
//        public void FeedEventCanonicalAliasTest()
//        {
//            var room = new MatrixRoom(null, "!abc:localhost");
//            var aliases = new string[]
//            {
//                "#cookbook:resturant",
//                "#menu:resturant"
//            };
//            var ev = new StateEvent()
//            {
//                Content = new RoomCanonicalAliasContent()
//                {
//                    Alias = "#restaurant:restaurant",
//                    AlternateAliases = aliases
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, @"@Half-Shot:localhost"));
//            Assert.That(room.CanonicalAlias, Is.EqualTo("#restaurant:restaurant"), "The canonical alias is correct.");
//        }

//        [Test]
//        public void FeedEventJoinRuleTest()
//        {
//            var room = new MatrixRoom(null, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomJoinRulesContent()
//                {
//                    JoinRule = JoinRule.Public
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, @"@Half-Shot:localhost"));
//            Assert.That(room.JoinRule, Is.EqualTo(EMatrixRoomJoinRules.Public), "The join rule is correct.");
//        }

//        [Test]
//        public void FeedEventRoomMemberTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom((MatrixApi) mock.Object, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Join
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            Assert.That(room.Members.ContainsKey("@foobar:localhost"), Is.True, "The member is in the room.");
//            Assert.That(room.Members.ContainsValue((RoomMembershipContent)ev.Content), Is.True, "The member is in the room.");
//        }

//        [Test]
//        public void FeedEventRoomMemberNoFireEventsTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom((MatrixApi) mock.Object, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Join
//                }
//            };
//            mock.Setup(f => f.Sync.IsInitialSync).Returns(true);
//            var didFire = false;
//            room.OnUserJoined += (n, a) => didFire = true;
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            Assert.That(didFire, Is.False);
//            Assert.That(room.Members.ContainsKey("@foobar:localhost"), Is.True, "The member is in the room.");
//            Assert.That(room.Members.ContainsValue((RoomMembershipContent)ev.Content), Is.True, "The member is in the room.");
//        }

//        [Test]
//        public void FeedEventRoomMemberFireEventsTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom((MatrixApi) mock.Object, "!abc:localhost");
//            var didFire = new bool[5];
//            var fireCount = 0;
//            room.OnUserJoined += (n, a) => didFire[0] = true;
//            room.OnUserChange += (n, a) => didFire[1] = true;
//            room.OnUserLeft += (n, a) => didFire[2] = true;
//            room.OnUserInvited += (n, a) => didFire[3] = true;
//            room.OnUserBanned += (n, a) => didFire[4] = true;
//            room.OnEvent += (n, a) => fireCount++;

//            var ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Join
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));

//            Assert.That(didFire[0], Is.True, "Processed join");
//            ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Join,
//                    DisplayName = "Foobar!"
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));

//            Assert.That(didFire[1], Is.True, "Processed change");
//            ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Leave
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            Assert.That(didFire[2], Is.True, "Processed leave");

//            ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Invite
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            Assert.That(didFire[3], Is.True, "Processed invite");

//            ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Ban
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            Assert.That(didFire[4], Is.True, "Processed ban");
//            Assert.That(fireCount, Is.EqualTo(5), "OnEvent should fire each time.");
//        }

//        [Test]
//        public void FeedEventRoomMessageTest()
//        {
//            var fireCount = 0;
//            var didFire = false;
//            var room = new MatrixRoom(null, "!abc:localhost");
//            room.OnMessage += (n, a) => didFire = true;
//            room.OnEvent += (n, a) => fireCount++;
//            // NoAgeRestriction
//            room.MessageMaximumAge = 0;
//            var ev = new RoomEvent()
//            {
//                Content = new TextMessageContent()
//                {
//                    MessageBody = ""
//                }
//            };
//            room.FeedEvent(Utils.MockRoomEvent(ev, age: 5000));
//            Assert.That(didFire, Is.True, "Message without age limit.");
//            // AgeRestriction, Below Limit
//            room.MessageMaximumAge = 5000;
//            didFire = false;
//            room.FeedEvent(Utils.MockRoomEvent(ev, age: 2500));
//            Assert.That(didFire, Is.True, "Message below age limit.");
//            // AgeRestriction, Above Limit
//            didFire = false;
//            room.FeedEvent(Utils.MockRoomEvent(ev, age: 5001));
//            Assert.That(didFire, Is.False, "Message above age limit.");
//            //Test Subclass
//            didFire = false;
//            room.FeedEvent(Utils.MockRoomEvent(ev));
//            Assert.That(didFire, Is.True, "Subclassed message accepted.");
//            // OnEvent should fire each time
//            Assert.That(fireCount, Is.EqualTo(4));
//        }

//        [Test]
//        public void SetMemberDisplayNameNoMemberTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom(mock.Object, "!abc:localhost");
//            Assert.That(
//                () => room.SetMemberDisplayName("@foobar:localhost"),
//                Throws.TypeOf<MatrixException>()
//                    .With.Property("Message").EqualTo("Couldn't find the user's membership event")
//            );
//        }

//        [Test]
//        public void SetMemberAvatarNoMemberTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom(mock.Object, "!abc:localhost");
//            Assert.That(
//                () => room.SetMemberAvatar(new Uri("@foobar:localhost", UriKind.Relative)),
//                Throws.TypeOf<MatrixException>()
//                    .With.Property("Message").EqualTo("Couldn't find the user's membership event")
//            );
//        }

//        [Test]
//        public void SetMemberDisplayNameTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom(mock.Object, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Join
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            room.SetMemberDisplayName("@foobar:localhost");
//        }

//        [Test]
//        public void SetMemberAvatarTest()
//        {
//            var mock = Utils.MockApi();
//            var room = new MatrixRoom(mock.Object, "!abc:localhost");
//            var ev = new StateEvent()
//            {
//                Content = new RoomMembershipContent()
//                {
//                    MembershipState = MembershipState.Join
//                }
//            };
//            room.FeedEvent(Utils.MockStateEvent(ev, "@foobar:localhost"));
//            room.SetMemberAvatar(new Uri("@foobar:localhost", UriKind.Relative));
//        }
//    }
//}