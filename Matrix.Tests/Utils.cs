using System;
using Moq;
using Matrix.Structures;
using Matrix;
using Matrix.Api.ClientServer;
using Matrix.Api.ClientServer.Events;

namespace Matrix.Tests
{
    public class Utils
    {
        public static IStateEvent<T> MockStateEvent<T>(
            IStateEvent<T> mockEvent,
            string stateKey,
            int age = 0)
        where T: class, IStateEventContent
        {
            mockEvent.StateKey = stateKey;
            mockEvent.UnsignedData.Age = age;
            return mockEvent;
        }

        public static IRoomEvent<T> MockRoomEvent<T>(
            IRoomEvent<T> mockEvent,
            string stateKey = null,
            int age = 0)
            where T : class, IRoomEventContent
        {
            mockEvent.UnsignedData.Age = age;
            return mockEvent;
        }

        public static Mock<MatrixApi> MockApi()
        {
            var baseUrl = new Uri("https://localhost");
            var mock = new Mock<MatrixApi>(baseUrl);
            mock.SetupGet(f => f.UserId).Returns("@foobar:localhost");
            mock.SetupGet(f => f.BaseUrl).Returns(new Uri("https://localhost"));
            mock.SetupGet(f => f.Sync.Token).Returns("AGoodSyncToken");
            mock.Setup(f => f.GetAccessToken()).Returns("AGoodAccessToken");
            mock.Setup(f => f.GetCurrentLogin()).Returns(new AuthenticationContext());
            mock.SetupGet(f => f.Sync.IsInitialSync).Returns(false);
            mock.Setup(f => f.Room.SendState(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IStateEventContent>(),
                It.IsAny<string>())
            );
            return mock;
        }
    }
}