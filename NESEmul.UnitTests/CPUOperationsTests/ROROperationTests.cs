using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ROROperationTests : OperationBaseTests
    {
        [Test]
        public void RORAccumOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.RORAccum);
            CPU.Accumulator = 0x59;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x2C));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }

        [Test]
        public void RORZPOperationTest()
        {
            InitZPMode(OpCodes.RORZP, 0xD8);
            CPU.CarryFlag = true;
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0F);
            Assert.That(value, Is.EqualTo(0xEC));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
        }

        [Test]
        public void RORZPXOperationTest()
        {
            InitZPXMode(OpCodes.RORZPX, 0x01);
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0F);
            Assert.That(value, Is.EqualTo(0x0));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }

        [Test]
        public void RORAbsOperationTest()
        {
            CPU.CarryFlag = true;
            InitAbsMode(OpCodes.RORAbs, 0x0);
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0102);
            Assert.That(value, Is.EqualTo(0x80));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
        }

        [Test]
        public void RORAbsXOperationTest()
        {
            InitAbsXMode(OpCodes.RORAbsX, 0x2);
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0103);
            Assert.That(value, Is.EqualTo(0x01));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }
    }
}