using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests
{
    [TestFixture]
    public class CPUOperationsTests
    {
        private CPU _cpu;
        private Memory _memory;

        [SetUp]
        public void Setup()
        {
            _memory = new Memory();
            _cpu = new CPU(0, 0, _memory);
        }

        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.ADCImTestCases))]
        public void ADCImTest(byte accumValue, byte value, byte resultValue, bool carryFlagResult, bool zeroFlagResult, bool overflowFlagResult, bool negativeFlagResult)
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCIm);
            _cpu.Accumulator = accumValue;
            _memory.StoreByteInMemory(1, value);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(resultValue));
            Assert.That(_cpu.CarryFlag, Is.EqualTo(carryFlagResult), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(zeroFlagResult), "Zero flag");
            Assert.That(_cpu.OverflowFlag, Is.EqualTo(overflowFlagResult), "Overflow flag");
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(negativeFlagResult), "Negative flag");
        }

        [Test]
        public void ADCImWithCarryFlagTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCIm);
            _cpu.CarryFlag = true;
            _cpu.Accumulator = 0x1;
            _memory.StoreByteInMemory(1, 0x2);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
            Assert.That(_cpu.CarryFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(_cpu.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(_cpu.OverflowFlag, Is.EqualTo(false), "Overflow flag");
            Assert.That(_cpu.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void ADCZPTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCZP);
            _cpu.Accumulator = 0x1;
            _memory.StoreByteInMemory(1, 0xF);
            _memory.StoreByteInMemory(0xF, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCZPXTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCZPX);
            _cpu.Accumulator = 0x1;
            _cpu.IndexRegisterX = 0x1;
            _memory.StoreByteInMemory(1, 0xE);
            _memory.StoreByteInMemory(0xF, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCAbsTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCAbs);
            _cpu.Accumulator = 0x1;
            _memory.StoreByteInMemory(1, 0x02);
            _memory.StoreByteInMemory(2, 0x01);
            _memory.StoreByteInMemory(0x0102, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
        }

        [Theory]
        public void ADCAbsXYTest(bool isXMode)
        {
            _memory.StoreByteInMemory(0, isXMode ? (byte)OpCodes.ADCAbsX : (byte)OpCodes.ADCAbsY);
            _cpu.Accumulator = 0x1;
            if (isXMode)
                _cpu.IndexRegisterX = 0x1;
            else
                _cpu.IndexRegisterY = 0x1;
            _memory.StoreByteInMemory(1, 0x02);
            _memory.StoreByteInMemory(2, 0x01);
            _memory.StoreByteInMemory(0x0103, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCIndirectXTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCIndX);
            _cpu.Accumulator = 0x1;
            _cpu.IndexRegisterX = 0xFF;
            _memory.StoreByteInMemory(0x1, 0x10);
            _memory.StoreByteInMemory(0x0F, 0x02);
            _memory.StoreByteInMemory(0x10, 0x04);
            _memory.StoreByteInMemory(0x0402, 0x3);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCIndirectYTest()
        {
            _memory.StoreByteInMemory(0, (byte) OpCodes.ADCIndY);
            _cpu.Accumulator = 0x1;
            _cpu.IndexRegisterY = 0xFF;
            _memory.StoreByteInMemory(0x1, 0x10);
            _memory.StoreByteInMemory(0x10, 0x04);
            _memory.StoreByteInMemory(0x11, 0x02);
            _memory.StoreByteInMemory(0x303, 0x03);
            _cpu.Do();
            Assert.That(_cpu.Accumulator, Is.EqualTo(0x4));
        }

    }
}