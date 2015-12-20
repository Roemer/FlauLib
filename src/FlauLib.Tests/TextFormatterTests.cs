using FlauLib.Tools;
using NUnit.Framework;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace FlauLib.Tests
{
    [TestFixture]
    public class TextFormatterTests
    {
        [Test]
        public void SizeSuffixTest()
        {
            Assert.AreEqual("1000 bytes", TextFormatter.SizeSuffix(1000));
            Assert.AreEqual("2 KB", TextFormatter.SizeSuffix(2048));
            Assert.AreEqual("977 KB", TextFormatter.SizeSuffix(1000000));
        }

        
        [Fact]
        public void SizeSuffixTest2()
        {
            Xunit.Assert.Equal("1000 bytes", TextFormatter.SizeSuffix(1000));
            Xunit.Assert.Equal("2 KB", TextFormatter.SizeSuffix(2048));
            Xunit.Assert.Equal("977 KB", TextFormatter.SizeSuffix(1000000));
        }
    }
}
