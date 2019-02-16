using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class DECOperationTests : OperationBaseTests
    {
        [Test]
        public void DECZPOperationTest()
        {
            InitZPMode(OpCodes.DECZP, 0x10);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0xF));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void DECZPXOperationTest()
        {
            InitZPXMode(OpCodes.DECZPX, 0x1);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
        }
        
        [Test]
        public void DECAbsOperationTest()
        {
            InitAbsMode(OpCodes.DECAbs, 0x81);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0102);
            Assert.That(val, Is.EqualTo(0x80));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void DECAbsXOperationTest()
        {
            InitAbsXMode(OpCodes.DECAbsX, 0x80);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0103);
            Assert.That(val, Is.EqualTo(0x7F));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void DEXOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.DEX);
            CPU.IndexRegisterX = 0x00;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0xFF));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void DEYOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.DEY);
            CPU.IndexRegisterY = 0xFF;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0xFE));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
    }
}