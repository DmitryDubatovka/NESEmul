using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests
{
    [TestFixture]
    public class CPUANDOperationTests
    {
        private Memory _memory;
        private CPU _cpu;

        [SetUp]
        public void Setup()
        {
            _memory = new Memory();
            _cpu = new CPU(0, 0, _memory);
        }

        [Test]
        public void ANDImTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ANDIm);
            _cpu.Accumulator = 0xFF;
            _memory.StoreByteInMemory(1, 0x80);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x80));
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }

        [Test]
        public void ANDZPTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ANDZP);
            _cpu.Accumulator = 0x1;
            _memory.StoreByteInMemory(1, 0xF);
            _memory.StoreByteInMemory(0xF, 0x2);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x00));
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(true), "Zero flag");
        }

        [Test]
        public void ANDZPXTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ANDZPX);
            _cpu.Accumulator = 0x1;
            _cpu.IndexRegisterX = 0x1;
            _memory.StoreByteInMemory(1, 0xE);
            _memory.StoreByteInMemory(0xF, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x1));
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }
    }
}