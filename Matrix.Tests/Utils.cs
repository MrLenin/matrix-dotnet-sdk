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
            MatrixEvent ev = new MatrixEvent();
            ev.content = content;
            if(stateKey != null) {
                ev.state_key = stateKey;
            }
            ev.age = age;
            return ev;
        }

        public static Mock<MatrixApi> MockApi()
        {
            var baseUrl = new Uri("https://localhost");
            var mock = new Mock<MatrixApi>(baseUrl);
            mock.SetupGet(f => f.UserId).Returns("@foobar:localhost");
            mock.SetupGet(f => f.BaseUrl).Returns(new Uri("https://localhost"));
            mock.Setup(f => f.GetSyncToken()).Returns("AGoodSyncToken");
            mock.Setup(f => f.GetAccessToken()).Returns("AGoodAccessToken");
            mock.Setup(f => f.GetCurrentLogin()).Returns(new MatrixLoginResponse());
            mock.Setup(f => f.RunningInitialSync).Returns(false);
            mock.Setup(f => f.RoomStateSend(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<MatrixRoomStateEvent>(),
                It.IsAny<string>())
            );
            return mock;
        }
    }
}
