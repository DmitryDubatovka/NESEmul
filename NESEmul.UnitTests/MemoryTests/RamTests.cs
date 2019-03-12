using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.MemoryTests
{
    [TestFixture]
    public class RamTests
    {
        [Test]
        public void RamMirrorsTest()
        {
            var memory = new Memory();
            var value = memory.LoadByteFromMemory(0x100);
            Assert.That(value, Is.EqualTo(0));
            memory.StoreByteInMemory(0x100, 0xEE);
            value = memory.LoadByteFromMemory(0x100);
            Assert.That(value, Is.EqualTo(0xEE));
            value = memory.LoadByteFromMemory(0x900);
            Assert.That(value, Is.EqualTo(0xEE));
            value = memory.LoadByteFromMemory(0x1100);
            Assert.That(value, Is.EqualTo(0xEE));
            value = memory.LoadByteFromMemory(0x1900);
            Assert.That(value, Is.EqualTo(0xEE));
        }
    }
}