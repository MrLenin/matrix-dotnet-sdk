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
        public static IStateEvent MockStateEvent<T>(
            IStateEvent mockEvent,
            string stateKey,
            int age = 0)
        where T: class, IStateContent
        {
            mockEvent.StateKey = stateKey;
            mockEvent.UnsignedData.Age = age;
            return mockEvent;
        }

        public static IRoomEvent MockRoomEvent<T>(
            IRoomEvent mockEvent,
            string stateKey = null,
            int age = 0)
            where T : class, IRoomContent
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
            //mock.SetupGet(f => f.Sync.Token).Returns("AGoodSyncToken");
            mock.Setup(f => f.GetAccessToken()).Returns("AGoodAccessToken");
            mock.Setup(f => f.GetCurrentLogin()).Returns(new AuthContext());
            //mock.SetupGet(f => f.Sync.IsInitialSync).Returns(false);
            mock.Setup(f => f.Room.SendState(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<IStateContent>(),
                It.IsAny<string>())
            );
            return mock;
        }
    }
}