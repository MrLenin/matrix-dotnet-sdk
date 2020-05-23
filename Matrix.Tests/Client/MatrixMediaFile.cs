using System;
using NUnit.Framework;
using Matrix.Client;
using Moq;
namespace Matrix.Tests.Client
{
    [TestFixture]
    public class MatrixMediaFileTests
    {
        [Test]
        public void TestCreateMediaFile()
        {
            var mxcUrl = new Uri("mxc://half-shot.uk/oSnvUaEqIQcsVfAuulWeeBVB");
            const string contentType = "image/png";

            var mock = Utils.MockApi();
            var media = new MatrixMediaFile((MatrixApi)mock.Object, mxcUrl, contentType);
            Assert.That(media.GetUrl(), Is.EqualTo(new Uri("https://localhost/_matrix/media/r0/download/half-shot.uk/oSnvUaEqIQcsVfAuulWeeBVB")));
            Assert.That(media.GetThumbnailUrl(256,256,"crop"), Is.EqualTo(new Uri("https://localhost/_matrix/media/r0/thumbnail/half-shot.uk/oSnvUaEqIQcsVfAuulWeeBVB?width=256&height=256&method=crop")));
        }
    }
}