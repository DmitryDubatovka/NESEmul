using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class EOROperationTests : OperationBaseTests
    {
        [Test]
        public void EORImmTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.EORImm);
            CPU.Accumulator = 0x1;
            Memory.StoreByteInMemory(1, 0x2);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x3));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void EORZPTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.EORZP);
            CPU.Accumulator = 0x1;
            Memory.StoreByteInMemory(1, 0xF);
            Memory.StoreByteInMemory(0xF, 0x1);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x0));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void EORZPXTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.EORZPX);
            CPU.Accumulator = 0x81;
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0xE);
            Memory.StoreByteInMemory(0xF, 0x1);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }
    }
}