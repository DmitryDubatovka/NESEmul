using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ANDOperationTests : OperationBaseTests
    {
        [Test]
        public void ANDImmTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ANDImm);
            CPU.Accumulator = 0xFF;
            Memory.StoreByteInMemory(1, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }

        [Test]
        public void ANDZPTest()
        {
            InitZPMode(OpCodes.ANDZP, 0x2);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x00));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
        }

        [Test]
        public void ANDZPXTest()
        {
            InitZPXMode(OpCodes.ANDZPX, 0x3);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x1));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }
    }
}