using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ROLOperationTests : OperationBaseTests
    {
        [Test]
        public void ROLAccumOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ROLAccum);
            CPU.Accumulator = 0x59;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xB2));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
        }

        [Test]
        public void ROLZPOperationTest()
        {
            InitZPMode(OpCodes.ROLZP, 0xD9);
            CPU.CarryFlag = true;
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0F);
            Assert.That(value, Is.EqualTo(0xB3));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
        }
        
        [Test]
        public void ROLZPXOperationTest()
        {
            InitZPXMode(OpCodes.ROLZPX, 0x80);
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0F);
            Assert.That(value, Is.EqualTo(0x0));
            Assert.That(CPU.CarryFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void ROLAbsOperationTest()
        {
            CPU.CarryFlag = true;
            InitAbsMode(OpCodes.ROLAbs, 0x0);
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0102);
            Assert.That(value, Is.EqualTo(0x01));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void ROLAbsXOperationTest()
        {
            InitAbsXMode(OpCodes.ROLAbsX, 0x2);
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x0103);
            Assert.That(value, Is.EqualTo(0x04));
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }
    }
}