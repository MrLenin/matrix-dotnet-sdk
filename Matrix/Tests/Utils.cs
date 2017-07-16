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
            string state_key = null,
            int age = 0)
        {
            MatrixEvent ev = new MatrixEvent();
            ev.content = content;
            if(state_key != null) {
                ev.state_key = state_key;
            }
            ev.age = age;
            return ev;
        }

        public static Mock<MatrixAPI> MockAPI() {
            var mock = new Mock<MatrixAPI>("https://localhost");
            mock.Setup(f => f.user_id).Returns("@foobar:localhost");
            mock.Setup(f => f.BaseURL).Returns("https://localhost");
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