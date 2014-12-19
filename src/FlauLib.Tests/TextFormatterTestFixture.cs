using FlauLib.Tools;
using NUnit.Framework;

namespace FlauLib.Tests
{
    [TestFixture]
    public class TextFormatterTestFixture
    {
        [Test]
        public void SizeSuffixTest()
        {
            Assert.AreEqual("1000 bytes", TextFormatter.SizeSuffix(1000));
            Assert.AreEqual("2 KB", TextFormatter.SizeSuffix(2048));
            Assert.AreEqual("977 KB", TextFormatter.SizeSuffix(1000000));
        }

        [Test]
        public void FailingTest()
        {
            Assert.AreEqual(true, false);
        }
    }
}
