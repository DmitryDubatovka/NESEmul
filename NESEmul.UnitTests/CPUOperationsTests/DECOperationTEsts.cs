using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class DECOperationTests : BranchOperationTests
    {
        [Test]
        public void DECZPOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.DECZP);
            Memory.StoreByteInMemory(1, 0xF);
            Memory.StoreByteInMemory(0xF, 0x10);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0xF));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void DECZPXOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.DECZPX);
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0xE);
            Memory.StoreByteInMemory(0xF, 0x1);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0xF);
            Assert.That(val, Is.EqualTo(0));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
        }
        
        [Test]
        public void DECAbsOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.DECAbs);
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(2, 0x01);
            Memory.StoreByteInMemory(0x0102, 0x81);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0102);
            Assert.That(val, Is.EqualTo(0x80));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void DECAbsXOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.DECAbsX);
            CPU.IndexRegisterX = 0x01;
            Memory.StoreByteInMemory(1, 0x01);
            Memory.StoreByteInMemory(2, 0x01);
            Memory.StoreByteInMemory(0x0102, 0x80);
            CPU.Do();
            var val = Memory.LoadByteFromMemory(0x0102);
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