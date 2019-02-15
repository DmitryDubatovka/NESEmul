using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class LDAOperationTests : OperationBaseTests
    {
        [Test]
        public void LDAImmTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDAImm);
            CPU.Accumulator = 0;
            Memory.StoreByteInMemory(1, 0x10);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x10));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDAZPTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDAZP);
            CPU.Accumulator = 0x10;
            Memory.StoreByteInMemory(1, 0xF);
            Memory.StoreByteInMemory(0xF, 0x0);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x0));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDAAbsTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDAAbs);
            CPU.Accumulator = 0x0;
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(2, 0x01);
            Memory.StoreByteInMemory(0x0102, 0xFF);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFF));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }
    }
}