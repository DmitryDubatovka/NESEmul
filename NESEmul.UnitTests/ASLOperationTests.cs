using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests
{
    [TestFixture]
    public class ASLOperationTests
    {
        private CPU _cpu;
        private Memory _memory;

        [SetUp]
        public void Setup()
        {
            _memory = new Memory();
            _cpu = new CPU(0, 0, _memory);
        }

        [Test]
        public void ASLAccumTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ASLAccum);
            _cpu.Accumulator = 0xFF;
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0xFE));
            Assert.That(_cpu.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }

        [Test]
        public void ASLZPTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ASLZP);
            _cpu.Accumulator = 0x1;
            _memory.StoreByteInMemory(1, 0xF);
            _memory.StoreByteInMemory(0xF, 0x80);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x1));
            var value = _memory.LoadByteFromMemory(0xF);
            Assert.That(value, Is.EqualTo(0x0));
            Assert.That(_cpu.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void ADCAbsXYTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ASLAbsX);
            _cpu.Accumulator = 0x1;
            _cpu.IndexRegisterX = 0x1;
            _memory.StoreByteInMemory(1, 0x02);
            _memory.StoreByteInMemory(2, 0x01);
            _memory.StoreByteInMemory(0x0103, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x1));
            var value = _memory.LoadByteFromMemory(0x0103);
            Assert.That(value, Is.EqualTo(0x6));
            Assert.That(_cpu.CarryFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }
    }
}