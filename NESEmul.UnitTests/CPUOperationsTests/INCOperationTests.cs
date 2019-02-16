using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class INCOperationTests : OperationBaseTests
    {
        [Test]
        public void INCZPOperationTest()
        {
            InitZPMode(OpCodes.INCZP, 0x10);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0x11));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void INCZPXOperationTest()
        {
            InitZPXMode(OpCodes.INCZPX, 0xFF);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
        }
        
        [Test]
        public void INCAbsOperationTest()
        {
            InitAbsMode(OpCodes.INCAbs, 0x7F);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0102);
            Assert.That(val, Is.EqualTo(0x80));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }

        [Test]
        public void INXOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.INX);
            CPU.IndexRegisterX = 0xFE;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0xFF));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void INYOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.INY);
            CPU.IndexRegisterY = 0xFD;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0xFE));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
    }
}