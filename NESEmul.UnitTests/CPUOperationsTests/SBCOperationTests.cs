using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class SBCOperationTests : OperationBaseTests
    {
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.SBCImmTestCases))]
        public void SBCImmTest(byte accumValue, byte value, byte resultValue, bool carryFlagResult, bool zeroFlagResult, bool overflowFlagResult, bool negativeFlagResult)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.SBCImm);
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
        public void SBCImmWithCarryFlagTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.SBCImm);
            CPU.CarryFlag = true;
            CPU.Accumulator = 0x1;
            Memory.StoreByteInMemory(1, 0x2);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFF));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.OverflowFlag, Is.EqualTo(true), "Overflow flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }

        [Test]
        public void SBCZPTest()
        {
            InitZPMode(OpCodes.SBCZP, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFD));
        }

        [Test]
        public void SBCZPXTest()
        {
            InitZPXMode(OpCodes.SBCZPX, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFD));
        }

        [Test]
        public void SBCAbsTest()
        {
            InitAbsMode(OpCodes.SBCAbs, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFD));
        }

        [Theory]
        public void SBCAbsXYTest(bool isXMode)
        {
            if(isXMode)
                InitAbsXMode(OpCodes.SBCAbsX, 0x3);
            else
                InitAbsYMode(OpCodes.SBCAbsY, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFD));
        }

        [Test]
        public void SBCIndirectXTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.SBCIndX);
            CPU.Accumulator = 0x1;
            CPU.IndexRegisterX = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x0F, 0x02);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x0402, 0x3);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFD));
        }

        [Test]
        public void ADCIndirectYTest()
        {
            Memory.StoreByteInMemory(0, (byte) OpCodes.SBCIndY);
            CPU.Accumulator = 0x1;
            CPU.IndexRegisterY = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x11, 0x02);
            Memory.StoreByteInMemory(0x303, 0x03);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFD));
        }
    }
}