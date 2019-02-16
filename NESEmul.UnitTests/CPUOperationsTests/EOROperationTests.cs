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
            InitZPMode(OpCodes.EORZP, 0x1);
            CPU.Accumulator = 0x1;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x0));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void EORZPXTest()
        {
            InitZPXMode(OpCodes.EORZPX, 0x1);
            CPU.Accumulator = 0x81;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }
    }
}