using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ANDOperationTests : OperationBaseTests
    {
        [Test]
        public void ANDImTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ANDIm);
            CPU.Accumulator = 0xFF;
            Memory.StoreByteInMemory(1, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }

        [Test]
        public void ANDZPTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ANDZP);
            CPU.Accumulator = 0x1;
            Memory.StoreByteInMemory(1, 0xF);
            Memory.StoreByteInMemory(0xF, 0x2);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x00));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
        }

        [Test]
        public void ANDZPXTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ANDZPX);
            CPU.Accumulator = 0x1;
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0xE);
            Memory.StoreByteInMemory(0xF, 0x3);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x1));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }
    }
}