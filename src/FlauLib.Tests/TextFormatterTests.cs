using FlauLib.Tools;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace FlauLib.Tests
{
    [TestFixture]
    public class TextFormatterTests
    {
        [Test]
        public void SizeSuffixTest()
        {
            Assert.Equals("1000 bytes", TextFormatter.SizeSuffix(1000));
            Assert.Equals("2 KB", TextFormatter.SizeSuffix(2048));
            Assert.Equals("977 KB", TextFormatter.SizeSuffix(1000000));
        }
    }
}
