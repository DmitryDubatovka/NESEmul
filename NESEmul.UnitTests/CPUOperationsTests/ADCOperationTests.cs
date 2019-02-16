using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ADCOperationTests : OperationBaseTests
    {
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.ADCImmTestCases))]
        public void ADCImmTest(byte accumValue, byte value, byte resultValue, bool carryFlagResult, bool zeroFlagResult, bool overflowFlagResult, bool negativeFlagResult)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ADCImm);
            CPU.Accumulator = accumValue;
            Memory.StoreByteInMemory(1, value);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(resultValue));
            Assert.That(CPU.CarryFlag, Is.EqualTo(carryFlagResult), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlagResult), "Zero flag");
            Assert.That(CPU.OverflowFlag, Is.EqualTo(overflowFlagResult), "Overflow flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlagResult), "Negative flag");
        }

        [Test]
        public void ADCImmWithCarryFlagTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ADCImm);
            CPU.CarryFlag = true;
            CPU.Accumulator = 0x1;
            Memory.StoreByteInMemory(1, 0x2);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.OverflowFlag, Is.EqualTo(false), "Overflow flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void ADCZPTest()
        {
            InitZPMode(OpCodes.ADCZP, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCZPXTest()
        {
            InitZPXMode(OpCodes.ADCZPX, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCAbsTest()
        {
            InitAbsMode(OpCodes.ADCAbs, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
        }

        [Theory]
        public void ADCAbsXYTest(bool isXMode)
        {
            Memory.StoreByteInMemory(0, isXMode ? (byte)OpCodes.ADCAbsX : (byte)OpCodes.ADCAbsY);
            CPU.Accumulator = 0x1;
            if (isXMode)
                CPU.IndexRegisterX = 0x1;
            else
                CPU.IndexRegisterY = 0x1;
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(2, 0x01);
            Memory.StoreByteInMemory(0x0103, 0x3);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCIndirectXTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ADCIndX);
            CPU.Accumulator = 0x1;
            CPU.IndexRegisterX = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x0F, 0x02);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x0402, 0x3);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
        }

        [Test]
        public void ADCIndirectYTest()
        {
            Memory.StoreByteInMemory(0, (byte) OpCodes.ADCIndY);
            CPU.Accumulator = 0x1;
            CPU.IndexRegisterY = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x11, 0x02);
            Memory.StoreByteInMemory(0x303, 0x03);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x4));
        }

    }
}