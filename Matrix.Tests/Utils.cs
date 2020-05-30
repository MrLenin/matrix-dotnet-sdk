using System;
using Moq;
using Matrix.Structures;
using Matrix;

namespace Matrix.Tests
{
    public class Utils
    {
        public static MatrixEvent MockEvent(
            MatrixEventContent content,
            string stateKey = null,
            int age = 0)
        {
            var ev = new MatrixEvent
            {
                Content = content
            };

            if (stateKey != null) ev.StateKey = stateKey;
            ev.Age = age;
            return ev;
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
                It.IsAny<MatrixRoomStateEvent>(),
                It.IsAny<string>())
            );
            return mock;
        }
    }
}