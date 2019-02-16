using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class LSROperationTests : OperationBaseTests
    {
        [Test]
        public void LSRAccumTest()
        {
            Memory.StoreByteInMemory(0, (byte) OpCodes.LSRAcc);
            CPU.Accumulator = 0x2;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x1));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false), "Carry flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }

        [Test]
        public void LSRZPTest()
        {
            InitZPMode(OpCodes.LSRZP, 0x3);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0x1));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }

        [Test]
        public void LSRZPXTest()
        {
            InitZPXMode(OpCodes.LSRZPX, 0x1);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0x0));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
        }

        [Test]
        public void LSRAbsTest()
        {
            InitAbsMode(OpCodes.LSRAbs, 0x81);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0102);
            Assert.That(val, Is.EqualTo(0x40));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }
        
        [Test]
        public void LSRAbsXTest()
        {
            InitAbsXMode(OpCodes.LSRAbsX, 0x81);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0103);
            Assert.That(val, Is.EqualTo(0x40));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true), "Carry flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
        }
    }
}