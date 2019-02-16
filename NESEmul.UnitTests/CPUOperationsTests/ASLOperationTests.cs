using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ASLOperationTests : OperationBaseTests
    {
        [Test]
        public void ASLAccumTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ASLAccum);
            CPU.Accumulator = 0xFF;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFE));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }

        [Test]
        public void ASLZPTest()
        {
            InitZPMode(OpCodes.ASLZP, 0x80);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x1));
            var value = Memory.LoadByteFromMemory(0xF);
            Assert.That(value, Is.EqualTo(0x0));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void ADCAbsXTest()
        {
            InitAbsXMode(OpCodes.ASLAbsX, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x1));
            var value = Memory.LoadByteFromMemory(0x0103);
            Assert.That(value, Is.EqualTo(0x6));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }
    }
}