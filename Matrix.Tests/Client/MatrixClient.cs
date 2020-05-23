using System;
using NUnit.Framework;
using Matrix;
using Matrix.Client;
using Matrix.Structures;
using Moq;

namespace Matrix.Tests.Client
{
    [TestFixture]
    public class MatrixClientTests
    {
        [Test]
        public void CreateMatrixClientTest()
        {
            var mock = Utils.MockApi();
            _ = new MatrixClient((MatrixApi)mock.Object);
        }

        [Test]
        public void GetSyncTokenTest()
        {
            var mock = Utils.MockApi();
            var client = new MatrixClient((MatrixApi) mock.Object);
            Assert.That(client.GetSyncToken(), Is.EqualTo("AGoodSyncToken"));
        }

        [Test]
        public void GetAccessTokenTest()
        {
            var mock = Utils.MockApi();

            var client = new MatrixClient((MatrixApi) mock.Object);
            Assert.That(client.GetAccessToken(), Is.EqualTo("AGoodAccessToken"));
        }

        [Test]
        public void GetCurrentLoginTest()
        {
            var mock = Utils.MockApi();
            var client = new MatrixClient((MatrixApi) mock.Object);
            Assert.That(client.GetCurrentLogin(), Is.Not.Null);
        }

        [Test]
        public void GetUserTest()
        {
            var mock = Utils.MockApi();
            // Without userid parameter
            mock.Setup(f => f.ClientProfile(It.IsAny<string>()));
            var client = new MatrixClient((MatrixApi) mock.Object);
            Assert.That(client.GetUser(), Is.Null);

            mock.Setup(f => f.ClientProfile(It.IsAny<string>())).Returns(new MatrixProfile());
            var user = client.GetUser();
            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserId, Is.EqualTo("@foobar:localhost"));

            //With userid parameter.
            mock.Setup(f => f.ClientProfile(It.IsAny<string>()));
            Assert.That(client.GetUser("@barbaz:localhost"), Is.Null);

            mock.Setup(f => f.ClientProfile(It.IsAny<string>())).Returns(new MatrixProfile());
            user = client.GetUser("@barbaz:localhost");
            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserId, Is.EqualTo("@barbaz:localhost"));
        }
    }
}